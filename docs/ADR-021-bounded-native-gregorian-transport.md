# ADR021: bounded native Gregorian fragment transport

26 November 2026. Accepted for the owner-authorized GR-901–GR-1300 development batch, subject to qualification and reviewed passing merge gates.

Use the existing signed Core1.86.0 annotated string carrier for exact original gYear/gYearMonth/gMonth/gMonthDay/gDay values. Keep private immutable component records in Schema, and add five scalar kinds plus nullable year/month/day/timezone and collapsed-text getters. Missing fields remain null in public values; no authority is exported.

Implement REC D.2.1 completion and partial ordering with BigInteger day arithmetic. Completion fields never become public supplied fields. Equality, identity and primitive disjointness remain distinct, including stronger identity checks for global attribute fixed overrides. Keep explicit raw/year/facet/graph/lifetime capacities and Unsupported cases. The adopted equal-exclusive-bound correction also updates earlier floating/duration schema checks without rewriting historical evidence.

[Adoption and contracts](gregorian-400/README.md), [guide](gregorian-400/GUIDE.md). MinVer minimum advances to0.13; no Core change, general legacy operators, full PSVI/C45/public query factory or registry publication is implied.
