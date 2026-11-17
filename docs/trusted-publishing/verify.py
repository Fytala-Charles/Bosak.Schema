"""Offline verification of local trusted-publishing evidence; performs no login/push."""
import hashlib, json, pathlib, zipfile
here = pathlib.Path(__file__).resolve().parent
root = here.parent.parent
def sha(data): return hashlib.sha256(data).hexdigest()
manifest = json.loads((here / 'MANIFEST.json').read_text(encoding='utf-8'))
for name, expected in manifest['files'].items():
    data = (root / name).read_bytes().replace(b'\r\n', b'\n')
    assert sha(data) == expected, name
evidence = json.loads((here / 'EVIDENCE-MANIFEST.json').read_text(encoding='utf-8'))
archive = here / 'SOURCE-ONLY-EVIDENCE.zip'
assert sha(archive.read_bytes()) == evidence['archiveSha256']
with zipfile.ZipFile(archive) as zipped:
    assert len(zipped.namelist()) == len(set(zipped.namelist()))
    assert set(zipped.namelist()) == set(evidence['entries'])
    for name, expected in evidence['entries'].items():
        assert pathlib.Path(name).suffix in {'.py','.cjs','.yml','.log','.trx','.json'}, name
        assert sha(zipped.read(name)) == expected, name
receipt = json.loads((here / 'VERIFICATION.json').read_text(encoding='utf-8'))
assert (receipt['productTests'],receipt['pythonControls'],receipt['nodeControls']) == (272,19,22)
assert receipt['hostedRun'] is None and receipt['loginOrPushExecuted'] is False
rehearsal = json.loads((here / 'REHEARSAL.json').read_text(encoding='utf-8'))
assert rehearsal['success'] and not rehearsal['eligibleForPublication']
assert rehearsal['packageSha256'] == receipt['originalPackageSha256']
print('PASS: source hashes, source-only evidence archive, 272 product/41 control receipts; no hosted/publication claim.')
