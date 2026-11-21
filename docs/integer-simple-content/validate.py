"""Lint proposed artifacts only, without compiling schemas or executing an engine.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import hashlib
import json
import subprocess
import xml.etree.ElementTree as ET

P = Path(__file__).resolve().parent
ROOT = P.parent.parent


def main():
    sha = lambda value: hashlib.sha256(value).hexdigest()
    read = lambda path: json.loads(path.read_text(encoding="utf-8-sig"))
    plan = read(P / "CASE-PLAN.json")
    assert len(plan["rows"]) == 60
    assert len(plan["boundRelations"]) == 48
    assert len(plan["gates"]) == 22
    ids = [row["id"] for row in plan["rows"] + plan["gates"]]
    assert len(ids) == len(set(ids))
    for row in plan["rows"]:
        assert row["status"] == "Proposed authored payload; not executed"
        assert row["expectedCompile"] in {"Valid", "Invalid", "Unsupported"}
        ET.fromstring(row["schema"])
        ET.fromstring(row["instance"])
        if row["expectedCompile"] != "Valid":
            assert row["expectedAssessment"] is None
    rules = read(P / "FACET-RULES.json")
    assert len(rules["baseBoundConditions"]) == 16
    assert len(rules["sameDatatypePairs"]) == 4
    operators = {"<": lambda a, b: a < b, "<=": lambda a, b: a <= b,
                 ">": lambda a, b: a > b, ">=": lambda a, b: a >= b}
    for condition in rules["baseBoundConditions"]:
        rows = [r for r in plan["boundRelations"]
                if r["newFacet"] == condition["newFacet"] and r["baseFacet"] == condition["baseFacet"]]
        assert len(rows) == 3
        assert {int(r["newValue"]) - int(r["baseValue"]) for r in rows} == {-1, 0, 1}
        for row in rows:
            valid = operators[condition["requiredOperator"]](int(row["newValue"]), int(row["baseValue"]))
            assert (row["expectedCompile"] == "Valid") == valid
    source = read(P / "SOURCE-REVIEW.json")
    for row in source["files"]:
        assert sha((ROOT / row["path"]).read_bytes()) == row["worktreeSha256"]
        blob = subprocess.check_output(["git", "show", source["source"] + ":" + row["path"]], cwd=ROOT)
        assert sha(blob) == row["gitBlobSha256"]
    upstream = read(P / "UPSTREAM-REVIEW.json")
    assert sha((ROOT / "docs/conformance-pilot/CASES.json").read_bytes()) == upstream["manifestSha256"]
    for name, expected in upstream["files"].items():
        assert sha((ROOT / "tests/TestData/ConformancePilot/upstream" / name).read_bytes()) == expected
    manifest = read(ROOT / "docs/conformance-pilot/CASES.json")
    selected_names = {row[key] for row in manifest["cases"] for key in ["metadata", "schema", "instance"]}
    frozen_files = [ROOT / "tests/TestData/ConformancePilot/upstream" / name for name in selected_names]
    assert len(frozen_files) == 42
    for path in frozen_files:
        relative = path.relative_to(ROOT).as_posix()
        blob = subprocess.check_output(["git", "show", source["source"] + ":" + relative], cwd=ROOT)
        assert path.read_bytes() == blob
    seals = ["post-xsi-type", "xsi-type-contract", "xsi-type-contract/feasibility-evidence",
             "xsi-type-contract/implementation/evidence", "namespace-form/feasibility-evidence",
             "namespace-form/evidence", "namespace-form-review", "complex-simple-restriction",
             "complex-simple-restriction/feasibility-evidence",
             "complex-simple-restriction/implementation-evidence"]
    entries = 0
    for name in seals:
        directory = ROOT / "docs" / name
        for filename, expected in read(directory / "PACKET-SHA256.json").items():
            assert sha((directory / filename).read_bytes()) == expected, (name, filename)
            entries += 1
    protected = ["src", "tests", "tools", ".github", "Directory.Build.props", "global.json"]
    assert not subprocess.check_output(["git", "diff", "HEAD", "--name-only", "--", *protected], cwd=ROOT).strip()
    assert not subprocess.check_output(["git", "ls-files", "--others", "--exclude-standard", "--", *protected], cwd=ROOT).strip()
    result = dict(status="Passed artifact lint only", proposedPayloads=60,
                  proposedBoundComparisons=48, proposedSeparateGates=22,
                  wellFormedXmlDocuments=120, sourceReviewFilesVerified=len(source["files"]),
                  reviewedUpstreamFilesVerified=len(upstream["files"]), frozenFilesUnchanged=42,
                  priorSealsVerified=len(seals), priorSealedEntriesVerified=entries,
                  productionInputsUnchanged=True, engineExecuted=False, newPilotExecuted=False,
                  semanticAdoption=False)
    (P / "VALIDATION.json").write_text(json.dumps(result, indent=2) + "\n", encoding="utf-8")
    print("Artifact lint passed: 60 payloads / 48 bound comparisons / 22 gates; no engine execution.")


if __name__ == "__main__":
    main()
