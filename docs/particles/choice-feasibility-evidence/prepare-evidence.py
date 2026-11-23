from pathlib import Path
import json,hashlib,shutil,xml.etree.ElementTree as ET
r=Path('.guard-tmp/schema-choice-feasibility-20261123'); w=Path('.worktrees/Schema.Particles'); e=w/'docs/particles/choice-feasibility-evidence';e.mkdir(exist_ok=True)
sha=lambda b:hashlib.sha256(b).hexdigest()
read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
base=read(r/'BASE-SOURCE.json'); rows=[]; overlays=[]
for f in sorted((r/'candidate').rglob('*.cs')):
 rel=f.relative_to(r/'candidate').as_posix(); h=sha(f.read_bytes()); old=base['files'].get(rel)
 rows.append(dict(path=rel,baseSha256=old,sha256=h))
 if h!=old:
  overlays.append(rel); out=e/'overlay'/rel;out.parent.mkdir(parents=True,exist_ok=True);shutil.copyfile(f,out)
assert len(rows)==60 and len(overlays)==4
(e/'CANDIDATE-SOURCE.json').write_text(json.dumps(dict(base=base['base'],files=rows,overlays=overlays),indent=2)+'\n',encoding='utf-8')
for f in r.iterdir():
 if f.is_file():shutil.copyfile(f,e/f.name)
for name in ['minimum','current','baseline-minimum','baseline-current','regression','surface','signatures','iterations']:
 for f in (r/name).rglob('*'):
  if not f.is_file() or any(p in ['bin','obj','schema','data','TestData'] for p in f.relative_to(r/name).parts):continue
  if name=='regression' and f.suffix=='.cs':continue
  out=e/f.relative_to(r);out.parent.mkdir(parents=True,exist_ok=True);shutil.copyfile(f,out)
p=w/'docs/derived-integer-types/feasibility-evidence/reconstruct.py'
s=p.read_text(encoding='utf-8').replace('==59','==60').replace('Verified59candidate source hashes from58Gitblobs plus3explicit overlays','Verified60candidate source hashes from59Gitblobs plus4explicit overlays').replace('ROOT=P.parents[2]','ROOT=P.parents[3]')
(e/'reconstruct.py').write_text(s,encoding='utf-8')
ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
def results(files):
 out={}
 for f in files:
  for t in ET.parse(f).findall('.//t:UnitTestResult',ns):
   assert t.attrib['outcome']=='Passed',(str(f),t.attrib)
   name=t.attrib['testName'];assert name not in out,name;out[name]=t.attrib['outcome']
 return out
prior=results((w/'docs/derived-integer-types/implementation-evidence/source').rglob('*.trx'))
now=results((r/'regression/results').rglob('*.trx'))
assert len(now)==len(prior)==3620 and now==prior,(len(now),len(prior))
(e/'SOURCE-REGRESSION.json').write_text(json.dumps(dict(count=3620,exactTestNamesAndOutcomesMatch=True,expectationReconciliations=0,testNames=sorted(now)),indent=2)+'\n',encoding='utf-8')
for lane in ['minimum','current']:
 assert len(results((r/lane/'results/verified').glob('*.trx')))==119
for p in w.glob('docs/**/00COPYRIGHT*'):
 shutil.copyfile(p,e/p.name);break
