// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Verifies native type selection, original facts and ownership.
// SPECIAL NOTES        : Internal native authority controls; no full XSD conformance claim.
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

namespace Bosak.Schema.Tests;

/// <summary>Exercises actual native selected-type facts through signed public Core operations.</summary>
public sealed class NativeInstanceTypeCoreTests
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
    /// <summary>Original declaration and its type remain distinct from effective governing and atomic evidence.</summary>
    /// <param name="family">Existing scalar family.</param>
    [Theory][InlineData("decimal")][InlineData("string")][InlineData("integer")]
    public void SelectedRestriction_OriginalDAndTRemainDistinctFromG(string family)
    {
        var payload=Payload(family,family=="string"?"A":"9");using var f=new Fixture(payload.Schema,payload.Xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=f.Element([0]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;
        var declared=fact.NativeDeclaration.Type;var governing=fact.NativeType;Assert.NotSame(declared,governing);Assert.Equal(family,declared.Name);Assert.Equal(Xs,declared.NamespaceUri);Assert.Equal("Limited",governing.Name);Assert.Equal((Ns,"Limited"),node.SchemaTypeAnnotation!.Value);
        var type=f.Provider.ResolveType(new(Ns,"Limited"),default);Assert.Equal(SchemaResultKind.Supported,type.Kind);Assert.Same(type.Value,fact.Type);Assert.True(f.Provider.Owns(fact.Declaration));Assert.True(f.Session.Advisor!.OwnsNode(node,fact,f.Input,f.Assessment.Origin));Assert.Same(fact,((ISchemaAnnotatedNode)node).OriginalSchemaFacts);
        f.Run(operation=>
        {
            var raw=fact.ReadTypedValue(default);Assert.Equal(SchemaResultKind.Supported,raw.Kind);Assert.Same(fact.Type,raw.Value.GoverningType);
            var admitted=operation.ReadTypedValue(f.Snapshot!,raw.Value);Assert.Equal(SchemaResultKind.Unsupported,admitted.Kind);Assert.Equal("BOSAK-ORIGINAL-DATA-UNSUPPORTED",admitted.DiagnosticCode);
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
    /// <summary>Typed identity separately admits selected G without borrowing data-profile authority.</summary>
    /// <param name="family">Scalar family.</param>
    [Theory][InlineData("decimal")][InlineData("string")][InlineData("integer")]
    public void TypedIdentity_UsesItsOwnNativeAssessmentAndSelectedEvidence(string family)
    {
        var p=Payload(family,family=="string"?"A":"9");var schema=NativeDecimalSchema.Compile(p.Schema,new(256,256),maxStringCharacters:1024).Value!;var provider=new NativeSchemaProvider(schema);using var session=SchemaQuerySession.CompileXPath("$value",new(1,8,1,256,256),assessor:provider).Value;using var input=NativeSchemaInput.Capture(p.Xml).Value!;
        var assessment=(NativeSchemaProvider.Assessment)session.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);Assert.Equal(SchemaAssessmentStatus.Valid,assessment.Status);var fact=assessment.ElementFacts[1];Assert.NotSame(fact.NativeDeclaration.Type,fact.NativeType);Assert.Equal("Limited",fact.NativeType.Name);Assert.True(session.Advisor.OwnsNode(input.ElementNodes[1],fact,input,assessment.Origin));using var snapshot=session.OpenSnapshot(input,input,assessment.Origin).Value;
        new NativeSessionRunner(session).Run([snapshot],operation=>
        {
            var raw=fact.ReadTypedValue(default);Assert.Equal(SchemaResultKind.Supported,raw.Kind);Assert.Same(fact.Type,raw.Value.GoverningType);var admitted=operation.ReadTypedValue(snapshot,raw.Value);Assert.Equal(SchemaResultKind.Supported,admitted.Kind);
            var value=operation.Evaluate(new Bosak.XPath.Runtime.Vm.EvaluationContext().WithVariable("value",admitted.Value));Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(fact.Type,evidence.ActualType);Assert.Same(assessment.Origin,evidence.OriginalAssessment);return 0;
        });
    }
    private sealed record Episode : ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }

    /// <summary>Instance-local namespace bindings, not schema bindings, select the type.</summary>
    /// <param name="shadow">Locally replaces the alias with a namespace where the type is absent.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void InstancePrefixShadowing_UsesActualOwner(bool shadow)
    {
        var p=Payload(request:"q:Limited");p.Xml=p.Xml.Replace("<t:r ",$"<t:r xmlns:q='{Ns}' ",StringComparison.Ordinal);if(shadow)p.Xml=p.Xml.Replace("<t:v ","<t:v xmlns:q='urn:foreign' ",StringComparison.Ordinal);using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(shadow?SchemaAssessmentStatus.Invalid:SchemaAssessmentStatus.Valid,f.Assessment.Status);if(!shadow)Assert.Equal("Limited",f.Input.FactsFor((NativeSessionNode)f.Element([0]))!.NativeType.Name);else Assert.False(f.Input.IsAssessed);
    }
    /// <summary>An undeclared instance default namespace does not fall back to schema target namespace.</summary>
    /// <param name="clearDefault">Clears the default namespace only at the selected owner.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void DefaultNamespaceUndeclaration_IsNotSchemaFallback(bool clearDefault)
    {
        var p=Payload(request:"Limited");p.Xml=p.Xml.Replace("<t:r ",$"<t:r xmlns='{Ns}' ",StringComparison.Ordinal);if(clearDefault)p.Xml=p.Xml.Replace("<t:v ","<t:v xmlns='' ",StringComparison.Ordinal);using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(clearDefault?SchemaAssessmentStatus.Invalid:SchemaAssessmentStatus.Valid,f.Assessment.Status);
    }
    /// <summary>Directly restricted siblings are not derived from an anonymous declaration type.</summary>
    [Fact]
    public void AnonymousDeclaredType_DoesNotAcquireNamedAlias()
    {
        var p=Payload();p.Schema=p.Schema.Replace("<xs:element name='v' type='xs:decimal'/>","<xs:element name='v'><xs:simpleType><xs:restriction base='xs:decimal'><xs:maxInclusive value='20'/></xs:restriction></xs:simpleType></xs:element>",StringComparison.Ordinal);using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);
    }
    /// <summary>Legitimate cross-family derivation is deferred rather than misclassified as invalid.</summary>
    [Fact]
    public void DecimalToNamedInteger_IsValidDerivationButUnsupportedRepresentation()
    {
        var p=Payload("integer");p.Schema=p.Schema.Replace("name='v' type='xs:integer'","name='v' type='xs:decimal'",StringComparison.Ordinal);using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(SchemaAssessmentStatus.Unsupported,f.Assessment.Status);Assert.Equal(NativeDecimalUnsupportedReason.ValueType,f.Assessment.UnsupportedReason);Assert.False(f.Input.IsAssessed);
    }
    /// <summary>Known complex simple-content derivation from decimal is deferred, not falsely unrelated.</summary>
    [Fact]
    public void SimpleToComplexSimpleContent_IsUnsupportedRatherThanInvalid()
    {
        var p=Payload(request:"t:Money");p.Schema=p.Schema.Replace("<xs:element name='r'>","<xs:complexType name='Money'><xs:simpleContent><xs:extension base='xs:decimal'/></xs:simpleContent></xs:complexType><xs:element name='r'>",StringComparison.Ordinal);using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(SchemaAssessmentStatus.Unsupported,f.Assessment.Status);Assert.Equal(NativeDecimalUnsupportedReason.ValueType,f.Assessment.UnsupportedReason);
    }
    /// <summary>Complex simple-content types cannot be weakened to their simple base.</summary>
    [Fact]
    public void ComplexSimpleContentToItsBase_IsInvalid()
    {
        var schema=$"<xs:schema xmlns:xs='{Xs}' xmlns:t='{Ns}' targetNamespace='{Ns}'><xs:complexType name='Money'><xs:simpleContent><xs:extension base='xs:decimal'/></xs:simpleContent></xs:complexType><xs:element name='r' type='t:Money'/></xs:schema>";using var f=new Fixture(schema,$"<t:r xmlns:t='{Ns}' xmlns:i='{Xsi}' xmlns:xs='{Xs}' i:type='xs:decimal'>9</t:r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);
    }
    /// <summary>Request prefix spelling is not recognition when its namespace denotes an application attribute.</summary>
    [Fact]
    public void LiteralXsiPrefixInApplicationNamespace_IsNotATypeRequest()
    {
        var p=Payload();p.Schema=p.Schema.Replace("</xs:sequence></xs:complexType>","</xs:sequence><xs:attribute name='type' type='xs:string' form='qualified'/></xs:complexType>",StringComparison.Ordinal);p.Xml=p.Xml.Replace(" i:type='t:Limited'","",StringComparison.Ordinal).Replace("<t:r ",$"<t:r xmlns:xsi='{Ns}' xsi:type='arbitrary' ",StringComparison.Ordinal);using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Single(f.Assessment.AttributeFacts);Assert.Equal("arbitrary",f.Read(f.Attribute([],Ns,"type")).StringValue);Assert.Equal("decimal",f.Input.FactsFor((NativeSessionNode)f.Element([0]))!.NativeType.Name);
    }
    /// <summary>Unsupported and invalid requests publish neither partial node facts nor a snapshot.</summary>
    /// <param name="request">Requested QName.</param>
    /// <param name="expected">Expected status.</param>
    [Theory][InlineData("missing:Type",SchemaAssessmentStatus.Invalid)][InlineData("xs:date",SchemaAssessmentStatus.Unsupported)][InlineData("xs:integer",SchemaAssessmentStatus.Unsupported)]
    public void RefusedRequests_PublishNothing(string request,SchemaAssessmentStatus expected)
    {
        var p=Payload(request:request);using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(expected,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Null(f.Snapshot);Assert.Null(f.Assessment.Evidence);Assert.Empty(f.Assessment.Facts);
    }
    /// <summary>Selected-type query refusal rolls back cache charge before a smaller original target is read.</summary>
    [Fact]
    public void SelectedTypeQueryCapacity_RollbackKeepsNextTargetUsable()
    {
        var p=Payload(value:"10");p.Schema=p.Schema.Replace("name='v' type='xs:decimal'","name='v' type='xs:decimal' maxOccurs='2'",StringComparison.Ordinal);p.Xml=p.Xml.Replace("</t:r>","<t:v i:type='t:Limited'>9</t:v></t:r>",StringComparison.Ordinal);using var session=NativeSchemaNodeDataSession.Create(p.Schema,Options(query:1,cache:1)).Value!;using var document=session.Assess(p.Xml,Context).Value!;Assert.Throws<NotSupportedException>(()=>document.ReadElement([0]));Assert.Equal("9",Text(document.ReadElement([1])));Assert.Equal("9",Text(document.ReadElement([1])));Assert.Contains("10",document.ExportXml());
    }

}
