// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 28 November 2026
// PURPOSE              : Proves original anonymous binary and QName bases, values and namespace authority.
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
namespace Bosak.Schema.Tests.AnonymousInterchangeBases;
/// <summary>Checks original unnamed binary and QName bases and namespace-sensitive authority.</summary>
public sealed class NativeAnonymousInterchangeBasesProofTests
{
    private static readonly string[] Kinds=["hexBinary","base64Binary","QName"];
    private static string Good(string kind)=>kind switch{"hexBinary"=>"0aFF","base64Binary"=>"Cv8=",_=>"p:item"};
    private static string Alias(string kind)=>kind switch{"hexBinary"=>"0Aff","base64Binary"=>"C v 8 =",_=>"q:item"};
    private static string Base(string kind="hexBinary",string facets="")=>"<xs:simpleType><xs:restriction base='xs:"+kind+"'>"+facets+"</xs:restriction></xs:simpleType>";
    private static string Restrict(string source,string facets="")=>"<xs:simpleType><xs:restriction>"+source+facets+"</xs:restriction></xs:simpleType>";
    private static string Schema(string definition,bool named=false,string extra="")=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:p='urn:one' xmlns:q='urn:one'>"+(named?"<xs:simpleType name='T'>"+definition["<xs:simpleType>".Length..]:"")+"<xs:element name='a'"+(named?" type='T'/>":">"+definition+"</xs:element>")+extra+"</xs:schema>";
    private static string Xml(string value)=>"<a xmlns:p='urn:one' xmlns:q='urn:one'>"+value+"</a>";
    private static NativeDecimalSchema Compile(string source)=>NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;
    private static NativeInterchangeKind Kind(string kind)=>Enum.Parse<NativeInterchangeKind>(char.ToUpperInvariant(kind[0])+kind[1..]);
    /// <summary>Named and unnamed outer types retain their original base and native atomic provenance.</summary>
    /// <param name="kind">Interchange family.</param><param name="named">Outer naming selector.</param>
    [Theory][InlineData("hexBinary",false)][InlineData("hexBinary",true)][InlineData("base64Binary",false)][InlineData("base64Binary",true)][InlineData("QName",false)][InlineData("QName",true)]
    public void TypeAuthority(string kind,bool named)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind)),named),Xml(Good(kind)),atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var t=fact.NativeType;Assert.Null(t.BaseName);Assert.Equal(named?"T":null,t.Name);Assert.NotNull(t.DerivationBase);Assert.Null(t.DerivationBase!.Name);Assert.NotSame(t,t.DerivationBase);Assert.Equal(Kind(kind),t.InterchangeKind);Assert.Equal(t.InterchangeKind,t.DerivationBase.InterchangeKind);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(t.DerivationBase)));var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Equal(kind=="QName"?XdmValueKind.QName:XdmValueKind.String,atom.Value.Kind);if(kind!="QName")Assert.Equal(kind,atom.Value.SchemaTypeName);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>Enumeration ancestry preserves value equality and original facet namespaces.</summary>
    /// <param name="kind">Interchange family.</param>
    [Theory][InlineData("hexBinary")][InlineData("base64Binary")][InlineData("QName")]
    public void FacetAncestry(string kind)
    {
        var original=Base(kind,"<xs:enumeration value='"+Good(kind)+"'/>");var c=Compile(Schema(Restrict(original,"<xs:enumeration value='"+Alias(kind)+"'/>")));Assert.NotNull(c.Root.Type.DerivationBase!.InterchangeFacets);Assert.NotSame(c.Root.Type.InterchangeFacets,c.Root.Type.DerivationBase.InterchangeFacets);Assert.Equal(NativeOutcome.Valid,c.Assess(Xml(Good(kind))).Outcome);Assert.Equal(NativeOutcome.Valid,c.Assess(Xml(Alias(kind))).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(Base(kind,"<xs:pattern value='.*'/>"),"<xs:enumeration value='"+Good(kind)+"'/>")),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Detached values, recovery, ownership, cancellation and expiry retain original contracts.</summary>
    /// <param name="kind">Interchange family.</param>
    [Theory][InlineData("hexBinary")][InlineData("base64Binary")][InlineData("QName")]
    public void ValuesAndLifetime(string kind)
    {
        var source=Schema(Restrict(Base(kind)));using var owner=NativeSchemaSession.Create(source,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess(Xml("?"),context).Outcome);using var d=owner.Assess(Xml(Good(kind)),context).Value!;var value=Assert.Single(d.ReadRootItems());d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootItems());Assert.Equal(Enum.Parse<NativeScalarKind>(char.ToUpperInvariant(kind[0])+kind[1..]),value.Kind);if(kind=="QName"){Assert.Equal("urn:one",value.QNameValue.NamespaceUri);Assert.Equal("item",value.QNameValue.LocalName);}else Assert.Equal(new byte[]{10,255},value.BinaryValue.ToArray());using var again=owner.Assess(Xml(Alias(kind)),context).Value!;Assert.Single(again.ReadRootItems());
        using var f=new Fixture(source,Xml(Good(kind)),atoms:16);using var other=new Fixture(source,Xml(Good(kind)),atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var handle=f.Provider.TypeComponent(fact.NativeType.DerivationBase!);Assert.True(f.Provider.Owns(handle));Assert.False(other.Provider.Owns(handle));var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>fact.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);
    }
    /// <summary>Forward named dependencies and final prohibitions retain source edges for all interchange families.</summary>
    [Fact]public void ForwardAndFinal()
    {
        foreach(var kind in Kinds){var source=Schema(Restrict("<xs:simpleType><xs:restriction base='B'/></xs:simpleType>"),true,"<xs:simpleType name='B'><xs:restriction base='xs:"+kind+"'/></xs:simpleType>");var c=Compile(source);Assert.Same(c.NamedTypes.Single(t=>t.Name=="B"),c.Root.Type.DerivationBase!.DerivationBase);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(source.Replace("name='B'","name='B' final='restriction'"),new(256,256),maxStringCharacters:1024).Outcome);}
    }
    /// <summary>Shared references and separate source definitions retain original unnamed bases.</summary>
    [Fact]public void SharedAndDistinct()
    {
        foreach(var kind in Kinds){var definition=Restrict(Base(kind));var c=Compile(Schema(definition,true,"<xs:element name='b' type='T'/>"));Assert.Same(c.Roots.First().Root.Type,c.Roots.Skip(1).First().Root.Type);var d=Compile(Schema(definition,false,"<xs:element name='b'>"+definition+"</xs:element>"));Assert.NotSame(d.Roots.First().Root.Type.DerivationBase,d.Roots.Skip(1).First().Root.Type.DerivationBase);Assert.DoesNotContain(d.NamedTypes,t=>ReferenceEquals(t,d.Root.Type.DerivationBase));}
    }
    /// <summary>Inline depth and shared source-site capacities retain their finite ceilings.</summary>
    [Fact]public void Budgets()
    {
        var definition=Base();for(var i=0;i<4;i++)definition=Restrict(definition);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(definition),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(definition)),new(256,256),maxStringCharacters:1024).Outcome);var members=Enumerable.Repeat(Restrict(Base()),8).ToArray();string Union()=>"<xs:simpleType><xs:union>"+string.Concat(members)+"</xs:union></xs:simpleType>";Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);members[0]=Restrict(members[0]);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Union()),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>QName values use original facet bindings and independently scoped instance bindings.</summary>
    [Fact]public void NamespaceAuthority()
    {
        var c=Compile(Schema(Restrict(Base("QName","<xs:enumeration xmlns:p='urn:inner' value='p:item'/>"))));Assert.Equal(NativeOutcome.Valid,c.Assess("<a xmlns:r='urn:inner'>r:item</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess(Xml("p:item")).Outcome);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(Restrict(Base("QName","<xs:enumeration xmlns:p='urn:inner' value='p:item'/>"),"<xs:enumeration value='p:item'/>")),new(256,256),maxStringCharacters:1024).Outcome);var local=Compile(Schema(Restrict(Base("QName"),"<xs:enumeration xmlns:r='urn:one' value='r:item'/>")));Assert.Equal(NativeOutcome.Valid,local.Assess(Xml("q:item")).Outcome);
    }
    /// <summary>Nilled declarations retain their original governing type and typed-empty values.</summary>
    [Fact]public void NilledAuthority()
    {
        foreach(var kind in Kinds){var source=Schema(Restrict(Base(kind))).Replace("<xs:element name='a'>","<xs:element name='a' nillable='true'>");using var f=new Fixture(source,"<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal(Kind(kind),fact.NativeType.InterchangeKind);Assert.NotNull(fact.NativeType.DerivationBase);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);Assert.Empty(typed.AtomicItems);}
    }
    /// <summary>Binary lexical and octet capacities refuse before facts and preserve owner recovery.</summary>
    /// <param name="kind">Binary family.</param>
    [Theory][InlineData("hexBinary")][InlineData("base64Binary")]
    public void BinaryBudgets(string kind)
    {
        using var owner=NativeSchemaSession.Create(Schema(Restrict(Base(kind))),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);string Text(int count)=>kind=="hexBinary"?new string('0',count*2):Convert.ToBase64String(new byte[count]);using var maximum=owner.Assess(Xml(Text(128)),context).Value!;Assert.Equal(128,Assert.Single(maximum.ReadRootItems()).BinaryValue.Length);var refused=owner.Assess(Xml(Text(129)),context);Assert.Equal(NativeSchemaOutcome.Unsupported,refused.Outcome);Assert.Null(refused.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,refused.Diagnostics[0].UnsupportedReason);using var recovered=owner.Assess(Xml(Good(kind)),context).Value!;Assert.Single(recovered.ReadRootItems());
    }
}
