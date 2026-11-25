# Calendar API review

55 exported type names are unchanged. Metadata comparison with the qualified pre-calendar development package finds zero removals and five added reflection rows: three enum constants (`DateTime=7`, `Time=8`, `DateTimeStamp=9`), the `CalendarValue` property and its getter. This row enumeration includes accessors and constructors and is separate from older 413-row inventories. [Exact comparison](API-REVIEW.json).

The returned Core value is detached and immutable, not a transferable schema authority. Existing scalar enum values and constructors remain unchanged; wrong accessors still throw. Query/native capacities and timezone presence are explicit. No public query-session factory is added; internal guarded issuer integration is separately qualified. No general C45/PSVI acceptance is inferred.
