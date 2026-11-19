# Bounded Unsupported-reason candidate

19 November 2026. The local development candidate adds `NativeDecimalDiagnostic.UnsupportedReason` and the fixed `NativeDecimalUnsupportedReason` enum. Existing stage codes/messages, outcomes and grammar are preserved; categories describe one detected refusal and may be Unspecified. All365source cases pass (312 implementation/53public, including20new controls), as do53package-only cases and retained124/23 unchanged-binary replacement catalogs. These lanes overlap and are not additive. The54release and six workload-integrity controls pass. MinVer minimum advances to0.6 for this additive feature;0.6.0-alpha.0.2 is local/unsigned only. Published0.5.0-beta.1 and allCore1.81.2 pins remain unchanged. No commit/push, tag or publication.

## Basis and API

The owner selected existing public examples for ranking, then requested continuation
of the diagnostic-first recommendation. [Pre-implementation contract](CONTRACT.md)
and [basis](BASIS.json) fix the additive behavior before source edits. This remains
a local implementation candidate, not release or general provider acceptance.
Original W3C adoption/source hashes and intentional grammar deviations are unchanged.

| Reason | Value | Detected refusal |
|---|---|---|
| Unspecified | 0 | Unmapped policy/environment/transport refusal; every Invalid diagnostic |
| Namespace | 1 | Existing rejected schema namespace setting |
| DeclarationShape | 2 | Existing checked declaration/content shape |
| ValueType | 3 | Local declaration does not resolve to admitted decimal type |
| Attributes | 4 | Rejected schema or instance attribute |
| AssertionGrammar | 5 | Unsupported expression syntax/name/operator |
| RepresentationCapacity | 6 | Explicit byte/node/lexical/numeric or assertion bound |

Only Unsupported diagnostics can carry nonzero reasons. Valid remains diagnostic-free.
Callers must tolerate Unspecified and future categories. Messages stay fixed, under
160 characters and free of caller XML, names, paths, exception graphs or authority.
No public result/diagnostic constructor or settable property is added.

```csharp
if (result.Outcome == NativeDecimalOutcome.Unsupported)
{
    var reason = result.Diagnostics[0].UnsupportedReason;
    // Explain a detected category; Unspecified is a normal fallback.
}
```

This API is in the unpublished development candidate, not the published beta.1.
No general source scan, exhaustive gap inventory or line/column promise is made.
Refusal order stays unchanged. The first forbidden schema attribute in XML order
determines its reason. Namespaced instance names that were Invalid remain Invalid.
DTD/policy refusals with no mapped category remain Unsupported/Unspecified.
Assertion compilation preserves explicit capacities instead of disguising them as
missing grammar. Arguments/cancellation/overlap/quota/closed-access still throw.

## Verification

Twenty new controls use public APIs only. They cover all categories, stable stage
messages/codes, input-order precedence, privacy, recovery, numeric/byte/lexical
limits, unchanged Invalid handling, detached output and inaccessible constructors.
Complete public `sku.xsd` and `orders.xsd` now replay as Unsupported/Namespace;
this confirms the first detected blocker, not support for their other constructs.

| Lane | Result | Scope |
|---|---|---|
| Full source solution | 365/0/0 | 312 implementation +53external public API;20new controls |
| Local NuGet-only consumer | 53/0/0 | Separate copied tests/fixtures, isolated cache, no project reference |
| Retained legacy binary | 124/0/0 | Original test/runtime files, only Schema.dll replaced |
| Retained published consumer binary | 23/0/0 | Original test/runtime files, only Schema.dll replaced |
| Release refusal controls | 54 pass | 32Python +22Node; mocked, no login/push |
| Workload controls | 6 pass | Tool builds; integrity checks, no new timing claims |

The local candidate archive/cache/runtime Schema DLL hashes match; package assets
retain nine Core1.81.2 libraries. Retained-binary inventories confirm every other
file is unchanged. Build output has no warnings/errors. Original beta compatibility
controls/receipts and source dossiers remain untouched.

Initial package restore treated the CLI URI source as a filesystem path; an explicit
two-source NuGet.Config resolved this. Initial VSTest result-directory argument
composition failed before tests ran; corrected string arguments passed both lanes.
Those controller diagnostics remain raw and are not product failures.

The older static gap assessment is historical after canonical docs advance. Its
LF-normalized hashes were verified and its snapshot retained under
`.guard-tmp/schema-unsupported-reasons-20261119/assessment-before/` before changes.
No original sealed receipt was rewritten. Fresh source and package tests establish
this candidate; historical totals are not pooled with them.

## Delivery boundary

No engine changes or conformance claim; Schema-only additive diagnostics. Core pins
stay1.81.2. The0.6 development minimum follows the repository's feature-versioning
rule and is not approval to publish0.5.0-beta.2 or any other version. The existing
release controller remains pinned to reviewed beta.1 source/counts and would need
separate review before a future release. No controller weakening or publication.
Review the candidate and, when explicitly requested, commit/push/PR. Braid's
production licensing pause remains independent; no private material was copied.
