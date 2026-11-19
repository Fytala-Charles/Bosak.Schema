// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Verifies named type component identity through actual Core assessment.
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

using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks named type authority and original facts through actual Core APIs.</summary>
public sealed class NativeNamedTypeCoreTests
{
    private const string Schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='urn:orders' targetNamespace='urn:orders' elementFormDefault='qualified'><xs:element name='order' type='t:OrderType'/><xs:element name='quote' type='t:OrderType'/><xs:complexType name='OrderType'><xs:sequence><xs:element name='amount' type='xs:decimal'/></xs:sequence></xs:complexType><xs:complexType name='Unused'><xs:sequence><xs:element name='value' type='xs:decimal'/></xs:sequence></xs:complexType></xs:schema>";
    private static NativeSchemaProvider Provider() => new(NativeDecimalSchema.Compile(Schema, new NativeNumericLimits(256, 256)).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider provider) => SchemaQuerySession.CompileXPath("$value", new SchemaQueryLimits(1,4,1,256,256), assessor:provider).Value;
    private static NativeSchemaInput Input(string name) => NativeSchemaInput.Capture($"<o:{name} xmlns:o='urn:orders'><o:amount>12.50</o:amount></o:{name}>").Value!;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession session, NativeSchemaInput input, ISchemaComponentHandle? governing = null) =>
        (NativeSchemaProvider.Assessment)session.Advisor!.Assess(new(input, SchemaAssessmentMode.Strict, null, governing, new Episode(), null), default);
    private sealed record Episode : ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime => new(2026,11,19,0,0,0,TimeSpan.Zero);
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes => 0;
    }

    /// <summary>Shares one named type across distinct original root declarations.</summary>
    [Fact]
    public void SharedNamedType_RetainsTypeAndDeclarationIdentity()
    {
        var provider=Provider();using var session=Session(provider);
        using var first=Input("order");using var second=Input("quote");
        var a=Assess(session,first);var b=Assess(session,second);
        Assert.Equal(SchemaAssessmentStatus.Valid,a.Status);Assert.Equal(SchemaAssessmentStatus.Valid,b.Status);
        var named=provider.ResolveType(new("urn:orders","OrderType"),default).Value;
        Assert.Same(named,a.Facts[0].Type);Assert.Same(named,b.Facts[0].Type);
        Assert.NotSame(a.Facts[0].Declaration,b.Facts[0].Declaration);
        Assert.Same(provider.ResolveElementDeclaration(new("urn:orders","quote"),default).Value,b.Facts[0].Declaration);
        Assert.Equal(("urn:orders","OrderType"),b.ValidatedRoot!.SchemaTypeAnnotation!.Value);
        Assert.Equal(("urn:orders","quote"),b.ValidatedRoot.SchemaElementDeclaration!.Value);
        Assert.True(provider.IsSameComponent(a.Facts[0].Type,b.Facts[0].Type,default).Value);
        Assert.False(provider.IsSameComponent(a.Facts[0].Type,a.Facts[0].Declaration,default).Value);
    }

    /// <summary>Named type lookup uses expanded names and preserves unused components.</summary>
    [Fact]
    public void Lookup_UsesExpandedTypeNameAndSeparateAuthority()
    {
        var provider=Provider();var foreign=Provider();
        var type=provider.ResolveType(new("urn:orders","OrderType"),default).Value;
        Assert.Equal(SchemaResultKind.Unsupported,provider.ResolveType(new("urn:other","OrderType"),default).Kind);
        Assert.Equal(SchemaResultKind.Unsupported,provider.ResolveType(new("","OrderType"),default).Kind);
        Assert.Equal(SchemaResultKind.Supported,provider.ResolveType(new("urn:orders","Unused"),default).Kind);
        var other=foreign.ResolveType(new("urn:orders","OrderType"),default).Value;
        Assert.False(provider.Owns(other));Assert.NotSame(type,other);
        Assert.Throws<ArgumentException>(()=>provider.IsSameComponent(type,other,default));
    }

    /// <summary>Named element-only roots retain FOTY0012 while decimal leaves retain exact provenance.</summary>
    [Fact]
    public void NamedRoot_HasNoTypedValue_AndLeafRemainsExact()
    {
        var provider=Provider();using var session=Session(provider);using var input=Input("quote");
        var result=Assess(session,input);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);
        Assert.True(result.ValidatedRoot!.HasNoTypedValue);
        var error=Assert.Throws<Bosak.XPath.Runtime.Vm.XPathErrorException>(()=>result.ValidatedRoot.TypedValue);
        Assert.Equal("FOTY0012",error.CodeLocalName);
        Assert.Equal("FOTY0012",result.Facts[0].ReadTypedValue(default).DiagnosticCode);
        var leaf=result.Facts[1].ReadTypedValue(default).Value;
        Assert.True(leaf.TryGetSingleAtomic(out var item));Assert.Equal("12.5",item.Value.ExactDecimalValue.ToInvariantString(256));
        Assert.Same(provider.DecimalType,item.TypeEvidence.ActualType);
        Assert.Equal(("http://www.w3.org/2001/XMLSchema","decimal"),input.ElementNodes[1].SchemaTypeAnnotation!.Value);
        using var snapshot=session.OpenSnapshot(input,input,result.Origin).Value;
        var value=new NativeSessionRunner(session).Run([snapshot],op=>op.ReadTypedValue(snapshot,leaf).Value);
        Assert.Equal("12.5",value.ExactDecimalValue.ToInvariantString(256));
        input.Dispose();Assert.Throws<ObjectDisposedException>(()=>result.ValidatedRoot.SchemaTypeAnnotation);
    }

    /// <summary>A supplied declaration must govern the actual selected root and cannot publish partial facts.</summary>
    [Fact]
    public void GoverningDeclaration_MustMatchSelectedRoot()
    {
        var provider=Provider();using var session=Session(provider);using var input=Input("quote");
        var wrong=provider.ResolveElementDeclaration(new("urn:orders","order"),default).Value;
        var refused=Assess(session,input,wrong);Assert.Equal(SchemaAssessmentStatus.Unsupported,refused.Status);
        Assert.Null(refused.Evidence);Assert.Null(refused.ValidatedRoot);Assert.Empty(refused.Facts);
        var right=provider.ResolveElementDeclaration(new("urn:orders","quote"),default).Value;
        Assert.Equal(SchemaAssessmentStatus.Valid,Assess(session,input,right).Status);
    }

    /// <summary>Equal named types never grant access to foreign original facts or inputs.</summary>
    [Fact]
    public void NamedType_ForeignFactsAndDisposalRemainGuarded()
    {
        using var first=Session(Provider());using var second=Session(Provider());using var a=Input("order");using var b=Input("order");
        var x=Assess(first,a);var y=Assess(second,b);
        Assert.False(first.Advisor!.OwnsAssessment(y.Origin));
        Assert.False(first.Advisor.OwnsNode(x.ValidatedRoot!,y.Facts[0],a,x.Origin));
        Assert.False(first.Advisor.OwnsNode(x.ValidatedRoot!,x.Facts[0],b,x.Origin));
        a.Dispose();Assert.Throws<ObjectDisposedException>(()=>x.Facts[0].Type);
    }
}
