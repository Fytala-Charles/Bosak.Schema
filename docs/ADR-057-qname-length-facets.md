# ADR057: QName length components

Status: qualified source candidate,28November2026.

Admit bounded QName length/minLength/maxLength schema components while every lexically valid QName remains value-valid for those facets. Enforce inheritance/fixed consistency and the special length/min-max ancestor rule; retain original type/facet namespace authority. The binary component/octet route and NOTATION exclusion remain unchanged. [Scope and qualification](qname-length-facets/README.md). No API/Core pin/publication change.
