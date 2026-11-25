// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies actual mixed governing/atomic identities and original ordered text facts.
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
namespace Bosak.Schema.Tests.MixedSequences;
/// <summary>Verifies mixed content has actual untypedAtomic provenance instead of string type substitution.</summary>
public sealed class NativeMixedSequencesIdentityTests
{
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType mixed='true'><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
 private const string Xml="<r>pre<v>1.2300</v>post</r>";
 private const string Xs="http://www.w3.org/2001/XMLSchema";
 /// <summary>Both actual atomic provenance and payload annotation retain untypedAtomic under the governing complex type.</summary>
 [Fact]public void ActualUntypedAtomic(){using var f=new Fixture(Schema,Xml);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(fact.NativeType.IsComplex);Assert.True(fact.NativeType.MixedContent);Assert.False(fact.HasNoTypedValue);var cell=Assert.Single(fact.ReadTypedValue(default).Value.AtomicItems);Assert.Equal(XdmValueKind.String,cell.Value.Kind);Assert.Equal("untypedAtomic",cell.Value.SchemaTypeName);Assert.Same(fact.Type,cell.TypeEvidence!.DeclaredType);Assert.Same(f.Provider.ResolveType(new(Xs,"untypedAtomic"),default).Value,cell.TypeEvidence!.ActualType);Assert.NotSame(f.Provider.ResolveType(new(Xs,"string"),default).Value,cell.TypeEvidence!.ActualType);var transported=f.Run(op=>f.Evaluate(op,f.Element([])));Assert.Equal("untypedAtomic",transported.SchemaTypeName);Assert.Equal("pre1.2300post",transported.StringValue);}
 /// <summary>An empty mixed aggregate is one empty untypedAtomic string, not an empty typed sequence or nil.</summary>
 [Fact]public void EmptyStringIsOneAtomic(){using var f=new Fixture(Schema.Replace("type='xs:decimal'/>","type='xs:decimal' minOccurs='0'/>"),"<r/>");var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.False(fact.IsNilled);Assert.False(fact.HasNoTypedValue);var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.TryGetSingleAtomic(out var cell));Assert.Single(typed.AtomicItems);Assert.Equal("",cell.Value.StringValue);Assert.Equal(NativeScalarKind.String,f.Read(f.Element([])).Kind);}
 /// <summary>Original text/element order, child positions and primitive values remain independent.</summary>
 [Fact]public void OriginalOrder(){using var f=new Fixture(Schema,Xml);var children=new List<IXdmNode>();foreach(var item in f.Element([]).Children())children.Add(item.NodeValue);Assert.Equal(3,children.Count);Assert.Equal(XdmNodeKind.Text,children[0].NodeKind);Assert.Equal(XdmNodeKind.Element,children[1].NodeKind);Assert.Equal(XdmNodeKind.Text,children[2].NodeKind);Assert.Same(f.Element([0]),children[1]);Assert.Equal("pre1.2300post",f.Element([]).StringValue);Assert.Equal("1.23",f.Read(f.Element([0])).DecimalValue.ToInvariantString(256));}
 /// <summary>Named mixed global references retain actual declaration and governing type handles.</summary>
 [Fact]public void NamedGlobal(){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:complexType name='M' mixed='true'><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence></xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element ref='box'/></xs:sequence></xs:complexType></xs:element><xs:element name='box' type='M'/></xs:schema>";using var f=new Fixture(schema,"<r><box>pre<v>1.2300</v>post</box></r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.Same(fact.Declaration,f.Provider.ResolveElementDeclaration(new("","box"),default).Value);Assert.Same(fact.Type,f.Provider.ResolveType(new("","M"),default).Value);Assert.Equal("pre1.2300post",f.Read(f.Element([0])).StringValue);}
 /// <summary>Mixed text aggregation does not consume attribute text or alter typed attribute/child values.</summary>
 [Fact]public void AttributesAndChildren(){var schema=Schema.Replace("</xs:sequence></xs:complexType>","</xs:sequence><xs:attribute name='a' type='xs:decimal' fixed='1.23'/></xs:complexType>");using var f=new Fixture(schema,Xml.Replace("<r>","<r a='1.23'>"));Assert.Equal("pre1.2300post",f.Read(f.Element([])).StringValue);Assert.Equal("1.23",f.Read(f.Attribute([],"","a")).DecimalValue.ToInvariantString(256));Assert.Equal("1.23",f.Read(f.Element([0])).DecimalValue.ToInvariantString(256));}
 /// <summary>Mixed issued values and their atomic components retain exact authority/lifetime.</summary>
 [Fact]public void AuthorityAndLifetime(){using var a=new Fixture(Schema,Xml);using var b=new Fixture(Schema,Xml);var x=a.Input.FactsFor((NativeSessionNode)a.Element([]))!;var y=b.Input.FactsFor((NativeSessionNode)b.Element([]))!;var xa=Assert.Single(x.ReadTypedValue(default).Value.AtomicItems).TypeEvidence!.ActualType;var ya=Assert.Single(y.ReadTypedValue(default).Value.AtomicItems).TypeEvidence!.ActualType;Assert.False(a.Provider.Owns(ya));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(xa,ya,default));var issued=x.ReadTypedValue(default).Value;var detached=a.Read(x.Node);a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>issued.AtomicItems.ToArray());Assert.Equal("pre1.2300post",detached.StringValue);}
 /// <summary>A malformed typed child prevents publication of otherwise valid mixed parent facts.</summary>
 [Fact]public void InvalidHasNoFacts(){using var f=new Fixture(Schema,"<r>pre<v>bad</v>post</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);Assert.False(f.Input.IsAssessed);}
 /// <summary>The legacy decimal profile retains its mixed metadata refusal.</summary>
 [Fact]public void Legacy(){var result=NativeDecimalSession.Create(Schema,new(256,256,1,4,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);using var control=NativeDecimalSession.Create(Schema.Replace(" mixed='true'",""),new(256,256,1,4,1,256,256)).Value!;}
}
