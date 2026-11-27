// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Verifies skip empty-owner projection and absent attribute authority and strict attribute authority.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.SkipEmptyAttributes;
/// <summary>Proves lax attribute types, declaration absence and bounded root projection.</summary>
public sealed class NativeSkipEmptyAttributesProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(bool named=false,bool nested=false,string mode="skip")
    {
        var body="<xs:anyAttribute processContents='"+mode+"'/>";var ct="<xs:complexType>"+body+"</xs:complexType>";var root=nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e'>"+ct+"</xs:element></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r'>"+ct+"</xs:element>";
        if(named)root="<xs:complexType name='T'>"+body+"</xs:complexType><xs:element name='r' type='T'/><xs:element name='s' type='T'/>";
        return "<xs:schema xmlns:xs='"+X+"'><xs:attribute name='a' type='xs:integer'/><xs:attribute name='id' type='xs:ID'/><xs:attribute name='ref' type='xs:IDREF'/>"+root+"</xs:schema>";
    }
    /// <summary>Skipped known and unknown attributes have no declaration and force canonical projection.</summary>
    /// <param name="known">Whether a same-name global exists.</param><param name="named">Named schema owner selector.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void RootAuthority(bool known,bool named)
    {
        using var f=new Fixture(Schema(named),known?"<r a='bad'/>":"<r u=' raw '/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Same(f.Provider.ResolveElementDeclaration(new("","r"),default).Value,owner.Declaration);Assert.Same(f.Provider.ResolveType(new(X,"anyType"),default).Value,owner.Type);Assert.False(owner.NativeType.EmptyContent);var attr=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"",known?"a":"u"))!;Assert.Null(attr.Declaration);Assert.Same(f.Provider.ResolveType(new(X,"anySimpleType"),default).Value,attr.Type);var atom=Assert.Single(attr.ReadTypedValue(default).Value.AtomicItems);Assert.Equal("untypedAtomic",atom.Value.SchemaTypeName);Assert.Equal(known?"bad":" raw ",atom.Value.StringValue);Assert.Same(attr.Type,atom.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"untypedAtomic"),default).Value,atom.TypeEvidence.ActualType);var rootAtom=Assert.Single(owner.ReadTypedValue(default).Value.AtomicItems);Assert.Equal("",rootAtom.Value.StringValue);Assert.Equal("untypedAtomic",rootAtom.Value.SchemaTypeName);if(known)Assert.NotNull(f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value);if(named)Assert.NotSame(f.Provider.ResolveType(new("","T"),default).Value,owner.Type);f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>attr.Declaration);Assert.Throws<ObjectDisposedException>(()=>owner.ReadTypedValue(default));
    }
    /// <summary>Shared aliases remain empty schema types across skipped and no-extra assessments.</summary>
    [Fact]public void AliasRecovery()
    {
        var c=NativeDecimalSchema.Compile(Schema(named:true),new(256,256),maxStringCharacters:1024).Value!;var roots=c.Roots.Where(p=>p.Root.Name is "r" or "s").ToArray();Assert.Same(roots[0].Root.Type,roots[1].Root.Type);foreach(var name in new[]{"r","s"}){Assert.True(c.Assess("<"+name+" a='bad'/>").Value!.Elements[0].GoverningType.IsAnyType);Assert.True(roots[0].Root.Type.EmptyContent);Assert.True(c.Assess("<"+name+"/>").Value!.Elements[0].GoverningType.EmptyContent);}using var session=NativeSchemaSession.Create(Schema(named:true),new(256,256,1024,65536,2,256,16,256,1024)).Value!;using(var d=session.Assess("<r a='bad'/>",new(DateTimeOffset.UnixEpoch,0)).Value!){Assert.True(d.HasRootScalar);Assert.Equal("bad",d.ReadAttribute(0).Value.StringValue);}using(var d=session.Assess("<s/>",new(DateTimeOffset.UnixEpoch,0)).Value!){Assert.False(d.HasRootScalar);Assert.Empty(d.ReadRootItems());}
    }
    /// <summary>Explicit uses retain original authority and validation over the skip wildcard.</summary>
    [Fact]public void ExplicitPrecedence()
    {
        var schema=Schema().Replace("<xs:anyAttribute","<xs:attribute ref='a'/><xs:anyAttribute");using var good=new Fixture(schema,"<r a='001'/>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);Assert.True(good.Input.FactsFor((NativeSessionNode)good.Element([]))!.NativeType.EmptyContent);Assert.NotNull(good.Input.FactsFor((NativeSessionNode)good.Attribute([],"","a"))!.Declaration);Assert.Equal("1",good.Read(good.Attribute([],"","a")).IntegerValue.ToString());using var bad=new Fixture(schema,"<r a='bad'/>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);
    }
    /// <summary>Skipped ID names supply no ID evidence to an explicitly validated reference.</summary>
    [Fact]public void IdentityBoundary()
    {
        using var skipped=new Fixture(Schema(),"<r id='bad id' ref='missing'/>");Assert.Equal(SchemaAssessmentStatus.Valid,skipped.Assessment.Status);Assert.Null(skipped.Input.FactsFor((NativeSessionNode)skipped.Attribute([],"","id"))!.Declaration);var schema=Schema().Replace("<xs:anyAttribute","<xs:attribute ref='ref'/><xs:anyAttribute");using var explicitRef=new Fixture(schema,"<r id='v' ref='v'/>");Assert.Equal(SchemaAssessmentStatus.Invalid,explicitRef.Assessment.Status);Assert.False(explicitRef.Input.IsAssessed);
    }
    /// <summary>Any skipped extra below a strict ancestor remains Unsupported, while no extras succeed.</summary>
    [Fact]public void NestedBoundary()
    {
        foreach(var attr in new[]{"a='bad'","u='x'"}){using var f=new Fixture(Schema(nested:true),"<r><e "+attr+"/></r>");Assert.Equal(SchemaAssessmentStatus.Unsupported,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);}using var empty=new Fixture(Schema(nested:true),"<r><e/></r>");Assert.Equal(SchemaAssessmentStatus.Valid,empty.Assessment.Status);Assert.True(empty.Input.FactsFor((NativeSessionNode)empty.Element([0]))!.NativeType.EmptyContent);
    }
    /// <summary>Known skipped text remains bounded and refusal permits recovery.</summary>
    [Fact]public void Capacity()
    {
        var c=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:3).Value!;var failed=c.Assess("<r a='abcd'/>");Assert.Equal(NativeOutcome.Unsupported,failed.Outcome);Assert.Null(failed.Value);Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,failed.UnsupportedReason);Assert.Equal(NativeOutcome.Valid,c.Assess("<r a='abc'/>").Outcome);
    }
    /// <summary>Original empty validity is checked before projecting the root.</summary>
    [Fact]public void EmptyValidity()
    {
        foreach(var xml in new[]{"<r a='bad'> </r>","<r a='bad'><c/></r>"}){using var f=new Fixture(Schema(),xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);}
    }
    /// <summary>Canonical types coexist with an existing anyType root.</summary>
    [Fact]public void CanonicalCoexistence()
    {
        var schema=Schema(named:true).Replace("</xs:schema>","<xs:element name='other'/></xs:schema>");using var f=new Fixture(schema,"<r a='bad'/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Same(f.Provider.ResolveType(new(X,"anyType"),default).Value,f.Input.FactsFor((NativeSessionNode)f.Element([]))!.Type);
    }
    /// <summary>Skipped facts preserve cancellation and foreign authority.</summary>
    [Fact]public void Ownership()
    {
        using var f=new Fixture(Schema(),"<r a='bad'/>");using var other=new Fixture(Schema(),"<r a='bad'/>");var local=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","a"))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Attribute([],"","a"))!;Assert.False(f.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(local.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>local.ReadTypedValue(cancel.Token));
    }
}
