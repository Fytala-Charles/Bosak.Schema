"""Reconstruct the isolated source candidate; no build, network, launch or deletion.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import argparse,json,hashlib,subprocess
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
parser=argparse.ArgumentParser();parser.add_argument("directory",type=Path);parser.add_argument("--verify-only",action="store_true");a=parser.parse_args();plan=read(P/"CANDIDATE-SOURCE.json")
if not a.verify_only:
    assert not a.directory.exists(),"Use a new directory."
    payloads=[]
    for row in plan["files"]:
        if row["baseSha256"] is None:b=(P/"overlay"/row["path"]).read_bytes()
        else:
            b=subprocess.check_output(["git","show",plan["base"]+":src/Bosak.Schema/"+row["path"]],cwd=ROOT);assert sha(b)==row["baseSha256"]
            if row["path"] in plan["overlays"]:b=(P/"overlay"/row["path"]).read_bytes()
        assert sha(b)==row["sha256"];payloads.append((row["path"],b))
    a.directory.mkdir(parents=True)
    for relative,b in payloads:
        f=a.directory/relative;f.parent.mkdir(parents=True,exist_ok=True);f.write_bytes(b)
for row in plan["files"]:assert sha((a.directory/row["path"]).read_bytes())==row["sha256"]
assert len(list(a.directory.rglob("*.cs")))==61
print("Verified61candidate source hashes from60Gitblobs plus4explicit overlays; no build/run/network/delete.")
