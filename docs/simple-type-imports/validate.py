"""Verify slice15evidence without executing sealed historical validators.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import hashlib,json,xml.etree.ElementTree as E
P=Path(__file__).resolve().parent;R=P.parents[1];read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'));sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
for rel,h in read(P/'PRIOR-SEALS.json').items():
 p=R/rel;assert sha(p)==h
 for name,digest in read(p).items():assert sha(p.parent/name)==digest
for rel,h in read(P/'INPUTS.json').items():assert sha(R/rel)==h,rel
for x in read(P/'ADOPTION.json')['sources']:assert sha(Path(x['reviewBodyPath']))==x['sha256']
ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
for lane,count in [('source-final',8998),('consumer-results',7558),('native-results',95)]:
 rows=[]
 for p in (P/'evidence'/lane).glob('*.trx'):rows+=E.parse(p).getroot().findall('.//t:UnitTestResult',ns)
 assert len(rows)==count and all(x.attrib['outcome']=='Passed' for x in rows)
s=read(P/'SUMMARY.json');assert s['api']['expectedAdditiveApiDelta'] and len(s['api']['addedRows'])==2 and not s['api']['removedOrChangedRows'] and s['originalNamesPreserved']==8788 and not s['published']
assert sum(x['counts']['mismatch']+x['counts']['harnessError'] for x in s['corpus'])==0
assert sum(x['cases'] for x in s['corpus'] if x['lane']!='pilot')==129
assert all(x['signatureExitCode']==0 for x in s['identity']['core'])
print('Verified84prior sealed packets, exact current inputs,8998source/7558package/95native passes, original8788names, reviewed two-row additive API delta and21/129corpus outcomes; retained migration boundary explicit.')
