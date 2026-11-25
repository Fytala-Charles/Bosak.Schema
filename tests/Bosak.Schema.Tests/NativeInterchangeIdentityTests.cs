// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies exact binary and QName contracts through both public facades.
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
using System.Collections.Immutable;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
using Fixture = Bosak.Schema.Tests.NodeData.Fixture;
using Xunit;
namespace Bosak.Schema.Tests.NativeInterchange;
/// <summary>Verifies binary and QName facts and authority against actual Core sessions.</summary>
public sealed class NativeInterchangeIdentityTests
{
    private const string Start="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>";
    private static string Value(string kind) => kind == "QName" ? "item" : kind == "hexBinary" ? "61" : "YQ==";
    /// <summary>Checks exact binary and QName payloads and actual type proof for primitive and named atoms.</summary>
    /// <param name="kind">Interchange family.</param><param name="named">Named restriction selector.</param>
    [Theory][InlineData("hexBinary",false)][InlineData("base64Binary",false)][InlineData("QName",false)][InlineData("hexBinary",true)][InlineData("base64Binary",true)][InlineData("QName",true)]
    public void ExactFacts(string kind,bool named)
    {
        var schema=Start+(named?"<xs:simpleType name='N'><xs:restriction base='xs:"+kind+"'/></xs:simpleType>":"")+"<xs:element name='r' type='"+(named?"N":"xs:"+kind)+"'/></xs:schema>";
        using var f=new Fixture(schema,"<r>"+Value(kind)+"</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.TryGetSingleAtomic(out var atomic));Assert.Equal(kind == "QName" ? XdmValueKind.QName : XdmValueKind.String, atomic.Value.Kind);
        Assert.Equal(Enum.Parse<NativeScalarKind>(char.ToUpperInvariant(kind[0])+kind[1..]),f.Read(f.Input.Root).Kind);Assert.Same(fact.Type,typed.GoverningType);Assert.Same(f.Provider.TypeComponent(fact.NativeType),atomic.TypeEvidence.ActualType);Assert.True(f.Provider.Owns(atomic.TypeEvidence.ActualType));
        f.Run(op=>{var atom=f.Evaluate(op,f.Input.Root);Assert.Equal(kind == "QName" ? XdmValueKind.QName : XdmValueKind.String,atom.Kind);Assert.True(SchemaAtomics.TryGetProvenance(atom,out var provenance));Assert.Same(fact.Type,provenance!.DeclaredType);Assert.Same(atomic.TypeEvidence.ActualType,provenance.ActualType);Assert.Same(f.Assessment.Origin,provenance.OriginalAssessment);if(kind != "QName") Assert.Equal(kind, atom.SchemaTypeName); else Assert.Equal("item", atom.QNameValue.LocalName);return true;});
        using var other=new Fixture(schema,"<r>"+Value(kind)+"</r>");Assert.Throws<ArgumentException>(()=>other.Provider.IsSameComponent(fact.Type,other.Provider.DecimalType,default));
    }
    /// <summary>Checks anonymous, shared named and canonical global binary and QName identities.</summary>
    [Fact]
    public void ComponentIdentity()
    {
        var anonymous=Start+"<xs:element name='r'><xs:simpleType><xs:restriction base='xs:hexBinary'/></xs:simpleType></xs:element></xs:schema>";
        using(var f=new Fixture(anonymous,"<r>"+Value("hexBinary")+"</r>"))Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Input.Root)!.NativeType.Name);
        var schema=Start+"<xs:simpleType name='N'><xs:restriction base='xs:hexBinary'/></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='g'><xs:complexType><xs:sequence><xs:element ref='d'/></xs:sequence></xs:complexType></xs:element><xs:element name='h'><xs:complexType><xs:sequence><xs:element ref='d'/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element><xs:element name='d' type='N'/></xs:schema>";
        using var shared=new Fixture(schema,"<r><g><d>"+Value("hexBinary")+"</d></g><h><d>"+Value("hexBinary")+"</d></h></r>");
        var a=shared.Input.FactsFor((NativeSessionNode)shared.Element([0,0]))!;var b=shared.Input.FactsFor((NativeSessionNode)shared.Element([1,0]))!;
        Assert.Same(a.Type,b.Type);Assert.Same(a.Declaration,b.Declaration);Assert.Same(a.NativeDeclaration,b.NativeDeclaration);Assert.NotSame(a.Node,b.Node);
    }
}
