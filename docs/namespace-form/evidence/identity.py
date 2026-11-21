from pathlib import Path
import json,hashlib,zipfile,subprocess,xml.etree.ElementTree as ET
p=Path(__file__).parent;sha=lambda b:hashlib.sha256(b).hexdigest()
package=next((p/'feed').glob('*.nupkg'))
with zipfile.ZipFile(package) as z:dllsha=sha(z.read('lib/net10.0/Bosak.Schema.dll'))
signatures=p/'signatures';signatures.mkdir(exist_ok=True)
lanes={}
for lane,version,commit in [('minimum-consumer','1.81.2','7249d0e01628c47f92b89a3380bc69cf58d70045'),('current-consumer','1.81.17','6a7618e9dcf70eb8a67d5d1789ff7b787133b52d')]:
 cache=p/(lane+'-cache');runtime=p/lane/'bin/Release/net10.0';assets=json.loads((p/lane/'obj/project.assets.json').read_text(encoding='utf-8-sig'))
 assert not any(x['type']=='project' for x in assets['libraries'].values())
 assert sha((runtime/'Bosak.Schema.dll').read_bytes())==dllsha
 own=cache/'bosak.schema/0.6.0-alpha.0.25'
 assert (own/package.name.lower()).read_bytes()==package.read_bytes() and sha((own/'lib/net10.0/Bosak.Schema.dll').read_bytes())==dllsha
 rows=[]
 for name in assets['libraries']:
  if not name.startswith('Bosak.') or name.startswith('Bosak.Schema/'):continue
  ident,v=name.split('/');assert v==version
  folder=cache/ident.lower()/v;archive=folder/(ident.lower()+'.'+v+'.nupkg')
  with zipfile.ZipFile(archive) as z:
   assert '.signature.p7s' in z.namelist();assert z.read('lib/net10.0/'+ident+'.dll')==(folder/'lib/net10.0'/ (ident+'.dll')).read_bytes()==(runtime/(ident+'.dll')).read_bytes()
   root=ET.fromstring(z.read(next(x for x in z.namelist() if x.endswith('.nuspec'))));repo=root.find('.//{*}repository');assert repo is not None and repo.attrib['commit']==commit,(ident,repo.attrib)
  with (signatures/(lane+'-'+ident+'.log')).open('wb') as log:result=subprocess.run(['dotnet','nuget','verify','--all',str(archive)],stdout=log,stderr=subprocess.STDOUT,stdin=subprocess.DEVNULL,timeout=90)
  assert result.returncode==0,ident
  rows.append(dict(id=ident,version=v,sourceCommit=commit,archiveSha256=sha(archive.read_bytes()),signatureVerified=True,payloadIdentical=True))
 assert len(rows)==9
 lanes[lane]=dict(core=rows,packageOnly=True,schemaPayloadIdentical=True)
retained=[]
for lane in json.loads((p/'RETAINED.json').read_text(encoding='utf-8-sig')):
 old=Path(lane['source']);new=p/(lane['label']+'-retained');count=0
 for f in old.rglob('*'):
  if f.is_file() and f.relative_to(old).as_posix()!='Bosak.Schema.dll':assert f.read_bytes()==(new/f.relative_to(old)).read_bytes();count+=1
 assert sha((new/'Bosak.Schema.dll').read_bytes())==dllsha
 retained.append(dict(lane=lane['label'],unchangedNonSchemaFiles=count,onlySchemaDllChanged=True))
beta=Path('D:/Development/Bosak/.guard-tmp/schema-beta1-publication-20261118/cache/bosak.schema/0.5.0-beta.1/bosak.schema.0.5.0-beta.1.nupkg')
assert sha(beta.read_bytes())=='045fe07dda92dc19aeb954c4c2a99fe936ffc0d4202c29ccd59b0c42582cca70'
betaLane=next(x for x in json.loads((p/'RETAINED.json').read_text(encoding='utf-8-sig')) if x['label']=='beta33')
with zipfile.ZipFile(beta) as z:assert z.read('lib/net10.0/Bosak.Schema.dll')==(Path(betaLane['source'])/'Bosak.Schema.dll').read_bytes()
with (signatures/'published-beta1.log').open('wb') as log:result=subprocess.run(['dotnet','nuget','verify','--all',str(beta)],stdout=log,stderr=subprocess.STDOUT,stdin=subprocess.DEVNULL,timeout=90)
assert result.returncode==0
(p/'IDENTITY-FINAL.json').write_text(json.dumps(dict(package=package.name,packageSha256=sha(package.read_bytes()),schemaDllSha256=dllsha,unsignedLocalCandidate=True,lanes=lanes,retained=retained,publishedBeta1SignatureAndPayloadVerified=True),indent=2)+'\n',encoding='utf-8')
print('Verified 18 Core signatures/source pins/payloads, local candidate identities, three unchanged callers and signed beta.1 baseline.')
