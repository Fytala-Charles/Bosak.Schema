// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies exact binary or QName contracts through both public facades.
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
using Bosak.Schema;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies binary or QName cancellation, detached values and lifetime refusal.</summary>
public sealed class NativeInterchangeLifetimeConsumerTests
{
    private static string Show(NativeScalarValue value) => value.Kind == NativeScalarKind.QName ? value.QNameValue.LocalName : value.Kind == NativeScalarKind.HexBinary ? Convert.ToHexString(value.BinaryValue.AsSpan()) : Convert.ToBase64String(value.BinaryValue.AsSpan());
    private static NativeSchemaSessionOptions Options => new(256,256,256,65536,1,256,1,256,256);
    private static NativeSchemaAssessmentContext Context => new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Gets the seven lifetime contracts for all binary or QName types and both facades.</summary>
    /// <returns>Stable contract IDs, native type and facade.</returns>
    public static IEnumerable<object[]> Cases()
    {
        foreach(int i in Enumerable.Range(91,7)) foreach(string kind in new[]{"hexBinary","base64Binary","QName"}) foreach(bool data in new[]{false,true}) yield return ["CAL-"+i.ToString("D3"),kind,data];
    }
    /// <summary>Exercises the successful operation and its refusal boundary.</summary>
    /// <param name="id">Contract ID.</param><param name="kind">Native binary or QName type.</param><param name="data">Original-node facade.</param>
    [Theory][MemberData(nameof(Cases))]
    public void Contract(string id,string kind,bool data)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:"+kind+"'/></xs:schema>";
        var value=kind=="QName"?"item":kind=="hexBinary"?"61":"YQ==";var xml="<r>"+value+"</r>";
        using var cancellation=new CancellationTokenSource();cancellation.Cancel();var token=cancellation.Token;
        NativeScalarValue detached;
        if(data)
        {
            if(id=="CAL-093")Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaNodeDataSession.Create(schema,Options,token));
            using var session=NativeSchemaNodeDataSession.Create(schema,Options).Value!;
            if(id=="CAL-093")Assert.ThrowsAny<OperationCanceledException>(()=>session.Assess(xml,Context,token));
            using var document=session.Assess(xml,Context).Value!;detached=document.ReadElement([]);
            switch(id)
            {
                case "CAL-091": document.Dispose();break;
                case "CAL-092": Assert.Throws<InvalidOperationException>(()=>detached.DateValue);Assert.Throws<InvalidOperationException>(()=>detached.StringValue);break;
                case "CAL-094": Assert.ThrowsAny<OperationCanceledException>(()=>document.ReadElement([],token));Assert.ThrowsAny<OperationCanceledException>(()=>document.ExportXml(token));Assert.Equal(value,Show(document.ReadElement([])));break;
                case "CAL-095": document.Dispose();Assert.Throws<ObjectDisposedException>(()=>document.ReadElement([]));Assert.Throws<ObjectDisposedException>(()=>document.ExportXml());break;
                case "CAL-096": session.Dispose();Assert.Throws<ObjectDisposedException>(()=>document.ReadElement([]));Assert.Throws<ObjectDisposedException>(()=>session.Assess(xml,Context));break;
                case "CAL-097": Assert.Throws<InvalidOperationException>(()=>session.Assess(xml,Context));document.Dispose();Assert.Equal(NativeSchemaOutcome.Invalid,session.Assess("<r>!invalid!</r>",Context).Outcome);using(var reopened=session.Assess(xml,Context).Value!)Assert.Equal(value,Show(reopened.ReadElement([])));break;
            }
        }
        else
        {
            if(id=="CAL-093")Assert.ThrowsAny<OperationCanceledException>(()=>NativeSchemaSession.Create(schema,Options,token));
            using var session=NativeSchemaSession.Create(schema,Options).Value!;
            if(id=="CAL-093")Assert.ThrowsAny<OperationCanceledException>(()=>session.Assess(xml,Context,token));
            using var document=session.Assess(xml,Context).Value!;detached=document.ReadRootScalar();
            switch(id)
            {
                case "CAL-091": document.Dispose();break;
                case "CAL-092": Assert.Throws<InvalidOperationException>(()=>detached.DateValue);Assert.Throws<InvalidOperationException>(()=>detached.StringValue);break;
                case "CAL-094": Assert.ThrowsAny<OperationCanceledException>(()=>document.ReadRootScalar(token));Assert.ThrowsAny<OperationCanceledException>(()=>document.ExportXml(token));Assert.Equal(value,Show(document.ReadRootScalar()));break;
                case "CAL-095": document.Dispose();Assert.Throws<ObjectDisposedException>(()=>document.ReadRootScalar());Assert.Throws<ObjectDisposedException>(()=>document.ExportXml());break;
                case "CAL-096": session.Dispose();Assert.Throws<ObjectDisposedException>(()=>document.ReadRootScalar());Assert.Throws<ObjectDisposedException>(()=>session.Assess(xml,Context));break;
                case "CAL-097": Assert.Throws<InvalidOperationException>(()=>session.Assess(xml,Context));document.Dispose();Assert.Equal(NativeSchemaOutcome.Invalid,session.Assess("<r>!invalid!</r>",Context).Outcome);using(var reopened=session.Assess(xml,Context).Value!)Assert.Equal(value,Show(reopened.ReadRootScalar()));break;
            }
        }
        Assert.Equal(value,Show(detached));
        using var wrong=NativeSchemaSession.Create(schema.Replace("xs:"+kind+"'","xs:string'",StringComparison.Ordinal),Options).Value!;
        using var text=wrong.Assess("<r>not a binary or QName</r>",Context).Value!;Assert.Throws<InvalidOperationException>(()=>text.ReadRootScalar().CalendarValue);
    }
}
