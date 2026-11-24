"""Validate the calendar design packet without executing proposed semantic cases.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[1];sha=lambda b:hashlib.sha256(b).hexdigest()
def read(p):
 b=p.read_bytes();return json.loads(b.decode('utf-16') if b.startswith((b'\xff\xfe',b'\xfe\xff')) else b.decode('utf-8-sig'))
packets=entries=0
for p in (ROOT/'docs').rglob('PACKET-SHA256.json'):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(58,3830),(packets,entries)
inputs=read(P/'ENGINE-INPUTS.json');assert len(inputs)==323
for rel,h in inputs.items():assert sha((ROOT/rel).read_bytes())==h,rel
base='2eb999052188a872a54548c71c378d6a05a8cc3c'
assert subprocess.check_output(['git','rev-parse','HEAD'],cwd=ROOT,text=True).strip()==base
for s in read(P/'SOURCE-REVIEW.json')['sources']:
 b=(P/'reviewed-source'/s['lane']/s['path']).read_bytes();assert sha(b)==s['sha256']
 if s['lane'] in ['minimum','current'] and s['path'].endswith('XPathDateTime.cs'):
  assert b'public long Year' in b and b'public int Millisecond' in b
for s in read(P/'SPECIFICATION-REVIEW.json')['sources']:
 f=Path(s['reviewBodyPath']);assert sha(f.read_bytes())==s['sha256']
plan=read(P/'CASE-PLAN.json');cases=plan['cases'];assert len(cases)==37 and len({r['id'] for r in cases})==37
for r in cases:
 assert not r['executed'] and r['proposedCompile']=='Valid';assert r['proposedAssessment'] in ['Valid','Invalid','Unsupported'];ET.fromstring(r['schema']);ET.fromstring(r['instance'])
gates=read(P/'NATIVE-GATES.json')['gates'];assert len(gates)==18 and all(not g['executed'] for g in gates)
originals=read(P/'ORIGINAL-EXAMPLES.json');assert originals['count']==9 and len(originals['rows'])==9
for row in originals['rows']:assert sha((ROOT/row['schema']).read_bytes())==row['schemaSha256']
ci=read(P/'MAIN-CI.json');assert ci['headSha']==base and ci['conclusion']=='success'
assert read(P/'PR45-MERGE.json')['mergeCommit']['oid']==base
render=read(P/'DOCUMENTATION-CHECKS.json');assert len(render['pages'])==12 and not render['visualAcceptance']
changed=subprocess.check_output(['git','diff','--name-only'],cwd=ROOT,text=True).splitlines();assert not any(p.startswith(('src/','tests/','.github/')) for p in changed)
print('Verified 58 prior packets /3830 entries; all323 production inputs unchanged; exact Core/Schema source and normative identities;37 well-formed unexecuted XML plans,18 unexecuted native gates,9 unchanged original schemas and green merged-main receipt. No semantic cases executed.')
