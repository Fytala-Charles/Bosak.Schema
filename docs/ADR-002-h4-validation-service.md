# ADR-002: H4 — Public Validation Service & `validation`/`@type` Runtime Semantics

> **Transferred 2026-10-30** from the private Bosak.Licensing repository as REQ-009 seam-history evidence. This document predates the Apache-2.0 public split of 29 October 2026: its "commercial add-on", license-gate and activation framing is historical context, not current behavior — public schema services are activation-free. See [AGENT_HANDOVER](AGENT_HANDOVER.md).

> **Status:** **Merged into Bosak core main 2026-09-22** — PR [#15](https://github.com/Fytala-Charles/Bosak/pull/15), merge commit `77a7c5b` (CI green); ships in v0.12.0-beta
> **Date:** 2026-09-22
> **Context:** REQ-001 (FEATURE_REQUESTS.md) · seam contract `SEAM_DESIGN.md` §3 (hook H4) · builds on [ADR-001](./ADR-001-h3-typed-construction-api.md) (H3, merged as REQ-098)
> **Core tracking:** landed in `Fytala-Charles/Bosak` as the fourth and final seam PR (after REQ-097/098) — REQ-099

---

## 1. Context

Hook **H4** is the last of the four seam hooks: *a public validation service for XSLT `validation` / `@type` attributes* (`SEAM_DESIGN.md` §3). It unblocks audit groups G5 (validation modes + `default-validation`, 13 tests) and G6 (validating constructors, XTTE15xx family, 133 tests — the largest bucket).

Core investigation (2026-09-22) established:

1. **The attributes are accepted but inert.** In schema-aware mode the XTSE1660 gate (`Stylesheet.cs` ~:3206-3233) accepts `validation`/`@type`/`default-validation`, but no instruction model exists — `TransformEngine` interprets the stylesheet's source `XElement` tree, so the attributes survive implicitly and are read nowhere at runtime. `default-validation` is not parsed into any property; runtime lookup walks `instruction.Ancestors()` to the module root.
2. **All needed validation tricks already exist privately.** `VmEngine.ValidateNode` (~:3553-3764) implements: strict/lax modes, the lax `xs:anyType` root-declaration augmentation, named-type validation via `xsi:type` injection/removal, document-shape checks. It is `private static` inside the VM, raises XQuery error codes, and round-trips through serialization.
3. **H3's annotator is strict-only.** `XdmSchemaAnnotator.ValidateSubtree` does global-element strict validation; it lacks lax mode, named-`*type*` targets, attribute-node validation, `strip`/`preserve` modes, and document-shape checks.
4. **H3's processors are insufficient for H4 wiring.** `ConstructedElementProcessor` is `Action<IXdmNode>` — it receives no instruction context, so an add-on cannot recover per-instruction `validation`/`type` values. H4 therefore needs TransformEngine-internal wiring (the attributes are readable at every site because the instruction element is in scope).
5. **PSVI does not survive copies.** `CopyXdmNode` (~:8870) drops annotations — breaks `validation="preserve"` (and nilled) on copies. The secondary `xsl:result-document` path (~:16492-16540) never calls the H3 document finalize hook.
6. **`xsl:strip-type-annotations` / `input-type-annotations` have zero implementation** (27 schema-gated tests).

## 2. Decision

### 2.1 Public validation service — generalize `XdmSchemaAnnotator` (Providers)

```csharp
public enum XdmValidationMode { Strict, Lax, Strip, Preserve }
public sealed record XdmValidationOptions(XdmValidationMode Mode, XmlQualifiedName? TypeName = null, bool DocumentLevel = false);

public static partial class XdmSchemaAnnotator
{
    public static XdmSubtreeValidationResult Validate(XElement element, XmlSchemaSet schemas,
        XdmValidationOptions options, ValidationEventHandler? handler = null, bool throwOnInvalid = false);
    public static XdmSubtreeValidationResult ValidateAttribute(XAttribute attribute, XmlSchemaSet schemas,
        XdmValidationOptions options, ValidationEventHandler? handler = null, bool throwOnInvalid = false);
}
```

- Ports the proven `VmEngine.ValidateNode` algorithms to the public service: lax `anyType` root augmentation, named-type `xsi:type` injection/removal (with generated prefix), document-shape check, element-only whitespace stripping (shared with `XDocumentProvider.StripElementOnlyContentWhitespace`).
- **The service stays error-code-agnostic** (structured failure reasons); hosts map codes: XSLT → XTTE15xx, XQuery → XQDY0027/0061/0084 (same split as today). A later refactor may make `VmEngine.ValidateNode` delegate to it (Runtime already references Providers) — not part of this PR unless free.
- `Strip` removes existing PSVI (fresh unvalidated subtree); `Preserve` keeps annotations (with the copy-preservation fix in 2.2); `Strict`/`Lax` validate per options.

### 2.2 Runtime wiring — `TransformEngine` (Xslt), schema-aware-gated

At each construction site, read `validation`/`type` from the instruction element (LREs: `xsl:validation`/`xsl:type`), with `default-validation` inherited via the ancestors walk, and validate the constructed node through the service when a schema set is in scope:

| Site | Node | Failure codes |
|------|------|---------------|
| `xsl:element` (~:5824-5928), LRE (`CopyLiteralElement` ~:7780/:8103), `xsl:copy` (`ExecuteSingleCopy` ~:9088) | element | XTTE1510 (content invalid), XTTE1512 (strict, no top-level declaration), XTTE1540 (lax/`@type` failure), XTTE1545 (QName/NOTATION-valued content) |
| `xsl:attribute` (~:5930-6021) | attribute | XTTE1515 (attribute invalid), XTTE1535 (`@type` = complex type on attribute), XTTE1555 (parentless attribute) |
| `xsl:document`, `xsl:result-document`, implicit result doc (`GetResultDocument` ~:1332-1352, `ExecuteResultDocument` ~:16223 — incl. the secondary-path finalize gap ~:16492-16540) | document | XTTE1550 (document shape / doc-level failure) |

- **Gating:** wiring is active only when the stylesheet carries schema information (`CompiledSchemaSet`/context `SchemaSet` non-null). With `SchemaAware=false` there is no schema set, so every path is null-conditional — bit-identical to today (Hard Rule 2), and `lax`/`preserve`/`strip` on basic processors keep their errata behavior (no-op).
- **Errors:** raised as `XsltRuntimeException(code, description, …)` (catchable by `xsl:try`, matches the conformance harness's message-contains matching).
- **`@type` QName resolution** via the in-assembly `Stylesheet.ExpandQName`, honoring `xpath-default-namespace`.

### 2.3 Companion fixes folded into the same PR

- **PSVI preservation in `CopyXdmNode`** (~:8870): carry `GetSchemaInfo()` annotations onto copied nodes (elements, attributes) so `validation="preserve"` and nilled properties survive copies (validation-1202/1203/1204 shape).
- **Secondary `xsl:result-document` finalize gap** (~:16492-16540): route through `FinalizeResultDocument` like the principal path.
- **`xsl:strip-type-annotations` / `input-type-annotations`**: parse the declarations (`Stylesheet`) and apply PSVI stripping to loaded input documents (input) / to copied nodes (strip on output).

## 3. Alternatives Considered

| Alternative | Verdict | Why |
|-------------|---------|-----|
| Add-on drives validation purely through the H3 `Action<IXdmNode>` processors | Rejected | The processors carry no instruction context — per-instruction `validation`/`type` is unrecoverable; would force an XSLT-side attribute-reading layer in the add-on that duplicates the engine's own tree walk |
| Expose `VmEngine.ValidateNode` as public | Rejected | It is VM-internal, raises XQuery codes, round-trips through serialization; the service belongs in Providers next to the annotator (Runtime may delegate later) |
| Make the core emit XTTE15xx directly from the service | Rejected | Keeps the service host-agnostic; the code mapping is XSLT-specific and lives in TransformEngine |
| Wire validation only at the document boundary | Rejected | `validation`/`@type` are per-instruction semantics with per-instruction error codes; document-only validation cannot produce XTTE1515/1535/1555 at the right point |

## 4. Consequences

- All additions are freeze-compatible; default (`SchemaAware=false`) behavior stays bit-identical (no schema set in scope → no validation path executes).
- The core gains genuine schema-aware validation semantics behind the opt-in flag — consistent with the seam philosophy: the hooks are useful to any integrator; the *packaged, license-gated* conformance product remains Bosak.Schema's differentiator.
- Verification of the core PR: unit tests in `Bosak.Xslt.Tests` driving the semantics via `SchemaAware=true`; full sweep must stay at 10,220/55/4,325 (runner has no schema-aware mode yet — that harness mode is Bosak.Schema-track work).
- Out of scope (later): streaming-tagged validation sets (~215 tests, Phase C); `xsl:mode/@typed` template-dispatch semantics beyond the existing XTTE3100 built-in rule.

## 5. Acceptance

- [ ] `XdmValidationMode`/`XdmValidationOptions` + `Validate`/`ValidateAttribute` on `XdmSchemaAnnotator`, covering strict/lax/named-type/document-level/attribute cases
- [ ] `validation`/`@type` runtime semantics wired at all sites in §2.2 with the XTTE1510/1512/1515/1535/1540/1545/1550/1555 family
- [ ] `default-validation` inheritance; `xsl:strip-type-annotations`/`input-type-annotations` implemented
- [ ] PSVI preserved across `xsl:copy`/`copy-of`; secondary result-document path finalized
- [ ] With `SchemaAware=false` and no schema set: sweep aggregate identical (10,220/55/4,325), QT3 unchanged, all unit suites green
- [ ] Core doc sync per its AGENTS.md checklist (REQ-099 registry entry, INTEGRATION, ARCHITECTURE, AGENT_HANDOVER)

---

*Linked from: `SEAM_DESIGN.md` §3 (hook H4) · `FEATURE_REQUESTS.md` REQ-001 acceptance criteria · `ARCHITECTURE.md` §4*
