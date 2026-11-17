// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Tests restricted schema paths, bounded XML and controlled XSLT acquisition.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 17-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Diagnostics;
using System.Text;
using System.Xml.Linq;
using Bosak.XPath.Runtime.Resources;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Tests restricted schema paths, bounded XML and controlled XSLT acquisition.</summary>
public sealed class RestrictedResolutionTests : IDisposable
{
    private const string Schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:test'><xs:element name='value' type='xs:decimal'/></xs:schema>";
    private readonly string directory = Path.Combine(Path.GetTempPath(), "bosak-restricted-" + Guid.NewGuid().ToString("N"));
    private readonly List<string> files = [];
    private readonly List<string> directories = [];
    private readonly List<string> links = [];
    private string Allowed => Path.Combine(directory, "allowed");

    /// <summary>Creates private fixture directories without acquiring user files.</summary>
    public RestrictedResolutionTests()
    {
        foreach (var path in new[] { directory, Allowed, Path.Combine(Allowed, "nested"), Path.Combine(directory, "allowed-sibling") })
        {
            Directory.CreateDirectory(path);
            directories.Add(path);
        }
    }

    /// <summary>Verifies open containedfile returnsdetachedreadonlyexactbytes.</summary>
    [Fact]
    public void Open_ContainedFile_ReturnsDetachedReadonlyExactBytes()
    {
        var path = Write(Path.Combine(Allowed, "nested", "schema.xsd"), Schema);
        using var stream = new RestrictedSchemaFileResolver(Allowed, Encoding.UTF8.GetByteCount(Schema)).Open(new Uri(path));
        Assert.False(stream.CanWrite);
        Assert.Equal(0, stream.Position);
        File.WriteAllText(path, "changed");
        Assert.Equal(Schema, new StreamReader(stream).ReadToEnd());
    }

