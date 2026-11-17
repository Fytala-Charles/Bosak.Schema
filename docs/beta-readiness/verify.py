"""Offline beta assessment/audit evidence verification; no acceptance or publication."""
from pathlib import Path
import json,hashlib,subprocess,zipfile
here=Path(__file__).resolve().parent
root=here.parent.parent
def sha(data): return hashlib.sha256(data.replace(b'\r\n',b'\n')).hexdigest()
for name,expected in json.loads((here/'MANIFEST.json').read_text(encoding='utf-8')).items():
    assert sha((root/name).read_bytes())==expected,name
basis=json.loads((here/'BASIS.json').read_text(encoding='utf-8'))
for name,expected in basis['inputs'].items():
    data=(root/name).read_bytes()
    if sha(data)!=expected:
        assert name in {'docs/ADR-009-native-decimal-session-profile.md','docs/ADR-010-owned-native-decimal-facade.md'}
        data=subprocess.check_output(['git','show',basis['base']+':'+name],cwd=root)
    assert sha(data)==expected,name
audit=json.loads((here/'CONTRACT-VERIFICATION.json').read_text(encoding='utf-8'))
for name,expected in audit['inputs'].items():
    data=(root/name).read_bytes()
    if sha(data)!=expected:
        assert name=='src/Bosak.Schema/SchemaAwareXslt.cs'
        data=subprocess.check_output(['git','show',audit['reviewBase']+':'+name],cwd=root)
    assert sha(data)==expected,name
assert not audit['engineSemanticsChanged'] and audit['exceptionsCaughtInsideProbe'] and audit['probeCases']==5
archive=here/'CONTRACT-EVIDENCE.zip'
assert hashlib.sha256(archive.read_bytes()).hexdigest()==audit['evidenceArchiveSha256']
with zipfile.ZipFile(archive) as z:
    assert set(z.namelist())=={'Program.cs','Audit.csproj','OBSERVATIONS.json','API.json'}
inventory=json.loads((here/'API-INVENTORY.json').read_text(encoding='utf-8'))
assert inventory['exportedTypes']==39 and inventory['declaredRows']==313 and inventory['allTypesClassified']
print('PASS: audit/report hashes, unchanged engine inputs, five caught observations and complete type classification; beta acceptance pending.')
