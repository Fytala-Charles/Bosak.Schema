# Beta.1 preparation basis

17 November 2026. Owner instruction: “Please continue with your steps 1 - 4”.

Product source is the accepted PR12 merge `2a5abf6fb663313dc32bfeca5bbb8740bbe987ae`; main build44174792275 passed. Proposed tag `v0.5.0-beta.1` must peel to that exact commit. The first qualification uses a tag only in an isolated local clone; no remote tag or NuGet publication is authorized by this preparation.

This change adopts no new XML/XSD semantics. Immutable behavior/specification evidence remains in the accepted source dossiers and ADR009/010. It updates only release admission: normal MinVer beta identity, exact source/tag, 345 source cases, 33 package-only cases, retained legacy124 and published-alpha23 control/replacement cases, and nine signed Core1.81.2 dependency payloads. Existing alpha publication receipts remain unchanged.

Controller and product source are deliberately separate. A local rehearsal manifest has a zero controller identity and is not hosted publication evidence. Hosted preparation requires delivery of the controller and the reviewed remote tag; its fresh artifact/hash must then be reviewed separately before publication.
