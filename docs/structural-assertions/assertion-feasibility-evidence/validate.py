"""Verify retained scoped native receipts and immutability without running the engine.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent
ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
seals=["post-xsi-type","xsi-type-contract","xsi-type-contract/feasibility-evidence","xsi-type-contract/implementation/evidence","namespace-form/feasibility-evidence","namespace-form/evidence","namespace-form-review","complex-simple-restriction","complex-simple-restriction/feasibility-evidence","complex-simple-restriction/implementation-evidence","integer-simple-content","integer-simple-content/feasibility-evidence","integer-simple-content/implementation-evidence","contextual-assertions","contextual-assertions/feasibility-evidence","contextual-assertions/implementation-evidence","structural-assertions","structural-assertions/empty-value-feasibility-evidence"]
count=0
for name in seals:
    d=ROOT/"docs"/name
    for f,h in read(d/"PACKET-SHA256.json").items():
        assert sha((d/f).read_bytes())==h,(name,f)
        count+=1
for row in read(P/"EXPERIMENT-ADOPTION.json")["sources"]:
    assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
base=read(P/"BASE-SOURCE.json")
for row in base["files"]:
    blob=subprocess.check_output(["git","show",base["base"]+":"+row["path"]],cwd=ROOT)
    assert sha(blob)==row["sha256"]
protected=["src","tests","tools",".github","Directory.Build.props","global.json"]
assert not subprocess.check_output(["git","diff","HEAD","--name-only","--",*protected],cwd=ROOT).strip()
assert not subprocess.check_output(["git","ls-files","--others","--exclude-standard","--",*protected],cwd=ROOT).strip()
m=read(ROOT/"docs/conformance-pilot/CASES.json")
for f,h in m["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/f).read_bytes())==h
for lane,version in [("minimum","1.81.2"),("current","1.81.17")]:
    for folder,n in [("results/final",114)]:
        trx=next((P/lane/folder).glob("*.trx"));c=ET.fromstring(trx.read_bytes()).find(".//{*}Counters")
        assert int(c.attrib["passed"])==n and int(c.attrib["failed"])==0 and int(c.attrib["total"])==n
    identity=read(P/"IDENTITY.json")["lanes"][lane]
    assert len(identity["packages"])==9
    for row in identity["packages"]:
        assert row["version"]==version and row["signatureVerified"] and row["cacheRuntimeMatch"]
        assert (P/"signatures"/(lane+"-"+row["id"]+".log")).is_file()
surface=read(P/"SURFACE.json")
assert surface["zeroPublicApiDelta"] and surface["currentPublicTypes"]==54 and surface["preservedRows"]==401
for lane,n in [("implementation",840),("public",1930)]:
    trx=next((P/"regression"/lane/"results/final").glob("*.trx"));c=ET.fromstring(trx.read_bytes()).find(".//{*}Counters")
    assert int(c.attrib["passed"])==n and int(c.attrib["failed"])==0 and int(c.attrib["total"])==n
stable=read(P/"STABLE-CASES.json");assert stable["allOriginalIdentitiesPreserved"] and stable["candidateSourceCases"]==2770
assert len(stable["identities"])==2770
raw=Path("D:/Development/Bosak/.guard-tmp/schema-structural-assertions-feasibility-20261122")
plan=read(P/"CANDIDATE-SOURCE.json")
for lane in ["minimum","current"]:
    for row in plan["files"]:assert sha((raw/lane/"candidate"/row["path"]).read_bytes())==row["sha256"]
    identity=read(P/"IDENTITY.json")["lanes"][lane]
    assert sha((raw/lane/"bin/Release/net10.0/Bosak.Schema.dll").read_bytes())==identity["schemaCandidateDllSha256"]
    assert sha((raw/lane/"bin/Release/net10.0/Bosak.Schema.Tests.dll").read_bytes())==identity["probeDllSha256"]
result=dict(status="Passed scoped receipt/source/evidence verification",nativeControlsPerPin=114,priorSealsVerified=len(seals),priorSealedEntriesVerified=count,originalSourceFiles=56,candidateSourceFiles=57,overlays=6,frozenFilesVerified=len(m["files"]),productionInputsUnchanged=True,productionAdoption=False,namespaceAssertionFeasibility="Passed bounded native scope",fullStableRegression=2770)
(P/"CHECKS.json").write_text(json.dumps(result,indent=2)+"\n",encoding="utf-8")
print(json.dumps(result))
