# ADR-001: H3 — Typed-Construction / Annotation API Shape

> **Transferred 2026-10-30** from the private Bosak.Licensing repository as REQ-009 seam-history evidence. This document predates the Apache-2.0 public split of 29 October 2026: its "commercial add-on", license-gate and activation framing is historical context, not current behavior — public schema services are activation-free. See [AGENT_HANDOVER](AGENT_HANDOVER.md).

> **Status:** **Merged into Bosak core main 2026-09-22** — PR [#13](https://github.com/Fytala-Charles/Bosak/pull/13), merge commit `65bf888` (CI green)
> **Date:** 2026-09-22
> **Context:** REQ-001 (FEATURE_REQUESTS.md) · seam contract `SEAM_DESIGN.md` §3 (hook H3)
> **Core tracking:** lands in `Fytala-Charles/Bosak` as the next seam PR after REQ-097 (H1/H2)

---

## 1. Context

The seam audit (`SEAM_DESIGN.md` §3) requires hook **H3 — a public way for host code invoked through the constructor hooks to produce nodes carrying complex-type schema annotations (validated construction)**, unblocking audit groups G4–G6 (typed values & node properties; validation modes; validating constructors).

Investigation of the core (2026-09-22) established the load-bearing facts:

1. **Every typed-value surface in the engine already reads PSVI exclusively.** `XDocumentNode.TypedValue`, `HasNoTypedValue`, `IsComplexType`, `SchemaTypeAnnotation`, `SchemaElementDeclaration`, `SchemaAttributeDeclaration`, `IsNilled`, `IsId`/`IsIdref`, the `SchemaElementTest`/`SchemaAttributeTest`/`ElementTypeTest`/`AttributeTypeTest` opcodes, and `fn:deep-equal` all read `XObject.GetSchemaInfo()` (System.Xml.Linq `IXmlSchemaInfo` annotation), populated only by `XDocument.Validate(..., addSchemaInfo: true)`.
2. **No public mechanism annotates a constructed node.** The XQuery `validate{}` path round-trips through serialization (`VmEngine.ValidateNode`); constructed nodes carry only `ConstructedElementAnnotation` (→ `xs:anyType`).
3. **All XSLT construction bypasses the existing constructor hooks.** `TransformEngine` builds raw `XElement`/`XAttribute` at ~10 sites (xsl:element `:5810`, literal result elements `:7829`, `xsl:copy` `:9950`, free-standing attributes `:5943`, …). The existing `ElementConstructorHook`/`AttributeConstructorHook`/`ContentNodeConstructorHook` are invoked only from the XQuery opcodes, and their spec-based signatures (`XdmElementSpec`, `XdmContentItem` lists) do not fit XSLT's incremental append-driven construction.
4. **Hard Rule 2** (core behavior bit-identical without the package) and the freeze (pre-1.0 window) constrain the design: null-conditional hook consultation only, no behavior change when hooks are unset.

## 2. Decision

H3 is two additions to the **core public API**, plus hook consultation at XSLT construction finalize points.

### 2.1 Annotation service — `Bosak.XPath.Providers`

```csharp
public static class XdmSchemaAnnotator
{
    // Validates an in-memory subtree in place: PSVI (IXmlSchemaInfo) annotations are attached
    // to the live XObjects, so every typed-value surface in the engine works on the node immediately.
    public static XdmSubtreeValidationResult ValidateSubtree(XElement element, XmlSchemaSet schemas,
        ValidationEventHandler? handler = null, bool throwOnInvalid = false);

    // Attaches a host-built annotation without validation (e.g. declaration-only annotation,
    // nilled elements, nodes validated elsewhere).
    public static void Annotate(XObject node, IXmlSchemaInfo annotation);
}
```

- `ValidateSubtree` wraps the subtree in a temporary `XDocument`, calls `Validate(schemas, handler, addSchemaInfo: true)`, and returns a small result type (`IsValid`, `Errors`). Because PSVI attaches to the live `XObject`s, the wrapper is discarded and the caller's tree is annotated — no round-trip, no node identity change.
- `IXmlSchemaInfo` is a simple read-only interface; hosts may implement it or reuse the instances produced by validation.
- Placement in Providers (not Api) keeps it next to `XDocumentProvider.ValidateXDocument`, which it generalizes from whole documents to arbitrary subtrees.

### 2.2 Construction interception — `EvaluationContext` (Runtime) + `TransformEngine` (Xslt)

Two new null-conditional processors, invoked **after content is complete** (bottom-up, innermost element first):

```csharp
public Action<IXdmNode>? ConstructedElementProcessor { get; set; }   // xsl:element, LRE, xsl:copy element results
public Action<IXdmNode>? ConstructedDocumentProcessor { get; set; }  // final result-document boundary
```

- **In-place `Action<IXdmNode>`, not a substituting `Func`.** The 100% case for validated construction is mutating PSVI on the same node; substitution would require unwrapping an arbitrary `IXdmNode` back to `XElement`, which the public surface cannot do cleanly.
- **Finalize points** (hook consulted only when non-null; default behavior bit-identical):
  - `xsl:element` before return (`TransformEngine` ~`:5799-5901`)
  - literal result elements before return (`CopyLiteralElement` ~`:7754`)
  - `xsl:copy` element branch before return (~`:9950`)
  - result-document wrap points (`~:1330-1345`, `BuildResultFromNodesAndDocument` ~`:14476`)
- Attribute annotation flows through element-subtree validation (PSVI annotates attributes of validated elements); no separate attribute processor in H3.
- Registration pattern matches `DocumentPostProcessor`: the add-on sets the processors on the `EvaluationContext` it passes to `Transform(...)`.

## 3. Alternatives Considered

| Alternative | Verdict | Why |
|-------------|---------|-----|
| Reuse the existing spec-based `ElementConstructorHook` in XSLT | Rejected | XSLT construction is incremental-append, not spec-batch; threading `XdmContentItem` lists through `TransformEngine` is an invasive refactor of hot paths |
| Substituting `Func<IXdmNode,IXdmNode>` processors | Rejected (for v1) | Substitution needs public unwrapping of `IXdmNode` → `XObject`; annotation-in-place covers all G4–G6 flows |
| New XSLT-specific callback types on `XsltCompiler`/`XsltExecutable` | Rejected | Breaks the seam pattern (host already owns the `EvaluationContext`); two parallel hook systems would confuse integrators |
| Core implements full `validation`/`@type` runtime semantics | Out of scope | That is **H4** (public validation service + XTTE15xx mapping); H3 is deliberately "annotate + notify", the add-on decides *when* to validate |

## 4. Consequences

- **Freeze-compatible additions only** — no existing signature changes; unset processors leave every code path byte-identical (Hard Rule 2 satisfied by construction).
- The add-on (Bosak.Schema) drives validated construction by setting the processors and calling `XdmSchemaAnnotator.ValidateSubtree` inside them; *when* validation happens (per-element vs. document boundary, `validation`/`@type` attributes) is add-on policy enabled by H3, productized by H4.
- Validation of nested constructed subtrees runs bottom-up; per-element validation is O(depth) redundant re-validation — acceptable, and the add-on can defer to the document boundary to avoid it.
- **Companion core work (separate PRs, own merits):**
  - `schema-element()`/`schema-attribute()` match patterns silently never match (`PatternCompiler.CompileNodeTestPredicate` has no branch — the name test mis-splits on the inner colon). Needed for audit group G3.
  - H4: runtime semantics for `validation`/`@type` (XTTE1510/1512/1530 family — zero hits today) and a public validation service.

## 5. Acceptance

- [x] `XdmSchemaAnnotator.ValidateSubtree` annotates a constructed subtree; `node.TypedValue` / `SchemaTypeAnnotation` / `instance of` with a complex type with simple content return spec-correct results through the existing engine surfaces
- [x] Setting `ConstructedElementProcessor` on the consumer's `EvaluationContext` fires it for `xsl:element`, LRE, and `xsl:copy` element results, after content completion
- [x] Setting `ConstructedDocumentProcessor` fires it at the result-document boundary
- [x] With processors unset, XSLT sweep aggregate is identical to the REQ-097 baseline (10,220/55/4,325) and unit suites pass unchanged — verified on the core branch: unit 2,510/2,510, sweep 10,220/55/4,325, QT3 31,142/0/679
- [x] Core doc sync per its AGENTS.md checklist (REQ-098 registry entry, INTEGRATION §3.1b, ARCHITECTURE, AGENT_HANDOVER session-21)

*Implementation notes (2026-09-22, core branch `feature/xslt-schema-seam-h3`):* `throwOnInvalid`
throws `XmlSchemaValidationException` (repo idiom, matching `ValidateXDocument`), and
`ValidateSubtree` validates a deep clone and copies PSVI back onto the live nodes when the
subtree is attached — LINQ-to-XML reparenting would otherwise clone it silently (document
roots included, where `Parent` is null). Both deviations are recorded in the core REQ-098
decision log.

---

*Linked from: `SEAM_DESIGN.md` §3 (hook H3) · `FEATURE_REQUESTS.md` REQ-001 acceptance criteria · `ARCHITECTURE.md` §4*
