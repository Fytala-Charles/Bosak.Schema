from pathlib import Path
import json,subprocess,sys
D=Path(__file__).resolve().parent
ledger=json.loads((D/'CONTRACTS.json').read_text(encoding='utf-8'))['contracts']
assert len(ledger)==200 and len({r['id'] for r in ledger})==200
assert all(r['status']=='qualified' and r['tests'] and r['evidence'] for r in ledger)
for phase in ['atomic','composition']:subprocess.run([sys.executable,str(D/phase/'validate.py')],check=True)
print('All200behavioral contracts and both immutable qualification packets verify; no full-XSD/publication claim.')
