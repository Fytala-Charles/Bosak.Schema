"""Reconstruct isolated source/probe inputs; no network, build, launch or delete.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import argparse
import hashlib
import json
import subprocess

E = Path(__file__).resolve().parent


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--repository", required=True, type=Path)
    parser.add_argument("--output", required=True, type=Path)
    args = parser.parse_args()
    repo, out = args.repository.resolve(), args.output.resolve()
    sha = lambda value: hashlib.sha256(value).hexdigest()
    read = lambda path: json.loads(path.read_text(encoding="utf-8-sig"))
    baseline = read(E / "BASE-SOURCE.json")
    git = ["git", "-c", "safe.directory=" + repo.as_posix()]
    assert subprocess.check_output(git + ["rev-parse", "HEAD"], cwd=repo).decode().strip() == baseline["base"]
    protected = ["src", "tests", "tools", ".github", "Directory.Build.props", "global.json"]
    assert not subprocess.check_output(git + ["diff", "HEAD", "--name-only", "--", *protected], cwd=repo).strip()
    assert not subprocess.check_output(git + ["ls-files", "--others", "--exclude-standard", "--", *protected], cwd=repo).strip()
    assert not out.exists(), "Use a new output directory; nothing is deleted or overwritten."
    payloads = {}
    for name, record in baseline["files"].items():
        rel = Path(name)
        assert not rel.is_absolute() and ".." not in rel.parts
        raw = subprocess.check_output(git + ["show", baseline["base"] + ":src/Bosak.Schema/" + rel.as_posix()], cwd=repo)
        assert sha(raw) == record["sha256"], name
        payloads[name] = raw
    candidate = read(E / "CANDIDATE-SOURCE.json")
    overlays = {name: (E / "candidate" / name).read_bytes() for name in candidate["modifiedOrAdded"]}
    for name, raw in overlays.items():
        assert sha(raw) == candidate["files"][name]
    out.mkdir(parents=True)
    for tree in ["original", "candidate"]:
        for name, raw in payloads.items():
            target = out / tree / name
            target.parent.mkdir(parents=True, exist_ok=True)
            target.write_bytes(overlays.get(name, raw) if tree == "candidate" else raw)
    for name, expected in candidate["files"].items():
        assert sha((out / "candidate" / name).read_bytes()) == expected
    names = ["ComplexTests.cs", "BaselineTests.cs", "Fixtures.cs", "Directory.Build.props",
             "NuGet.Config", "case-plan.json", "facet-rules.json", "verify-identity.py"]
    names += [p.relative_to(E).as_posix() for folder in ["baseline", "minimum", "current", "regression", "surface", "regression-overrides"]
              for p in (E / folder).rglob("*") if p.is_file()]
    old_scratch = "D:/Development/Bosak/.guard-tmp/schema-integer-simple-content-feasibility-20261121"
    old_repo = "D:/Development/Bosak/.worktrees/Schema.IntegerSimpleContent"
    for name in names:
        raw = (E / name).read_bytes()
        if name.endswith(".csproj"):
            raw = raw.decode("utf-8-sig").replace(old_scratch, out.as_posix()).replace(old_repo, repo.as_posix()).encode("utf-8")
        target = out / name
        target.parent.mkdir(parents=True, exist_ok=True)
        target.write_bytes(raw)
    print("Reconstructed 54 baseline/54 candidate files with three hash-checked overlays and rebound probes; no engine executed.")


if __name__ == "__main__":
    main()
