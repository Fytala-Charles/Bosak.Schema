# Strict wildcard namespace and QName exclusions

Development 0.23 qualifies bounded notNamespace/notQName exclusions on strict empty-owner attribute wildcards, including source-bound QNames and ##defined. All 22,704 source tests (1,729 internal /20,975 public) and20,975 overlapping fresh package consumers pass. All22,570 prior names remain;134 controls are added. Pilot21matches; structural43matches/86Unsupported; datatype582matches/16Unsupported; zeroerrors. API55types/663rows and nine signed Core1.86.0 payloads are unchanged. Unpublished; wider owners/modes/composition and full XSD1.1/PSVI/C45 remain separate.

62 contracts through both facades and10native proofs qualify complement matching, QName namespace bindings, ##defined, explicit-use precedence, schema consistency, original authority and failure boundaries. Four measured public compile expectations change; all original fixture bytes and test names remain.

- [Original contract](CONTRACT.md) and [exact adoption](ADOPTION.json)
- [Consumer guide](GUIDE.md), [contracts](CONTRACTS.json), [test results](TEST-RESULTS.json) and [qualification](QUALIFICATION.json)
- [API review](API-REVIEW.json), [historical reconciliation](HISTORICAL-RECONCILIATION.json), [setup note](TEST-SETUP-CORRECTIONS.json) and [corpus delta](CORPUS-DELTA.json)
- [Current matrix](CURRENT-MATRIX.json), [review](REVIEW.json) and [beta readiness](BETA-READINESS.md)

Run `python docs/wildcard-exclusions/validate.py` for offline packet verification. Package coverage overlaps source; no overall specification percentage is implied.
