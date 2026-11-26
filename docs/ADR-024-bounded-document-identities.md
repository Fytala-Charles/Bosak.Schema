# ADR024: bounded document identity validation

Accepted for the development 0.16 candidate, 26 November 2026.

Adopt the immutable REC sections and hashes recorded before implementation in [ADOPTION](identity-coverage/ADOPTION.json). After successful local value assessment, build an assessment-local identity table from actual selected atomic/member types. ID attributes bind their owning element, ID elements bind their parent within the validation root, and all identity strings must have exactly one distinct binding element. Repeated bindings to the same owner are permitted. Graph failure precedes any successful document/fact publication.

Reuse existing owned string/list transport and component handles without public API additions. Preserve original XML and fixed redacted public diagnostics. Bound raw lexical values, distinct strings and occurrences explicitly; refuse capacity overflow and default/fixed identity constraints. Do not infer key/keyref/unique, DTD/xml:id or public fn:id support. See [guide](identity-coverage/GUIDE.md) for scope and [qualification](identity-coverage/QUALIFICATION.json) for evidence.
