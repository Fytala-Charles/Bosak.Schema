// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : REQ-009 S1 security tests: resolution policy enforcement and bounded import resolution.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation (REQ-009 S1)                                                                    |

//                      | Charles Korthout | 0.2   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Text;
using System.Xml;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>
/// Security tests for the REQ-009 S1 compile pipeline: the default-deny resolution posture
/// (no network, no DTD, optional URI-prefix allowlist, per-document byte cap) and the bounded
/// import/include resolution (depth and per-compile count limits, explicit denial diagnostics).
/// </summary>
public class SecurityTests : IDisposable
{
    private readonly List<string> _tempFiles = [];

    [Fact]
    public void Open_HttpUriUnderLocalFilesPolicy_ThrowsResolutionDenied()
    {
        var resolver = new SecureSchemaResolver(SchemaResolutionPolicy.LocalFiles);

        var ex = Assert.Throws<SchemaResolutionException>(() => resolver.Open(new Uri("https://schemas.example.com/types.xsd")));
        Assert.Equal(SchemaDiagnosticCodes.ResolutionDenied, ex.Code);
    }

    [Fact]
    public void Open_UriOutsideAllowedPrefixes_ThrowsResolutionDenied()
    {
        var policy = SchemaResolutionPolicy.LocalFiles with { AllowedUriPrefixes = ["file:///allowed/"] };
        var resolver = new SecureSchemaResolver(policy);

        Assert.Throws<SchemaResolutionException>(() => resolver.Open(new Uri("file:///elsewhere/types.xsd")));
    }

    [Fact]
    public void Open_DocumentExceedingByteCap_ThrowsResolutionDenied()
    {
        var path = WriteTempFile(new string('x', 4096));
        var policy = SchemaResolutionPolicy.LocalFiles with { MaxDocumentBytes = 1024 };
        var resolver = new SecureSchemaResolver(policy);

        var ex = Assert.Throws<SchemaResolutionException>(() => resolver.Open(new Uri(path)));
        Assert.Equal(SchemaDiagnosticCodes.ResolutionDenied, ex.Code);
    }

    [Fact]
    public void Open_DocumentWithDoctypeUnderDtdDenyPolicy_ThrowsResolutionDenied()
    {
        var path = WriteTempFile("<!DOCTYPE schema [<!ENTITY e 'v'>]><xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"/>");
        var resolver = new SecureSchemaResolver(SchemaResolutionPolicy.LocalFiles);

        var ex = Assert.Throws<SchemaResolutionException>(() => resolver.Open(new Uri(path)));
        Assert.Equal(SchemaDiagnosticCodes.ResolutionDenied, ex.Code);
    }

    [Fact]
    public void Open_DocumentWithDoctypeUnderDtdAllowPolicy_OpensStream()
    {
        var path = WriteTempFile("<!DOCTYPE schema [<!ENTITY e 'v'>]><xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"/>");
        var policy = new SchemaResolutionPolicy(AllowLocalFiles: true, AllowNetwork: false, AllowDtd: true, MaxDocumentBytes: 1024 * 1024);
        var resolver = new SecureSchemaResolver(policy);

        using var stream = resolver.Open(new Uri(path));
        Assert.True(stream.Length > 0);
    }

    [Fact]
    public void Compile_ImportWithoutResolver_EmitsResolutionUnavailableDiagnostic()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:import namespace="urn:dep" schemaLocation="dep.xsd"/>
            </xs:schema>
            """;

        var compiled = new SchemaCompiler().Compile(SchemaComponentTests.ToStream(document), new Uri("file:///test/root.xsd"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));

        var diagnostic = Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.ResolutionUnavailable);
        Assert.Contains("dep.xsd", diagnostic.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Compile_ImportUnderDenyAllResolver_EmitsResolutionDeniedDiagnostic()
    {
        const string document = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:import namespace="urn:dep" schemaLocation="dep.xsd"/>
            </xs:schema>
            """;
        var options = new SchemaCompileOptions(XsdLanguageVersion.Xsd11, new SecureSchemaResolver(SchemaResolutionPolicy.DenyAll));

        var compiled = new SchemaCompiler().Compile(SchemaComponentTests.ToStream(document), new Uri("file:///test/root.xsd"), options);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.ResolutionDenied);
    }

    [Fact]
    public void Compile_ImportChainExceedingDepthLimit_EmitsResolutionLimitDiagnostic()
    {
        const string root = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:include schemaLocation="a.xsd"/>
            </xs:schema>
            """;
        var resolver = new StubResolver(uri => ChainDocument(uri, "a.xsd", "b.xsd", "c.xsd", "d.xsd"));
        var options = new SchemaCompileOptions(XsdLanguageVersion.Xsd11, resolver, MaxImportDepth: 2);

        var compiled = new SchemaCompiler().Compile(SchemaComponentTests.ToStream(root), new Uri("file:///test/root.xsd"), options);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.ResolutionLimit);
    }

    [Fact]
    public void Compile_ResolutionCountExceedingPerCompileLimit_EmitsResolutionLimitDiagnostic()
    {
        const string root = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
              <xs:include schemaLocation="g1.xsd"/>
            </xs:schema>
            """;
        var resolver = new StubResolver(uri =>
        {
            var name = Path.GetFileNameWithoutExtension(uri.LocalPath);
            return name.StartsWith('g') && int.Parse(name[1..]) < 70
                ? IncludeDocument($"g{int.Parse(name[1..]) + 1}.xsd")
                : EmptySchemaDocument;
        });
        var options = new SchemaCompileOptions(XsdLanguageVersion.Xsd11, resolver, MaxImportDepth: 128);

        var compiled = new SchemaCompiler().Compile(SchemaComponentTests.ToStream(root), new Uri("file:///test/root.xsd"), options);

        Assert.Single(compiled.Diagnostics, d => d.Code == SchemaDiagnosticCodes.ResolutionLimit);
    }

    [Fact]
    public void Compile_StreamWithDoctype_ThrowsXmlException()
    {
        const string document = "<!DOCTYPE xs:schema><xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"/>";

        Assert.Throws<XmlException>(() =>
            new SchemaCompiler().Compile(SchemaComponentTests.ToStream(document), new Uri("file:///test/root.xsd"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11)));
    }

    public void Dispose()
    {
        foreach (var file in _tempFiles)
        {
            File.Delete(file);
        }

        GC.SuppressFinalize(this);
    }

    private string WriteTempFile(string content)
    {
        var path = Path.Combine(Path.GetTempPath(), $"bosak-schema-test-{Guid.NewGuid():N}.xsd");
        File.WriteAllText(path, content);
        _tempFiles.Add(path);
        return path;
    }

    private const string EmptySchemaDocument = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"/>
        """;

    private static string IncludeDocument(string next) =>
        $"""<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"><xs:include schemaLocation="{next}"/></xs:schema>""";

    private static string ChainDocument(Uri requested, params string[] chain)
    {
        for (var i = 0; i < chain.Length - 1; i++)
        {
            if (Path.GetFileName(requested.LocalPath) == chain[i])
            {
                return IncludeDocument(chain[i + 1]);
            }
        }

        return EmptySchemaDocument;
    }

    private sealed class StubResolver : ISchemaResourceResolver
    {
        private readonly Func<Uri, string> _content;

        internal StubResolver(Func<Uri, string> content)
        {
            _content = content;
        }

        public Stream Open(Uri resolvedUri)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(_content(resolvedUri)));
        }
    }
}
