"""Reconstruct the sealed isolated xsi:type candidate in a new scratch directory.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
No network, compilation, execution, deletion or global Git configuration changes.
"""
from pathlib import Path
import argparse, hashlib, json, subprocess

def digest(data): return hashlib.sha256(data).hexdigest()
def safe(root,name):
    path=(root/name).resolve()
    if not path.is_relative_to(root.resolve()):raise ValueError("Path escapes output")
    return path

def main():
    parser=argparse.ArgumentParser();parser.add_argument("--repository",required=True);parser.add_argument("--output",required=True);args=parser.parse_args()
    evidence=Path(__file__).resolve().parent;repo=Path(args.repository).resolve();output=Path(args.output).resolve()
    base=json.loads((evidence/"BASE-SOURCE.json").read_text(encoding="utf-8"));candidate=json.loads((evidence/"CANDIDATE-SOURCE.json").read_text(encoding="utf-8"))
    def git(*items):return subprocess.check_output(["git","-c","safe.directory="+repo.as_posix(),*items],cwd=repo)
    if git("rev-parse","HEAD").decode().strip()!=base["productBase"]:raise ValueError("Requires exact recorded base checkout")
    paths=["src","tests","tools","Directory.Build.props","global.json"]
    if git("diff","HEAD","--name-only","--",*paths).strip() or git("ls-files","--others","--exclude-standard","--",*paths).strip():raise ValueError("Requires unchanged source/test/tool/configuration inputs")
    if output.exists():raise ValueError("Use a new output directory")
    # All guard checks precede output creation; no destructive operation is used.
    output.mkdir(parents=True)
    for row in base["files"]:
        payload=git("show",base["productBase"]+":src/Bosak.Schema/"+row["path"])
        if digest(payload)!=row["sha256"]:raise ValueError("Baseline blob mismatch")
        for lane in ["original","candidate"]:
            dest=safe(output,lane+"/"+row["path"]);dest.parent.mkdir(parents=True,exist_ok=True);dest.write_bytes(payload)
    for name,expected in candidate["changedFiles"].items():
        payload=safe(evidence,"candidate/"+name).read_bytes()
        if digest(payload)!=expected:raise ValueError("Candidate overlay mismatch")
        dest=safe(output,"candidate/"+name);dest.parent.mkdir(parents=True,exist_ok=True);dest.write_bytes(payload)
    for name,expected in candidate["allCandidateFiles"].items():
        if digest(safe(output,"candidate/"+name).read_bytes())!=expected:raise ValueError("Candidate tree mismatch")
    names=["Directory.Build.props","NuGet.Config","BeforeTests.cs","XsiTypeTests.cs","NativeDataTests.cs","Fixtures.cs","cases.json","proposal-cases.json","verify-identity.py","BASE-SOURCE.json","CANDIDATE-SOURCE.json"]
    for lane in ["baseline","minimum","current"]:names += [lane+"/Probe.csproj",lane+"/schema/SchemaCandidate.csproj"]
    names += ["surface/Surface.csproj","surface/Program.cs","regression/implementation/Regression.csproj","regression/public/Regression.csproj"]
    names += [f.relative_to(evidence).as_posix() for f in (evidence/"regression-overrides").rglob("*") if f.is_file()]
    old_repo="D:/Development/Bosak/.worktrees/Schema.XsiTypeContract";old_raw="D:/Development/Bosak/.guard-tmp/schema-xsi-type-feasibility-20261121"
    for name in names:
        source=safe(evidence,name);dest=safe(output,name);dest.parent.mkdir(parents=True,exist_ok=True)
        if name.endswith(".csproj"):
            text=source.read_text(encoding="utf-8").replace(old_repo,repo.as_posix()).replace(old_raw,output.as_posix());dest.write_text(text,encoding="utf-8")
        else:dest.write_bytes(source.read_bytes())
    print("Reconstructed exact baseline/candidate source trees and isolated projects; no build or launch performed.")
if __name__=="__main__":main()
