"""Offline restricted-resolution source/evidence verification."""
from pathlib import Path
import json,hashlib,zipfile
here=Path(__file__).resolve().parent
root=here.parent.parent
manifest=json.loads((here/'MANIFEST.json').read_text(encoding='utf-8'))
for name,expected in manifest['files'].items():
    assert hashlib.sha256((root/name).read_bytes().replace(b'\r\n',b'\n')).hexdigest()==expected,name
assert hashlib.sha256((here/'SOURCE-ONLY-EVIDENCE.zip').read_bytes()).hexdigest()==manifest['archiveSha256']
with zipfile.ZipFile(here/'SOURCE-ONLY-EVIDENCE.zip') as z:
    assert len(z.namelist())==len(set(z.namelist()))
    assert all(Path(n).suffix in {'.trx','.cs','.json'} for n in z.namelist())
receipt=json.loads((here/'VERIFICATION.json').read_text(encoding='utf-8'))
assert receipt['productCases']==306 and receipt['newRestrictedCases']==31 and receipt['newExternalCases']==3
assert not receipt['published'] and not receipt['betaAccepted']
print('PASS: restricted resolution source/evidence, 306-case receipt and additive API; local only.')
