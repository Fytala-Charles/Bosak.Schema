"""Verify retained experiment evidence without rerunning native semantics.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,xml.etree.ElementTree as E
P=Path(__file__).resolve().parent;R=P.parents[1]
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
packets=entries=0
for p in (R/'docs').rglob('PACKET-SHA256.json'):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha(p.parent/rel)==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(67,4444),(packets,entries)
inputs=read(P/'ENGINE-INPUTS.json');assert len(inputs)==319
for rel,h in inputs.items():assert sha(R/rel)==h,rel
files=read(P/'CANDIDATE-SOURCE.json')['files'];assert len(files)==64 and sum(x['overlay'] for x in files)==3
for x in files:assert sha(P/'evidence/candidate'/x['path'])==x['candidateSha256']
for x in read(P/'EXPERIMENT-ADOPTION.json')['sources']:assert sha(Path(x['reviewBodyPath']))==x['sha256']
assert len(read(P/'case-plan.json')['cases'])==68
assert len(read(P/'PLAN-CORRECTIONS.json'))==2
for row in read(P/'case-plan.json')['cases']:E.fromstring(row['schema']);E.fromstring(row['instance'])
core=read(P/'IDENTITY.json')['core'];assert len(core)==9 and all(x['version']=='1.81.17' and x['signatureExitCode']==0 and x['identicalToPriorSignedArchive'] for x in core)
api=read(P/'SURFACE.json');assert api['zeroPublicApiDelta'] and api['currentPublicTypes']==55 and api['preservedRows']==408
ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
for lane,n in [('probe-reviewed-results',428),('internal-reviewed-results',1345),('public-reviewed-results',3097)]:
 fs=list((P/'receipts'/lane).glob('*.trx'));assert len(fs)==1
 results=E.parse(fs[0]).getroot().findall('.//t:UnitTestResult',ns);assert len(results)==n and all(x.attrib['outcome']=='Passed' for x in results)
names=read(P/'TEST-NAMES.json');assert names['sameMultisetOfOriginalDisplayNames'] and names['qualifiedCases']==4442
corpus=read(P/'CORPUS-COMPARISON.json');assert [x['cases'] for x in corpus]==[21,68,61] and [len(x['changedRows']) for x in corpus]==[0,0,2]
for lane in ['pilot','expanded1','expanded2']:
 rows=read(P/'evidence'/lane/'native.json')['rows'];assert all(x['error'] is None for x in rows)
print('Verified67prior packets/4444entries,319unchanged engine inputs,64candidate source files/3overlays,428native and4442regression passes with original display names, nine signed Core identities, zero API delta and exact21/129corpus changes. Source feasibility only.')
