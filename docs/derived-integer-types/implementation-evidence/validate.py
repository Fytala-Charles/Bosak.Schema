"""Validate production receipts, preserved evidence and exact qualification inputs.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,xml.etree.ElementTree as ET,zipfile,re,subprocess
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda f:json.loads(f.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
prior=entries=0
for receipt in (ROOT/"docs").rglob("PACKET-SHA256.json"):
    if receipt.parent==P:continue
    for f,h in read(receipt).items():assert sha((receipt.parent/f).read_bytes())==h,(receipt,f);entries+=1
    prior+=1
for row in read(P.parent/"PRODUCTION-ADOPTION.json")["sources"]:assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
for f,h in read(P/"SOURCE-INPUTS.json").items():assert sha((ROOT/f).read_bytes())==h,f
for f in (P/"candidate").rglob("*"):
    if f.is_file():assert f.read_bytes()==(ROOT/f.relative_to(P/"candidate")).read_bytes()
expected={"source":3620,"minimum-consumer-results":2494,"current-consumer-results":2494,"current-native-results":119,"legacy124-results":124,"alpha23-results":23,"beta33-results":33}
for lane,n in expected.items():
    total=0
    for f in (P/lane).glob("*.trx"):
        c=ET.parse(f).find(".//{*}Counters");assert int(c.attrib["failed"])==0;total+=int(c.attrib["passed"])
    assert total==n,(lane,total)
stable=read(P/"STABLE-CASES.json");assert stable["priorStimuliPreserved"]==3185 and stable["newCases"]==435 and not stable["removedStimuli"]
admission=read(P.parent/"ADMISSIONS.json");assert admission["logicalAdmissions"]==11 and admission["executedCases"]==14 and len(admission["rows"])==11
byFile={}
for row in admission["rows"]:
    if "file" not in row:continue
    assert all(row["before"][k]==row["after"][k] for k in ["id","schema","instance"])
    byFile.setdefault(row["file"],[]).append(row)
for name,rows in byFile.items():
    original=json.loads(subprocess.check_output(["git","show","f9e9ad7c0599ba7b36b6d8d71704875990b54b98:"+name],cwd=ROOT));actual=read(ROOT/name)
    before=original if isinstance(original,list) else original.get("cases",original.get("rows"));after=actual if isinstance(actual,list) else actual.get("cases",actual.get("rows"))
    assert {x["id"] for x,y in zip(before,after) if x!=y}=={row["id"] for row in rows}
    for row in rows:assert row["before"]==next(x for x in before if x["id"]==row["id"]) and row["after"]==next(x for x in after if x["id"]==row["id"])
change=read(P/"native-expectations/CHANGE.json");assert sha((P/"native-expectations/original.cs.txt").read_bytes())==change["beforeGitBlobSha256"]
assert sha((ROOT/change["file"]).read_bytes())==change["afterWorkspaceSha256"]
manifest=read(ROOT/"docs/conformance-pilot/CASES.json")
for f,h in manifest["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/f).read_bytes())==h
assert read(P/"pilot/RESULTS.json")["counts"]==dict(match=21,unsupported=0,mismatch=0,harnessError=0)
assert not read(P/"PILOT-DELTA.json")["classificationChanges"]
expandedFiles=0
for folder in [ROOT/"docs/expanded-conformance",ROOT/"docs/expanded-conformance/cohort2"]:
    for f,h in read(folder/"CASES.json")["files"].items():assert sha((folder/"upstream"/f).read_bytes())==h;expandedFiles+=1
expanded=dict(match=8,unsupported=121,mismatch=0,harnessError=0)
for pin in ["minimum","current"]:
    counts={k:0 for k in expanded}
    for cohort in [1,2]:
        report=read(P/f"expanded-{pin}-{cohort}/RESULTS.json")
        for k,v in report["counts"].items():counts[k]+=v
    assert counts==expanded
for cohort in [1,2]:assert read(P/f"expanded-minimum-{cohort}/RESULTS.json")["rows"]==read(P/f"expanded-current-{cohort}/RESULTS.json")["rows"]
delta=read(P/"EXPANDED-DELTA.json");assert len(delta["classificationChanges"])==5 and not delta["detailChanges"]
api=read(P/"SURFACE.json");assert api["zeroPublicApiDelta"] and api["currentPublicTypes"]==54 and api["preservedRows"]==401
raw=Path("D:/Development/Bosak/.guard-tmp/schema-derived-integers-production-20261122")
identity=read(P/"IDENTITY-FINAL.json");package=raw/"feed"/identity["package"];assert sha(package.read_bytes())==identity["packageSha256"]
with zipfile.ZipFile(package) as z:
    binary=z.read("lib/net10.0/Bosak.Schema.dll");assert sha(binary)==identity["schemaDllSha256"]
    assert z.read("NUGET_README.md")== (ROOT/"docs/NUGET_README.md").read_bytes()
assert binary==(ROOT/"src/Bosak.Schema/bin/Release/net10.0/Bosak.Schema.dll").read_bytes()==(ROOT/"tools/Bosak.Schema.ConformancePilot/bin/Release/net10.0/Bosak.Schema.dll").read_bytes()
for lane,core in [("minimum-consumer","1.81.2"),("current-consumer","1.81.17")]:
    assets=read(raw/lane/"obj/project.assets.json");assert all(x["type"]!="project" for x in assets["libraries"].values())
    assert binary==(raw/lane/"bin/Release/net10.0/Bosak.Schema.dll").read_bytes()
    assert len(identity["lanes"][lane]["core"])==9
    assert all(row["version"]==core and row["signatureVerified"] for row in identity["lanes"][lane]["core"])
assert binary==(raw/"current-runner/bin/Release/net10.0/Bosak.Schema.dll").read_bytes()==(raw/"current-native/bin/Release/net10.0/Bosak.Schema.dll").read_bytes()
for row in identity["lanes"]["current-consumer"]["core"]:
    name=row["id"]+".dll";assert (raw/"current-runner/bin/Release/net10.0"/name).read_bytes()==(raw/"current-consumer/bin/Release/net10.0"/name).read_bytes()
example=re.findall(r"```xml\s*\n(.*?)```",(ROOT/"docs/NUGET_README.md").read_text(encoding="utf-8"),re.S)[-1];assert (P/"readme-example/example.xsd").read_text(encoding="utf-8")==example
assert "passes both facades" in (P/"readme-example/example.log").read_text()
assert "RESULT: ALL CHECKS PASSED" in (P/"DOCS-CHECK.log").read_text(encoding="utf-8-sig")
docs=read(P/"DOCUMENTATION-CHECKS.json");assert len(docs["pages"])==13 and docs["localLinks"]==735
checks=dict(status="Qualified production derived-integer candidate",sourceTests=3620,sourceInternal=1126,sourcePublic=2494,priorStimuli=3185,newCases=435,logicalAdmissions=11,executedAdmissions=14,packageConsumersPerCore=2494,packageNative=119,retained=[124,23,33],pilotMatches=21,pilotErrors=0,frozenFiles=42,expandedFiles=expandedFiles,expanded=expanded,newExpandedMatches=5,otherExpandedDetailChanges=0,priorSeals=prior,priorSealedEntries=entries,sourceInputs=len(read(P/"SOURCE-INPUTS.json")),publicApiDelta=0,productionAdoption=True,packagePublished=False,renderedPages=13,localLinks=735,visualAcceptance=False)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
