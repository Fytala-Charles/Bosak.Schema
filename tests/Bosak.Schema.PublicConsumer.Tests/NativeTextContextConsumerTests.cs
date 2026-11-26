// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Tests closed text imports, namespace identity and inherited length constraints.
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
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Checks foreign same-name text types and normative inherited length combinations.</summary>
public sealed class NativeTextContextConsumerTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,256,65536,1,256,16,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Checks same-local-name imported types retain independent values and namespace bindings.</summary>
    /// <param name="data">Original-node selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void ImportedTypes(bool data)
    {
        string Library(string ns,string literal)=>"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='"+ns+"'><xs:simpleType name='Label'><xs:restriction base='xs:token'><xs:enumeration value='"+literal+"'/></xs:restriction></xs:simpleType></xs:schema>";
        var catalog=new Dictionary<string,string>{["https://example.test/root.xsd"]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:a='urn:a' xmlns:b='urn:b'><xs:import namespace='urn:a' schemaLocation='a.xsd'/><xs:import namespace='urn:b' schemaLocation='b.xsd'/><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='a:Label'/><xs:element name='b' type='b:Label'/></xs:sequence></xs:complexType></xs:element></xs:schema>",["https://example.test/a.xsd"]=Library("urn:a","a"),["https://example.test/b.xsd"]=Library("urn:b","b")};
        const string good="<r><a> a </a><b> b </b></r>",bad="<r><a>b</a><b>a</b></r>";
        if(data){var c=NativeSchemaNodeDataSession.Create("https://example.test/root.xsd",catalog,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess(bad,Context).Outcome);using var d=s.Assess(good,Context).Value!;Assert.Equal("a",d.ReadElement([0]).StringValue);Assert.Equal("b",d.ReadElement([1]).StringValue);}
        else{var c=NativeSchemaSession.Create("https://example.test/root.xsd",catalog,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess(bad,Context).Outcome);using var d=s.Assess(good,Context).Value!;Assert.Equal("a",d.ReadScalar(0).StringValue);Assert.Equal("b",d.ReadScalar(1).StringValue);}
    }
    /// <summary>Checks same-step length/bounds require the normative ancestor without length.</summary>
    /// <param name="data">Original-node selector.</param><param name="qualifiedAncestor">Qualifying ancestor selector.</param>
    [Theory][InlineData(false,false)][InlineData(true,false)][InlineData(false,true)][InlineData(true,true)]
    public void InheritedLength(bool data,bool qualifiedAncestor)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='B'><xs:restriction base='xs:token'>"+(qualifiedAncestor?"<xs:minLength value='1'/><xs:maxLength value='3'/>":"")+"</xs:restriction></xs:simpleType><xs:simpleType name='T'><xs:restriction base='B'><xs:length value='2'/><xs:minLength value='1'/><xs:maxLength value='3'/></xs:restriction></xs:simpleType><xs:element name='r' type='T'/></xs:schema>";
        var expected=qualifiedAncestor?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Invalid;
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(expected,c.Outcome);using var s=c.Value;if(s is null)return;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>a</r>",Context).Outcome);using var d=s.Assess("<r> ab </r>",Context).Value!;Assert.Equal("ab",d.ReadElement([]).StringValue);}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(expected,c.Outcome);using var s=c.Value;if(s is null)return;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>a</r>",Context).Outcome);using var d=s.Assess("<r> ab </r>",Context).Value!;Assert.Equal("ab",d.ReadRootScalar().StringValue);}
    }
    /// <summary>Checks new builtins do not reduce the finite user type definition allowance.</summary>
    /// <param name="count">Definition count.</param><param name="valid">Expected admission.</param>
    [Theory][InlineData(16,true)][InlineData(17,false)]
    public void Definitions(int count,bool valid)
    {
        var definitions=string.Concat(Enumerable.Range(0,count).Select(i=>"<xs:simpleType name='T"+i+"'><xs:restriction base='xs:token'/></xs:simpleType>"));
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+definitions+"<xs:element name='r' type='T0'/></xs:schema>";
        var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(valid?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Unsupported,c.Outcome);using var s=c.Value;if(s is null)return;using var d=s.Assess("<r> a </r>",Context).Value!;Assert.Equal("a",d.ReadRootScalar().StringValue);
    }
    /// <summary>Checks repeated inherited bounds cannot hide an ancestor's exact length.</summary>
    /// <param name="length">Final exact length.</param><param name="valid">Expected schema validity.</param>
    [Theory][InlineData(1,false)][InlineData(2,true)][InlineData(3,false)]
    public void RepeatedBoundsRetainExactLength(int length,bool valid)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='B'><xs:restriction base='xs:token'><xs:minLength value='1'/><xs:maxLength value='3'/></xs:restriction></xs:simpleType><xs:simpleType name='C'><xs:restriction base='B'><xs:length value='2'/></xs:restriction></xs:simpleType><xs:simpleType name='D'><xs:restriction base='C'><xs:minLength value='1'/><xs:maxLength value='3'/></xs:restriction></xs:simpleType><xs:simpleType name='T'><xs:restriction base='D'><xs:length value='"+length+"'/></xs:restriction></xs:simpleType><xs:element name='r' type='T'/></xs:schema>";
        var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(valid?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Invalid,c.Outcome);using var s=c.Value;if(s is null)return;using var d=s.Assess("<r> ab </r>",Context).Value!;Assert.Equal("ab",d.ReadRootScalar().StringValue);
    }
}
