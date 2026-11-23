"""Verify all feasibility without altering historical packets or launching binaries.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET,re
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"));sha=lambda b:hashlib.sha256(b).hexdigest()
packets=entries=0
for p in (ROOT/"docs").rglob("PACKET-SHA256.json"):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(52,3234),(packets,entries)
inputs=read(P.parent/"ENGINE-INPUTS.json");assert len(inputs)==296
for rel,h in inputs.items():assert sha((ROOT/rel).read_bytes())==h,rel
plan=read(P/"CANDIDATE-SOURCE.json");assert len(plan["files"])==61 and len(plan["overlays"])==4
for row in plan["files"]:
 if row["baseSha256"] is not None:assert sha(subprocess.check_output(["git","show",plan["base"]+":src/Bosak.Schema/"+row["path"]],cwd=ROOT))==row["baseSha256"]
 if row["path"] in plan["overlays"]:assert sha((P/"overlay"/row["path"]).read_bytes())==row["sha256"]
ns={"t":"http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
def tests(path):
 out={}
 for f in path.rglob("*.trx"):
  for row in ET.parse(f).findall(".//t:UnitTestResult",ns):
   n=row.attrib["testName"];assert n not in out,n;out[n]=row.attrib["outcome"]
 return out
for lane in ["minimum","current"]:
 result=tests(P/lane/"results/delivery");assert len(result)==146 and set(result.values())=={"Passed"}
 baseline=tests(P/("baseline-"+lane)/"delivery-results");assert len(baseline)==86 and set(baseline.values())=={"Passed"}
previous=tests(ROOT/"docs/particles/implementation-evidence/source-final34")
initial=tests(P/"regression/initial-Bosak.Schema.Tests")|tests(P/"regression/initial-Bosak.Schema.PublicConsumer.Tests")
final=tests(P/"regression/final-internal")|tests(P/"regression/initial-Bosak.Schema.PublicConsumer.Tests")
assert len(previous)==len(initial)==len(final)==3739 and previous.keys()==initial.keys()==final.keys()
assert set(previous.values())==set(final.values())=={"Passed"}
failed={k:v for k,v in initial.items() if v!="Passed"};assert len(failed)==18
reconcile=read(P.parent/"EXISTING-REFUSALS.json")["rows"];expected={r["id"] for r in reconcile if r["authoredExpectationChangeRequired"]}
assert {re.search(r'id: "(PT-\d+)"',n).group(1) for n in failed}==expected
assert all(".AllRemainsExcluded(" in n for n in failed)
source=read(P/"case-plan.json")["cases"];assert len(source)==43
assert sha((P/"case-plan.json").read_bytes())==sha((P.parent/"CASE-PLAN.json").read_bytes())
assert len(read(P/"GATE-REVIEW.json")["gates"])==20
original=read(P.parent/"ORIGINAL-EXAMPLES.json")["rows"];copied=read(P/"originals.json");assert len(original)==len(copied)==3
for row,out in zip(original,copied):
 assert row["case"]["id"]==out["id"]
 for kind in ["schema","instance"]:assert sha((P/out[kind]).read_bytes())==row["inputSha256"][kind]
 for kind in ["metadata","schema","instance"]:assert sha((ROOT/row["corpusDirectory"]/"upstream"/row["case"][kind]).read_bytes())==row["inputSha256"][kind]
surface=read(P/"SURFACE.json");assert surface["zeroPublicApiDelta"] and surface["currentPublicTypes"]==54 and surface["preservedRows"]==401
identity=read(P/"IDENTITY.json");assert sum(len(v["packages"]) for v in identity["lanes"].values())==18
for lane,v in identity["lanes"].items():
 assert not v["coreProjectReferences"] and v["isolatedSchemaProjectReference"]
 for row in v["packages"]:assert row["signatureVerified"] and row["cacheRuntimeMatch"]
adoption=read(P/"EXPERIMENT-ADOPTION.json");assert not adoption["productionAdoption"] and adoption["base"]==plan["base"]
assert adoption["contractSealSha256"]==sha((P.parent/"PACKET-SHA256.json").read_bytes())
render=read(P/"DOCUMENTATION-CHECKS.json");assert len(render["pages"])==12 and render["localLinks"]==754
assert "RESULT: ALL CHECKS PASSED" in (P/"DOCS-CHECK.log").read_text(encoding="utf-8-sig")
subprocess.run(["git","diff","--exit-code",plan["base"],"--","src","tests","tools",".github","Directory.Build.props","global.json"],cwd=ROOT,check=True,stdout=subprocess.DEVNULL)
out=dict(date="2026-11-23",base=plan["base"],priorPackets=packets,priorEntries=entries,unchangedProductionInputs=296,gitBaseFiles=60,candidateFiles=61,overlays=4,candidateControlsPerPin=146,baselineRefusalsPerPin=86,originalSourceCases=3739,initialSourcePassed=3721,initialSourcePlannedFailures=18,reconciledSourcePassed=3739,unexpectedSourceFailures=0,authoredCompileExpectationChanges=18,sourceCaseRemovals=0,sourceCaseRenames=0,signedCorePackages=18,publicApiDelta=0,originalAllExamples=3,renderedPages=12,localLinks=754,productionAdoption=False,productPackageProduced=False,fullCorpusExecuted=False,retainedCallersExecuted=False,published=False)
(P/"CHECKS.json").write_text(json.dumps(out,indent=2)+"\n",encoding="utf-8");print(json.dumps(out))
