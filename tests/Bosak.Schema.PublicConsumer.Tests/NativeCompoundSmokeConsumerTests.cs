// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies compound native values through actual Core operations.
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
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks actual Core ordered list/union atomization and explicit read boundaries.</summary>
public sealed class NativeCompoundSmokeConsumerTests
{
    private static NativeSchemaSessionOptions Options => new(256,256,256,65536,1,256,16,256,256);
    private static NativeSchemaAssessmentContext Context => new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Checks empty, singleton and multiple-item list boundaries.</summary>
    /// <param name="data">Original-node selector.</param><param name="lexical">Supplied list.</param><param name="count">Expected item count.</param>
    [Theory][InlineData(false,"",0)][InlineData(true,"",0)][InlineData(false,"1",1)][InlineData(true,"1",1)][InlineData(false,"1 2 3",3)][InlineData(true,"1 2 3",3)]
    public void Lists(bool data,string lexical,int count)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='L'><xs:list itemType='xs:integer'/></xs:simpleType><xs:element name='r' type='L'/></xs:schema>";
        var xml="<r>"+lexical+"</r>";ImmutableArray<NativeScalarValue> items;
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.True(c.Outcome==NativeSchemaOutcome.Valid,string.Join(";",c.Diagnostics));using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.Throws<NotSupportedException>(()=>d.ReadElement([]));items=d.ReadElementItems([]);}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.False(d.HasRootScalar);Assert.Throws<NotSupportedException>(()=>d.ReadRootScalar());items=d.ReadRootItems();}
        Assert.Equal(count,items.Length);for(int i=0;i<items.Length;i++)Assert.Equal(i+1,items[i].IntegerValue);
    }
    /// <summary>Checks source-order selection and atomic scalar compatibility.</summary>
    /// <param name="data">Original-node selector.</param><param name="members">Ordered member types.</param><param name="expected">Selected detached family.</param>
    [Theory][InlineData(false,"xs:integer xs:string",NativeScalarKind.Integer)][InlineData(true,"xs:integer xs:string",NativeScalarKind.Integer)][InlineData(false,"xs:string xs:integer",NativeScalarKind.String)][InlineData(true,"xs:string xs:integer",NativeScalarKind.String)]
    public void UnionOrder(bool data,string members,NativeScalarKind expected)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='U'><xs:union memberTypes='"+members+"'/></xs:simpleType><xs:element name='r' type='U'/></xs:schema>";
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;using var d=s.Assess("<r>1</r>",Context).Value!;Assert.Equal(expected,d.ReadElement([]).Kind);Assert.Equal(expected,Assert.Single(d.ReadElementItems([])).Kind);}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;using var d=s.Assess("<r>1</r>",Context).Value!;Assert.True(d.HasRootScalar);Assert.Equal(expected,d.ReadRootScalar().Kind);Assert.Equal(expected,Assert.Single(d.ReadRootItems()).Kind);}
    }
    /// <summary>Checks selected list members retain their explicit list boundary.</summary>
    /// <param name="data">Original-node selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void UnionOfList(bool data)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='L'><xs:list itemType='xs:integer'/></xs:simpleType><xs:simpleType name='U'><xs:union memberTypes='L xs:string'/></xs:simpleType><xs:element name='r' type='U'/></xs:schema>";
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;using var d=s.Assess("<r>1 2</r>",Context).Value!;Assert.Throws<NotSupportedException>(()=>d.ReadElement([]));Assert.Equal(2,d.ReadElementItems([]).Length);}
        else{using var s=NativeSchemaSession.Create(schema,Options).Value!;using var d=s.Assess("<r>1 2</r>",Context).Value!;Assert.Throws<NotSupportedException>(()=>d.ReadRootScalar());Assert.Equal(2,d.ReadRootItems().Length);}
    }
}
