from pathlib import Path
import hashlib,json
R=Path(__file__).resolve().parents[2];D=Path(__file__).resolve().parent
for rel,h in json.loads((D/'INPUTS.json').read_text()).items():assert hashlib.sha256((R/rel).read_bytes()).hexdigest()==h,rel
for x in json.loads((D/'PRIOR-SEALS.json').read_text()):
 seal=R/x['packet'];assert hashlib.sha256(seal.read_bytes()).hexdigest()==x['sha256']
 for rel,h in json.loads(seal.read_text(encoding='utf-8-sig')).items():assert hashlib.sha256((seal.parent/rel).read_bytes()).hexdigest()==h,str(seal.parent/rel)
review=json.loads((D/'CORE-SOURCE-REVIEW.json').read_text())
for s in review['sources']:assert hashlib.sha256((D/'reviewed-core-source'/s['path']).read_bytes()).hexdigest()==s['sha256']
d=json.loads((D/'OBSERVATIONS.json').read_text());assert d['observations']==48 and not d['errors'];assert all(x['passed'] for x in d['rows']);assert sum(x.get('exact',False) for x in d['rows'])==8
ident=json.loads((R/'docs/mixed-sequences/SUMMARY.json').read_text())['identity'];assert d['schemaDllSha256']==ident['schemaDllSha256'];assert d['coreDllSha256']==next(x['runtimeSha256'] for x in ident['core'] if x['id']=='Bosak.XPath.Core')
s=json.loads((R/'docs/schema-batch-20261124/STATUS.json').read_text());assert s['maximumSlices']==20 and len(s['slices'])==20;assert all(x['status']=='merged-main-verified' and x['mainCI'] for x in s['slices'][:19]);assert s['slices'][19]['productionSemanticImplementation']==False
print('Verified unchanged 396 inputs, all prior seals, exact Core review/runtime identities and 48 handled observations; batch capped at 20.')
