"""Verify production candidate receipts without compiling or executing the engine.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,xml.etree.ElementTree as ET,zipfile
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
seals=[f for f in (ROOT/"docs").rglob("PACKET-SHA256.json") if P not in f.parents]
sealed=0
for receipt in seals:
    for f,h in read(receipt).items():
        assert sha((receipt.parent/f).read_bytes())==h,(str(receipt),f)
        sealed+=1
adoption=read(P.parent/"PRODUCTION-ADOPTION.json")
for row in adoption["sources"]:assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
for f,h in read(P/"SOURCE-INPUTS.json").items():assert sha((ROOT/f).read_bytes())==h,f
for f in (P/"candidate").rglob("*"):
    if f.is_file():assert f.read_bytes()==(ROOT/f.relative_to(P/"candidate")).read_bytes()
manifest=read(ROOT/"docs/conformance-pilot/CASES.json")
for f,h in manifest["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/f).read_bytes())==h
expected={"source":3050,"minimum-consumer-results":2102,"current-consumer-results":2102,"current-native-results":48,"legacy124-results":124,"alpha23-results":23,"beta33-results":33}
for lane,n in expected.items():
    total=0
    for f in (P/lane).glob("*.trx"):
        c=ET.fromstring(f.read_bytes()).find(".//{*}Counters");assert int(c.attrib["failed"])==0;total+=int(c.attrib["passed"])
    assert total==n,(lane,total)
stable=read(P/"STABLE-CASES.json");assert stable["priorCasesPreserved"]==2886 and stable["newCases"]==164 and not stable["removed"]
assert read(P/"pilot/RESULTS.json")["counts"]==dict(match=21,mismatch=0,unsupported=0,harnessError=0)
assert [x["id"] for x in read(P/"PILOT-DELTA.json")["classificationChanges"]]==[]
api=read(P/"SURFACE.json");assert api["zeroPublicApiDelta"] and api["preservedRows"]==401 and api["currentPublicTypes"]==54
raw=Path("D:/Development/Bosak/.guard-tmp/schema-string-pattern-production-20261122")
identity=read(P/"IDENTITY-FINAL.json");package=raw/"feed"/identity["package"]
assert sha(package.read_bytes())==identity["packageSha256"]
with zipfile.ZipFile(package) as z:
    binary=z.read("lib/net10.0/Bosak.Schema.dll");assert sha(binary)==identity["schemaDllSha256"]
    assert z.read("NUGET_README.md")== (ROOT/"docs/NUGET_README.md").read_bytes()
assert binary==(ROOT/"src/Bosak.Schema/bin/Release/net10.0/Bosak.Schema.dll").read_bytes()
assert binary==(ROOT/"tools/Bosak.Schema.ConformancePilot/bin/Release/net10.0/Bosak.Schema.dll").read_bytes()
for lane,core in [("minimum-consumer","1.81.2"),("current-consumer","1.81.17")]:
    assets=read(raw/lane/"obj/project.assets.json");assert all(x["type"]!="project" for x in assets["libraries"].values())
    assert binary==(raw/lane/"bin/Release/net10.0/Bosak.Schema.dll").read_bytes()
    assert all(row["version"]==core and row["signatureVerified"] for row in identity["lanes"][lane]["core"])
expandedFiles=0
for folder in [ROOT/"docs/expanded-conformance",ROOT/"docs/expanded-conformance/cohort2"]:
    for f,h in read(folder/"CASES.json")["files"].items():
        assert sha((folder/"upstream"/f).read_bytes())==h,f
        expandedFiles+=1
expectedExpanded=dict(match=3,unsupported=126,mismatch=0,harnessError=0)
for pin in ["minimum","current"]:
    counts={k:0 for k in expectedExpanded}
    for cohort in [1,2]:
        result=read(P/f"expanded-{pin}-{cohort}/RESULTS.json")
        for k,v in result["counts"].items():counts[k]+=v
    assert counts==expectedExpanded,(pin,counts)
delta=read(P/"EXPANDED-DELTA.json")
assert delta["classificationsIdenticalAcrossPins"] and delta["after"]==expectedExpanded
assert len(delta["changes"]["cohort2"])==3 and not delta["changes"]["cohort1"]
for row in identity["lanes"]["current-consumer"]["core"]:
    name=row["id"]+".dll"
    assert (raw/"current-runner/bin/Release/net10.0"/name).read_bytes()==(raw/"current-consumer/bin/Release/net10.0"/name).read_bytes()
assert binary==(raw/"current-runner/bin/Release/net10.0/Bosak.Schema.dll").read_bytes()
checks=dict(status="Passed production receipt/hash validation",sourceTests=3050,sourceInternal=948,sourcePublic=2102,priorStableCases=2886,newSourceCases=164,packageConsumersPerCore=2102,packageNative=48,retained=[124,23,33],pilotMatches=21,pilotUnsupported=0,pilotErrors=0,frozenFiles=42,expandedFiles=expandedFiles,expanded=expectedExpanded,priorSeals=len(seals),priorSealedEntries=sealed,sourceInputs=len(read(P/"SOURCE-INPUTS.json")),publicApiDelta=0,productionAdoption=True,packagePublished=False)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
