"""Lint contract artifacts only; never compile a schema or execute an engine.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import hashlib,json,subprocess,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent
ROOT=P.parent.parent
sha=lambda b:hashlib.sha256(b).hexdigest()
def main():
    plan=json.loads((P/'CASE-PLAN.json').read_text(encoding='utf-8-sig'))
    assert len(plan['rows'])==53 and len(plan['gates'])==25
    ids=[r['id'] for r in plan['rows']+plan['gates']]
    assert len(ids)==len(set(ids))
    for row in plan['rows']:
        assert row['status']=='Proposed authored payload; not executed'
        assert row['expectedCompile'] in ['Valid','Invalid','Unsupported']
        assert row['expectedAssessment'] is None or row['expectedAssessment'] in ['Valid','Invalid','Unsupported']
        ET.fromstring(row['schema']);ET.fromstring(row['instance'])
    rules=json.loads((P/'FACET-RULES.json').read_text(encoding='utf-8-sig'))
    bounds=rules['baseBoundConditions']
    assert len(bounds)==16 and len({(r['newFacet'],r['baseFacet']) for r in bounds})==16
    assert {r['requiredOperator'] for r in bounds}=={'<','<=','>','>='}
    assert len(rules['sameDatatypePairs'])==4 and len(rules['sameStepExclusions'])==2
    source=json.loads((P/'SOURCE-REVIEW.json').read_text(encoding='utf-8-sig'))
    for f in source['files']:
        assert sha((ROOT/f['path']).read_bytes())==f['worktreeSha256']
        raw=subprocess.check_output(['git','show',source['source']+':'+f['path']],cwd=ROOT)
        assert sha(raw)==f['gitBlobSha256']
    upstream=json.loads((P/'UPSTREAM-REVIEW.json').read_text(encoding='utf-8-sig'))
    for name,h in upstream['files'].items():assert sha((ROOT/'tests/TestData/ConformancePilot/upstream'/name).read_bytes())==h
    paths=['src','tests','tools','Directory.Build.props','global.json','.github']
    assert not subprocess.check_output(['git','diff','--name-only','--',*paths],cwd=ROOT).strip()
    assert not subprocess.check_output(['git','ls-files','--others','--exclude-standard','--',*paths],cwd=ROOT).strip()
    result=dict(status='Passed artifact lint only',proposedPayloads=53,proposedSeparateGates=25,xmlWellFormedDocuments=106,baseFacetRelations=16,sameDatatypePairs=4,sourceReviewFilesVerified=len(source['files']),reviewedUpstreamFilesVerified=len(upstream['files']),productInputsUnchanged=True,engineExecuted=False,newPilotExecuted=False,semanticAdoption=False)
    (P/'VALIDATION.json').write_text(json.dumps(result,indent=2)+'\n',encoding='utf-8')
    print('Artifact lint passed:53payloads/25gates; XML only, no Schema/engine execution.')
if __name__=='__main__':main()
