"""Verify native pattern feasibility without building or executing a matcher.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import hashlib,json,subprocess,xml.etree.ElementTree as E
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
raw=Path("D:/Development/Bosak/.guard-tmp/schema-string-pattern-feasibility-20261122")
for row in read(P/"EXPERIMENT-ADOPTION.json")["sources"]:assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
assert sha((P.parent/"CONTRACT.md").read_bytes())==read(P/"EXPERIMENT-ADOPTION.json")["contractSha256"]
plan=read(P/"CANDIDATE-SOURCE.json");assert len(plan["files"])==58 and len(plan["overlays"])==6
for row in plan["files"]:
    if row["baseSha256"] is not None:
        original=subprocess.check_output(["git","show",plan["base"]+":src/Bosak.Schema/"+row["path"]],cwd=ROOT);assert sha(original)==row["baseSha256"]
    if row["path"] in plan["overlays"]:assert sha((P/"overlay"/row["path"]).read_bytes())==row["sha256"]
    for lane in ["minimum","current"]:assert sha((raw/lane/"candidate"/row["path"]).read_bytes())==row["sha256"]
assert "RegularExpressions" not in (P/"overlay/Assessment/NativePattern.cs").read_text(encoding="utf-8")
identity=read(P/"IDENTITY.json")
for lane in ["minimum","current"]:
    f=next((P/lane/"results/final-accepted").glob("*.trx"));c=E.fromstring(f.read_bytes()).find(".//{*}Counters");assert int(c.attrib["passed"])==164 and int(c.attrib["failed"])==0
    for field,name in [("schemaCandidateDllSha256","Bosak.Schema.dll"),("probeDllSha256","Bosak.Schema.Tests.dll")]:assert sha((raw/lane/"bin/Release/net10.0"/name).read_bytes())==identity["lanes"][lane][field]
    packages=identity["lanes"][lane]["packages"];assert len(packages)==9 and all(x["signatureVerified"] for x in packages)
    oracle=read(P/lane/"oracle-counts.json");assert oracle["comparisons"]==37147 and oracle["patterns"]==307 and oracle["inputs"]==121
f=next((P/"baseline/results/qualified").glob("*.trx"));c=E.fromstring(f.read_bytes()).find(".//{*}Counters");assert int(c.attrib["passed"])==4 and int(c.attrib["failed"])==0
for label,folder,passed,failed in [("implementation","final-unreconciled",900,0),("public","final-unreconciled",1984,2),("public","reconciled",1986,0)]:
    f=next((P/"regression"/label/"results"/folder).glob("*.trx"));c=E.fromstring(f.read_bytes()).find(".//{*}Counters");assert int(c.attrib["passed"])==passed and int(c.attrib["failed"])==failed
admissions=read(P/"REGRESSION-ADMISSIONS.json");assert sha((ROOT/admissions["source"]).read_bytes())==admissions["sourceSha256"]
assert [x["id"] for x in admissions["rows"]]==["M2D1-66","M2D1-80"]
for row in admissions["rows"]:
    for field in ["id","schema","instance"]:assert row["before"][field]==row["after"][field]
    assert row["after"]["values"]==[dict(kind="String",value="abc")]
assert read(P/"STABLE-CASES.json")["allOriginalIdentitiesPreserved"] and read(P/"STABLE-CASES.json")["candidateCases"]==2886
api=read(P/"SURFACE.json");assert api["zeroPublicApiDelta"] and api["preservedRows"]==401 and api["currentPublicTypes"]==54
for row in read(P/"ORIGINAL-INPUTS.json"):
    for field in ["schema","instance"]:assert sha((ROOT/"docs/expanded-conformance/cohort2/upstream"/row[field]).read_bytes())==row[field+"Sha256"]
inputs=read(ROOT/"docs/structural-assertions/implementation-evidence/SOURCE-INPUTS.json")
for name,h in inputs.items():assert sha((ROOT/name).read_bytes())==h
assert not subprocess.check_output(["git","diff","HEAD","--name-only","--","src","tests","tools",".github","Directory.Build.props","global.json"],cwd=ROOT).strip()
assert not subprocess.check_output(["git","ls-files","--others","--exclude-standard","--","src","tests","tools",".github"],cwd=ROOT).strip()
seals=[s for s in (ROOT/"docs").rglob("PACKET-SHA256.json") if not s.is_relative_to(P)]
entries=0
for seal in seals:
    for name,h in read(seal).items():assert sha((seal.parent/name).read_bytes())==h,(seal,name);entries+=1
checks=dict(status="Passed native feasibility artifact/receipt verification",candidateFiles=58,sourceOverlays=6,nativeControlsPerCore=164,unchangedMinimumBaselineControls=4,oracleComparisonsPerCore=37147,implementationPassed=900,publicTruePass=1984,publicTrueFail=2,publicScratchReconciledPass=1986,stableCaseIdentities=2886,scratchAdmissions=["M2D1-66","M2D1-80"],originalPublicInputsMeasured=3,apiDelta=0,signedCorePackages=18,productionInputsUnchanged=len(inputs),priorSeals=len(seals),priorSealedEntries=entries,productionAdoption=False,fullFrozenOrExpandedRun=False,newPackageOrRetainedRun=False)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
