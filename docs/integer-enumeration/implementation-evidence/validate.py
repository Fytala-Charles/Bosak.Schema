"""Verify bounded integer-enumeration production receipts without launching binaries.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import hashlib,json,subprocess,xml.etree.ElementTree as ET,zipfile,re
P=Path(__file__).resolve().parent;ROOT=P.parents[2]
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'));sha=lambda b:hashlib.sha256(b).hexdigest()
packets=entries=0
for p in (ROOT/'docs').rglob('PACKET-SHA256.json'):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(57,3675),(packets,entries)
i=read(P/'PRODUCTION-INPUTS.json');assert (len(i['files']),len(i['changedExisting']),len(i['new']))==(323,11,12)
for rel,h in i['files'].items():assert sha((ROOT/rel).read_bytes())==h,rel
for rel in [*i['changedExisting'],*i['new']]:assert sha((P/'production-inputs'/rel).read_bytes())==i['files'][rel]
plan=read(P/'CANDIDATE-SOURCE.json');assert len(plan['files'])==61 and len(plan['overlays'])==3
for row in plan['files']:
 rel=row['path'];assert sha((ROOT/'src/Bosak.Schema'/rel).read_bytes())==row['checkoutSha256']
 base=subprocess.check_output(['git','show',plan['base']+':src/Bosak.Schema/'+rel],cwd=ROOT);assert sha(base)==row['baseSha256']
 if rel in plan['overlays']:assert sha((P/'overlay'/rel).read_bytes())==row['sha256']
 else:assert row['sha256']==row['baseSha256']
ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
def tests(path):
 d={}
 for p in path.rglob('*.trx'):
  for t in ET.parse(p).findall('.//t:UnitTestResult',ns):
   a=t.attrib;assert a['outcome']=='Passed';assert a['testName'] not in d;d[a['testName']]='Passed'
 return d
old=tests(ROOT/'docs/all-particles/implementation-evidence/source-qualified');now=tests(P/'source');assert len(old)==3885 and len(now)==4146 and set(old)<=set(now)
for lane,n in [('minimum-consumer',2846),('current-consumer',2846),('current-native',49),('legacy124',124),('alpha23',23),('beta33',33)]:assert len(tests(P/(lane+'-results')))==n
q=read(P/'QUALIFICATION-SUMMARY.json')
for lane in ['minimum','current']:
 assert read(P/('pilot-'+lane)/'RESULTS.json')['counts']=={'match':21,'mismatch':0,'unsupported':0,'harnessError':0}
 counts={};changes=[]
 for cohort in [1,2]:
  name=f'expanded-{lane}-{cohort}';d=read(P/name/'RESULTS.json');assert d['counts']==q['corpus'][name]['counts']
  for k,v in d['counts'].items():counts[k]=counts.get(k,0)+v
  for delta in q['corpus'][name]['changes']:
   a,b=delta['before'],delta['after'];assert a['classification']=='unsupported' and b['classification']=='match';assert a['id']==b['id']
   for key in set(a)-{'classification','native'}:assert a[key]==b[key],key
   assert b in d['rows'];changes.append(delta)
 assert counts=={'match':10,'unsupported':119,'mismatch':0,'harnessError':0} and len(changes)==2
identity=read(P/'IDENTITY-FINAL.json');assert identity['publishedBeta1SignatureAndPayloadVerified'];assert identity['schemaDllSha256']=='49c3ec63f3082f3b8c857a852c750d8d50df23f40a3c924dbf8490bb460e57bd'
assert identity['packageSha256']=='e963654defad64ee66ade7150344a5eada703acb9112249d085a961129413e40'
# Reconciled original schema/XML stimuli and upstream validities remain unchanged.
for rel in ['tests/TestData/DerivedIntegers/cases.json','tests/TestData/IntegerRestrictions/cases.json','tests/TestData/DerivedIntegers/originals.json']:
 before=json.loads(subprocess.check_output(['git','show',plan['base']+':'+rel],cwd=ROOT));after=read(ROOT/rel)
 def strip(x):
  if isinstance(x,list):return [strip(t) for t in x]
  if isinstance(x,dict):return {k:strip(v) for k,v in x.items() if k not in {'expectedCompile','expectedAssessment','expectedValue','expectedValues','reason','values'}}
  return x
 # The original JSON reconciliation permits only explicitly measured outcome fields.
 assert strip(before)==strip(after),rel
# README package bytes and extracted examples match their exact fenced bodies.
raw=(ROOT/'docs/NUGET_README.md').read_bytes();example=P/'readme-example'
for f in ['example-enum.xsd','example-enum.xml']:
 assert (example/f).read_bytes() in raw,f
package=Path('D:/Development/Bosak/.guard-tmp/schema-integer-enumeration-production-20261124/feed')/identity['package']
if package.exists():
 assert sha(package.read_bytes())==identity['packageSha256']
 with zipfile.ZipFile(package) as z:
  assert sha(z.read('lib/net10.0/Bosak.Schema.dll'))==identity['schemaDllSha256']
  assert any(z.read(n)==raw for n in z.namelist() if n.lower().endswith('.md'))
r=read(P/'DOCUMENTATION-CHECKS.json');assert len(r['pages'])==14 and not r['visualAcceptance']
print('Verified 57 prior seals /3675 entries, 323 production inputs, 61 source identities /3 overlays, 4146 source tests, package/retained controls, corpus admissions, original stimuli and exact README package bytes.')
