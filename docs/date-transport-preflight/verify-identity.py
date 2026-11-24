from pathlib import Path
import json,hashlib,zipfile,subprocess
P=Path('D:/Development/Bosak/.guard-tmp/schema-date-transport-feasibility-20261124');O=Path('D:/Development/Bosak/.guard-tmp/schema-integer-enumeration-production-20261124');read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'));sha=lambda b:hashlib.sha256(b).hexdigest();original=read(O/'IDENTITY-FINAL.json');rows=[]
for lane in ['minimum','current']:
 runtime=P/lane/'bin/Release/net10.0';cache=O/(lane+'-consumer-cache');info=original['lanes'][lane+'-consumer'];manifest={}
 for c in info['core']:
  archive=cache/c['id'].lower()/c['version']/(c['id'].lower()+'.'+c['version']+'.nupkg');assert sha(archive.read_bytes())==c['archiveSha256']
  with zipfile.ZipFile(archive) as z:b=z.read('lib/net10.0/'+c['id']+'.dll')
  assert b==(runtime/(c['id']+'.dll')).read_bytes();manifest[c['id']]=dict(c,runtimeSha256=sha(b),archiveUnchanged=True)
  if c['id']=='Bosak.XPath.Core':
   with (P/lane/'signature.log').open('wb') as log:r=subprocess.run(['dotnet','nuget','verify','--all',str(archive)],stdout=log,stderr=subprocess.STDOUT,timeout=60)
   assert r.returncode==0
 assert sha((runtime/'Bosak.Schema.dll').read_bytes())==original['schemaDllSha256']
 project=(P/lane/'Probe.csproj').read_text(encoding='utf-8');assert 'ProjectReference' not in project and 'Microsoft.NET.Test.Sdk' not in project
 rows.append(dict(lane=lane,packageOnly=True,core=manifest,schemaDllSha256=original['schemaDllSha256'],coreSignatureFreshlyVerified=True,otherSignatures='Original18-signed-Core qualification receipts retained; exact archives and current runtime payloads rechecked'))
(P/'IDENTITY.json').write_text(json.dumps(dict(lanes=rows),indent=2)+'\n',encoding='utf-8');print('Verified18unchanged Core archives/runtime payloads, two fresh Core signatures and unchanged Schema payload; package-only projects.')
