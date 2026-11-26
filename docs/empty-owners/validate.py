"""Verify empty-owner checkpoint evidence and qualified source identities offline."""
from pathlib import Path
import hashlib
import json

D = Path(__file__).resolve().parent
R = D.parent.parent
def digest(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()
for name, expected in json.loads((D / 'PACKET-SHA256.json').read_text(encoding='utf-8')).items():
    assert digest(D / name) == expected, name
for name, expected in json.loads((D / 'BUILD-CONTEXT.json').read_text(encoding='utf-8')).items():
    raw = (R / name).read_bytes()
    if hashlib.sha256(raw).hexdigest() != expected:
        # Repository text normalization is permitted; original qualified bytes remain hashed.
        assert hashlib.sha256(raw.replace(b'\n', b'\r\n')).hexdigest() == expected or hashlib.sha256(raw.replace(b'\r\n', b'\n')).hexdigest() == expected, name
q = json.loads((D / 'QUALIFICATION.json').read_text(encoding='utf-8'))
assert q['sourcePassed'] == 21870 and q['packagePublicPassed'] == 20191
assert q['priorSourceNamesPreserved'] == 21730 and q['newSourceNames'] == 140
assert len(q['core']) == 9
api = json.loads((D / 'API-REVIEW.json').read_text(encoding='utf-8'))
assert api['exportedTypes'] == 55 and api['candidateRows'] == 663
assert api['added'] == api['removed'] == []
contracts = json.loads((D / 'CONTRACTS.json').read_text(encoding='utf-8'))
assert contracts['count'] == len(contracts['contracts']) == 66
assert len({r['id'] for r in contracts['contracts']}) == 66
for lane in ['PILOT', 'EXPANDED1', 'EXPANDED2']:
    counts = json.loads((D / (lane + '-RESULTS.json')).read_text(encoding='utf-8'))['counts']
    assert counts['mismatch'] == counts['harnessError'] == 0
assert json.loads((D / 'DATATYPE-RESULTS.json').read_text(encoding='utf-8'))['counts'] == {'match': 568, 'unsupported': 30}
trial = json.loads((D / 'APPLICATION-TRIALS.json').read_text(encoding='utf-8'))
for name, expected in trial['originalFixtureHashes'].items():
    assert digest(D / 'invoice' / name) == expected, name
print('Empty-owner packet, qualified source, 66 contracts, frozen corpus outcomes and original invoice identities verified.')
