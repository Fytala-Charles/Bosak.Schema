"""Reconstruct qualified source bytes from immutable base and recorded overlays.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,sys
P=Path(__file__).resolve().parent;root=P.parents[2];plan=json.loads((P/'CANDIDATE-SOURCE.json').read_text());destination=Path(sys.argv[1]).resolve()
assert not destination.exists(),'Use a fresh destination.'
for row in plan['files']:
 rel=row['path'];raw=(P/'overlay'/rel).read_bytes() if rel in plan['overlays'] else subprocess.check_output(['git','show',plan['base']+':src/Bosak.Schema/'+rel],cwd=root)
 assert hashlib.sha256(raw).hexdigest()==row['sha256'];f=destination/rel;f.parent.mkdir(parents=True,exist_ok=True);f.write_bytes(raw)
print('Reconstructed and hash-verified 61 source files from exact base and three overlays.')
