from pathlib import Path
import json,hashlib,zipfile,subprocess
P=Path('D:/Development/Bosak/.guard-tmp/schema-date-simple-content-feasibility-20261124');O=Path('D:/Development/Bosak/.guard-tmp/schema-native-date-production-20261124');W=Path('D:/Development/Bosak/.worktrees/Schema.Coverage');read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'));sha=lambda b:hashlib.sha256(b).hexdigest();original=read(O/'IDENTITY-FINAL.json');lanes=[]
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
(P/'IDENTITY.json').write_text(json.dumps(dict(lanes=lanes,scope='Isolated Schema source candidate with signed Core packages, not packaged Schema qualification'),indent=2)+'\n',encoding='utf-8')
base=read(P/'BASE-SOURCE.json');files=[];overlays=[]
for row in base['files']:
 rel=row['path'];b=subprocess.check_output(['git','show',base['base']+':src/Bosak.Schema/'+rel],cwd=W);raw=(P/'candidate'/rel).read_bytes();entry=dict(row,baseGitSha256=sha(b),candidateSha256=sha(raw))
 if sha(raw)!=row['checkoutSha256']:overlays.append(rel)
 else:assert raw==b or raw==b.replace(b'\n',b'\r\n');entry['baseTransform']='identity' if raw==b else 'LF-to-CRLF'
 files.append(entry)
assert len(files)==63 and len(overlays)==3
(P/'CANDIDATE-SOURCE.json').write_text(json.dumps(dict(base=base['base'],files=files,overlays=overlays),indent=2)+'\n',encoding='utf-8');print('Verified18Core archive/runtime identities, two fresh Core signatures and63source files/three overlays.')
