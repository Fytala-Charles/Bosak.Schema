"""Verify correction source/evidence offline; never admits a failed hosted run."""
from pathlib import Path
import json,hashlib,zipfile
here=Path(__file__).resolve().parent
root=here.parent.parent
for name,expected in json.loads((here/'MANIFEST.json').read_text(encoding='utf-8')).items():
    assert hashlib.sha256((root/name).read_bytes().replace(b'\r\n',b'\n')).hexdigest()==expected,name
evidence=json.loads((here/'EVIDENCE.json').read_text(encoding='utf-8'))
assert hashlib.sha256((here/'SOURCE-ONLY-EVIDENCE.zip').read_bytes()).hexdigest()==evidence['sha256']
with zipfile.ZipFile(here/'SOURCE-ONLY-EVIDENCE.zip') as z:
    assert len(z.namelist())==len(set(z.namelist()))
    assert all(Path(n).suffix in {'.log','.json','.trx'} for n in z.namelist())
receipt=json.loads((here/'VERIFICATION.json').read_text(encoding='utf-8'))
assert receipt['success'] and not receipt['hostedRunAdmissible'] and not receipt['publicationExecuted']
assert receipt['package']['sha256']=='b88a25bdaa8f59294aaac775b3c11c79bb442c858943eb73c629a5d081d03a45'
assert receipt['pythonControls']==23 and receipt['nodeControls']==22
print('PASS: correction source/evidence hashes; actual archive locally verifies; failed run remains ineligible.')
