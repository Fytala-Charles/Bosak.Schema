// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 28 November 2026
// PURPOSE              : Proves named integer base identity, exact facets, pattern ancestry and lifetimes.
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
namespace Bosak.Schema.Tests.NamedIntegerBases;
/// <summary>Checks original named integer bases through ordinary and numeric-pattern routes.</summary>
public sealed class NativeNamedIntegerBasesProofTests
{
    private static string Schema(string family="integer",bool pattern=false,string own="",string parent="",string extra="")=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='T'><xs:restriction base='B'>"+own+(pattern?"<xs:pattern value='.*5'/>":"")+"</xs:restriction></xs:simpleType><xs:element name='a' type='T'/><xs:simpleType name='B'><xs:restriction base='xs:"+family+"'>"+parent+"</xs:restriction></xs:simpleType>"+extra+"</xs:schema>";
    private static NativeDecimalSchema Compile(string source)=>NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024).Value!;
    /// <summary>Every built-in family retains the actual named base and governing/atomic authority.</summary>
    /// <param name="family">Built-in integer family.</param><param name="pattern">Selects the numeric-pattern path.</param>
    [Theory]
    [InlineData("integer",false)][InlineData("integer",true)][InlineData("nonPositiveInteger",false)][InlineData("nonPositiveInteger",true)][InlineData("negativeInteger",false)][InlineData("negativeInteger",true)]
    [InlineData("long",false)][InlineData("long",true)][InlineData("int",false)][InlineData("int",true)][InlineData("short",false)][InlineData("short",true)][InlineData("byte",false)][InlineData("byte",true)]
    [InlineData("nonNegativeInteger",false)][InlineData("nonNegativeInteger",true)][InlineData("unsignedLong",false)][InlineData("unsignedLong",true)][InlineData("unsignedInt",false)][InlineData("unsignedInt",true)][InlineData("unsignedShort",false)][InlineData("unsignedShort",true)][InlineData("unsignedByte",false)][InlineData("unsignedByte",true)][InlineData("positiveInteger",false)][InlineData("positiveInteger",true)]
    public void TypeAuthority(string family,bool pattern)
    {
        var text=family is "negativeInteger" or "nonPositiveInteger"?"-5":"5";var source=Schema(family,pattern);var c=Compile(source);var named=c.NamedTypes.Single(t=>t.Name=="B");Assert.Same(named,c.Root.Type.DerivationBase);Assert.Equal("B",c.Root.Type.BaseName!.LocalName);Assert.Equal("",c.Root.Type.BaseName.NamespaceName);Assert.True(named.IsInteger);Assert.True(c.Root.Type.IsInteger);Assert.Equal(pattern,c.Root.Type.NumericPatterns is not null);using var f=new Fixture(source,"<a>"+text+"</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal("T",fact.NativeType.Name);Assert.Equal("B",fact.NativeType.DerivationBase!.Name);Assert.True(f.Provider.Owns(f.Provider.TypeComponent(fact.NativeType.DerivationBase)));var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);var atom=Assert.Single(typed.AtomicItems);Assert.Equal(XdmValueKind.Integer,atom.Value.Kind);Assert.Equal(text,atom.Value.IntegerValue.ToString());Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(fact.Type,atom.TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,atom.TypeEvidence.OriginalAssessment);
    }
    /// <summary>Exact inherited endpoints, enumeration and digit restrictions follow the original named base.</summary>
    [Fact]public void FacetAncestry()
    {
        var c=Compile(Schema("int",own:"<xs:maxExclusive value='5'/>",parent:"<xs:maxInclusive value='5'/>"));Assert.Equal(NativeOutcome.Valid,c.Assess("<a>4</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>5</a>").Outcome);Assert.NotSame(c.Root.Type.IntegerFacets,c.Root.Type.DerivationBase!.IntegerFacets);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(own:"<xs:totalDigits value='3'/>",parent:"<xs:totalDigits value='2'/>"),new(256,256),maxStringCharacters:1024).Outcome);var enumeration=Compile(Schema(own:"<xs:enumeration value='+005'/>",parent:"<xs:enumeration value='5'/>"));Assert.Equal(NativeOutcome.Valid,enumeration.Assess("<a>0005</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,enumeration.Assess("<a>6</a>").Outcome);
    }
    /// <summary>Exact detached values, owner recovery, cancellation, foreign components and expiry remain bounded.</summary>
    [Fact]public void PrecisionAndLifetime()
    {
        const string value="123456789012345678901234567890123456789";var source=Schema();using var owner=NativeSchemaSession.Create(source,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,owner.Assess("<a>bad</a>",context).Outcome);using var d=owner.Assess("<a>"+value+"</a>",context).Value!;var item=Assert.Single(d.ReadRootItems());d.Dispose();Assert.Equal(value,item.IntegerValue.ToString());Assert.Throws<ObjectDisposedException>(()=>d.ReadRootItems());using var recovered=owner.Assess("<a>5</a>",context).Value!;Assert.Single(recovered.ReadRootItems());using var f=new Fixture(source,"<a>5</a>",atoms:16);using var other=new Fixture(source,"<a>5</a>",atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var handle=f.Provider.TypeComponent(fact.NativeType.DerivationBase!);Assert.True(f.Provider.Owns(handle));Assert.False(other.Provider.Owns(handle));var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(fact.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>fact.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.Declaration);
    }
    /// <summary>Forward named definitions, eager cycles and final prohibitions keep their existing classifications.</summary>
    [Fact]public void ForwardFinalAndCycles()
    {
        var c=Compile(Schema(extra:"<xs:simpleType name='U'><xs:restriction base='T'/></xs:simpleType>"));Assert.Same(c.Root.Type,c.NamedTypes.Single(t=>t.Name=="U").DerivationBase);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema().Replace("name='B'","name='B' final='restriction'"),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema().Replace("base='xs:integer'","base='T'"),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Invalid,NativeDecimalSchema.Compile(Schema(extra:"<xs:simpleType name='U'><xs:restriction base='B'><xs:fractionDigits value='1'/></xs:restriction></xs:simpleType>"),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Foreign named simple bases preserve original namespace and shared component identity through catalog assembly.</summary>
    [Fact]public void CatalogQNameAndSharing()
    {
        const string root="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:b='urn:base'><xs:import namespace='urn:base' schemaLocation='base.xsd'/><xs:simpleType name='T'><xs:restriction base='b:B'><xs:maxExclusive value='5'/></xs:restriction></xs:simpleType><xs:element name='a' type='T'/><xs:element name='c' type='T'/></xs:schema>";const string lib="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:base'><xs:simpleType name='B'><xs:restriction base='xs:int'><xs:maxInclusive value='5'/></xs:restriction></xs:simpleType></xs:schema>";var assembled=NativeDecimalSchema.AssembleCatalog("https://example.test/main.xsd",new Dictionary<string,string>{{"https://example.test/main.xsd",root},{"https://example.test/base.xsd",lib}},default).Value!;var c=NativeDecimalSchema.Compile(assembled.Text,new(256,256),maxStringCharacters:1024,importedTypeNamespaces:assembled.SimpleTypeNamespaces).Value!;Assert.Equal("urn:base",c.Root.Type.BaseName!.NamespaceName);Assert.Equal("urn:base",c.Root.Type.DerivationBase!.NamespaceUri);Assert.Same(c.Roots.First().Root.Type,c.Roots.Last().Root.Type);using var f=new Fixture(assembled.Text,"<a>4</a>",importedTypeNamespaces:assembled.SimpleTypeNamespaces,atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal("urn:base",f.Input.FactsFor((NativeSessionNode)f.Element([]))!.NativeType.DerivationBase!.NamespaceUri);
    }
    /// <summary>Nilled original declarations retain named integer governing type and typed-empty values.</summary>
    [Fact]public void NilledAuthority()
    {
        using var f=new Fixture(Schema().Replace("name='a' type='T'","name='a' type='T' nillable='true'"),"<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal("B",fact.NativeType.DerivationBase!.Name);var typed=fact.ReadTypedValue(default).Value;Assert.Same(fact.Type,typed.GoverningType);Assert.Empty(typed.AtomicItems);
    }
    /// <summary>Named type and supplied-digit ceilings remain independent of the new base route.</summary>
    [Fact]public void Capacities()
    {
        string Chain(int count)=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+string.Concat(Enumerable.Range(0,count).Select(i=>"<xs:simpleType name='N"+i+"'><xs:restriction base='"+(i==0?"xs:integer":"N"+(i-1))+"'/></xs:simpleType>"))+"<xs:element name='a' type='N"+(count-1)+"'/></xs:schema>";Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Chain(16),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Chain(17),new(256,256),maxStringCharacters:1024).Outcome);using var owner=NativeSchemaSession.Create(Schema(),new(2,0,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Unsupported,owner.Assess("<a>0005</a>",context).Outcome);using var recovered=owner.Assess("<a>5</a>",context).Value!;Assert.Single(recovered.ReadRootItems());
    }
    /// <summary>Inherited pattern exclusions, actual list-item identity and no-facts invalidity remain.</summary>
    [Fact]public void PatternsAndMembers()
    {
        Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(own:"<xs:enumeration value='5'/>",parent:"<xs:pattern value='.*'/>"),new(256,256),maxStringCharacters:1024).Outcome);using var bad=new Fixture(Schema(pattern:true),"<a>6</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.Empty(bad.Assessment.Facts);var source=Schema().Replace("<xs:restriction base='B'></xs:restriction>","<xs:list itemType='B'/>");using var f=new Fixture(source,"<a>5 6</a>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.IsList);Assert.All(typed.AtomicItems,item=>{Assert.Same(fact.Type,item.TypeEvidence.DeclaredType);Assert.Same(f.Provider.TypeComponent(fact.NativeType.ListItemType!),item.TypeEvidence.ActualType);});
    }
}
