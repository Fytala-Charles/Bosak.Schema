"""Validate production receipts and hashes without rerunning historical validators.
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
assert (packets,entries)==(68,4599),(packets,entries)
for rel,h in read(R/'docs/core-18117-baseline/SHA256.json').items():assert sha(R/'docs/core-18117-baseline'/rel)==h
inputs=read(P/'PRODUCTION-INPUTS.json');assert len(inputs['inputs'])==329 and len(inputs['changedExisting'])==6 and len(inputs['added'])==10
for rel,h in inputs['inputs'].items():assert sha(R/rel)==h,rel
source=read(P/'CANDIDATE-SOURCE.json');assert source['sourceFiles']==64
for row in source['files']:
 assert sha(R/'src/Bosak.Schema'/row['path'])==row['qualifiedCheckoutSha256']
 assert sha(P/'qualified-source'/row['path'])==row['qualifiedCheckoutSha256']
for row in read(P/'PRODUCTION-ADOPTION.json')['sources']:assert sha(Path(row['reviewBodyPath']))==row['sha256']
assert read(P/'PRODUCTION-ADOPTION.json')['qualifiedPacketSha256']==sha(R/'docs/date-enumeration-feasibility/PACKET-SHA256.json')
identity=read(P/'IDENTITY.json');assert identity['finalReviewedArtifact'] and identity['packageCacheSourceRuntimesIdentical'] and len(identity['core'])==9
assert all(x['version']=='1.81.17' and x['signatureExitCode']==0 and x['identicalToPriorSignedArchive'] for x in identity['core'])
assert all(x['version']=='1.81.17' for x in identity['dependencies'])
api=read(P/'SURFACE.json');assert api['zeroPublicApiDelta'] and api['currentPublicTypes']==55 and api['preservedRows']==408
ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
for lane,n in [('source-reviewed',4874),('consumer-reviewed-results',3509),('native-reviewed-results',20)]:
 results=[]
 for path in (P/'evidence'/lane).glob('*.trx'):results+=E.parse(path).getroot().findall('.//t:UnitTestResult',ns)
 assert len(results)==n and all(x.attrib['outcome']=='Passed' for x in results),(lane,len(results))
retained=read(P/'RETAINED-RESULTS.json');assert len(retained)==5
assert sorted((x['total'],x['passed'],x['failed']) for x in retained)==sorted([(124,124,0),(23,22,1),(22,22,0),(33,32,1),(32,32,0)])
for x in retained:
 if x['failed']:assert x['failureClassification']=='Historical1.81.2identity assertion only' and not x['excludedHistoricalPinFact']
for x in read(P/'RETAINED-MIGRATION.json'):assert x['callerBinaryUnchanged'] and 'not Schema-only' in x['scope']
assert len(read(P/'GATE-EVIDENCE.json')['gates'])==20
corpus=read(P/'CORPUS-COMPARISON.json');assert [x['cases'] for x in corpus]==[21,68,61] and [len(x['changes']) for x in corpus]==[0,0,2]
for lane in ['pilot','expanded1','expanded2']:
 initial=read(P/'evidence'/lane/'native.json')['rows'];final=read(P/'evidence'/lane/'native-reviewed.json')['rows'];assert initial==final and all(x['error'] is None for x in final)
log=(P/'evidence/examples-reviewed.log').read_text(encoding='utf-16' if (P/'evidence/examples-reviewed.log').read_bytes().startswith(b'\xff\xfe') else 'utf-8-sig');assert 'Additional packaged gates pass' in log and 'date-enumeration example passes' in log
summary=read(P/'SUMMARY.json');assert summary['sourceNamesPreserved'] and summary['originalCases']==4442 and summary['addedControls']==432 and not summary['published']
print('Verified68prior packets/4599entries plus Core baseline seal;329inputs/64source files;4874source/3509package/20native passes; zero55/408API delta; exact21/129corpus; explicit retained124/22/32migration and two preserved old-pin failures. No publication.')
