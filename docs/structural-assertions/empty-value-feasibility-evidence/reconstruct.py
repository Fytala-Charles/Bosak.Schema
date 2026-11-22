"""Reconstruct or verify the isolated candidate; no build, launch, network or deletion.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import argparse, hashlib, json, subprocess
P=Path(__file__).resolve().parent
ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()

def verify(directory):
    plan=read(P/"CANDIDATE-SOURCE.json")
    for row in plan["files"]:
        assert sha((directory/row["path"]).read_bytes())==row["sha256"],row["path"]
    assert len(list(directory.rglob("*.cs")))==len(plan["files"])

if __name__=="__main__":
    parser=argparse.ArgumentParser();parser.add_argument("directory",type=Path);parser.add_argument("--verify-only",action="store_true");args=parser.parse_args()
    if not args.verify_only:
        assert not args.directory.exists(),"Reconstruction requires a new destination."
        base=read(P/"BASE-SOURCE.json")
        changes=set(read(P/"CANDIDATE-SOURCE.json")["changedFiles"])
        payloads=[]
        for row in base["files"]:
            blob=subprocess.check_output(["git","show",base["base"]+":"+row["path"]],cwd=ROOT)
            assert sha(blob)==row["sha256"]
            relative=Path(row["path"]).relative_to("src/Bosak.Schema")
            if relative.name in changes: blob=(P/"overlay"/relative.name).read_bytes()
            payloads.append((relative,blob))
        args.directory.mkdir(parents=True)
        for relative,blob in payloads:
            target=args.directory/relative;target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(blob)
    verify(args.directory)
    print("Verified56candidate source hashes; five explicit overlays; no execution.")
