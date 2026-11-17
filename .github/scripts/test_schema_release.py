"""Release controls use synthetic ZIP/TRX files and mocked launch/network boundaries."""
# Copyright 2026 Fytala. SPDX-License-Identifier: Apache-2.0
import json
from pathlib import Path
import tempfile
import unittest
from unittest.mock import patch
from urllib.error import HTTPError, URLError
import xml.etree.ElementTree as ET
import zipfile

import schema_release as release


class ReleaseControls(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory()
        self.addCleanup(self.temp.cleanup)
        self.root = Path(self.temp.name)
        self.source = self.root / "source"
        (self.source / "docs").mkdir(parents=True)
        for name in ["LICENSE.md", "NOTICE", "docs/NUGET_README.md"]:
            (self.source / name).write_text(name, encoding="utf-8")
        self.out = self.root / "package"
        self.out.mkdir()
        self.package = self.out / ("Bosak.Schema." + release.VERSION + ".nupkg")
        self.entries = self.contents()
        self.write_package()
        self.results = self.root / "results"
        self.trx("source", [312, 33])
        self.trx("consumer", [33])
        self.trx("legacy-control", [124])
        self.trx("legacy-candidate", [124])
        release.save(self.results / "legacy.json", {"baselineCommit": release.LEGACY, "count": 124, "onlySchemaDllChanged": True, "sameCases": True})
        self.identity = {"packageSha256": release.digest(self.package.read_bytes()), "packageOnly": True, "core": [{"id": name, "version": "1.81.2", "identical": True, "signatureExitCode": 0} for name in sorted(release.CORE_ASSEMBLIES)]}
        release.save(self.results / "consumer-identity.json", self.identity)
        self.trx("published-control", [23])
        self.trx("published-candidate", [23])
        release.save(self.results / "published.json", {"baselineCommit": release.ALPHA_SOURCE, "count": 23, "onlySchemaDllChanged": True, "sameCases": True})
        self.alpha = dict(self.identity, packageSha256=release.ALPHA_SHA256, schemaSignatureExitCode=0)
        release.save(self.results / "published-identity.json", self.alpha)
        self.controller = "1" * 40
        self.tree = "2" * 40
        self.source_patch = patch.object(release, "source_identity", return_value=self.tree)
        self.source_patch.start()
        self.addCleanup(self.source_patch.stop)

    def contents(self):
        dependencies = "".join(f'<dependency id="{name}" version="1.81.2"/>' for name in sorted(release.DEPENDENCIES))
        spec = f'''<package xmlns="http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"><metadata><id>Bosak.Schema</id><version>{release.VERSION}</version><license type="expression">Apache-2.0</license><readme>NUGET_README.md</readme><repository type="git" url="https://github.com/{release.REPOSITORY}.git" commit="{release.SOURCE}"/><dependencies><group targetFramework="net10.0">{dependencies}</group></dependencies></metadata></package>'''
        return {"Bosak.Schema.nuspec": spec.encode(), "LICENSE.md": b"LICENSE.md", "NOTICE": b"NOTICE", "NUGET_README.md": b"docs/NUGET_README.md", "lib/net10.0/Bosak.Schema.dll": b"synthetic-not-executable", "lib/net10.0/Bosak.Schema.xml": b"<doc/>"}

    def write_package(self):
        with zipfile.ZipFile(self.package, "w", zipfile.ZIP_DEFLATED) as archive:
            for name, data in self.entries.items():
                archive.writestr(name, data)

    def trx(self, lane, counts):
        folder = self.results / lane
        folder.mkdir(parents=True, exist_ok=True)
        namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"
        for index, count in enumerate(counts):
            root = ET.Element("TestRun", xmlns=namespace)
            result = ET.SubElement(root, "Results")
            for number in range(count):
                ET.SubElement(result, "UnitTestResult", testName="case" + str(number), outcome="Passed")
            ET.SubElement(ET.SubElement(root, "ResultSummary"), "Counters", total=str(count), passed=str(count), failed="0")
            ET.ElementTree(root).write(folder / (str(index) + ".trx"), encoding="utf-8", xml_declaration=True)

    def seal(self):
        return release.seal(self.out, self.source, release.SOURCE, release.VERSION, self.controller, self.results)

    def verify(self, digest):
        return release.verify(self.out, self.source, release.SOURCE, release.VERSION, self.controller, digest)

    def test_qualified_manifest_and_payload_pass_without_launcher(self):
        with patch.object(release.subprocess, "run", side_effect=AssertionError("No process in verification")):
            manifest = self.verify(self.seal().upper())
        self.assertEqual(manifest["qualification"]["sourceCases"], 345)

    def test_manifest_hash_tamper_refuses(self):
        original = self.seal()
        with (self.out / "release-manifest.json").open("ab") as stream:
            stream.write(b" ")
        with self.assertRaisesRegex(ValueError, "digest mismatch"):
            self.verify(original)

    def test_unexpected_entry_is_named_and_still_refused(self):
        self.entries["unexpected.txt"] = b"diagnostic-only"
        self.write_package()
        with self.assertRaisesRegex(ValueError, r"Unexpected package content: .*unexpected.txt"):
            self.seal()
        self.assertFalse((self.out / "release-manifest.json").exists())

    def test_nuget_named_core_properties_are_qualified_and_hashed(self):
        name = "package/services/metadata/core-properties/nuget.psmdcp"
        self.entries[name] = b"<coreProperties/>"
        self.write_package()
        self.identity["packageSha256"] = release.digest(self.package.read_bytes())
        release.save(self.results / "consumer-identity.json", self.identity)
        manifest = self.verify(self.seal())
        self.assertEqual(manifest["package"]["entries"][name], release.digest(self.entries[name]))

    def test_similar_metadata_names_remain_refused(self):
        for name in ["package/services/metadata/core-properties/nuget-extra.psmdcp", "package/services/metadata/core-properties/nuget.psmdcp.dll", "package/services/metadata/core-properties/NuGet.psmdcp"]:
            with self.subTest(name=name):
                self.entries = self.contents()
                self.entries[name] = b"unapproved"
                self.write_package()
                with self.assertRaises(ValueError):
                    self.seal()

    def test_failure_artifact_is_distinct_from_eligible_package(self):
        workflow = (Path(__file__).parent.parent / "workflows/release.yml").read_text(encoding="utf-8")
        self.assertIn("if: failure()\n        with:\n          name: schema-rejected-package-", workflow)
        self.assertIn("retention-days: 7", workflow)
        admitted = workflow.index("name: schema-nuget-${{ github.sha }}")
        rejected = workflow.index("name: schema-rejected-package-")
        self.assertLess(admitted, rejected)
        self.assertNotIn("if: always()", workflow[workflow.rfind("- uses:", 0, admitted):admitted])

    def test_package_tamper_refuses(self):
        original = self.seal()
        self.entries["lib/net10.0/Bosak.Schema.dll"] = b"changed"
        self.write_package()
        with self.assertRaisesRegex(ValueError, "payload mismatch"):
            self.verify(original)

    def test_extra_artifact_file_refuses(self):
        original = self.seal()
        (self.out / "extra.nupkg").write_bytes(b"not-a-package")
        with self.assertRaisesRegex(ValueError, "artifact set"):
            self.verify(original)

    def test_wrong_manifest_controller_refuses(self):
        original = self.seal()
        with self.assertRaisesRegex(ValueError, "provenance"):
            release.verify(self.out, self.source, release.SOURCE, release.VERSION, "3" * 40, original)

    def test_wrong_source_metadata_refuses(self):
        self.entries["Bosak.Schema.nuspec"] = self.entries["Bosak.Schema.nuspec"].replace(release.SOURCE.encode(), b"3" * 40)
        self.write_package()
        with self.assertRaisesRegex(ValueError, "Source metadata"):
            self.seal()

    def test_dependency_change_refuses(self):
        self.entries["Bosak.Schema.nuspec"] = self.entries["Bosak.Schema.nuspec"].replace(b'"1.81.2"', b'"1.81.17"')
        self.write_package()
        with self.assertRaisesRegex(ValueError, "Dependency"):
            self.seal()

    def test_legal_file_change_refuses(self):
        self.entries["NOTICE"] = b"changed"
        self.write_package()
        with self.assertRaisesRegex(ValueError, "legal/README"):
            self.seal()

    def test_missing_xml_docs_refuses(self):
        del self.entries["lib/net10.0/Bosak.Schema.xml"]
        self.write_package()
        with self.assertRaisesRegex(ValueError, "documentation"):
            self.seal()

    def test_unsafe_zip_paths_refuse(self):
        for name in ["../escape", "/absolute", "C:/drive", "bad\\path", "build/runner.ps1"]:
            with self.subTest(name=name):
                self.entries = self.contents()
                self.entries[name] = b"blocked"
                self.write_package()
                with self.assertRaises(ValueError):
                    self.seal()

    def test_nuspec_dtd_refuses(self):
        self.entries["Bosak.Schema.nuspec"] = b'<!DOCTYPE package [<!ENTITY a "bad">]>' + self.entries["Bosak.Schema.nuspec"]
        self.write_package()
        with self.assertRaisesRegex(ValueError, "DTD"):
            self.seal()

    def test_duplicate_zip_entry_refuses(self):
        with self.assertWarnsRegex(UserWarning, "Duplicate"):
            with zipfile.ZipFile(self.package, "a") as archive:
                archive.writestr("NOTICE", b"duplicate")
        with self.assertRaisesRegex(ValueError, "Duplicate"):
            self.seal()

    def test_wrong_catalog_or_partial_legacy_refuses(self):
        self.trx("consumer", [22])
        with self.assertRaisesRegex(ValueError, "qualification catalogs"):
            self.seal()

    def test_wrong_core_closure_refuses(self):
        self.identity["core"][0]["id"] = "Bosak.Unknown"
        release.save(self.results / "consumer-identity.json", self.identity)
        with self.assertRaisesRegex(ValueError, "identity incomplete"):
            self.seal()

    def test_signature_failure_refuses(self):
        self.identity["core"][0]["signatureExitCode"] = 1
        release.save(self.results / "consumer-identity.json", self.identity)
        with self.assertRaisesRegex(ValueError, "identity incomplete"):
            self.seal()

    def test_foreign_source_or_dirty_checkout_refuses(self):
        self.source_patch.stop()
        with self.assertRaisesRegex(ValueError, "reviewed source"):
            release.source_identity(self.source, "3" * 40)
        with patch.object(release.subprocess, "check_output", side_effect=[release.SOURCE + "\n", " M source.cs\n"]):
            with self.assertRaisesRegex(ValueError, "clean"):
                release.source_identity(self.source, release.SOURCE)
        self.source_patch.start()

    def test_missing_published_lane_refuses(self):
        (self.results / "published.json").unlink()
        with self.assertRaises(OSError):
            self.seal()

    def test_published_catalog_drift_refuses(self):
        self.trx("published-candidate", [22])
        with self.assertRaisesRegex(ValueError, "Published alpha TRX"):
            self.seal()

    def test_published_archive_or_signature_failure_refuses(self):
        for field, value in [("packageSha256", "0" * 64), ("schemaSignatureExitCode", 1), ("packageOnly", False)]:
            with self.subTest(field=field):
                changed = dict(self.alpha, **{field: value})
                release.save(self.results / "published-identity.json", changed)
                with self.assertRaisesRegex(ValueError, "Published alpha identity"):
                    self.seal()

    def test_wrong_or_missing_beta_tag_refuses(self):
        self.source_patch.stop()
        answers = [release.SOURCE, "", "https://github.com/" + release.REPOSITORY, "3" * 40]
        with patch.object(release.subprocess, "check_output", side_effect=answers), patch.object(release.subprocess, "run"):
            with self.assertRaisesRegex(ValueError, "tag mismatch"):
                release.source_identity(self.source, release.SOURCE)
        with patch.object(release.subprocess, "check_output", side_effect=answers[:3] + [release.subprocess.CalledProcessError(128, "git")]), patch.object(release.subprocess, "run"):
            with self.assertRaises(release.subprocess.CalledProcessError):
                release.source_identity(self.source, release.SOURCE)
        self.source_patch.start()

    def test_registry_404_permits_preflight_only(self):
        with patch.object(release, "urlopen", side_effect=HTTPError("url", 404, "absent", {}, None)):
            release.registry_absent(release.VERSION)

    def test_alpha_version_cannot_be_admitted_as_beta(self):
        with self.assertRaisesRegex(ValueError, "prerelease"):
            release.package_info(self.package, self.source, release.SOURCE, release.ALPHA_VERSION)

    def test_beta_tag_and_exact_source_are_accepted(self):
        self.source_patch.stop()
        answers = [release.SOURCE, "", "https://github.com/" + release.REPOSITORY, release.SOURCE, self.tree]
        with patch.object(release.subprocess, "check_output", side_effect=answers) as git, patch.object(release.subprocess, "run"):
            self.assertEqual(release.source_identity(self.source, release.SOURCE), self.tree)
        self.assertIn("refs/tags/" + release.TAG + "^{commit}", git.call_args_list[3].args[0])
        self.source_patch.start()

    def test_retained_lanes_preserve_original_binaries_and_refuse_reuse(self):
        for published, label, count, assembly in [(False, "legacy", 124, "Bosak.Schema.Tests.dll"), (True, "published", 23, "Bosak.Schema.PublicConsumer.Tests.dll")]:
            with self.subTest(label=label):
                baseline = self.root / label / "baseline"
                baseline.mkdir(parents=True)
                (baseline / "Bosak.Schema.dll").write_bytes(b"old-schema")
                (baseline / assembly).write_bytes(b"retained-tests")
                (baseline / "fixture.xml").write_bytes(b"unchanged")
                def launch(args, log):
                    target = Path(args[2]).parent
                    self.assertEqual((target / assembly).read_bytes(), b"retained-tests")
                    self.assertEqual((target / "fixture.xml").read_bytes(), b"unchanged")
                    lane = "candidate" if target.name == "candidate" else "control"
                    self.trx(label + "-" + lane, [count])
                with patch.object(release, "command", side_effect=launch) as launcher:
                    release.legacy_check(baseline, self.package, self.results, published)
                    self.assertEqual(launcher.call_count, 2)
                    with self.assertRaisesRegex(ValueError, "preserved"):
                        release.legacy_check(baseline, self.package, self.results, published)
                self.assertEqual((baseline / "Bosak.Schema.dll").read_bytes(), b"old-schema")
                self.assertEqual(release.read_json(self.results / (label + ".json"))["count"], count)

    def test_retained_layout_mutation_refuses(self):
        baseline = self.root / "mutation" / "baseline"
        baseline.mkdir(parents=True)
        (baseline / "Bosak.Schema.dll").write_bytes(b"old-schema")
        (baseline / "Bosak.Schema.Tests.dll").write_bytes(b"retained-tests")
        def launch(args, log):
            Path(args[2]).write_bytes(b"unexpected-mutation")
        with patch.object(release, "command", side_effect=launch):
            with self.assertRaisesRegex(ValueError, "mutated"):
                release.legacy_check(baseline, self.package, self.results)

    def test_workflow_beta_pins_and_both_retained_lanes(self):
        workflow = (Path(__file__).resolve().parents[1] / "workflows/release.yml").read_text(encoding="utf-8")
        for value in ["SOURCE_COMMIT: " + release.SOURCE, "PACKAGE_VERSION: " + release.VERSION, "ref: " + release.ALPHA_SOURCE, "schema_release.py published-consumer", "schema_release.py published --baseline", "schema_release.py legacy --baseline"]:
            self.assertIn(value, workflow)

    def test_registry_existing_auth_failure_or_network_failure_refuse(self):
        response = unittest.mock.MagicMock()
        for result in [response, HTTPError("url", 403, "denied", {}, None), URLError("offline")]:
            with self.subTest(result=type(result).__name__):
                kwargs = {"side_effect": result} if isinstance(result, Exception) else {"return_value": result}
                with patch.object(release, "urlopen", **kwargs):
                    with self.assertRaises((ValueError, OSError)):
                        release.registry_absent(release.VERSION)

    def test_workflow_publisher_never_builds_or_suppresses_duplicates(self):
        workflow = (Path(__file__).resolve().parents[1] / "workflows/release.yml").read_text(encoding="utf-8")
        publisher = workflow.split("\n  publish:\n", 1)[1]
        for forbidden in ["dotnet build", "dotnet pack", "dotnet test", "dotnet restore", "--skip-duplicate"]:
            self.assertNotIn(forbidden, publisher)
        self.assertLess(publisher.index("schema_release.py absent"), publisher.index("NuGet/login@v1"))
        self.assertLess(publisher.index("NuGet/login@v1"), publisher.index("dotnet nuget push"))
        self.assertIn("id-token: write", publisher)
        self.assertEqual(workflow.count("id-token: write"), 1)
        self.assertIn("default: false", workflow)
        self.assertNotIn("    tags:", workflow)


if __name__ == "__main__":
    unittest.main()
