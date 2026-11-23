"""Validate a contract based on a reviewed unmerged PR; no engine execution.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
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
source=read(P/"SOURCE-REVIEW.json");assert len(source["rows"])==8
for row in source["rows"]:
    assert sha((P/"reviewed-schema"/row["path"]).read_bytes())==row["sha256"]
    assert sha(subprocess.check_output(["git","show",row["commit"]+":"+row["path"]],cwd=ROOT))==row["sha256"]
for f,h in read(P/"ENGINE-INPUTS.json").items():assert sha((ROOT/f).read_bytes())==h
cases=read(P/"CASE-PLAN.json")["cases"];assert len(cases)==50 and len({x["id"] for x in cases})==50
assert {x["phase"] for x in cases}=={"choice-first","all-second"}
for row in cases:
    ET.fromstring(row["schema"]);ET.fromstring(row["instance"]);assert row["status"].startswith("Proposed unexecuted")
assert len(read(P/"NATIVE-GATES.json")["gates"])==18
examples=read(P/"ORIGINAL-EXAMPLES.json")["rows"];assert len(examples)==6
for row in examples:
    assert row["elementsWithoutExplicitType"]
    for key,h in row["inputSha256"].items():assert sha((ROOT/row["corpusDirectory"]/"upstream"/row["case"][key]).read_bytes())==h
pr=read(P/"PR42-STATE.json");assert pr["state"]=="OPEN" and pr["headRefOid"]==spec["base"]
ci=read(P/"PR42-CI.json");assert ci["headSha"]==spec["base"] and ci["conclusion"]=="success"
assert read(P/"STATUS.json")["pr42MergePending"] and not read(P/"STATUS.json")["semanticAdoption"]
assert not subprocess.check_output(["git","diff","--name-only","--","src","tests","tools",".github","Directory.Build.props","global.json"],cwd=ROOT).strip()
assert "RESULT: ALL CHECKS PASSED" in (P/"DOCS-CHECK.log").read_text(encoding="utf-8-sig")
docs=read(P/"DOCUMENTATION-CHECKS.json");assert len(docs["pages"])==11 and docs["localLinks"]==695
checks=dict(status="Passed contract validation; semantics unexecuted",base=spec["base"],baseKind="Reviewed unmerged PR42 head",mergePending=True,priorSeals=prior,priorEntries=entries,specificationBodies=4,sourceReviewFiles=8,engineInputs=len(read(P/"ENGINE-INPUTS.json")),plannedXmlPayloads=50,plannedNativeGates=18,originalCoBlockedExamples=6,productSourceChanged=False,semanticAdoption=False,renderedPages=11,localLinks=695,visualAcceptance=False)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
