"""Refusal and measurement integrity checks use mocked launch boundaries only."""
# Copyright 2026 Fytala. SPDX-License-Identifier: Apache-2.0
import importlib.util
from pathlib import Path
import tempfile
import unittest
from unittest.mock import Mock
import subprocess

spec = importlib.util.spec_from_file_location("schema_workloads", Path(__file__).with_name("run-schema-workloads.py"))
runner = importlib.util.module_from_spec(spec)
spec.loader.exec_module(runner)


class WorkloadControls(unittest.TestCase):
    def valid(self):
        return {"success": True, "assertionsChecked": True, "workload": "native-small", "iterations": 1, "warmup": 0,
                "schemaAssemblySha256": "a" * 64, "elapsedMilliseconds": [1.5], "callingThreadAllocatedBytes": [123], "peakWorkingSetBytes": 1000}

    def test_complete_expected_sample_is_accepted(self):
        runner.validate(self.valid(), "native-small", 1, 0, "a" * 64)

    def test_drift_nan_and_truncated_samples_refuse(self):
        for key, value in [("success", False), ("schemaAssemblySha256", "b" * 64), ("elapsedMilliseconds", [float("nan")]),
                           ("callingThreadAllocatedBytes", []), ("peakWorkingSetBytes", 0), ("iterations", 2)]:
            with self.subTest(key=key):
                data = self.valid()
                data[key] = value
                with self.assertRaises(ValueError):
                    runner.validate(data, "native-small", 1, 0, "a" * 64)

    def test_invalid_arguments_and_binary_drift_refuse_before_launch(self):
        with tempfile.TemporaryDirectory() as directory:
            path = Path(directory)
            assembly = path / "Bosak.Schema.Workloads.dll"
            assembly.write_bytes(b"synthetic-not-executable")
            launcher = Mock(side_effect=AssertionError("Must not launch"))
            with self.assertRaises(ValueError):
                runner.run_one(assembly, path, "unknown", 1, 0, "refuse", {}, launcher)
            with self.assertRaises(ValueError):
                runner.run_one(assembly, path, "native-small", 1, 0, "refuse", {assembly.name: "wrong"}, launcher)
            launcher.assert_not_called()

    def test_watchdog_timeout_is_propagated_and_logs_retained(self):
        with tempfile.TemporaryDirectory() as directory:
            path = Path(directory)
            assembly = path / "Bosak.Schema.Workloads.dll"
            assembly.write_bytes(b"synthetic-not-executable")
            launcher = Mock(side_effect=subprocess.TimeoutExpired("mock", 30))
            with self.assertRaises(subprocess.TimeoutExpired):
                runner.run_one(assembly, path, "native-small", 1, 0, "timeout", {assembly.name: runner.digest(assembly)}, launcher)
            self.assertEqual(launcher.call_args.kwargs["timeout"], 30)
            self.assertTrue((path / "timeout.stderr.log").exists())

    def test_failed_process_stops_before_reading_success_shape(self):
        with tempfile.TemporaryDirectory() as directory:
            path = Path(directory)
            assembly = path / "Bosak.Schema.Workloads.dll"
            assembly.write_bytes(b"synthetic-not-executable")
            launcher = Mock(return_value=subprocess.CompletedProcess("mock", 1))
            with self.assertRaisesRegex(ValueError, "Diagnostic process failed"):
                runner.run_one(assembly, path, "native-small", 1, 0, "failed", {assembly.name: runner.digest(assembly)}, launcher)
            self.assertTrue((path / "failed.json").exists())

    def test_nearest_rank_p95_and_exceeded_memory_envelope_remain_visible(self):
        cold = [{"parentWallMs": 10, "data": {"elapsedMilliseconds": [1], "callingThreadAllocatedBytes": [100], "peakWorkingSetBytes": 1000}} for _ in range(3)]
        warm = {"data": {"elapsedMilliseconds": list(range(1, 31)), "callingThreadAllocatedBytes": [100] * 30,
                         "peakWorkingSetBytes": runner.THRESHOLDS["processPeakWorkingSetMaxBytes"] + 1}}
        row = runner.summarize("native-small", cold + [warm])
        self.assertEqual(row["warmWorkloadP95Ms"], 29)
        self.assertFalse(row["withinProposedLocalEnvelope"])


if __name__ == "__main__":
    unittest.main()
