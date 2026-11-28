// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 28 November 2026
// PURPOSE              : Proves original anonymous floating bases, IEEE payloads and facet authority.
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
namespace Bosak.Schema.Tests.AnonymousFloatingBases;
/// <summary>Checks original unnamed floating bases, IEEE values and bounded facet authority.</summary>
public sealed class NativeAnonymousFloatingBasesProofTests
{
    private static readonly string[] Kinds=["float","double"];
    private static string Base(string kind="float",string facets="")=>"<xs:simpleType><xs:restriction base='xs:"+kind+"'>"+facets+"</xs:restriction></xs:simpleType>";
    private static string Restrict(string source,string facets="")=>"<xs:simpleType><xs:restriction>"+source+facets+"</xs:restriction></xs:simpleType>";
    private static string Schema(string definition,bool named=false,string extra="")=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+(named?"<xs:simpleType name='T'>"+definition["<xs:simpleType>".Length..]:"")+"<xs:element name='a'"+(named?" type='T'/>":">"+definition+"</xs:element>")+extra+"</xs:schema>";
    private static NativeDecimalSchema Compile(string source)=>NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;
    private static NativeFloatingKind Kind(string kind)=>kind=="float"?NativeFloatingKind.Float:NativeFloatingKind.Double;
    /// <summary>Named and unnamed outer types retain the original direct base and atomic provenance.</summary>
    /// <param name="kind">Floating family.</param><param name="named">Outer naming selector.</param>
    [Theory][InlineData("float",false)][InlineData("float",true)][InlineData("double",false)][InlineData("double",true)]
    public void TypeAuthority(string kind,bool named)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind)),named),"<a>1.25</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var t=fact.NativeType;Assert.Null(t.BaseName);Assert.Equal(named?"T":null,t.Name);Assert.NotNull(t.DerivationBase);Assert.Null(t.DerivationBase!.Name);Assert.NotSame(t,t.DerivationBase);Assert.Equal(Kind(kind),t.FloatingKind);Assert.Equal(t.FloatingKind,t.DerivationBase.FloatingKind);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(t.DerivationBase)));var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Equal(kind=="float"?XdmValueKind.Float:XdmValueKind.Double,atom.Value.Kind);Assert.Equal(1.25,atom.Value.DoubleValue);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>Bound/fixed ancestry and lexical-pattern exclusions retain existing family semantics.</summary>
    /// <param name="kind">Floating family.</param>
    [Theory][InlineData("float")][InlineData("double")]
    public void FacetAncestry(string kind)
    {
        var original=Base(kind,"<xs:minInclusive value='1.25' fixed='true'/>");var c=Compile(Schema(Restrict(original,"<xs:maxInclusive value='2'/>")));Assert.NotNull(c.Root.Type.DerivationBase!.FloatingFacets);Assert.NotSame(c.Root.Type.FloatingFacets,c.Root.Type.DerivationBase.FloatingFacets);Assert.Equal(NativeOutcome.Valid,c.Assess("<a>1.25</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>1</a>").Outcome);Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(Restrict(original,"<xs:minInclusive value='125e-2'/>")),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(Restrict(original,"<xs:minInclusive value='2'/>")),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(Base(kind,"<xs:pattern value='.*'/>"),"<xs:enumeration value='1.25'/>")),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Detached floating values, recovery, ownership, cancellation and expiry retain contracts.</summary>
    /// <param name="kind">Floating family.</param>
    [Theory][InlineData("float")][InlineData("double")]
    public void ValuesAndLifetime(string kind)
    {
        var source=Schema(Restrict(Base(kind)));using var owner=NativeSchemaSession.Create(source,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a>bad</a>",context).Outcome);using var d=owner.Assess("<a>-0</a>",context).Value!;var value=Assert.Single(d.ReadRootItems());d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootItems());Assert.Equal(kind=="float"?NativeScalarKind.Float:NativeScalarKind.Double,value.Kind);if(kind=="float")Assert.Equal(int.MinValue,BitConverter.SingleToInt32Bits(value.FloatValue));else Assert.Equal(long.MinValue,BitConverter.DoubleToInt64Bits(value.DoubleValue));using var again=owner.Assess("<a>1.25</a>",context).Value!;Assert.Single(again.ReadRootItems());
        using var f=new Fixture(source,"<a>1.25</a>",atoms:16);using var other=new Fixture(source,"<a>1.25</a>",atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var handle=f.Provider.TypeComponent(fact.NativeType.DerivationBase!);Assert.True(f.Provider.Owns(handle));Assert.False(other.Provider.Owns(handle));var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>fact.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);
    }
    /// <summary>Fixed IEEE bit patterns prove family-specific rounding, sign and edge values.</summary>
    /// <param name="kind">Floating family.</param><param name="text">Original lexical value.</param><param name="bits">Expected IEEE hexadecimal bits.</param>
    [Theory]
    [InlineData("float","1.25","3FA00000")][InlineData("float","16777217","4B800000")][InlineData("float","-0","80000000")][InlineData("float","-1e-999","80000000")][InlineData("float","1e999","7F800000")][InlineData("float","-1e999","FF800000")][InlineData("float","1.401298464324817e-45","00000001")][InlineData("float","3.4028234663852886e38","7F7FFFFF")]
    [InlineData("double","1.25","3FF4000000000000")][InlineData("double","9007199254740993","4340000000000000")][InlineData("double","-0","8000000000000000")][InlineData("double","-1e-999","8000000000000000")][InlineData("double","1e999","7FF0000000000000")][InlineData("double","-1e999","FFF0000000000000")][InlineData("double","4.9406564584124654e-324","0000000000000001")][InlineData("double","1.7976931348623157e308","7FEFFFFFFFFFFFFF")]
    public void IeeeBits(string kind,string text,string bits)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind))),"<a>"+text+"</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var atom=Assert.Single(fact.ReadTypedValue(default).Value.AtomicItems);Assert.Equal(kind=="float"?XdmValueKind.Float:XdmValueKind.Double,atom.Value.Kind);var actual=kind=="float"?unchecked((uint)BitConverter.SingleToInt32Bits((float)atom.Value.DoubleValue)).ToString("X8"):unchecked((ulong)BitConverter.DoubleToInt64Bits(atom.Value.DoubleValue)).ToString("X16");Assert.Equal(bits,actual);
    }
    /// <summary>NaN enumeration and zero equality preserve special payloads without widening bound support.</summary>
    /// <param name="kind">Floating family.</param>
    [Theory][InlineData("float")][InlineData("double")]
    public void SchemaSpecialEquality(string kind)
    {
        using var f=new Fixture(Schema(Restrict(Base(kind),"<xs:enumeration value='NaN'/>")),"<a>NaN</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(double.IsNaN(Assert.Single(fact.ReadTypedValue(default).Value.AtomicItems).Value.DoubleValue));var c=Compile(Schema(Restrict(Base(kind),"<xs:enumeration value='0'/>")));Assert.Equal(NativeOutcome.Valid,c.Assess("<a>-0</a>").Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(Restrict(Base(kind),"<xs:minInclusive value='NaN'/>")),new(256,256),maxStringCharacters:1024).Outcome);var empty=Compile(Schema(Restrict(Base(kind),"<xs:minExclusive value='1'/><xs:maxExclusive value='1'/>")));Assert.Equal(NativeOutcome.Invalid,empty.Assess("<a>1</a>").Outcome);
    }
    /// <summary>Forward named dependencies and final prohibitions retain source edges for all floating families.</summary>
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
    /// <summary>Nilled declarations retain original floating governing types and typed-empty values.</summary>
    [Fact]public void NilledAuthority()
    {
        foreach(var kind in Kinds){var source=Schema(Restrict(Base(kind))).Replace("<xs:element name='a'>","<xs:element name='a' nillable='true'>");using var f=new Fixture(source,"<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal(Kind(kind),fact.NativeType.FloatingKind);Assert.NotNull(fact.NativeType.DerivationBase);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);Assert.Empty(typed.AtomicItems);}
    }
    /// <summary>NaN bound failure issues no facts, and lexical capacity refusal leaves sessions usable.</summary>
    [Fact]public void FailureAndBudget()
    {
        foreach(var kind in Kinds){using var f=new Fixture(Schema(Restrict(Base(kind),"<xs:minInclusive value='0'/>")),"<a>NaN</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);using var owner=NativeSchemaSession.Create(Schema(Restrict(Base(kind))),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);var result=owner.Assess("<a>"+new string(' ',257)+"1</a>",context);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,result.Diagnostics[0].UnsupportedReason);using var d=owner.Assess("<a>1.25</a>",context).Value!;Assert.Single(d.ReadRootItems());}
    }
}
