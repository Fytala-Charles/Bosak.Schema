"""Check production-choice receipts without editing old evidence or launching binaries.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"));sha=lambda b:hashlib.sha256(b).hexdigest()
packets=entries=0
for p in (ROOT/"docs").rglob("PACKET-SHA256.json"):
 if p.parent==P:continue
 for name,h in read(p).items():assert sha((p.parent/name).read_bytes())==h,(p,name)
 packets+=1;entries+=len(read(p))
inputs=read(P/"PRODUCTION-INPUTS.json")
for rel,h in inputs["files"].items():assert sha((ROOT/rel).read_bytes())==h,rel
assert len(inputs["changedExisting"])==5 and len(inputs["new"])==20 and len(inputs["files"])==296
for rel,row in inputs["changedExisting"].items():assert sha((P/"production-inputs"/rel).read_bytes())==row["after"]
for rel in inputs["new"]:assert sha((P/"production-inputs"/rel).read_bytes())==inputs["files"][rel]
plan=read(P/"CANDIDATE-SOURCE.json");assert len(plan["files"])==60 and len(plan["overlays"])==4
for row in plan["files"]:
 assert sha((ROOT/"src/Bosak.Schema"/row["path"]).read_bytes())==row["checkoutSha256"]
 if row["baseSha256"] is not None:
  b=subprocess.check_output(["git","show",plan["base"]+":src/Bosak.Schema/"+row["path"]],cwd=ROOT);assert sha(b)==row["baseSha256"]
 if row["path"] in plan["overlays"]:assert sha((P/"overlay"/row["path"]).read_bytes())==row["sha256"]
ns={"t":"http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
def tests(path):
 rows={}
 for p in path.rglob("*.trx"):
  for t in ET.parse(p).findall(".//t:UnitTestResult",ns):
   assert t.attrib["outcome"]=="Passed",(p,t.attrib)
   name=t.attrib["testName"];assert name not in rows;rows[name]="Passed"
 return rows
prior=tests(ROOT/"docs/derived-integer-types/implementation-evidence/source");now=tests(P/"source-final34");assert len(prior)==3620 and len(now)==3739 and all(now.get(k)==v for k,v in prior.items())
for lane in ["minimum-consumer","current-consumer"]:assert len(tests(P/(lane+"-final34-results")))==2548
assert len(tests(P/"current-native-final34-results"))==65
for lane,n in [("legacy124",124),("alpha23",23),("beta33",33)]:assert len(tests(P/(lane+"-final34-results")))==n
for lane in ["minimum","current"]:
 assert read(P/("pilot-final-"+lane)/"RESULTS.json")["counts"]==dict(match=21,mismatch=0,unsupported=0,harnessError=0)
 delta=read(P/"EXPANDED-DELTA.json")[lane];assert len(delta["deltas"])==6 and delta["counts"]==dict(match=8,unsupported=121,mismatch=0,harnessError=0)
 for d in delta["deltas"]:
  a=d["before"];b=d["after"];assert a["classification"]==b["classification"]=="unsupported"
  assert {k:v for k,v in a["native"].items() if k!="reason"}=={k:v for k,v in b["native"].items() if k!="reason"}
# All original corpus bytes remain verified independently of the successful runner receipts.
frozen=read(ROOT/"docs/conformance-pilot/CASES.json")
for rel,h in frozen["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/rel).read_bytes())==h
expandedFiles=0
for folder in [ROOT/"docs/expanded-conformance",ROOT/"docs/expanded-conformance/cohort2"]:
 manifest=read(folder/"CASES.json")
 for rel,h in manifest["files"].items():assert sha((folder/"upstream"/rel).read_bytes())==h
 expandedFiles+=len(manifest["files"])
assert len(frozen["files"])==42 and expandedFiles==274
surface=read(P/"SURFACE.json");assert surface["zeroPublicApiDelta"] and surface["currentPublicTypes"]==54 and surface["preservedRows"]==401
identity=read(P/"IDENTITY-FINAL.json");assert identity["unsignedLocalCandidate"] and identity["publishedBeta1SignatureAndPayloadVerified"]
assert sum(len(v["core"]) for v in identity["lanes"].values())==18
for lane,v in identity["lanes"].items():
 assert v["packageOnly"] and v["schemaPayloadIdentical"]
 for row in v["core"]:assert row["signatureVerified"] and row["payloadIdentical"]
assert all(v["onlySchemaDllChanged"] for v in identity["retained"])
adoption=read(P.parent/"PRODUCTION-ADOPTION.json");assert adoption["base"]==plan["base"] and not adoption["allPhaseAdopted"]
assert adoption["feasibilitySealSha256"]==sha((P.parent/"choice-feasibility-evidence/PACKET-SHA256.json").read_bytes())
docs=read(P/"DOCUMENTATION-CHECKS.json");assert len(docs["pages"])==14 and docs["localLinks"]==727
out=dict(date="2026-11-23",priorPackets=packets,priorEntries=entries,productionInputs=296,changedExistingInputs=5,newInputs=20,sourceFiles=60,sourceOverlays=4,sourceTests=3739,preservedSourceCases=3620,addedSourceControls=119,packageConsumersPerCore=2548,currentPackageNative=65,retained=[124,23,33],signedCorePackages=18,publicApiDelta=0,frozenFiles=42,expandedFiles=274,expandedFirstRefusalChangesPerCore=6,allAdopted=False,freshNuGetAdvisoryData=False,published=False)
(P/"CHECKS.json").write_text(json.dumps(out,indent=2)+"\n",encoding="utf-8");print(json.dumps(out))
