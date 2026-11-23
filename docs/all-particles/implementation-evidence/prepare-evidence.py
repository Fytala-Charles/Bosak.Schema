from pathlib import Path
import json,hashlib,shutil
w=Path('D:/Development/Bosak/.worktrees/Schema.All');p=Path('D:/Development/Bosak/.guard-tmp/schema-all-production-20261123');e=w/'docs/all-particles/implementation-evidence';e.mkdir(exist_ok=True);read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'));dump=lambda p,o:p.write_text(json.dumps(o,indent=2)+'\n',encoding='utf-8');sha=lambda b:hashlib.sha256(b).hexdigest()
for f in p.iterdir():
 if f.is_file():shutil.copyfile(f,e/f.name)
for name in ['source-qualified','minimum-consumer','current-consumer','current-native','minimum-runner','current-runner','surface-probe','readme-example','signatures','minimum-consumer-results','current-consumer-results','current-native-results','legacy124-results','alpha23-results','beta33-results','pilot-minimum','pilot-current','expanded-minimum-1','expanded-minimum-2','expanded-current-1','expanded-current-2']:
 for f in (p/name).rglob('*'):
  if not f.is_file() or any(x in ['bin','obj'] for x in f.relative_to(p/name).parts):continue
  out=e/f.relative_to(p);out.parent.mkdir(parents=True,exist_ok=True);shutil.copyfile(f,out)
base=read(w/'docs/all-particles/ENGINE-INPUTS.json');current={};changed={}
for rel,h in base.items():
 current[rel]=sha((w/rel).read_bytes())
 if current[rel]!=h:changed[rel]=dict(before=h,after=current[rel])
assert len(changed)==6,set(changed)
new=['src/Bosak.Schema/Assessment/NativeAll.cs','tests/Bosak.Schema.PublicConsumer.Tests/NativeAllContractTests.cs','tests/Bosak.Schema.Tests/NativeAllIdentityTests.cs','tests/Bosak.Schema.Tests/NativeAllBoundaryTests.cs','tests/Bosak.Schema.Tests/NativeAllAdditionalTests.cs']+[f.relative_to(w).as_posix() for f in (w/'tests/TestData/AllParticles').iterdir()]
assert len(new)==15
for rel in new:current[rel]=sha((w/rel).read_bytes())
dump(e/'PRODUCTION-INPUTS.json',dict(base='724d834b7e2cf1353bdacf100802ff29866c8c71',baselineCount=296,files=current,changedExisting=changed,new=new))
for rel in list(changed)+new:
 out=e/'production-inputs'/rel;out.parent.mkdir(parents=True,exist_ok=True);out.write_bytes((w/rel).read_bytes())
plan=read(w/'docs/all-particles/feasibility-evidence/CANDIDATE-SOURCE.json');plan['baseFilesUseCanonicalGitBytes']=True
for row in plan['files']:
 b=(w/'src/Bosak.Schema'/row['path']).read_bytes();row['checkoutSha256']=sha(b)
 if row['path'] in plan['overlays']:
  row['sha256']=sha(b);out=e/'overlay'/row['path'];out.parent.mkdir(parents=True,exist_ok=True);out.write_bytes(b)
 else:assert row['sha256']==row['baseSha256']
dump(e/'CANDIDATE-SOURCE.json',plan);shutil.copyfile(w/'docs/all-particles/feasibility-evidence/reconstruct.py',e/'reconstruct.py')
dump(e/'REVIEW.json',dict(date='2026-11-23',scope='Self-review of bounded root all production candidate; no independent review agent',blockingFindings=[],checked=['Expanded QName uniqueness and independent member minima','Absent optional group versus present incomplete group and all-optional emptiability','Physical-order original nodes, declarations, governing types and scalar slots','All sum budget, choice max preservation and eager omitted source definitions','Named-root cloning, mixed catalogs and both compiler routes; nested all refusal','No partial publication on failure, shared pattern recovery, authority and lifetime','All3739previous logical stimuli retained;18measured compile expectations explicitly adopted','No public API, Core pin, legacy grammar or controller change'],limits=['Nested/all-wide grammar,implicit anyType and broader scalar/facet/type-chain coverage excluded','No full PSVI/C45, general typed-query/public closure or performance acceptance','No mid-execution timing claim'],nugetAdvisoryChecksEnabled=True,initialRestrictedRestoreFailurePreserved=True))
print('Recorded311 production inputs, sixexisting changes and15new files; source and qualification receipts copied.')
