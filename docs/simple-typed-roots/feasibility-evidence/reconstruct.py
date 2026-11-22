"""Reconstruct only the isolated source tree; no engine execution or network.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import argparse,json,hashlib,subprocess
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
parser=argparse.ArgumentParser();parser.add_argument("directory",type=Path);a=parser.parse_args()
assert not a.directory.exists(),"Use a fresh directory."
m=json.loads((P/"CANDIDATE-SOURCE.json").read_text());payloads=[]
for row in m["files"]:
    b=subprocess.check_output(["git","show",m["base"]+":src/Bosak.Schema/"+row["path"]],cwd=ROOT)
    assert hashlib.sha256(b).hexdigest()==row["gitBaseSha256"]
    if row["overlay"]:b=(P/"overlay"/row["path"]).read_bytes()
    elif row["lineEndings"]=="CRLF":b=b.replace(b"\r\n",b"\n").replace(b"\n",b"\r\n")
    assert hashlib.sha256(b).hexdigest()==row["sha256"],row["path"]
    payloads.append((row["path"],b))
a.directory.mkdir(parents=True)
for f,b in payloads:
    p=a.directory/f;p.parent.mkdir(parents=True,exist_ok=True);p.write_bytes(b)
print("Verified/reconstructed58files from exact Git base and3overlays; no build or engine run.")
