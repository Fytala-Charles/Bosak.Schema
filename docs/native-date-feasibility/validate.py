"""Verify isolated revised date feasibility without launching binaries or altering earlier packets.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[1];sha=lambda b:hashlib.sha256(b).hexdigest();read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
packets=entries=0
for p in (ROOT/'docs').rglob('PACKET-SHA256.json'):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(60,3879),(packets,entries)
inputs=read(ROOT/'docs/date-time-transport/ENGINE-INPUTS.json');assert len(inputs)==323
for rel,h in inputs.items():assert sha((ROOT/rel).read_bytes())==h,rel
plan=read(P/'CANDIDATE-SOURCE.json');assert len(plan['files'])==61 and len(plan['overlays'])==5 and len(plan['newFiles'])==2
for row in plan['files']:
 b=subprocess.check_output(['git','show',plan['base']+':src/Bosak.Schema/'+row['path']],cwd=ROOT);assert sha(b)==row['baseGitSha256']
 if row['path'] in plan['overlays']:assert sha((P/'overlay'/row['path']).read_bytes())==row['candidateSha256']
 else:
  if row['baseTransform']=='LF-to-CRLF':b=b.replace(b'\n',b'\r\n')
  assert sha(b)==row['candidateSha256']
for rel,h in plan['newFiles'].items():assert sha((P/'overlay'/rel).read_bytes())==h
ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
def tests(path):
 d={}
 for f in path.rglob('*.trx'):
  for t in ET.parse(f).findall('.//t:UnitTestResult',ns):
   a=t.attrib;assert a['testName'] not in d,(f,a['testName']);d[a['testName']]=a['outcome']
 return d
old=tests(ROOT/'docs/integer-enumeration/implementation-evidence/source');assert len(old)==4146
for lane in ['minimum','current']:
 native=tests(P/lane/'final-native-results');now=tests(P/lane/'final-regression-results');assert len(native)==99 and set(native.values())=={'Passed'};assert len(now)==4147 and set(now.values())=={'Passed'}
 removed=set(old)-set(now);added=set(now)-set(old);assert len(removed)==1 and len(added)==2
 assert next(iter(removed)).replace('expected: Unsupported','expected: Invalid') in added
 assert len([n for n in added if 'NativeAllIdentityTests.PhysicalOriginalIdentity' in n and 'AL-008' in n])==1
 initial=tests(P/lane/'regression-results');assert len(initial)==4146 and list(initial.values()).count('Failed')==(9 if lane=='minimum' else 10)
for rel,h in read(P/'REGRESSION-INPUTS.json').items():assert sha((P/rel).read_bytes())==h
# Original XML, schema strings, IDs, upstream expected validities and all non-outcome metadata remain intact.
for change in read(P/'REGRESSION-RECONCILIATION.json')['changes']:
 rel=change['path']
 if not rel.endswith('.json'):continue
 original=json.loads(subprocess.check_output(['git','show',plan['base']+':'+rel],cwd=ROOT));new=read(P/'minimum/reconciled-inputs'/rel)
 def strip(x):
  if isinstance(x,list):return [strip(v) for v in x]
  if isinstance(x,dict):return {k:strip(v) for k,v in x.items() if k not in {'expectedCompile','expectedAssessment','proposedAssessment','unsupportedReason'}}
  return x
 assert strip(original)==strip(new),rel
api=read(P/'SURFACE.json');assert api['oldPublicTypes']==54 and api['currentPublicTypes']==55 and api['preservedRows']==401 and not api['removedOrChangedRows'] and len(api['addedRows'])==7
identity=read(P/'IDENTITY.json');assert len(identity['lanes'])==2
for lane in identity['lanes']:
 assert lane['corePackagesOnly'] and lane['freshCoreSignature'] and len(lane['core'])==9
 for c in lane['core']:assert c['signatureVerified'] and c['payloadIdentical']
a=read(P/'EXPERIMENT-ADOPTION.json');assert a['yearMinimum']==-9223372036854775807 and a['yearMaximum']==9223372036854775807
for s in a['sources']:assert sha(Path(s['reviewBodyPath']).read_bytes())==s['sha256']
original=read(ROOT/'docs/date-time-transport/CASE-PLAN.json');revised=read(P/'case-plan.json');assert len(revised['cases'])==37
for before,after in zip(original['cases'],revised['cases']):
 expected=dict(before)
 if before['id']=='DT-013':expected['proposedAssessment']='Unsupported'
 assert expected==after
assert len(read(P/'DOCUMENTATION-CHECKS.json')['pages'])==12
print('Verified60 prior packets/3879 entries,323 unchanged production inputs,63 exact candidate source identities,99 unique native controls and4147 reconciled source cases per pin; explicit9-semantic/current-version failures, one display rename/one eligible identity case, preserved original stimuli, seven additive API rows and18 signed Core payload identities.')
