# ADR043: AnyType nil states and independent attributes

Status: qualified source candidate,27November2026.

Permit nil only on admitted anyType declarations and route true nil to original typed-empty states. Factor wildcard attribute assessment so root/descendant nil cannot bypass known value checks or existing unknown-attribute boundaries. Preserve ordinary mixed values and general complex/all-choice/override exclusions. Reconcile original AT-055/056 compile expectations without editing fixtures. [Adoption and qualification](anytype-nillability/README.md). API/Core pin unchanged.
