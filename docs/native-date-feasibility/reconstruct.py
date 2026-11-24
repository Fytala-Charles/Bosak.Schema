"""Reconstruct the exact isolated date source tree, without building or launching it.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib,subprocess,sys
P=Path(__file__).resolve().parent;ROOT=P.parents[1];plan=json.loads((P/'CANDIDATE-SOURCE.json').read_text(encoding='utf-8'));out=Path(sys.argv[1]).resolve();assert not out.exists(),'Use a fresh destination.'
for r in plan['files']:
 rel=r['path']
 if rel in plan['overlays']:b=(P/'overlay'/rel).read_bytes()
 else:
  b=subprocess.check_output(['git','show',plan['base']+':src/Bosak.Schema/'+rel],cwd=ROOT);assert hashlib.sha256(b).hexdigest()==r['baseGitSha256']
  if r['baseTransform']=='LF-to-CRLF':b=b.replace(b'\n',b'\r\n')
 assert hashlib.sha256(b).hexdigest()==r['candidateSha256'];f=out/rel;f.parent.mkdir(parents=True,exist_ok=True);f.write_bytes(b)
for rel,h in plan['newFiles'].items():
 b=(P/'overlay'/rel).read_bytes();assert hashlib.sha256(b).hexdigest()==h;f=out/rel;f.parent.mkdir(parents=True,exist_ok=True);f.write_bytes(b)
print('Reconstructed63 exact candidate source files from immutable base, five overlays and two new files.')
