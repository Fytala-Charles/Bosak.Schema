// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 28 November 2026
// PURPOSE              : Proves original anonymous URI/text bases, values and identity authority.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 28-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.AnonymousTextBases;
/// <summary>Checks original unnamed URI/text bases, lexical families and document identity authority.</summary>
public sealed class NativeAnonymousTextBasesProofTests
{
    private static readonly string[] Kinds=["anyURI","language","Name","NCName","NMTOKEN","ID","IDREF"];
    private static string Good(string kind)=>kind switch{"anyURI"=>"relative/path","language"=>"en-US","Name"=>"p:item","NMTOKEN"=>"1:item",_=>"item"};
    private static string Base(string kind="anyURI",string facets="")=>"<xs:simpleType><xs:restriction base='xs:"+kind+"'>"+facets+"</xs:restriction></xs:simpleType>";
    private static string Restrict(string source,string facets="")=>"<xs:simpleType><xs:restriction>"+source+facets+"</xs:restriction></xs:simpleType>";
    private static string Schema(string definition,bool named=false,string extra="")=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+(named?"<xs:simpleType name='T'>"+definition["<xs:simpleType>".Length..]:"")+"<xs:element name='a'><xs:complexType><xs:sequence><xs:element name='binding' type='xs:ID'/><xs:element name='v'"+(named?" type='T'/>":">"+definition+"</xs:element>")+"</xs:sequence></xs:complexType></xs:element>"+extra+"</xs:schema>";
    private static string Xml(string value)=>"<a><binding>item</binding><v>"+value+"</v></a>";
    private static NativeDecimalSchema Compile(string source)=>NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:16384).Value!;
    /// <summary>Named and unnamed outer types retain their original direct base and atomic provenance.</summary>
    /// <param name="kind">URI or text family.</param><param name="named">Outer naming selector.</param>
    [Theory][InlineData("anyURI",false)][InlineData("anyURI",true)][InlineData("language",false)][InlineData("language",true)][InlineData("Name",false)][InlineData("Name",true)][InlineData("NCName",false)][InlineData("NCName",true)][InlineData("NMTOKEN",false)][InlineData("NMTOKEN",true)][InlineData("ID",false)][InlineData("ID",true)][InlineData("IDREF",false)][InlineData("IDREF",true)]
    public void TypeAuthority(string kind,bool named)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind)),named),Xml(Good(kind)),atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;var t=fact.NativeType;Assert.Null(t.BaseName);Assert.Equal(named?"T":null,t.Name);Assert.NotNull(t.DerivationBase);Assert.Null(t.DerivationBase!.Name);Assert.NotSame(t,t.DerivationBase);Assert.Equal(kind=="anyURI",t.IsUri);Assert.Equal(kind=="anyURI"?NativeTextKind.Token:Enum.Parse<NativeTextKind>(char.ToUpperInvariant(kind[0])+kind[1..]),t.TextKind);Assert.Equal(t.TextKind,t.DerivationBase.TextKind);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(t.DerivationBase)));Assert.Equal(kind is "ID" or "IDREF",NativeDecimalSchema.IsIdentityAtomic(t));var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Equal(XdmValueKind.String,atom.Value.Kind);Assert.Equal(Good(kind),atom.Value.StringValue);if(kind=="anyURI")Assert.Equal("anyURI",atom.Value.SchemaTypeName);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>Normalized enumeration ancestry and patterned-ancestor refusals retain original authority.</summary>
    /// <param name="kind">URI or text family.</param>
    [Theory][InlineData("anyURI")][InlineData("language")][InlineData("Name")][InlineData("NCName")][InlineData("NMTOKEN")][InlineData("ID")][InlineData("IDREF")]
    public void FacetAncestry(string kind)
    {
        var good=Good(kind);var original=Base(kind,"<xs:enumeration value=' "+good+" '/>");var source=Schema(Restrict(original,"<xs:enumeration value='"+good+"'/>"),true);var c=Compile(source);var t=c.NamedTypes.Single(t=>t.Name=="T");Assert.NotNull(t.DerivationBase!.TextFacets);Assert.NotSame(t.TextFacets,t.DerivationBase.TextFacets);Assert.Equal(NativeOutcome.Valid,c.Assess(Xml(" "+good+" ")).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(Base(kind,"<xs:pattern value='.*'/>"),"<xs:enumeration value='"+good+"'/>")),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Detached values, recovery, ownership, cancellation and expiry retain original contracts.</summary>
    /// <param name="kind">URI or text family.</param>
    [Theory][InlineData("anyURI")][InlineData("language")][InlineData("Name")][InlineData("NCName")][InlineData("NMTOKEN")][InlineData("ID")][InlineData("IDREF")]
    public void ValuesAndLifetime(string kind)
    {
        var source=Schema(Restrict(Base(kind)));using var owner=NativeSchemaSession.Create(source,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<bad/>",context).Outcome);using var d=owner.Assess(Xml(" "+Good(kind)+" "),context).Value!;var value=Assert.Single(d.ReadScalarItems(1));d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadScalarItems(1));Assert.Equal(kind=="anyURI"?NativeScalarKind.AnyUri:NativeScalarKind.String,value.Kind);Assert.Equal(Good(kind),kind=="anyURI"?value.UriValue:value.StringValue);using var again=owner.Assess(Xml(Good(kind)),context).Value!;Assert.Single(again.ReadScalarItems(1));
        using var f=new Fixture(source,Xml(Good(kind)),atoms:16);using var other=new Fixture(source,Xml(Good(kind)),atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;var handle=f.Provider.TypeComponent(fact.NativeType.DerivationBase!);Assert.True(f.Provider.Owns(handle));Assert.False(other.Provider.Owns(handle));var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([1]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>fact.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);
    }
    /// <summary>Named forward dependencies retain their original edge and final prohibition.</summary>
    [Fact]public void ForwardAndFinal()
    {
        foreach(var kind in Kinds){var source=Schema(Restrict("<xs:simpleType><xs:restriction base='B'/></xs:simpleType>"),true,"<xs:simpleType name='B'><xs:restriction base='xs:"+kind+"'/></xs:simpleType>");var c=Compile(source);Assert.Same(c.NamedTypes.Single(t=>t.Name=="B"),c.NamedTypes.Single(t=>t.Name=="T").DerivationBase!.DerivationBase);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(source.Replace("name='B'","name='B' final='restriction'"),new(256,256),maxStringCharacters:1024).Outcome);}
    }
    /// <summary>Shared named uses and separate anonymous source definitions retain original object identity.</summary>
    [Fact]public void SharedAndDistinct()
    {
        foreach(var kind in Kinds){var definition=Restrict(Base(kind));var source=Schema(definition,true).Replace("<xs:element name='v' type='T'/>","<xs:element name='v' type='T'/><xs:element name='w' type='T'/>");using var shared=new Fixture(source,Xml(Good(kind)).Replace("</a>","<w>"+Good(kind)+"</w></a>"),atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,shared.Assessment.Status);var one=shared.Input.FactsFor((NativeSessionNode)shared.Element([1]))!;var two=shared.Input.FactsFor((NativeSessionNode)shared.Element([2]))!;Assert.Same(one.NativeType,two.NativeType);source=Schema(definition).Replace("</xs:sequence>","<xs:element name='w'>"+definition+"</xs:element></xs:sequence>");using var distinct=new Fixture(source,Xml(Good(kind)).Replace("</a>","<w>"+Good(kind)+"</w></a>"),atoms:16);one=distinct.Input.FactsFor((NativeSessionNode)distinct.Element([1]))!;two=distinct.Input.FactsFor((NativeSessionNode)distinct.Element([2]))!;Assert.NotSame(one.NativeType.DerivationBase,two.NativeType.DerivationBase);}
    }
    /// <summary>Inline depth and shared source-site capacities retain their finite ceilings.</summary>
    [Fact]public void Budgets()
    {
        var definition=Base();for(var i=0;i<4;i++)definition=Restrict(definition);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(definition),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(definition)),new(256,256),maxStringCharacters:1024).Outcome);var members=Enumerable.Repeat(Restrict(Base()),8).ToArray();string Union()=>"<xs:simpleType><xs:union>"+string.Concat(members)+"</xs:union></xs:simpleType>";Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);members[0]=Restrict(members[0]);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>References before their IDs resolve only in the current complete assessment.</summary>
    [Fact]public void ForwardIdentityAndIsolation()
    {
        var source=Schema(Restrict(Base("IDREF"))).Replace("<xs:element name='binding' type='xs:ID'/>","").Replace("</xs:sequence>","<xs:element name='binding' type='xs:ID' minOccurs='0'/></xs:sequence>");using var owner=NativeSchemaSession.Create(source,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);using var first=owner.Assess("<a><v>item</v><binding>item</binding></a>",context).Value!;Assert.Equal("item",Assert.Single(first.ReadScalarItems(0)).StringValue);var missing=owner.Assess("<a><v>item</v></a>",context);Assert.Equal(NativeSchemaOutcome.Invalid,missing.Outcome);Assert.Null(missing.Value);using var recovered=owner.Assess("<a><v>other</v><binding>other</binding></a>",context).Value!;Assert.Equal("other",Assert.Single(recovered.ReadScalarItems(0)).StringValue);
    }
    /// <summary>Late identity failure publishes no facts or evidence.</summary>
    [Fact]public void IdentityFailureHasNoFacts()
    {
        using var f=new Fixture(Schema(Restrict(Base("IDREF"))),Xml("missing"),atoms:16);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);
    }
    /// <summary>Nilled declarations retain original governing text types and typed-empty values.</summary>
    [Fact]public void NilledAuthority()
    {
        foreach(var kind in Kinds){var source=Schema(Restrict(Base(kind))).Replace("name='v'>","name='v' nillable='true'>");using var f=new Fixture(source,"<a><binding>item</binding><v xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/></a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;Assert.NotNull(fact.NativeType.DerivationBase);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);Assert.Empty(typed.AtomicItems);}
    }
    /// <summary>Length counts Unicode scalars independently of UTF-16 representation capacity.</summary>
    [Fact]public void UnicodeScalarLength()
    {
        foreach(var kind in new[]{"anyURI","Name","NCName","NMTOKEN"}){var c=Compile(Schema(Restrict(Base(kind),"<xs:length value='1'/>")));Assert.Equal(NativeOutcome.Valid,c.Assess(Xml("\U0001F600")).Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess(Xml("\U0001F600x")).Outcome);}
    }
    /// <summary>Identity-specific lexical budgets and owner recovery remain before publication.</summary>
    [Fact]public void IdentityBudgets()
    {
        foreach(var kind in new[]{"ID","IDREF"}){using var owner=NativeSchemaSession.Create(Schema(Restrict(Base(kind))),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);var result=owner.Assess(Xml(new string('i',257)),context);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,result.Diagnostics[0].UnsupportedReason);using var d=owner.Assess(Xml("item"),context).Value!;Assert.Single(d.ReadScalarItems(1));}
    }
}
