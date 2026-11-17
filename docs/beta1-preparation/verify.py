"""Verify retained preparation evidence offline; no launch or publication authority."""
# Copyright 2026 Fytala. SPDX-License-Identifier: Apache-2.0
import hashlib
import json
from pathlib import Path
import zipfile

HERE = Path(__file__).resolve().parent
ROOT = HERE.parents[1]


def sha(data):
    return hashlib.sha256(data).hexdigest()


def main():
    manifest = json.loads((HERE / "MANIFEST.json").read_text(encoding="utf-8"))
    assert manifest["sourceCommit"] == "2a5abf6fb663313dc32bfeca5bbb8740bbe987ae"
    for name, expected in manifest["sourceLfSha256"].items():
        assert sha((ROOT / name).read_bytes().replace(b"\r\n", b"\n")) == expected, name
    archive = HERE / "SOURCE-ONLY-EVIDENCE.zip"
    assert sha(archive.read_bytes()) == manifest["archiveSha256"]
    with zipfile.ZipFile(archive) as bundle:
        assert len(bundle.namelist()) == len(set(bundle.namelist()))
        assert set(bundle.namelist()) == set(manifest["archiveEntries"])
        for name, expected in manifest["archiveEntries"].items():
            assert not name.startswith("/") and ".." not in Path(name).parts
            assert Path(name).suffix.lower() in {".py", ".cjs", ".yml", ".json", ".log", ".trx"}
            assert sha(bundle.read(name)) == expected, name
    receipt = json.loads((HERE / "LOCAL-ARTIFACT.json").read_text())
    release = json.loads((HERE / "LOCAL-RELEASE-MANIFEST.json").read_text())
    assert sha((HERE / "LOCAL-RELEASE-MANIFEST.json").read_bytes()) == receipt["manifestSha256"]
    assert release["package"] == receipt["package"]
    assert release["sourceCommit"] == receipt["source"] == manifest["sourceCommit"]
    assert release["version"] == "0.5.0-beta.1" and release["sourceTag"] == receipt["tag"] == "v0.5.0-beta.1"
    assert release["controllerCommit"] == "0" * 40
    assert receipt["localOnly"] and not receipt["hostedPreparation"] and not receipt["publicationAuthorized"] and not receipt["remoteTagPushed"]
    q = release["qualification"]
    assert (q["sourceCases"], q["packageCases"], q["legacyCasesPerLane"], q["publishedAlphaCasesPerLane"]) == (345, 33, 124, 23)
    print("PASS: source/document hashes, source-only archive, local beta identity and qualification scope. No hosted preparation/publication inferred.")


if __name__ == "__main__":
    main()
