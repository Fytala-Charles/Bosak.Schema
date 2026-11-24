"""Verify post-particle prioritization without rerunning or editing historical evidence.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,collections,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[1]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"));sha=lambda b:hashlib.sha256(b).hexdigest()
def log(p):
 b=p.read_bytes();return b.decode("utf-16") if b.startswith((b"\xff\xfe",b"\xfe\xff")) else b.decode("utf-8-sig")
base=read(P/"RANKING.json")["base"];packets=entries=0
for p in (ROOT/"docs").rglob("PACKET-SHA256.json"):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(54,3497),(packets,entries)
inputs=read(P/"ENGINE-INPUTS.json");assert len(inputs)==311
for rel,h in inputs.items():assert sha((ROOT/rel).read_bytes())==h,rel
merge=read(P/"PR44-MERGE.json");ci=read(P/"MAIN-CI.json");assert merge["state"]=="MERGED" and merge["mergeCommit"]["oid"]==ci["headSha"]==base and ci["conclusion"]=="success"
assert subprocess.check_output(["git","rev-parse",base+"^{tree}"],cwd=ROOT)==subprocess.check_output(["git","rev-parse",merge["headRefOid"]+"^{tree}"],cwd=ROOT)
passed=[s for s in log(P/"MAIN-CI-FULL.log").splitlines() if "Passed!" in s];assert any("1251" in s for s in passed) and any("2634" in s for s in passed)
sample=read(P/"CURRENT-SAMPLE.json");assert not sample["corpusReexecuted"] and sample["caseCount"]==129 and sample["counts"]==dict(match=8,unsupported=121,mismatch=0,harnessError=0) and sample["conformancePercentage"] is None
rows=[]
for c in sample["corpora"]:
 path=ROOT/c["qualifiedResult"];assert sha(path.read_bytes())==c["qualifiedResultSha256"]
 assert sha((ROOT/c["manifest"]).read_bytes())==c["manifestSha256"]
 manifest=read(ROOT/c["manifest"])
 for name,h in manifest["files"].items():assert sha((ROOT/c["manifest"]).parent.joinpath("upstream",name).read_bytes())==h
 rows+=read(path)["rows"]
assert len(rows)==129 and len({r["id"] for r in rows})==129
actual=collections.defaultdict(collections.Counter)
for r in rows:actual[r["bucket"]][r["classification"]]+=1
assert dict(actual)==sample["buckets"] and len(actual)==18
audit=read(P/"STATIC-COFEATURES.json")["rows"];assert len(audit)==129
for r in audit:assert sha((ROOT/r["schema"]).read_bytes())==r["schemaSha256"]
examples=read(P/"INTEGER-ENUMERATION-EXAMPLES.json")["rows"];assert len(examples)==2
for r in examples:
 assert r["case"]["classification"]=="unsupported" and r["admissionForecast"] is None
 for kind in ["schema","instance","metadata"]:assert sha((ROOT/r["corpusDirectory"]/"upstream"/r["case"][kind]).read_bytes())==r[kind+"Sha256"]
source=read(P/"SOURCE-REVIEW.json");assert len(source["rows"])==6
for r in source["rows"]:
 assert sha((P/"reviewed-source"/r["path"]).read_bytes())==r["sha256"]
 assert sha(subprocess.check_output(["git","show",base+":"+r["path"]],cwd=ROOT))==r["sha256"]
rank=read(P/"RANKING.json")["rows"];assert len(rank)==6 and rank[0]["candidate"]=="Bounded integer enumeration restrictions" and all(r["admissionForecast"] is None for r in rank)
for r in read(P/"CURRENT-SLICES.json")["slices"]:assert (ROOT/r["evidence"]).is_file()
render=read(P/"DOCUMENTATION-CHECKS.json");assert len(render["pages"])==11 and render["localLinks"]==749
assert "RESULT: ALL CHECKS PASSED" in log(P/"DOCS-CHECK.log")
subprocess.run(["git","diff","--exit-code",base,"--","src","tests","tools",".github","Directory.Build.props","global.json","docs/specification-coverage/MATRIX.json"],cwd=ROOT,check=True,stdout=subprocess.DEVNULL)
out=dict(date="2026-11-24",base=base,priorPackets=packets,priorEntries=entries,unchangedProductInputs=311,mergedMainSourceTests=3885,mergedMainCiSuccess=True,reusedOriginalCases=129,unsupported=121,matches=8,featureBuckets=18,sourceReviewFiles=6,rankingRows=6,originalIntegerExamples=2,newCorpusExecution=False,semanticAdoption=False,implementationChanged=False,conformancePercentage=None,published=False,renderedPages=11,localLinks=749)
(P/"CHECKS.json").write_text(json.dumps(out,indent=2)+"\n",encoding="utf-8");print(json.dumps(out))
