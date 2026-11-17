"""Run bounded Schema workload processes with a watchdog and reproducible raw receipts."""
# Copyright 2026 Fytala. SPDX-License-Identifier: Apache-2.0
import argparse
import hashlib
import json
import math
from pathlib import Path
import subprocess
import time

WORKLOADS = ("native-small", "native-max-bytes", "legacy-facets", "restricted-xslt", "refusal-recovery")
THRESHOLDS = {"coldWorkloadMaxMs": 5000, "warmWorkloadP95Ms": 250,
              "coldCallingThreadAllocationMaxBytes": 64 * 1024 * 1024,
              "warmCallingThreadAllocationMaxBytes": 16 * 1024 * 1024,
              "processPeakWorkingSetMaxBytes": 512 * 1024 * 1024}


def require(condition, message):
    if not condition:
        raise ValueError(message)


def digest(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def save(path, data):
    path.write_text(json.dumps(data, indent=2) + "\n", encoding="utf-8")


def validate(data, workload, count, warmup, expected_schema):
    require(data.get("success") is True and data.get("assertionsChecked") is True, "Correctness receipt missing")
    require(data.get("workload") == workload and data.get("iterations") == count and data.get("warmup") == warmup, "Workload identity mismatch")
    require(data.get("schemaAssemblySha256") == expected_schema, "Schema assembly changed")
    times, allocations = data.get("elapsedMilliseconds", []), data.get("callingThreadAllocatedBytes", [])
    require(len(times) == len(allocations) == count, "Incomplete samples")
    require(all(type(v) in (float, int) and math.isfinite(v) and v >= 0 for v in times), "Invalid elapsed sample")
    require(all(type(v) is int and v >= 0 for v in allocations), "Invalid allocation sample")
    require(type(data.get("peakWorkingSetBytes")) is int and data["peakWorkingSetBytes"] > 0, "Invalid process memory")
    if workload == "native-max-bytes":
        require(data.get("schemaBytes") == 32768 and data.get("instanceBytes") == 65536, "Boundary workload drift")


def run_one(assembly, output, workload, count, warmup, label, manifest, launcher=subprocess.run):
    require(workload in WORKLOADS and 1 <= count <= 1000 and 0 <= warmup <= 20, "Invalid diagnostic arguments")
    for name, expected in manifest.items():
        require(digest(assembly.parent / name) == expected, "Binary/configuration hash changed")
    raw = output / (label + ".json")
    error = output / (label + ".stderr.log")
    started = time.perf_counter()
    with raw.open("wb") as stdout, error.open("wb") as stderr:
        completed = launcher(["dotnet", str(assembly), workload, str(count), str(warmup)], stdout=stdout, stderr=stderr, timeout=30, check=False)
    require(completed.returncode == 0, "Diagnostic process failed; inspect retained output")
    data = json.loads(raw.read_text(encoding="utf-8-sig"))
    validate(data, workload, count, warmup, manifest["Bosak.Schema.dll"])
    return {"file": raw.name, "sha256": digest(raw), "parentWallMs": (time.perf_counter() - started) * 1000, "data": data}


def summarize(workload, runs):
    cold = runs[:3]
    warm = runs[3]["data"]
    times = sorted(warm["elapsedMilliseconds"])
    row = {"workload": workload,
           "coldWorkloadMaxMs": max(r["data"]["elapsedMilliseconds"][0] for r in cold),
           "coldParentWallMaxMs": max(r["parentWallMs"] for r in cold),
           "warmWorkloadP50Ms": times[math.ceil(len(times) * .5) - 1],
           "warmWorkloadP95Ms": times[math.ceil(len(times) * .95) - 1],
           "warmWorkloadMaxMs": max(times),
           "coldCallingThreadAllocationMaxBytes": max(r["data"]["callingThreadAllocatedBytes"][0] for r in cold),
           "warmCallingThreadAllocationMaxBytes": max(warm["callingThreadAllocatedBytes"]),
           "processPeakWorkingSetMaxBytes": max(r["data"]["peakWorkingSetBytes"] for r in runs)}
    row["withinProposedLocalEnvelope"] = all(row[key] <= maximum for key, maximum in THRESHOLDS.items())
    return row


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--assembly", type=Path, required=True)
    parser.add_argument("--output", type=Path, required=True)
    args = parser.parse_args()
    assembly = args.assembly.resolve()
    require(assembly.is_file() and assembly.name == "Bosak.Schema.Workloads.dll", "Built workload assembly required")
    require(not args.output.exists(), "Use a new output directory; prior receipts are retained")
    args.output.mkdir(parents=True)
    manifest = {path.name: digest(path) for path in sorted(assembly.parent.iterdir()) if path.suffix in {".dll", ".json"}}
    require("Bosak.Schema.dll" in manifest and assembly.name in manifest, "Incomplete build output")
    save(args.output / "BINARY-MANIFEST.json", manifest)
    results = []
    raw_runs = []
    for workload in WORKLOADS:
        runs = [run_one(assembly, args.output, workload, 1, 0, workload + "-cold-" + str(i), manifest) for i in range(3)]
        runs.append(run_one(assembly, args.output, workload, 30, 5, workload + "-warm", manifest))
        row = summarize(workload, runs)
        results.append(row)
        raw_runs.extend({k: v for k, v in item.items() if k != "data"} for item in runs)
        print(workload + ": " + ("within proposed local envelope" if row["withinProposedLocalEnvelope"] else "outside proposed local envelope"), flush=True)
    save(args.output / "SUMMARY.json", {"protocol": {"coldProcesses": 3, "warmProcesses": 1, "warmup": 5, "warmSamples": 30, "watchdogSeconds": 30},
                                        "thresholds": THRESHOLDS, "results": results, "runs": raw_runs,
                                        "environment": {key: runs[0]["data"][key] for key in ("runtime", "os", "architecture", "processors", "serverGc", "schemaAssemblySha256")},
                                        "betaAccepted": False, "productionSla": False})


if __name__ == "__main__":
    try:
        main()
    except (ValueError, OSError, KeyError, json.JSONDecodeError, subprocess.SubprocessError) as error:
        raise SystemExit("Schema workload qualification stopped: " + str(error)) from None
