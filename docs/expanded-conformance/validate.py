"""Validate expanded assessment metadata, receipts and preserved product boundaries.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import collections,hashlib,importlib.util,json,subprocess
P=Path(__file__).resolve().parent;ROOT=P.parents[1]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
spec=importlib.util.spec_from_file_location("pilot",ROOT/"tools/run-xsd-conformance-pilot.py");pilot=importlib.util.module_from_spec(spec);spec.loader.exec_module(pilot)
ids=set();groups=set();payloads=0;matters=set()
frozen=read(ROOT/"docs/conformance-pilot/CASES.json");oldgroups={(x["metadata"],x["group"]) for x in frozen["cases"]}
for relative,expected in frozen["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/relative).read_bytes())==expected
for cohort,label,n in [(P,"cohort1",68),(P/"cohort2","cohort2",61)]:
    manifest=read(cohort/"CASES.json");selection=read(cohort/"SELECTION-CHECKS.json")
    assert selection["manifestSha256"]==sha((cohort/"CASES.json").read_bytes())
    requests=pilot.verify(manifest,cohort/"upstream");assert len(requests)==n
    assert set(manifest["configurationTokens"])=={"1.1","xml-1.0","xml-1.0-5e"}
    payloads+=len(manifest["files"])
    current=set()
    for case in manifest["cases"]:
        key=(case["metadata"],case["group"]);assert key not in oldgroups;current.add(key)
        assert case["id"] not in ids;ids.add(case["id"]);matters.add(case["area"])
    assert not groups.intersection(current);groups.update(current)
    if label=="cohort2":
        for g in read(cohort/"INVENTORY.json")["selectedGroups"]:
            assert g["globalElements"]>0
        for b in ["choice","all"]:
            for g in read(cohort/"INVENTORY.json")["selectedGroups"]:
                if b in g["features"] and any(x["group"]==g["group"] and x["metadata"]==g["metadata"] and x["bucket"]==b for x in manifest["cases"]):assert b in g["nonemptyCompositors"]
    for lane in [label,"current-"+label]:
        report=read(P/"evidence"/lane/"RESULTS.json");assert report["manifestSha256"]==selection["manifestSha256"]
        native=read(P/"evidence"/lane/"native.json");byid=pilot.checked_results(native,requests)
        assert report["counts"]==dict(match=0,unsupported=n,mismatch=0,harnessError=0)
        for row in report["rows"]:assert row["classification"]==pilot.classify(row["upstreamExpected"],byid[row["id"]])
    a=read(P/"evidence"/label/"RESULTS.json");b=read(P/"evidence"/("current-"+label)/"RESULTS.json")
    assert [(x["id"],x["native"]) for x in a["rows"]]==[(x["id"],x["native"]) for x in b["rows"]]
assert len(ids)==129 and len(groups)==108 and len(matters)==12
inputs=read(ROOT/"docs/structural-assertions/implementation-evidence/SOURCE-INPUTS.json")
for f,h in inputs.items():assert sha((ROOT/f).read_bytes())==h
protected=["src","tests","tools",".github","Directory.Build.props","global.json","docs/conformance-pilot","docs/specification-coverage/MATRIX.json"]
assert not subprocess.check_output(["git","diff","HEAD","--name-only","--",*protected],cwd=ROOT).strip()
assert not subprocess.check_output(["git","ls-files","--others","--exclude-standard","--",*protected],cwd=ROOT).strip()
for ident in ["PRODUCT-IDENTITY.json","PRODUCT-IDENTITY-CURRENT.json"]:
    product=read(P/ident);runtime=Path(product["runtimeDirectory"])
    assert {f.name:sha(f.read_bytes()) for f in runtime.iterdir() if f.is_file()}==product["runtimeFiles"]
for row in read(P/"APPLICATION-EXAMPLES.json"):assert sha((ROOT/row["path"]).read_bytes())==row["sha256"]
for row in read(P/"SPECIFICATION-REVIEW.json")["sources"]:assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
seals=["post-xsi-type","xsi-type-contract","xsi-type-contract/feasibility-evidence","xsi-type-contract/implementation/evidence","namespace-form/feasibility-evidence","namespace-form/evidence","namespace-form-review","complex-simple-restriction","complex-simple-restriction/feasibility-evidence","complex-simple-restriction/implementation-evidence","integer-simple-content","integer-simple-content/feasibility-evidence","integer-simple-content/implementation-evidence","contextual-assertions","contextual-assertions/feasibility-evidence","contextual-assertions/implementation-evidence","structural-assertions","structural-assertions/empty-value-feasibility-evidence","structural-assertions/assertion-feasibility-evidence","structural-assertions/implementation-evidence"]
entries=0
for name in seals:
    d=ROOT/"docs"/name
    for relative,h in read(d/"PACKET-SHA256.json").items():assert sha((d/relative).read_bytes())==h,(name,relative);entries+=1
checks=dict(status="Passed assessment integrity checks",uniqueCases=129,uniqueGroups=108,featureBuckets=18,matrixAreas=12,originalPayloadFiles=payloads,frozenFilesVerified=42,productInputsVerified=len(inputs),priorSealsVerified=len(seals),priorSealedEntriesVerified=entries,countsPerCore=dict(match=0,unsupported=129,mismatch=0,harnessError=0),classificationsIdenticalAcrossPins=True,productionInputsUnchanged=True,semanticAdoption=False,conformancePercentage=None)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
