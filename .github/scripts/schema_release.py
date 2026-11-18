"""Prepare and verify one reviewed Schema prerelease; never log in or push a package."""
# Copyright 2026 Fytala. SPDX-License-Identifier: Apache-2.0
import argparse
import hashlib
import json
import re
import shutil
import subprocess
from pathlib import Path, PurePosixPath
from urllib.error import HTTPError
from urllib.request import urlopen
import xml.etree.ElementTree as ET
import zipfile

REPOSITORY = "Fytala-Charles/Bosak.Schema"
LEGACY = "c953ebe0057bea1616d3e4f891f7b0a5fc4bd99b"
ALPHA_SOURCE = "1783d1a99b37e206b10592c24b3a5fa3ac126969"
ALPHA_VERSION = "0.5.0-alpha.0.7"
ALPHA_SHA256 = "6a11b8980b6f7cca68beec160845bff83edf317b51e27fcfd06775da22f48722"
SOURCE = "2a5abf6fb663313dc32bfeca5bbb8740bbe987ae"
VERSION = "0.5.0-beta.1"
TAG = "v" + VERSION
DEPENDENCIES = {"Bosak.XPath.Api", "Bosak.XPath.Providers", "Bosak.Xslt", "Bosak.XQuery"}
CORE_ASSEMBLIES = DEPENDENCIES | {"Bosak.XPath.Core", "Bosak.XPath.Parser", "Bosak.XPath.Compiler", "Bosak.XPath.Runtime", "Bosak.XPath.Standard"}
TRX = {"t": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}


def require(condition, message):
    if not condition:
        raise ValueError(message)


def digest(data):
    return hashlib.sha256(data).hexdigest()


def read_json(path):
    return json.loads(Path(path).read_text(encoding="utf-8-sig"))


def save(path, value):
    Path(path).write_text(json.dumps(value, sort_keys=True, indent=2) + "\n", encoding="utf-8")


def source_identity(root, revision):
    require(re.fullmatch(r"[0-9a-f]{40}", revision) is not None, "Immutable source commit required")
    require(revision == SOURCE, "This release controller only admits the reviewed source")
    git = lambda *args: subprocess.check_output(["git", "-C", str(root), *args], text=True).strip()
    require(git("rev-parse", "HEAD") == revision, "Source checkout mismatch")
    require(not git("status", "--porcelain"), "Source checkout must be clean")
    remote = git("remote", "get-url", "origin").removesuffix(".git").rstrip("/")
    require(remote.lower() == "https://github.com/" + REPOSITORY.lower(), "Wrong source repository")
    subprocess.run(["git", "-C", str(root), "merge-base", "--is-ancestor", revision, "origin/main"], check=True)
    require(git("rev-parse", "refs/tags/" + TAG + "^{commit}") == revision, "Reviewed beta tag mismatch")
    return git("rev-parse", "HEAD^{tree}")


