"""Validate the sealed post-anonymous coverage review without executing product code."""
from pathlib import Path
import collections
import hashlib
import json
import zipfile

D = Path(__file__).resolve().parent
R = D.parent.parent


def read(name):
    return json.loads((D / name).read_text(encoding="utf-8"))


def digest(raw):
    return hashlib.sha256(raw).hexdigest()


def normalized(raw, expected):
    lf = raw.replace(b"\r\n", b"\n")
    return expected in {digest(raw), digest(lf), digest(lf.replace(b"\n", b"\r\n"))}


for name, expected in read("PACKET-SHA256.json").items():
    assert digest((D / name).read_bytes()) == expected, name
inventory = read("CASE-INVENTORY.json")
assert len(inventory["rows"]) == 80
assert len({row["id"] for row in inventory["rows"]}) == 80
assert len({row["schemaSha256"] for row in inventory["rows"]}) == 65
for name, expected in {**inventory["inputs"], **inventory["manifests"]}.items():
    assert digest((R / name).read_bytes()) == expected, name
for name, expected in read("PROTECTED-INPUTS.json").items():
    assert normalized((R / name).read_bytes(), expected), name
for name, expected in read("HISTORICAL-PACKETS.json").items():
    assert digest((R / name).read_bytes()) == expected, name
basis = read("EVIDENCE-BASIS.json")
assert not basis["newSemanticAdoption"] and not basis["newPublication"]
assert not basis["reusedPriorQualification"]["sourceSuiteRerun"]
assert not basis["reusedPriorQualification"]["packageSuiteRerun"]
assert basis["reusedPriorQualification"]["source"] == 29374
assert basis["reusedPriorQualification"]["packageOverlapping"] == 27263
assert len(basis["core"]) == 9
runtime = read("RUNTIME-IDENTITIES.json")
assert runtime["schemaDllSha256"] == basis["schemaDllSha256"]
assert runtime["core"] == basis["core"]
assert runtime["caughtDiagnostic"] and not runtime["productModified"]
assert digest((D / "Diagnostic.cs.txt").read_bytes()) == runtime["diagnosticSourceSha256"]
assert digest((D / "Diagnostic.csproj.txt").read_bytes()) == runtime["diagnosticProjectSha256"]
assert digest((D / basis["baselineQualification"]).read_bytes()) == basis["baselineQualificationSha256"]
public = {row["id"]: row for row in read("PUBLIC-REFUSALS.json")["rows"]}
details = {row["id"]: row for row in read("DETAILED-REFUSALS.json")}
assert len(public) == len(details) == 80
assert collections.Counter(row["phase"] for row in details.values()) == {"compile": 79, "assessment": 1}
for row in inventory["rows"]:
    p, detail = public[row["id"]], details[row["id"]]
    assert p["error"] is None and detail["error"] is None
    assert p["compile"] == "Unsupported" or p["assessment"] == "Unsupported"
    assert detail["outcome"] == "Unsupported"
    assert p["reason"] == detail["reason"] == row["firstPublicReason"]
blockers = read("BLOCKERS.json")
assert {row["id"] for row in blockers["rows"]} == set(public)
counts = collections.Counter(row["primaryGroup"] for row in blockers["rows"])
assert len(counts) == 16 and sum(counts.values()) == 80
assert counts == {key: value["cases"] for key, value in blockers["groups"].items()}
assert all(not row["blockerReviewExhaustive"] for row in blockers["rows"])
ranking = read("RANKING.json")["ranking"]
assert [row["rank"] for row in ranking] == list(range(1, 7))
assert ranking[0]["group"] == "named-integer-chain"
assert all(not row["implementationAdopted"] for row in ranking)
composition = read("COMPOSITION-PREREQUISITES.json")
assert composition["cases"] == 11 and composition["distinctSchemas"] == 8
assert all(not row["routeExecuted"] for row in composition["rows"])
assert all(not ref["presentInFrozenDirectory"] for row in composition["rows"] for ref in row["references"])
matrix = read("CURRENT-MATRIX.json")
assert len(matrix["rows"]) == 28
assert matrix["normativeDenominator"] is matrix["conformancePercentage"] is None
assert not matrix["qualification"]["freshSourceSuiteRun"]
assert not matrix["qualification"]["freshPackageSuiteRun"]
assert matrix["qualification"]["freshSelectedRefusalRun"]
assert all(not row["fullyConformant"] and not row["normativeRequirementEnumerationComplete"] for row in matrix["rows"])
for row in matrix["rows"]:
    for name in row["evidence"]:
        assert name in matrix["evidence"], name
for name, evidence in matrix["evidence"].items():
    if evidence["repository"] == "schema":
        assert evidence["revision"] == inventory["productBase"]
        assert normalized((R / evidence["path"]).read_bytes(), evidence["sha256"]), name
    else:
        assert evidence["reviewRole"].startswith("Historical linked Core evidence")
assert read("REVIEW-PINS.json")["testFixtures"]["revision"] == "7bc3365c652a322f3d762021b3879eb92dae7e30"
with zipfile.ZipFile(D / "EXECUTION-EVIDENCE.zip") as evidence:
    for name, expected in read("EXECUTION-HASHES.json").items():
        assert digest(evidence.read(name)) == expected, name
assert read("DOCUMENTATION-CHECKS.json")["visualAcceptance"] is False
print("PASS: 80 unchanged refusals /65 schemas, 16 groups, 28 areas, pinned evidence and protected inputs")
