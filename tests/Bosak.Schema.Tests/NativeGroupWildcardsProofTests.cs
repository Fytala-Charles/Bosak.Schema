// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies strict wildcard authority and all/choice root content semantics.
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
namespace Bosak.Schema.Tests.GroupWildcards;
/// <summary>Proves original all/choice owner and strict wildcard attribute identities.</summary>
public sealed class NativeGroupWildcardsProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(string group,bool named=false,bool optional=false)
    {
        var body="<xs:"+group+(optional?" minOccurs='0'":"")+"><xs:element name='c' type='xs:integer'/><xs:element name='d' type='xs:token'/></xs:"+group+"><xs:anyAttribute notQName='b'/>";
        var root=named?"<xs:complexType name='T'>"+body+"</xs:complexType><xs:element name='r' type='T'/><xs:element name='s' type='T'/>":"<xs:element name='r'><xs:complexType>"+body+"</xs:complexType></xs:element>";
        return "<xs:schema xmlns:xs='"+X+"'><xs:attribute name='a' type='xs:integer'/><xs:attribute name='id' type='xs:ID'/><xs:attribute name='ref' type='xs:IDREF'/>"+root+"</xs:schema>";
    }
    private static string Xml(string group,string attrs="a='001'")=>"<r "+attrs+"><d> x </d>"+(group=="all"?"<c>02</c>":"")+"</r>";
    /// <summary>Owners and attributes retain original authority while children stay in physical order.</summary>
    /// <param name="group">Compositor selector.</param><param name="named">Named type selector.</param>
    [Theory][InlineData("all",false)][InlineData("all",true)][InlineData("choice",false)][InlineData("choice",true)]
    public void OriginalAuthority(string group,bool named)
    {
        using var f=new Fixture(Schema(group,named),Xml(group));Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var attribute=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","a"))!;
        Assert.True(owner.HasNoTypedValue);Assert.False(owner.NativeType.EmptyContent);Assert.Equal(SchemaResultKind.Unsupported,owner.ReadTypedValue(default).Kind);Assert.Same(f.Provider.ResolveElementDeclaration(new("","r"),default).Value,owner.Declaration);if(named)Assert.Same(f.Provider.ResolveType(new("","T"),default).Value,owner.Type);
        Assert.Same(f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value,attribute.Declaration);Assert.Same(f.Provider.ResolveType(new(X,"integer"),default).Value,attribute.Type);Assert.NotSame(owner.Type,attribute.Type);var atom=Assert.Single(attribute.ReadTypedValue(default).Value.AtomicItems);Assert.Same(attribute.Type,atom.TypeEvidence.DeclaredType);Assert.Same(attribute.Type,atom.TypeEvidence.ActualType);Assert.Equal("1",f.Read(f.Attribute([],"","a")).IntegerValue.ToString());
        Assert.Equal("x",f.Read(f.Element([0])).StringValue);if(group=="all")Assert.Equal("2",f.Read(f.Element([1])).IntegerValue.ToString());f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>attribute.Declaration);Assert.Throws<ObjectDisposedException>(()=>owner.ReadTypedValue(default));
    }
    /// <summary>Shared named-root aliases keep immutable wildcard plans and actual complex identity.</summary>
    /// <param name="group">Compositor selector.</param>
    [Theory][InlineData("all")][InlineData("choice")]
    public void NamedAliases(string group)
    {
        var schema=Schema(group,true);var compiled=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024).Value!;var roots=compiled.Roots.Where(p=>p.Root.Name is "r" or "s").ToArray();Assert.Equal(2,roots.Length);Assert.Same(roots[0].Root.Type,roots[1].Root.Type);Assert.NotSame(roots[0].Root,roots[1].Root);Assert.Same(roots[0].CompiledAttributeWildcard,roots[1].CompiledAttributeWildcard);Assert.NotNull(roots[0].CompiledAttributeWildcard);
        using var session=NativeSchemaSession.Create(schema,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);foreach(var name in new[]{"r","s"}){var xml=Xml(group).Replace("<r ","<"+name+" ").Replace("</r>","</"+name+">");using var d=session.Assess(xml,context).Value!;Assert.False(d.HasRootScalar);Assert.Equal("x",d.ReadScalar(0).StringValue);Assert.Equal("1",d.ReadAttribute(0).Value.IntegerValue.ToString());}
    }
    /// <summary>Absent optional group still has element-only no-typed-value semantics.</summary>
    /// <param name="group">Compositor selector.</param>
    [Theory][InlineData("all")][InlineData("choice")]
    public void OptionalAbsent(string group)
    {
        using var f=new Fixture(Schema(group,optional:true),"<r a='1'/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(owner.HasNoTypedValue);Assert.False(owner.NativeType.EmptyContent);Assert.Equal(SchemaResultKind.Unsupported,owner.ReadTypedValue(default).Kind);
    }
    /// <summary>Content and attribute failures publish no partial facts and later assessment recovers.</summary>
    /// <param name="group">Compositor selector.</param>
    [Theory][InlineData("all")][InlineData("choice")]
    public void FailureAndRecovery(string group)
    {
        foreach(var xml in new[]{Xml(group,"a='bad'"),Xml(group,"unknown='1'"),"<r a='1'><c>bad</c></r>"}){using var f=new Fixture(Schema(group),xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);}
        using var session=NativeSchemaSession.Create(Schema(group),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,session.Assess(Xml(group,"a='bad'"),context).Outcome);using var d=session.Assess(Xml(group),context).Value!;Assert.Equal("x",d.ReadScalar(0).StringValue);
    }
    /// <summary>Identity validation, foreign ownership and cancellation retain original boundaries.</summary>
    /// <param name="group">Compositor selector.</param>
    [Theory][InlineData("all")][InlineData("choice")]
    public void IdentityAndOwnership(string group)
    {
        using var good=new Fixture(Schema(group),Xml(group,"id='v' ref='v'"));Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);using var bad=new Fixture(Schema(group),Xml(group,"ref='missing'"));Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);
        using var other=new Fixture(Schema(group),Xml(group,"id='v'"));var local=good.Input.FactsFor((NativeSessionNode)good.Attribute([],"","id"))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Attribute([],"","id"))!;Assert.False(good.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>good.Provider.IsSameComponent(local.Type,foreign.Type,default));using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>local.ReadTypedValue(c.Token));
    }
}
