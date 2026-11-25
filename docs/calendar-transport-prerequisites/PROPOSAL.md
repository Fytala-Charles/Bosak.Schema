# Exact calendar transport proposal

The signed Core 1.81.17 package stores calendar fractional seconds as an integer Millisecond. The package-only diagnostic records eight exact component round trips, eight exact millisecond DTO controls, 24 submillisecond precision losses and two distinct-input collisions. Six native facade checks refuse dateTime/time/dateTimeStamp without returning a session. These measurements describe public component transport, not XSD lexical validation or newly supported schema datatypes.

A future Core contract should introduce an additive exact fractional-second representation, for example a nonnegative integer coefficient and decimal scale under explicit capacity limits. Preserve existing carriers and their compatibility; do not silently change their precision or imply that DateTimeOffset is lossless. Decide the public representation only after an API review, including conversion policies and explicit refusal when exact values exceed configured capacity.

Specify calendar validation separately from component storage: extended year ranges and year zero, timezone presence versus explicit UTC, the +/-14:00 boundary, 24:00 normalization, fractional seconds, leap-second policy, and dateTimeStamp derivation with required timezone. Equality, identity, hashing, canonical serialization and arithmetic need distinct documented rules; equal instants do not imply identical timezone annotations. Do not inherit unboundedness or .NET year limits accidentally.

The bridge must preserve the actual atomic type, governing type, source-node authority, document lifetime and original lexical content. Verify exact Core scalar transport before admitting any Schema type. Current date-only support and its explicit exclusions remain separate.

Before implementation: adopt immutable normative sections, select representation/capacities and review compatibility. Then implement happy, boundary and refusal cases in Core; run its required unit tests, frozen QT3 31,397/0/424 and gated qt4 checks. Only after signed package availability should a separately adopted Schema slice qualify native/session/package/retained/corpus behavior against that pin. C45/public Core API acceptance and broader PSVI remain pending.

This batch changes no Core production source or dependency pin and publishes no package. This proposal is not an approved API or a promise of complete XSD 1.1 conformance.
