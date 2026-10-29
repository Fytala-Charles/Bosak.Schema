# Schema 0.8 count assertion review — 29 October 2026

The bounded count implementation is reviewed and ready for checkpoint. No additional production correction was needed. Six implementation source identities match the sealed [implementation qualification](COUNT-ASSERTION-IMPLEMENTATION.md). A fresh complete Schema run passes **397/397**, with no failures or skips; its raw TRX is retained under `evidence/`.

Review covered complete grammar admission, count/child name disambiguation, depth/token/character limits, v4 selection from admitted syntax, validation before assertion execution, and narrow cardinality-error handling. Existing ownership, original declarations, typed copies and ambiguity refusal remain covered. The one count-refusal expectation migration is recorded in the original implementation dossier; historical receipts remain unchanged.

Core production stays 1.34.1. Its source-identical qualification is inherited: 24,214 unit passes, QT3 31,397/0/424, qt4 8,941/0/1,918 across 230 sets. These are not fresh engine runs. No release or main-branch merge is included.

Next: implement the separately scoped [presence assertions](PRESENCE-ASSERTION-PLAN.md). This checkpoint does not implement those functions. Core delivery commit: `git log -1 --format=%H -- docs/schema-count-review/README.md`. Schema delivery commit: `git log -1 --format=%H -- docs/COUNT-ASSERTION-REVIEW.md`. Raw review receipts are retained in the core count-review dossier.
