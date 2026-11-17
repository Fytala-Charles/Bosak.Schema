"""Verify local diagnostic candidate hashes; no network or publication."""
from pathlib import Path
import hashlib,json,zipfile
here=Path(__file__).resolve().parent
root=here.parent.parent
for name,expected in json.loads((here/'MANIFEST.json').read_text(encoding='utf-8')).items():
    actual=hashlib.sha256((root/name).read_bytes().replace(b'\r\n',b'\n')).hexdigest()
    assert actual==expected,name
receipt=json.loads((here/'VERIFICATION.json').read_text(encoding='utf-8'))
assert not receipt['eligibleArtifact'] and receipt['publisherSkipped'] and not receipt['allowlistChanged']
assert hashlib.sha256((here/'FAILURE-EVIDENCE.zip').read_bytes()).hexdigest()==receipt['failureEvidenceSha256']
with zipfile.ZipFile(here/'FAILURE-EVIDENCE.zip') as z:
    assert len(z.namelist())==len(set(z.namelist()))
    assert all(Path(n).suffix in {'.json','.log','.trx'} for n in z.namelist())
print('PASS: diagnostic source/evidence hashes; failed run remains ineligible; no allowlist change.')