def package_info(path, root, revision, version):
    require(version == VERSION, "Only the reviewed prerelease is admitted")
    require(path.is_file() and not path.is_symlink() and path.stat().st_size <= 4 * 1024 * 1024, "Invalid package file")
    require(path.name == f"Bosak.Schema.{version}.nupkg", "Unexpected package name")
    with zipfile.ZipFile(path) as archive:
        entries = archive.infolist()
        names = [entry.filename for entry in entries]
        require(len(names) == len(set(names)) and len(names) <= 32, "Duplicate or excessive package entries")
        require(sum(entry.file_size for entry in entries) <= 8 * 1024 * 1024, "Expanded package exceeds bound")
        for name in names:
            leaf = PurePosixPath(name)
            require(not leaf.is_absolute() and ".." not in leaf.parts and "\\" not in name and ":" not in name, "Unsafe entry")
        nuspecs = [name for name in names if name.endswith(".nuspec")]
        require(len(nuspecs) == 1, "Exactly one nuspec required")
        raw = archive.read(nuspecs[0])
        require(b"<!DOCTYPE" not in raw.upper() and b"<!ENTITY" not in raw.upper(), "Nuspec DTD refused")
        spec = ET.fromstring(raw)
        ns = {"n": spec.tag.split("}")[0][1:]}
        metadata = spec.find("n:metadata", ns)
        require(metadata is not None, "Missing metadata")
        field = lambda name: metadata.findtext("n:" + name, namespaces=ns)
        require(field("id") == "Bosak.Schema" and field("version") == version, "Package identity mismatch")
        repo = metadata.find("n:repository", ns)
        require(repo is not None and repo.attrib.get("commit") == revision and repo.attrib.get("url", "").removesuffix(".git").lower() == "https://github.com/" + REPOSITORY.lower(), "Source metadata mismatch")
        license_node = metadata.find("n:license", ns)
        require(license_node is not None and license_node.text == "Apache-2.0" and license_node.attrib.get("type") == "expression", "Wrong license")
        require(field("readme") == "NUGET_README.md", "Wrong package README")
        groups = metadata.findall("n:dependencies/n:group", ns)
        require(len(groups) == 1 and groups[0].attrib.get("targetFramework") == "net10.0", "Wrong framework")
        dependencies = groups[0].findall("n:dependency", ns)
        require(len(dependencies) == 4 and {d.attrib.get("id") for d in dependencies} == DEPENDENCIES and all(d.attrib.get("version") == "1.81.2" for d in dependencies), "Dependency change refused")
        require([n for n in names if n.endswith(".dll")] == ["lib/net10.0/Bosak.Schema.dll"], "Extra or missing DLL")
        allowed = {"Bosak.Schema.nuspec", "_rels/.rels", "[Content_Types].xml", "LICENSE.md", "NOTICE", "NUGET_README.md", "lib/net10.0/Bosak.Schema.dll", "lib/net10.0/Bosak.Schema.xml", "package/services/metadata/core-properties/nuget.psmdcp"}
        unexpected = [n for n in names if n not in allowed and not re.fullmatch(r"package/services/metadata/core-properties/[0-9a-f]+\.psmdcp", n)]
        require(not unexpected, "Unexpected package content: " + repr(unexpected))
        for name, source in [("LICENSE.md", "LICENSE.md"), ("NOTICE", "NOTICE"), ("NUGET_README.md", "docs/NUGET_README.md")]:
            require(archive.read(name).replace(b"\r\n", b"\n") == (root / source).read_bytes().replace(b"\r\n", b"\n"), "Source/legal/README mismatch")
        require("lib/net10.0/Bosak.Schema.xml" in names, "XML API documentation missing")
        return {"file": path.name, "bytes": path.stat().st_size, "sha256": digest(path.read_bytes()), "entries": {n: digest(archive.read(n)) for n in names}}


def case_catalog(folder):
    catalogs = []
    for path in sorted(folder.glob("*.trx")):
        tree = ET.parse(path)
        rows = sorted((r.attrib["testName"], r.attrib["outcome"]) for r in tree.findall(".//t:UnitTestResult", TRX))
        counts = tree.find("t:ResultSummary/t:Counters", TRX)
        require(counts is not None and len(rows) == int(counts.attrib["total"]) and all(outcome == "Passed" for _, outcome in rows), "Incomplete or failed TRX")
        catalogs.append(rows)
    return catalogs


def seal(directory, root, revision, version, controller, results):
    tree = source_identity(root, revision)
    require(re.fullmatch(r"[0-9a-f]{40}", controller) is not None, "Controller commit required")
    packages = sorted(directory.glob("*.nupkg"))
    require(len(packages) == 1, "Exactly one package required")
    package = package_info(packages[0], root, revision, version)
    source_cases = case_catalog(results / "source")
    consumer_cases = case_catalog(results / "consumer")
    require(sorted(map(len, source_cases)) == [33, 312] and len(consumer_cases) == 1 and len(consumer_cases[0]) == 33, "Wrong qualification catalogs")
    require(consumer_cases[0] == next(rows for rows in source_cases if len(rows) == 33), "Package consumer catalog drift")
    legacy = read_json(results / "legacy.json")
    require(legacy.get("baselineCommit") == LEGACY and legacy.get("count") == 124 and legacy.get("onlySchemaDllChanged") is True and legacy.get("sameCases") is True, "Legacy lane incomplete")
    legacy_control = case_catalog(results / "legacy-control")
    legacy_candidate = case_catalog(results / "legacy-candidate")
    require(len(legacy_control) == len(legacy_candidate) == 1 and len(legacy_control[0]) == 124 and legacy_control == legacy_candidate, "Legacy TRX drift")
    published = read_json(results / "published.json")
    require(published.get("baselineCommit") == ALPHA_SOURCE and published.get("count") == 23 and published.get("onlySchemaDllChanged") is True and published.get("sameCases") is True, "Published alpha lane incomplete")
    a, b = case_catalog(results / "published-control"), case_catalog(results / "published-candidate")
    require(len(a) == len(b) == 1 and len(a[0]) == 23 and a == b, "Published alpha TRX drift")
    alpha = read_json(results / "published-identity.json")
    require(alpha.get("packageSha256") == ALPHA_SHA256 and alpha.get("packageOnly") is True and alpha.get("schemaSignatureExitCode") == 0 and len(alpha.get("core", [])) == 9 and {d["id"] for d in alpha["core"]} == CORE_ASSEMBLIES and all(d["version"] == "1.81.2" and d["identical"] and d["signatureExitCode"] == 0 for d in alpha["core"]), "Published alpha identity incomplete")
    identity = read_json(results / "consumer-identity.json")
    require(identity.get("packageSha256") == package["sha256"] and identity.get("packageOnly") is True and len(identity.get("core", [])) == 9 and {d["id"] for d in identity["core"]} == CORE_ASSEMBLIES and all(d["version"] == "1.81.2" and d["identical"] and d["signatureExitCode"] == 0 for d in identity["core"]), "Package/dependency identity incomplete")
    manifest = {"schemaVersion": 1, "repository": REPOSITORY, "sourceCommit": revision, "sourceTree": tree, "sourceTag": TAG, "controllerCommit": controller, "version": version, "package": package, "qualification": {"sourceCases": 345, "packageCases": 33, "legacyCasesPerLane": 124, "publishedAlphaCasesPerLane": 23, "publishedAlphaSha256": ALPHA_SHA256, "coreDependencies": "1.81.2", "coreSignatureAndPayloadIdentity": True, "catalogsOverlap": True}}
    save(directory / "release-manifest.json", manifest)
    return digest((directory / "release-manifest.json").read_bytes())


