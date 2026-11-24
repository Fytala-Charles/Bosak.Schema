"""Verify bounded production date qualification without launching binaries.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET,zipfile,re
P=Path(__file__).resolve().parent;ROOT=P.parents[1];sha=lambda b:hashlib.sha256(b).hexdigest();read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
packets=entries=0
for p in (ROOT/'docs').rglob('PACKET-SHA256.json'):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(61,3974),(packets,entries)
i=read(P/'PRODUCTION-INPUTS.json');assert (len(i['files']),len(i['changedExisting']),len(i['new']))==(328,14,5)
for rel,h in i['files'].items():assert sha((ROOT/rel).read_bytes())==h,rel
for rel in [*i['changedExisting'],*i['new']]:assert sha((P/'production-inputs'/rel).read_bytes())==i['files'][rel]
plan=read(P/'CANDIDATE-SOURCE.json');assert len(plan['files'])==63 and len(plan['overlays'])==7
for row in plan['files']:
 rel=row['path'];assert sha((ROOT/'src/Bosak.Schema'/rel).read_bytes())==row['checkoutSha256']
 if row['baseSha256'] is not None:assert sha(subprocess.check_output(['git','show',plan['base']+':src/Bosak.Schema/'+rel],cwd=ROOT))==row['baseSha256']
 if rel in plan['overlays']:
  b=(P/'overlay'/rel).read_bytes();assert sha(b)==row['sha256']
  original=(ROOT/'docs/native-date-feasibility/overlay'/rel).read_bytes()
  def body(b):
   lines=b.decode('utf-8-sig').splitlines();n=0
   while n<len(lines) and (not lines[n].strip() or lines[n].startswith('//')):n+=1
   return '\n'.join(lines[n:])
  assert body(original)==body(b),rel
 else:assert row['sha256']==row['baseSha256']
ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
def tests(path):
 d={}
 for p in path.rglob('*.trx'):
  for t in ET.parse(p).findall('.//t:UnitTestResult',ns):
   a=t.attrib;assert a['outcome']=='Passed';assert a['testName'] not in d;d[a['testName']]='Passed'
 return d
old=tests(ROOT/'docs/integer-enumeration/implementation-evidence/source');now=tests(P/'source-qualified');assert len(old)==4146 and len(now)==4246
removed=set(old)-set(now);added=set(now)-set(old);assert len(removed)==1 and len(added)==101
assert next(iter(removed)).replace('expected: Unsupported','expected: Invalid') in added
assert len([n for n in added if 'NativeAllIdentityTests.PhysicalOriginalIdentity' in n and 'AL-008' in n])==1
assert len([n for n in added if 'NativeDate' in n])==99
for lane,n in [('minimum-consumer-qualified',2938),('current-consumer-qualified',2938),('current-native',7),('legacy124',124),('alpha23',23),('beta33',33)]:assert len(tests(P/(lane+'-results')))==n
# Exact original schema/XML/IDs and upstream expectations, excluding reconciled outcome fields.
for change in read(P/'ADMISSIONS.json')['changes']:
 rel=change['path']
 if not rel.endswith('.json'):continue
 old=json.loads(subprocess.check_output(['git','show',plan['base']+':'+rel],cwd=ROOT));new=read(ROOT/rel)
 def strip(x):
  if isinstance(x,list):return [strip(v) for v in x]
  if isinstance(x,dict):return {k:strip(v) for k,v in x.items() if k not in {'expectedCompile','expectedAssessment','proposedAssessment','unsupportedReason'}}
  return x
 assert strip(old)==strip(new),rel
summary=read(P/'SUMMARY.json')
for lane in ['minimum','current']:
 assert read(P/('pilot-'+lane)/'RESULTS.json')['counts']=={'match':21,'mismatch':0,'unsupported':0,'harnessError':0}
 counts={};changes=[]
 for c in [1,2]:
  name=f'expanded-{lane}-{c}';r=read(P/name/'RESULTS.json');q=summary['corpus'][name];assert r['counts']==q['counts']
  for k,v in r['counts'].items():counts[k]=counts.get(k,0)+v
  for delta in q['changes']:
   a,b=delta['before'],delta['after'];assert a['id']==b['id'] and b in r['rows']
   for k in set(a)-{'native','classification'}:assert a[k]==b[k],k
   changes.append(delta)
 assert counts=={'match':12,'unsupported':117,'mismatch':0,'harnessError':0} and len(changes)==3
 assert sum(d['after']['classification']=='match' for d in changes)==2
 wildcard=next(d for d in changes if d['after']['classification']=='unsupported');assert wildcard['before']['native']['reason']=='ValueType' and wildcard['after']['native']['reason']=='DeclarationShape'
api=read(P/'SURFACE.json');assert api['oldPublicTypes']==54 and api['currentPublicTypes']==55 and api['preservedRows']==401 and len(api['addedRows'])==7 and not api['removedOrChangedRows']
identity=read(P/'IDENTITY-FINAL.json');assert identity['publishedBeta1SignatureAndPayloadVerified'] and identity['unsignedLocalCandidate']
for lane in identity['lanes'].values():
 assert lane['packageOnly'] and lane['schemaPayloadIdentical'] and len(lane['core'])==9
 for c in lane['core']:assert c['signatureVerified'] and c['payloadIdentical']
raw=ROOT/'../../.guard-tmp/schema-native-date-production-20261124';package=raw/'feed'/identity['package'];dll=identity['schemaDllSha256']
if package.exists():
 assert sha(package.read_bytes())==identity['packageSha256']
 with zipfile.ZipFile(package) as z:
  assert sha(z.read('lib/net10.0/Bosak.Schema.dll'))==dll
  assert any(z.read(n)==(ROOT/'docs/NUGET_README.md').read_bytes() for n in z.namelist() if n.lower().endswith('.md'))
 for lane in ['minimum-consumer','current-consumer','current-native','minimum-runner','current-runner','surface-probe','readme-example']:
  assert sha((raw/lane/'bin/Release/net10.0/Bosak.Schema.dll').read_bytes())==dll,lane
 assert sha((ROOT/'src/Bosak.Schema/bin/Release/net10.0/Bosak.Schema.dll').read_bytes())==dll
 for lane in ['legacy124','alpha23','beta33']:assert sha((raw/(lane+'-retained')/'Bosak.Schema.dll').read_bytes())==dll
readme=(ROOT/'docs/NUGET_README.md').read_bytes()
for name in ['example-date.xsd','example-date.xml','example-enum.xsd','example-enum.xml','example-all.xsd','example-all.xml']:assert (P/'readme-example'/name).read_bytes() in readme,name
assert len(read(P/'DOCUMENTATION-CHECKS.json')['pages'])==15
adoption=read(P/'PRODUCTION-ADOPTION.json');assert adoption['productionAdoption'] and adoption['yearMinimum']==-9223372036854775807
for s in adoption['sources']:assert sha(Path(s['reviewBodyPath']).read_bytes())==s['sha256']
print('Verified61 prior packets/3974 entries;328 production inputs,63 source identities and seven body-identical qualified overlays;4246 source/2938 consumers per pin/native7/retained124-23-33; explicit expectation/display reconciliation; exact two corpus admissions and one first-refusal change; additive seven-row API,18 signed Core payloads and unchanged beta; package/runtime/source DLL and exact README identities.')
