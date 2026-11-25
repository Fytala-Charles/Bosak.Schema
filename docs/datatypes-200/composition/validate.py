from pathlib import Path
import hashlib
import json
import xml.etree.ElementTree as ET
import zipfile

D = Path(__file__).resolve().parent

def read(name):
    return json.loads((D / name).read_text(encoding='utf-8'))

def sha(data):
    return hashlib.sha256(data).hexdigest()

for name, digest in read('PACKET-SHA256.json').items():
    assert sha((D / name).read_bytes()) == digest, name
q = read('QUALIFICATION.json')
assert q['sourcePassed'] == 12907 and q['sourceInternal'] == 1551
assert q['sourcePublic'] == q['packagePublicPassed'] == 11356
assert q['overlappingCoverage'] and q['unsignedLocalOnly'] and not q['newSchemaPublished']
assert len(q['core']) == 9 and all(c['version'] == '1.86.0' and c['signatureExitCode'] == 0 for c in q['core'])
api = read('API-REVIEW.json')
assert api['exportedTypes'] == 55 and not api['removed'] and len(api['added']) == 6
with zipfile.ZipFile(D / 'QUALIFICATION-EVIDENCE.zip') as archive:
    for name, digest in q['sourceInputs'].items():
        assert sha(archive.read('qualified-source/' + name)) == digest, name
    assert sha(archive.read('packaged-NUGET_README.md')) == q['packageReadmeSha256']
    for folder, expected in [('composition-qualified-source-results', 12907), ('composition-qualified-package-results', 11356)]:
        counters = [ET.fromstring(archive.read(n)).find('.//{*}Counters').attrib for n in archive.namelist() if n.startswith(folder + '/') and n.endswith('.trx')]
        assert counters and all(c['failed'] == '0' for c in counters)
        assert sum(int(c['passed']) for c in counters) == expected
    for name, digest in q['buildContext'].items():
        assert sha(archive.read('build-context/' + name)) == digest, name
    for name, digest in q['historicalInputs'].items():
        assert sha(archive.read('historical-fixtures/' + name)) == digest, name
    cases = json.loads(archive.read('contract-tests/cases.json'))['cases']
    assert len(cases) == 252 and {r['contract'] for r in cases} == {'DT-' + str(i) for i in range(181, 301)}
    for lane in ['pilot', 'expanded1', 'expanded2']:
        result = json.loads(archive.read('corpus/' + lane + '/RESULTS.json'))
        assert result['counts']['mismatch'] == result['counts']['harnessError'] == 0
print('Composition120 source/package evidence, selected-member proof and additive API verify offline.')
