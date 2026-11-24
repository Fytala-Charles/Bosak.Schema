"""Verify post-date coverage assessment without running proposed semantic cases.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,collections
P=Path(__file__).resolve().parent;ROOT=P.parents[1];sha=lambda b:hashlib.sha256(b).hexdigest()
def read(p):
 b=p.read_bytes();return json.loads(b.decode('utf-16') if b.startswith((b'\xff\xfe',b'\xfe\xff')) else b.decode('utf-8-sig'))
packets=entries=0
for p in (ROOT/'docs').rglob('PACKET-SHA256.json'):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(62,4134),(packets,entries)
i=read(P/'ENGINE-INPUTS.json');assert len(i)==328
for rel,h in i.items():assert sha((ROOT/rel).read_bytes())==h,rel
base='383914562d89d3f7fad3a6a60cf4e29884cce912';assert subprocess.check_output(['git','rev-parse','HEAD'],cwd=ROOT,text=True).strip()==base
ci=read(P/'MAIN-CI.json');assert ci['headSha']==base and ci['status']=='completed' and ci['conclusion']=='success'
assert read(P/'PR46-MERGE.json')['mergeCommit']['oid']==base
sample=read(P/'CURRENT-SAMPLE.json');assert not sample['newCorpusRun'] and sample['counts']=={'match':12,'unsupported':117,'mismatch':0,'harnessError':0} and sample['cases']==129
for rel,h in sample['receiptSha256'].items():assert sha((ROOT/rel).read_bytes())==h
minimum=[];current=[]
for lane,target in [('minimum',minimum),('current',current)]:
 for c in [1,2]:target.extend(read(ROOT/f'docs/native-date-implementation/expanded-{lane}-{c}/RESULTS.json')['rows'])
assert minimum==current and len(current)==129
ledger=read(P/'CALENDAR-LEDGER.json')['rows'];assert len(ledger)==9
assert collections.Counter(r['reviewedGap'] for r in ledger)=={'Delivered bounded date':2,'Date simple-content extension':2,'Mixed complex content':2,'Date enumeration':2,'Date range facet':1}
for r in ledger:
 assert sha((ROOT/r['schemaPath']).read_bytes())==r['schemaSha256'];assert sha((ROOT/r['instancePath']).read_bytes())==r['instanceSha256']
 original=next(x for x in current if x['id']==r['id'])
 assert all(r[k]==v for k,v in original.items())
 if r['reviewedGap']=='Mixed complex content':assert r['native']['reason']=='Attributes' and b'mixed="true"' in (ROOT/r['schemaPath']).read_bytes()
 if r['reviewedGap']=='Date simple-content extension':assert b'extension base="xsd:date"' in (ROOT/r['schemaPath']).read_bytes()
for r in read(P/'SOURCE-REVIEW.json')['sources']:
 b=(P/'reviewed-source'/r['path']).read_bytes();assert sha(b)==r['sha256'];assert b==subprocess.check_output(['git','show',base+':'+r['path']],cwd=ROOT)
for r in read(P/'SPECIFICATION-REVIEW.json')['sources']:assert sha(Path(r['reviewBodyPath']).read_bytes())==r['sha256']
rank=read(P/'RANKING.json');assert rank['rows'][0]['candidate']=='Bounded date simple-content extension' and all(r['admissionForecast'] is None for r in rank['rows'])
assert len(read(P/'DOCUMENTATION-CHECKS.json')['pages'])==16
changed=subprocess.check_output(['git','diff','--name-only'],cwd=ROOT,text=True).splitlines();assert not any(p.startswith(('src/','tests/','.github/')) for p in changed)
print('Verified62 prior packets/4134 entries;328 unchanged production inputs; exact merged-main/CI/source/normative pins; reused129-case receipts with12matches/117Unsupported; nine unchanged calendar rows/files split2delivered/2extension/2mixed/2enumeration/1bound; no new semantic execution/adoption or admission forecast.')
