# Beta workload qualification evidence

17 November 2026. Base22aa9ad84959f95d92dc82626117955edd1fdb0b. [Report](../BETA_QUALIFICATION.md), [pre-measurement basis](BASIS.json), [owner scope](SCOPE-DECISION.json), [verification](VERIFICATION.json), [offline verifier](verify.py).

Five workloads,20 child processes,15 first-use samples,150 measured warmed executions and25 warmups. Every workload includes correctness checks; all final mixed-numeric runs satisfy proposed local smoke envelopes. These are measurements, not additional unit-test cases. Product345, release45 and runner6 controls have separate scopes. No engine or test source changed; a non-packable standalone tool and CI build/integrity controls were added.

Source-only evidence retains final raw JSON/binary-hash manifest, hardware/runtime metadata,345-case TRX and tool source. No DLL/package/credential is included. Original integer-only measurements remain in the ignored Core packet and are not pooled into the final summary. Binary/configuration pins and manifest reproduce which built assemblies ran; actual binaries stay ignored. Earlier sealed dossiers remain unchanged and require their own recorded revisions.

Owner selected bounded decimal-session beta with exclusions; ADR009/010, C45/contract acceptance, operating budgets, exact beta compatibility/tag/controller and publication remain pending. One Windows host, pre-cancelled calls only for new cancellation timing, no native heap peak, long leak run or broader platform acceptance. Rawpacket `.guard-tmp/schema-beta-qualification-20261117`. No commit/push, beta tag, release dispatch or publication occurred.
