// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Qualifies isolated native type selection, original facts and ownership.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Xunit;

namespace XsiTypeProbe;

/// <summary>Exercises actual native selected-type facts through signed public Core operations.</summary>
public sealed class XsiTypeTests
{
    private const string Xs="http://www.w3.org/2001/XMLSchema";
    private const string Xsi="http://www.w3.org/2001/XMLSchema-instance";
    private const string Ns="urn:types";
    private static NativeSchemaSessionOptions Options(int query=256,int cache=32,int export=65536)=>new(256,256,1024,export,1,cache,1,query,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static string Text(NativeScalarValue value)=>value.Kind switch{NativeScalarKind.String=>value.StringValue,NativeScalarKind.Boolean=>value.BooleanValue?"true":"false",NativeScalarKind.Integer=>value.IntegerValue.ToString(CultureInfo.InvariantCulture),_=>value.DecimalValue.ToInvariantString(256)};
    private static (string Schema,string Xml) Payload(string family="decimal",string value="9",string request="t:Limited",string declared="primitive")
    {
        var facet=family=="string"?"<xs:maxLength value='1'/>":"<xs:maxInclusive value='10'/>";
        var type=declared=="named"?"t:Limited":"xs:"+family;
        var schema=$"<xs:schema xmlns:xs='{Xs}' xmlns:t='{Ns}' targetNamespace='{Ns}' elementFormDefault='qualified'><xs:simpleType name='Limited'><xs:restriction base='xs:{family}'>{facet}</xs:restriction></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='{type}'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        var xml=$"<t:r xmlns:t='{Ns}' xmlns:i='{Xsi}' xmlns:xs='{Xs}'><t:v i:type='{request}'>{System.Security.SecurityElement.Escape(value)}</t:v></t:r>";
        return (schema,xml);
    }
    /// <summary>Gets the concrete proposal payloads, separately for each public owned facade.</summary>
    /// <returns>Proposed identity, payload and facade choice.</returns>
    public static IEnumerable<object[]> Cases()
    {
        using var data=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"proposal-cases.json")));
        foreach(var row in data.RootElement.GetProperty("rows").EnumerateArray())
            if(row.TryGetProperty("schema",out _))foreach(var nodeData in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetRawText(),nodeData];
    }
    /// <summary>Both owned facades enforce selected types and complete request failures without partial output.</summary>
    /// <param name="id">Original proposed identity.</param>
    /// <param name="payload">Schema, instance and bounded expected result.</param>
    /// <param name="nodeData">Selects the fixed data facade.</param>
    [Theory][MemberData(nameof(Cases))]
    public void ProposalPayloads_ExecuteActualNativeSelection(string id,string payload,bool nodeData)
    {
        Assert.StartsWith("XT-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;var schema=row.GetProperty("schema").GetString()!;var xml=row.GetProperty("instance").GetString()!;var expected=row.GetProperty("proposedAssessment").GetString()!.Split('/');
        NativeScalarValue? detached=null;
        if(nodeData)
        {
            var created=NativeSchemaNodeDataSession.Create(schema,Options());Assert.Equal(NativeSchemaOutcome.Valid,created.Outcome);using var session=created.Value!;var result=session.Assess(xml,Context);Assert.Equal(expected[0],result.Outcome.ToString());
            if(result.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(result.Value);if(expected.Length>1)Assert.Equal(expected[1],Assert.Single(result.Diagnostics).UnsupportedReason.ToString());return;}
            using var document=result.Value!;detached=document.ReadElement([0]);var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));var text=Text(detached);document.Dispose();using var reopened=session.Assess(exported,Context).Value!;Assert.Equal(text,Text(reopened.ReadElement([0])));
        }
        else
        {
            var created=NativeSchemaSession.Create(schema,Options());Assert.Equal(NativeSchemaOutcome.Valid,created.Outcome);using var session=created.Value!;var result=session.Assess(xml,Context);Assert.Equal(expected[0],result.Outcome.ToString());
            if(result.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(result.Value);if(expected.Length>1)Assert.Equal(expected[1],Assert.Single(result.Diagnostics).UnsupportedReason.ToString());return;}
            using var document=result.Value!;Assert.Equal(1,document.ScalarCount);detached=document.ReadScalar(0);var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));var text=Text(detached);document.Dispose();using var reopened=session.Assess(exported,Context).Value!;Assert.Equal(text,Text(reopened.ReadScalar(0)));
        }
        Assert.NotNull(detached);Assert.NotEmpty(Text(detached));
    }
    /// <summary>Original declaration and its type remain distinct from effective governing and atomic evidence.</summary>
    /// <param name="family">Existing scalar family.</param>
    [Theory][InlineData("decimal")][InlineData("string")][InlineData("integer")]
    public void SelectedRestriction_OriginalDAndTRemainDistinctFromG(string family)
    {
        var payload=Payload(family,family=="string"?"A":"9");using var f=new Fixture(payload.Schema,payload.Xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=f.Element([0]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;
        var declared=fact.NativeDeclaration.Type;var governing=fact.NativeType;Assert.NotSame(declared,governing);Assert.Same(declared,governing.BaseType);Assert.Equal(family,declared.Name);Assert.Equal(Xs,declared.NamespaceUri);Assert.Equal("Limited",governing.Name);Assert.Equal((Ns,"Limited"),node.SchemaTypeAnnotation!.Value);
        var type=f.Provider.ResolveType(new(Ns,"Limited"),default);Assert.Equal(SchemaResultKind.Supported,type.Kind);Assert.Same(type.Value,fact.Type);Assert.True(f.Provider.Owns(fact.Declaration));Assert.True(f.Session.Advisor!.OwnsNode(node,fact,f.Input,f.Assessment.Origin));Assert.Same(fact,((ISchemaAnnotatedNode)node).OriginalSchemaFacts);
        f.Run(operation=>
        {
            var raw=fact.ReadTypedValue(default);Assert.Equal(SchemaResultKind.Supported,raw.Kind);Assert.Same(fact.Type,raw.Value.GoverningType);
            var admitted=operation.ReadTypedValue(f.Snapshot!,raw.Value);Assert.True(admitted.Kind==SchemaResultKind.Supported,"ReadTypedValue refusal: "+admitted.DiagnosticCode);Assert.True(SchemaAtomics.TryGetProvenance(admitted.Value,out var direct));Assert.Same(fact.Type,direct!.DeclaredType);Assert.Same(fact.Type,direct.ActualType);
            var evaluated=f.Evaluate(operation,node);Assert.True(SchemaAtomics.TryGetProvenance(evaluated,out var evidence));Assert.Same(f.Assessment.Origin,evidence!.OriginalAssessment);Assert.Same(fact.Type,evidence.DeclaredType);Assert.Same(fact.Type,evidence.ActualType);return 0;
        });
        var detached=f.Read(node);f.Dispose();Assert.Equal(family=="string"?"A":"9",Text(detached));Assert.Throws<ObjectDisposedException>(()=>node.SchemaTypeAnnotation);
    }
    /// <summary>Selected facets invalidate values that fit the original primitive and publish no facts.</summary>
    /// <param name="family">Scalar family.</param>
    [Theory][InlineData("decimal")][InlineData("string")][InlineData("integer")]
    public void SelectedFacetFailure_PublishesNoFacts(string family)
    {
        var p=Payload(family,family=="string"?"AB":"11");using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Null(f.Snapshot);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);Assert.Throws<NotSupportedException>(()=>f.Read(f.Element([0])));
    }
    /// <summary>Same-type scalar data without a request retains original declaration identity.</summary>
    [Fact]
    public void NoRequest_IdentityIsPreserved()
    {
        var p=Payload();p.Xml=p.Xml.Replace(" i:type='t:Limited'","",StringComparison.Ordinal);using var f=new Fixture(p.Schema,p.Xml);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.Same(fact.NativeDeclaration.Type,fact.NativeType);Assert.Equal("9",Text(f.Read(f.Element([0]))));
    }
    /// <summary>Same named complex simple-content type retains complex G and distinct atomic C.</summary>
    [Fact]
    public void ComplexSimpleContent_GoverningAndAtomicTypesStayDistinct()
    {
        var schema=$"<xs:schema xmlns:xs='{Xs}' xmlns:t='{Ns}' targetNamespace='{Ns}'><xs:complexType name='Money'><xs:simpleContent><xs:extension base='xs:decimal'><xs:attribute name='code' type='xs:string'/></xs:extension></xs:simpleContent></xs:complexType><xs:element name='r' type='t:Money'/></xs:schema>";
        using var f=new Fixture(schema,$"<t:r xmlns:t='{Ns}' xmlns:i='{Xsi}' i:type='t:Money' code='USD'>9.50</t:r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=f.Element([]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.Same(fact.NativeDeclaration.Type,fact.NativeType);Assert.True(fact.NativeType.IsComplex);Assert.Equal("9.5",Text(f.Read(node)));Assert.Equal("USD",f.Read(f.Attribute([],"","code")).StringValue);
        f.Run(operation=>{var value=f.Evaluate(operation,node);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(f.Provider.DecimalType,evidence.ActualType);Assert.NotSame(evidence.DeclaredType,evidence.ActualType);return 0;});
    }
    /// <summary>Nested original owners process same complex and selected scalar types consistently.</summary>
    /// <param name="simpleContent">Selects a complex scalar terminal instead of an element-only container.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void NestedOwners_UseOriginalTypeSelection(bool simpleContent)
    {
        var body=simpleContent?"<xs:simpleContent><xs:extension base='xs:decimal'><xs:attribute name='code' type='xs:string'/></xs:extension></xs:simpleContent>":"<xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence><xs:attribute name='code' type='xs:string'/>";
        var schema=$"<xs:schema xmlns:xs='{Xs}' xmlns:t='{Ns}' targetNamespace='{Ns}' elementFormDefault='qualified'><xs:simpleType name='Limited'><xs:restriction base='xs:decimal'><xs:maxInclusive value='10'/></xs:restriction></xs:simpleType><xs:complexType name='Box'>{body}</xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='t:Box'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        var content=simpleContent?"9.50":"<t:v i:type='t:Limited'>9.50</t:v>";using var f=new Fixture(schema,$"<t:r xmlns:t='{Ns}' xmlns:i='{Xsi}'><t:a i:type='t:Box' code='X'>{content}</t:a></t:r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal("X",f.Read(f.Attribute([0],"","code")).StringValue);var target=f.Element(simpleContent?[0]:[0,0]);Assert.Equal("9.5",Text(f.Read(target)));Assert.Single(f.Assessment.DescendantAttributeFacts);Assert.Equal(new[]{0},f.Assessment.DescendantAttributeFacts[0].OwnerElementPath);
    }
    /// <summary>Selected-type requests cannot transfer ownership between equal source sessions or copied XML.</summary>
    [Fact]
    public void SelectedFacts_ForeignAndCopiedNodesAreRefused()
    {
        var p=Payload();using var a=new Fixture(p.Schema,p.Xml);using var b=new Fixture(p.Schema,p.Xml);var node=a.Element([0]);var facts=a.Input.FactsFor((NativeSessionNode)node)!;Assert.False(b.Session.Advisor!.OwnsNode(node,facts,a.Input,a.Assessment.Origin));Assert.Throws<ArgumentException>(()=>b.Run(op=>b.Evaluate(op,node)));using var copy=NativeSchemaInput.Capture(p.Xml).Value!;Assert.Throws<ArgumentException>(()=>a.Run(op=>a.Evaluate(op,copy.ElementNodes[1])));Assert.Equal("9",Text(b.Read(b.Element([0]))));
    }
    /// <summary>Special request attributes stay outside application-attribute typed reads and counts.</summary>
    [Fact]
    public void RequestAttribute_IsNotAFabricatedStringFact()
    {
        var p=Payload();using var f=new Fixture(p.Schema,p.Xml);Assert.Empty(f.Assessment.AttributeFacts);Assert.Empty(f.Assessment.DescendantAttributeFacts);Assert.Throws<NotSupportedException>(()=>f.Read(f.Attribute([0],Xsi,"type")));using var session=NativeSchemaNodeDataSession.Create(p.Schema,Options()).Value!;using var document=session.Assess(p.Xml,Context).Value!;Assert.Throws<NotSupportedException>(()=>document.ReadAttribute([0],Xsi,"type"));Assert.Equal("9",Text(document.ReadElement([0])));
    }
    /// <summary>Invalid requested facets and cancellation leave later document admission available.</summary>
    [Fact]
    public void FailureAndCancellation_PreserveRecovery()
    {
        var good=Payload();var bad=Payload(value:"11");using var session=NativeSchemaNodeDataSession.Create(good.Schema,Options()).Value!;var failed=session.Assess(bad.Xml,Context);Assert.Equal(NativeSchemaOutcome.Invalid,failed.Outcome);Assert.Null(failed.Value);using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>session.Assess(good.Xml,Context,cancel.Token));using var document=session.Assess(good.Xml,Context).Value!;Assert.Equal("9",Text(document.ReadElement([0])));
    }
    /// <summary>The decimal-only facade keeps its old explicit request refusal.</summary>
    [Fact]
    public void LegacyFacade_StillRefusesRequests()
    {
        const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        using var session=NativeDecimalSession.Create(schema,new(256,256,1,8,1,256,256)).Value!;var result=session.Assess($"<r xmlns:i='{Xsi}' xmlns:xs='{Xs}'><v i:type='xs:decimal'>9</v></r>",new(DateTimeOffset.UnixEpoch,0));Assert.Equal(NativeDecimalOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);
    }
}
