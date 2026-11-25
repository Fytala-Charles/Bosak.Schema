// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Tests bounded Boolean assessment through the public owned scalar facade.
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
//                      | Charles Korthout | 0.2   | 24-11-2026     | Reconcile measured direct Boolean aliases without changing original payloads |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Exercises four Boolean lexical forms, invalid/refusal boundaries and detached true/false values.</summary>
public sealed class NativeBooleanConsumerTests
{
    /// <summary>Gets authored Boolean cases with immutable IDs and explicit expectations.</summary>
    /// <returns>Case identity/payload pairs.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var input=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","Booleans","m2c2-cases.json")));
        foreach(var row in input.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];
    }
    private static string Text(NativeScalarValue value)=>value.Kind switch
    {
        NativeScalarKind.String=>value.StringValue,
        NativeScalarKind.Decimal=>value.DecimalValue.ToInvariantString(256),
        NativeScalarKind.Boolean=>value.BooleanValue?"true":"false",
        _=>throw new NotSupportedException()
    };
    /// <summary>Checks complete outcomes, exact true/false, lexical export/reopen and detached lifetime.</summary>
    /// <param name="id">Stable authored case ID.</param>
    /// <param name="payload">Schema, instance and expected values.</param>
    [Theory][MemberData(nameof(Cases))]
    public void BooleanAssessment_SeparatesValueValidityAndProfileRefusal(string id,string payload)
    {
        Assert.StartsWith("M2C2-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;
        var options=new NativeSchemaSessionOptions(256,256,row.GetProperty("maxStringCharacters").GetInt32(),65536,1,8,1,256,256);
        var created=NativeSchemaSession.Create(row.GetProperty("schema").GetString()!,options);
        Assert.Equal(id is "M2C2-45" or "M2C2-46"?"Valid":row.GetProperty("expectedCompile").GetString(),created.Outcome.ToString());
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
            var v=doc.ReadScalar(i);detached.Add(v);Assert.Equal(values[i].GetProperty("kind").GetString(),v.Kind.ToString());
            if(v.Kind==NativeScalarKind.Boolean)
            {
                Assert.Equal(values[i].GetProperty("value").GetBoolean(),v.BooleanValue);Assert.Throws<InvalidOperationException>(()=>v.StringValue);Assert.Throws<InvalidOperationException>(()=>v.DecimalValue);
            }
            else { Assert.Throws<InvalidOperationException>(()=>v.BooleanValue);Assert.Equal(values[i].GetProperty("value").GetString(),Text(v)); }
        }
        var exported=doc.ExportXml();Assert.True(XNode.DeepEquals(XDocument.Parse(xml,LoadOptions.PreserveWhitespace).Root,XDocument.Parse(exported,LoadOptions.PreserveWhitespace).Root));
        doc.Dispose();using var reopened=s.Assess(exported,context).Value!;Assert.Equal(values.Length,reopened.ScalarCount);
        for(int i=0;i<values.Length;i++)Assert.Equal(Text(detached[i]),Text(reopened.ReadScalar(i)));
        s.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.ReadScalar(0));foreach(var v in detached)Assert.NotNull(Text(v));
    }

    /// <summary>Old decimal APIs refuse Boolean schemas and prior enum values do not change.</summary>
    [Fact]
    public void LegacyDecimalAndExistingKinds_RemainStable()
    {
        const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='b' type='xs:boolean'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        var old=NativeDecimalSession.Create(schema,new(256,256,1,4,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,old.Outcome);Assert.Null(old.Value);
        Assert.Equal(1,(int)NativeScalarKind.String);Assert.Equal(2,(int)NativeScalarKind.Decimal);Assert.Equal(3,(int)NativeScalarKind.Boolean);
        using var s=NativeSchemaSession.Create(schema,new(256,256,1,65536,1,4,1,1,0)).Value!;using var d=s.Assess("<r><b>false</b></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;
        Assert.Equal(1,d.ScalarCount);Assert.False(d.ReadScalar(0).BooleanValue);
    }
}
