# PR33 hosted lifecycle correction

20 November 2026. [Scope and diagnosis](SCOPE.md),
[original failed run](evidence/ORIGINAL-HOSTED-RUN.json),
[original failure log](evidence/original-hosted-failure.log), and
[correction/results](evidence/RESULTS.json).

The first hosted run at b21bec9 failed two existing lifecycle cases before their
background work entered; all 1,366 public cases passed. Blocking Task.Run workers
were subject to thread-pool scheduling while parallel test coordinators waited.
A shared test-only LongRunning worker now starts the same synchronous gate/close
work on dedicated threads across the eight classes using that pattern. Timeouts,
barriers, assertions and every case identity stay unchanged.

All 25 lifecycle cases pass locally, then the complete 1,886-source catalog passes
(520 implementation / 1,366 public), with exactly the same named outcomes as the
qualified source run. No tests are skipped or downgraded. Nine test files change;
product source/configuration/API and public test/fixture inputs are unchanged.

This separate packet preserves the original failed hosted log and all earlier
sealed qualification/review receipts. The original unsigned package, API, pilot
and signature evidence remains attached to its original source/version identity;
MinVer metadata may advance on later builds. No product semantic change,
additional specification adoption, package publication or Core change is made.
