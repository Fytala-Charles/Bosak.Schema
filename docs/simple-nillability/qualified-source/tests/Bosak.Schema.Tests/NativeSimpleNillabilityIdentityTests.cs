// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies actual nilled Core facts and supported empty typed sequences.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.SimpleNillability;
/// <summary>Verifies actual nillability separately from detached empty value transport.</summary>
public sealed class NativeSimpleNillabilityIdentityTests
{
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:decimal' nillable='true'/></xs:schema>";
 private const string Xml="<r xmlns:i='http://www.w3.org/2001/XMLSchema-instance' i:nil='true'/>";
 /// <summary>Nilled facts carry a supported empty typed value and actual scalar declaration/type.</summary>
 [Fact]public void EmptyFacts(){using var f=new Fixture(Schema,Xml);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(fact.IsNilled);Assert.False(fact.HasNoTypedValue);Assert.True(fact.NativeDeclaration.IsNillable);var typed=fact.ReadTypedValue(default);Assert.Equal(SchemaResultKind.Supported,typed.Kind);Assert.Empty(typed.Value.AtomicItems);Assert.False(typed.Value.TryGetSingleAtomic(out _));Assert.Equal(XdmValueKind.Undefined,f.Run(op=>f.Evaluate(op,f.Element([]))).Kind);Assert.Equal(NativeScalarKind.Empty,f.Read(f.Element([])).Kind);}
 /// <summary>A nilled reference shares its actual global declaration rather than a per-use copy.</summary>
 [Fact]public void CanonicalGlobal(){var schema=Schema.Replace("<xs:element name='r' type='xs:decimal' nillable='true'/>","<xs:element name='r'><xs:complexType><xs:sequence><xs:element ref='v'/></xs:sequence></xs:complexType></xs:element><xs:element name='v' type='xs:decimal' nillable='true'/>");using var f=new Fixture(schema,Xml.Replace("<r ","<r><v ").Replace("/>","/></r>"));var fact=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.True(fact.IsNilled);Assert.Same(fact.Declaration,f.Provider.ResolveElementDeclaration(new("","v"),default).Value);Assert.True(fact.NativeDeclaration.IsNillable);}
 /// <summary>Nil and ordinary repeated originals share one declaration but retain distinct typed values.</summary>
 [Fact]public void RepeatedOriginals(){var schema=Schema.Replace("<xs:element name='r' type='xs:decimal' nillable='true'/>","<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal' nillable='true' maxOccurs='2'/></xs:sequence></xs:complexType></xs:element>");using var f=new Fixture(schema,"<r><v xmlns:i='http://www.w3.org/2001/XMLSchema-instance' i:nil='true'/><v>1.23</v></r>");var a=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;Assert.NotSame(a.Node,b.Node);Assert.Same(a.Declaration,b.Declaration);Assert.True(a.IsNilled);Assert.False(b.IsNilled);Assert.Equal(NativeScalarKind.Empty,f.Read(a.Node).Kind);Assert.Equal("1.23",f.Read(b.Node).DecimalValue.ToInvariantString(256));}
 /// <summary>A valid xsi:type is still the actual governing type even when its lexical membership is absent.</summary>
 [Fact]public void XsiType(){var schema=Schema.Replace("<xs:element name='r'", "<xs:simpleType name='D'><xs:restriction base='xs:decimal'><xs:enumeration value='1.23'/></xs:restriction></xs:simpleType><xs:element name='r'");using var f=new Fixture(schema,Xml.Replace("i:nil='true'","i:nil='true' i:type='D'"));var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(fact.IsNilled);Assert.Equal("D",fact.NativeType.Name);Assert.Same(fact.Type,f.Provider.ResolveType(new("","D"),default).Value);Assert.Empty(fact.ReadTypedValue(default).Value.AtomicItems);}
 /// <summary>Nil flags and empty issued values retain authority/lifetime while detached Empty remains usable.</summary>
 [Fact]public void AuthorityAndLifetime(){using var a=new Fixture(Schema,Xml);using var b=new Fixture(Schema,Xml);var x=a.Input.FactsFor((NativeSessionNode)a.Element([]))!;var y=b.Input.FactsFor((NativeSessionNode)b.Element([]))!;Assert.False(a.Provider.Owns(y.Type));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(x.Type,y.Type,default));var issued=x.ReadTypedValue(default).Value;var detached=a.Read(x.Node);a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>x.IsNilled);Assert.Throws<ObjectDisposedException>(()=>issued.AtomicItems.ToArray());Assert.Equal(NativeScalarKind.Empty,detached.Kind);}
 /// <summary>Text-bearing nilled nodes cannot issue successful facts or assessment evidence.</summary>
 [Fact]public void InvalidHasNoFacts(){using var f=new Fixture(Schema,Xml.Replace("/>","> </r>"));Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
 /// <summary>Legacy valid decimal schemas retain explicit nillability metadata refusal.</summary><param name="flag">Boolean lexical flag.</param>
 [Theory][InlineData("true")][InlineData("false")]public void Legacy(string flag){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal' nillable='"+flag+"'/></xs:sequence></xs:complexType></xs:element></xs:schema>";var refused=NativeDecimalSession.Create(schema,new(256,256,1,4,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,refused.Outcome);Assert.Null(refused.Value);using var control=NativeDecimalSession.Create(schema.Replace(" nillable='"+flag+"'",""),new(256,256,1,4,1,256,256)).Value!;}
}
