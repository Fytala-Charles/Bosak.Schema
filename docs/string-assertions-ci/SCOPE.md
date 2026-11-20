# Hosted lifecycle synchronization correction

20 November 2026. PR33 head b21bec96bcbeda2a4e1e6fad005928da8f8a9e8c.
The first hosted run44816791727 passed all1,366public tests but failed two
pre-existing lifecycle cases waiting10seconds for background work to enter.
Failures were NativeAttributeLifecycleTests and NativeNestedSimpleContentLifecycleTests,
Close_AdmittedWork_WaitsAndRevokesAfterConsumption(closeSession:false), line48.

The tests start gate-holding synchronous work with Task.Run, then block the
coordinator on ManualResetEventSlim. Their close workers also block on the same
gate. Concurrent test classes can delay those queued workers through thread-pool
contention. The timeout logs establish delayed entry, not a product assertion
failure; thread-pool contention is the synchronization diagnosis.

Use a test-only dedicated-worker helper (LongRunning, TaskScheduler.Default) for
this identical blocking pattern across eight lifecycle test classes. Keep all
barriers, assertions, timeouts, case names and product code unchanged. This removes
thread-pool scheduling from the tested gate/close protocol. No test is skipped,
weakened or retried as a substitute for correction.

No product semantic/API change or new specification adoption. Preserve the
original failed hosted log and all sealed review/qualification packets. Run
all lifecycle cases, then the complete1,886case source suite and hosted CI. Earlier
package/frozen-pilot receipts remain historical qualification of identical product
source; do not rewrite their binary hashes after MinVer metadata advances.
