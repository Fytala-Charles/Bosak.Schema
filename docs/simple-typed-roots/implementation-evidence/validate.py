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
expected={"source":3185,"minimum-consumer-results":2178,"current-consumer-results/final-header":2178,"current-native-results":59,"legacy124-results":124,"alpha23-results":23,"beta33-results":33}
for lane,n in expected.items():
    total=0
    for f in (P/lane).glob("*.trx"):
        c=ET.parse(f).find(".//{*}Counters");assert int(c.attrib["failed"])==0;total+=int(c.attrib["passed"])
    assert total==n,(lane,total)
stable=read(P/"STABLE-CASES.json");assert stable["priorCasesPreserved"]==3050 and stable["newCases"]==135 and not stable["removed"]
admission=read(P.parent/"ADMISSIONS.json")["rows"];assert len(admission)==1 and admission[0]["id"]=="AS-050"
a,b=admission[0]["before"],admission[0]["after"];assert {k for k in a if a[k]!=b[k]}=={"label","expectedCompile"}
original=json.loads(subprocess.check_output(["git","show","1f2cca34ce8dc93b25f11b402bbe60330b8ee1cf:tests/TestData/AnonymousSimpleRestrictions/cases.json"],cwd=ROOT))
actual=read(ROOT/"tests/TestData/AnonymousSimpleRestrictions/cases.json");assert [x["id"] for x,y in zip(original,actual) if x!=y]==["AS-050"]
assert a==next(x for x in original if x["id"]=="AS-050") and b==next(x for x in actual if x["id"]=="AS-050")
manifest=read(ROOT/"docs/conformance-pilot/CASES.json")
for f,h in manifest["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/f).read_bytes())==h
assert read(P/"pilot/RESULTS.json")["counts"]==dict(match=21,unsupported=0,mismatch=0,harnessError=0)
assert not read(P/"PILOT-DELTA.json")["classificationChanges"]
expandedFiles=0
for folder in [ROOT/"docs/expanded-conformance",ROOT/"docs/expanded-conformance/cohort2"]:
    for f,h in read(folder/"CASES.json")["files"].items():assert sha((folder/"upstream"/f).read_bytes())==h;expandedFiles+=1
expanded=dict(match=3,unsupported=126,mismatch=0,harnessError=0)
for pin in ["minimum","current"]:
    counts={k:0 for k in expanded}
    for cohort in [1,2]:
        report=read(P/f"expanded-{pin}-{cohort}/RESULTS.json")
        for k,v in report["counts"].items():counts[k]+=v
    assert counts==expanded
for cohort in [1,2]:assert read(P/f"expanded-minimum-{cohort}/RESULTS.json")["rows"]==read(P/f"expanded-current-{cohort}/RESULTS.json")["rows"]
delta=read(P/"EXPANDED-DELTA.json");assert not delta["classificationChanges"] and len(delta["reasonChanges"])==8
api=read(P/"SURFACE.json");assert api["zeroPublicApiDelta"] and api["currentPublicTypes"]==54 and api["preservedRows"]==401
raw=Path("D:/Development/Bosak/.guard-tmp/schema-simple-roots-production-20261122")
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
docs=read(P/"DOCUMENTATION-CHECKS.json");assert len(docs["pages"])==13 and docs["localLinks"]==706
checks=dict(status="Qualified production simple-root candidate",sourceTests=3185,sourceInternal=1007,sourcePublic=2178,priorStableCases=3050,newSourceCases=135,packageConsumersPerCore=2178,packageNative=59,retained=[124,23,33],pilotMatches=21,pilotErrors=0,frozenFiles=42,expandedFiles=expandedFiles,expanded=expanded,expandedReasonOnlyChanges=8,priorSeals=prior,priorSealedEntries=entries,sourceInputs=len(read(P/"SOURCE-INPUTS.json")),publicApiDelta=0,productionAdoption=True,packagePublished=False,renderedPages=13,localLinks=706,visualAcceptance=False)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
