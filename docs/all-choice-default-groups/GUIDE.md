# Default groups on bounded all and choice owners

Both owned native facades support active schema default groups on existing bounded root/named all and choice complex owners. The source schema QName and per-type Boolean flags retain their original namespace scopes and independent semantics. Original cached group uses are combined with explicit uses by actual declaration identity; shared repeats are deduplicated and distinct same-name uses are Invalid. Canonical global attribute references remain original.

The existing model parsers and recognizers are unchanged. All retains unordered physical member acceptance, optional/required members and duplicate rejection. Choice retains the bounded repeated-single-branch closure and rejects mixed alternatives. Original physical order, child declarations, native types and scalar values remain; default attributes do not reorder or create XML nodes.

Existing effective-use16, source/member/branch/occurrence/depth/schema/string/regex/atom/value/lifetime limits remain. Required default-group uses remain required even when a content group is optional. Optional attribute absence is supported; missing default/fixed-valued attributes still require augmentation and remain Unsupported. Provided values retain existing equality/typing rules.

Active wildcard, mixed, assertion, simple-content and complex-content combinations remain excluded. Nested all/choice and broader particle/occurrence grammar are unchanged. The separate empty/finite-sequence opt-out closed-attribute policy is not broadened; an all/choice opt-out with zero uses retains the earlier ordinary-extra-attribute refusal. Legacy decimal-only behavior and source acquisition are unchanged.

Complete assessment precedes facts. Original owner/member/attribute identities, no-facts failure, XML/export, cancellation, foreign ownership, recovery, detached values and expiry retain their contracts. The 98 authored contracts run on both facades; 14 native proofs verify real source/type/value authority and limits. Original sample cases remain unchanged and are reported separately. No public API/Core pin or publication change is included.
