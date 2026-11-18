# Beta.1 publication receipt

18 November 2026. [Machine-readable receipt](RECEIPT.json) records the approved hosted preparation, NuGet publication, repository signature/payload checks, registry-only consumer, exact product tag and GitHub prerelease assets. [Current release guide](../BETA1_RELEASE.md).

The unsigned hosted package and the signed registry download have different outer hashes. All nine approved entries were byte-identical; NuGet added only `.signature.p7s`. The signed registry archive was attached to GitHub, and both GitHub asset digests were checked before and after publication. Earlier local/alpha/preparation/contract dossiers retain their original dates, identities and scope. This receipt supersedes pending-publication statements for current release status only.

[Representative invoice trial](../BETA1_INVOICE_TRIAL.md) and [receipt](INVOICE-TRIAL.json) are separate consumer observations, not additive product coverage or production acceptance.

Run `python docs/beta1-publication/verify.py` to verify the retained source-only archive and normalized document hashes offline. This check does not rebuild, rerun consumers or publish a package. The archive contains authored diagnostic source, public fixtures and original logs/receipts; no assemblies, packages or credentials are included. Rendering checks are programmatic HTML/link checks, not screenshot acceptance.
