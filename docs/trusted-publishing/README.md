# Trusted-publishing workflow/controller qualification

17 November 2026. Base1783d1a99b37e206b10592c24b3a5fa3ac126969. [Pre-implementation basis](BASIS.json), [owner setup and sequence](../TRUSTED_PUBLISHING.md), [verification receipt](VERIFICATION.json), [offline verifier](verify.py).

The owner selected NuGet trusted publishing after repository metadata showed no configured Schema publishing secrets, variables, environments or publisher. This local candidate adds release.yml, a pure preparation/package verifier, same-workflow run admission guard and refusal controls; normal build CI runs those controls. Python cache ignores are housekeeping. Product C# sources, tests, framework, version rules, Core references and managed assets are unchanged.

| Qualification | Result | Scope |
|---|---|---|
| Schema Release build | Zero warnings/errors | Normal product build; no dependency change |
| Full Schema suite | 272pass/0fail/0skip | Existing249implementation+23public identities |
| Python release controls | 19pass | Synthetic package/TRX cases; payload/provenance/dependency/legal/unsafe-path/registry/workflow refusals; no login or push |
| Node provenance controls | 22pass | Mocked GitHub run/jobs/artifacts; valid source plus21refusals before outputs |
| Workflow YAML | Both files parse | Manual dispatch, defaultfalse and OIDC permission scope checked |
| PowerShell syntax | Eleven step bodies parse | Parser only; no execution of workflow push body |
| Real artifact/controller rehearsal | Pass | Preserved localalpha0.7package, nine actual Core signatures,272/23receipts,124legacy cases per lane,seal/verify andregistryGET |
| Hosted workflow run | Not performed | Candidate uncommitted/unpushed; owner settings and accepted workflow required |

The rehearsal's controller is forty zeroes, explicitly not a hosted SHA; no eligible GitHub preparation run exists. The workflow will create a new hosted artifact whose exact digest must be independently reviewed; the original local candidate remains unchanged and is not silently replaced. Source-only evidence snapshots/receipts are hashed here; raw binaries, parser dependency, ZIP/TRX/logs and rehearsal layouts remain under the ignored Core workspace packet.

No policy/variable/account change, secret/token access, login, push, tag, release, registry mutation, commit/push or publication authorization is claimed. Next: explicit commit/push/PR approval, hosted review/merge, manual preparation-only run, new exact-artifact review and explicit publication decision. User must configure the NuGet policy and profile variable before the publisher can authenticate. Full schema/profile/API/performance acceptance remains outside scope.
