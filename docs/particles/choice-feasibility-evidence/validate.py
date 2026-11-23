"""Verify choice feasibility receipts without changing prior evidence or launching binaries.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,xml.etree.ElementTree as ET,subprocess
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
packets=entries=0
for p in (ROOT/"docs").rglob("PACKET-SHA256.json"):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
inputs=read(P.parent/"ENGINE-INPUTS.json")
for rel,h in inputs.items():assert sha((ROOT/rel).read_bytes())==h,rel
plan=read(P/"CANDIDATE-SOURCE.json");assert len(plan["files"])==60 and len(plan["overlays"])==4
for row in plan["files"]:
 if row["baseSha256"] is not None:
  b=subprocess.check_output(["git","show",plan["base"]+":src/Bosak.Schema/"+row["path"]],cwd=ROOT);assert sha(b)==row["baseSha256"]
 if row["path"] in plan["overlays"]:assert sha((P/"overlay"/row["path"]).read_bytes())==row["sha256"]
ns={"t":"http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
def tests(files):
 rows={}
 for p in files:
  for t in ET.parse(p).findall(".//t:UnitTestResult",ns):
   assert t.attrib["outcome"]=="Passed",p
   name=t.attrib["testName"];assert name not in rows,name;rows[name]="Passed"
 return rows
for lane in ["minimum","current"]:assert len(tests((P/lane/"results/verified").rglob("*.trx")))==119
for lane in ["baseline-minimum","baseline-current"]:assert len(tests((P/lane).rglob("*.trx")))==4
now=tests((P/"regression/results").rglob("*.trx"));prior=tests((ROOT/"docs/derived-integer-types/implementation-evidence/source").rglob("*.trx"));assert now==prior and len(now)==3620
assert read(P/"SURFACE.json")["zeroPublicApiDelta"]
identity=read(P/"IDENTITY.json");assert sum(len(l["packages"]) for l in identity["lanes"].values())==18
for l in identity["lanes"].values():
 for row in l["packages"]:assert row["signatureVerified"] and row["cacheRuntimeMatch"]
merge=read(P/"PR42-MERGE.json");ci=read(P/"MAIN-CI.json");assert merge["state"]=="MERGED" and ci["conclusion"]=="success" and ci["headSha"]==plan["base"]==merge["mergeCommit"]["oid"]
assert subprocess.check_output(["git","rev-parse",plan["base"]+"^{tree}"],cwd=ROOT)==subprocess.check_output(["git","rev-parse",merge["headRefOid"]+"^{tree}"],cwd=ROOT)
docs=read(P/"DOCUMENTATION-CHECKS.json");assert len(docs["pages"])==12
out=dict(date="2026-11-23",priorPackets=packets,priorEntries=entries,unchangedProductionInputs=len(inputs),sourceFiles=60,overlays=4,nativeControlsPerPin=119,baselineRefusalsPerPin=4,unchangedSourceTests=3620,expectationReconciliations=0,signedCorePackages=18,publicApiDelta=0,allSemanticsDeferred=True,productionAdoption=False,publication=False,mergeAndMainCiVerified=True)
(P/"CHECKS.json").write_text(json.dumps(out,indent=2)+"\n",encoding="utf-8");print(json.dumps(out))
