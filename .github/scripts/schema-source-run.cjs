// Copyright 2026 Fytala. SPDX-License-Identifier: Apache-2.0
// Refuse unqualified runs before artifact download, OIDC login or package push.
module.exports = async ({ github, context, core, env = process.env }) => {
  const repository = `${context.repo.owner}/${context.repo.repo}`;
  if (repository.toLowerCase() !== 'fytala-charles/bosak.schema') throw Error('Wrong repository');
  if (!/^[1-9][0-9]*$/.test(env.SOURCE_RUN_ID || '') || !Number.isSafeInteger(Number(env.SOURCE_RUN_ID))) throw Error('Valid preparation run ID required');
  if (!/^[0-9a-f]{64}$/i.test(env.EXPECTED_MANIFEST_SHA256 || '')) throw Error('Reviewed manifest SHA-256 required');
  const params = { ...context.repo, run_id: Number(env.SOURCE_RUN_ID) };
  const { data: run } = await github.rest.actions.getWorkflowRun(params);
  if (run.status !== 'completed' || run.conclusion !== 'success' || run.event !== 'workflow_dispatch' || run.run_attempt !== 1 || run.head_branch !== 'main' || run.path !== '.github/workflows/release.yml' || run.head_repository?.full_name?.toLowerCase() !== repository.toLowerCase() || !/^[0-9a-f]{40}$/.test(run.head_sha || '')) throw Error('Successful first-attempt same-workflow main preparation required');
  const { data: jobs } = await github.rest.actions.listJobsForWorkflowRun({ ...params, filter: 'latest', per_page: 100 });
  if (jobs.total_count !== jobs.jobs.length || jobs.jobs.length > 2 || jobs.jobs.filter(j => j.name === 'prepare' && j.status === 'completed' && j.conclusion === 'success').length !== 1 || jobs.jobs.some(j => j.name !== 'prepare' && !(j.name === 'publish' && j.conclusion === 'skipped'))) throw Error('Run did not exclusively prepare an artifact');
  const { data: artifacts } = await github.rest.actions.listWorkflowRunArtifacts({ ...params, per_page: 100 });
  if (artifacts.total_count !== artifacts.artifacts.length) throw Error('Incomplete artifact inventory');
  const name = 'schema-nuget-' + run.head_sha;
  const selected = artifacts.artifacts.filter(a => a.name === name && !a.expired && a.workflow_run?.id === Number(env.SOURCE_RUN_ID) && a.workflow_run?.head_sha === run.head_sha && /^sha256:[0-9a-f]{64}$/.test(a.digest || ''));
  if (selected.length !== 1) throw Error('Exactly one live preparation artifact required');
  core.setOutput('controller_sha', run.head_sha);
  core.setOutput('artifact_name', name);
};
