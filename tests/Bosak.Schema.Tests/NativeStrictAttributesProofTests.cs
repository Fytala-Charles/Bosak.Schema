// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies original strict wildcard authority and empty owner typed values.
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
namespace Bosak.Schema.Tests.StrictAttributes;
/// <summary>Proves strict wildcard facts retain actual globals and empty complex owners.</summary>
public sealed class NativeStrictAttributesProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(bool nested=false,bool named=false)
    {
        var c="<xs:complexType><xs:anyAttribute/></xs:complexType>";
        var root=nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e'>"+c+"</xs:element></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r'>"+c+"</xs:element>";
        if(named)root="<xs:complexType name='T'><xs:anyAttribute/></xs:complexType>"+(nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e' type='T'/></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r' type='T'/>");
        return "<xs:schema xmlns:xs='"+X+"'><xs:attribute name='a' type='xs:integer'/><xs:attribute name='id' type='xs:ID'/><xs:attribute name='ref' type='xs:IDREF'/>"+root+"</xs:schema>";
    }
    /// <summary>Actual wildcard declarations are canonical; owner empty values remain empty.</summary>
    /// <param name="nested">Nested owner selector.</param><param name="named">Named type selector.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void CanonicalAuthority(bool nested,bool named)
    {
        var child=nested;using var f=new Fixture(Schema(nested,named),child?"<r><e a='001'/></r>":"<r a='001'/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var path=child?new[]{0}:Array.Empty<int>();var node=(NativeSessionNode)f.Attribute([..path],"","a");var fact=f.Input.FactsFor(node)!;
        Assert.Same(f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value,fact.Declaration);Assert.Equal("a",fact.NativeDeclaration.Name);Assert.True(f.Provider.Owns(fact.Declaration!));
        var item=Assert.Single(fact.ReadTypedValue(default).Value.AtomicItems);Assert.Same(fact.Type,item.TypeEvidence.ActualType);Assert.Same(fact.Type,item.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"integer"),default).Value,fact.Type);Assert.Equal("1",f.Read(node).IntegerValue.ToString());
        var owner=f.Input.FactsFor((NativeSessionNode)f.Element([..path]))!;Assert.True(owner.NativeType.EmptyContent);Assert.Empty(owner.ReadTypedValue(default).Value.AtomicItems);Assert.NotNull(owner.Declaration);
        f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);Assert.Throws<ObjectDisposedException>(()=>owner.ReadTypedValue(default));
    }
    /// <summary>Strict unknown and known invalid attributes publish no partial facts.</summary>
    [Fact]public void FailureAndRecovery()
    {
        foreach(var xml in new[]{"<r unknown='1'/>","<r a='bad'/>"}){using var f=new Fixture(Schema(),xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);}
        using var s=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r unknown='1'/>",context).Outcome);using var good=s.Assess("<r a='001'/>",context).Value!;Assert.False(good.HasRootScalar);Assert.Equal("1",good.ReadAttribute(0).Value.IntegerValue.ToString());
    }
    /// <summary>Actual global identity attributes participate in the existing document graph.</summary>
    [Fact]public void Identity()
    {
        using var good=new Fixture(Schema(),"<r id='v' ref='v'/>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);using var bad=new Fixture(Schema(),"<r ref='missing'/>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);
    }
    /// <summary>Foreign authority and cancelled reads remain refused.</summary>
    [Fact]public void ForeignAndCancelled()
    {
        using var a=new Fixture(Schema(),"<r a='1'/>");using var b=new Fixture(Schema(),"<r a='1'/>");var fa=a.Input.FactsFor((NativeSessionNode)a.Attribute([],"","a"))!;var fb=b.Input.FactsFor((NativeSessionNode)b.Attribute([],"","a"))!;Assert.False(a.Provider.Owns(fb.Type));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(fa.Type,fb.Type,default));using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>fa.ReadTypedValue(c.Token));
    }
    /// <summary>Missing global defaults do not create attributes or synthetic declarations.</summary>
    [Fact]public void NoAugmentation()
    {
        var s=Schema().Replace("name='a' type='xs:integer'","name='a' type='xs:integer' default='1'");using var f=new Fixture(s,"<r/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Empty(f.Assessment.AttributeFacts);Assert.Empty(f.Input.FactsFor((NativeSessionNode)f.Element([]))!.ReadTypedValue(default).Value.AtomicItems);
    }
}
