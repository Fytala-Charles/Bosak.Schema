// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Verifies integer enumeration, authority, lifetime and original fixtures.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Qualify placement and pin original baseline inputs |
//                      | Charles Korthout | 0.3   | 24-11-2026     | Document explicit original fixture identity in probes |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace EnumProbe;
/// <summary>Checks enumeration refusal, original fixtures, authority and lifetime.</summary>
public sealed class EnumBoundaryTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,1,256,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='N'><xs:restriction base='xs:integer'><xs:enumeration value='0'/><xs:enumeration value='1'/></xs:restriction></xs:simpleType><xs:element name='r' type='N'/></xs:schema>";
    /// <summary>Checks cancellation, membership recovery and detached lifetime.</summary><param name="data">Facade.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void CancelRecoveryLifetime(bool data)
    {
        using var c=new CancellationTokenSource();c.Cancel();NativeScalarValue detached;
        if(data){Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaNodeDataSession.Create(Schema,Options,c.Token));using var s=NativeSchemaNodeDataSession.Create(Schema,Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r>1</r>",Context,c.Token));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>2</r>",Context).Outcome);var d=s.Assess("<r>+001</r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadElement([],c.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(c.Token));detached=d.ReadElement([]);Assert.Throws<InvalidOperationException>(()=>s.Assess("<r>0</r>",Context));var xml=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadElement([]));using var again=s.Assess(xml,Context).Value!;Assert.Equal(1,again.ReadElement([]).IntegerValue);s.Dispose();Assert.Throws<ObjectDisposedException>(()=>again.ReadElement([]));}
        else{Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaSession.Create(Schema,Options,c.Token));using var s=NativeSchemaSession.Create(Schema,Options).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>s.Assess("<r>1</r>",Context,c.Token));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>2</r>",Context).Outcome);var d=s.Assess("<r>+001</r>",Context).Value!;Assert.ThrowsAny<OperationCanceledException>(()=>d.ReadRootScalar(c.Token));Assert.ThrowsAny<OperationCanceledException>(()=>d.ExportXml(c.Token));detached=d.ReadRootScalar();Assert.Throws<InvalidOperationException>(()=>s.Assess("<r>0</r>",Context));var xml=d.ExportXml();d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootScalar());using var again=s.Assess(xml,Context).Value!;Assert.Equal(1,again.ReadRootScalar().IntegerValue);s.Dispose();Assert.Throws<ObjectDisposedException>(()=>again.ReadRootScalar());}
        Assert.Equal(1,detached.IntegerValue);
    }
    /// <summary>Invalid membership publishes no facts; foreign authority and disposal remain enforced.</summary>
    [Fact]
    public void FactsAuthorityAndFailure()
    {
        using(var bad=new Fixture(Schema,"<r>2</r>")){Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);Assert.Empty(bad.Assessment.Facts);Assert.Null(bad.Assessment.Evidence);}
        using var a=new Fixture(Schema,"<r>1</r>");using var b=new Fixture(Schema,"<r>1</r>");var node=a.Input.Root;var fact=a.Input.FactsFor((NativeSessionNode)node)!;Assert.False(b.Session.Advisor!.OwnsNode(node,fact,b.Input,b.Assessment.Origin));Assert.Throws<ArgumentException>(()=>b.Run(op=>b.Evaluate(op,node)));Assert.Equal(SchemaResultKind.Unsupported,a.Provider.GetComponentClosure(fact.Type,default).Kind);a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));
    }
    /// <summary>Legacy decimal routes do not widen.</summary>
    [Fact]
    public void LegacyRefuses(){var c=NativeDecimalSession.Create(Schema,new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);}
    /// <summary>Gets original metadata expectations through both facades.</summary><returns>Original inputs.</returns>
    public static IEnumerable<object[]> Originals(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"originals.json")));foreach(var r in d.RootElement.EnumerateArray())foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];}
    /// <summary>Original XML and upstream validity are preserved.</summary><param name="id">Original ID.</param><param name="json">Original record.</param><param name="data">Facade.</param>
    [Theory][MemberData(nameof(Originals))]
    public void OriginalValidity(string id,string json,bool data)
    {
        Assert.StartsWith("MS-DataTypes",id);using var doc=JsonDocument.Parse(json);var r=doc.RootElement;string schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,r.GetProperty("schema").GetString()!)),xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,r.GetProperty("instance").GetString()!));var expected=r.GetProperty("expected").GetString()=="valid"?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Invalid;
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(expected,a.Outcome);using var d=a.Value;if(d is not null){Assert.Equal(0,d.ReadElement([0]).IntegerValue);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}else Assert.Equal(NativeSchemaOutcome.Invalid,expected);}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(expected,a.Outcome);using var d=a.Value;if(d is not null){Assert.Equal(0,d.ReadScalar(0).IntegerValue);Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}else Assert.Equal(NativeSchemaOutcome.Invalid,expected);}
    }
}
