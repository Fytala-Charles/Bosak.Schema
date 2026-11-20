// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Qualifies public owned original-node reads, selectors, limits and lifetime.
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

using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Qualifies owned original-node data through the public consumer API.</summary>
public sealed class NativeNodeDataConsumerTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal' maxOccurs='2'/></xs:sequence><xs:attribute name='id' type='xs:string'/></xs:complexType></xs:element></xs:schema>";
    private const string Xml="<r id='A'><v>123</v><v>1</v></r>";
    private static NativeSchemaSessionOptions Options(int query=256,int scale=256,int cache=32,int export=65536)=>new(256,256,1024,export,1,cache,1,query,scale);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static string Text(NativeScalarValue value)=>value.Kind switch{NativeScalarKind.String=>value.StringValue,NativeScalarKind.Boolean=>value.BooleanValue?"true":"false",NativeScalarKind.Integer=>value.IntegerValue.ToString(CultureInfo.InvariantCulture),_=>value.DecimalValue.ToInvariantString(256)};
    private static ImmutableArray<int> Path(JsonElement value)=>value.EnumerateArray().Select(v=>v.GetInt32()).ToImmutableArray();
    /// <summary>Gets authored native payloads covering original node owners and scalar families.</summary>
    /// <returns>Case identity and native input payload.</returns>
    public static IEnumerable<object[]> Cases(){using var data=JsonDocument.Parse(File.ReadAllText(System.IO.Path.Combine(AppContext.BaseDirectory,"TestData","NodeData","cases.json")));foreach(var row in data.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];}
    /// <summary>Reads original typed elements and attributes and preserves detached results after disposal.</summary>
    /// <param name="id">Stable authored identity.</param>
    /// <param name="payload">Schema, XML and selected original nodes.</param>
    [Theory][MemberData(nameof(Cases))]
    public void OwnedNodeData_ReadsOriginalScalars(string id,string payload)
    {
        Assert.StartsWith("ND-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;
        var created=NativeSchemaNodeDataSession.Create(row.GetProperty("schema").GetString()!,Options());Assert.Equal(NativeSchemaOutcome.Valid,created.Outcome);using var session=created.Value!;
        var assessed=session.Assess(row.GetProperty("xml").GetString()!,Context);Assert.Equal(NativeSchemaOutcome.Valid,assessed.Outcome);using var document=assessed.Value!;
        var value=document.ReadElement(Path(row.GetProperty("path")));Assert.Equal(row.GetProperty("kind").GetString(),value.Kind.ToString().ToLowerInvariant());Assert.Equal(row.GetProperty("expected").GetString(),Text(value));
        foreach(var attr in row.GetProperty("attrs").EnumerateArray()){var actual=document.ReadAttribute(Path(attr.GetProperty("path")),attr.GetProperty("namespace").GetString()!,attr.GetProperty("name").GetString()!);Assert.Equal(attr.GetProperty("kind").GetString(),actual.Kind.ToString().ToLowerInvariant());Assert.Equal(attr.GetProperty("expected").GetString(),Text(actual));}
        Assert.True(XNode.DeepEquals(XElement.Parse(row.GetProperty("xml").GetString()!),XElement.Parse(document.ExportXml())));
        document.Dispose();session.Dispose();Assert.Equal(row.GetProperty("expected").GetString(),Text(value));
    }
    /// <summary>Element-only atomization refuses with FOTY0012 and leaves subsequent reads usable.</summary>
    [Fact]
    public void ElementOnly_RefusesWithoutPoisoningReads(){using var session=NativeSchemaNodeDataSession.Create(Schema,Options()).Value!;using var doc=session.Assess(Xml,Context).Value!;Assert.Contains("FOTY0012",Assert.Throws<NotSupportedException>(()=>doc.ReadElement([])).Message);Assert.Equal("1",Text(doc.ReadElement([1])));}
    /// <summary>Selector errors report public parameter names and preserve subsequent reads.</summary>
    [Fact]
    public void Selectors_ValidateArguments()
    {
        using var session=NativeSchemaNodeDataSession.Create(Schema,Options()).Value!;using var doc=session.Assess(Xml,Context).Value!;
        Assert.Equal("elementPath",Assert.Throws<ArgumentException>(()=>doc.ReadElement(default)).ParamName);
        foreach(var path in new ImmutableArray<int>[] {[-1],[2],[0,0],[0,0,0,0]})Assert.Equal("elementPath",Assert.Throws<ArgumentOutOfRangeException>(()=>doc.ReadElement(path)).ParamName);
        Assert.Equal("ownerElementPath",Assert.Throws<ArgumentException>(()=>doc.ReadAttribute(default,"","id")).ParamName);
        Assert.Equal("ownerElementPath",Assert.Throws<ArgumentOutOfRangeException>(()=>doc.ReadAttribute([2],"","id")).ParamName);
        Assert.Equal("namespaceUri",Assert.Throws<ArgumentNullException>(()=>doc.ReadAttribute([],null!,"id")).ParamName);
        Assert.Equal("localName",Assert.Throws<ArgumentNullException>(()=>doc.ReadAttribute([],"",null!)).ParamName);
        foreach(var name in new[]{"","x:y","a b"})Assert.Equal("localName",Assert.Throws<ArgumentException>(()=>doc.ReadAttribute([],"",name)).ParamName);
        Assert.Equal("localName",Assert.Throws<ArgumentException>(()=>doc.ReadAttribute([],"urn:wrong","id")).ParamName);
        Assert.Equal("A",doc.ReadAttribute([],"","id").StringValue);
    }
    /// <summary>Narrow query limits roll back a refused cache entry and keep smaller reads usable.</summary>
    /// <param name="scale">Exercises decimal scale instead of coefficient digits.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void QueryLimit_RefusalRecovers(bool scale)
    {
        using var session=NativeSchemaNodeDataSession.Create(Schema,Options(query:scale?256:2,scale:scale?1:256,cache:1)).Value!;using var doc=session.Assess(scale?"<r><v>1.23</v><v>1</v></r>":Xml,Context).Value!;
        Assert.Throws<NotSupportedException>(()=>doc.ReadElement([0]));Assert.Equal("1",Text(doc.ReadElement([1])));Assert.Equal("1",Text(doc.ReadElement([1])));Assert.NotEmpty(doc.ExportXml());
    }
    /// <summary>Distinct original nodes share a bounded cache without aliasing repeated reads.</summary>
    [Fact]
    public void Cache_TracksOriginalTargets(){using var session=NativeSchemaNodeDataSession.Create(Schema,Options(cache:1)).Value!;using var doc=session.Assess(Xml,Context).Value!;Assert.Equal("123",Text(doc.ReadElement([0])));Assert.Equal("123",Text(doc.ReadElement([0])));Assert.Throws<NotSupportedException>(()=>doc.ReadElement([1]));Assert.Throws<NotSupportedException>(()=>doc.ReadAttribute([],"","id"));}
    /// <summary>Cancellation and export refusal preserve access; document and session disposal revoke it.</summary>
    [Fact]
    public void LifetimeAndCancellation_RecoverAndRevoke()
    {
        using var session=NativeSchemaNodeDataSession.Create(Schema,Options(export:10)).Value!;using var cancel=new CancellationTokenSource();cancel.Cancel();
        Assert.Throws<OperationCanceledException>(()=>session.Assess(Xml,Context,cancel.Token));var doc=session.Assess(Xml,Context).Value!;
        Assert.Throws<OperationCanceledException>(()=>doc.ReadElement([0],cancel.Token));Assert.Throws<OperationCanceledException>(()=>doc.ReadAttribute([],"","id",cancel.Token));Assert.Throws<OperationCanceledException>(()=>doc.ExportXml(cancel.Token));
        Assert.Throws<NotSupportedException>(()=>doc.ExportXml());Assert.Equal("123",Text(doc.ReadElement([0])));Assert.Throws<InvalidOperationException>(()=>session.Assess(Xml,Context));
        var value=doc.ReadAttribute([],"","id");doc.Dispose();doc.Dispose();Assert.Throws<ObjectDisposedException>(()=>doc.ReadElement([0]));Assert.Throws<ObjectDisposedException>(()=>doc.ReadAttribute([],"","id"));Assert.Throws<ObjectDisposedException>(()=>doc.ExportXml());
        using var second=session.Assess(Xml,Context).Value!;session.Dispose();session.Dispose();Assert.Throws<ObjectDisposedException>(()=>second.ReadElement([0]));Assert.Throws<ObjectDisposedException>(()=>session.Assess(Xml,Context));Assert.Equal("A",value.StringValue);
    }
    /// <summary>Invalid assessment publishes no document and leaves admission available.</summary>
    [Fact]
    public void InvalidInput_DoesNotAdmit(){using var session=NativeSchemaNodeDataSession.Create(Schema,Options()).Value!;var bad=session.Assess("<r><v>bad</v></r>",Context);Assert.Equal(NativeSchemaOutcome.Invalid,bad.Outcome);Assert.Null(bad.Value);using var good=session.Assess(Xml,Context).Value!;Assert.Equal("A",good.ReadAttribute([],"","id").StringValue);}
    /// <summary>Unassessed schema-location attributes cannot be atomized as declared attributes.</summary>
    [Fact]
    public void HintAttribute_IsUnsupported(){using var session=NativeSchemaNodeDataSession.Create(Schema,Options()).Value!;using var doc=session.Assess("<r xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:noNamespaceSchemaLocation='unused'><v>1</v></r>",Context).Value!;Assert.Throws<NotSupportedException>(()=>doc.ReadAttribute([],"http://www.w3.org/2001/XMLSchema-instance","noNamespaceSchemaLocation"));Assert.Equal("1",Text(doc.ReadElement([0])));}
    /// <summary>Factories validate required arguments without creating sessions.</summary>
    [Fact]
    public void Factories_ValidateRequiredArguments(){Assert.Throws<ArgumentNullException>(()=>NativeSchemaNodeDataSession.Create(null!,Options()));Assert.Throws<ArgumentNullException>(()=>NativeSchemaNodeDataSession.Create(Schema,null!));using var s=NativeSchemaNodeDataSession.Create(Schema,Options()).Value!;Assert.Throws<ArgumentNullException>(()=>s.Assess(null!,Context));Assert.Throws<ArgumentNullException>(()=>s.Assess(Xml,null!));}
}
