// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies original markup and opaque schema annotation contracts.
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
namespace Bosak.Schema.Tests.MarkupCoverage;

/// <summary>Verifies original markup nodes, physical element paths, provenance and expiry.</summary>
public sealed class NativeMarkupCoverageIdentityTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(string model="sequence")=>$"<xs:schema xmlns:xs='{X}'><xs:element name='r'><xs:complexType><xs:{model}><xs:element name='v' type='xs:int'/></xs:{model}></xs:complexType></xs:element></xs:schema>";
    /// <summary>Original comment/PI wrappers remain ordered owned nodes without fabricated schema facts.</summary>
    /// <param name="model">Admitted model group.</param>
    [Theory][InlineData("sequence")][InlineData("all")][InlineData("choice")]
    public void OriginalMarkupAuthority(string model)
    {
        const string xml="<r><!--before--><?p payload?><v>1<!--inside--><?q ignored?>2</v><!--after--></r>";
        using var f=new Fixture(Schema(model),xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var nodes=new List<NativeSessionNode>();foreach(var item in f.Input.Root.Children(XdmNodeKind.All))nodes.Add((NativeSessionNode)item.NodeValue);
        Assert.Equal(new[]{XdmNodeKind.Comment,XdmNodeKind.ProcessingInstruction,XdmNodeKind.Element,XdmNodeKind.Comment},nodes.Select(n=>n.NodeKind));
        Assert.Same(f.Element([0]),nodes[2]);Assert.Equal("12",nodes[2].StringValue);Assert.Equal("12",f.Read(nodes[2]).IntegerValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
        foreach(var n in nodes.Where(n=>n.NodeKind!=XdmNodeKind.Element))
        {
            Assert.True(f.Input.OwnsNode(n));Assert.Same(f.Input.Root,n.Parent);Assert.Null(f.Input.FactsFor(n));Assert.Null(n.SchemaTypeAnnotation);Assert.Equal(XdmValueKind.String,n.TypedValue.Kind);Assert.Equal(n.StringValue,n.TypedValue.StringValue);Assert.Throws<NotSupportedException>(()=>n.OriginalSchemaFacts);
        }
        var facts=f.Input.FactsFor(nodes[2])!;Assert.True(f.Session.Advisor!.OwnsTypedValue(facts.ReadTypedValue(default).Value,f.Input,f.Assessment.Origin));
        var cursor=f.Input.Root.Children(XdmNodeKind.All).GetEnumerator();Assert.True(cursor.MoveNext());var detached=f.Read(nodes[2]).IntegerValue;
        f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>cursor.MoveNext());Assert.Throws<ObjectDisposedException>(()=>nodes[0].StringValue);Assert.Throws<ObjectDisposedException>(()=>facts.ReadTypedValue(default));Assert.Equal("12",detached.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }
    /// <summary>Opaque appinfo never supplies anonymous type sites or declarations to the compiler.</summary>
    [Fact]
    public void OpaqueSourceSites()
    {
        var payload=string.Concat(Enumerable.Range(0,20).Select(i=>"<xs:attribute name='fake"+i+"'><xs:simpleType><xs:restriction base='missing:bad'/></xs:simpleType></xs:attribute>"));
        var schema=Schema().Replace("<xs:element name='r'>","<xs:annotation><xs:appinfo>"+payload+"</xs:appinfo></xs:annotation><xs:element name='r'>",StringComparison.Ordinal);
        using var f=new Fixture(schema,"<r><v>1</v></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal("1",f.Read(f.Element([0])).IntegerValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }
    /// <summary>Annotation payload nodes still consume the original pre-normalization XML budget.</summary>
    [Fact]
    public void OpaqueNodesAreBounded()
    {
        var schema=Schema().Replace("<xs:element name='r'>","<xs:annotation><xs:appinfo>"+string.Concat(Enumerable.Repeat("<x/>",256))+"</xs:appinfo></xs:annotation><xs:element name='r'>",StringComparison.Ordinal);
        var c=NativeSchemaSession.Create(schema,new(256,256,16384,65536,1,256,16,256,1024));Assert.Equal(NativeSchemaOutcome.Unsupported,c.Outcome);Assert.Null(c.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(c.Diagnostics).UnsupportedReason);
    }
    /// <summary>Closed schema assembly normalizes annotation payloads before component projection.</summary>
    [Fact]
    public void AnnotatedClosedInclude()
    {
        const string uri="https://example.invalid/root.xsd";var schema=$"<xs:schema xmlns:xs='{X}'><xs:annotation/><xs:include schemaLocation='part.xsd'><xs:annotation><xs:documentation>included</xs:documentation></xs:annotation></xs:include><xs:element name='r' type='D'/></xs:schema>";
        var part=$"<xs:schema xmlns:xs='{X}'><xs:annotation/><xs:simpleType name='D'><xs:annotation><xs:appinfo><xs:complexType name='bad'/></xs:appinfo></xs:annotation><xs:restriction base='xs:int'/></xs:simpleType></xs:schema>";
        var c=NativeSchemaSession.Create(uri,new Dictionary<string,string>{{uri,schema},{"https://example.invalid/part.xsd",part}},new(256,256,1024,65536,1,256,16,256,256));Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var owner=c.Value!;var a=owner.Assess("<r>2</r>",new(DateTimeOffset.UnixEpoch,0));Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.Equal("2",d.ReadRootScalar().IntegerValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }
}
