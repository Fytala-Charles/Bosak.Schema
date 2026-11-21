// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Verifies isolated complex restriction identities, facets and original ownership.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
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
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;

namespace ComplexProbe;

/// <summary>Uses actual native original assessments with signed public Core.</summary>
public sealed class ComplexTests
{
    private const string Xs="http://www.w3.org/2001/XMLSchema";
    private const string Xsi="http://www.w3.org/2001/XMLSchema-instance";
    private const string Ns="urn:complex:restriction";
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static string Text(NativeScalarValue v)=>v.DecimalValue.ToInvariantString(256);
    private static string Schema(string facets="<xs:maxExclusive value='5'/>",string baseFacets="",bool nested=false)=>
        $"<xs:schema xmlns:xs='{Xs}' xmlns:t='{Ns}' targetNamespace='{Ns}' elementFormDefault='qualified'><xs:simpleType name='Value'><xs:restriction base='xs:decimal'>{baseFacets}</xs:restriction></xs:simpleType><xs:complexType name='Base'><xs:simpleContent><xs:extension base='t:Value'/></xs:simpleContent></xs:complexType><xs:complexType name='Derived'><xs:simpleContent><xs:restriction base='t:Base'>{facets}</xs:restriction></xs:simpleContent></xs:complexType>{(nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='t:Derived'/></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r' type='t:Derived'/>")}</xs:schema>";
    private static string Xml(string value="4.5",bool nested=false)=>$"<t:r xmlns:t='{Ns}'>{(nested?$"<t:v>{value}</t:v>":value)}</t:r>";
    /// <summary>Gets all concrete proposal payloads for both public facades.</summary>
    /// <returns>Identity, payload and facade.</returns>
    public static IEnumerable<object[]> Cases()
    {using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"case-plan.json")));foreach(var row in d.RootElement.GetProperty("rows").EnumerateArray())foreach(var data in new[]{false,true})yield return [row.GetProperty("id").GetString()!,row.GetRawText(),data];}
    /// <summary>Executes concrete proposal validity/refusal through the actual public owned facades.</summary>
    /// <param name="id">Proposal identity.</param>
    /// <param name="payload">Full fixture and bounded expected outcomes.</param>
    /// <param name="data">Selects original-node data.</param>
    [Theory][MemberData(nameof(Cases))]
    public void ProposedPayloads_UseNativeAssessment(string id,string payload,bool data)
    {
        Assert.StartsWith("CR-",id);using var d=JsonDocument.Parse(payload);var r=d.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compiled=r.GetProperty("expectedCompile").GetString();var assessed=r.GetProperty("expectedAssessment").GetString();var reason=r.GetProperty("unsupportedReason").GetString();var path=r.GetProperty("path").EnumerateArray().Select(x=>x.GetInt32()).ToImmutableArray();
        if(data)
        {
            var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(compiled,c.Outcome.ToString());if(c.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(c.Value);Assert.Equal("BSK-NATIVE-SCHEMA-"+compiled!.ToUpperInvariant(),Assert.Single(c.Diagnostics).Code);if(reason is not null)Assert.Equal(reason,c.Diagnostics[0].UnsupportedReason.ToString());return;}
            using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(assessed,a.Outcome.ToString());if(a.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(a.Value);if(reason is not null)Assert.Equal(reason,Assert.Single(a.Diagnostics).UnsupportedReason.ToString());return;}
            using var document=a.Value!;var value=document.ReadElement(path);Assert.Equal(NativeScalarKind.Decimal,value.Kind);var text=Text(value);var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));document.Dispose();using var reopened=s.Assess(exported,Context).Value!;Assert.Equal(text,Text(reopened.ReadElement(path)));
        }
        else
        {
            var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(compiled,c.Outcome.ToString());if(c.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(c.Value);Assert.Equal("BSK-NATIVE-SCHEMA-"+compiled!.ToUpperInvariant(),Assert.Single(c.Diagnostics).Code);if(reason is not null)Assert.Equal(reason,c.Diagnostics[0].UnsupportedReason.ToString());return;}
            using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(assessed,a.Outcome.ToString());if(a.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(a.Value);if(reason is not null)Assert.Equal(reason,Assert.Single(a.Diagnostics).UnsupportedReason.ToString());return;}
            using var document=a.Value!;var value=document.HasRootScalar?document.ReadRootScalar():document.ReadScalar(0);Assert.Equal(NativeScalarKind.Decimal,value.Kind);var text=Text(value);var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));document.Dispose();using var reopened=s.Assess(exported,Context).Value!;Assert.Equal(text,Text(reopened.HasRootScalar?reopened.ReadRootScalar():reopened.ReadScalar(0)));
        }
    }
    /// <summary>Gets boundary triplets for each of the16 pinned new/base facet conditions.</summary>
    /// <returns>Facet names and candidate/inherited values with their normative expected compile result.</returns>
    public static IEnumerable<object[]> Bounds()
    {using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"facet-rules.json")));foreach(var row in d.RootElement.GetProperty("baseBoundConditions").EnumerateArray())foreach(var n in new[]{4,5,6}){var op=row.GetProperty("requiredOperator").GetString();var valid=op switch{"<"=>n<5,"<="=>n<=5,">"=>n>5,_=>n>=5};yield return [row.GetProperty("newFacet").GetString()!,row.GetProperty("baseFacet").GetString()!,n,valid];}}
    /// <summary>Checks all16 facet relations at less/equal/greater boundaries without sampling an instance.</summary>
    /// <param name="own">New facet.</param>
    /// <param name="parent">Inherited facet.</param>
    /// <param name="n">Candidate value.</param>
    /// <param name="valid">Normative acceptance from the pinned proposal table.</param>
    [Theory][MemberData(nameof(Bounds))]
    public void FacetConditions_ValidateBeforeInstances(string own,string parent,int n,bool valid)
    {var r=NativeDecimalSchema.Compile(Schema($"<xs:{own} value='{n}'/>",$"<xs:{parent} value='5'/>"),new(256,256),maxStringCharacters:1024);Assert.Equal(valid?NativeOutcome.Valid:NativeOutcome.Invalid,r.Outcome);if(!valid)Assert.Null(r.Value);}
    /// <summary>Checks actual complex and atomic identities and provenance at root and nested terminal.</summary>
    /// <param name="nested">Selects nested owner.</param>
    /// <param name="emptyFacets">Creates the required distinct content type even without facets.</param>
    [Theory][InlineData(false,false)][InlineData(true,false)][InlineData(false,true)][InlineData(true,true)]
    public void OriginalComponents_HaveTwoDistinctEdges(bool nested,bool emptyFacets)
    {
        using var f=new Fixture(Schema(emptyFacets?"":"<xs:maxExclusive value='5'/>","<xs:minInclusive value='0'/>",nested),Xml(nested:nested));Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=f.Element(nested?[0]:[]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;var g=fact.NativeType;var b=g.DerivationBase!;var c1=g.ContentType!;var c0=b.ContentType!;
        Assert.Same(fact.NativeDeclaration.Type,g);Assert.Equal("Derived",g.Name);Assert.Equal("Base",b.Name);Assert.NotSame(g,b);Assert.Null(c1.Name);Assert.Equal(Ns,c1.NamespaceUri);Assert.NotSame(c1,c0);Assert.Same(c0,c1.DerivationBase);Assert.Equal("Value",c0.Name);Assert.False(fact.HasNoTypedValue);Assert.Equal((Ns,"Derived"),node.SchemaTypeAnnotation!.Value);Assert.Same(fact,((ISchemaAnnotatedNode)node).OriginalSchemaFacts);Assert.True(f.Session.Advisor!.OwnsNode(node,fact,f.Input,f.Assessment.Origin));
        f.Run(op=>{var raw=fact.ReadTypedValue(default);Assert.Equal(SchemaResultKind.Supported,raw.Kind);Assert.Same(fact.Type,raw.Value.GoverningType);var guard=op.ReadTypedValue(f.Snapshot!,raw.Value);Assert.Equal(SchemaResultKind.Unsupported,guard.Kind);Assert.Equal("BOSAK-ORIGINAL-DATA-UNSUPPORTED",guard.DiagnosticCode);var value=f.Evaluate(op,node);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.NotSame(evidence.DeclaredType,evidence.ActualType);Assert.True(f.Provider.Owns(evidence.ActualType));Assert.Same(f.Assessment.Origin,evidence.OriginalAssessment);return 0;});Assert.Equal("4.5",Text(f.Read(node)));
    }
    /// <summary>Failed newly narrowed facets publish neither facts nor a snapshot.</summary>
    [Fact]
    public void InvalidSelectedFacets_PublishNothing()
    {using var f=new Fixture(Schema(),Xml("5"));Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Snapshot);Assert.Null(f.Assessment.Evidence);}
    /// <summary>Foreign equal-source nodes and XML copies retain no authority.</summary>
    [Fact]
    public void ForeignAndCopy_StillRefuse()
    {using var a=new Fixture(Schema(),Xml());using var b=new Fixture(Schema(),Xml());var node=a.Element([]);var fact=a.Input.FactsFor((NativeSessionNode)node)!;Assert.False(b.Session.Advisor!.OwnsNode(node,fact,a.Input,a.Assessment.Origin));Assert.Throws<ArgumentException>(()=>b.Run(op=>b.Evaluate(op,node)));using var copy=NativeSchemaInput.Capture(Xml()).Value!;Assert.Throws<ArgumentException>(()=>a.Run(op=>a.Evaluate(op,copy.Root)));}
    /// <summary>Two equal complex contexts own distinct anonymous atomic components.</summary>
    [Fact]
    public void Siblings_DoNotInternContentIdentity()
    {
        var schema=Schema("");schema=schema.Replace("<xs:element name='r' type='t:Derived'/>","<xs:complexType name='Sibling'><xs:simpleContent><xs:restriction base='t:Base'/></xs:simpleContent></xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='t:Derived'/><xs:element name='b' type='t:Sibling'/></xs:sequence></xs:complexType></xs:element>",StringComparison.Ordinal);
        using var f=new Fixture(schema,$"<t:r xmlns:t='{Ns}'><t:a>4</t:a><t:b>4</t:b></t:r>");var a=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!.NativeType;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!.NativeType;Assert.NotSame(a,b);Assert.NotSame(a.ContentType,b.ContentType);Assert.Same(a.DerivationBase,b.DerivationBase);Assert.Same(a.ContentType!.DerivationBase,b.ContentType!.DerivationBase);Assert.Equal("4",Text(f.Read(f.Element([0]))));Assert.Equal("4",Text(f.Read(f.Element([1]))));
    }
    /// <summary>Typed identity independently authenticates actual anonymous C1 without rebinding data-profile episodes.</summary>
    [Fact]
    public void TypedIdentity_HasItsOwnOriginalEvidence()
    {
        var compiled=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:1024).Value!;var provider=new NativeSchemaProvider(compiled);using var session=SchemaQuerySession.CompileXPath("$value",new(1,32,1,256,256),assessor:provider).Value;using var input=NativeSchemaInput.Capture(Xml()).Value!;var a=(NativeSchemaProvider.Assessment)session.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);Assert.Equal(SchemaAssessmentStatus.Valid,a.Status);var fact=a.ElementFacts[0];using var snapshot=session.OpenSnapshot(input,input,a.Origin).Value;
        new NativeSessionRunner(session).Run([snapshot],op=>{var raw=fact.ReadTypedValue(default);var admitted=op.ReadTypedValue(snapshot,raw.Value);Assert.Equal(SchemaResultKind.Supported,admitted.Kind);var value=op.Evaluate(new Bosak.XPath.Runtime.Vm.EvaluationContext().WithVariable("value",admitted.Value));Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(a.Origin,evidence.OriginalAssessment);Assert.NotSame(evidence.DeclaredType,evidence.ActualType);return 0;});
    }
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    /// <summary>Failure and cancellation leave admission for later valid original documents.</summary>
    [Fact]
    public void RefusalCancellationRecovery_DetachesAndRevokes()
    {using var s=NativeSchemaNodeDataSession.Create(Schema(),Options).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess(Xml("5"),Context).Outcome);using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>s.Assess(Xml(),Context,cancel.Token));using var d=s.Assess(Xml(),Context).Value!;var value=d.ReadElement([]);Assert.Equal("4.5",Text(value));s.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadElement([]));Assert.Equal("4.5",Text(value));}
}
