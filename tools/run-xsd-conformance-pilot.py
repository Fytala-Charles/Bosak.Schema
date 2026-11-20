"""Hash/metadata-verified native validity pilot; Unsupported is never a conformance match.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import argparse, hashlib, json, subprocess, sys, xml.etree.ElementTree as ET
T="{http://www.w3.org/XML/2004/xml-schema-test-suite/}"
L="{http://www.w3.org/1999/xlink}href"

def safe_path(root, name):
    relative=Path(name)
    if relative.is_absolute():raise ValueError("Absolute corpus path refused")
    path=(root/relative).resolve()
    if not path.is_relative_to(root.resolve()):raise ValueError("Corpus path escapes selected root")
    return path

def applies(node, tokens, expected=False):
    values=node.attrib.get("version", "").split()
    return not values or (all(v in tokens for v in values) if expected else any(v in tokens for v in values))

def expected_value(node, tokens):
    values={e.attrib["validity"] for e in node.findall(T+"expected") if applies(e,tokens,True)}
    if len(values)!=1 or not values <= {"valid","invalid"}:raise ValueError("Ambiguous or unsupported version expectation")
    return next(iter(values))

def verify(manifest, root):
    for name,digest in manifest["files"].items():
        if hashlib.sha256(safe_path(root,name).read_bytes()).hexdigest()!=digest:raise ValueError("Corpus hash mismatch: "+name)
    ids=set();requests=[];tokens=set(manifest["configurationTokens"])
    for case in manifest["cases"]:
        if case["id"] in ids:raise ValueError("Duplicate case identity")
        ids.add(case["id"])
        for key in ["metadata","schema","instance"]:
            if case[key] not in manifest["files"]:raise ValueError("Case file missing from hash manifest")
        meta_path=safe_path(root,case["metadata"]);meta=ET.parse(meta_path).getroot()
        groups=[g for g in meta.findall(T+"testGroup") if g.attrib.get("name")==case["group"]]
        if meta.attrib.get("name")!=case["testSet"] or len(groups)!=1:raise ValueError("Wrong metadata identity")
        group=groups[0];schemas=[s for s in group.findall(T+"schemaTest") if s.attrib.get("name")==case["schemaTest"]];instances=[i for i in group.findall(T+"instanceTest") if i.attrib.get("name")==case["instanceTest"]]
        if len(schemas)!=1 or len(instances)!=1:raise ValueError("Ambiguous test identity")
        schema=schemas[0];instance=instances[0]
        if not all(applies(n,tokens) for n in [meta,group,schema,instance]):raise ValueError("Case outside declared version configuration")
        for test in [schema,instance]:
            current=test.find(T+"current")
            if current is None or current.attrib.get("status")!="accepted":raise ValueError("Unaccepted/queried case refused")
        if expected_value(schema,tokens)!="valid" or case["upstreamSchemaExpected"]!="valid":raise ValueError("Pilot requires valid schemas")
        if expected_value(instance,tokens)!=case["upstreamExpected"]:raise ValueError("Expected validity differs from pinned metadata")
        for test,tag,key in [(schema,"schemaDocument","schema"),(instance,"instanceDocument","instance")]:
            docs=test.findall(T+tag)
            if len(docs)!=1:raise ValueError("Pilot requires one local document")
            href=docs[0].attrib[L]
            if ":" in href or href.startswith(("/","\\")):raise ValueError("Remote/absolute metadata href refused")
            resolved=(meta_path.parent/href).resolve()
            if resolved!=safe_path(root,case[key]):raise ValueError("Metadata/document path differs")
        requests.append(dict(id=case["id"],schema=str(safe_path(root,case["schema"])),instance=str(safe_path(root,case["instance"]))))
    return requests

def classify(expected, row):
    if expected not in ["valid","invalid"]:raise ValueError("Unknown upstream expectation")
    if row.get("error"):return "harnessError"
    compile=row.get("compile");actual=row.get("assessment")
    if compile=="Unsupported":return "unsupported"
    if compile=="Invalid":return "mismatch"
    if compile!="Valid":raise ValueError("Incomplete native result")
    if actual=="Unsupported":return "unsupported"
    if actual not in ["Valid","Invalid"]:raise ValueError("Incomplete native assessment")
    return "match" if actual.lower()==expected else "mismatch"

def checked_results(actual, requests):
    rows=actual["rows"];by_id={r["id"]:r for r in rows}
    if len(rows)!=len(requests) or len(by_id)!=len(requests) or set(by_id)!={r["id"] for r in requests}:raise ValueError("Missing/duplicate/unexpected driver result")
    return by_id

def verify_candidate(repository, base, runner, evidence_path):
    evidence=json.loads(evidence_path.read_text(encoding="utf-8"))
    if evidence["productBase"]!=base:raise ValueError("Candidate product base differs")
    paths=["src","Directory.Build.props","global.json"]
    changed=subprocess.check_output(["git","diff","--name-only","-z",base,"--",*paths],cwd=repository).decode("utf-8").split("\0")
    untracked=subprocess.check_output(["git","ls-files","--others","--exclude-standard","-z","--",*paths],cwd=repository).decode("utf-8").split("\0")
    if set(filter(None,changed+untracked)) != set(evidence["changedProductionFiles"]):raise ValueError("Candidate changed production file set differs")
    for name,digest in evidence["changedProductionFiles"].items():
        if hashlib.sha256(safe_path(repository,name).read_bytes()).hexdigest()!=digest:raise ValueError("Candidate source hash differs")
    runtime={p.name:hashlib.sha256(p.read_bytes()).hexdigest() for p in runner.parent.iterdir() if p.is_file()}
    if runner.name!=evidence["runner"] or runtime!=evidence["runtimeFiles"]:raise ValueError("Candidate runtime hash set differs")
    return dict(sha256=hashlib.sha256(evidence_path.read_bytes()).hexdigest(),**evidence)

def verify_baseline(repository, base):
    paths=["src","Directory.Build.props","global.json"]
    subprocess.run(["git","diff","--exit-code",base,"--",*paths],cwd=repository,check=True,stdout=subprocess.DEVNULL)
    untracked=subprocess.check_output(["git","ls-files","--others","--exclude-standard","-z","--",*paths],cwd=repository)
    if untracked:raise ValueError("Untracked baseline production source refused")

def main():
    repository=Path(__file__).resolve().parent.parent
    parser=argparse.ArgumentParser();parser.add_argument("--output",required=True);parser.add_argument("--runner",required=True);parser.add_argument("--candidate-evidence");parser.add_argument("--timeout",type=int,default=60);args=parser.parse_args()
    if args.timeout<1 or args.timeout>120:raise ValueError("Timeout outside1..120seconds")
    manifest_path=repository/"docs/conformance-pilot/CASES.json";manifest=json.loads(manifest_path.read_text(encoding="utf-8"));root=repository/"tests/TestData/ConformancePilot/upstream"
    requests=verify(manifest,root)
    # Driver/tool edits are separate from production semantics. Refuse unreviewed product changes.
    candidate=None;runner=Path(args.runner).resolve()
    if args.candidate_evidence:candidate=verify_candidate(repository,manifest["sourceCommit"],runner,Path(args.candidate_evidence))
    else:verify_baseline(repository,manifest["sourceCommit"])
    output=Path(args.output).resolve();output.mkdir(parents=True,exist_ok=False);request=output/"request.json";result=output/"native.json";request.write_text(json.dumps(requests,indent=2)+"\n",encoding="utf-8")
    with (output/"driver.log").open("wb") as log:subprocess.run(["dotnet",str(Path(args.runner).resolve()),str(request),str(result)],stdin=subprocess.DEVNULL,stdout=log,stderr=subprocess.STDOUT,timeout=args.timeout,check=True)
    actual=json.loads(result.read_text(encoding="utf-8-sig"));by_id=checked_results(actual,requests)
    rows=[];counts={k:0 for k in ["match","mismatch","unsupported","harnessError"]}
    for case in manifest["cases"]:
        native=by_id[case["id"]];status=classify(case["upstreamExpected"],native);counts[status]+=1;rows.append(dict(id=case["id"],expected=case["upstreamExpected"],classification=status,**{k:v for k,v in native.items() if k!="id"}))
    report=dict(upstream=manifest["upstream"],productBase=manifest["sourceCommit"],manifestSha256=hashlib.sha256(manifest_path.read_bytes()).hexdigest(),caseCount=len(rows),counts=counts,unsupportedIsNotPass=True,scope="Selected upstream instance validity only; no PSVI/whole-suite claim",driverIdentity={k:v for k,v in actual.items() if k!="rows"},rows=rows)
    if candidate is not None:report["candidateEvidence"]=candidate
    (output/"RESULTS.json").write_text(json.dumps(report,indent=2)+"\n",encoding="utf-8");print(json.dumps(counts));return 2 if counts["mismatch"] or counts["harnessError"] else 0

if __name__=="__main__":
    try:sys.exit(main())
    except (OSError,ValueError,subprocess.SubprocessError,ET.ParseError,KeyError) as error:
        print("Pilot refused: "+type(error).__name__+": "+str(error),file=sys.stderr);sys.exit(3)
