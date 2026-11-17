# Restricted schema file resolution

17 November 2026. Local additive beta-readiness implementation; not yet published. [Pre-change basis](restricted-resolution/BASIS.json), [qualification](restricted-resolution/README.md). Existing convenience factories and textual URI-prefix policy retain their alpha behavior.

## Explicit restricted entry points

```csharp
var directory = Path.GetFullPath("schemas");
var resolver = new RestrictedSchemaFileResolver(directory, maxDocumentBytes: 64 * 1024);
var options = new SchemaCompileOptions(XsdLanguageVersion.Xsd11, resolver);
// The caller separately bounds and owns the root input stream.
var model = SchemaServices.CreateSchemaCompiler().Compile(rootStream, rootUri, options);
// Inspect model.Diagnostics; descriptor compilation is not full structural validation.

var xslt = SchemaAwareXslt.CreateRestrictedCompiler(directory, maxDocumentBytes: 64 * 1024);
var executable = xslt.Compile(stylesheetText, new Uri(Path.Combine(directory, "main.xsl")).AbsoluteUri);
```

Use an absolute existing directory and a positive explicit per-document byte limit. Relative XSLT schema locations resolve against the supplied stylesheet/module base URI, not implicitly against the directory argument. For direct resolver calls, supply an absolute file URI. The resolver returns a detached read-only stream at position zero; the caller disposes it.

The resolver checks canonical directory containment and the exact captured root spelling, so a Windows URI spelling the root with different casing is conservatively refused. This avoids using platform-insensitive comparison as authority on case-sensitive Windows directories. Parent references that normalize inside the root may work; escapes and sibling-prefix paths do not. UNC/hosted/network URIs, credentials, query/fragment, Windows alternate streams and trailing-dot/space aliases are refused. Root, ancestor, directory and file reparse points are refused. Files must be ordinary trusted host-maintained files.

Reads enforce the byte cap while buffering, then parse the complete XML with DTD prohibited and no external resolver before returning bytes. UTF-16 DTDs and DTDs following long prologs are refused; this is not an8192-byte text scan. No full XSD validity is inferred from well-formed XML. A denied path/size/XML produces SchemaResolutionException with BSK-XSD-004 and a fixed message; unavailable files produce BSK-XSD-006. Null URI and invalid construction arguments throw argument exceptions. No file path/parser detail is copied into these resolver messages; surrounding Core receipts may still identify requested URIs.

## Controlled XSLT behavior

The opt-in factory uses published Core1.81.2 ControlledResourcePolicy, not the fallback-capable SchemaResolver callback. Its authority approves only Schema-route bytes satisfying the same resolver, including nested xs:include/import/redefine resolution. All other resource acquisition routes are denied; stylesheet module acquisition, document/text/network reads and other workflows need a separately reviewed policy. No custom fetcher is called and there is no fallback to the legacy convenience callback. The returned Core compiler remains mutable: a host replacing ResourcePolicy or injecting other authority changes this contract.

Resource denial and whole-schema validity are different. The existing Core/.NET pipeline can treat a denied unused nested include as a warning and still compile. A required type in that denied document causes compilation failure in the qualification. Inspect ResourcePolicy.Receipts for denied acquisitions; compile success is not proof that every schema hint was resolved. This slice changes neither Core's warning rules nor native finite-schema grammar.

## Host boundary and limits

The directory and all filesystem ancestors must be host-controlled and stable during checking/opening. Reparse-point checks and FileStream opening are not atomic; this is not protection against a malicious local actor replacing paths concurrently. Hard-link provenance, mount ownership, special-file attacks, total-memory/time budgets and concurrent file mutation are outside this contract. Do not expose a writable root to untrusted users and call it a sandbox. Buffered bytes detach later consumer reads from the source file, but do not guarantee an atomic source snapshot during acquisition.

The per-document cap does not bound total imports or root stylesheet/schema input; legacy compiler depth/count policy still applies to its imports. There is no cancellation parameter or measured IO latency guarantee in this resolver. Native decimal sessions retain their separate string/XML limits. Core references stay1.81.2. No beta, tag, engine-gate change or release is introduced.
