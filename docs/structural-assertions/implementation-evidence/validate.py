"""Verify production candidate receipts without compiling or executing the engine.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,xml.etree.ElementTree as ET,zipfile
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
seals=["post-xsi-type","xsi-type-contract","xsi-type-contract/feasibility-evidence","xsi-type-contract/implementation/evidence","namespace-form/feasibility-evidence","namespace-form/evidence","namespace-form-review","complex-simple-restriction","complex-simple-restriction/feasibility-evidence","complex-simple-restriction/implementation-evidence","integer-simple-content","integer-simple-content/feasibility-evidence","integer-simple-content/implementation-evidence","contextual-assertions","contextual-assertions/feasibility-evidence","contextual-assertions/implementation-evidence","structural-assertions","structural-assertions/empty-value-feasibility-evidence","structural-assertions/assertion-feasibility-evidence"]
sealed=0
for name in seals:
    d=ROOT/"docs"/name
    for f,h in read(d/"PACKET-SHA256.json").items():assert sha((d/f).read_bytes())==h,(name,f);sealed+=1
adoption=read(P.parent/"PRODUCTION-ADOPTION.json")
for row in adoption["sources"]:assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
for f,h in read(P/"SOURCE-INPUTS.json").items():assert sha((ROOT/f).read_bytes())==h,f
for f in (P/"candidate").rglob("*"):
    if f.is_file():assert f.read_bytes()==(ROOT/f.relative_to(P/"candidate")).read_bytes()
manifest=read(ROOT/"docs/conformance-pilot/CASES.json")
for f,h in manifest["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/f).read_bytes())==h
expected={"source-final":2886,"minimum-consumer-results":1986,"current-consumer-results":1986,"current-native-results":60,"legacy124-results":124,"alpha23-results":23,"beta33-results":33}
for lane,n in expected.items():
    total=0
    for f in (P/lane).glob("*.trx"):
        c=ET.fromstring(f.read_bytes()).find(".//{*}Counters");assert int(c.attrib["failed"])==0;total+=int(c.attrib["passed"])
    assert total==n,(lane,total)
stable=read(P/"STABLE-CASES.json");assert stable["priorCasesPreserved"]==2770 and stable["newCases"]==116 and not stable["removed"]
assert read(P/"pilot/RESULTS.json")["counts"]==dict(match=21,mismatch=0,unsupported=0,harnessError=0)
assert [x["id"] for x in read(P/"PILOT-DELTA.json")["classificationChanges"]]==["Assert/assert_027/assert_027_2"]
api=read(P/"SURFACE.json");assert api["zeroPublicApiDelta"] and api["preservedRows"]==401 and api["currentPublicTypes"]==54
raw=Path("D:/Development/Bosak/.guard-tmp/schema-structural-assertions-production-20261122")
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
checks=dict(status="Passed production receipt/hash validation",sourceTests=2886,sourceInternal=900,sourcePublic=1986,priorStableCases=2770,newSourceCases=116,packageConsumersPerCore=1986,packageNative=60,retained=[124,23,33],pilotMatches=21,pilotUnsupported=0,pilotErrors=0,frozenFiles=42,priorSeals=len(seals),priorSealedEntries=sealed,sourceInputs=len(read(P/"SOURCE-INPUTS.json")),publicApiDelta=0,productionAdoption=True,packagePublished=False)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
