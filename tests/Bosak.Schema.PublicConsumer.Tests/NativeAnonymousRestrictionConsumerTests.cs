// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Qualifies anonymous local restrictions through public scalar sessions.
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
//                      | Charles Korthout | 0.2   | 20-11-2026     | Project the adopted anonymous integer scalar without changing prior decimal/string cases |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Text.Json;
using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Qualifies authored clause-mapped anonymous restriction cases through public APIs.</summary>
public sealed class NativeAnonymousRestrictionConsumerTests
{
    private static NativeSchemaSessionOptions Options(int strings=1024,int cache=128,int export=65536,int digits=256)=>new(digits,256,strings,export,1,cache,1,256,256);
    private static string Schema(string family="decimal",string facets="")=>$"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v'><xs:simpleType><xs:restriction base='xs:{family}'>{facets}</xs:restriction></xs:simpleType></xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Gets the authored clause-mapped native cases, separately from upstream fixtures.</summary>
    /// <returns>Stable identity and schema/instance/expectation payloads.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var doc=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","AnonymousSimpleRestrictions","cases.json")));
        foreach(var row in doc.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];
    }
    /// <summary>Checks exact compile and assessment outcomes, detached values and preserved XML.</summary>
    /// <param name="id">Authored case identity.</param>
    /// <param name="payload">Clause-mapped case JSON.</param>
    [Theory][MemberData(nameof(Cases))]
    public void AnonymousRestriction_ValidatesOriginalInstance(string id,string payload)
    {
        Assert.StartsWith("AS-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;Assert.NotEmpty(row.GetProperty("clauses").EnumerateArray());
        var created=NativeSchemaSession.Create(row.GetProperty("schema").GetString()!,Options());Assert.Equal(row.GetProperty("expectedCompile").GetString(),created.Outcome.ToString());
        if(created.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(created.Value);if(row.GetProperty("reason").ValueKind==JsonValueKind.String)Assert.Equal(row.GetProperty("reason").GetString(),created.Diagnostics[0].UnsupportedReason.ToString());return;}
        using var session=created.Value!;var xml=row.GetProperty("instance").GetString()!;var assessed=session.Assess(xml,Context);Assert.Equal(row.GetProperty("expectedAssessment").GetString(),assessed.Outcome.ToString());
        if(assessed.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(assessed.Value);return;}
        using var document=assessed.Value!;var expected=row.GetProperty("values").EnumerateArray().Select(v=>v.GetString()).ToArray();Assert.Equal(expected.Length,document.ScalarCount);
        for(var i=0;i<expected.Length;i++){var value=document.ReadScalar(i);Assert.Equal(expected[i],value.Kind==NativeScalarKind.String?value.StringValue:value.Kind==NativeScalarKind.Integer?value.IntegerValue.ToString(System.Globalization.CultureInfo.InvariantCulture):value.DecimalValue.ToInvariantString(256));}
        Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(document.ExportXml())));
    }
    /// <summary>Anonymous restrictions do not widen the legacy decimal facade.</summary>
    /// <param name="family">Restriction family.</param>
    [Theory][InlineData("decimal")][InlineData("string")]
    public void LegacyAnonymousDefinitions_RemainUnsupported(string family)
    {
        var result=NativeDecimalSession.Create(Schema(family),new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);
    }
    /// <summary>Representation limits precede facet checks and a failure leaves the session usable.</summary>
    /// <param name="family">Decimal or string capacity route.</param>
    [Theory][InlineData("decimal")][InlineData("string")]
    public void RepresentationOverflow_DoesNotPublishOrPoison(string family)
    {
        using var session=NativeSchemaSession.Create(Schema(family),Options(strings:2,digits:2)).Value!;
        var result=session.Assess("<r><v>123</v></r>",Context);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,result.Diagnostics[0].UnsupportedReason);
        using var recovered=session.Assess("<r><v>1</v></r>",Context).Value!;Assert.Equal(1,recovered.ScalarCount);
    }
    /// <summary>Cancellation, bounded export and disposal retain the established owner lifetime.</summary>
    [Fact]
    public void CancellationAndExportFailure_PreserveRecovery()
    {
        using var session=NativeSchemaSession.Create(Schema(),Options(export:10)).Value!;using var cancelled=new CancellationTokenSource();cancelled.Cancel();
        Assert.Throws<OperationCanceledException>(()=>session.Assess("<r><v>1</v></r>",Context,cancelled.Token));
        var document=session.Assess("<r><v>1</v></r>",Context).Value!;Assert.Throws<NotSupportedException>(()=>document.ExportXml());Assert.Equal("1",document.ReadScalar(0).DecimalValue.ToInvariantString(256));document.Dispose();Assert.Throws<ObjectDisposedException>(()=>document.ReadScalar(0));
        using var recovered=session.Assess("<r><v>2</v></r>",Context).Value!;Assert.Equal("2",recovered.ReadScalar(0).DecimalValue.ToInvariantString(256));
    }
}
