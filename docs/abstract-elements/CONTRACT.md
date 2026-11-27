# Bounded abstract global element declarations

27 November 2026. Recorded before implementation against PR96 merge0adcae065de77572be0bbc683af80948e751ca90 and signed Core1.86.0. [Immutable adoption](ADOPTION.json).

Admit the abstract Boolean attribute on existing supported global element declarations. Parse true/false/1/0 with XML whitespace collapse and the existing256UTF16 Boolean source bound; bad admitted lexical forms are Invalid and excess capacity is Unsupported. Record abstractness in an immutable set of canonical global declaration identities after compilation. Do not mark shared types or same-named local declarations abstract.

Direct use of an abstract global declaration is Invalid, including a root, a referenced particle or a known global matched beneath anyType lax content. Reject before xsi:type or xsi:nil handling can admit it. Explicit false/0 and omitted abstract retain ordinary existing validity and original type/declaration facts. Unused abstract declarations may compile without invalidating concrete declarations sharing the same type. Failed assessment publishes no document/facts; later concrete assessment recovers without changing compiled metadata.

Substitution groups, abstract complex-type definitions and local abstract attributes remain Unsupported. No new public abstract-flag accessor or full PSVI/C45 acceptance. Existing schema assembly/type/content/value/namespace constraints, nillability, no augmentation, finite limits, XML export, cancellation, foreign authority and expiry remain. Legacy decimal-only grammar remains unchanged.

Qualify both facades and actual native evidence across root/reference/known-lax paths, shared types and same-name locals, Boolean source bounds, xsi/nil and failure recovery. Preserve original inputs/test names and record measured expectation changes separately. Full source/fresh exact package/API/signed Core/original samples/invoice/docs/release controls precede reviewed PR/green merge/main CI. No publication or fullXSD1.1 acceptance implied.
