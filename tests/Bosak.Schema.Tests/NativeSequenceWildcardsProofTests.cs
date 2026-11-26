// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies sequence owner identity and independent strict attribute/child validation.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.SequenceWildcards;
/// <summary>Proves original sequence owner, child and wildcard attribute authority.</summary>
public sealed class NativeSequenceWildcardsProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(bool nested=false,bool named=false,bool optional=false)
    {
        var body="<xs:sequence><xs:element name='c' type='xs:integer'"+(optional?" minOccurs='0'":"")+"/></xs:sequence><xs:anyAttribute notQName='b'/>";
        var type="<xs:complexType>"+body+"</xs:complexType>";
        var root=nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e'>"+type+"</xs:element></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r'>"+type+"</xs:element>";
        if(named)root="<xs:complexType name='T'>"+body+"</xs:complexType>"+(nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e' type='T'/></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r' type='T'/><xs:element name='s' type='T'/>");
        return "<xs:schema xmlns:xs='"+X+"'><xs:attribute name='a' type='xs:integer'/><xs:attribute name='id' type='xs:ID'/><xs:attribute name='ref' type='xs:IDREF'/>"+root+"</xs:schema>";
    }
    /// <summary>Complex owner and atomic attribute/child identities remain independent.</summary>
    /// <param name="nested">Nested owner selector.</param><param name="named">Named type selector.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void OriginalAuthority(bool nested,bool named)
    {
        using var f=new Fixture(Schema(nested,named),nested?"<r><e a='001'><c>02</c></e></r>":"<r a='001'><c>02</c></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var path=nested?new[]{0}:Array.Empty<int>();
        var owner=f.Input.FactsFor((NativeSessionNode)f.Element([..path]))!;var attribute=f.Input.FactsFor((NativeSessionNode)f.Attribute([..path],"","a"))!;var child=f.Input.FactsFor((NativeSessionNode)f.Element([..path,0]))!;
        Assert.True(owner.HasNoTypedValue);Assert.False(owner.NativeType.EmptyContent);Assert.Equal(SchemaResultKind.Unsupported,owner.ReadTypedValue(default).Kind);Assert.NotNull(owner.Declaration);Assert.True(f.Provider.Owns(owner.Type));
        if(named)Assert.Same(f.Provider.ResolveType(new("","T"),default).Value,owner.Type);
        Assert.Same(f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value,attribute.Declaration);Assert.Same(f.Provider.ResolveType(new(X,"integer"),default).Value,attribute.Type);Assert.NotSame(owner.Type,attribute.Type);Assert.Same(attribute.Type,child.Type);Assert.Equal("1",f.Read(f.Attribute([..path],"","a")).IntegerValue.ToString());Assert.Equal("2",f.Read(f.Element([..path,0])).IntegerValue.ToString());
        var atom=Assert.Single(attribute.ReadTypedValue(default).Value.AtomicItems);Assert.Same(attribute.Type,atom.TypeEvidence.ActualType);Assert.Same(attribute.Type,atom.TypeEvidence.DeclaredType);
        f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>attribute.Declaration);Assert.Throws<ObjectDisposedException>(()=>owner.ReadTypedValue(default));
    }
    /// <summary>Both named root aliases retain wildcard plans with the same original complex type.</summary>
    [Fact]public void NamedRootAliases()
    {
        var schema=Schema(named:true);var compiled=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024).Value!;var roots=compiled.Roots.Where(p=>p.Root.Name is "r" or "s").ToArray();Assert.Equal(2,roots.Length);Assert.Same(roots[0].Root.Type,roots[1].Root.Type);Assert.NotSame(roots[0].Root,roots[1].Root);Assert.Same(roots[0].SequenceAttributeWildcard,roots[1].SequenceAttributeWildcard);Assert.NotNull(roots[0].SequenceAttributeWildcard);
        using var s=NativeSchemaSession.Create(schema,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);foreach(var name in new[]{"r","s"}){using var d=s.Assess("<"+name+" a='1'><c>2</c></"+name+">",context).Value!;Assert.False(d.HasRootScalar);Assert.Equal("1",d.ReadAttribute(0).Value.IntegerValue.ToString());}
    }
    /// <summary>Absent optional children do not turn an element-only owner into empty content.</summary>
    [Fact]public void OptionalChildrenAbsent()
    {
        using var f=new Fixture(Schema(optional:true),"<r a='1'/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(owner.HasNoTypedValue);Assert.False(owner.NativeType.EmptyContent);Assert.Equal(SchemaResultKind.Unsupported,owner.ReadTypedValue(default).Kind);
    }
    /// <summary>Child and attribute failures publish nothing and later valid assessment recovers.</summary>
    [Fact]public void FailureAndRecovery()
    {
        foreach(var xml in new[]{"<r a='bad'><c>1</c></r>","<r a='1'><c>bad</c></r>","<r unknown='1'><c>1</c></r>"}){using var f=new Fixture(Schema(),xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);}
        using var s=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r a='bad'><c>1</c></r>",context).Outcome);using var good=s.Assess("<r a='1'><c>2</c></r>",context).Value!;Assert.Equal("2",good.ReadScalar(0).IntegerValue.ToString());
    }
    /// <summary>Original wildcard identity attributes still resolve document-wide.</summary>
    [Fact]public void Identity()
    {
        using var good=new Fixture(Schema(),"<r id='v' ref='v'><c>1</c></r>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);using var bad=new Fixture(Schema(),"<r ref='missing'><c>1</c></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);
    }
    /// <summary>Foreign components and cancelled reads retain the existing boundary.</summary>
    [Fact]public void ForeignAndCancelled()
    {
        using var a=new Fixture(Schema(),"<r a='1'><c>2</c></r>");using var b=new Fixture(Schema(),"<r a='1'><c>2</c></r>");var fa=a.Input.FactsFor((NativeSessionNode)a.Attribute([],"","a"))!;var fb=b.Input.FactsFor((NativeSessionNode)b.Attribute([],"","a"))!;Assert.False(a.Provider.Owns(fb.Type));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(fa.Type,fb.Type,default));using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>fa.ReadTypedValue(c.Token));
    }
    /// <summary>Legacy decimal-only grammar does not acquire wildcard syntax.</summary>
    [Fact]public void LegacyProfileUnchanged()
    {
        var schema="<xs:schema xmlns:xs='"+X+"'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='c' type='xs:decimal'/></xs:sequence><xs:anyAttribute/></xs:complexType></xs:element></xs:schema>";Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(schema,new(256,256)).Outcome);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(schema.Replace("<xs:anyAttribute/>",""),new(256,256)).Outcome);
    }
    /// <summary>Known anyType children can use actual strict sequence plans without losing authority.</summary>
    [Fact]public void KnownLaxContainer()
    {
        var schema=Schema().Replace("<xs:element name='r'>","<xs:element name='s'>").Replace("</xs:schema>","<xs:element name='r'/></xs:schema>");using var f=new Fixture(schema,"<r><s a='1'><c>2</c></s></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var s=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.True(s.HasNoTypedValue);Assert.Same(f.Provider.ResolveElementDeclaration(new("","s"),default).Value,s.Declaration);Assert.Same(f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value,f.Input.FactsFor((NativeSessionNode)f.Attribute([0],"","a"))!.Declaration);
    }
}
