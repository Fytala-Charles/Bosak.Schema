// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies original empty-owner and attribute authority, isolation and expiry.
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
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.EmptyOwners;
/// <summary>Verifies actual empty complex and attribute authority with original Core facts.</summary>
public sealed class NativeEmptyOwnerProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(bool nested=false,bool named=false)
    {
        var definition="<xs:complexType"+(named?" name='E'":"")+"><xs:attribute name='id' type='xs:ID'/><xs:attribute name='refs' type='xs:IDREFS'/></xs:complexType>";
        var e=named?"<xs:element name='e' type='E'/>":"<xs:element name='e'>"+definition+"</xs:element>";
        var r=nested?"<xs:element name='r'><xs:complexType><xs:sequence>"+e+"</xs:sequence></xs:complexType></xs:element>":named?"<xs:element name='r' type='E'/>":"<xs:element name='r'>"+definition+"</xs:element>";
        return "<xs:schema xmlns:xs='"+X+"'>"+(named?definition:"")+r+"</xs:schema>";
    }
    /// <summary>Empty owners keep actual governing handles, zero-atomic values and independent attribute facts.</summary>
    /// <param name="nested">Nested owner selector.</param><param name="named">Named type selector.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void OriginalAuthorityAndExpiry(bool nested,bool named)
    {
        var xml=nested?"<r><e id='v' refs='v v'><!--keep--><?p v?></e></r>":"<r id='v' refs='v v'><!--keep--><?p v?></r>";
        using var f=new Fixture(Schema(nested,named),xml,atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=(NativeSessionNode)f.Element(nested?[0]:[]);var fact=f.Input.FactsFor(node)!;
        Assert.True(fact.NativeType.EmptyContent);Assert.False(fact.HasNoTypedValue);Assert.True(node.IsComplexType);Assert.Equal(named?"E":null,fact.NativeType.Name);Assert.Same(f.Provider.TypeComponent(fact.NativeType),fact.Type);
        var raw=fact.ReadTypedValue(default).Value;Assert.False(raw.IsList);Assert.Empty(raw.AtomicItems);Assert.Same(fact.Type,raw.GoverningType);Assert.True(f.Session.Advisor!.OwnsTypedValue(raw,f.Input,f.Assessment.Origin));Assert.True(f.Session.Advisor.OwnsNode(node,fact,f.Input,f.Assessment.Origin));Assert.Equal(XdmValueKind.Undefined,f.Run(op=>f.Evaluate(op,node)).Kind);
        var attr=(NativeSessionNode)f.Attribute(nested?[0]:[],"","id");var af=f.Input.FactsFor(attr)!;Assert.Equal("ID",af.NativeType.Name);var item=Assert.Single(af.ReadTypedValue(default).Value.AtomicItems);Assert.Same(af.Type,item.TypeEvidence.ActualType);Assert.Equal("v",f.Read(attr).StringValue);
        if(nested){var outer=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(outer.HasNoTypedValue);Assert.Equal("FOTY0012",outer.ReadTypedValue(default).DiagnosticCode);}
        f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Throws<ObjectDisposedException>(()=>af.ReadTypedValue(default));
    }
    /// <summary>IDREFS attributes on empty owners preserve actual ordered member handles and cursor lifetime.</summary>
    [Fact]
    public void OriginalListAuthority()
    {
        using var f=new Fixture(Schema(),"<r id='v' refs='v v'/>",atoms:16);var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","refs"))!;var raw=fact.ReadTypedValue(default).Value;Assert.True(raw.IsList);var values=raw.AtomicItems.ToArray();Assert.Equal(2,values.Length);Assert.All(values,x=>{Assert.Same(f.Provider.TypeComponent(fact.NativeType.ListItemType!),x.TypeEvidence.ActualType);Assert.Equal("v",x.Value.StringValue);});var cursor=raw.AtomicItems.GetEnumerator();f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>cursor.MoveNext());
    }
    /// <summary>Invalid empty content and late unresolved identity references publish no facts.</summary>
    /// <param name="xml">Original invalid input.</param>
    [Theory][InlineData("<r id='v'> </r>")][InlineData("<r refs='missing'/>")]
    public void FailurePublishesNothing(string xml)
    {
        using var f=new Fixture(Schema(),xml,atoms:16);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);Assert.Null(f.Snapshot);
    }
    /// <summary>Failure, cancellation and disposal free admissions without cross-document binding leaks.</summary>
    [Fact]
    public void RecoveryAndCancellation()
    {
        using var s=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,16,256,256)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);using var first=s.Assess("<r id='a'/>",context).Value!;var bad=s.Assess("<r refs='a'/>",context);Assert.Equal(NativeSchemaOutcome.Invalid,bad.Outcome);Assert.Null(bad.Value);using var second=s.Assess("<r id='b' refs='b'/>",context).Value!;Assert.Equal("a",first.ReadAttribute(0).Value.StringValue);Assert.Equal("b",second.ReadAttribute(0).Value.StringValue);using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>s.Assess("<r/>",context,c.Token));
    }
}
