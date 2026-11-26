// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies original facts for provided element default and fixed constraints.
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
//                      | Charles Korthout | 0.21  | 26-11-2026     | Known-declaration controls retain explicit nullable preconditions.                        |
// ===========================================================================================================================================================
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.ElementValueConstraints;
/// <summary>Verifies actual provided-value constraints and original-node authority.</summary>
public sealed class NativeElementValueConstraintsIdentityTests
{
 private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:decimal' fixed='1.23'/></xs:schema>";
 /// <summary>Default metadata does not replace a distinct provided original value.</summary>
 [Fact]public void DefaultUsesProvided(){using var f=new Fixture(Schema.Replace("fixed=","default="),"<r>2.3400</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal("r",fact.NativeDeclaration.Name);Assert.Equal("2.3400",f.Element([]).StringValue);Assert.Equal("2.34",f.Read(fact.Node).DecimalValue.ToInvariantString(256));Assert.Equal(SchemaResultKind.NotFound,f.Provider.ResolveElementDeclaration(new("","literal"),default).Kind);}
 /// <summary>Exact fixed equality preserves the original lexical content and actual declaration/type.</summary>
 [Fact]public void FixedExactOriginal(){using var f=new Fixture(Schema,"<r>+0001.2300</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal("+0001.2300",f.Element([]).StringValue);Assert.Equal("1.23",f.Read(fact.Node).DecimalValue.ToInvariantString(256));Assert.Same(fact.Declaration,f.Provider.ResolveElementDeclaration(new("","r"),default).Value);Assert.False(fact.IsNilled);}
 /// <summary>Referenced fixed declarations remain one canonical component for repeated original nodes.</summary>
 [Fact]public void CanonicalGlobal(){var schema=Schema.Replace("<xs:element name='r' type='xs:decimal' fixed='1.23'/>","<xs:element name='r'><xs:complexType><xs:sequence><xs:element ref='v' maxOccurs='2'/></xs:sequence></xs:complexType></xs:element><xs:element name='v' type='xs:decimal' fixed='1.23'/>");using var f=new Fixture(schema,"<r><v>1.2300</v><v>+01.23</v></r>");var a=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;Assert.NotSame(a.Node,b.Node);Assert.Same(a.Declaration,b.Declaration);Assert.Same(a.Declaration,f.Provider.ResolveElementDeclaration(new("","v"),default).Value);Assert.Equal("1.23",f.Read(b.Node).DecimalValue.ToInvariantString(256));}
 /// <summary>Default metadata remains suppressed by nil, while fixed metadata makes nil invalid.</summary>
 [Fact]public void NilInteraction(){var schema=Schema.Replace("fixed=","nillable='true' default=");const string xml="<r xmlns:i='http://www.w3.org/2001/XMLSchema-instance' i:nil='true'/>";using var a=new Fixture(schema,xml);var fact=a.Input.FactsFor((NativeSessionNode)a.Element([]))!;Assert.True(fact.IsNilled);Assert.Empty(fact.ReadTypedValue(default).Value.AtomicItems);using var b=new Fixture(schema.Replace("default=","fixed="),xml);Assert.Equal(SchemaAssessmentStatus.Invalid,b.Assessment.Status);Assert.True(b.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(b.Assessment.Evidence);}
 /// <summary>Invalid fixed membership cannot issue successful facts or evidence.</summary>
 [Fact]public void InvalidHasNoFacts(){using var f=new Fixture(Schema,"<r>1.24</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.True(f.Assessment.Facts.IsDefaultOrEmpty);Assert.Null(f.Assessment.Evidence);}
 /// <summary>Constraint-bearing original facts retain independent authority and owned lifetime.</summary>
 [Fact]public void AuthorityAndLifetime(){using var a=new Fixture(Schema,"<r>1.23</r>");using var b=new Fixture(Schema,"<r>1.23</r>");var x=a.Input.FactsFor((NativeSessionNode)a.Element([]))!;var y=b.Input.FactsFor((NativeSessionNode)b.Element([]))!;Assert.False(a.Provider.Owns(y.Declaration!));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(x.Declaration!,y.Declaration!,default));var detached=a.Read(x.Node).DecimalValue;a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>x.ReadTypedValue(default));Assert.Equal("1.23",detached.ToInvariantString(256));}
 /// <summary>Legacy valid decimal owner schemas still refuse default/fixed metadata.</summary>
 [Fact]public void Legacy(){foreach(var kind in new[]{"default","fixed"}){var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal' "+kind+"='1.23'/></xs:sequence></xs:complexType></xs:element></xs:schema>";var result=NativeDecimalSession.Create(schema,new(256,256,1,4,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);}}
}
