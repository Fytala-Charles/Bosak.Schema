from pathlib import Path
import json,hashlib,xml.etree.ElementTree as ET,zipfile
p=Path('D:/Development/Bosak/.guard-tmp/schema-choice-production-20261123');old=Path('D:/Development/Bosak/.guard-tmp/schema-derived-integers-production-20261122');w=Path('D:/Development/Bosak/.worktrees/Schema.Particles');read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'));dump=lambda p,o:p.write_text(json.dumps(o,indent=2)+'\n',encoding='utf-8')
ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
def cases(path):
 rows={}
 for f in path.rglob('*.trx'):
  for t in ET.parse(f).findall('.//t:UnitTestResult',ns):
   assert t.attrib['outcome']=='Passed',(f,t.attrib['testName']);assert t.attrib['testName'] not in rows;rows[t.attrib['testName']]=t.attrib['outcome']
 return rows
prior=cases(w/'docs/derived-integer-types/implementation-evidence/source');now=cases(p/'source-final34');assert len(prior)==3620 and len(now)==3739 and all(now.get(k)==v for k,v in prior.items());dump(p/'STABLE-CASES.json',dict(previousCount=len(prior),currentCount=len(now),preservedCount=3620,removed=[],changed=[],added=sorted(set(now)-set(prior)),expectationReconciliations=0))
for lane in ['minimum-consumer','current-consumer']:assert len(cases(p/(lane+'-final34-results')))==2548
assert len(cases(p/'current-native-final34-results'))==65
for lane,count in [('legacy124',124),('alpha23',23),('beta33',33)]:assert len(cases(p/(lane+'-final34-results')))==count
expanded={};pilot={}
for lane in ['minimum','current']:
 deltas=[];counts=dict(match=0,unsupported=0,mismatch=0,harnessError=0)
 for cohort in [1,2]:
  before=read(old/f'expanded-{lane}-{cohort}/RESULTS.json');after=read(p/f'expanded-final-{lane}-{cohort}/RESULTS.json');assert len(before['rows'])==len(after['rows'])
  for a,b in zip(before['rows'],after['rows']):
   assert a['id']==b['id'];assert {k:v for k,v in a.items() if k not in ['native','classification']}=={k:v for k,v in b.items() if k not in ['native','classification']}
   if a!=b:deltas.append(dict(id=a['id'],before=a,after=b))
  for k,v in after['counts'].items():counts[k]+=v
 assert counts==dict(match=8,unsupported=121,mismatch=0,harnessError=0),counts
 expanded[lane]=dict(counts=counts,deltas=deltas,originalPayloadsAndExpectedOutcomesUnchanged=True,executionDate='2026-11-23',note='Unchanged upstream measurement scripts retain their historical 2026-11-22 report date field; invocation receipts are from 23 November.')
 before=read(old/'pilot/RESULTS.json');after=read(p/f'pilot-final-{lane}/RESULTS.json');assert before['rows']==after['rows'];assert after['counts']==dict(match=21,mismatch=0,unsupported=0,harnessError=0)
 pilot[lane]=dict(counts=after['counts'],deltas=[])
dump(p/'EXPANDED-DELTA.json',expanded);dump(p/'PILOT-DELTA.json',pilot)
identity=read(p/'IDENTITY-FINAL.json');assert identity['package']=='Bosak.Schema.0.6.0-alpha.0.34.nupkg'
with zipfile.ZipFile(p/'feed'/identity['package']) as z:
 assert z.read('NUGET_README.md')==(w/'docs/NUGET_README.md').read_bytes()
 assert hashlib.sha256(z.read('lib/net10.0/Bosak.Schema.dll')).hexdigest()==identity['schemaDllSha256']
for f in [w/'src/Bosak.Schema/bin/Release/net10.0/Bosak.Schema.dll',p/'current-native/bin/Release/net10.0/Bosak.Schema.dll',p/'readme-example/bin/Release/net10.0/Bosak.Schema.dll']:
 assert hashlib.sha256(f.read_bytes()).hexdigest()==identity['schemaDllSha256']
assert read(p/'SURFACE.json')['zeroPublicApiDelta']
dump(p/'RESULTS.json',dict(date='2026-11-23',base='aa07e801ad5f2d89ebccd7299a4f539800e20df1',sourceInternal=1191,sourcePublic=2548,sourceTotal=3739,preservedOriginalSourceCases=3620,addedSourceControls=119,packageConsumersPerCore=2548,currentPackageNative=65,retained=[124,23,33],publicTypes=54,publicRows=401,apiDelta=0,frozenMatches=21,expandedMatches=8,expandedUnsupported=121,corpusMismatches=0,corpusHarnessErrors=0,expectationReconciliations=0,allAdopted=False,nugetAuditDataFresh=False,localUnsignedPackage=identity['package'],packageSha256=identity['packageSha256'],schemaDllSha256=identity['schemaDllSha256'],published=False))
print(json.dumps(read(p/'RESULTS.json')));print('Expanded detail deltas:',{k:len(v['deltas']) for k,v in expanded.items()})
