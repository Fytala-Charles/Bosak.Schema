from pathlib import Path
import json,hashlib,zipfile,subprocess
P=Path('D:/Development/Bosak/.guard-tmp/schema-native-date-feasibility-20261124');O=Path('D:/Development/Bosak/.guard-tmp/schema-integer-enumeration-production-20261124');read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'));sha=lambda b:hashlib.sha256(b).hexdigest();original=read(O/'IDENTITY-FINAL.json');lanes=[]
for lane in ['minimum','current']:
 runtime=P/lane/'bin/Release/net10.0';rows=[]
 for c in original['lanes'][lane+'-consumer']['core']:
  archive=O/(lane+'-consumer-cache')/c['id'].lower()/c['version']/(c['id'].lower()+'.'+c['version']+'.nupkg');assert sha(archive.read_bytes())==c['archiveSha256']
  with zipfile.ZipFile(archive) as z:b=z.read('lib/net10.0/'+c['id']+'.dll')
  assert b==(runtime/(c['id']+'.dll')).read_bytes();rows.append(dict(c,runtimeSha256=sha(b)))
  if c['id']=='Bosak.XPath.Core':
   with (P/lane/'signature.log').open('wb') as log:r=subprocess.run(['dotnet','nuget','verify','--all',str(archive)],stdout=log,stderr=subprocess.STDOUT,timeout=60)
   assert r.returncode==0
 lanes.append(dict(lane=lane,corePackagesOnly=True,core=rows,candidateDllSha256=sha((runtime/'Bosak.Schema.dll').read_bytes()),freshCoreSignature=True))
(P/'IDENTITY.json').write_text(json.dumps(dict(lanes=lanes,scope='Isolated Schema source project with signed Core packages; not a packaged Schema consumer run'),indent=2)+'\n',encoding='utf-8')
base=read(P/'BASE-SOURCE.json');files=[];overlays=[]
for row in base['files']:
 f=P/'candidate'/row['path'];h=sha(f.read_bytes());files.append(dict(row,candidateSha256=h))
 if h!=row['sha256']:overlays.append(row['path'])
new=[f.relative_to(P/'candidate').as_posix() for f in (P/'candidate').rglob('*.cs') if f.relative_to(P/'candidate').as_posix() not in {r['path'] for r in base['files']}]
(P/'CANDIDATE-SOURCE.json').write_text(json.dumps(dict(base=base['base'],files=files,overlays=overlays,newFiles={rel:sha((P/'candidate'/rel).read_bytes()) for rel in new}),indent=2)+'\n',encoding='utf-8');print('Verified18 signed Core archive/runtime identities, fresh Core signatures and source manifest:',len(files),'base files,',len(overlays),'overlays,',len(new),'new.')
