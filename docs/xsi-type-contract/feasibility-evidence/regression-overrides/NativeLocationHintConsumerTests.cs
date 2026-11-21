// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Verifies unfetched location hints preserve supplied-schema validation and original XML.
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

using System.Xml.Linq;
using Xunit;

namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Verifies supplied-schema hint policy through public consumer APIs only.</summary>
public sealed class NativeLocationHintConsumerTests
{
    private const string Xsi = "http://www.w3.org/2001/XMLSchema-instance";
    private const string FlatSchema = """<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"><xs:element name="root"><xs:complexType><xs:sequence><xs:element name="v" type="xs:decimal"/></xs:sequence></xs:complexType></xs:element></xs:schema>""";
    private const string NestedSchema = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
          <xs:element name="root"><xs:complexType><xs:sequence>
            <xs:element name="box"><xs:complexType><xs:sequence>
              <xs:element name="price"><xs:complexType><xs:simpleContent><xs:extension base="xs:decimal"><xs:attribute name="currency" type="xs:string" use="required"/></xs:extension></xs:simpleContent></xs:complexType></xs:element>
              <xs:element name="label" type="xs:string"/>
            </xs:sequence><xs:attribute name="code" type="xs:string" use="required"/></xs:complexType></xs:element>
          </xs:sequence><xs:attribute name="id" type="xs:integer" use="required"/></xs:complexType></xs:element>
        </xs:schema>
        """;
    private const string NestedXml = """<root id="7"><box code="A"><price currency="EUR">12.50</price><label>original</label></box></root>""";
    private static NativeSchemaSessionOptions Options => new(256,256,1024,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context => new(DateTimeOffset.UnixEpoch,0);

    /// <summary>Gets hint names, uninterpreted values and all admitted owner shapes.</summary>
    /// <returns>Independent owner and lexical boundary cases.</returns>
    public static IEnumerable<object[]> HintCases()
    {
        foreach(var name in new[]{"schemaLocation","noNamespaceSchemaLocation"})
        foreach(var value in new[]{"","relative.xsd","urn:wrong http://127.0.0.1:1/must-not-fetch.xsd","  file:///does-not-exist/schema.xsd\t\n"})
        for(var owner=0;owner<4;owner++)yield return [name,value,owner];
    }

    /// <summary>Hints retain original XML and do not occupy application attribute or scalar slots.</summary>
    /// <param name="name">The exact hint local name.</param>
    /// <param name="value">The uninterpreted parsed hint value.</param>
    /// <param name="owner">Root, container, simple-content terminal or scalar leaf.</param>
    [Theory][MemberData(nameof(HintCases))]
    public void Hints_PreserveSuppliedSchemaValuesAndOriginalExport(string name,string value,int owner)
    {
        var xml=XElement.Parse(NestedXml);xml.SetAttributeValue(XNamespace.Xmlns+"hint",Xsi);
        var target=new[]{xml,xml.Element("box")!,xml.Element("box")!.Element("price")!,xml.Element("box")!.Element("label")!}[owner];
        target.SetAttributeValue(XName.Get(name,Xsi),value);
        using var session=NativeSchemaSession.Create(NestedSchema,Options).Value!;
        var result=session.Assess(xml.ToString(SaveOptions.DisableFormatting),Context);
        Assert.Equal(NativeSchemaOutcome.Valid,result.Outcome);using var document=result.Value!;
        Assert.Equal(1,document.AttributeCount);Assert.Equal(2,document.DescendantAttributeCount);Assert.Equal(2,document.ScalarCount);
        Assert.Equal("7",document.ReadAttribute(0).Value.IntegerValue.ToString());
        Assert.Equal("A",document.ReadDescendantAttribute(0).Value.StringValue);Assert.Equal("EUR",document.ReadDescendantAttribute(1).Value.StringValue);
        Assert.Equal("12.5",document.ReadScalar(0).DecimalValue.ToInvariantString(256));Assert.Equal("original",document.ReadScalar(1).StringValue);
        Assert.True(XNode.DeepEquals(xml,XElement.Parse(document.ExportXml())));
    }

    /// <summary>Flat schemas and simple-content roots admit hints through both root attribute branches.</summary>
    /// <param name="simple">Selects simple content rather than an element-only root.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void RootAndFlatLeafHints_AreNotSchemaSelectionInstructions(bool simple)
    {
        var schema=simple?"""<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"><xs:element name="root"><xs:complexType><xs:simpleContent><xs:extension base="xs:decimal"/></xs:simpleContent></xs:complexType></xs:element></xs:schema>""":FlatSchema;
        var xml=simple?$"<root xmlns:h='{Xsi}' h:noNamespaceSchemaLocation='missing.xsd'>2</root>":$"<root xmlns:h='{Xsi}' h:schemaLocation='urn:other missing.xsd'><v h:noNamespaceSchemaLocation='missing2.xsd'>2</v></root>";
        using var session=NativeSchemaSession.Create(schema,Options).Value!;
        using var document=session.Assess(xml,Context).Value!;
        Assert.Equal(0,document.AttributeCount);Assert.Contains("missing.xsd",document.ExportXml());
        Assert.Equal("2",(simple?document.ReadRootScalar():document.ReadScalar(0)).DecimalValue.ToInvariantString(256));
    }

    /// <summary>Hint presence cannot suppress required attributes or invalid decimal content.</summary>
    /// <param name="missingAttribute">Removes a required descendant attribute.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void Hints_DoNotBypassSchemaValidation(bool missingAttribute)
    {
        var xml=XElement.Parse(NestedXml);xml.SetAttributeValue(XName.Get("schemaLocation",Xsi),"urn:replacement replacement.xsd");
        if(missingAttribute)xml.Element("box")!.Attribute("code")!.Remove();else xml.Element("box")!.Element("price")!.Value="NaN";
        using var session=NativeSchemaSession.Create(NestedSchema,Options).Value!;
        var result=session.Assess(xml.ToString(),Context);Assert.Equal(NativeSchemaOutcome.Invalid,result.Outcome);Assert.Null(result.Value);
        using var recovered=session.Assess(NestedXml,Context).Value!;Assert.Equal(2,recovered.ScalarCount);
    }

    /// <summary>Other special requests remain unsupported even alongside an admitted hint.</summary>
    /// <param name="request">The refused special local name.</param>
    /// <param name="owner">Root or descendant with application attributes.</param>
    [Theory][InlineData("type",0)][InlineData("nil",0)][InlineData("unknown",0)][InlineData("type",1)][InlineData("nil",1)][InlineData("unknown",1)]
    public void OtherXsiRequests_RemainUnsupported(string request,int owner)
    {
        var xml=XElement.Parse(NestedXml);var target=owner==0?xml:xml.Element("box")!;
        target.SetAttributeValue(XName.Get("noNamespaceSchemaLocation",Xsi),"missing.xsd");target.SetAttributeValue(XName.Get(request,Xsi),"true");
        using var session=NativeSchemaSession.Create(NestedSchema,Options).Value!;
        var result=session.Assess(xml.ToString(),Context);Assert.Equal(request=="type"?NativeSchemaOutcome.Invalid:NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);
        if(request!="type")Assert.Equal(NativeSchemaUnsupportedReason.Attributes,result.Diagnostics[0].UnsupportedReason);else Assert.Equal("BSK-NATIVE-INSTANCE-INVALID",Assert.Single(result.Diagnostics).Code);
    }

    /// <summary>Unqualified and wrong-namespace lookalikes do not acquire special hint treatment.</summary>
    /// <param name="ns">The non-xsi namespace.</param>
    [Theory][InlineData("")][InlineData("urn:other")]
    public void Lookalikes_AreNotHints(string ns)
    {
        using var session=NativeSchemaSession.Create(FlatSchema,Options).Value!;
        var xml=XElement.Parse("<root><v>2</v></root>");xml.SetAttributeValue(XName.Get("schemaLocation",ns),"missing.xsd");
        var result=session.Assess(xml.ToString(),Context);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);
    }

    /// <summary>The legacy decimal facade preserves both location-hint refusals.</summary>
    /// <param name="name">The original unsupported hint.</param>
    [Theory][InlineData("schemaLocation")][InlineData("noNamespaceSchemaLocation")]
    public void LegacyDecimalHints_StillRefuse(string name)
    {
        using var session=NativeDecimalSession.Create(FlatSchema,new(256,256,1,32,1,256,256)).Value!;
        var result=session.Assess($"<root xmlns:h='{Xsi}' h:{name}='missing.xsd'><v>2</v></root>",new(DateTimeOffset.UnixEpoch,0));
        Assert.Equal(NativeDecimalOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);
    }

    /// <summary>Cancellation and detached export preserve session and document lifetime rules.</summary>
    [Fact]
    public void CancellationAndDisposal_PreserveRecoveryAndDetachedExport()
    {
        using var session=NativeSchemaSession.Create(FlatSchema,Options).Value!;
        var xml=$"<root xmlns:h='{Xsi}' h:noNamespaceSchemaLocation='missing.xsd'><v>2</v></root>";
        using var cancelled=new CancellationTokenSource();cancelled.Cancel();
        Assert.Throws<OperationCanceledException>(()=>session.Assess(xml,Context,cancelled.Token));
        var document=session.Assess(xml,Context).Value!;var exported=document.ExportXml();document.Dispose();
        Assert.Contains("missing.xsd",exported);Assert.Throws<ObjectDisposedException>(()=>document.ReadScalar(0));
        using var recovered=session.Assess(xml,Context).Value!;Assert.Equal(1,recovered.ScalarCount);
    }
}
