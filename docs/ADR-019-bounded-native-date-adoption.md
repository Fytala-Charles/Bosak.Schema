# ADR-019: Bounded native date adoption

**Status:** Adopted for this bounded production candidate; Git delivery/merge pending.
**Date:** 2026-11-24
**Deciders:** Repository owner through the authorized implementation sequence; general Core C45 acceptance remains separate.

## Context

The original ADR018 remains a sealed proposal covering broader calendar transport. Both pinned Core formatters overflow at long.MinValue; the isolated revised date candidate qualifies a symmetric interval excluding that point. DateTime/time still require exact fractional-second transport design. This ADR records the implemented date subset without rewriting historical adoption or proposal receipts.

## Decision

Adopt NativeScalarKind.Date=5, the detached immutable sealed NativeDateValue and NativeScalarValue.DateValue. Preserve existing enum values and401 API rows, with seven additive rows. Year is long; Month/Day are int; nullable int TimezoneOffsetMinutes preserves absence versus explicit zero. There is no public constructor, setter, comparison operator or DateTimeOffset conversion. Detached components confer no schema authority and survive disposal.

Validate bounded xs:date lexicals/calendars in Schema and publish genuine Core Date atoms with original declaration/governing/atomic provenance. Supported interval is [-long.MaxValue,long.MaxValue] with256 normalized lexical characters under existing document/work limits. Capacity refusal remains distinct from invalid calendar/lexical input. Native placements and direct facet-free aliases follow existing grammar. [Exact production adoption and qualification](native-date-implementation/README.md).

## Consequences

This adds bounded application-useful calendar values without changing Core pins or silently narrowing seconds. The excluded year is a documented capacity, not an XSD invalidity. Temporal facets, date complex simple content, broader type chains and dateTime/time remain separate obligations. Public C45/full PSVI, general calendar operations, complete XSD1.1 and release publication are not granted by this ADR. Compatibility, retained callers and exact package examples are qualified in the linked dossier; earlier receipts retain their original state.
