// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Tests bounded exact integer assessment through the public owned scalar facade.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2 | 26-11-2026     | Reconcile original markup admissions; preserve source payloads |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Exercises exact integer lexical, capacity and detached lifetime boundaries.</summary>
public sealed class NativeIntegerConsumerTests
{
    /// <summary>Gets stable authored integer cases.</summary>
    /// <returns>Case identity and payload pairs.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var input=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","Integers","m2c3-cases.json")));
        foreach(var row in input.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];
    }
    private static string Text(NativeScalarValue value)=>value.Kind switch
    {
        NativeScalarKind.String=>value.StringValue,
        NativeScalarKind.Decimal=>value.DecimalValue.ToInvariantString(256),
        NativeScalarKind.Boolean=>value.BooleanValue?"true":"false",
        NativeScalarKind.Integer=>value.IntegerValue.ToString(CultureInfo.InvariantCulture),
        _=>throw new NotSupportedException()
    };
    /// <summary>Checks exact values, complete failures, lexical export/reopen and detached lifetime.</summary>
    /// <param name="id">Stable authored case ID.</param>
    /// <param name="payload">Schema, instance and exact expectations.</param>
    [Theory][MemberData(nameof(Cases))]
    public void IntegerAssessment_SeparatesValidityCapacityAndProfile(string id,string payload)
    {
        Assert.StartsWith("M2C3-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;row=NativeMarkupLegacyVerification.Reconcile(row,nameof(NativeIntegerConsumerTests));
        var options=new NativeSchemaSessionOptions(row.GetProperty("maxNativeCoefficientDigits").GetInt32(),256,256,65536,1,8,1,256,256);
        var created=NativeSchemaSession.Create(row.GetProperty("schema").GetString()!,options);
        Assert.Equal(row.GetProperty("expectedCompile").GetString(),created.Outcome.ToString());
        if(created.Outcome!=NativeSchemaOutcome.Valid)
        {
            Assert.Null(created.Value);Assert.Equal("BSK-NATIVE-SCHEMA-UNSUPPORTED",Assert.Single(created.Diagnostics).Code);
            Assert.Equal(row.GetProperty("unsupportedReason").GetString(),created.Diagnostics[0].UnsupportedReason.ToString());return;
        }
        using var s=created.Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);var xml=row.GetProperty("instance").GetString()!;
        var assessed=s.Assess(xml,context);Assert.Equal(row.GetProperty("expectedAssessment").GetString(),assessed.Outcome.ToString());
        if(assessed.Outcome!=NativeSchemaOutcome.Valid)
        {
            Assert.Null(assessed.Value);Assert.Single(assessed.Diagnostics);
            Assert.Equal("BSK-NATIVE-INSTANCE-"+assessed.Outcome.ToString().ToUpperInvariant(),assessed.Diagnostics[0].Code);
            if(assessed.Outcome==NativeSchemaOutcome.Unsupported)Assert.Equal(row.GetProperty("unsupportedReason").GetString(),assessed.Diagnostics[0].UnsupportedReason.ToString());return;
        }
        using var doc=assessed.Value!;var values=row.GetProperty("values").EnumerateArray().ToArray();Assert.Equal(values.Length,doc.ScalarCount);var detached=new List<NativeScalarValue>();
        for(int i=0;i<values.Length;i++)
        {
            var v=doc.ReadScalar(i);detached.Add(v);Assert.Equal(values[i].GetProperty("kind").GetString(),v.Kind.ToString());Assert.Equal(values[i].GetProperty("value").GetString(),Text(v));
            if(v.Kind==NativeScalarKind.Integer)
            {
                Assert.Equal(BigInteger.Parse(values[i].GetProperty("value").GetString()!,CultureInfo.InvariantCulture),v.IntegerValue);
                Assert.Throws<InvalidOperationException>(()=>v.StringValue);Assert.Throws<InvalidOperationException>(()=>v.DecimalValue);Assert.Throws<InvalidOperationException>(()=>v.BooleanValue);
            }
            else Assert.Throws<InvalidOperationException>(()=>v.IntegerValue);
        }
        var exported=doc.ExportXml();Assert.True(XNode.DeepEquals(XDocument.Parse(xml,LoadOptions.PreserveWhitespace).Root,XDocument.Parse(exported,LoadOptions.PreserveWhitespace).Root));
        doc.Dispose();using var reopened=s.Assess(exported,context).Value!;Assert.Equal(values.Length,reopened.ScalarCount);
        for(int i=0;i<values.Length;i++)Assert.Equal(Text(detached[i]),Text(reopened.ReadScalar(i)));
        s.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.ReadScalar(0));foreach(var v in detached)Assert.NotNull(Text(v));
    }

    /// <summary>Legacy decimal refusal and separate string/decimal scale limits remain stable.</summary>
    [Fact]
    public void LegacyDecimalAndKinds_RemainStable()
    {
        const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='n' type='xs:integer'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        var old=NativeDecimalSession.Create(schema,new(256,256,1,4,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,old.Outcome);Assert.Null(old.Value);
        Assert.Equal(1,(int)NativeScalarKind.String);Assert.Equal(2,(int)NativeScalarKind.Decimal);Assert.Equal(3,(int)NativeScalarKind.Boolean);Assert.Equal(4,(int)NativeScalarKind.Integer);
        using var s=NativeSchemaSession.Create(schema,new(256,0,1,65536,1,4,1,256,0)).Value!;using var d=s.Assess("<r><n>-9223372036854775809</n></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;
        Assert.Equal(BigInteger.Parse("-9223372036854775809",CultureInfo.InvariantCulture),d.ReadScalar(0).IntegerValue);
    }
    /// <summary>Read capacity refusal leaves the document usable for lexical export and subsequent small values.</summary>
    [Fact]
    public void QueryDigitCapacity_RefusesLargeReadWithoutPoisoningDocument()
    {
        const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='n' type='xs:integer' maxOccurs='2'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        using var s=NativeSchemaSession.Create(schema,new(256,0,1,65536,1,4,1,2,0)).Value!;
        using var d=s.Assess("<r><n>123</n><n>1</n></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;
        Assert.Throws<NotSupportedException>(()=>d.ReadScalar(0));Assert.Contains("123",d.ExportXml());Assert.Equal(BigInteger.One,d.ReadScalar(1).IntegerValue);
    }

}
