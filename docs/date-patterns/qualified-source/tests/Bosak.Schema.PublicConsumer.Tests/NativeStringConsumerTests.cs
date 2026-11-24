// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Tests opt-in strings and decimals through the public owned scalar facade.
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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Exercises authored string/decimal cases through producer-only public results.</summary>
public sealed class NativeStringConsumerTests
{
    /// <summary>Gets stable authored cases with explicit semantic outcomes and capacities.</summary>
    /// <returns>Identity and JSON payload pairs.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var input=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","Strings","m2c1-cases.json")));
        foreach(var row in input.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];
    }
    /// <summary>Checks complete outcomes, mixed ordering, detached values, export/reopen and closure.</summary>
    /// <param name="id">Stable authored case ID.</param>
    /// <param name="payload">Schema, instance and exact expectations.</param>
    [Theory][MemberData(nameof(Cases))]
    public void MixedAssessment_MapsCompleteOutcomeAndDetachedValues(string id,string payload)
    {
        Assert.StartsWith("M2C1-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;
        var options=new NativeSchemaSessionOptions(256,256,row.GetProperty("maxStringCharacters").GetInt32(),524288,1,8,1,256,256);
        var created=NativeSchemaSession.Create(row.GetProperty("schema").GetString()!,options);
        Assert.Equal(row.GetProperty("expectedCompile").GetString(),created.Outcome.ToString());
        if(created.Outcome!=NativeSchemaOutcome.Valid)
        {
            Assert.Null(created.Value);Assert.Equal("BSK-NATIVE-SCHEMA-UNSUPPORTED",Assert.Single(created.Diagnostics).Code);
            Assert.Equal(row.GetProperty("unsupportedReason").GetString(),created.Diagnostics[0].UnsupportedReason.ToString());return;
        }
        using var session=created.Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
        var xml=row.GetProperty("instance").GetString()!;var assessed=session.Assess(xml,context);
        Assert.Equal(row.GetProperty("expectedAssessment").GetString(),assessed.Outcome.ToString());
        if(assessed.Outcome!=NativeSchemaOutcome.Valid)
        {
            Assert.Null(assessed.Value);Assert.Single(assessed.Diagnostics);
            Assert.Equal("BSK-NATIVE-INSTANCE-"+assessed.Outcome.ToString().ToUpperInvariant(),assessed.Diagnostics[0].Code);
            if(assessed.Outcome==NativeSchemaOutcome.Unsupported)Assert.Equal(row.GetProperty("unsupportedReason").GetString(),assessed.Diagnostics[0].UnsupportedReason.ToString());return;
        }
        Assert.Empty(assessed.Diagnostics);using var doc=assessed.Value!;var expected=row.GetProperty("values").EnumerateArray().ToArray();
        Assert.Equal(expected.Length,doc.ScalarCount);var detached=new List<NativeScalarValue>();
        for(int i=0;i<expected.Length;i++)
        {
            var value=doc.ReadScalar(i);detached.Add(value);Assert.Equal(expected[i].GetProperty("kind").GetString(),value.Kind.ToString());
            var text=value.Kind==NativeScalarKind.String?value.StringValue:value.DecimalValue.ToInvariantString(256);
            Assert.Equal(expected[i].GetProperty("value").GetString(),text);
            if(value.Kind==NativeScalarKind.String)Assert.Throws<InvalidOperationException>(()=>value.DecimalValue);
            else Assert.Throws<InvalidOperationException>(()=>value.StringValue);
        }
        var export=doc.ExportXml();Assert.True(XNode.DeepEquals(XDocument.Parse(xml,LoadOptions.PreserveWhitespace).Root,XDocument.Parse(export,LoadOptions.PreserveWhitespace).Root));
        doc.Dispose();using var reopened=session.Assess(export,context).Value!;
        Assert.Equal(expected.Length,reopened.ScalarCount);
        for(int i=0;i<expected.Length;i++)
        {
            var value=reopened.ReadScalar(i);Assert.Equal(detached[i].Kind,value.Kind);
            Assert.Equal(expected[i].GetProperty("value").GetString(),value.Kind==NativeScalarKind.String?value.StringValue:value.DecimalValue.ToInvariantString(256));
        }
        session.Dispose();Assert.Throws<ObjectDisposedException>(()=>reopened.ScalarCount);
        foreach(var value in detached)Assert.NotNull(value.Kind==NativeScalarKind.String?value.StringValue:value.DecimalValue.ToInvariantString(256));
    }
}