    /// <summary>Verifies constructor nonpositivecapacity throws.</summary>
    [Theory]
    [InlineData(0)] [InlineData(-1)]
    public void Constructor_NonpositiveCapacity_Throws(int capacity) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new RestrictedSchemaFileResolver(Allowed, capacity));

    /// <summary>Verifies constructor relativeormissingdirectory refuses.</summary>
    [Fact]
    public void Constructor_RelativeOrMissingDirectory_Refuses()
    {
        Assert.Throws<ArgumentException>(() => new RestrictedSchemaFileResolver("relative", 1024));
        Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Path.Combine(directory, "missing"), 1024));
    }

    /// <summary>Verifies open outsiderootorsiblingprefix refuses.</summary>
    [Theory]
    [InlineData("outside.xsd")]
    [InlineData("allowed-sibling/schema.xsd")]
    public void Open_OutsideRootOrSiblingPrefix_Refuses(string relative)
    {
        var path = Write(Path.Combine(directory, relative), Schema);
        var error = Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Allowed, 4096).Open(new Uri(path)));
        Assert.Equal(SchemaDiagnosticCodes.ResolutionDenied, error.Code);
        Assert.DoesNotContain(path, error.Message, StringComparison.Ordinal);
    }

    /// <summary>Verifies open nonlocalorrelativeuri refusesbeforeio.</summary>
    [Theory]
    [InlineData("https://example.invalid/schema.xsd")]
    [InlineData("file://remote-host/share/schema.xsd")]
    [InlineData("relative.xsd")]
    public void Open_NonlocalOrRelativeUri_RefusesBeforeIo(string uri) =>
        Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Allowed, 4096).Open(new Uri(uri, UriKind.RelativeOrAbsolute)));

    /// <summary>Verifies open queryorfragment refuses.</summary>
    [Theory]
    [InlineData("?query=1")]
    [InlineData("#fragment")]
    public void Open_QueryOrFragment_Refuses(string suffix)
    {
        var path = Write(Path.Combine(Allowed, "schema.xsd"), Schema);
        Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Allowed, 4096).Open(new Uri(new Uri(path).AbsoluteUri + suffix)));
    }

    /// <summary>Verifies open percentencodedparenttraversal refuses.</summary>
    [Fact]
    public void Open_PercentEncodedParentTraversal_Refuses()
    {
        Write(Path.Combine(directory, "outside.xsd"), Schema);
        var uri = new Uri(new Uri(Allowed + Path.DirectorySeparatorChar), "%2e%2e/outside.xsd");
        Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Allowed, 4096).Open(uri));
    }

    /// <summary>Verifies open malformedordtdincludingutf16 refuses.</summary>
    [Theory]
    [InlineData("<schema", false)]
    [InlineData("<!DOCTYPE schema><schema/>", false)]
    [InlineData("<!DOCTYPE schema><schema/>", true)]
    public void Open_MalformedOrDtdIncludingUtf16_Refuses(string content, bool utf16)
    {
        var path = Path.Combine(Allowed, "schema.xsd");
        if (utf16)
        {
            File.WriteAllText(path, content, Encoding.Unicode);
            files.Add(path);
        }
        else Write(path, content);
        Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Allowed, 4096).Open(new Uri(path)));
    }

    /// <summary>Verifies open dtdafterlongprolog refuses.</summary>
    [Fact]
    public void Open_DtdAfterLongProlog_Refuses()
    {
        var path = Write(Path.Combine(Allowed, "schema.xsd"), "<!--" + new string('x', 9000) + "--><!DOCTYPE schema><schema/>");
        Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Allowed, 20000).Open(new Uri(path)));
    }

    /// <summary>Verifies open onebyteovercapacity refuses.</summary>
    [Fact]
    public void Open_OneByteOverCapacity_Refuses()
    {
        var path = Write(Path.Combine(Allowed, "schema.xsd"), Schema);
        Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Allowed, Encoding.UTF8.GetByteCount(Schema) - 1).Open(new Uri(path)));
    }

    /// <summary>Verifies open missingfile returnsunavailablewithoutfallback.</summary>
    [Fact]
    public void Open_MissingFile_ReturnsUnavailableWithoutFallback()
    {
        var error = Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Allowed, 4096).Open(new Uri(Path.Combine(Allowed, "missing.xsd"))));
        Assert.Equal(SchemaDiagnosticCodes.ResolutionUnavailable, error.Code);
    }

    /// <summary>Verifies links rootandnesteddirectoryreparsepoints refuse.</summary>
    [Fact]
    public void Links_RootAndNestedDirectoryReparsePoints_Refuse()
    {
        var outside = Path.Combine(directory, "allowed-sibling");
        Write(Path.Combine(outside, "schema.xsd"), Schema);
        var link = Path.Combine(Allowed, "linked");
        if (OperatingSystem.IsWindows())
        {
            var start = new ProcessStartInfo("cmd.exe") { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true };
            foreach (var arg in new[] { "/c", "mklink", "/J", link, outside }) start.ArgumentList.Add(arg);
            using var child = Process.Start(start)!;
            if (!child.WaitForExit(10000))
            {
                child.Kill(entireProcessTree: true);
                Assert.Fail("Fixture junction creation did not finish.");
            }
            Assert.Equal(0, child.ExitCode);
        }
        else Directory.CreateSymbolicLink(link, outside);
        links.Add(link);
        Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(link, 4096));
        Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Allowed, 4096).Open(new Uri(Path.Combine(link, "schema.xsd"))));
    }

    /// <summary>Verifies legacycompiler restrictedimport opensallowedanddiagnosesescape.</summary>
    [Fact]
    public void LegacyCompiler_RestrictedImport_OpensAllowedAndDiagnosesEscape()
    {
        Write(Path.Combine(Allowed, "schema.xsd"), Schema);
        Write(Path.Combine(directory, "outside.xsd"), Schema);
        foreach (var (hint, allowed) in new[] { ("schema.xsd", true), ("../outside.xsd", false) })
        {
            using var source = new MemoryStream(Encoding.UTF8.GetBytes($"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:import namespace='urn:test' schemaLocation='{hint}'/></xs:schema>"));
            var result = new SchemaCompiler().Compile(source, new Uri(Path.Combine(Allowed, "root.xsd")), new SchemaCompileOptions(XsdLanguageVersion.Xsd11, new RestrictedSchemaFileResolver(Allowed, 4096)));
            Assert.Equal(allowed, result.Diagnostics.Count == 0);
            if (!allowed) Assert.Contains(result.Diagnostics, d => d.Code == SchemaDiagnosticCodes.ResolutionDenied);
        }
    }

    /// <summary>Verifies restrictedxslt allowedschema compilesthroughcontrolledpolicy.</summary>
    [Fact]
    public void RestrictedXslt_AllowedSchema_CompilesThroughControlledPolicy()
    {
        Write(Path.Combine(Allowed, "schema.xsd"), Schema);
        var compiler = SchemaAwareXslt.CreateRestrictedCompiler(Allowed, 4096);
        compiler.Compile(Stylesheet("schema.xsd"), new Uri(Path.Combine(Allowed, "main.xsl")).AbsoluteUri);
        Assert.Contains(compiler.ResourcePolicy!.Receipts, r => r.Route == ControlledResourceRoute.Schema && r.Outcome == ControlledResourceReceiptOutcome.Approved);
    }

    /// <summary>Verifies restrictedxslt deniedormissingschema neverusesconveniencefallback.</summary>
    [Theory]
    [InlineData("../outside.xsd")]
    [InlineData("missing.xsd")]
    [InlineData("https://example.invalid/schema.xsd")]
    public void RestrictedXslt_DeniedOrMissingSchema_NeverUsesConvenienceFallback(string hint)
    {
        Write(Path.Combine(directory, "outside.xsd"), Schema);
        var compiler = SchemaAwareXslt.CreateRestrictedCompiler(Allowed, 4096);
        var fallbackCalled = false;
        compiler.SchemaResolver = (_, _) => { fallbackCalled = true; return new MemoryStream(Encoding.UTF8.GetBytes(Schema)); };
        Assert.ThrowsAny<Exception>(() => compiler.Compile(Stylesheet(hint), new Uri(Path.Combine(Allowed, "main.xsl")).AbsoluteUri));
        Assert.False(fallbackCalled);
        Assert.Contains(compiler.ResourcePolicy!.Receipts, r => r.Outcome == ControlledResourceReceiptOutcome.Denied);
    }

    /// <summary>Verifies restrictedxslt nestedschemareferences usesamepolicy.</summary>
    [Theory]
    [InlineData("nested/child.xsd", true)]
    [InlineData("../outside.xsd", false)]
    public void RestrictedXslt_NestedSchemaReferences_UseSamePolicy(string hint, bool allowed)
    {
        Write(Path.Combine(Allowed, "nested", "child.xsd"), Schema);
        Write(Path.Combine(directory, "outside.xsd"), Schema);
        Write(Path.Combine(Allowed, "schema.xsd"), $"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:test'><xs:include schemaLocation='{hint}'/></xs:schema>");
        var compiler = SchemaAwareXslt.CreateRestrictedCompiler(Allowed, 4096);
        if (allowed) compiler.Compile(Stylesheet("schema.xsd"), new Uri(Path.Combine(Allowed, "main.xsl")).AbsoluteUri);
        else
        {
            // XmlSchemaSet may warn rather than fail when an unavailable include contributes no
            // referenced component. The resource must still be denied without approving its bytes.
            compiler.Compile(Stylesheet("schema.xsd"), new Uri(Path.Combine(Allowed, "main.xsl")).AbsoluteUri);
            Assert.Contains(compiler.ResourcePolicy!.Receipts, r => r.Outcome == ControlledResourceReceiptOutcome.Denied);
            Assert.DoesNotContain(compiler.ResourcePolicy.Receipts, r => r.Outcome == ControlledResourceReceiptOutcome.Approved && r.RequestedUri.EndsWith("outside.xsd", StringComparison.Ordinal));
        }
    }

    /// <summary>Verifies restrictedxslt deniednestedtypeisrequired compilationfails.</summary>
    [Fact]
    public void RestrictedXslt_DeniedNestedTypeIsRequired_CompilationFails()
    {
        Write(Path.Combine(directory, "outside.xsd"), "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:test'><xs:simpleType name='OutsideType'><xs:restriction base='xs:decimal'/></xs:simpleType></xs:schema>");
        Write(Path.Combine(Allowed, "schema.xsd"), "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='urn:test' targetNamespace='urn:test'><xs:include schemaLocation='../outside.xsd'/><xs:element name='value' type='t:OutsideType'/></xs:schema>");
        var compiler = SchemaAwareXslt.CreateRestrictedCompiler(Allowed, 4096);
        Assert.ThrowsAny<Exception>(() => compiler.Compile(Stylesheet("schema.xsd"), new Uri(Path.Combine(Allowed, "main.xsl")).AbsoluteUri));
        Assert.Contains(compiler.ResourcePolicy!.Receipts, r => r.Outcome == ControlledResourceReceiptOutcome.Denied);
    }

    /// <summary>Verifies open windowsambiguouscomponents refuse.</summary>
    [Theory]
    [InlineData("schema.xsd:stream")]
    [InlineData("schema.xsd.")]
    [InlineData("schema.xsd ")]
    public void Open_WindowsAmbiguousComponents_Refuse(string name)
    {
        if (!OperatingSystem.IsWindows()) return;
        Write(Path.Combine(Allowed, "schema.xsd"), Schema);
        Assert.Throws<SchemaResolutionException>(() => new RestrictedSchemaFileResolver(Allowed, 4096).Open(new Uri(Path.Combine(Allowed, name))));
    }

    /// <summary>Verifies restrictedxslt otheracquisitionroutes deny.</summary>
    [Fact]
    public void RestrictedXslt_OtherAcquisitionRoutes_Deny()
    {
        var compiler = SchemaAwareXslt.CreateRestrictedCompiler(Allowed, 4096);
        var request = new ControlledResourceRequest(ControlledResourceRoute.ModuleInclude, new Uri(Path.Combine(Allowed, "module.xsl")).AbsoluteUri);
        Assert.True(compiler.ResourcePolicy!.Authority.Authorize(request)!.IsDenied);
        const string stylesheet = "<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='3.0'><xsl:include href='module.xsl'/></xsl:stylesheet>";
        Write(Path.Combine(Allowed, "module.xsl"), "<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='3.0'/>");
        Assert.ThrowsAny<Exception>(() => compiler.Compile(stylesheet, new Uri(Path.Combine(Allowed, "main.xsl")).AbsoluteUri));
        Assert.Contains(compiler.ResourcePolicy.Receipts, r => r.Route == ControlledResourceRoute.ModuleInclude && r.Outcome == ControlledResourceReceiptOutcome.Denied);
    }

    private static string Stylesheet(string hint) => $"<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='3.0'><xsl:import-schema namespace='urn:test' schema-location='{hint}'/><xsl:template name='main'/></xsl:stylesheet>";
    private string Write(string path, string content)
    {
        File.WriteAllText(path, content, new UTF8Encoding(false));
        files.Add(path);
        return path;
    }

    /// <summary>Removes only explicitly created fixture entries, without recursive deletion.</summary>
    public void Dispose()
    {
        foreach (var link in links) Directory.Delete(link, false);
        foreach (var file in files.Distinct()) File.Delete(file);
        foreach (var path in directories.OrderByDescending(p => p.Length)) Directory.Delete(path, false);
    }
}
