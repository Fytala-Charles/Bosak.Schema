"""Verify production-all receipts without altering prior packets or launching binaries.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET,re
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"));sha=lambda b:hashlib.sha256(b).hexdigest()
def readlog(p):
 b=p.read_bytes();return b.decode("utf-16") if b.startswith((b"\xff\xfe",b"\xfe\xff")) else b.decode("utf-8-sig")
packets=entries=0
for p in (ROOT/"docs").rglob("PACKET-SHA256.json"):
 if p.parent==P:continue
 for name,h in read(p).items():assert sha((p.parent/name).read_bytes())==h,(p,name)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(53,3345),(packets,entries)
inputs=read(P/"PRODUCTION-INPUTS.json");assert len(inputs["files"])==311 and len(inputs["changedExisting"])==6 and len(inputs["new"])==15
for rel,h in inputs["files"].items():assert sha((ROOT/rel).read_bytes())==h,rel
for rel,r in inputs["changedExisting"].items():assert sha((P/"production-inputs"/rel).read_bytes())==r["after"]
for rel in inputs["new"]:assert sha((P/"production-inputs"/rel).read_bytes())==inputs["files"][rel]
plan=read(P/"CANDIDATE-SOURCE.json");assert len(plan["files"])==61 and len(plan["overlays"])==4
for r in plan["files"]:
 assert sha((ROOT/"src/Bosak.Schema"/r["path"]).read_bytes())==r["checkoutSha256"]
 if r["baseSha256"] is not None:assert sha(subprocess.check_output(["git","show",plan["base"]+":src/Bosak.Schema/"+r["path"]],cwd=ROOT))==r["baseSha256"]
 if r["path"] in plan["overlays"]:assert sha((P/"overlay"/r["path"]).read_bytes())==r["sha256"]
ns={"t":"http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
def tests(path):
 rows={}
 for p in path.rglob("*.trx"):
  for t in ET.parse(p).findall(".//t:UnitTestResult",ns):
   assert t.attrib["outcome"]=="Passed",(p,t.attrib)
   name=t.attrib["testName"];assert name not in rows;rows[name]="Passed"
 return rows
prior=tests(ROOT/"docs/particles/implementation-evidence/source-final34");now=tests(P/"source-qualified");assert len(prior)==3739 and len(now)==3885 and all(now.get(k)==v for k,v in prior.items())
for lane in ["minimum-consumer","current-consumer"]:assert len(tests(P/(lane+"-results")))==2634
assert len(tests(P/"current-native-results"))==60
for lane,n in [("legacy124",124),("alpha23",23),("beta33",33)]:assert len(tests(P/(lane+"-results")))==n
admissions=read(P.parent/"ADMISSIONS.json");rows=admissions["plannedRows"];assert len(rows)==23 and sum(r["authoredExpectationChangeRequired"] for r in rows)==18
assert admissions["measuredFailureCount"]==18 and admissions["unexpectedOriginalFailures"]==0
old=[r for r in read(ROOT/"tests/TestData/Particles/case-plan.json")["cases"] if r["phase"]=="all-second"]
for a,b in zip(rows,old):
 assert a["id"]==b["id"] and a["schemaSha256"]==sha(b["schema"].encode()) and a["instanceSha256"]==sha(b["instance"].encode())
 assert a["proposedCompile"]==b["expectedCompile"]
for lane in ["minimum","current"]:
 assert read(P/("pilot-"+lane)/"RESULTS.json")["counts"]==dict(match=21,mismatch=0,unsupported=0,harnessError=0)
 delta=read(P/"EXPANDED-DELTA.json")[lane];assert delta["counts"]==dict(match=8,unsupported=121,mismatch=0,harnessError=0) and len(delta["deltas"])==6
 for d in delta["deltas"]:
  a,b=d["before"],d["after"];assert a["classification"]==b["classification"]=="unsupported"
  assert {k:v for k,v in a["native"].items() if k!="reason"}=={k:v for k,v in b["native"].items() if k!="reason"}
frozen=read(ROOT/"docs/conformance-pilot/CASES.json")
for rel,h in frozen["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/rel).read_bytes())==h
expandedFiles=0
for folder in [ROOT/"docs/expanded-conformance",ROOT/"docs/expanded-conformance/cohort2"]:
 manifest=read(folder/"CASES.json")
 for rel,h in manifest["files"].items():assert sha((folder/"upstream"/rel).read_bytes())==h
 expandedFiles+=len(manifest["files"])
assert len(frozen["files"])==42 and expandedFiles==274
surface=read(P/"SURFACE.json");assert surface["zeroPublicApiDelta"] and surface["currentPublicTypes"]==54 and surface["preservedRows"]==401
identity=read(P/"IDENTITY-FINAL.json");assert identity["unsignedLocalCandidate"] and identity["publishedBeta1SignatureAndPayloadVerified"] and sum(len(v["core"]) for v in identity["lanes"].values())==18
for v in identity["lanes"].values():
 assert v["packageOnly"] and v["schemaPayloadIdentical"]
 for row in v["core"]:assert row["signatureVerified"] and row["payloadIdentical"]
assert all(v["onlySchemaDllChanged"] for v in identity["retained"])
examples=read(P/"PACKAGED-EXAMPLES.json");assert examples["readmeSha256"]==sha((ROOT/"docs/NUGET_README.md").read_bytes())
assert examples["allSchemaSha256"]==sha((P/"readme-example/example-all.xsd").read_bytes()) and examples["allInstanceSha256"]==sha((P/"readme-example/example-all.xml").read_bytes())
assert "Exact packaged all example passes" in readlog(P/"readme-example/run-exact.log")
adoption=read(P.parent/"PRODUCTION-ADOPTION.json");assert adoption["productionAdoption"] and adoption["base"]==plan["base"] and adoption["authoredCompileExpectationChanges"]==18
assert adoption["feasibilitySealSha256"]==sha((P.parent/"feasibility-evidence/PACKET-SHA256.json").read_bytes())
render=read(P/"DOCUMENTATION-CHECKS.json");assert len(render["pages"])==14 and render["localLinks"]==775
assert "RESULT: ALL CHECKS PASSED" in readlog(P/"DOCS-CHECK.log")
assert "NuGetAudit>false" not in (ROOT/"Directory.Build.props").read_text(encoding="utf-8")
out=dict(date="2026-11-23",base=plan["base"],priorPackets=packets,priorEntries=entries,productionInputs=311,changedExistingInputs=6,newInputs=15,sourceFiles=61,sourceOverlays=4,sourceTests=3885,preservedSourceCases=3739,addedSourceControls=146,authoredCompileExpectationChanges=18,packageConsumersPerCore=2634,currentPackageNative=60,retained=[124,23,33],signedCorePackages=18,publicApiDelta=0,frozenFiles=42,expandedFiles=274,expandedFirstRefusalChangesPerCore=6,productionAdoption=True,nugetAdvisoryChecksEnabled=True,published=False)
(P/"CHECKS.json").write_text(json.dumps(out,indent=2)+"\n",encoding="utf-8");print(json.dumps(out))
