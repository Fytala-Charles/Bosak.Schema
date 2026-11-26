# ADR020: bounded exact native duration transport

26 November 2026. Accepted for the owner-authorized DU-701–DU-900 development batch, subject to its qualification and reviewed green merge gates.

Duration values need independent signed months and exact decimal seconds; TimeSpan and .NET decimal cannot carry that XSD value space. Reuse the existing signed Core1.86.0 annotated duration-text carrier for original owned atomization, and keep a bounded private exact component representation in Schema. Add three scalar kinds and three strict immutable getters without modifying existing members. Actual governing/member components remain in the original authority/lifetime universe; detached components carry no schema authority.

Partial order uses all four REC reference instants through arbitrary-precision Gregorian day arithmetic. Derived families retain actual duration ancestry and total month/second order. Component identity determines enumeration/fixed equality. Unresolved lexical and coincident-projection ambiguities remain explicit Unsupported boundaries. General legacy operators, map keys, public query factories, augmentation and complete XSD/PSVI/C45 are outside this decision.

[Adoption and contracts](duration-200/README.md), [consumer guide](duration-200/GUIDE.md). Native text/IEEE/calendar and historical package/controller evidence remain separate. MinVer development minimum advances to0.12; no tag or registry publication is implied.
