// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies evidence-selected empty groups and anonymous attribute type contracts.
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
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.StructuralCoverage;

/// <summary>Verifies actual component authority, original typed facts and finite source budgets.</summary>
public sealed class NativeStructuralCoverageIdentityTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private const string Anonymous="<xs:simpleType><xs:restriction base='xs:int'><xs:minInclusive value='1'/></xs:restriction></xs:simpleType>";
    private static string Schema(string declarations,string content)=>$"<xs:schema xmlns:xs='{X}'>{declarations}<xs:element name='r'><xs:complexType><xs:sequence>{content}</xs:sequence></xs:complexType></xs:element></xs:schema>";
    /// <summary>Empty model groups retain authenticated empty atomization, original governing identity and expiry.</summary>
    /// <param name="group">Source model group.</param>
    [Theory][InlineData("<xs:all/>")][InlineData("<xs:all minOccurs='0'/>")][InlineData("<xs:choice minOccurs='0'/>")]
    public void EmptyFacts(string group)
    {
        var schema=$"<xs:schema xmlns:xs='{X}'><xs:complexType name='E'>{group}</xs:complexType><xs:element name='r' type='E'/></xs:schema>";
        using var f=new Fixture(schema,"<r/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.True(fact.NativeType.EmptyContent);Assert.False(fact.HasNoTypedValue);
        var raw=fact.ReadTypedValue(default);Assert.Equal(SchemaResultKind.Supported,raw.Kind);Assert.Empty(raw.Value.AtomicItems);Assert.Same(fact.Type,raw.Value.GoverningType);Assert.False(raw.Value.IsList);
        Assert.True(f.Session.Advisor!.OwnsTypedValue(raw.Value,f.Input,f.Assessment.Origin));
        Assert.Equal(XdmValueKind.Undefined,f.Run(op=>f.Evaluate(op,f.Input.Root)).Kind);
        f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));
    }
    /// <summary>Global references share one original anonymous type and canonical declaration across owners.</summary>
    [Fact]
    public void GlobalAnonymousIdentity()
    {
        var declarations="<xs:attribute name='a'>"+Anonymous+"</xs:attribute>";
        string Owner(string name)=>"<xs:element name='"+name+"'><xs:complexType><xs:sequence><xs:element name='v' type='xs:string'/></xs:sequence><xs:attribute ref='a'/></xs:complexType></xs:element>";
        using var f=new Fixture(Schema(declarations,Owner("one")+Owner("two")),"<r><one a='1'><v>x</v></one><two a='2'><v>x</v></two></r>");
        Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([0],"","a"))!;var b=f.Input.FactsFor((NativeSessionNode)f.Attribute([1],"","a"))!;
        Assert.Same(a.NativeType,b.NativeType);Assert.Null(a.NativeType.Name);Assert.Same(a.Type,b.Type);Assert.Same(a.Declaration,b.Declaration);
        Assert.Same(a.Declaration,f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value);Assert.NotSame(a.Node,b.Node);
        var raw=a.ReadTypedValue(default);Assert.Equal(SchemaResultKind.Supported,raw.Kind);Assert.Same(a.Type,raw.Value.GoverningType);Assert.Single(raw.Value.AtomicItems);
        Assert.True(f.Session.Advisor!.OwnsTypedValue(raw.Value,f.Input,f.Assessment.Origin));
        Assert.Equal("1",f.Read(a.Node).IntegerValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
        f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>a.ReadTypedValue(default));
    }
    /// <summary>Distinct source definitions never collapse to an invented common anonymous QName.</summary>
    [Fact]
    public void DistinctAnonymousIdentity()
    {
        string Owner(string n)=>"<xs:element name='"+n+"'><xs:complexType><xs:sequence><xs:element name='v' type='xs:string'/></xs:sequence><xs:attribute name='a'>"+Anonymous+"</xs:attribute></xs:complexType></xs:element>";
        using var f=new Fixture(Schema("",Owner("one")+Owner("two")),"<r><one a='1'><v>x</v></one><two a='2'><v>x</v></two></r>");
        var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([0],"","a"))!;var b=f.Input.FactsFor((NativeSessionNode)f.Attribute([1],"","a"))!;
        Assert.Null(a.NativeType.Name);Assert.Null(b.NativeType.Name);Assert.NotSame(a.NativeType,b.NativeType);Assert.NotSame(a.Type,b.Type);Assert.NotSame(a.Declaration,b.Declaration);
    }
    /// <summary>Unused global attribute types consume the shared original-site ceiling eagerly.</summary>
    /// <param name="count">Total anonymous source sites.</param><param name="expected">Expected result.</param>
    [Theory][InlineData(16,"Valid")][InlineData(17,"Unsupported")]
    public void AnonymousBudget(int count,string expected)
    {
        var declarations=string.Concat(Enumerable.Range(0,count-1).Select(i=>"<xs:attribute name='g"+i+"'>"+Anonymous+"</xs:attribute>"));
        var s=$"<xs:schema xmlns:xs='{X}'>{declarations}<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:string'/></xs:sequence><xs:attribute name='a'>{Anonymous}</xs:attribute></xs:complexType></xs:element></xs:schema>";
        var c=NativeSchemaSession.Create(s,new(256,256,16384,65536,1,256,16,256,1024));Assert.Equal(expected,c.Outcome.ToString());c.Value?.Dispose();
    }
}
