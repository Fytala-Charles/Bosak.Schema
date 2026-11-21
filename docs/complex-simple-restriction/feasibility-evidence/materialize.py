"""Reconstruct the isolated source/probe trees; no network/build/launch/delete.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import argparse,hashlib,json,shutil,subprocess
E=Path(__file__).resolve().parent
sha=lambda b:hashlib.sha256(b).hexdigest()
def main():
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--repository',required=True,type=Path)
    parser.add_argument('--output',required=True,type=Path)
    args=parser.parse_args();repo=args.repository.resolve();out=args.output.resolve()
    base=json.loads((E/'BASE-SOURCE.json').read_text(encoding='utf-8-sig'))
    git=['git','-c','safe.directory='+repo.as_posix()]
    assert subprocess.check_output(git+['rev-parse','HEAD'],cwd=repo).decode().strip()==base['base']
    paths=['src','tests','tools','Directory.Build.props','global.json','.github']
    assert not subprocess.check_output(git+['diff','--name-only','--',*paths],cwd=repo).strip()
    assert not subprocess.check_output(git+['ls-files','--others','--exclude-standard','--',*paths],cwd=repo).strip()
    assert not out.exists(),'Use a new output directory; nothing is deleted or overwritten.'
    out.mkdir(parents=True)
    for name,record in base['files'].items():
        rel=Path(name);assert not rel.is_absolute() and '..' not in rel.parts
        raw=subprocess.check_output(git+['show',base['base']+':src/Bosak.Schema/'+rel.as_posix()],cwd=repo)
        assert sha(raw)==record['sha256'],name
        for tree in ['original','candidate']:
            dest=out/tree/rel;dest.parent.mkdir(parents=True,exist_ok=True);dest.write_bytes(raw)
    candidate=json.loads((E/'CANDIDATE-SOURCE.json').read_text(encoding='utf-8-sig'))
    for name in candidate['modifiedOrAdded']:
        rel=Path(name);assert not rel.is_absolute() and '..' not in rel.parts
        raw=(E/'candidate'/rel).read_bytes();assert sha(raw)==candidate['files'][name]
        dest=out/'candidate'/rel;dest.parent.mkdir(parents=True,exist_ok=True);dest.write_bytes(raw)
    for name,h in candidate['files'].items():assert sha((out/'candidate'/name).read_bytes())==h,name
    originalScratch='D:/Development/Bosak/.guard-tmp/schema-complex-restriction-feasibility-20261121'
    originalRepo='D:/Development/Bosak/.worktrees/Schema.ComplexRestriction'
    files=['ComplexTests.cs','BaselineTests.cs','Fixtures.cs','case-plan.json','facet-rules.json','Directory.Build.props','NuGet.Config','verify-identity.py']
    files += [p.relative_to(E).as_posix() for folder in ['baseline','minimum','current','regression','regression-overrides','surface'] for p in (E/folder).rglob('*') if p.is_file() and p.suffix in ['.csproj','.cs','.json']]
    for name in files:
        dest=out/name;dest.parent.mkdir(parents=True,exist_ok=True)
        raw=(E/name).read_bytes()
        if dest.suffix=='.csproj':raw=raw.decode('utf-8-sig').replace(originalScratch,out.as_posix()).replace(originalRepo,repo.as_posix()).encode('utf-8')
        dest.write_bytes(raw)
    print('Reconstructed53baseline/54candidate source files and rebound probe/comparison projects; no engine executed.')
if __name__=='__main__':main()
