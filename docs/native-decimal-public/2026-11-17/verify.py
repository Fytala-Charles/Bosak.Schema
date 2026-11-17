"""Verify the source-only native decimal candidate packet without building or launching .NET."""
from pathlib import Path
import hashlib,json,zipfile
here=Path(__file__).resolve().parent
root=here.parents[2]
def sha(data): return hashlib.sha256(data).hexdigest()
archive=json.loads((here/'ARCHIVE.json').read_text(encoding='utf-8'))
assert sha((here/'EVIDENCE.zip').read_bytes())==archive['sha256']
with zipfile.ZipFile(here/'EVIDENCE.zip') as z:
    assert set(z.namelist())=={r['path'] for r in archive['entries']}
    for entry in archive['entries']:
        data=z.read(entry['path']);assert sha(data)==entry['sha256'],entry['path']
        assert not entry['path'].lower().endswith(('.dll','.exe','.nupkg','.pdb'))
        if entry['path'].startswith('inputs/'):
            current=(root/entry['path'][7:]).read_bytes()
            assert current.replace(b'\r\n',b'\n')==data.replace(b'\r\n',b'\n'),entry['path']
v=json.loads((here/'VERIFICATION.json').read_text(encoding='utf-8'))
assert sorted(int(r['counters']['total']) for r in v['solution'])==[23,249]
assert len(v['packageConsumer'])==1 and int(v['packageConsumer'][0]['counters']['total'])==23
assert all(outcome=='Passed' for r in v['solution']+v['packageConsumer'] for _,outcome in r['cases'])
assert v['sameConsumerCaseCatalog'] and v['schemaDllIdenticalBuildPackageConsumer']
assert all(p.endswith('/1.81.2') for p in v['corePackages'])
manifest=json.loads((here/'MANIFEST.json').read_text(encoding='utf-8'))
for entry in manifest['files']:
    data=(root/entry['path']).read_bytes()
    if entry['mode']=='lf': data=data.replace(b'\r\n',b'\n')
    assert sha(data)==entry['sha256'],entry['path']
print('PASS: source/archive integrity, 272 solution cases, 23 repeated package cases and qualified runtime identity receipt.')
