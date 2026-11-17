"""Verify local Schema0.5 package-review evidence without launching .NET or publishing."""
from pathlib import Path
import hashlib,json,zipfile
here=Path(__file__).resolve().parent;root=here.parents[1]
sha=lambda b:hashlib.sha256(b).hexdigest()
def read(name):return json.loads((here/name).read_text(encoding='utf-8-sig'))
v=read('VERIFICATION.json');api=read('API-DIFF.json');original=read('PACKAGE.json');stage=read('STAGING-PACKAGE.json')
assert sorted(int(s['counters']['total']) for s in v['sourceSolution'])==[23,249]
for lane in ['sourceSolution','originalPackageConsumer','stagingPackageConsumer']:
 assert all(outcome=='Passed' for s in v[lane] for _,outcome in s['cases'])
assert v['originalPackageConsumer'][0]['cases']==v['stagingPackageConsumer'][0]['cases']
legacy=v['legacyRetainedBinary'];a=legacy['legacy-baseline'];b=legacy['legacy-candidate']
assert a['count']==b['count']==124 and a['cases']==b['cases']
assert set(a['inventory'])==set(b['inventory'])
assert [f for f in a['inventory'] if a['inventory'][f]!=b['inventory'][f]]==['Bosak.Schema.dll']
assert not api['removedOrChangedRows'] and len(api['addedTypes'])==7 and len(api['addedRows'])==35
assert stage['runtimeAndXmlByteIdenticalToCleanCandidate'] and stage['archiveCacheOutputDllIdentity'] and stage['onlyPackageReferences']
assert all(dep['version']=='1.81.2' for dep in original['dependencies'])
core=read('CORE-PACKAGES.json')
assert len(core)==9 and all(r['version']=='1.81.2' and r['archiveCacheRuntimeIdentical'] and r['signatureVerifyExitCode']==0 for r in core)
assert original['version']==stage['version']=='0.5.0-alpha.0.6'
assert not v['published'] and v['stagingPackagingUncommitted']
archive=read('ARCHIVE.json');assert sha((here/'EVIDENCE.zip').read_bytes())==archive['sha256']
with zipfile.ZipFile(here/'EVIDENCE.zip') as z:
 assert set(z.namelist())=={e['path'] for e in archive['entries']}
 for entry in archive['entries']:
  data=z.read(entry['path']);assert sha(data)==entry['sha256'],entry['path']
  assert not entry['path'].lower().endswith(('.dll','.pdb','.exe','.nupkg','.spki','.pem','.pfx'))
  if entry['path']=='packaging/NUGET_README.md':assert data.replace(b'\r\n',b'\n')==(root/'docs/NUGET_README.md').read_bytes().replace(b'\r\n',b'\n')
for entry in read('MANIFEST.json')['files']:
 data=(root/entry['path']).read_bytes()
 if entry['mode']=='lf':data=data.replace(b'\r\n',b'\n')
 assert sha(data)==entry['sha256'],entry['path']
print('PASS: exact local package/API/license/consumer receipts and source-only review integrity; no publication or stable acceptance inferred.')
