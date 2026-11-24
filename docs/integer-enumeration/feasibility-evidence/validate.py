"""Verify integer-enumeration feasibility without mutating old packets or launching binaries.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"));sha=lambda b:hashlib.sha256(b).hexdigest()
def log(p):
 b=p.read_bytes();return b.decode("utf-16") if b.startswith((b"\xff\xfe",b"\xfe\xff")) else b.decode("utf-8-sig")
packets=entries=0
for p in (ROOT/"docs").rglob("PACKET-SHA256.json"):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(56,3547),(packets,entries)
for rel,h in read(P.parent/"ENGINE-INPUTS.json").items():assert sha((ROOT/rel).read_bytes())==h,rel
plan=read(P/"CANDIDATE-SOURCE.json");assert len(plan["files"])==61 and len(plan["overlays"])==3
for r in plan["files"]:
 assert sha(subprocess.check_output(["git","show",plan["base"]+":src/Bosak.Schema/"+r["path"]],cwd=ROOT))==r["baseSha256"]
 if r["path"] in plan["overlays"]:assert sha((P/"overlay"/r["path"]).read_bytes())==r["sha256"]
ns={"t":"http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
def results(paths):
 rows={}
 for p in paths:
  for t in ET.parse(p).findall(".//t:UnitTestResult",ns):
   name=t.attrib["testName"];assert name not in rows,name;rows[name]=t.attrib["outcome"]
 return rows
for lane in ["minimum","current"]:
 rows=results((P/lane/"results/sealed").glob("*.trx"));assert len(rows)==261 and set(rows.values())=={"Passed"}
 rows=results((P/("baseline-"+lane)/"final-results").glob("*.trx"));assert len(rows)==210 and set(rows.values())=={"Passed"}
before=results((ROOT/"docs/all-particles/implementation-evidence/source-qualified").glob("*.trx"))
initial=results((P/"regression").glob("initial-*/*.trx"));final=results((P/"regression").glob("reconciled-*/*.trx"))
assert len(before)==len(initial)==len(final)==3885 and before.keys()==initial.keys()==final.keys() and set(final.values())=={"Passed"}
failed=[n for n,v in initial.items() if v!="Passed"];assert len(failed)==10 and set(failed)==set(read(P/"REGRESSION-RECONCILIATION.json")["initialFailedNames"])
recon=read(P/"REGRESSION-RECONCILIATION.json")
for r in recon["files"]:
 assert sha((P/"regression/before"/r["path"]).read_bytes())==r["beforeSha256"]
 assert sha((P/"regression/tests"/r["path"]).read_bytes())==r["afterSha256"]
 # Schemas, instances and IDs are retained even where expectations change.
 if r["path"].endswith("cases.json"):
  def index(d):return {x["id"]:x for x in (d["cases"] if isinstance(d,dict) else d)}
  a=index(read(P/"regression/before"/r["path"]));b=index(read(P/"regression/tests"/r["path"]));assert a.keys()==b.keys()
  for key in a:
   for field in ["id","schema","instance"]:assert a[key][field]==b[key][field]
 assert r["beforeSha256"]!=r["afterSha256"]
assert sha((P/"original-contract.json").read_bytes())==sha((P.parent/"CASE-PLAN.json").read_bytes())
original=read(P/"original-contract.json")["cases"];qualified=read(P/"case-plan.json")["cases"];assert len(original)==len(qualified)==105
changes=[(a,b) for a,b in zip(original,qualified) if a!=b];assert len(changes)==1
a,b=changes[0];assert a["id"]=="IE-073" and b["id"]=="IE-073Q" and a["instance"]==b["instance"]
r=read(P/"PLAN-RECONCILIATION.json");assert r["original"]==a and r["qualified"]==b and not r["engineGrammarWidened"]
examples=read(P.parent/"ORIGINAL-EXAMPLES.json")["rows"];copies=read(P/"originals.json");assert len(examples)==len(copies)==2
for a,b in zip(examples,copies):
 assert a["case"]["id"]==b["id"] and a["case"]["upstreamExpected"]==b["expected"]
 for kind in ["schema","instance"]:assert sha((P/b[kind]).read_bytes())==a[kind+"Sha256"]
surface=read(P/"SURFACE.json");assert surface["zeroPublicApiDelta"] and surface["currentPublicTypes"]==54 and surface["preservedRows"]==401
identity=read(P/"IDENTITY.json");assert sum(len(v["packages"]) for v in identity["lanes"].values())==18
for v in identity["lanes"].values():
 assert not v["coreProjectReferences"] and v["isolatedSchemaProjectReference"]
 for r in v["packages"]:assert r["signatureVerified"] and r["cacheRuntimeMatch"]
a=read(P/"EXPERIMENT-ADOPTION.json");assert not a["productionAdoption"] and a["contractSealSha256"]==sha((P.parent/"PACKET-SHA256.json").read_bytes())
assert len(read(P/"GATE-REVIEW.json")["gates"])==18
render=read(P/"DOCUMENTATION-CHECKS.json");assert len(render["pages"])==12 and render["localLinks"]==785
assert "RESULT: ALL CHECKS PASSED" in log(P/"DOCS-CHECK.log")
subprocess.run(["git","diff","--exit-code",plan["base"],"--","src","tests","tools",".github","Directory.Build.props","global.json"],cwd=ROOT,check=True,stdout=subprocess.DEVNULL)
out=dict(date="2026-11-24",base=plan["base"],priorPackets=packets,priorEntries=entries,unchangedProductInputs=311,sourceFiles=61,sourceOverlays=3,candidateControlsPerPin=261,originalBaselineRefusalsPerPin=210,retainedOriginalContractPayloads=104,qualifiedAttributeVariants=1,originalSourceCases=3885,initialExpectationFailures=10,reconciledSourcePassed=3885,sourceCaseRemovals=0,sourceCaseRenames=0,originalSchemaAdmissions=2,originalValidInstances=1,originalInvalidInstances=1,signedCorePackages=18,publicApiDelta=0,productionAdoption=False,fullCorpusExecuted=False,productPackageProduced=False,retainedCallersExecuted=False,published=False,renderedPages=12,localLinks=785)
(P/"CHECKS.json").write_text(json.dumps(out,indent=2)+"\n",encoding="utf-8");print(json.dumps(out))
