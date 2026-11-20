from pathlib import Path
import json,hashlib,zipfile,subprocess,xml.etree.ElementTree as E
p=Path(__file__).parent;(p/'signatures').mkdir(exist_ok=True);sha=lambda b:hashlib.sha256(b).hexdigest();lanes={}
for lane,version,commit in [('minimum','1.81.2','7249d0e01628c47f92b89a3380bc69cf58d70045'),('current','1.81.17','6a7618e9dcf70eb8a67d5d1789ff7b787133b52d')]:
 assets=json.loads((p/lane/'obj/project.assets.json').read_text(encoding='utf-8-sig'));assert not any(r['type']=='project' for r in assets['libraries'].values());rows=[]
 for name in assets['libraries']:
  if not name.startswith('Bosak.'):continue
  ident,v=name.split('/');assert v==version
  cache=p/(lane+'-cache')/ident.lower()/v;archive=cache/(ident.lower()+'.'+v+'.nupkg')
  with zipfile.ZipFile(archive) as z:
   payload=z.read('lib/net10.0/'+ident+'.dll');assert payload==(cache/'lib/net10.0'/(ident+'.dll')).read_bytes()==(p/lane/'bin/Release/net10.0'/(ident+'.dll')).read_bytes()
   repo=E.fromstring(z.read(next(n for n in z.namelist() if n.endswith('.nuspec')))).find('.//{*}repository');assert repo.attrib['commit']==commit
  with (p/'signatures'/(lane+'-'+ident+'.log')).open('wb') as out:r=subprocess.run(['dotnet','nuget','verify','--all',str(archive)],stdout=out,stderr=subprocess.STDOUT,stdin=subprocess.DEVNULL,timeout=60)
  assert r.returncode==0,ident
  rows.append(dict(id=ident,version=v,sourceCommit=commit,archiveSha256=sha(archive.read_bytes()),dllSha256=sha(payload),signatureVerified=True,cacheRuntimeMatch=True))
 assert len(rows)==9;lanes[lane]=dict(packages=rows,projectReferences=False)
(p/'IDENTITY.json').write_text(json.dumps(dict(date='2026-11-20',lanes=lanes),indent=2)+'\n',encoding='utf-8');print('Verified18Core signatures/source pins/payload identities.')
