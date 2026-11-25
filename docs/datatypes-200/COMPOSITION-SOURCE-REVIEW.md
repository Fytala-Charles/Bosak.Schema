# Pre-implementation composition source review

The immutable adoption remains the dated XSD1.1 Datatypes Recommendation, 5 April 2012, body SHA-256 `cc2ae20cdeab02600c73fd13f1cc31dbbdd61f62aa455026fa84388b540443b8`. This review adds precision to the existing adoption; it does not replace its historical source identity.

Before list/union semantic implementation, review of §2.4.1.3 and Appendix I confirms that XSD1.1 explicitly permits a union with zero member types. Its value and lexical spaces are empty. The initial DT-239 planning label “Reject absent members” was inaccurate for an otherwise well-formed empty union. DT-239 will instead admit its schema definition and reject every supplied value as Invalid. The original label remains in the prior atomic PR commit and in the ledger correction metadata. This is a planning correction, not a test expectation changed to accommodate an implementation.

Other adopted details retained here: union members are attempted in their declared order; nested unions eventually select a basic atomic or list member; list item types must not have lists among their transitive union members; union transitive membership cannot contain itself. The finite profile will stop unsupported/capacity results rather than treating them as an invalid member alternative. Closed dependency and inline bounds remain explicit. This review does not add identity-graph builtins or arbitrary recursive support.

Additional implementation bounds recorded before changes: at most16 inline compound-member definitions in the source, inline construction depth4, and4096 shared compound assessment attempts per schema-literal compilation budget or per document-assessment budget. Each attempt checks cancellation and consumes the existing shared pattern work budget too. These independent limits prevent repeated union alternatives in an otherwise finite shared type graph from causing exponential unbounded work. Capacity exhaustion is Unsupported and cannot trigger union fallback.

Source: [dated XSD1.1 Datatypes §2.4.1.3](https://www.w3.org/TR/2012/REC-xmlschema11-2-20120405/#union-datatypes).

Before instance-selection changes: named restrictions of the declared compound type are admitted under existing ancestry rules. Selecting a union member directly through xsi:type remains Unsupported pending a dedicated qualification of union restriction facets under member override; valid member ancestry must not be mislabeled Invalid.

Review-pin correction: the initial adoption ledger's list/union section label “3.2.2/3.2.3” is a numbering error. The corresponding passages in the same immutable adopted body are §2.4.1.2/§2.4.1.3; simple-type component derivation is Structures§3.16. The original adoption record and its bytes remain unchanged. This clarification changes no source revision or claimed adoption date.

Before compound fixed-override changes: Datatypes§2.2.2 specifies schema sameness using equality or identity. New compound attribute fixed overrides therefore use compound value equality, including singleton-list/atomic equality and remembered calendar offsets that represent the same value. Earlier atomic calendar/date override behavior and its sealed evidence remain unchanged; this batch does not claim to resolve that legacy bounded deviation. Per-item actual type provenance is independent of value equality.
