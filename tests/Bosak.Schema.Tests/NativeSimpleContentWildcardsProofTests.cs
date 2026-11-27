// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Verifies original simple-content type identity, actual scalars and strict attribute authority.
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
namespace Bosak.Schema.Tests.SimpleContentWildcards;
/// <summary>Proves original simple-content components and strict attribute authority.</summary>
public sealed class NativeSimpleContentWildcardsProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(bool nested=false,bool named=false,string scalar="xs:decimal",string definitions="")
    {
        var body="<xs:simpleContent><xs:extension base='"+scalar+"'><xs:anyAttribute notQName='unused'/></xs:extension></xs:simpleContent>";
        var type="<xs:complexType>"+body+"</xs:complexType>";
        var root=nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e'>"+type+"</xs:element></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r'>"+type+"</xs:element>";
        if(named)root="<xs:complexType name='T'>"+body+"</xs:complexType>"+(nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e' type='T'/></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r' type='T'/><xs:element name='s' type='T'/>");
        return "<xs:schema xmlns:xs='"+X+"'>"+definitions+"<xs:attribute name='a' type='xs:integer'/><xs:attribute name='id' type='xs:ID'/><xs:attribute name='ref' type='xs:IDREF'/>"+root+"</xs:schema>";
    }
    /// <summary>Original complex governing and actual decimal components remain distinct.</summary>
    /// <param name="nested">Nested owner selector.</param><param name="named">Named governing type selector.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void OriginalAuthority(bool nested,bool named)
    {
        using var f=new Fixture(Schema(nested,named),nested?"<r><e a='001'> 01.20 </e></r>":"<r a='001'> 01.20 </r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var path=nested?new[]{0}:Array.Empty<int>();var owner=f.Input.FactsFor((NativeSessionNode)f.Element([..path]))!;Assert.False(owner.NativeType.MixedContent);Assert.False(owner.HasNoTypedValue);Assert.False(owner.NativeType.IsAnyType);Assert.NotNull(owner.Declaration);if(named)Assert.Same(f.Provider.ResolveType(new("","T"),default).Value,owner.Type);
        var atom=Assert.Single(owner.ReadTypedValue(default).Value.AtomicItems);Assert.Same(owner.Type,atom.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"decimal"),default).Value,atom.TypeEvidence.ActualType);Assert.NotSame(owner.Type,atom.TypeEvidence.ActualType);Assert.Equal("1.2",f.Read(f.Element([..path])).DecimalValue.ToInvariantString(256));var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([..path],"","a"))!;Assert.Same(f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value,a.Declaration);Assert.Equal("1",f.Read(f.Attribute([..path],"","a")).IntegerValue.ToString());f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>owner.ReadTypedValue(default));Assert.Throws<ObjectDisposedException>(()=>a.Declaration);
    }
    /// <summary>Root aliases share original governing type and compiled wildcard metadata.</summary>
    [Fact]public void Aliases()
    {
        var c=NativeDecimalSchema.Compile(Schema(named:true),new(256,256),maxStringCharacters:1024).Value!;var roots=c.Roots.Where(p=>p.Root.Name is "r" or "s").ToArray();Assert.Equal(2,roots.Length);Assert.Same(roots[0].Root.Type,roots[1].Root.Type);Assert.NotNull(roots[0].CompiledAttributeWildcard);Assert.Same(roots[0].CompiledAttributeWildcard,roots[1].CompiledAttributeWildcard);using var s=NativeSchemaSession.Create(Schema(named:true),new(256,256,1024,65536,2,256,16,256,1024)).Value!;foreach(var name in new[]{"r","s"}){using var d=s.Assess("<"+name+" a='001'>01.20</"+name+">",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.True(d.HasRootScalar);Assert.Equal("1.2",d.ReadRootScalar().DecimalValue.ToInvariantString(256));}
    }
    /// <summary>Invalid scalar or attribute never publishes partial facts and later input recovers.</summary>
    [Fact]public void FailureRecovery()
    {
        foreach(var xml in new[]{"<r a='bad'>1</r>","<r a='1'>bad</r>","<r unknown='1'>1</r>"}){using var f=new Fixture(Schema(),xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);}using var s=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,16,256,1024)).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r a='bad'>1</r>",new(DateTimeOffset.UnixEpoch,0)).Outcome);using var d=s.Assess("<r a='1'>01.20</r>",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.Equal("1.2",d.ReadRootScalar().DecimalValue.ToInvariantString(256));
    }
    /// <summary>Identity resolution, foreign authority and cancellation remain assessment-local.</summary>
    [Fact]public void IdentityOwnership()
    {
        using var f=new Fixture(Schema(),"<r id='v' ref='v'>1</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);using var bad=new Fixture(Schema(),"<r ref='missing'>1</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);using var other=new Fixture(Schema(),"<r a='1'>1</r>");var local=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.False(f.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(local.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>local.ReadTypedValue(cancel.Token));
    }
    /// <summary>String capacity refusal publishes no document and preserves recovery.</summary>
    [Fact]public void Capacity()
    {
        var c=NativeDecimalSchema.Compile(Schema(scalar:"xs:string"),new(256,256),maxStringCharacters:3).Value!;var failed=c.Assess("<r a='1'>abcd</r>");Assert.Equal(NativeOutcome.Unsupported,failed.Outcome);Assert.Null(failed.Value);Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,failed.UnsupportedReason);Assert.Equal(NativeOutcome.Valid,c.Assess("<r a='1'>abc</r>").Outcome);
    }
    /// <summary>Known global matches under anyType retain simple-content authority.</summary>
    [Fact]public void KnownLax()
    {
        var schema=Schema().Replace("<xs:element name='r'>","<xs:element name='s'>").Replace("</xs:schema>","<xs:element name='r'/></xs:schema>");using var f=new Fixture(schema,"<r><s a='1'>01.20</s></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.Same(f.Provider.ResolveElementDeclaration(new("","s"),default).Value,owner.Declaration);Assert.False(owner.NativeType.IsAnyType);Assert.Equal("1.2",f.Read(f.Element([0])).DecimalValue.ToInvariantString(256));
    }
    /// <summary>List content preserves two actual decimal items under its complex governing type.</summary>
    [Fact]public void ListEvidence()
    {
        using var f=new Fixture(Schema(scalar:"L",definitions:"<xs:simpleType name='L'><xs:list itemType='xs:decimal'/></xs:simpleType>"),"<r a='1'>01.20 2</r>",atoms:2);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var atoms=owner.ReadTypedValue(default).Value.AtomicItems.ToArray();Assert.Equal(2,atoms.Length);Assert.All(atoms,a=>{Assert.Same(owner.Type,a.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"decimal"),default).Value,a.TypeEvidence.ActualType);});
    }
    /// <summary>Union selection retains the selected decimal actual type beneath the complex owner.</summary>
    [Fact]public void UnionEvidence()
    {
        using var f=new Fixture(Schema(scalar:"U",definitions:"<xs:simpleType name='U'><xs:union memberTypes='xs:decimal xs:string'/></xs:simpleType>"),"<r a='1'>01.20</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var atom=Assert.Single(owner.ReadTypedValue(default).Value.AtomicItems);Assert.Same(owner.Type,atom.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"decimal"),default).Value,atom.TypeEvidence.ActualType);
    }
    /// <summary>The legacy decimal-only compiler remains outside the wildcard profile.</summary>
    [Fact]public void LegacyRefusal()=>Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(),new(256,256)).Outcome);
}
