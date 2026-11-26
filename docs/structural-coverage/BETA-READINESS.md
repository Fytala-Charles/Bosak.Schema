# Beta readiness after the structural checkpoint

26 November 2026. This development candidate is suitable for review as a bounded Schema beta candidate after green PR/main checks. It is not ready to advertise general XSD 1.1 conformance or unrestricted application-schema support. The last verified published Schema remains beta.1; no registry publication or release-controller operation is part of this plan.

The implementation qualification passes 21,358 source tests and 19,701 overlapping package-only public tests. The source lanes comprise 1,657 internal and 19,701 public cases. Five original historical case identities have explicitly reconciled admissions; source JSON and all original payloads remain unchanged. API stays 55 exported type names / 663 reflection rows; nine signed Core 1.86.0 payloads verify.

The frozen pilot remains 21 matches. The separate 129-case structural sample improves from 32 matches / 97 Unsupported to 38 / 91, with zero mismatches or harness errors. The independently frozen 598-case datatype sample remains 454 / 144, also with zero errors. Some formerly refused schemas now compile but their original instances still contain excluded nested comments. This is progress in a prerequisite, not an end-to-end admission; the first-refusal delta records it explicitly.

The unchanged public invoice trial passes both owned facades, exact 10/5 reads, original XML export, one-document capacity recovery, cancellation recovery and expired-read refusal. It is representative public data, not production-user acceptance, a general query/PSVI trial or a performance SLA.

The next evidence-selected work should investigate original-node comment/PI handling, then schema annotation processing, before broader anyType/assembly/wildcard design. Preserve original fixture bytes and qualify provenance/path behavior before relaxing those boundaries. The 102 authored contracts are below the authorized 400 ceiling; adding more repetitive cases would not resolve those separate architectural questions.
