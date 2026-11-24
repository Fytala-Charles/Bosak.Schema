"""Reconstruct qualified Windows checkout bytes from immutable base and date overlays.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,sys
P=Path(__file__).resolve().parent;ROOT=P.parents[1];plan=json.loads((P/'CANDIDATE-SOURCE.json').read_text(encoding='utf-8'));out=Path(sys.argv[1]).resolve();assert not out.exists(),'Use a fresh destination.'
sha=lambda b:hashlib.sha256(b).hexdigest()
for r in plan['files']:
 rel=r['path']
 if rel in plan['overlays']:b=(P/'overlay'/rel).read_bytes();assert sha(b)==r['sha256']
 else:
  b=subprocess.check_output(['git','show',plan['base']+':src/Bosak.Schema/'+rel],cwd=ROOT);assert sha(b)==r['baseSha256']==r['sha256']
  if sha(b)!=r['checkoutSha256']:b=b.replace(b'\n',b'\r\n')
 assert sha(b)==r['checkoutSha256'];f=out/rel;f.parent.mkdir(parents=True,exist_ok=True);f.write_bytes(b)
print('Reconstructed63 exact qualified checkout source files from immutable base and seven overlays/new files.')
