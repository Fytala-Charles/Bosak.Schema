"""Offline bounded contract/compatibility review verification; no acceptance or release action."""
from pathlib import Path
import json,hashlib,zipfile
here=Path(__file__).resolve().parent
root=here.parent.parent
def sha(data): return hashlib.sha256(data.replace(b'\r\n',b'\n')).hexdigest()
manifest=json.loads((here/'MANIFEST.json').read_text(encoding='utf-8'))
for name,expected in manifest['files'].items():
    assert sha((root/name).read_bytes())==expected,name
for name,expected in json.loads((here/'PRODUCT-PIN.json').read_text(encoding='utf-8'))['files'].items():
    assert sha((root/name).read_bytes())==expected,name
for name,record in json.loads((here/'ORIGINAL-ADRS.json').read_text(encoding='utf-8')).items():
    data=(here/record['copy']).read_bytes().replace(b'\r\n',b'\n')
    assert sha(data)==record['sourceSha256Lf']
    assert hashlib.sha1(b'blob '+str(len(data)).encode()+b'\0'+data).hexdigest()==record['sourceGitBlob']
for name in ['ADR-009-native-decimal-session-profile.md','ADR-010-owned-native-decimal-facade.md']:
    assert '**Status:** Accepted — bounded Schema beta contract' in (root/'docs'/name).read_text(encoding='utf-8')
package=json.loads((here/'PACKAGE.json').read_text(encoding='utf-8'))
assert package['version']=='0.5.0-alpha.0.14' and not package['betaReleaseCandidate'] and not package['published']
assert package['sha256']=='61b2a4b59f780d3d71d2e2676a3443103ee23f37fde0aeb5747f501ef2ce323c'
retained=json.loads((here/'RETAINED.json').read_text(encoding='utf-8'))
assert [r['caseCount'] for r in retained['lanes']]==[124,23]
assert all(r['onlySchemaDllChanged'] and r['sameCases'] for r in retained['lanes'])
archive=here/'SOURCE-ONLY-EVIDENCE.zip'
assert hashlib.sha256(archive.read_bytes()).hexdigest()==manifest['archiveSha256']
with zipfile.ZipFile(archive) as z:
    assert len(z.namelist())==len(set(z.namelist()))
    assert all(Path(n).suffix in {'.trx','.json','.log'} for n in z.namelist())
acceptance=json.loads((here/'VERIFICATION.json').read_text(encoding='utf-8'))['ownerAcceptance']
assert acceptance['boundedSchemaContractAccepted'] and not acceptance['generalCoreAccepted']
assert not acceptance['betaTagAuthorized'] and not acceptance['betaPublicationAuthorized']
print('PASS: owner-accepted bounded contracts, original ADR blobs and exact compatibility evidence; no beta release authorization.')
