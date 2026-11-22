"""Verify retained scoped native receipts and immutability without running the engine.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent
ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
seals=["post-xsi-type","xsi-type-contract","xsi-type-contract/feasibility-evidence","xsi-type-contract/implementation/evidence","namespace-form/feasibility-evidence","namespace-form/evidence","namespace-form-review","complex-simple-restriction","complex-simple-restriction/feasibility-evidence","complex-simple-restriction/implementation-evidence","integer-simple-content","integer-simple-content/feasibility-evidence","integer-simple-content/implementation-evidence","contextual-assertions","contextual-assertions/feasibility-evidence","contextual-assertions/implementation-evidence","structural-assertions"]
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
    for folder,n in [("baseline-final",4),("empty-accepted",29)]:
        trx=next((P/lane/folder).glob("*.trx"));c=ET.fromstring(trx.read_bytes()).find(".//{*}Counters")
        assert int(c.attrib["passed"])==n and int(c.attrib["failed"])==0 and int(c.attrib["total"])==n
    identity=read(P/"IDENTITY.json")["lanes"][lane]
    assert len(identity["packages"])==9
    for row in identity["packages"]:
        assert row["version"]==version and row["signatureVerified"] and row["cacheRuntimeMatch"]
        assert (P/"signatures"/(lane+"-"+row["id"]+".log")).is_file()
result=dict(status="Passed scoped receipt/source/evidence verification",baselineControlsPerPin=4,nativeControlsPerPin=29,priorSealsVerified=len(seals),priorSealedEntriesVerified=count,originalSourceFiles=56,overlays=5,frozenFilesVerified=len(m["files"]),productionInputsUnchanged=True,productionAdoption=False,namespaceAssertionFeasibility="Pending",fullStableRegression="Not run at this first gate")
(P/"CHECKS.json").write_text(json.dumps(result,indent=2)+"\n",encoding="utf-8")
print(json.dumps(result))
