// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Tests IEEE item cancellation, cache limits and owner lifetime.
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
using System.Collections.Immutable;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks compound capacity and lifetime boundaries with real native Core sessions.</summary>
public sealed class NativeNumericUriLifetimeConsumerTests
{
    private static NativeSchemaSessionOptions Options(int atoms=16,int cache=256)=>new(256,256,256,65536,1,cache,atoms,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private const string ListSchema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='L'><xs:list itemType='xs:double'/></xs:simpleType><xs:element name='r' type='L' nillable='true'/></xs:schema>";
    /// <summary>Checks cancellation, revocation, recovery and detached immutable items.</summary>
    /// <param name="data">Original-node selector.</param><param name="union">Atomic union selector.</param>
    [Theory][InlineData(false,false)][InlineData(true,false)][InlineData(false,true)][InlineData(true,true)]
    public void Lifetime(bool data,bool union)
    {
        var schema=union?ListSchema.Replace("<xs:list itemType='xs:double'/>","<xs:union memberTypes='xs:double xs:boolean'/>"):ListSchema;
        var xml=union?"<r>1</r>":"<r>1 2</r>";using var cancel=new CancellationTokenSource();cancel.Cancel();var token=cancel.Token;ImmutableArray<NativeScalarValue> detached;
        if(data)
        {
            Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaNodeDataSession.Create(schema,Options(),token));
            using var session=NativeSchemaNodeDataSession.Create(schema,Options()).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>session.Assess(xml,Context,token));
            using var document=session.Assess(xml,Context).Value!;Assert.Throws<InvalidOperationException>(()=>session.Assess(xml,Context));
            Assert.ThrowsAny<OperationCanceledException>(()=>document.ReadElementItems([],token));Assert.ThrowsAny<OperationCanceledException>(()=>document.ExportXml(token));detached=document.ReadElementItems([]);
            document.Dispose();Assert.Throws<ObjectDisposedException>(()=>document.ReadElementItems([]));Assert.Throws<ObjectDisposedException>(()=>document.ExportXml());
            Assert.Equal(NativeSchemaOutcome.Invalid,session.Assess("<r>!invalid!</r>",Context).Outcome);using var recovered=session.Assess(xml,Context).Value!;Assert.Equal(detached.Length,recovered.ReadElementItems([]).Length);session.Dispose();Assert.Throws<ObjectDisposedException>(()=>recovered.ReadElementItems([]));
        }
        else
        {
            Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaSession.Create(schema,Options(),token));
            using var session=NativeSchemaSession.Create(schema,Options()).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>session.Assess(xml,Context,token));
            using var document=session.Assess(xml,Context).Value!;Assert.Throws<InvalidOperationException>(()=>session.Assess(xml,Context));
            Assert.ThrowsAny<OperationCanceledException>(()=>document.ReadRootItems(token));Assert.ThrowsAny<OperationCanceledException>(()=>document.ExportXml(token));detached=document.ReadRootItems();
            document.Dispose();Assert.Throws<ObjectDisposedException>(()=>document.ReadRootItems());Assert.Throws<ObjectDisposedException>(()=>document.ExportXml());
            Assert.Equal(NativeSchemaOutcome.Invalid,session.Assess("<r>!invalid!</r>",Context).Outcome);using var recovered=session.Assess(xml,Context).Value!;Assert.Equal(detached.Length,recovered.ReadRootItems().Length);session.Dispose();Assert.Throws<ObjectDisposedException>(()=>recovered.ReadRootItems());
        }
        Assert.Equal(union?1:2,detached.Length);Assert.Equal(1d,detached[0].DoubleValue);
    }
    /// <summary>Checks query atom and cache limits fail inside the operation without poisoning the owner.</summary>
    /// <param name="data">Original-node selector.</param><param name="atoms">Query atom limit.</param><param name="cache">Cache cell limit.</param>
    [Theory][InlineData(false,1,256)][InlineData(true,1,256)][InlineData(false,16,1)][InlineData(true,16,1)]
    public void QueryCapacity(bool data,int atoms,int cache)
    {
        if(data){using var s=NativeSchemaNodeDataSession.Create(ListSchema,Options(atoms,cache)).Value!;using var d=s.Assess("<r>1 2</r>",Context).Value!;Assert.Throws<NotSupportedException>(()=>d.ReadElementItems([]));Assert.NotEmpty(d.ExportXml());d.Dispose();using var ok=s.Assess("<r>1</r>",Context).Value!;Assert.Single(ok.ReadElementItems([]));}
        else{using var s=NativeSchemaSession.Create(ListSchema,Options(atoms,cache)).Value!;using var d=s.Assess("<r>1 2</r>",Context).Value!;Assert.Throws<NotSupportedException>(()=>d.ReadRootItems());Assert.NotEmpty(d.ExportXml());d.Dispose();using var ok=s.Assess("<r>1</r>",Context).Value!;Assert.Single(ok.ReadRootItems());}
    }
    /// <summary>Checks equal empty item arrays do not erase the nil/list distinction.</summary>
    /// <param name="data">Original-node selector.</param><param name="nil">Actual nil selector.</param>
    [Theory][InlineData(false,false)][InlineData(true,false)][InlineData(false,true)][InlineData(true,true)]
    public void NilAndEmpty(bool data,bool nil)
    {
        var xml=nil?"<r xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>":"<r/>";
        if(data){using var s=NativeSchemaNodeDataSession.Create(ListSchema,Options()).Value!;using var d=s.Assess(xml,Context).Value!;Assert.Equal(nil,d.IsNilled([]));Assert.Empty(d.ReadElementItems([]));Assert.Throws<NotSupportedException>(()=>d.ReadElement([]));}
        else{using var s=NativeSchemaSession.Create(ListSchema,Options()).Value!;using var d=s.Assess(xml,Context).Value!;Assert.Equal(nil,d.IsRootNilled);Assert.False(d.HasRootScalar);Assert.Empty(d.ReadRootItems());Assert.Throws<NotSupportedException>(()=>d.ReadRootScalar());}
    }
    /// <summary>Checks every descendant/attribute item entry point honors cancellation and original paths.</summary>
    /// <param name="data">Original-node selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void PlacementReads(bool data)
    {
        const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='L'><xs:list itemType='xs:double'/></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='L'/><xs:element name='c'><xs:complexType><xs:sequence><xs:element name='w' type='xs:decimal'/></xs:sequence><xs:attribute name='b' type='L' use='required'/></xs:complexType></xs:element></xs:sequence><xs:attribute name='a' type='L' use='required'/></xs:complexType></xs:element></xs:schema>";
        const string xml="<r a='1 2'><v>3 4</v><c b='5 6'><w>0</w></c></r>";using var cancel=new CancellationTokenSource();cancel.Cancel();var token=cancel.Token;
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options()).Value!;using var d=s.Assess(xml,Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadAttributeItems([],"","a",token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadAttributeItems([1],"","b",token));Assert.Equal(1d,d.ReadAttributeItems([],"","a")[0].DoubleValue);Assert.Equal(5d,d.ReadAttributeItems([1],"","b")[0].DoubleValue);Assert.Equal(3d,d.ReadElementItems([0])[0].DoubleValue);Assert.Single(d.ReadElementItems([1,0]));Assert.Throws<NotSupportedException>(()=>d.ReadElementItems([]));}
        else{using var s=NativeSchemaSession.Create(schema,Options()).Value!;using var d=s.Assess(xml,Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadAttributeItems(0,token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadDescendantAttributeItems(0,token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadScalarItems(0,token));Assert.Equal(1d,d.ReadAttributeItems(0)[0].DoubleValue);Assert.Equal(5d,d.ReadDescendantAttributeItems(0)[0].DoubleValue);Assert.Equal(3d,d.ReadScalarItems(0)[0].DoubleValue);Assert.Single(d.ReadScalarItems(1));Assert.Throws<NotSupportedException>(()=>d.ReadRootItems());}
    }
}