note='''# Bounded choice feasibility — 23 November 2026

PR [#42](https://github.com/Fytala-Charles/Bosak.Schema/pull/42) merged as `aa07e801ad5f2d89ebccd7299a4f539800e20df1`. Main CI run `45139587221` passed all 3,620 source tests. The earlier [contract packet](README.md) preserves its pre-merge state.

The isolated native `xs:choice` candidate passes **119 controls on each signed Core pin**, 1.81.2 and 1.81.17. Its unchanged source suite passes **1,126 internal + 2,494 public tests**, with exactly the prior test names and outcomes and no expectation changes. Public API comparison retains **54 types / 401 rows**, with zero delta. Production source, tests, build inputs, packages and published beta remain unchanged.

The subset supports one root choice, outer minOccurs 0 or 1 and maxOccurs 1, with 1–16 direct scalar alternatives, each minOccurs 1 and maxOccurs 1–16. Selection uses expanded names and actual physical child order; alternatives cannot mix. Empty optional choice preserves an element-only root with no typed value. Anonymous scalar restrictions, named root types, existing root attributes and structural namespace assertions are covered within existing exclusions. Budget accounting uses the largest alternative, including the 16-by-16 boundary, rather than their sum.

Controls cover original node/declaration identity, selected scalar provenance, eager refusal of excluded unused alternatives, malformed patterns, shared pattern budget and recovery, cancellation before assessment, snapshot opening, disposal and detached export. Six unchanged original schema/instance pairs remain Unsupported on both facades because of independent co-blockers; these controls do not constitute a new corpus run. Existing legacy choice profiles refuse the feature.

**`xs:all` is deferred.** Its 23 planned schema payloads are refusal controls only; no order-independent validation, occurrence semantics or all-phase adoption is claimed. Repeated outer choices, nullable alternatives, nested choice containers, complex alternatives, groups and wildcards remain excluded. This experiment is not full XSD 1.1, full PSVI, a package qualification or a release.

The final suite contains 54 planned public-facade controls, 11 identity controls, 42 boundary/refusal controls and 12 unchanged-original controls per pin. Earlier failing runs are preserved under evidence: fixture links originally overwrote the output JSON, then timestamp-based copying retained that stale output. Correct filename links plus unconditional copying resolved the harness error without candidate source changes.

[Evidence and exact source manifest](choice-feasibility-evidence/CANDIDATE-SOURCE.json) record 59 Git source blobs plus four overlays (three replacements and one new file), yielding 60 source files. [Reconstruction](choice-feasibility-evidence/reconstruct.py) accepts a fresh output directory and verifies every hash. [Specification adoption](choice-feasibility-evidence/EXPERIMENT-ADOPTION.json) precedes the overlay. Signed archive/cache/runtime identities and all test receipts are included. Rendering checks are programmatic; visual acceptance is not claimed.

Next: review and adopt this bounded choice subset into production, then run the full source, consumer-package, native and retained qualification sequence. Assess `xs:all` separately after choice delivery. No Git delivery is authorized for this new candidate.
'''
(w/'docs/particles/CHOICE-FEASIBILITY.md').write_text(note,encoding='utf-8')
files=['AGENTS.md','.kimi/HANDOVER.md','ROADMAP.md','docs/ARCHITECTURE.md','docs/FEATURE_REQUESTS.md','docs/INTEGRATION.md','docs/AGENT_HANDOVER.md','docs/REQ-009-SPECIFICATION-COVERAGE.md','docs/specification-coverage/PROGRAM.md']
for name in files:
 p=w/name;s=p.read_text(encoding='utf-8');link='docs/particles/CHOICE-FEASIBILITY.md' if '/' not in name else ('../docs/particles/CHOICE-FEASIBILITY.md' if name.startswith('.kimi') else ('../particles/CHOICE-FEASIBILITY.md' if name.startswith('docs/specification-coverage') else 'particles/CHOICE-FEASIBILITY.md'))
 s+='\n\n### 23 November 2026 — merged integer support; isolated choice feasibility\n\nPR #42 merged as `aa07e801ad5f2d89ebccd7299a4f539800e20df1`; main CI `45139587221` passed 3,620 source tests. The [bounded choice experiment]('+link+') passes 119 controls per Core pin and all 3,620 unchanged source tests, with zero public API delta. Production inputs and published packages remain unchanged. Choice production adoption and full qualification are next; `xs:all` remains a separate deferred phase. Earlier pre-merge contract receipts remain historical evidence.\n'
 p.write_text(s,encoding='utf-8')
print('Created source overlay, exact 3620-test comparison, evidence and canonical updates.')
