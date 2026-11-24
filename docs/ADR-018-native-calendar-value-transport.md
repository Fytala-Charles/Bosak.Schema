# ADR-018: Native calendar value transport

**Status:** Proposed; API review, transport feasibility and semantic adoption pending.
**Date:** 2026-11-24
**Deciders:** Repository owner; public Core C45 acceptance remains separate.

## Context

REQ-009 prioritizes application-useful XSD 1.1 coverage across Bosak and Schema. Integer enumeration is merged in PR45. The selected calendar sample contains nine Unsupported cases, but their other requirements prevent a feature-isolated admission forecast. Schema's four scalar kinds have no temporal accessor. Both pinned Core XPathDateTime sources carry milliseconds, optional timezone and a long year; component construction does not prove validation or every operation's correctness.

## Proposed decision

Qualify xs:date first using an additive sealed detached NativeDateValue with long Year, int Month, int Day and nullable int TimezoneOffsetMinutes, exposed by NativeScalarValue.DateValue and NativeScalarKind.Date=5. Keep construction internal. Preserve actual Core Date atoms and declaration/governing/atomic provenance through both native providers and public facades. No DateTimeOffset conversion or inferred timezone. Proposed bounds are a signed Int64 year and256 normalized lexical characters, with Unsupported capacity outcomes and no loss.

Resolve exact decimal seconds in the shared Core carrier before enabling broader dateTime/time transport. A separate Core proposal and its conformance gates are required if the pinned carrier cannot represent the accepted value; a detached-only type or formatted string is not a substitute for exact original-node atomization. [Detailed contract and stop conditions](date-time-transport/CONTRACT.md).

## Alternatives and consequences

DateTimeOffset alone loses timezone absence and extended years; milliseconds alone lose higher fractional precision. Returning temporal text as String would hide the scalar family and lose genuine typed-value transport. A single struct with optional date/time fields would introduce invalid default/component combinations. The proposed sealed date object avoids those states and leaves later exact time/dateTime shapes reviewable. It adds API surface, so compatibility and consumer review precede implementation; it is not accepted merely because this document exists.

Date/time comparison, facets, dateTimeStamp, Gregorian fragments, durations, typed queries, public PSVI/C45 and general arithmetic remain separate. Original export lexicals, lifetime and authority boundaries remain required. No API/source/package change is made by this ADR.
