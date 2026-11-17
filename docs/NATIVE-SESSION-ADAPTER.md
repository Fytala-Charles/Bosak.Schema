# Internal native decimal session adapter

> 17 November 2026: the [public implementation candidate](NATIVE-DECIMAL-PUBLIC-API.md) supersedes the implementation-pending statements below. Historical prototype evidence is preserved; global acceptance and API freeze remain pending.

Date: 2026-11-15. Base: committed backend `aba6967`. Scope: internal implementation of the [finite decimal session profile](NATIVE-SESSION-PROFILE.md) on published Core 1.81.2. [Qualification receipt](native-session-adapter/2026-11-15/README.md). No public factory/API freeze or release is delivered.

## 1. Assessment and authority

NativeSchemaProvider wraps the native backend and implements ISchemaAssessor/ISchemaAuthority. It creates a distinct owned component universe per compilation, resolves the global root declaration and built-in decimal type, and compares only actually issued handles. Local declarations and the anonymous root type retain identity without synthesized global names. Unimplemented derivation, affiliation, closure, attributes and simple construction return Unsupported. Foreign component operations reject before foreign getters.

Bind accepts only the published engine-issued XPath 3.1, XDM 3.1 typed-identity environment, the inspected descriptor version and empty namespace context. It retains the exact captured object. Fingerprints describe captures and may change with explicit limits; they do not confer authority or justify binding a copied environment. Only the recorded native `$value` route/options are exercised; this does not certify arbitrary options or syntax.

NativeSchemaInput captures immutable bounded string XML through the backend's hardened parser. Each capture owns distinct source/snapshot identities, a private tree, canonical guarded node wrappers and a revocable lifetime. It never exposes the mutable backing XML. Root/document/child/axis traversal returns only canonical wrappers; outside-tree projection refuses. Serialization produces detached text, not authority.

Strict assessment is supported only for an unassessed native capture with a valid episode and optional owned global-root declaration. Assess validates the original complete grammar and creates original node facts, typed scalar containers and provenance only after backend Valid. Invalid/Unsupported returns no validated root or evidence. Diagnostic output is finite, caller-tightened and may be explicitly truncated; no fabricated W3C mapping or location. Episode date/timezone are captured once, timezone is bounded, and cancellation throws before publication. One successful original assessment is allowed per capture; repeated/foreign-advisor reassessment refuses without rewriting it.

Advisor ownership authenticates exact published origin, facts, input, node and scalar issuance. Copying valid handles, labels, schema text or wrapper objects grants nothing. Root facts have no typed value and are never nilled; decimal leaf facts preserve actual declaration/type and original assessment. The regular engine data(.) operation reports FOTY0012 as InvalidOperationException in this release; direct guarded root.TypedValue reports XPathErrorException with that code. Both routes are tested without changing Core.

## 2. Host operation and lifetime policy

One shared NativeSessionRunner per Core session is the selected sequential host boundary. It uses an explicit admission gate held until synchronous Run or the actual RunAsync task completes, including callback consumption and cleanup. A canceled external wait does not release this gate. Overlapping work through that runner refuses; a callback fault releases it. Creating multiple runners or bypassing the shared runner is outside this host profile. The raw Core APIs do not acquire a new provider lease for each overlapping run, so provider lease counts cannot enforce sequential admission.

NativeSchemaInput.Enter issues host leases over the actual captured resource. Owner Dispose revokes new admissions and guarded access, while existing engine/raw leases remain counted until actual completion. DrainAsync waits for them; canceling that wait does not drain or release them. ResourcesReleased denotes logical owner close plus zero retained leases, not a memory-reclamation/performance guarantee. Core snapshot/session disposal never substitutes for host resource ownership. Inputs and detached plain payloads remain distinct from borrowed facts/metadata and engine-issued scalar receipts.

The internal integration consumer compiles `$value`, assesses a captured instance with the session's actual advisor, opens an original snapshot and reads/evaluates its issued decimal inside the runner callback. It checks original declared/actual type and provenance, then returns a detached scalar string. This is an actual native backend/Core integration test in the friend test assembly, not a separately packaged downstream consumer or supported public entry point.

## 3. Qualification map and remaining acceptance

Follow-up after commit 939ba9a: the [owned-facade/expiry checkpoint](native-session-facade/2026-11-15/README.md) increases adapter coverage from 20 to 28 and adds eight internal facade cases (243 standalone total). It qualifies container/raw expiry, scalar receipt/snapshot expiry, scale bounds, gate/fault/cancellation cleanup and default-name rejection. ADR-010 records the concrete public proposal; prior 20-case receipts remain historical. Official docs and network-enabled NuGet audit now pass; earlier sandbox failures do not remain current blockers.

| Profile groups | Current evidence | Remaining boundary |
|---|---|---|
| P01/P02 | 76 backend cases plus actual advisor Valid/Invalid/Unsupported cases | Broader structural/assertion grammar remains refused |
| P03/P04/P06 | Owned component/fact/node/container issuance, cross-advisor and copied-evidence refusals, exact bound environment | Wider universe/host/context profiles and public API acceptance |
| P05/P07 | Real scalar identity/provenance and engine root FOTY0012 | General engine operations and external consumer qualification |
| P08 | Backend byte/parser/numeric boundaries; real native numeric, scalar-cache and snapshot quota refusals | Exhaustive remaining atom/scale/result-shape and resource budgets |
| P09/P10 | Revoked getters/deferred access, canceled drain wait with raw work retained, sequential runner refusal, deterministic mid-assessment cancellation | Stream/reader native overloads, broader disposal races, arbitrary caller policies |
| P11/P12 | Unsupported compatible handoff between independent equal schemas; explicit unsupported relations | Positive cross-universe proof and complete relation support |

No group-level/global sign-off is manufactured from this mapping. Full XSD 1.1, all N1–N8 coverage, arbitrary concurrent disposal, non-decimal/list/union/nil profiles, performance budgets, API freeze, publication and the separate historical Core integration lane remain distinct gates. The next work is to close the remaining bounded-profile controls and review the public entry point/consumer contract before expanding coverage.

*Last updated: 2026-11-15*
