"""Offline bounded beta qualification verification; no release/acceptance action."""
from pathlib import Path
import json,hashlib,zipfile,math
here=Path(__file__).resolve().parent
root=here.parent.parent
def sha(data): return hashlib.sha256(data.replace(b'\r\n',b'\n')).hexdigest()
manifest=json.loads((here/'MANIFEST.json').read_text(encoding='utf-8'))
for name,expected in manifest['files'].items():
    assert sha((root/name).read_bytes())==expected,name
pins=json.loads((here/'PRODUCT-PIN.json').read_text(encoding='utf-8'))
for name,expected in pins['files'].items():
    assert sha((root/name).read_bytes())==expected,name
archive=here/'SOURCE-ONLY-EVIDENCE.zip'
assert hashlib.sha256(archive.read_bytes()).hexdigest()==manifest['archiveSha256']
summary=json.loads((here/'MEASUREMENTS.json').read_text(encoding='utf-8'))
assert len(summary['runs'])==20 and len(summary['results'])==5
with zipfile.ZipFile(archive) as z:
    assert len(z.namelist())==len(set(z.namelist()))
    assert all(Path(n).suffix in {'.json','.log','.trx','.py','.cs','.csproj','.yml'} for n in z.namelist())
    for item in summary['runs']:
        raw=z.read('measurements/'+item['file'])
        assert hashlib.sha256(raw).hexdigest()==item['sha256']
        data=json.loads(raw)
        assert data['success'] and data['assertionsChecked']
    for row in summary['results']:
        assert row['withinProposedLocalEnvelope']==all(row[k]<=v for k,v in summary['thresholds'].items())
scope=json.loads((here/'SCOPE-DECISION.json').read_text(encoding='utf-8'))
assert scope['boundedBetaScopeSelected'] and not scope['betaPublicationAuthorized']
receipt=json.loads((here/'VERIFICATION.json').read_text(encoding='utf-8'))
assert receipt['productCases']==345 and receipt['workloadRunnerControls']==6 and not receipt['productionSla']
print('PASS: unchanged product inputs, final 20-process measurements, chosen scope and source-only evidence; beta acceptance/publication pending.')
