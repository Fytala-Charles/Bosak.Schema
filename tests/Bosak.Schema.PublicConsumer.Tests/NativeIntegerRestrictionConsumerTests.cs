// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Qualifies exact integer restrictions through public owned scalar sessions.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks exact integer facet outcomes, original attribute uses and detached scalar values.</summary>
public sealed class NativeIntegerRestrictionConsumerTests
{
    private static NativeSchemaSessionOptions Options(int digits=256,int query=256,int export=65536)=>new(digits,0,1024,export,1,32,1,query,0);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='Count'><xs:restriction base='xs:integer'><xs:totalDigits value='3'/></xs:restriction></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='Count' maxOccurs='2'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private static string Text(NativeScalarValue value){Assert.Equal(NativeScalarKind.Integer,value.Kind);return value.IntegerValue.ToString(CultureInfo.InvariantCulture);}
    /// <summary>Gets stable authored integer restriction cases with normative section references.</summary>
    /// <returns>Case identity and exact payload pairs.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var data=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","IntegerRestrictions","cases.json")));
        foreach(var row in data.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];
    }
    /// <summary>Separates component refusal, capacity and instance validity while preserving original XML.</summary>
    /// <param name="id">Authored case identity.</param>
    /// <param name="payload">Schema, XML, capacity and exact expectations.</param>
    [Theory][MemberData(nameof(Cases))]
    public void IntegerRestrictions_ValidateExactOriginalValues(string id,string payload)
    {
        Assert.StartsWith("IR-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;
        var created=NativeSchemaSession.Create(row.GetProperty("schema").GetString()!,Options(row.GetProperty("digits").GetInt32()));Assert.Equal(row.GetProperty("expectedCompile").GetString(),created.Outcome.ToString());
        if(created.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(created.Value);if(row.GetProperty("reason").ValueKind==JsonValueKind.String)Assert.Equal(row.GetProperty("reason").GetString(),created.Diagnostics[0].UnsupportedReason.ToString());return;}
        using var session=created.Value!;var xml=row.GetProperty("instance").GetString()!;var result=session.Assess(xml,Context);Assert.Equal(row.GetProperty("expectedAssessment").GetString(),result.Outcome.ToString());
        if(result.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(result.Value);if(result.Outcome==NativeSchemaOutcome.Unsupported && row.GetProperty("reason").ValueKind==JsonValueKind.String)Assert.Equal(row.GetProperty("reason").GetString(),result.Diagnostics[0].UnsupportedReason.ToString());return;}
        using var document=result.Value!;var expected=row.GetProperty("values").EnumerateArray().Select(x=>x.GetString()).ToArray();Assert.Equal(expected.Length,document.ScalarCount);var detached=new List<BigInteger>();
        for(var i=0;i<expected.Length;i++){var value=document.ReadScalar(i);Assert.Equal(expected[i],Text(value));detached.Add(value.IntegerValue);}
        var attrs=row.GetProperty("attributes").EnumerateArray().Select(x=>x.GetString()).ToArray();Assert.Equal(attrs.Length,document.AttributeCount);for(var i=0;i<attrs.Length;i++)Assert.Equal(attrs[i],Text(document.ReadAttribute(i).Value));
        var desc=row.GetProperty("descendantAttributes").EnumerateArray().Select(x=>x.GetString()).ToArray();Assert.Equal(desc.Length,document.DescendantAttributeCount);for(var i=0;i<desc.Length;i++)Assert.Equal(desc[i],Text(document.ReadDescendantAttribute(i).Value));
        var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));document.Dispose();using var reopened=session.Assess(exported,Context).Value!;for(var i=0;i<detached.Count;i++)Assert.Equal(detached[i],reopened.ReadScalar(i).IntegerValue);
    }
    /// <summary>Both named and anonymous restrictions retain legacy decimal refusal.</summary>
    /// <param name="anonymous">Selects a local anonymous restriction.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void LegacyFacade_DoesNotAdmitIntegerRestriction(bool anonymous)
    {
        var schema=anonymous?"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v'><xs:simpleType><xs:restriction base='xs:integer'/></xs:simpleType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>":Schema;
        var result=NativeDecimalSession.Create(schema,new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);
    }
    /// <summary>Narrow query capacity does not corrupt larger assessed integers or later reads.</summary>
    [Fact]
    public void QueryCapacity_KeepsExportAndSmallValuesUsable()
    {
        using var session=NativeSchemaSession.Create(Schema,Options(query:2)).Value!;using var document=session.Assess("<r><v>123</v><v>1</v></r>",Context).Value!;
        Assert.Throws<NotSupportedException>(()=>document.ReadScalar(0));Assert.Contains("123",document.ExportXml());Assert.Equal(BigInteger.One,document.ReadScalar(1).IntegerValue);
    }
    /// <summary>Cancellation and failed export preserve original scalar reads and lifetime rules.</summary>
    [Fact]
    public void CancellationAndDisposal_PreserveRecovery()
    {
        using var session=NativeSchemaSession.Create(Schema,Options(export:10)).Value!;using var cancelled=new CancellationTokenSource();cancelled.Cancel();Assert.Throws<OperationCanceledException>(()=>session.Assess("<r><v>1</v></r>",Context,cancelled.Token));
        var document=session.Assess("<r><v>1</v></r>",Context).Value!;Assert.Throws<NotSupportedException>(()=>document.ExportXml());Assert.Equal(BigInteger.One,document.ReadScalar(0).IntegerValue);document.Dispose();Assert.Throws<ObjectDisposedException>(()=>document.ReadScalar(0));using var recovered=session.Assess("<r><v>2</v></r>",Context).Value!;Assert.Equal(new BigInteger(2),recovered.ReadScalar(0).IntegerValue);
    }
}
