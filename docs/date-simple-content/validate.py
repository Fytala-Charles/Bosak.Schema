"""Validate the date extension contract and XML plan without executing semantic cases.
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
assert (packets,entries)==(63,4154),(packets,entries)
inputs=read(P/'ENGINE-INPUTS.json');assert len(inputs)==328
for rel,h in inputs.items():assert sha((ROOT/rel).read_bytes())==h,rel
plan=read(P/'CASE-PLAN.json');rows=plan['cases'];assert len(rows)==77 and len({r['id'] for r in rows})==77
XS='{http://www.w3.org/2001/XMLSchema}'
for r in rows:
 assert not r['executed'] and r['proposedCompile'] in ['Valid','Unsupported','Invalid'];assert r['proposedAssessment'] in ['Valid','Invalid','Unsupported','NotRun']
 s=ET.fromstring(r['schema']);x=ET.fromstring(r['instance'])
 if r.get('expandedNodes') is not None:assert len(list(x.iter()))==r['expandedNodes']
 if r.get('declaredAttributeUses') is not None:assert len(list(s.iter(XS+'attribute')))==r['declaredAttributeUses']
 if r.get('globalTypes') is not None:assert len(s.findall(XS+'complexType'))==r['globalTypes']
 if r.get('expandedAttributeUses') is not None:
  c=s.find(XS+'complexType');uses=len(list(c.iter(XS+'attribute')));root=s.find(XS+'element');terminal=next(e for e in root.iter(XS+'element') if e is not root);maximum=int(terminal.attrib.get('maxOccurs','1'));rootUses=len(list(root.iter(XS+'attribute')));assert uses*maximum+rootUses==r['expandedAttributeUses']
  # Budget counts declared maxima even though these optional attributes are absent in XML.
  assert not any(e.attrib for e in x.iter())
gates=read(P/'NATIVE-GATES.json')['gates'];assert len(gates)==18 and all(not g['executed'] for g in gates)
originals=read(P/'ORIGINAL-EXAMPLES.json')['rows'];assert len(originals)==2
for r in originals:
 assert sha((ROOT/r['schemaPath']).read_bytes())==r['schemaSha256'];assert sha((ROOT/r['instancePath']).read_bytes())==r['instanceSha256'];assert r['upstreamExpected'] in ['valid','invalid'] and r['classification']=='unsupported'
for r in read(P/'SOURCE-REVIEW.json')['sources']:
 b=(P/'reviewed-source'/r['path']).read_bytes();assert sha(b)==r['sha256'];assert b==subprocess.check_output(['git','show',r['commit']+':'+r['path']],cwd=ROOT)
for r in read(P/'SPECIFICATION-REVIEW.json')['sources']:assert sha(Path(r['reviewBodyPath']).read_bytes())==r['sha256']
for r in read(P/'POTENTIAL-EXPECTATION-IMPACTS.json')['rows']:assert sha((ROOT/r['path']).read_bytes())==r['sourceSha256']
assert len(read(P/'DOCUMENTATION-CHECKS.json')['pages'])==16
print('Verified63 prior packets/4154 entries;328 unchanged production inputs;77 unique well-formed unexecuted XML proposals including exact declared budget boundaries;18 unexecuted native gates;two unchanged original trials and exact source/normative/static-impact hashes. No implementation/adoption or semantic execution.')
