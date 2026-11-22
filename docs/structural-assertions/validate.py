"""Lint proposal inputs only; never compile Schema or evaluate assertions.
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
    read = lambda path: json.loads(path.read_text(encoding="utf-8-sig"))
    sha = lambda raw: hashlib.sha256(raw).hexdigest()
    plan = read(P / "CASE-PLAN.json")
    assert len(plan["rows"]) == 27 and len(plan["gates"]) == 18
    ids = [row["id"] for row in plan["rows"] + plan["gates"]]
    assert len(ids) == len(set(ids))
    for row in plan["rows"]:
        assert row["status"] == "Proposed authored payload; not executed"
        assert row["expectedCompile"] in {"Valid", "Invalid", "Unsupported"}
        ET.fromstring(row["schema"])
        ET.fromstring(row["instance"])
        if row["expectedCompile"] != "Valid":
            assert row["expectedAssessment"] is None
    source = read(P / "SOURCE-REVIEW.json")
    for row in source["files"]:
        assert sha((ROOT / row["path"]).read_bytes()) == row["worktreeSha256"]
        blob = subprocess.check_output(["git", "show", source["source"] + ":" + row["path"]], cwd=ROOT)
        assert sha(blob) == row["gitBlobSha256"]
    upstream = read(P / "UPSTREAM-REVIEW.json")
    manifest = read(ROOT / "docs/conformance-pilot/CASES.json")
    assert sha((ROOT / "docs/conformance-pilot/CASES.json").read_bytes()) == upstream["manifestSha256"]
    for name, expected in manifest["files"].items():
        assert sha((ROOT / "tests/TestData/ConformancePilot/upstream" / name).read_bytes()) == expected
    for name, expected in upstream["files"].items():
        assert sha((ROOT / "tests/TestData/ConformancePilot/upstream" / name).read_bytes()) == expected
    for row in read(P / "SPECIFICATION-REVIEW.json")["sources"]:
        assert sha(Path(row["reviewBodyPath"]).read_bytes()) == row["sha256"]
    for row in read(P / "CORE-CONTRACT-REVIEW.json")["pins"]:
        assert sha(Path(row["xmlPath"]).read_bytes()) == row["xmlSha256"]
        assert len(row["members"]) == 4
    seals = ["post-xsi-type", "xsi-type-contract", "xsi-type-contract/feasibility-evidence",
             "xsi-type-contract/implementation/evidence", "namespace-form/feasibility-evidence",
             "namespace-form/evidence", "namespace-form-review", "complex-simple-restriction",
             "complex-simple-restriction/feasibility-evidence", "complex-simple-restriction/implementation-evidence",
             "integer-simple-content", "integer-simple-content/feasibility-evidence",
             "integer-simple-content/implementation-evidence",
             "contextual-assertions", "contextual-assertions/feasibility-evidence",
             "contextual-assertions/implementation-evidence"]
    count = 0
    for name in seals:
        directory = ROOT / "docs" / name
        for relative, expected in read(directory / "PACKET-SHA256.json").items():
            assert sha((directory / relative).read_bytes()) == expected, (name, relative)
            count += 1
    protected = ["src", "tests", "tools", ".github", "Directory.Build.props", "global.json"]
    assert not subprocess.check_output(["git", "diff", "HEAD", "--name-only", "--", *protected], cwd=ROOT).strip()
    assert not subprocess.check_output(["git", "ls-files", "--others", "--exclude-standard", "--", *protected], cwd=ROOT).strip()
    result = dict(status="Passed proposal artifact lint only", proposedPayloads=27,
                  proposedGates=18, wellFormedXmlDocuments=54, sourceFilesVerified=9,
                  frozenCorpusFilesVerified=len(manifest["files"]),
                  focusedUpstreamFilesVerified=len(upstream["files"]),
                  priorSealsVerified=len(seals), priorSealedEntriesVerified=count,
                  semanticAdoption=False, engineExecuted=False, newPilotExecuted=False,
                  productionInputsUnchanged=True)
    (P / "VALIDATION.json").write_text(json.dumps(result, indent=2) + "\n", encoding="utf-8")
    print("Artifact lint passed:27payloads/18gates; no assertion or Schema execution.")


if __name__ == "__main__":
    main()
