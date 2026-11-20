"""Reconstructs the isolated native feasibility sources in a new scratch directory.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import argparse,hashlib,json,shutil,subprocess

def safe(root,name):
    path=(root/name).resolve()
    if not path.is_relative_to(root.resolve()):raise ValueError('Path escapes scratch directory')
    return path

def main():
    parser=argparse.ArgumentParser();parser.add_argument('--checkout',required=True);parser.add_argument('--output',required=True);args=parser.parse_args()
    evidence=Path(__file__).resolve().parent;checkout=Path(args.checkout).resolve();output=Path(args.output).resolve();manifest=json.loads((evidence/'BASE-SOURCE.json').read_text(encoding='utf-8'))
    if output.is_relative_to(evidence):raise ValueError('Scratch output must be outside sealed evidence')
    command=['git','-c','safe.directory='+checkout.as_posix(),'-C',str(checkout)]
    if subprocess.check_output(command+['rev-parse','HEAD']).decode().strip()!=manifest['base']:raise ValueError('Checkout must be at the recorded base')
    subprocess.run(command+['diff','--exit-code',manifest['base'],'--','src','tests'],check=True)
    if subprocess.check_output(command+['ls-files','--others','--exclude-standard','--','src','tests']):raise ValueError('Checkout has extra untracked source/tests')
    output.mkdir(parents=True,exist_ok=False)
    for item in evidence.iterdir():
        if item.is_dir():shutil.copytree(item,output/item.name)
        else:shutil.copyfile(item,output/item.name)
    patches={p.relative_to(evidence/'candidate').as_posix():p.read_bytes() for p in (evidence/'candidate').rglob('*.cs')}
    if set(patches)!={'Assessment/NativeSchemaProvider.cs','Assessment/NativeSessionNode.cs'}:raise ValueError('Unexpected candidate source overlay')
    for name,digest in manifest['gitBlobSha256'].items():
        blob=subprocess.check_output(command+['show',manifest['base']+':'+name])
        if hashlib.sha256(blob).hexdigest()!=digest:raise ValueError('Original Git blob hash mismatch')
        relative=Path(name).relative_to('src/Bosak.Schema')
        for directory in ['original','candidate']:
            target=safe(output/directory,relative);target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(blob)
    for name,blob in patches.items():safe(output/'candidate',Path(name)).write_bytes(blob)
    original_root='D:/Development/Bosak/.worktrees/Schema.TypedNodeContract'
    for project in (output/'regression').rglob('*.csproj'):
        text=project.read_text(encoding='utf-8').replace(original_root,checkout.as_posix());project.write_text(text,encoding='utf-8')
    print('Materialized original and two-file candidate sources at '+str(output))

if __name__=='__main__':main()
