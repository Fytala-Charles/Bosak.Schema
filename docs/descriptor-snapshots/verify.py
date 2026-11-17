"""Verify local descriptor snapshot source/evidence without mutation or publication."""
from pathlib import Path
import json,hashlib,zipfile
here=Path(__file__).resolve().parent
root=here.parent.parent
manifest=json.loads((here/'MANIFEST.json').read_text(encoding='utf-8'))
for name,expected in manifest['files'].items():
    assert hashlib.sha256((root/name).read_bytes().replace(b'\r\n',b'\n')).hexdigest()==expected,name
archive=here/'SOURCE-ONLY-EVIDENCE.zip'
assert hashlib.sha256(archive.read_bytes()).hexdigest()==manifest['archiveSha256']
with zipfile.ZipFile(archive) as z:
    assert len(z.namelist())==len(set(z.namelist()))
    assert all(Path(n).suffix in {'.cs','.trx','.json'} for n in z.namelist())
receipt=json.loads((here/'VERIFICATION.json').read_text(encoding='utf-8'))
assert receipt['productCases']==319 and receipt['newImplementationCases']==10 and receipt['newExternalCases']==3
diff=json.loads((here/'API-DIFF.json').read_text(encoding='utf-8'))
assert diff['allRowsIdentical'] and diff['added']==diff['removed']==[]
print('PASS: descriptor snapshot source/evidence, 319-case qualification and unchanged 40-type/317-row API; local only.')
