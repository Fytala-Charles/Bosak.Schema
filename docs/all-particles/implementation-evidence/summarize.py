from pathlib import Path
import json,xml.etree.ElementTree as E,hashlib,zipfile,re
w=Path('D:/Development/Bosak/.worktrees/Schema.All');p=Path('D:/Development/Bosak/.guard-tmp/schema-all-production-20261123');old=Path('D:/Development/Bosak/.guard-tmp/schema-choice-production-20261123');read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'));dump=lambda p,o:p.write_text(json.dumps(o,indent=2)+'\n',encoding='utf-8');ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
def cases(path):
 rows={}
 for f in path.rglob('*.trx'):
  for t in E.parse(f).findall('.//t:UnitTestResult',ns):
   assert t.attrib['outcome']=='Passed',(f,t.attrib['testName']);assert t.attrib['testName'] not in rows;rows[t.attrib['testName']]='Passed'
 return rows
prior=cases(w/'docs/particles/implementation-evidence/source-final34');now=cases(p/'source-qualified');assert len(prior)==3739 and len(now)==3885 and all(now.get(k)==v for k,v in prior.items());dump(p/'STABLE-CASES.json',dict(previousCount=3739,currentCount=3885,preservedCount=3739,removed=[],renamed=[],added=sorted(set(now)-set(prior)),authoredCompileExpectationChanges=18,allOriginalNames=sorted(prior)))
for lane in ['minimum','current']:assert len(cases(p/(lane+'-consumer-results')))==2634
assert len(cases(p/'current-native-results'))==60
for lane,count in [('legacy124',124),('alpha23',23),('beta33',33)]:assert len(cases(p/(lane+'-results')))==count
expanded={};pilot={}
for lane in ['minimum','current']:
 deltas=[];counts=dict(match=0,unsupported=0,mismatch=0,harnessError=0)
 for cohort in [1,2]:
  before=read(old/f'expanded-final-{lane}-{cohort}/RESULTS.json');after=read(p/f'expanded-{lane}-{cohort}/RESULTS.json');assert len(before['rows'])==len(after['rows'])
  for a,b in zip(before['rows'],after['rows']):
   assert a['id']==b['id'];assert {k:v for k,v in a.items() if k not in ['native','classification']}=={k:v for k,v in b.items() if k not in ['native','classification']}
   if a!=b:deltas.append(dict(id=a['id'],before=a,after=b))
  for k,v in after['counts'].items():counts[k]+=v
 assert counts==dict(match=8,unsupported=121,mismatch=0,harnessError=0),counts
 expanded[lane]=dict(counts=counts,deltas=deltas,originalPayloadsAndExpectedOutcomesUnchanged=True,executionDate='2026-11-23',historicalMeasurementScriptDateConstant='2026-11-22')
 before=read(old/f'pilot-final-{lane}/RESULTS.json');after=read(p/f'pilot-{lane}/RESULTS.json');assert before['rows']==after['rows'];assert after['counts']==dict(match=21,mismatch=0,unsupported=0,harnessError=0);pilot[lane]=dict(counts=after['counts'],deltas=[])
dump(p/'EXPANDED-DELTA.json',expanded);dump(p/'PILOT-DELTA.json',pilot)
identity=read(p/'IDENTITY-FINAL.json');assert identity['package']=='Bosak.Schema.0.6.0-alpha.0.35.nupkg';h=identity['schemaDllSha256']
for f in [w/'src/Bosak.Schema/bin/Release/net10.0/Bosak.Schema.dll',p/'current-native/bin/Release/net10.0/Bosak.Schema.dll',p/'surface-probe/bin/Release/net10.0/Bosak.Schema.dll',p/'readme-example/bin/Release/net10.0/Bosak.Schema.dll']:
 assert hashlib.sha256(f.read_bytes()).hexdigest()==h
with zipfile.ZipFile(p/'feed'/identity['package']) as z:
 assert z.read('NUGET_README.md')==(w/'docs/NUGET_README.md').read_bytes()
 text=z.read('NUGET_README.md').decode('utf-8');blocks=re.findall(r'```xml\r?\n(.*?)```',text[text.index('Example all-group schema:'):],re.S);assert len(blocks)==2
 for name,b in zip(['example-all.xsd','example-all.xml'],blocks):(p/'readme-example'/name).write_bytes(b.encode())
 dump(p/'PACKAGED-EXAMPLES.json',dict(readmeSha256=hashlib.sha256(z.read('NUGET_README.md')).hexdigest(),allSchemaSha256=hashlib.sha256(blocks[0].encode()).hexdigest(),allInstanceSha256=hashlib.sha256(blocks[1].encode()).hexdigest(),codeBlockBytesPreserved=True))
assert read(p/'SURFACE.json')['zeroPublicApiDelta']
dump(p/'RESULTS.json',dict(date='2026-11-23',base='724d834b7e2cf1353bdacf100802ff29866c8c71',sourceInternal=1251,sourcePublic=2634,sourceTotal=3885,preservedOriginalSourceCases=3739,addedSourceControls=146,authoredCompileExpectationChanges=18,packageConsumersPerCore=2634,currentPackageNative=60,retained=[124,23,33],publicTypes=54,publicRows=401,apiDelta=0,frozenMatches=21,expandedMatches=8,expandedUnsupported=121,corpusMismatches=0,corpusHarnessErrors=0,expandedFirstRefusalReasonChangesPerCore=len(expanded['minimum']['deltas']),nugetAdvisoryChecksEnabled=True,localUnsignedPackage=identity['package'],packageSha256=identity['packageSha256'],schemaDllSha256=h,published=False))
print(json.dumps(read(p/'RESULTS.json')));print('Expanded detail deltas:',{k:len(v['deltas']) for k,v in expanded.items()})
