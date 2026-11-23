from pathlib import Path
import json,hashlib,shutil,subprocess
w=Path('D:/Development/Bosak/.worktrees/Schema.Particles');p=Path('D:/Development/Bosak/.guard-tmp/schema-choice-production-20261123');e=w/'docs/particles/implementation-evidence';e.mkdir(exist_ok=True);sha=lambda b:hashlib.sha256(b).hexdigest();read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'));dump=lambda p,o:p.write_text(json.dumps(o,indent=2)+'\n',encoding='utf-8')
# Preserve diagnostic setup failures as well as successful final receipts; exclude executable/build outputs and caches.
for f in p.iterdir():
 if f.is_file():shutil.copyfile(f,e/f.name)
for name in ['source-qualified','source-final34','minimum-consumer','current-consumer','current-native','minimum-runner','current-runner','readme-example','surface-probe','iterations','signatures','minimum-consumer-results','current-consumer-results','minimum-consumer-qualified-results','current-consumer-qualified-results','minimum-consumer-final34-results','current-consumer-final34-results','current-native-results','current-native-final34-results','legacy124-results','alpha23-results','beta33-results','legacy124-final34-results','alpha23-final34-results','beta33-final34-results','pilot-minimum','pilot-current','pilot-final-minimum','pilot-final-current','expanded-minimum-1','expanded-minimum-2','expanded-current-1','expanded-current-2','expanded-final-minimum-1','expanded-final-minimum-2','expanded-final-current-1','expanded-final-current-2']:
 for f in (p/name).rglob('*'):
  if not f.is_file() or any(x in ['bin','obj'] for x in f.relative_to(p/name).parts):continue
  out=e/f.relative_to(p);out.parent.mkdir(parents=True,exist_ok=True);shutil.copyfile(f,out)
old=read(w/'docs/particles/ENGINE-INPUTS.json');changed={};current={}
for rel,h in old.items():
 b=(w/rel).read_bytes();current[rel]=sha(b)
 if current[rel]!=h:changed[rel]=dict(before=h,after=current[rel])
assert set(changed)=={'src/Bosak.Schema/Assessment/NativeDecimalSchema.cs','src/Bosak.Schema/Assessment/NativeNestedSchema.cs','src/Bosak.Schema/Assessment/NativeSimpleRestriction.cs','tests/Bosak.Schema.Tests/Bosak.Schema.Tests.csproj','tests/Bosak.Schema.PublicConsumer.Tests/Bosak.Schema.PublicConsumer.Tests.csproj'},set(changed)
new=['src/Bosak.Schema/Assessment/NativeChoice.cs','tests/Bosak.Schema.Tests/NativeChoiceIdentityTests.cs','tests/Bosak.Schema.Tests/NativeChoiceBoundaryTests.cs','tests/Bosak.Schema.PublicConsumer.Tests/NativeChoiceContractTests.cs']+[f.relative_to(w).as_posix() for f in (w/'tests/TestData/Particles').iterdir()]
for rel in new:current[rel]=sha((w/rel).read_bytes())
dump(e/'PRODUCTION-INPUTS.json',dict(base='aa07e801ad5f2d89ebccd7299a4f539800e20df1',baselineCount=len(old),files=current,changedExisting=changed,new=new))
for rel in list(changed)+new:
 out=e/'production-inputs'/rel;out.parent.mkdir(parents=True,exist_ok=True);out.write_bytes((w/rel).read_bytes())
plan=read(w/'docs/particles/choice-feasibility-evidence/CANDIDATE-SOURCE.json')
for row in plan['files']:
 row['sha256']=sha((w/'src/Bosak.Schema'/row['path']).read_bytes())
 if row['path'] in plan['overlays']:
  out=e/'overlay'/row['path'];out.parent.mkdir(parents=True,exist_ok=True);out.write_bytes((w/'src/Bosak.Schema'/row['path']).read_bytes())
dump(e/'CANDIDATE-SOURCE.json',plan)
shutil.copyfile(w/'docs/particles/choice-feasibility-evidence/reconstruct.py',e/'reconstruct.py')
checks=read(p/'RESULTS.json');delta=read(p/'EXPANDED-DELTA.json');checks['expandedFirstRefusalReasonChangesPerCore']=6;dump(e/'RESULTS.json',checks)
# Explicit local code review, with no broader conformance acceptance.
dump(e/'REVIEW.json',dict(date='2026-11-23',scope='Bounded production choice; xs:all deferred',findings=[],reviewedFiles=list(changed)+new[:4],checked=['Expanded-name duplicate detection and eager scalar compilation','Physical order and selected declarations; no alternative backtracking','Optional choice retains element-only no-typed-value root','Worst-case max alternative budget; shared pattern work','Named root cloning, flat/nested routing and nested-choice refusal','No public API, Core pin, legacy grammar or release-controller delta'],limits=['No independent review agent was used','Mid-execution cancellation timing and broader xsi:type combinations are not newly claimed','No fresh NuGet advisory data; cached packages, signatures and runtime identities verified']))
print('Prepared production receipts and exact input/source manifests:',len(current),'inputs')
