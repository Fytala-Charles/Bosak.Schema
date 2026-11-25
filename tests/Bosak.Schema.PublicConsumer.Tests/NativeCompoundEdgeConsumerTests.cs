// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies compound read capacity, cancellation, lifetime and imported contexts.
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
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks complex restrictions, fixed overrides and member selection refusal.</summary>
public sealed class NativeCompoundEdgeConsumerTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,256,65536,1,256,16,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Checks simple-content restriction carries actual atomic compound content.</summary>
    /// <param name="data">Original-node selector.</param><param name="union">Union selector.</param>
    [Theory][InlineData(false,false)][InlineData(true,false)][InlineData(false,true)][InlineData(true,true)]
    public void ComplexRestriction(bool data,bool union)
    {
        var construction=union?"<xs:union memberTypes='xs:integer xs:boolean'/>":"<xs:list itemType='xs:integer'/>";var literal=union?"true":"1 2";
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='A'>"+construction+"</xs:simpleType><xs:complexType name='B'><xs:simpleContent><xs:extension base='A'/></xs:simpleContent></xs:complexType><xs:complexType name='D'><xs:simpleContent><xs:restriction base='B'><xs:enumeration value='"+literal+"'/></xs:restriction></xs:simpleContent></xs:complexType><xs:element name='r' type='D'/></xs:schema>";
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>3</r>",Context).Outcome);using var d=s.Assess("<r>"+literal+"</r>",Context).Value!;Assert.Equal(union?1:2,d.ReadElementItems([]).Length);}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>3</r>",Context).Outcome);using var d=s.Assess("<r>"+literal+"</r>",Context).Value!;Assert.Equal(union?1:2,d.ReadRootItems().Length);}
    }
    /// <summary>Checks equal singleton-list/decimal fixed overrides without discarding selected member proof.</summary>
    /// <param name="data">Original-node selector.</param><param name="same">Equal-value selector.</param>
    [Theory][InlineData(false,false)][InlineData(true,false)][InlineData(false,true)][InlineData(true,true)]
    public void GlobalFixedEquality(bool data,bool same)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='L'><xs:list itemType='xs:integer'/></xs:simpleType><xs:simpleType name='U'><xs:union memberTypes='L xs:decimal'/></xs:simpleType><xs:attribute name='a' type='U' fixed='1'/><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attribute ref='a' fixed='"+(same?"1.0":"2.0")+"'/></xs:complexType></xs:element></xs:schema>";
        var expected=same?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Invalid;
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(expected,c.Outcome);using var s=c.Value;if(s is null)return;using var d=s.Assess("<r a='1.0'><v>0</v></r>",Context).Value!;Assert.Equal(1,Assert.Single(d.ReadAttributeItems([],"","a")).DecimalValue.Coefficient);}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(expected,c.Outcome);using var s=c.Value;if(s is null)return;using var d=s.Assess("<r a='1.0'><v>0</v></r>",Context).Value!;Assert.Equal(1,Assert.Single(d.ReadAttributeItems(0)).DecimalValue.Coefficient);}
    }
    /// <summary>Checks valid member override is Unsupported rather than mislabeled Invalid.</summary>
    /// <param name="data">Original-node selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void MemberSelectionBoundary(bool data)
    {
        const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='U'><xs:union memberTypes='xs:string xs:integer'/></xs:simpleType><xs:element name='r' type='U'/></xs:schema>";
        const string xml="<r xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xs='http://www.w3.org/2001/XMLSchema' xsi:type='xs:integer'>1</r>";
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;Assert.Equal(NativeSchemaOutcome.Unsupported,s.Assess(xml,Context).Outcome);using var d=s.Assess("<r>1</r>",Context).Value!;Assert.Equal(NativeScalarKind.String,Assert.Single(d.ReadElementItems([])).Kind);}
        else{using var s=NativeSchemaSession.Create(schema,Options).Value!;Assert.Equal(NativeSchemaOutcome.Unsupported,s.Assess(xml,Context).Outcome);using var d=s.Assess("<r>1</r>",Context).Value!;Assert.Equal(NativeScalarKind.String,Assert.Single(d.ReadRootItems()).Kind);}
    }
}
