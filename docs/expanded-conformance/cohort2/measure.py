"""Measure the preselected public sample with the unchanged caught native driver.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import argparse,collections,hashlib,importlib.util,json,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
sha=lambda b:hashlib.sha256(b).hexdigest()
def read(path):return json.loads(path.read_text(encoding="utf-8-sig"))
def dump(path,value):path.write_text(json.dumps(value,indent=2)+"\n",encoding="utf-8")
if __name__=="__main__":
    parser=argparse.ArgumentParser();parser.add_argument("--output",type=Path,required=True);parser.add_argument("--identity",type=Path);args=parser.parse_args();assert not args.output.exists(),"Require a fresh result directory."
    manifest=read(P/"CASES.json");selection=read(P/"SELECTION-CHECKS.json");assert sha((P/"CASES.json").read_bytes())==selection["manifestSha256"]
    spec=importlib.util.spec_from_file_location("pilot",ROOT/"tools/run-xsd-conformance-pilot.py");pilot=importlib.util.module_from_spec(spec);spec.loader.exec_module(pilot)
    requests=pilot.verify(manifest,P/"upstream")
    identity=read(args.identity or P/"PRODUCT-IDENTITY.json");runtime=Path(identity["runtimeDirectory"])
    assert {f.name:sha(f.read_bytes()) for f in runtime.iterdir() if f.is_file()}==identity["runtimeFiles"]
    assert sha((runtime/"Bosak.Schema.dll").read_bytes())==identity["schemaDllSha256"]
    frozen=read(ROOT/"docs/conformance-pilot/CASES.json")
    for f,h in frozen["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/f).read_bytes())==h
    args.output.mkdir(parents=True);dump(args.output/"request.json",requests)
    with (args.output/"driver.log").open("wb") as log:
        result=subprocess.run(["dotnet",str(runtime/identity["runner"]),str((args.output/"request.json").resolve()),str((args.output/"native.json").resolve())],stdout=log,stderr=subprocess.STDOUT,stdin=subprocess.DEVNULL,timeout=120,check=True)
    report=read(args.output/"native.json");rows=pilot.checked_results(report,requests);counts=collections.Counter();output=[];buckets={}
    for case in manifest["cases"]:
        native=rows[case["id"]];classification=pilot.classify(case["upstreamExpected"],native);counts[classification]+=1
        item=dict(**case,classification=classification,native=native);output.append(item)
        buckets.setdefault(case["bucket"],collections.Counter())[classification]+=1
    # Keep explicit zeros. Unsupported is an assessment finding, never a pass or Invalid.
    total={k:counts[k] for k in ["match","unsupported","mismatch","harnessError"]}
    dump(args.output/"RESULTS.json",dict(date="2026-11-22",productCommit=identity["productCommit"],manifestSha256=selection["manifestSha256"],core=identity["core"],caseCount=len(output),groupCount=selection["selectedGroups"],counts=total,buckets={k:dict(v) for k,v in buckets.items()},rows=output,driverIdentity={k:v for k,v in report.items() if k!="rows"},scope="Expanded selected upstream instance-validity assessment only; no PSVI or complete conformance claim",unsupportedIsNotPass=True,frozenPilotExecuted=False,frozenFilesVerified=len(frozen["files"])))
    print(json.dumps(total))
    raise SystemExit(2 if counts["mismatch"] or counts["harnessError"] else 0)
