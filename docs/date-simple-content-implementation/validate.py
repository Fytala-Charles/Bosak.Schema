"""Verify bounded production date extension receipts without launching binaries.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET,zipfile
P=Path(__file__).resolve().parent;ROOT=P.parents[1];sha=lambda b:hashlib.sha256(b).hexdigest();read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
packets=entries=0
for p in (ROOT/'docs').rglob('PACKET-SHA256.json'):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(65,4251),(packets,entries)
i=read(P/'PRODUCTION-INPUTS.json');assert (len(i['files']),len(i['changedExisting']),len(i['new']))==(338,6,10)
for rel,h in i['files'].items():assert sha((ROOT/rel).read_bytes())==h,rel
for rel in [*i['changedExisting'],*i['new']]:assert sha((P/'production-inputs'/rel).read_bytes())==i['files'][rel]
plan=read(P/'CANDIDATE-SOURCE.json');assert len(plan['files'])==63 and len(plan['overlays'])==3
for row in plan['files']:
 rel=row['path'];assert sha((ROOT/'src/Bosak.Schema'/rel).read_bytes())==row['checkoutSha256']
 assert sha(subprocess.check_output(['git','show',plan['base']+':src/Bosak.Schema/'+rel],cwd=ROOT))==row['baseSha256']
 if rel in plan['overlays']:
  b=(P/'overlay'/rel).read_bytes();assert sha(b)==row['sha256']
  def body(b):
   lines=b.decode('utf-8-sig').splitlines();n=0
   while n<len(lines) and (not lines[n].strip() or lines[n].startswith('//')):n+=1
   return '\n'.join(lines[n:])
  assert body(b)==body((ROOT/'docs/date-simple-content-feasibility/overlay'/rel).read_bytes())
 else:assert row['sha256']==row['baseSha256']
ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
def tests(p):
 d={}
 for f in p.glob('*.trx'):
  for r in ET.parse(f).findall('.//t:UnitTestResult',ns):assert r.attrib['outcome']=='Passed';assert r.attrib['testName'] not in d;d[r.attrib['testName']]='Passed'
 return d
old=tests(ROOT/'docs/native-date-implementation/source-qualified');now=tests(P/'source');assert len(old)==4246 and len(now)==4442 and set(old)<=set(now)
assert len([x for x in set(now)-set(old) if 'NativeDateSimpleContent' in x])==196
for lane,n in [('minimum-consumer-final',3097),('current-consumer-final',3097),('current-native-final',37),('legacy124',124),('alpha23',23),('beta33',33)]:assert len(tests(P/(lane+'-results')))==n
source=ROOT/'tests/Bosak.Schema.PublicConsumer.Tests/NativeDateConsumerTests.cs';s=source.read_text(encoding='utf-8-sig');assert 'NativeSchemaSession.Create(complex,Options)' in s and 'admitted.Value?.Dispose();' in s
for name in ['"time"','"dateTime"','"dateTimeStamp"','"gYear"','"duration"']:assert name in s
fixture=ROOT/'tests/TestData/DateSimpleContent';assert (fixture/'case-plan.json').read_bytes()==(ROOT/'docs/date-simple-content/CASE-PLAN.json').read_bytes()
for r in read(fixture/'originals.json')['rows']:
 assert sha((fixture/r['schemaFile']).read_bytes())==r['schemaSha256']==sha((ROOT/r['schemaPath']).read_bytes())
 assert sha((fixture/r['instanceFile']).read_bytes())==r['instanceSha256']==sha((ROOT/r['instancePath']).read_bytes())
summary=read(P/'SUMMARY.json')
for lane in ['minimum','current']:
 assert read(P/f'pilot-final-{lane}/RESULTS.json')['counts']=={'match':21,'mismatch':0,'unsupported':0,'harnessError':0}
 total={};changes=[]
 for c in [1,2]:
  name=f'expanded-final-{lane}-{c}';r=read(P/name/'RESULTS.json');q=summary['corpus'][name];assert r['counts']==q['counts']
  for k,v in r['counts'].items():total[k]=total.get(k,0)+v
  for delta in q['changes']:
   a,b=delta['before'],delta['after'];assert a['classification']=='unsupported' and b['classification']=='match' and 'contenttype00201m' in b['id'];assert b in r['rows']
   for k in set(a)-{'classification','native'}:assert a[k]==b[k],k
   changes.append(delta)
 assert total=={'match':14,'unsupported':115,'mismatch':0,'harnessError':0} and len(changes)==2
api=read(P/'SURFACE.json');assert api['oldPublicTypes']==api['currentPublicTypes']==55 and api['preservedRows']==408 and not api['addedRows'] and not api['removedOrChangedRows']
id=read(P/'IDENTITY-FINAL.json');assert id['publishedBeta1SignatureAndPayloadVerified'] and id['unsignedLocalCandidate']
for lane in id['lanes'].values():
 assert lane['packageOnly'] and lane['schemaPayloadIdentical'] and len(lane['core'])==9
 for c in lane['core']:assert c['signatureVerified'] and c['payloadIdentical']
raw=ROOT/'../../.guard-tmp/schema-date-simple-content-production-20261124';package=raw/'feed'/id['package'];dll=id['schemaDllSha256']
if package.exists():
 assert sha(package.read_bytes())==id['packageSha256']
 with zipfile.ZipFile(package) as z:
  assert sha(z.read('lib/net10.0/Bosak.Schema.dll'))==dll
  assert any(z.read(n)==(ROOT/'docs/NUGET_README.md').read_bytes() for n in z.namelist() if n.lower().endswith('.md'))
 for lane in ['minimum-consumer','current-consumer','current-native','minimum-runner','current-runner','surface-probe','readme-example']:assert sha((raw/lane/'bin/Release/net10.0/Bosak.Schema.dll').read_bytes())==dll
 assert sha((ROOT/'src/Bosak.Schema/bin/Release/net10.0/Bosak.Schema.dll').read_bytes())==dll
 for lane in ['legacy124','alpha23','beta33']:assert sha((raw/(lane+'-retained')/'Bosak.Schema.dll').read_bytes())==dll
readme=(ROOT/'docs/NUGET_README.md').read_bytes();assert b'date simple-content extension/restriction' not in readme
for name in ['extension','date','enum','all']:
 for ext in ['xsd','xml']:assert (P/'readme-example'/('example-'+name+'.'+ext)).read_bytes() in readme
assert read(P/'PACKAGE-README-REVISION.json')['unchangedSchemaDllSha256']==dll
assert len(read(P/'DOCUMENTATION-CHECKS.json')['pages'])==16
for s in read(P/'PRODUCTION-ADOPTION.json')['sources']:assert sha(Path(s['reviewBodyPath']).read_bytes())==s['sha256']
print('Verified65 prior packets/4251entries;338 production inputs/63 source identities/three qualified overlays;4442source,3097consumers per pin/native37/retained124-23-33;196new controls and4246original names; two corpus admissions only; API55/408zero delta;18signed Core identities/beta unchanged; exact final README/package/runtime/source DLL identities.')