def verify(directory, root, revision, version, controller, expected):
    require(re.fullmatch(r"[0-9a-f]{64}", expected, re.I) is not None, "Reviewed manifest SHA-256 required")
    manifest_path = directory / "release-manifest.json"
    require(manifest_path.is_file() and not manifest_path.is_symlink() and manifest_path.stat().st_size <= 128 * 1024, "Invalid manifest")
    require(digest(manifest_path.read_bytes()) == expected.lower(), "Manifest digest mismatch")
    manifest = read_json(manifest_path)
    require(manifest.get("sourceTag") == TAG and manifest.get("schemaVersion") == 1 and manifest.get("repository") == REPOSITORY and manifest.get("sourceCommit") == revision and manifest.get("controllerCommit") == controller and manifest.get("version") == version, "Manifest provenance mismatch")
    require(source_identity(root, revision) == manifest.get("sourceTree"), "Source tree mismatch")
    expected_files = {"release-manifest.json", f"Bosak.Schema.{version}.nupkg"}
    require({f.name for f in directory.iterdir()} == expected_files, "Unexpected artifact set")
    require(package_info(directory / f"Bosak.Schema.{version}.nupkg", root, revision, version) == manifest.get("package"), "Package payload mismatch")
    require(manifest.get("qualification") == {"sourceCases": 345, "packageCases": 33, "legacyCasesPerLane": 124, "publishedAlphaCasesPerLane": 23, "publishedAlphaSha256": ALPHA_SHA256, "coreDependencies": "1.81.2", "coreSignatureAndPayloadIdentity": True, "catalogsOverlap": True}, "Qualification profile mismatch")
    return manifest


def registry_absent(version):
    require(version == VERSION, "Wrong prerelease")
    url = f"https://api.nuget.org/v3-flatcontainer/bosak.schema/{version}/bosak.schema.{version}.nupkg"
    try:
        with urlopen(url, timeout=30) as response:
            response.read(1)
    except HTTPError as error:
        try:
            require(error.code == 404, "Registry did not confirm absence")
            return
        finally:
            error.close()
    raise ValueError("Version already exists; push refused")


def command(args, log):
    with log.open("wb") as output:
        subprocess.run(args, check=True, stdout=output, stderr=subprocess.STDOUT, timeout=180)


