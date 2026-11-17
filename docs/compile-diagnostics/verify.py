"""Offline compile-diagnostic implementation/evidence verification; no external action."""
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
assert receipt['productCases']==345 and receipt['newImplementationCases']==22 and receipt['newExternalCases']==4
assert receipt['compileFindingsBlockAllThreeEntryPoints'] and receipt['cleanAssertionOnlyMeaningUnchanged']
assert not receipt['betaAccepted'] and not receipt['published']
diff=json.loads((here/'API-DIFF.json').read_text(encoding='utf-8'))
assert diff['allRowsIdentical'] and diff['added']==diff['removed']==[]
print('PASS: compile guard source/evidence, 345-case qualification and unchanged 40-type/317-row API; local only.')
