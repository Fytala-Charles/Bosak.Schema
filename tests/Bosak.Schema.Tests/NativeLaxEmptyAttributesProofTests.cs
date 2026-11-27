// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Verifies lax empty-owner projection and absent attribute authority and strict attribute authority.
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
namespace Bosak.Schema.Tests.LaxEmptyAttributes;
/// <summary>Proves lax attribute types, declaration absence and bounded root projection.</summary>
public sealed class NativeLaxEmptyAttributesProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(bool named=false,bool nested=false,string mode="lax")
    {
        var body="<xs:anyAttribute processContents='"+mode+"'/>";var ct="<xs:complexType>"+body+"</xs:complexType>";var root=nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e'>"+ct+"</xs:element></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r'>"+ct+"</xs:element>";
        if(named)root="<xs:complexType name='T'>"+body+"</xs:complexType><xs:element name='r' type='T'/><xs:element name='s' type='T'/>";
        return "<xs:schema xmlns:xs='"+X+"'><xs:attribute name='a' type='xs:integer'/><xs:attribute name='id' type='xs:ID'/><xs:attribute name='ref' type='xs:IDREF'/>"+root+"</xs:schema>";
    }
    /// <summary>Known-only owners retain empty typing; unknown attributes require canonical projection.</summary>
    /// <param name="known">Whether the attribute has an original global declaration.</param><param name="named">Named schema type selector.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void RootAuthority(bool known,bool named)
    {
        using var f=new Fixture(Schema(named),known?"<r a='001'/>":"<r u=' 01.20 '/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Same(f.Provider.ResolveElementDeclaration(new("","r"),default).Value,owner.Declaration);Assert.False(owner.HasNoTypedValue);Assert.Equal(known,owner.NativeType.EmptyContent);Assert.Equal(!known,owner.NativeType.IsAnyType);
        var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"",known?"a":"u"))!;
        if(known){Assert.Empty(owner.ReadTypedValue(default).Value.AtomicItems);Assert.Same(f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value,a.Declaration);Assert.Equal("1",f.Read(f.Attribute([],"","a")).IntegerValue.ToString());if(named)Assert.Same(f.Provider.ResolveType(new("","T"),default).Value,owner.Type);}
        else{Assert.Null(a.Declaration);Assert.Same(f.Provider.ResolveType(new(X,"anySimpleType"),default).Value,a.Type);Assert.Same(f.Provider.ResolveType(new(X,"anyType"),default).Value,owner.Type);var atom=Assert.Single(a.ReadTypedValue(default).Value.AtomicItems);Assert.Equal("untypedAtomic",atom.Value.SchemaTypeName);Assert.Equal(" 01.20 ",atom.Value.StringValue);Assert.Same(a.Type,atom.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"untypedAtomic"),default).Value,atom.TypeEvidence.ActualType);var rootAtom=Assert.Single(owner.ReadTypedValue(default).Value.AtomicItems);Assert.Equal("",rootAtom.Value.StringValue);Assert.Equal("untypedAtomic",rootAtom.Value.SchemaTypeName);Assert.Same(owner.Type,rootAtom.TypeEvidence.DeclaredType);Assert.Same(atom.TypeEvidence.ActualType,rootAtom.TypeEvidence.ActualType);if(named)Assert.NotSame(f.Provider.ResolveType(new("","T"),default).Value,owner.Type);}
        f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>owner.ReadTypedValue(default));Assert.Throws<ObjectDisposedException>(()=>a.Declaration);
    }
    /// <summary>Known integer and undeclared text facts remain independent in one partial episode.</summary>
    [Fact]public void MixedAttributes()
    {
        using var f=new Fixture(Schema(),"<r a='001' u=' raw '/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal("1",f.Read(f.Attribute([],"","a")).IntegerValue.ToString());Assert.Equal(" raw ",f.Read(f.Attribute([],"","u")).StringValue);Assert.NotNull(f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","a"))!.Declaration);Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","u"))!.Declaration);
    }
    /// <summary>Aliases share schema types while partial projection remains assessment-local.</summary>
    [Fact]public void AliasesAndRecovery()
    {
        var c=NativeDecimalSchema.Compile(Schema(named:true),new(256,256),maxStringCharacters:1024).Value!;var roots=c.Roots.Where(p=>p.Root.Name is "r" or "s").ToArray();Assert.Same(roots[0].Root.Type,roots[1].Root.Type);foreach(var name in new[]{"r","s"}){var partial=c.Assess("<"+name+" u='x'/>");Assert.Equal(NativeOutcome.Valid,partial.Outcome);Assert.True(partial.Value!.Elements[0].GoverningType.IsAnyType);Assert.True(roots[0].Root.Type.EmptyContent);Assert.True(c.Assess("<"+name+" a='1'/>").Value!.Elements[0].GoverningType.EmptyContent);}
        using var s=NativeSchemaSession.Create(Schema(named:true),new(256,256,1024,65536,2,256,16,256,1024)).Value!;using(var d=s.Assess("<r u='x'/>",new(DateTimeOffset.UnixEpoch,0)).Value!){Assert.True(d.HasRootScalar);Assert.Equal("",d.ReadRootScalar().StringValue);}using(var d=s.Assess("<s a='1'/>",new(DateTimeOffset.UnixEpoch,0)).Value!){Assert.False(d.HasRootScalar);Assert.Empty(d.ReadRootItems());}
    }
    /// <summary>Known-only lax attributes on nested empty owners preserve the original empty type.</summary>
    [Fact]public void NestedKnown()
    {
        using var f=new Fixture(Schema(nested:true),"<r><e a='001'/></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.True(owner.NativeType.EmptyContent);Assert.False(owner.NativeType.IsAnyType);Assert.Empty(owner.ReadTypedValue(default).Value.AtomicItems);Assert.Equal("1",f.Read(f.Attribute([0],"","a")).IntegerValue.ToString());
    }
    /// <summary>Unknown attributes below strict ancestors refuse before publication.</summary>
    [Fact]public void NestedUnknown()
    {
        using var f=new Fixture(Schema(nested:true),"<r><e u='x'/></r>");Assert.Equal(SchemaAssessmentStatus.Unsupported,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);
    }
    /// <summary>Invalid known globals do not become skipped in a partial root.</summary>
    [Fact]public void KnownInvalid()
    {
        using var f=new Fixture(Schema(),"<r a='bad' u='x'/>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);
    }
    /// <summary>Original empty validity runs before projected anyType can expose values.</summary>
    [Fact]public void EmptyValidity()
    {
        foreach(var xml in new[]{"<r u='x'> </r>","<r u='x'><c/></r>"}){using var f=new Fixture(Schema(),xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);}
    }
    /// <summary>Unknown lexical capacity refusal publishes no result and later input recovers.</summary>
    [Fact]public void Capacity()
    {
        var c=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:3).Value!;var failed=c.Assess("<r u='abcd'/>");Assert.Equal(NativeOutcome.Unsupported,failed.Outcome);Assert.Null(failed.Value);Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,failed.UnsupportedReason);Assert.Equal(NativeOutcome.Valid,c.Assess("<r u='abc'/>").Outcome);
    }
    /// <summary>Known ID/IDREF constraints remain active with declaration-absent siblings.</summary>
    [Fact]public void Identity()
    {
        using var good=new Fixture(Schema(),"<r id='v' ref='v' u='x'/>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);using var bad=new Fixture(Schema(),"<r ref='missing' u='x'/>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);
    }
    /// <summary>Canonical anyType registration is shared with an existing anyType root.</summary>
    [Fact]public void CanonicalCoexistence()
    {
        var schema=Schema(named:true).Replace("</xs:schema>","<xs:element name='other'/></xs:schema>");using var f=new Fixture(schema,"<r u='x'/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Same(f.Provider.ResolveType(new(X,"anyType"),default).Value,f.Input.FactsFor((NativeSessionNode)f.Element([]))!.Type);
    }
    /// <summary>Projected and absent facts retain foreign-authority and cancellation boundaries.</summary>
    [Fact]public void Ownership()
    {
        using var f=new Fixture(Schema(),"<r u='x'/>");using var other=new Fixture(Schema(),"<r u='x'/>");var local=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","u"))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Attribute([],"","u"))!;Assert.False(f.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(local.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>local.ReadTypedValue(cancel.Token));
    }
}
