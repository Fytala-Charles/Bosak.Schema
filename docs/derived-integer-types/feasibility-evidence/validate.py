"""Validate isolated native feasibility receipts without running the engine.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda f:json.loads(f.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
prior=entries=0
for receipt in (ROOT/"docs").rglob("PACKET-SHA256.json"):
    if receipt.parent==P:continue
    for f,h in read(receipt).items():assert sha((receipt.parent/f).read_bytes())==h,(receipt,f);entries+=1
    prior+=1
m=read(P/"CANDIDATE-SOURCE.json");assert len(m["files"])==59 and len(m["overlays"])==3
for row in m["files"]:
    if row["baseSha256"] is None:b=(P/"overlay"/row["path"]).read_bytes()
    else:
        b=subprocess.check_output(["git","show",m["base"]+":src/Bosak.Schema/"+row["path"]],cwd=ROOT);assert sha(b)==row["baseSha256"]
        if row["path"] in m["overlays"]:b=(P/"overlay"/row["path"]).read_bytes()
    assert sha(b)==row["sha256"]
for f,h in read(P.parent/"ENGINE-INPUTS.json").items():assert sha((ROOT/f).read_bytes())==h
for row in read(P/"EXPERIMENT-ADOPTION.json")["sources"]:assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
def count(folder):
    passed=failed=0
    for f in folder.glob("*.trx"):
        c=ET.parse(f).find(".//{*}Counters");passed+=int(c.attrib["passed"]);failed+=int(c.attrib["failed"])
    return passed,failed
for lane,expected in {"minimum/results/qualified":(435,0),"current/results/qualified":(435,0),"baseline-minimum/results/qualified":(24,0),"baseline-current/results/qualified":(24,0),"regression/results/internal":(1005,2),"regression/results/public":(2166,12),"regression/results/internal-qualified":(1007,0),"regression/results/public-reconciled":(2178,0)}.items():assert count(P/lane)==expected,(lane,count(P/lane))
a=read(P/"REGRESSION-ADMISSIONS.json");assert a["logicalAdmissions"]==11 and a["executedCases"]==14 and len(a["rows"])==11
for row in a["rows"]:
    if "file" not in row:continue
    before,after=row["before"],row["after"]
    assert all(before[k]==after[k] for k in ["id","schema","instance"])
    original=read(ROOT/row["file"]);rows=original if isinstance(original,list) else original.get("cases",original.get("rows"));assert before==next(x for x in rows if x["id"]==row["id"])
    assert before["expectedCompile"]=="Unsupported" and after["expectedCompile"]=="Valid"
stable=read(P/"STABLE-CASES.json");assert stable["priorIdentities"]==stable["reconciledIdentities"]==3185 and stable["noTestsRemoved"]
observed=read(P/"ORIGINAL-IDENTITY-OBSERVATIONS.json");assert observed["perCore"]==84 and observed["lanes"]["minimum"]==observed["lanes"]["current"]
assert len(observed["lanes"]["minimum"])==84 and all(x["actualOriginalTypeVerified"] and x["observedDescriptiveType"] is None for x in observed["lanes"]["minimum"])
assert (P/"case-plan.json").read_bytes()==(P.parent/"CASE-PLAN.json").read_bytes()
assert (P/"builtin-types.json").read_bytes()==(P.parent/"BUILTIN-TYPES.json").read_bytes()
for c in read(P/"case-plan.json")["cases"]:ET.fromstring(c["schema"]);ET.fromstring(c["instance"])
for i,row in enumerate(read(P.parent/"ORIGINAL-EXAMPLES.json")["rows"]):
    for key in ["schema","instance"]:
        name=read(P/"originals.json")[i][key];assert sha((P/name).read_bytes())==row["inputSha256"][key]
api=read(P/"SURFACE.json");assert api["zeroPublicApiDelta"] and api["currentPublicTypes"]==54 and api["preservedRows"]==401
identity=read(P/"IDENTITY.json");raw=Path("D:/Development/Bosak/.guard-tmp/schema-derived-integers-feasibility-20261122");signed=0
for lane in ["minimum","current"]:
    info=identity["lanes"][lane];assert not info["coreProjectReferences"] and info["isolatedSchemaProjectReference"]
    runtime=raw/lane/"bin/Release/net10.0"
    assert sha((runtime/"Bosak.Schema.dll").read_bytes())==info["schemaCandidateDllSha256"]
    assert sha((runtime/"Bosak.Schema.Tests.dll").read_bytes())==info["probeDllSha256"]
    for row in info["packages"]:
        assert row["signatureVerified"] and row["cacheRuntimeMatch"] and (P/"signatures"/(lane+"-"+row["id"]+".log")).exists()
        assert sha((runtime/(row["id"]+".dll")).read_bytes())==row["dllSha256"];signed+=1
assert signed==18
assert not subprocess.check_output(["git","diff","--name-only","--","src","tests","tools",".github","Directory.Build.props","global.json"],cwd=ROOT).strip()
assert "RESULT: ALL CHECKS PASSED" in (P/"DOCS-CHECK.log").read_text(encoding="utf-8-sig")
docs=read(P/"DOCUMENTATION-CHECKS.json");assert len(docs["pages"])==12 and docs["localLinks"]==684
checks=dict(status="Qualified bounded native derived-integer feasibility",nativePerCore=435,baselinePerCore=24,sourceInternalReconciled=1007,sourcePublicReconciled=2178,originalStimuli=3185,logicalAdmissions=11,executedAdmissions=14,originalIdentityPerCore=84,signedCorePackages=18,publicApiDelta=0,sourceFiles=59,overlays=3,priorSeals=prior,priorEntries=entries,engineInputs=261,productSourceUnchanged=True,productionAdoption=False,packageRetainedCorpusExecuted=False,renderedPages=12,localLinks=684,visualAcceptance=False)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
