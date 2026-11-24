"""Verify isolated date extension feasibility without executing diagnostic binaries.
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
assert (packets,entries)==(64,4173),(packets,entries)
inputs=read(ROOT/'docs/date-simple-content/ENGINE-INPUTS.json');assert len(inputs)==328
for rel,h in inputs.items():assert sha((ROOT/rel).read_bytes())==h,rel
plan=read(P/'CANDIDATE-SOURCE.json');assert len(plan['files'])==63 and len(plan['overlays'])==3
for r in plan['files']:
 rel=r['path'];base=subprocess.check_output(['git','show',plan['base']+':src/Bosak.Schema/'+rel],cwd=ROOT);assert sha(base)==r['baseGitSha256']
 if rel in plan['overlays']:assert sha((P/'overlay'/rel).read_bytes())==r['candidateSha256']
 else:
  if r['baseTransform']=='LF-to-CRLF':base=base.replace(b'\n',b'\r\n')
  assert sha(base)==r['candidateSha256']==r['checkoutSha256']
ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
def tests(path):
 d={}
 for p in path.glob('*.trx'):
  for t in ET.parse(p).findall('.//t:UnitTestResult',ns):
   a=t.attrib;assert a['testName'] not in d;d[a['testName']]=a['outcome']
 return d
old=tests(ROOT/'docs/native-date-implementation/source-qualified');assert len(old)==4246
for lane in ['minimum','current']:
 a=tests(P/lane/'qualified-results');assert len(a)==196 and set(a.values())=={'Passed'}
 b=tests(P/lane/('reconciled-results' if lane=='minimum' else 'final-regression-results'));assert len(b)==4246 and set(b.values())=={'Passed'} and set(b)==set(old)
 initial=tests(P/lane/'initial-results');assert len(initial)==189 and list(initial.values()).count('Failed')==31
 regression=tests(P/lane/'regression-results');assert len(regression)==4246 and list(regression.values()).count('Failed')==(1 if lane=='minimum' else 2)
for rel,h in read(P/'REGRESSION-INPUTS.json').items():assert sha((P/rel).read_bytes())==h
changed=P/'minimum/reconciled-inputs/tests/Bosak.Schema.PublicConsumer.Tests/NativeDateConsumerTests.cs';text=changed.read_text(encoding='utf-8-sig');assert 'NativeSchemaSession.Create(complex,Options)' in text and 'admitted.Value?.Dispose();' in text
for t in ['"time"','"dateTime"','"dateTimeStamp"','"gYear"','"duration"']:assert t in text
assert (P/'case-plan.json').read_bytes()==(ROOT/'docs/date-simple-content/CASE-PLAN.json').read_bytes()
for r in read(P/'originals.json')['rows']:
 assert sha((P/r['schemaFile']).read_bytes())==r['schemaSha256']==sha((ROOT/r['schemaPath']).read_bytes())
 assert sha((P/r['instanceFile']).read_bytes())==r['instanceSha256']==sha((ROOT/r['instancePath']).read_bytes())
api=read(P/'SURFACE.json');assert api['oldPublicTypes']==api['currentPublicTypes']==55 and api['preservedRows']==408 and not api['addedRows'] and not api['removedOrChangedRows']
for lane in read(P/'IDENTITY.json')['lanes']:
 assert lane['corePackagesOnly'] and lane['freshCoreSignature'] and len(lane['core'])==9
 for c in lane['core']:assert c['signatureVerified'] and c['payloadIdentical']
for s in read(P/'EXPERIMENT-ADOPTION.json')['sources']:assert sha(Path(s['reviewBodyPath']).read_bytes())==s['sha256']
assert len(read(P/'DOCUMENTATION-CHECKS.json')['pages'])==16
print('Verified64 prior packets/4173entries and328 unchanged production inputs;63 source identities/three overlays;196native and4246 unchanged source names per pin; initial overlapping-operation diagnostic and single semantic/current-version reconciliations retained; exact original trials and contract bytes; API55/408zero delta and18signed Core identities. No full package/retained/corpus or production adoption claim.')
