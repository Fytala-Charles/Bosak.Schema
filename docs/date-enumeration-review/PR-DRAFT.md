Title: Add bounded date enumeration and adopt Core 1.81.17

Direct xs:date restrictions now enforce enumeration membership using exact Gregorian value equality while retaining original instance components and timezone presence. Existing roots, terminals, attributes and date simple-content extensions use the restricted types. Stray restriction text is refused through the shared schema parser. Schema development dependencies advance to Core1.81.17; public APIs remain unchanged.

Validation:4,874source tests,3,509package-consumer tests,20native checks, six exact packaged examples and supplemental selection/capacity gates pass. Frozen21pilot unchanged; expanded129sample16matches/113Unsupported/zeroerrors. All69previous packets/4,797sealed entries verify. Release/documentation/workload controls pass.

Compatibility: future packages require Core1.81.17. Unchanged retained callers run with migrated Core DLLs/dependency graphs. Legacy124passes; fullalpha23/beta33each retain one old-Core1.81.2assertion failure. Explicit filtered migration runs pass22/32cases; current full surface/authority checks use1.81.17. This does not claim23/33full passes or Schema-only binary replacement.

No package publication, tag or release-controller change. Full XSD1.1/PSVI/C45, general date comparison and other temporal facets remain separate.
