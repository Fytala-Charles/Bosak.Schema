"""Measurement-integrity controls, not Schema conformance cases.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
import importlib.util, json, sys, tempfile, unittest
from pathlib import Path
from unittest.mock import patch
spec=importlib.util.spec_from_file_location("pilot",Path(__file__).with_name("run-xsd-conformance-pilot.py"));pilot=importlib.util.module_from_spec(spec);spec.loader.exec_module(pilot)
class PilotControls(unittest.TestCase):
    def test_unsupported_is_never_a_validity_match(self):
        for expected in ["valid","invalid"]:
            for row in [dict(compile="Unsupported",assessment=None),dict(compile="Valid",assessment="Unsupported")]:self.assertEqual("unsupported",pilot.classify(expected,row))
    def test_validity_comparisons_are_exact(self):
        for expected,actual,result in [("valid","Valid","match"),("invalid","Invalid","match"),("invalid","Valid","mismatch"),("valid","Invalid","mismatch")]:self.assertEqual(result,pilot.classify(expected,dict(compile="Valid",assessment=actual)))
    def test_schema_invalid_does_not_count_as_instance_invalid(self):
        self.assertEqual("mismatch",pilot.classify("invalid",dict(compile="Invalid",assessment=None)))
    def test_harness_error_and_missing_result_are_not_matches(self):
        self.assertEqual("harnessError",pilot.classify("invalid",dict(error="Exception")))
        with self.assertRaises(ValueError):pilot.classify("valid",dict(compile="Valid",assessment=None))
    def test_version_tokens_use_or_for_tests_and_and_for_expected(self):
        node=pilot.ET.fromstring('<test version="1.1 unknown-feature"/>');self.assertTrue(pilot.applies(node,{"1.1"}));self.assertFalse(pilot.applies(node,{"1.1"},True))
    def test_ambiguous_version_expectations_refuse(self):
        node=pilot.ET.fromstring('<test xmlns="http://www.w3.org/XML/2004/xml-schema-test-suite/"><expected validity="valid"/><expected version="1.1" validity="invalid"/></test>')
        with self.assertRaises(ValueError):pilot.expected_value(node,{"1.1"})
    def test_path_escape_refuses(self):
        with tempfile.TemporaryDirectory() as root:
            with self.assertRaises(ValueError):pilot.safe_path(Path(root),"../outside.xml")
    def test_hash_change_refuses_before_metadata(self):
        with tempfile.TemporaryDirectory() as directory:
            root=Path(directory);(root/'x').write_bytes(b'changed')
            with self.assertRaisesRegex(ValueError,"hash mismatch"):pilot.verify(dict(files={'x':'0'*64},cases=[],configurationTokens=[]),root)
    def test_duplicate_missing_and_extra_results_refuse(self):
        requests=[dict(id='a')]
        for rows in [[],[dict(id='b')],[dict(id='a'),dict(id='a')]]:
            with self.assertRaises(ValueError):pilot.checked_results(dict(rows=rows),requests)
    def test_preflight_failure_never_launches_driver(self):
        with patch.object(sys,'argv',['pilot','--output','unused','--runner','unused']),patch.object(pilot,'verify',side_effect=ValueError('preflight')),patch.object(pilot.subprocess,'run') as launcher:
            with self.assertRaises(ValueError):pilot.main()
            launcher.assert_not_called()
    def candidate_setup(self, root):
        source=root/'src';source.mkdir();(source/'a.cs').write_bytes(b'qualified')
        runtime=root/'runtime';runtime.mkdir();runner=runtime/'runner.dll';runner.write_bytes(b'qualified binary')
        evidence=dict(productBase='base',changedProductionFiles={'src/a.cs':pilot.hashlib.sha256(b'qualified').hexdigest()},runner='runner.dll',runtimeFiles={'runner.dll':pilot.hashlib.sha256(b'qualified binary').hexdigest()})
        path=root/'evidence.json';path.write_text(json.dumps(evidence),encoding='utf-8');return runner,path
    def test_qualified_candidate_requires_exact_source_and_runtime(self):
        with tempfile.TemporaryDirectory() as directory:
            root=Path(directory);runner,path=self.candidate_setup(root)
            with patch.object(pilot.subprocess,'check_output',side_effect=[b'src/a.cs\0',b'']):self.assertEqual('base',pilot.verify_candidate(root,'base',runner,path)['productBase'])
            (root/'src/a.cs').write_bytes(b'drift')
            with patch.object(pilot.subprocess,'check_output',side_effect=[b'src/a.cs\0',b'']):
                with self.assertRaisesRegex(ValueError,'source hash'):pilot.verify_candidate(root,'base',runner,path)
    def test_candidate_extra_untracked_production_file_refuses(self):
        with tempfile.TemporaryDirectory() as directory:
            root=Path(directory);runner,path=self.candidate_setup(root)
            with patch.object(pilot.subprocess,'check_output',side_effect=[b'src/a.cs\0',b'src/new.cs\0']):
                with self.assertRaisesRegex(ValueError,'file set'):pilot.verify_candidate(root,'base',runner,path)
    def test_candidate_runtime_drift_and_wrong_base_refuse(self):
        with tempfile.TemporaryDirectory() as directory:
            root=Path(directory);runner,path=self.candidate_setup(root);runner.write_bytes(b'drift')
            with patch.object(pilot.subprocess,'check_output',side_effect=[b'src/a.cs\0',b'']):
                with self.assertRaisesRegex(ValueError,'runtime hash'):pilot.verify_candidate(root,'base',runner,path)
            with self.assertRaisesRegex(ValueError,'base differs'):pilot.verify_candidate(root,'wrong-base',runner,path)
    def test_baseline_untracked_source_refuses(self):
        with patch.object(pilot.subprocess,'run'),patch.object(pilot.subprocess,'check_output',return_value=b'src/new.cs\0'):
            with self.assertRaisesRegex(ValueError,'Untracked baseline'):pilot.verify_baseline(Path('.'),'base')
    def test_baseline_clean_source_accepts(self):
        with patch.object(pilot.subprocess,'run') as check,patch.object(pilot.subprocess,'check_output',return_value=b''):
            pilot.verify_baseline(Path('.'),'base');check.assert_called_once()
if __name__=='__main__':unittest.main()
