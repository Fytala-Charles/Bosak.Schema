// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Tests owned compiler snapshots and nested descriptor collection mutation refusal.
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

using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Tests owned compiler snapshots and nested descriptor collection mutation refusal.</summary>
public sealed class DescriptorSnapshotTests
{
    private const string Schema = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:p="urn:lower" xmlns:P="urn:upper" targetNamespace="urn:test" defaultAttributes="defaults">
          <xs:defaultOpenContent mode="interleave" appliesToEmpty="true"><xs:any notNamespace="##local urn:blocked" notQName="##defined"/></xs:defaultOpenContent>
          <xs:attributeGroup name="defaults"><xs:attribute name="flag" type="xs:string"/></xs:attributeGroup>
          <xs:simpleType name="Positive"><xs:restriction base="xs:decimal"><xs:assertion test="$value gt 0"/></xs:restriction></xs:simpleType>
          <xs:complexType name="T" defaultAttributesApply="false">
            <xs:openContent mode="suffix"><xs:any namespace="urn:other" notQName="##defined"/></xs:openContent>
            <xs:sequence><xs:element name="amount" type="xs:decimal"/></xs:sequence>
            <xs:assert test="true()"/>
          </xs:complexType>
          <xs:element name="root" type="T"><xs:alternative test="@kind eq 'a'" type="T"/><xs:alternative type="T"/></xs:element>
        </xs:schema>
        """;

    /// <summary>Checks all outer descriptor collections reject ordinary mutable-interface changes.</summary>
    [Fact]
    public void Compile_AllOuterCollections_RefuseMutation()
    {
        var model = Compile(Schema);
        Assert.Empty(model.Diagnostics);
        Frozen(model.Assertions);
        Frozen(model.Alternatives);
        Frozen(model.ElementDeclarations);
        Frozen(model.SimpleTypes);
        Frozen(model.OpenContents);
        Frozen(model.DefaultOpenContents);
        Frozen(model.DefaultAttributeReferences);
        Frozen(model.DefaultAttributeOptOuts);
        Frozen(model.Diagnostics);
    }

    /// <summary>Checks error models and empty models use the same immutable publication boundary.</summary>
    [Theory]
    [InlineData("<not-schema/>", true)]
    [InlineData("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'/>", false)]
    public void Compile_EmptyOrInvalidModel_RefusesCollectionMutation(string xml, bool diagnostics)
    {
        var model = Compile(xml);
        Assert.Equal(diagnostics, model.Diagnostics.Count != 0);
        Frozen(model.Assertions);
        Frozen(model.Diagnostics);
        Frozen(model.SimpleTypes);
    }

    /// <summary>Checks namespace bindings are immutable while preserving ordinal prefix identity.</summary>
    [Fact]
    public void Compile_AssertionAndAlternativeBindings_RefuseMutation()
    {
        var model = Compile(Schema);
        foreach (var bindings in model.Assertions.Select(a => a.NamespaceBindings).Concat(model.Alternatives.Select(a => a.NamespaceBindings)))
        {
            Assert.NotNull(bindings);
            Assert.Equal("urn:lower", bindings["p"]);
            Assert.Equal("urn:upper", bindings["P"]);
            var mutable = Assert.IsAssignableFrom<IDictionary<string, string>>(bindings);
            Assert.Throws<NotSupportedException>(() => mutable["p"] = "urn:changed");
            Assert.Throws<NotSupportedException>(() => mutable.Clear());
            Assert.Equal("urn:lower", bindings["p"]);
        }
    }

    /// <summary>Checks facet lists freeze without breaking the documented shared assertion instances.</summary>
    [Fact]
    public void Compile_FacetAssertions_FreezeAndRetainSharedIdentity()
    {
        var model = Compile(Schema);
        var simple = Assert.Single(model.SimpleTypes);
        Frozen(simple.Assertions);
        var facet = Assert.Single(simple.Assertions);
        Assert.Same(Assert.Single(model.Assertions, a => a.Context == AssertionContext.SimpleTypeAssertion), facet);
        var service = new XsdValidationService();
        Assert.True(service.ValidateValue(model, XName.Get("Positive", "urn:test"), "2").IsValid);
        Assert.False(service.ValidateValue(model, XName.Get("Positive", "urn:test"), "-2").IsValid);
    }

    /// <summary>Checks all wildcard namespace and exclusion token collections are immutable.</summary>
    [Fact]
    public void Compile_OpenAndDefaultWildcards_RefuseTokenMutation()
    {
        var model = Compile(Schema);
        foreach (var wildcard in model.OpenContents.Concat(model.DefaultOpenContents).Select(d => d.Wildcard!))
        {
            Frozen(wildcard.NamespaceConstraint);
            Frozen(wildcard.NotNamespace);
            Frozen(wildcard.NotQName);
        }
        Assert.Equal("urn:other", Assert.Single(model.OpenContents).Wildcard!.NamespaceConstraint[0]);
        Assert.Equal(new[] { "##local", "urn:blocked" }, Assert.Single(model.DefaultOpenContents).Wildcard!.NotNamespace);
    }

    /// <summary>Checks returned data is detached from the XML document supplied through a reader.</summary>
    [Fact]
    public void Compile_SourceXmlMutation_DoesNotRewriteSnapshot()
    {
        var source = XDocument.Parse(Schema);
        ICompiledSchema model;
        using (var reader = source.CreateReader())
            model = new SchemaCompiler().Compile(reader, new Uri("file:///snapshot.xsd"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
        var digest = model.Sha256Digest;
        source.Root!.SetAttributeValue(XNamespace.Xmlns + "p", "urn:changed");
        source.Descendants(XName.Get("assertion", "http://www.w3.org/2001/XMLSchema")).Single().SetAttributeValue("test", "false()");
        Assert.Equal("urn:lower", model.Assertions[0].NamespaceBindings!["p"]);
        Assert.Equal("$value gt 0", model.SimpleTypes[0].Assertions[0].TestExpression);
        Assert.Equal(digest, model.Sha256Digest);
    }

    /// <summary>Checks record copies preserve public compatibility without mutating the original snapshot.</summary>
    [Fact]
    public void RecordWith_CallerOwnedReplacement_DoesNotAliasOriginalCollections()
    {
        var model = Assert.IsType<CompiledSchema>(Compile(Schema));
        var originalCount = model.Assertions.Count;
        var callerList = model.Assertions.ToList();
        var copy = model with { Assertions = callerList };
        callerList.Clear();
        Assert.Empty(copy.Assertions);
        Assert.Equal(originalCount, model.Assertions.Count);
        Frozen(model.Assertions);
    }

    /// <summary>Checks independent compilations retain equivalent ordered content and no shared mutable state.</summary>
    [Fact]
    public void Compile_RepeatedSchema_PreservesDigestOrderAndIndependentSnapshot()
    {
        var a = Compile(Schema);
        var b = Compile(Schema);
        Assert.Equal(a.Sha256Digest, b.Sha256Digest);
        Assert.Equal(a.SchemaId, b.SchemaId);
        Assert.Equal(a.Assertions.Select(x => x.TestExpression), b.Assertions.Select(x => x.TestExpression));
        Assert.NotSame(a.Assertions[0], b.Assertions[0]);
        Frozen(a.Assertions);
        Assert.Equal(2, b.Assertions.Count);
    }

    /// <summary>Checks finite concurrent readers evaluate one immutable model without caller mutation.</summary>
    [Fact]
    public async Task Snapshot_ParallelFacetReaders_RetainValues()
    {
        var model = Compile(Schema);
        var service = new XsdValidationService();
        var results = await Task.WhenAll(Enumerable.Range(1, 8).Select(i => Task.Run(() =>
            service.ValidateValue(model, XName.Get("Positive", "urn:test"), i.ToString(System.Globalization.CultureInfo.InvariantCulture)).IsValid)));
        Assert.All(results, Assert.True);
        Assert.Equal("urn:lower", model.Assertions[0].NamespaceBindings!["p"]);
    }

    private static ICompiledSchema Compile(string xml)
    {
        using var reader = XDocument.Parse(xml).CreateReader();
        return new SchemaCompiler().Compile(reader, new Uri("file:///snapshot.xsd"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
    }

    private static void Frozen<T>(IReadOnlyList<T> values)
    {
        var count = values.Count;
        var mutable = Assert.IsAssignableFrom<IList<T>>(values);
        Assert.Throws<NotSupportedException>(() => mutable.Clear());
        if (count > 0) Assert.Throws<NotSupportedException>(() => mutable[0] = values[0]);
        Assert.Equal(count, values.Count);
    }
}
