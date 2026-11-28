# QName length-facet compatibility

28 November 2026. Fresh immutable ADOPTION.json recorded before implementation from verified PR114 main.

Admit length/minLength/maxLength on existing QName restrictions. Every lexically valid QName is facet-valid regardless of those numeric values; do not measure prefix text, namespace URI, local name, UTF16 spelling or binary octets. Preserve independent expanded-name enumeration and lexical pattern checks.

Compile the nonnegative bounded numeric components, inherited nondecreasing minLength/nonincreasing maxLength, unchangeable inherited length, fixed per-facet values and min/max ordering. When effective length coexists with min/max, require both numeric consistency and a restriction ancestor with the same min/max value and no length facet. Retain original named/anonymous type links and source namespace scopes. The binary component and octet-value route stays unchanged. NOTATION is excluded.

Existing malformed/duplicate/inapplicable syntax outcomes and all profile limits remain explicit. Complete assessment precedes facts; retain nil, original XML, recovery, cancellation, foreign ownership, detached values and expiry. Preserve prior test names/input bytes and archive measured expectation changes. Run six original upstream sample cases unchanged, with authored variants labeled separately. Fully qualify source and exact-package consumers, API, signed Core, original samples/invoice/docs/release/workload, then authorized commit/push/PR review/green merge/exact-main. No registry/tag/controller or full conformance claim. Next authorized scope: schema default-attribute groups.
