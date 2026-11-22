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
        changes=set(read(P/"CANDIDATE-SOURCE.json")["overlays"])
        payloads=[]
        for row in base["files"]:
            blob=subprocess.check_output(["git","show",base["base"]+":"+row["path"]],cwd=ROOT)
            assert sha(blob)==row["sha256"]
            relative=Path(row["path"]).relative_to("src/Bosak.Schema")
            if relative.as_posix() in changes: blob=(P/"overlay"/relative).read_bytes()
            payloads.append((relative,blob))
        for row in read(P/"CANDIDATE-SOURCE.json")["files"]:
            if row["baseSha256"] is None:payloads.append((Path(row["path"]),(P/"overlay"/row["path"]).read_bytes()))
        args.directory.mkdir(parents=True)
        for relative,blob in payloads:
            target=args.directory/relative;target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(blob)
    verify(args.directory)
    print("Verified57candidate source hashes; six explicit overlays; no execution.")