def legacy_check(baseline, package, results, published=False):
    label, count, assembly, commit = ("published", 23, "Bosak.Schema.PublicConsumer.Tests.dll", ALPHA_SOURCE) if published else ("legacy", 124, "Bosak.Schema.Tests.dll", LEGACY)
    root = baseline.parent / (label + "-layouts")
    require(not root.exists(), "Existing legacy layouts must be preserved")
    records = []
    for name in ["control", "candidate"]:
        target = root / name
        shutil.copytree(baseline, target)
        if name == "candidate":
            with zipfile.ZipFile(package) as archive:
                (target / "Bosak.Schema.dll").write_bytes(archive.read("lib/net10.0/Bosak.Schema.dll"))
        before = {f.relative_to(target).as_posix(): digest(f.read_bytes()) for f in target.rglob("*") if f.is_file()}
        command(["dotnet", "vstest", str(target / assembly), "--logger:trx", "--ResultsDirectory:" + str(results / (label + "-" + name))], results / (label + "-" + name + ".log"))
        require(before == {rel: digest((target / rel).read_bytes()) for rel in before}, "Legacy layout mutated")
        records.append(before)
    require(set(records[0]) == set(records[1]) and [name for name in records[0] if records[0][name] != records[1][name]] == ["Bosak.Schema.dll"], "Unexpected legacy replacement")
    a, b = case_catalog(results / (label + "-control")), case_catalog(results / (label + "-candidate"))
    require(a == b and len(a) == 1 and len(a[0]) == count, "Retained case mismatch")
    save(results / (label + ".json"), {"baselineCommit": commit, "count": count, "onlySchemaDllChanged": True, "sameCases": True, "inventories": records})


def consumer_identity(package, cache, output, assets, results, published=False):
    data = read_json(assets)
    require(not any(item["type"] == "project" for item in data["libraries"].values()), "Consumer source override refused")
    version = ALPHA_VERSION if published else VERSION
    require(data["libraries"].get("Bosak.Schema/" + version, {}).get("type") == "package", "Schema package reference mismatch")
    if published:
        require(digest(package.read_bytes()) == ALPHA_SHA256, "Published alpha archive mismatch")
        command(["dotnet", "nuget", "verify", "--all", str(package)], results / "published-schema.signature.log")
    own = cache / "bosak.schema" / version
    require((own / package.name.lower()).read_bytes() == package.read_bytes(), "Cached package mismatch")
    with zipfile.ZipFile(package) as archive:
        require(archive.read("lib/net10.0/Bosak.Schema.dll") == (own / "lib/net10.0/Bosak.Schema.dll").read_bytes() == (output / "Bosak.Schema.dll").read_bytes(), "Runtime Schema mismatch")
    rows = []
    for identity in sorted(data["libraries"]):
        if not identity.startswith("Bosak.") or identity.startswith("Bosak.Schema/"):
            continue
        name, version = identity.split("/")
        require(version == "1.81.2", "Core version drift")
        folder = cache / name.lower() / version
        archive_path = folder / (name.lower() + "." + version + ".nupkg")
        with zipfile.ZipFile(archive_path) as archive:
            require(".signature.p7s" in archive.namelist(), "Core signature missing")
            leaf = "lib/net10.0/" + name + ".dll"
            require(archive.read(leaf) == (folder / leaf).read_bytes() == (output / (name + ".dll")).read_bytes(), "Core payload drift")
        command(["dotnet", "nuget", "verify", "--all", str(archive_path)], results / (("published-" if published else "") + name + ".signature.log"))
        rows.append({"id": name, "version": version, "identical": True, "signatureExitCode": 0, "archiveSha256": digest(archive_path.read_bytes())})
    require(len(rows) == 9 and {r["id"] for r in rows} == CORE_ASSEMBLIES, "Incomplete Core closure")
    save(results / ("published-identity.json" if published else "consumer-identity.json"), {"packageSha256": digest(package.read_bytes()), "packageOnly": True, "schemaSignatureExitCode": 0 if published else None, "core": rows})


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("operation", choices=["seal", "verify", "absent", "legacy", "consumer", "published", "published-consumer"])
    for arg in ["directory", "source-root", "source-commit", "version", "controller-commit", "expected-manifest", "results", "baseline", "package", "cache", "output", "assets"]:
        parser.add_argument("--" + arg)
    args = parser.parse_args()
    if args.operation == "seal":
        print(seal(Path(args.directory), Path(args.source_root), args.source_commit, args.version, args.controller_commit, Path(args.results)))
    elif args.operation in ["verify", "absent"]:
        verify(Path(args.directory), Path(args.source_root), args.source_commit, args.version, args.controller_commit, args.expected_manifest)
        if args.operation == "absent":
            registry_absent(args.version)
        print("Verified reviewed package" + ("; registry version absent" if args.operation == "absent" else ""))
    elif args.operation in ["legacy", "published"]:
        legacy_check(Path(args.baseline), Path(args.package), Path(args.results), args.operation == "published")
    else:
        consumer_identity(Path(args.package), Path(args.cache), Path(args.output), Path(args.assets), Path(args.results), args.operation == "published-consumer")


if __name__ == "__main__":
    try:
        main()
    except (ValueError, OSError, KeyError, TypeError, ET.ParseError, zipfile.BadZipFile, subprocess.SubprocessError) as error:
        raise SystemExit("Schema release refused: " + str(error)) from None
