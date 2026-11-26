"""Verify frozen upstream bytes and reconcile baseline outcomes offline."""
from pathlib import Path
import collections
import hashlib
import json

D = Path(__file__).resolve().parent
manifest = json.loads((D / 'datatype-cohort/CASES.json').read_text(encoding='utf-8'))
results = json.loads((D / 'datatype-cohort/BASELINE-RESULTS.json').read_text(encoding='utf-8'))
for name, digest in manifest['files'].items():
    assert hashlib.sha256((D / 'datatype-cohort/upstream' / name).read_bytes()).hexdigest() == digest, name
cases = {row['id']: row for row in manifest['cases']}
assert len(cases) == len(manifest['cases']) == 598
assert {row['id'] for row in results['rows']} == set(cases)
assert len(results['rows']) == len(cases)
counts = collections.Counter()
for row in results['rows']:
    case = cases[row['id']]
    assert row['expected'] == case['upstreamExpected']
    assert row['family'] == case['bucket']
    assert row['actual']['error'] is None
    if row['classification'] == 'match':
        assert row['actual']['compile'] == 'Valid'
        assert row['actual']['assessment'].lower() == case['upstreamExpected']
    else:
        assert row['classification'] == 'unsupported'
        assert 'Unsupported' in (row['actual']['compile'], row['actual']['assessment'])
    counts[row['classification']] += 1
assert dict(counts) == results['counts'] == {'match': 454, 'unsupported': 144}
assert len(results['core']) == 9
print('598 original fixture identities and outcomes verified: 454 match / 144 Unsupported / zero errors.')
