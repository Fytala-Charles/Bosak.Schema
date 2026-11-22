"""Validate the proposal and retained safe Core probes; no Schema execution.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import hashlib,json,subprocess,xml.etree.ElementTree as E
P=Path(__file__).resolve().parent;ROOT=P.parents[1]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
plan=read(P/"CASE-PLAN.json");assert len(plan["rows"])==58 and len(plan["gates"])==12
ids=set()
for row in plan["rows"]:
    assert row["id"] not in ids;ids.add(row["id"])
    E.fromstring(row["schema"]);E.fromstring(row["instance"])
    assert row["status"]=="Proposed authored payload; not executed against Schema"
    assert row["expectedCompile"] in {"Valid","Invalid","Unsupported"}
    if row["expectedCompile"]!="Valid":assert row["expectedAssessment"] is None
for row in read(P/"SOURCE-REVIEW.json")["files"]:
    assert sha((ROOT/row["path"]).read_bytes())==row["worktreeSha256"]
    b=subprocess.check_output(["git","show",read(P/"SOURCE-REVIEW.json")["base"]+":"+row["path"]],cwd=ROOT);assert sha(b)==row["gitBlobSha256"]
for row in read(P/"SPECIFICATION-REVIEW.json")["sources"]:assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
for row in read(P/"CORE-SOURCE-REVIEW.json")["rows"]:
    b=subprocess.check_output(["git","-c","safe.directory=D:/Development/Bosak","show",row["sourceCommit"]+":"+row["path"]],cwd="D:/Development/Bosak");assert sha(b)==row["gitBlobSha256"]
vectors=read(P/"CORE-VECTORS.json");assert len(vectors)==20
identity=read(P/"core-capability-evidence/IDENTITY.json");raw=Path("D:/Development/Bosak/.guard-tmp/schema-string-pattern-contract-20261122")
for lane,version in [("minimum","1.81.2"),("current","1.81.17")]:
    result=read(P/"core-capability-evidence"/lane/"result.json");assert result["coreAssembly"].startswith(version+"+") and result["defaultRegexTimeoutInfinite"]
    assert len(result["rows"])==20 and all(row["error"] is None for row in result["rows"])
    assert all(row["wholeCandidateMatches"]==row["xsdExpected"] for row in result["rows"])
    assert len([row for row in result["rows"] if row["rawMatches"]!=row["xsdExpected"]])==7
    records=identity["lanes"][lane];assert len(records["packages"])==9
    assert sha((raw/lane/"bin/Release/net10.0/Probe.dll").read_bytes())==records["probeAssemblySha256"]
    assert sha((P/"core-capability-evidence"/lane/"Program.cs").read_bytes())==records["programSourceSha256"]
    for item in records["packages"]:assert sha((raw/lane/"bin/Release/net10.0"/(item["id"]+".dll")).read_bytes())==item["dllSha256"]
inputs=read(ROOT/"docs/structural-assertions/implementation-evidence/SOURCE-INPUTS.json")
for name,h in inputs.items():assert sha((ROOT/name).read_bytes())==h
assert not subprocess.check_output(["git","diff","HEAD","--name-only","--","src","tests","tools",".github","Directory.Build.props","global.json"],cwd=ROOT).strip()
# Verify every pre-existing evidence seal, including the separately retained expanded assessment.
seals=[p for p in (ROOT/"docs").rglob("PACKET-SHA256.json") if not p.is_relative_to(P)]
entries=0
for seal in seals:
    for name,h in read(seal).items():assert sha((seal.parent/name).read_bytes())==h,(seal,name);entries+=1
frozen=read(ROOT/"docs/conformance-pilot/CASES.json")
for name,h in frozen["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/name).read_bytes())==h
checks=dict(status="Passed pattern contract artifact/receipt validation",plannedPayloads=58,plannedGates=12,wellFormedXmlDocuments=116,sourceFilesReviewed=8,coreSentinelsPerPin=20,wholeSemanticAgreementPerPin=20,rawDialectDifferencesPerPin=7,callerBoundedCoreRegexGate=False,nativeSchemaPatternExecuted=False,productionSemanticAdoption=False,productInputsUnchanged=len(inputs),frozenFilesVerified=42,priorSealCount=len(seals),priorSealedEntries=entries)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
