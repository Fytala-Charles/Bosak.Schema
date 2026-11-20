# Delivery review — 20 November 2026

Reviewed the two private production-file changes, 47 public consumer cases,
original upstream corpus/notices, immutable pre-code adoption, candidate source
and runtime binding, preserved historical receipts and documentation limits.
The opt-in profile flag controls both hint names at all admitted owners; legacy
decimal, type/nil requests, undeclared attributes, scalar/facet checks and typed
slot ordering keep their previous behavior. No new public API or resolver exists.

One review finding was fixed: the default baseline source check used Git diff,
which omitted untracked production files. It now also refuses those files before
launch. Two controls cover untracked refusal and the clean baseline. All **15
harness controls pass**, superseding the 13-control checkpoint for current tooling;
the original 13-control receipt stays unchanged. Production/runtime bytes did not
change during this review, so the original **1,302 passing source tests** remain
the applicable regression receipt. No unnecessary source rerun was performed.

The exact candidate rerun reproduces **1 validity match / 20 Unsupported / zero
mismatches/errors**. Only ctB073.v is admitted; four invalid xsi:type cases remain
Unsupported. Both sealed original evidence packets were rechecked byte-for-byte.
This does not qualify package-only/retained consumer lanes or general XSD PSVI.

[Proposed PR title](PR-TITLE.txt) and [body](PR-BODY.md) are ready for delivery.
No staging, commit, push or PR creation occurred. AGENTS.md Git Rules require an
explicit user request before commit/push. After delivery and hosted review, the
next semantic slice is separately adopted anonymous local decimal/string restrictions.
