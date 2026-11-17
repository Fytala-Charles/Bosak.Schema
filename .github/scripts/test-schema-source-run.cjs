// Copyright 2026 Fytala. SPDX-License-Identifier: Apache-2.0
const test = require('node:test');
const assert = require('node:assert/strict');
const validate = require('./schema-source-run.cjs');
const sha = 'a'.repeat(40), hash = 'b'.repeat(64);
function fixture() {
  const run = { status:'completed', conclusion:'success', event:'workflow_dispatch', run_attempt:1, head_branch:'main', path:'.github/workflows/release.yml', head_repository:{full_name:'Fytala-Charles/Bosak.Schema'}, head_sha:sha };
  const jobs = {total_count:2,jobs:[{name:'prepare',status:'completed',conclusion:'success'},{name:'publish',status:'completed',conclusion:'skipped'}]};
  const artifacts = {total_count:1,artifacts:[{name:'schema-nuget-'+sha,expired:false,digest:'sha256:'+hash,workflow_run:{id:123,head_sha:sha}}]};
  const output = {};
  const args = {github:{rest:{actions:{getWorkflowRun:async()=>({data:run}),listJobsForWorkflowRun:async()=>({data:jobs}),listWorkflowRunArtifacts:async()=>({data:artifacts})}}},context:{repo:{owner:'Fytala-Charles',repo:'Bosak.Schema'}},core:{setOutput:(k,v)=>{output[k]=v;}},env:{SOURCE_RUN_ID:'123',EXPECTED_MANIFEST_SHA256:hash}};
  return {run,jobs,artifacts,args,output};
}
test('qualified preparation is admitted without login or push', async()=>{const f=fixture();await validate(f.args);assert.equal(f.output.controller_sha,sha);assert.equal(f.output.artifact_name,'schema-nuget-'+sha);});
const mutations = {
  'invalid run ID':f=>{f.args.env.SOURCE_RUN_ID='0';},
  'unsafe numeric run ID':f=>{f.args.env.SOURCE_RUN_ID='9999999999999999999';},
  'missing reviewed digest':f=>{f.args.env.EXPECTED_MANIFEST_SHA256='';},
  'wrong repository':f=>{f.args.context.repo.repo='Other';},
  'running preparation':f=>{f.run.status='in_progress';},
  'failed preparation':f=>{f.run.conclusion='failure';},
  'pull-request event':f=>{f.run.event='pull_request';},
  'rerun attempt':f=>{f.run.run_attempt=2;},
  'non-main branch':f=>{f.run.head_branch='candidate';},
  'wrong workflow':f=>{f.run.path='.github/workflows/build.yml';},
  'fork source':f=>{f.run.head_repository.full_name='other/Bosak.Schema';},
  'invalid head':f=>{f.run.head_sha='main';},
  'no successful prepare job':f=>{f.jobs.jobs[0].conclusion='skipped';},
  'publish run cannot be reused':f=>{f.jobs.jobs[1].conclusion='success';},
  'truncated job inventory':f=>{f.jobs.total_count=101;},
  'truncated artifact inventory':f=>{f.artifacts.total_count=101;},
  'missing artifact':f=>{f.artifacts.artifacts=[];f.artifacts.total_count=0;},
  'expired artifact':f=>{f.artifacts.artifacts[0].expired=true;},
  'artifact source mismatch':f=>{f.artifacts.artifacts[0].workflow_run.head_sha='c'.repeat(40);},
  'artifact run mismatch':f=>{f.artifacts.artifacts[0].workflow_run.id=124;},
  'duplicate artifact':f=>{f.artifacts.artifacts.push(structuredClone(f.artifacts.artifacts[0]));f.artifacts.total_count=2;}
};
for(const [name,mutate] of Object.entries(mutations)) test(name+' refuses before outputs',async()=>{const f=fixture();mutate(f);await assert.rejects(()=>validate(f.args));assert.deepEqual(f.output,{});});
