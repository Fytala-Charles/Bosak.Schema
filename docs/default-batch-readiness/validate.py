from pathlib import Path
import json,hashlib
D=Path(__file__).resolve().parent;R=D.parent.parent
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
for name,digest in json.loads((D/'PACKET-SHA256.json').read_text()).items():assert sha(D/name)==digest,name
for name,digest in json.loads((D/'PROTECTED-INPUTS.json').read_text()).items():assert sha(R/name)==digest,name
e=json.loads((D/'EVIDENCE-BASIS.json').read_text());q=json.loads((R/'docs/simple-content-default-groups/QUALIFICATION.json').read_text());assert e['auditOnly'] and not e['semanticAdoption'] and not e['productChanges'] and not e['testChanges'] and not e['versionChanges'] and not e['publication'];assert not e['sourceSuite']['fresh'] and not e['packageSuite']['fresh'];assert e['runtime']['schema']==q['schemaDllSha256'] and e['runtime']['core']==q['core']
summary=json.loads((D/'AUDIT-SUMMARY.json').read_text());blockers=json.loads((D/'BLOCKERS.json').read_text())['rows'];assert len(blockers)==summary['unsupported'];assert len({(r['lane'],r['id']) for r in blockers})==len(blockers);assert len({r['schemaSha256'] for r in blockers})==summary['distinctSchemas'];assert summary['errors']==0
for r in blockers:
 for field in ['schema','instance']:assert sha(R/r[field])==r[field+'Sha256'],r['id']
 assert r['detailedPhase'] in ['compile','assessment']
for name,total in [('PILOT',21),('EXPANDED1',68),('EXPANDED2',61),('DATATYPE',598)]:
 r=json.loads((D/(name+'-RESULTS.json')).read_text());assert r['freshRun'] and len(r['rows'])==total;assert r['counts'].get('mismatch',0)==r['counts'].get('harnessError',0)==0;assert sum(r['counts'].values())==total
matrix=json.loads((D/'CURRENT-MATRIX.json').read_text());assert len(matrix['rows'])==28 and all(not r['fullyConformant'] for r in matrix['rows']);assert matrix['normativeDenominator'] is None and matrix['conformancePercentage'] is None;assert not matrix['qualification']['freshSourceSuiteRun'] and not matrix['qualification']['freshPackageSuiteRun'] and matrix['qualification']['freshSelectedRefusalRun']
print('PASS: fresh audit, reused suite boundaries, runtime identities, original inputs and sealed packet')
