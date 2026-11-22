"""Validate the simple-root contract without executing proposed semantics.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import hashlib,json,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[1]
read=lambda f:json.loads(f.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
prior=entries=0
for receipt in (ROOT/"docs").rglob("PACKET-SHA256.json"):
    if receipt.parent==P:continue
    for f,h in read(receipt).items():assert sha((receipt.parent/f).read_bytes())==h,(receipt,f);entries+=1
    prior+=1
spec=read(P/"SPECIFICATION-REVIEW.json")
for row in spec["sources"]:assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
source=read(P/"SOURCE-REVIEW.json")
for row in source["rows"]:
    assert sha((P/"reviewed-schema"/row["path"]).read_bytes())==row["gitBlobSha256"]
    b=subprocess.check_output(["git","show",row["commit"]+":"+row["path"]],cwd=ROOT)
    assert sha(b)==row["gitBlobSha256"]
for row in read(P/"CORE-SOURCE-REVIEW.json")["rows"]:assert sha((P/row["copy"]).read_bytes())==row["gitBlobSha256"]
cases=read(P/"CASE-PLAN.json")["cases"]
assert len(cases)==38 and len({c["id"] for c in cases})==38
for c in cases:
    ET.fromstring(c["schema"]);ET.fromstring(c["instance"])
    assert c["status"].startswith("Proposed unexecuted")
assert len(read(P/"NATIVE-GATES.json")["gates"])==17
frozen=read(ROOT/"docs/conformance-pilot/CASES.json")
for f,h in frozen["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/f).read_bytes())==h
main=read(P/"MAIN-CI.json");assert main["headSha"]==spec["base"] and main["conclusion"]=="success"
merge=read(P/"PR40-MERGE.json");assert merge["state"]=="MERGED" and merge["mergeCommit"]["oid"]==spec["base"]
headTree=subprocess.check_output(["git","rev-parse",spec["base"]+"^{tree}"],cwd=ROOT)
reviewTree=subprocess.check_output(["git","rev-parse",merge["headRefOid"]+"^{tree}"],cwd=ROOT)
assert headTree==reviewTree
assert not subprocess.check_output(["git","diff","--name-only","--","src","tests","tools",".github","Directory.Build.props","global.json"],cwd=ROOT).strip()
docs=read(P/"DOCUMENTATION-CHECKS.json");assert len(docs["pages"])==11 and docs["localLinks"]==636
assert "RESULT: ALL CHECKS PASSED" in (P/"DOCS-CHECK.log").read_text(encoding="utf-8-sig")
checks=dict(status="Passed contract evidence validation; proposed semantics unexecuted",base=spec["base"],priorSeals=prior,priorEntries=entries,reviewedSpecificationBodies=4,reviewedSchemaFiles=10,reviewedCoreFiles=4,plannedXmlPayloads=38,plannedNativeGates=17,frozenFiles=len(frozen["files"]),productSourceChanged=False,semanticAdoption=False,mainCi="success",reviewedTreePreserved=True,renderedPages=11,localLinks=636,visualAcceptance=False)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
