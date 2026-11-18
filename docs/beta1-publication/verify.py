"""Verify recorded publication and representative-consumer evidence offline."""
# Copyright 2026 Fytala. SPDX-License-Identifier: Apache-2.0
import hashlib
import json
from pathlib import Path
import zipfile

HERE = Path(__file__).resolve().parent
ROOT = HERE.parents[1]


def digest(raw):
    return hashlib.sha256(raw).hexdigest()


def main():
    manifest = json.loads((HERE / "MANIFEST.json").read_text(encoding="utf-8"))
    for name, expected in manifest["sourceLfSha256"].items():
        assert digest((ROOT / name).read_bytes().replace(b"\r\n", b"\n")) == expected, name
    archive = HERE / "SOURCE-ONLY-EVIDENCE.zip"
    assert digest(archive.read_bytes()) == manifest["archiveSha256"]
    with zipfile.ZipFile(archive) as bundle:
        assert len(bundle.namelist()) == len(set(bundle.namelist()))
        assert set(bundle.namelist()) == set(manifest["archiveEntries"])
        for name, expected in manifest["archiveEntries"].items():
            assert not name.startswith("/") and ".." not in Path(name).parts
            assert Path(name).suffix.lower() in {".cs", ".csproj", ".config", ".xml", ".xsd", ".txt", ".md", ".json", ".log", ".trx"}
            assert digest(bundle.read(name)) == expected, name
    receipt = json.loads((HERE / "RECEIPT.json").read_text())
    assert receipt["published"] and receipt["version"] == "0.5.0-beta.1"
    assert receipt["publishRun"] == 44245557198 and receipt["preparationRun"] == 44244606595
    assert receipt["allApprovedEntriesByteIdentical"] and receipt["repositorySignatureVerified"]
    assert receipt["registryOnlyConsumerCases"] == 33 and receipt["sourceCases"] == 345
    assert receipt["githubAssetDigests"]["Bosak.Schema.0.5.0-beta.1.nupkg"] == "sha256:" + receipt["signedPackageSha256"]
    trial = json.loads((HERE / "INVOICE-TRIAL.json").read_text())
    assert trial["result"]["passed"] and not trial["productionAcceptance"] and not trial["newEngineDefectObserved"]
    assert trial["identity"]["packageOnly"] and trial["identity"]["packageSha256"] == receipt["signedPackageSha256"]
    assert len(trial["identity"]["core"]) == 9 and all(row["signatureExitCode"] == 0 and row["identical"] for row in trial["identity"]["core"])
    print("PASS: documentation hashes, retained publication/trial evidence, signed package identity and bounded consumer scope. Offline verification does not republish or rerun consumers.")


if __name__ == "__main__":
    main()
