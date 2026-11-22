"""Verify sealed-source, native receipts and preserved contract evidence.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
prior=entries=0
for receipt in (ROOT/"docs").rglob("PACKET-SHA256.json"):
    if receipt.parent==P:continue
    for f,h in read(receipt).items():assert sha((receipt.parent/f).read_bytes())==h,(receipt,f);entries+=1
    prior+=1
m=read(P/"CANDIDATE-SOURCE.json");assert len(m["files"])==58 and len(m["overlays"])==3
for row in m["files"]:
    b=subprocess.check_output(["git","show",m["base"]+":src/Bosak.Schema/"+row["path"]],cwd=ROOT)
    assert sha(b)==row["gitBaseSha256"]
    assert b.replace(b"\r\n",b"\n")== (ROOT/"src/Bosak.Schema"/row["path"]).read_bytes().replace(b"\r\n",b"\n")
    if row["overlay"]:b=(P/"overlay"/row["path"]).read_bytes()
    elif row["lineEndings"]=="CRLF":b=b.replace(b"\r\n",b"\n").replace(b"\n",b"\r\n")
    assert sha(b)==row["sha256"]
for row in read(P/"EXPERIMENT-ADOPTION.json")["sources"]:assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
def counters(folder):
    result=dict(passed=0,failed=0,total=0)
    for f in folder.glob("*.trx"):
        c=ET.parse(f).find(".//{*}Counters")
        for k in result:result[k]+=int(c.attrib[k])
    return result
expected={"minimum/results/final":(135,0),"current/results/final":(135,0),"baseline-minimum/results":(4,0),"baseline-current/results/corrected":(4,0),"regression/results/internal":(948,0),"regression/results/public":(2101,1),"regression/results/public-final":(2102,0)}
for folder,(passed,failed) in expected.items():assert counters(P/folder)==dict(passed=passed,failed=failed,total=passed+failed),folder
admission=read(P/"REGRESSION-ADMISSIONS.json")["rows"];assert len(admission)==1 and admission[0]["id"]=="AS-050"
a,b=admission[0]["before"],admission[0]["after"];assert {k for k in a if a[k]!=b[k]}=={"expectedCompile"};assert a["expectedCompile"]=="Unsupported" and b["expectedCompile"]=="Valid"
original=next(x for x in read(ROOT/"tests/TestData/AnonymousSimpleRestrictions/cases.json") if x["id"]=="AS-050");assert original==a
assert {x["id"] for x in read(P/"PLAN-RECONCILIATIONS.json")["rows"]}=={"SR-008","SR-009"}
assert (P/"case-plan.json").read_bytes()==(P.parent/"CASE-PLAN.json").read_bytes()
assert (P/"sku.xsd").read_bytes()==(ROOT/"tests/TestData/Schemas/sku.xsd").read_bytes()
for case in read(P/"case-plan.json")["cases"]:ET.fromstring(case["schema"]);ET.fromstring(case["instance"])
assert read(P/"STABLE-CASES.json")["priorCasesPreserved"]==3050
api=read(P/"SURFACE.json");assert api["zeroPublicApiDelta"] and api["currentPublicTypes"]==54 and api["preservedRows"]==401
ident=read(P/"IDENTITY.json");n=0
for lane in ["minimum","current"]:
    assert ident["lanes"][lane]["coreProjectReferences"] is False
    for row in ident["lanes"][lane]["packages"]:
        assert row["signatureVerified"] and row["cacheRuntimeMatch"];assert (P/"signatures"/(lane+"-"+row["id"]+".log")).exists();n+=1
assert n==18
frozen=read(ROOT/"docs/conformance-pilot/CASES.json")
for f,h in frozen["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/f).read_bytes())==h
assert not subprocess.check_output(["git","diff","--name-only","--","src","tests","tools",".github","Directory.Build.props","global.json"],cwd=ROOT).strip()
assert "RESULT: ALL CHECKS PASSED" in (P/"DOCS-CHECK.log").read_text(encoding="utf-8-sig")
docs=read(P/"DOCUMENTATION-CHECKS.json");assert len(docs["pages"])==12 and docs["localLinks"]==653
checks=dict(status="Qualified isolated bounded simple-root feasibility",nativePerCore=135,baselinePerCore=4,sourceRegressionInternal=948,sourceRegressionPublicScratchReconciled=2102,explicitAdmission="AS-050",priorStableIdentities=3050,overlays=3,sourceFiles=58,signedCoreIdentities=18,publicApiDelta=0,priorSeals=prior,priorSealedEntries=entries,frozenFiles=42,renderedPages=12,localLinks=653,productSourceUnchanged=True,productionAdoption=False,packageRetainedCorpusExecuted=False)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
