// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Verifies expanded declaration identity through real Core sessions and node facts.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;
using Bosak.XPath.Api;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks namespace-aware state without weakening Core authority boundaries.</summary>
public sealed class NativeNamespaceCoreTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:orders' elementFormDefault='qualified'><xs:element name='order'><xs:complexType><xs:sequence><xs:element name='amount' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private const string Xml="<p:order xmlns:p='urn:orders'><p:amount>12.50</p:amount></p:order>";
    private static NativeSchemaProvider Provider() => new(NativeDecimalSchema.Compile(Schema,new NativeNumericLimits(256,256)).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider provider) => SchemaQuerySession.CompileXPath("$value",new SchemaQueryLimits(1,4,1,256,256),assessor:provider).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession session,NativeSchemaInput input) =>
        (NativeSchemaProvider.Assessment)session.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode : ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime => new(2026,11,19,0,0,0,TimeSpan.Zero);
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes => 0;
    }

    /// <summary>Uses complete names and independently compiled authority identities.</summary>
    [Fact]
    public void DeclarationLookup_UsesNamespaceAndLocalName()
    {
        var provider=Provider();var foreign=Provider();
        Assert.Same(provider.RootDeclaration,provider.ResolveElementDeclaration(new("urn:orders","order"),default).Value);
        Assert.Equal(SchemaResultKind.NotFound,provider.ResolveElementDeclaration(new("","order"),default).Kind);
        Assert.Equal(SchemaResultKind.NotFound,provider.ResolveElementDeclaration(new("urn:Orders","order"),default).Kind);
        Assert.False(provider.Owns(foreign.RootDeclaration));
    }

    /// <summary>Runs actual assessment, guarded annotations, root atomization and exact scalar evaluation.</summary>
    [Fact]
    public void CoreSession_NamespacedDocument_RetainsOriginalFactsAndTypedValue()
    {
        var provider=Provider();using var session=Session(provider);using var input=NativeSchemaInput.Capture(Xml).Value!;
        var result=Assess(session,input);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);
        var root=result.ValidatedRoot!;Assert.Equal("urn:orders",root.NamespaceUri);
        Assert.Equal(("urn:orders","order"),root.SchemaElementDeclaration!.Value);
        Assert.Same(provider.RootDeclaration,result.Facts[0].Declaration);
        Assert.True(root.HasNoTypedValue);Assert.Equal("FOTY0012",result.Facts[0].ReadTypedValue(default).DiagnosticCode);
        using var snapshot=session.OpenSnapshot(input,input,result.Origin).Value;
        var scalar=result.Facts[1].ReadTypedValue(default).Value;
        string value=new NativeSessionRunner(session).Run([snapshot],operation=>
        {
            var read=operation.ReadTypedValue(snapshot,scalar);Assert.Equal(SchemaResultKind.Supported,read.Kind);
            var evaluated=operation.Evaluate(new EvaluationContext().WithVariable("value",read.Value));
            Assert.True(SchemaAtomics.TryGetProvenance(evaluated,out var provenance));
            Assert.Same(result.Origin,provenance!.OriginalAssessment);Assert.Same(provider.DecimalType,provenance.ActualType);
            var error=Assert.Throws<InvalidOperationException>(()=>XPath31Expression.Compile("data(.)").Evaluate(operation.GetRoot(snapshot)).GetEffectiveBooleanValue());
            Assert.StartsWith("FOTY0012:",error.Message,StringComparison.Ordinal);
            return evaluated.ExactDecimalValue.ToInvariantString(256);
        });
        Assert.Equal("12.5",value);Assert.Equal(XName.Get("order","urn:orders"),XDocument.Parse(root.ToXmlString()).Root!.Name);
        input.Dispose();Assert.Throws<ObjectDisposedException>(()=>root.NamespaceUri);
    }

    /// <summary>Equal document and schema strings never grant foreign facts or source identity.</summary>
    [Fact]
    public void CoreSession_EqualForeignInputs_DoNotAcquireAuthority()
    {
        using var first=Session(Provider());using var second=Session(Provider());
        using var a=NativeSchemaInput.Capture(Xml).Value!;using var b=NativeSchemaInput.Capture(Xml).Value!;
        var x=Assess(first,a);var y=Assess(second,b);
        Assert.True(first.Advisor!.OwnsNode(x.ValidatedRoot!,x.Facts[0],a,x.Origin));
        Assert.False(first.Advisor.OwnsNode(x.ValidatedRoot!,y.Facts[0],a,x.Origin));
        Assert.False(first.Advisor.OwnsNode(x.ValidatedRoot!,x.Facts[0],b,x.Origin));
        Assert.False(first.Advisor.OwnsAssessment(y.Origin));
    }
}
