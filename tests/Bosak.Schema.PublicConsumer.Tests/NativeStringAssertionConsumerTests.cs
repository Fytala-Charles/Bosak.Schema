// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Qualifies bounded string assertions through both public owned session facades.
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
using System.Text.Json;
using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Qualifies string value/general equality, literals, namespaces and bounded refusal.</summary>
public sealed class NativeStringAssertionConsumerTests
{
    private static NativeSchemaSessionOptions Options(int strings=1024)=>new(256,256,strings,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Gets authored clause-mapped inputs for each owned facade.</summary>
    /// <returns>Stable identity, payload and facade selection.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var data=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","StringAssertions","cases.json")));
        foreach(var row in data.RootElement.EnumerateArray())foreach(var dataFacade in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetRawText(),dataFacade];
    }
    /// <summary>Checks complete assessment before admitting original scalar values and detached export.</summary>
    /// <param name="id">Stable authored identity.</param>
    /// <param name="payload">Schema, instance, clauses and exact expectations.</param>
    /// <param name="dataFacade">Selects original-node atomization instead of the scalar facade.</param>
    [Theory][MemberData(nameof(Cases))]
    public void StringAssertions_AssessCompleteOriginalDocument(string id,string payload,bool dataFacade)
    {
        Assert.StartsWith("SA-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;Assert.NotEmpty(row.GetProperty("clauses").EnumerateArray());
        var schema=row.GetProperty("schema").GetString()!;var xml=row.GetProperty("instance").GetString()!;var options=Options(row.GetProperty("maxStringCharacters").GetInt32());
        var expectedCompile=row.GetProperty("expectedCompile").GetString();var expectedAssessment=row.GetProperty("expectedAssessment").GetString();
        IDisposable? session=null;IDisposable? document=null;NativeScalarValue[] values=[];string? exported=null;
        try
        {
            if(dataFacade)
            {
                var created=NativeSchemaNodeDataSession.Create(schema,options);Assert.Equal(expectedCompile,created.Outcome.ToString());
                if(created.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(created.Value);CheckReason(created.Diagnostics,row);return;}
                session=created.Value!;var result=created.Value!.Assess(xml,Context);Assert.Equal(expectedAssessment,result.Outcome.ToString());
                if(result.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(result.Value);if(result.Outcome==NativeSchemaOutcome.Unsupported)CheckReason(result.Diagnostics,row);return;}
                document=result.Value!;values=row.GetProperty("values").EnumerateArray().Select((_,index)=>result.Value!.ReadElement([index])).ToArray();exported=result.Value!.ExportXml();
            }
            else
            {
                var created=NativeSchemaSession.Create(schema,options);Assert.Equal(expectedCompile,created.Outcome.ToString());
                if(created.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(created.Value);CheckReason(created.Diagnostics,row);return;}
                session=created.Value!;var result=created.Value!.Assess(xml,Context);Assert.Equal(expectedAssessment,result.Outcome.ToString());
                if(result.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(result.Value);if(result.Outcome==NativeSchemaOutcome.Unsupported)CheckReason(result.Diagnostics,row);return;}
                document=result.Value!;Assert.Equal(row.GetProperty("values").GetArrayLength(),result.Value!.ScalarCount);values=Enumerable.Range(0,result.Value.ScalarCount).Select(index=>result.Value.ReadScalar(index)).ToArray();exported=result.Value.ExportXml();
            }
            Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported!)));
        }
        finally{document?.Dispose();session?.Dispose();}
        var expected=row.GetProperty("values").EnumerateArray().ToArray();Assert.Equal(expected.Length,values.Length);
        for(var index=0;index<values.Length;index++){Assert.Equal(expected[index].GetProperty("kind").GetString(),values[index].Kind.ToString());Assert.Equal(expected[index].GetProperty("value").GetString(),values[index].Kind==NativeScalarKind.String?values[index].StringValue:values[index].DecimalValue.ToInvariantString(256));}
    }
    private static void CheckReason(System.Collections.Immutable.ImmutableArray<NativeSchemaDiagnostic> diagnostics,JsonElement row)=>Assert.Equal(row.GetProperty("reason").GetString(),Assert.Single(diagnostics).UnsupportedReason.ToString());
    /// <summary>Equality is culture independent and does not case-fold or normalize Unicode.</summary>
    /// <param name="culture">Culture used by the consumer thread.</param>
    [Theory][InlineData("tr-TR")][InlineData("en-US")]
    public void StringEquality_UsesCodepointSemantics(string culture)
    {
        var previous=CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture=CultureInfo.GetCultureInfo(culture);
            const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:string'/></xs:sequence><xs:assert test=\"v eq 'I'\"/></xs:complexType></xs:element></xs:schema>";
            using var session=NativeSchemaSession.Create(schema,Options()).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,session.Assess("<r><v>?</v></r>",Context).Outcome);using var good=session.Assess("<r><v>I</v></r>",Context).Value!;Assert.Equal("I",good.ReadScalar(0).StringValue);
        }
        finally{CultureInfo.CurrentCulture=previous;}
    }
    /// <summary>Failed assertions and cancellation publish no admission and preserve session recovery.</summary>
    /// <param name="nodeData">Selects the fixed original-node facade.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void FailedAssertion_LeavesSessionUsable(bool nodeData)
    {
        const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:string' maxOccurs='2'/></xs:sequence><xs:assert test=\"v eq 'A'\"/></xs:complexType></xs:element></xs:schema>";
        using var cancelled=new CancellationTokenSource();cancelled.Cancel();
        if(nodeData){using var session=NativeSchemaNodeDataSession.Create(schema,Options()).Value!;Assert.Throws<OperationCanceledException>(()=>session.Assess("<r><v>A</v></r>",Context,cancelled.Token));Assert.Null(session.Assess("<r><v>A</v><v>A</v></r>",Context).Value);using var doc=session.Assess("<r><v>A</v></r>",Context).Value!;Assert.Equal("A",doc.ReadElement([0]).StringValue);}
        else{using var session=NativeSchemaSession.Create(schema,Options()).Value!;Assert.Throws<OperationCanceledException>(()=>session.Assess("<r><v>A</v></r>",Context,cancelled.Token));var invalid=session.Assess("<r><v>A</v><v>A</v></r>",Context);Assert.Equal(NativeSchemaOutcome.Invalid,invalid.Outcome);Assert.Null(invalid.Value);using var doc=session.Assess("<r><v>A</v></r>",Context).Value!;Assert.Equal("A",doc.ReadScalar(0).StringValue);}
    }
}
