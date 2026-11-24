"""Check contract integrity and plan syntax; never execute date validation.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import hashlib,json,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent; R=P.parents[1]
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
packets=entries=0
for p in (R/'docs').rglob('PACKET-SHA256.json'):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha(p.parent/rel)==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(66,4432),(packets,entries)
for rel,h in read(P/'ENGINE-INPUTS.json').items():assert sha(R/rel)==h,rel
rows=read(P/'CASE-PLAN.json')['cases'];assert len(rows)==68 and len({x['id'] for x in rows})==68
for row in rows:
 assert row['executed'] is False
 ET.fromstring(row['schema']);ET.fromstring(row['instance'])
 assert row['proposedCompile'] in ['Valid','Invalid','Unsupported']
 assert row['proposedAssessment'] in ['Valid','Invalid','Unsupported','NotRun']
 if row['proposedCompile']!='Valid':assert row['proposedAssessment']=='NotRun'
for x in read(P/'SPECIFICATION-REVIEW.json')['sources']:assert sha(Path(x['reviewBodyPath']))==x['sha256']
originals=read(P/'ORIGINAL-EXAMPLES.json')['rows'];assert len(originals)==2
for row in originals:
 assert not row['executedForThisContract']
 for kind in ['schema','instance']:assert sha(R/row[kind+'Path'])==row[kind+'Sha256']
 assert '<!--' in (R/row['instancePath']).read_text(encoding='utf-8-sig')
gates=read(P/'NATIVE-GATES.json')['gates'];assert len(gates)==20 and all(not x['executed'] for x in gates)
print('Verified 66 prior packets /4432 unchanged sealed entries, exact current engine inputs, 68 unique well-formed unexecuted XML plans, 20 unexecuted gates, immutable normative hashes and two unchanged original trials. No semantic execution or adoption.')
