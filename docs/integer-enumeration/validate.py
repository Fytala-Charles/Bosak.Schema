"""Verify the proposed contract; no native engine execution or historical mutation.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[1]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"));sha=lambda b:hashlib.sha256(b).hexdigest()
def log(p):
 b=p.read_bytes();return b.decode("utf-16") if b.startswith((b"\xff\xfe",b"\xfe\xff")) else b.decode("utf-8-sig")
packets=entries=0
for p in (ROOT/"docs").rglob("PACKET-SHA256.json"):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(55,3521),(packets,entries)
status=read(P/"STATUS.json");base=status["base"]
inputs=read(P/"ENGINE-INPUTS.json");assert len(inputs)==311
for rel,h in inputs.items():assert sha((ROOT/rel).read_bytes())==h,rel
plan=read(P/"CASE-PLAN.json");cases=plan["cases"];assert len(cases)==105 and len({r["id"] for r in cases})==105
for r in cases:
 ET.fromstring(r["schema"]);ET.fromstring(r["instance"])
 assert r["expectedCompile"] in ["Valid","Invalid","Unsupported"]
 assert r["expectedAssessment"] in ["Valid","Invalid","NotRun"]
 assert (r["expectedCompile"]=="Valid")== (r["expectedAssessment"]!="NotRun")
 assert "unexecuted" in r["status"]
assert len({r["family"] for r in cases if "family" in r})==13
assert any("Sibling facet intersection" in r["purpose"] and r["expectedCompile"]=="Valid" and r["expectedAssessment"]=="Invalid" for r in cases)
assert any("Inherited enumeration cannot" in r["purpose"] and r["expectedCompile"]=="Invalid" for r in cases)
assert any("Decimal complex" in r["purpose"] and r["expectedCompile"]=="Unsupported" for r in cases)
assert plan["enumerationSourceEntryLimit"]==16 and plan["enumerationLexicalCharacterLimit"]==256
assert len(read(P/"NATIVE-GATES.json")["gates"])==18
review=read(P/"SOURCE-REVIEW.json");assert len(review["rows"])==11
for r in review["rows"]:
 assert sha((P/"reviewed-source"/r["path"]).read_bytes())==r["sha256"]
 assert sha(subprocess.check_output(["git","show",base+":"+r["path"]],cwd=ROOT))==r["sha256"]
impact=read(P/"POTENTIAL-EXPECTATION-IMPACTS.json");assert len(impact["rows"])==6 and not impact["removeOrRenameOriginalStimuli"]
for r in impact["rows"]:assert not r["measuredChange"] and sha((ROOT/r["path"]).read_bytes())==r["currentFileSha256"]
originals=read(P/"ORIGINAL-EXAMPLES.json")["rows"];assert len(originals)==2
assert {r["case"]["upstreamExpected"] for r in originals}=={"valid","invalid"}
for r in originals:
 assert r["case"]["classification"]=="unsupported" and r["admissionForecast"] is None
 for kind in ["schema","instance","metadata"]:assert sha((ROOT/r["corpusDirectory"]/"upstream"/r["case"][kind]).read_bytes())==r[kind+"Sha256"]
spec=read(P/"SPECIFICATION-REVIEW.json");assert len(spec["sources"])==4 and spec["status"]=="Integer-enumeration contract review; no semantic adoption"
assert not status["semanticAdoption"] and not status["implementationChanged"] and not status["nativeFeasibilityExecuted"]
render=read(P/"DOCUMENTATION-CHECKS.json");assert len(render["pages"])==11 and render["localLinks"]==766
assert "RESULT: ALL CHECKS PASSED" in log(P/"DOCS-CHECK.log")
subprocess.run(["git","diff","--exit-code",base,"--","src","tests","tools",".github","Directory.Build.props","global.json","docs/specification-coverage/MATRIX.json"],cwd=ROOT,check=True,stdout=subprocess.DEVNULL)
out=dict(date="2026-11-24",base=base,priorPackets=packets,priorEntries=entries,unchangedProductInputs=311,sourceReviewFiles=11,specificationBodies=4,proposedXmlCases=105,integerFamilies=13,nativeGates=18,originalExamples=2,potentialExpectationImpactGroups=6,measuredExpectationChanges=None,semanticAdoption=False,nativeFeasibilityExecuted=False,implementationChanged=False,renderedPages=11,localLinks=766,published=False)
(P/"CHECKS.json").write_text(json.dumps(out,indent=2)+"\n",encoding="utf-8");print(json.dumps(out))
