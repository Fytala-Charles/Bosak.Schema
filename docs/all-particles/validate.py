"""Read-only historical evidence verification for the separate all-phase contract.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[1]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"));sha=lambda b:hashlib.sha256(b).hexdigest()
status=read(P/"STATUS.json");base=status["base"];packets=entries=0
for p in (ROOT/"docs").rglob("PACKET-SHA256.json"):
 if p.parent==P:continue
 for name,h in read(p).items():assert sha((p.parent/name).read_bytes())==h,(p,name)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(51,3207),(packets,entries)
inputs=read(P/"ENGINE-INPUTS.json");assert len(inputs)==296
for rel,h in inputs.items():assert sha((ROOT/rel).read_bytes())==h,rel
source=read(P/"SOURCE-REVIEW.json");assert source["base"]==base and len(source["rows"])==10
for r in source["rows"]:
 assert sha((P/"reviewed-schema"/r["path"]).read_bytes())==r["sha256"]
 assert sha(subprocess.check_output(["git","show",base+":"+r["path"]],cwd=ROOT))==r["sha256"]
plan=read(P/"CASE-PLAN.json");cases=plan["cases"];assert len(cases)==43 and len({r["id"] for r in cases})==43
old=[r for r in read(ROOT/"docs/particles/CASE-PLAN.json")["cases"] if r["phase"]=="all-second"];assert len(old)==23
for before,after in zip(old,cases[:23]):
 for key in ["id","schema","instance","expectedCompile","expectedAssessment"]:assert before[key]==after[key],key
for row in cases:ET.fromstring(row["schema"]);ET.fromstring(row["instance"])
refusals=read(P/"EXISTING-REFUSALS.json")["rows"];assert len(refusals)==23 and sum(r["authoredExpectationChangeRequired"] for r in refusals)==18
for row,case in zip(refusals,cases):
 assert row["id"]==case["id"] and row["schemaSha256"]==sha(case["schema"].encode()) and row["instanceSha256"]==sha(case["instance"].encode())
 assert row["currentCompile"]=="Unsupported" and row["proposedCompile"]==case["expectedCompile"]
assert len(read(P/"NATIVE-GATES.json")["gates"])==20
original=read(P/"ORIGINAL-EXAMPLES.json")["rows"];assert len(original)==3
for row in original:
 for key in ["metadata","schema","instance"]:assert sha((ROOT/row["corpusDirectory"]/"upstream"/row["case"][key]).read_bytes())==row["inputSha256"][key]
merge=read(P/"PR43-MERGE.json");ci=read(P/"MAIN43-CI.json");assert merge["state"]=="MERGED" and merge["mergeCommit"]["oid"]==ci["headSha"]==base and ci["conclusion"]=="success"
assert subprocess.check_output(["git","rev-parse",base+"^{tree}"],cwd=ROOT)==subprocess.check_output(["git","rev-parse",merge["headRefOid"]+"^{tree}"],cwd=ROOT)
log=(P/"MAIN43-CI-FULL.log").read_text(encoding="utf-8-sig");passed=[s for s in log.splitlines() if "Passed!" in s];assert any("1191" in s for s in passed) and any("2548" in s for s in passed)
assert not status["semanticAdoption"] and not status["sourceChanged"] and not status["nativeFeasibilityExecuted"]
assert len(read(P/"SPECIFICATION-REVIEW.json")["sources"])==4
render=read(P/"DOCUMENTATION-CHECKS.json");assert len(render["pages"])==11 and render["localLinks"]==732
assert "RESULT: ALL CHECKS PASSED" in (P/"DOCS-CHECK.log").read_text(encoding="utf-8-sig")
subprocess.run(["git","diff","--exit-code",base,"--","src","tests","tools",".github","Directory.Build.props","global.json"],cwd=ROOT,check=True,stdout=subprocess.DEVNULL)
out=dict(date="2026-11-23",base=base,priorPackets=packets,priorEntries=entries,unchangedInputs=296,sourceReviewFiles=10,specificationBodies=4,cases=43,inheritedPlans=23,additionalPlans=20,nativeGates=20,originalCoBlockedExamples=3,existingRefusalControls=23,proposedFutureExpectationChanges=18,semanticAdoption=False,nativeFeasibilityExecuted=False,mergedMainSourceTests=3739,mergedMainCiSuccess=True,renderedPages=11,localLinks=732,published=False)
(P/"CHECKS.json").write_text(json.dumps(out,indent=2)+"\n",encoding="utf-8");print(json.dumps(out))
