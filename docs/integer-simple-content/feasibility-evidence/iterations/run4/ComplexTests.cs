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
    private static string Text(NativeScalarValue v)=>v.IntegerValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
    private static string Schema(string facets="<xs:maxExclusive value='5'/>",string baseFacets="",bool nested=false)=>
        $"<xs:schema xmlns:xs='{Xs}' xmlns:t='{Ns}' targetNamespace='{Ns}' elementFormDefault='qualified'><xs:simpleType name='Value'><xs:restriction base='xs:integer'>{baseFacets}</xs:restriction></xs:simpleType><xs:complexType name='Base'><xs:simpleContent><xs:extension base='t:Value'/></xs:simpleContent></xs:complexType><xs:complexType name='Derived'><xs:simpleContent><xs:restriction base='t:Base'>{facets}</xs:restriction></xs:simpleContent></xs:complexType>{(nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='t:Derived'/></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r' type='t:Derived'/>")}</xs:schema>";
    private static string Xml(string value="4",bool nested=false)=>$"<t:r xmlns:t='{Ns}'>{(nested?$"<t:v>{value}</t:v>":value)}</t:r>";
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
        Assert.StartsWith("IC-",id);using var d=JsonDocument.Parse(payload);var r=d.RootElement;var schema=r.GetProperty("schema").GetString()!;var xml=r.GetProperty("instance").GetString()!;var compiled=r.GetProperty("expectedCompile").GetString();var assessed=r.GetProperty("expectedAssessment").GetString();var reason=r.GetProperty("unsupportedReason").GetString();var path=r.GetProperty("path").EnumerateArray().Select(x=>x.GetInt32()).ToImmutableArray();
        if(data)
        {
            var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(compiled,c.Outcome.ToString());if(c.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(c.Value);Assert.Equal("BSK-NATIVE-SCHEMA-"+compiled!.ToUpperInvariant(),Assert.Single(c.Diagnostics).Code);if(reason is not null)Assert.Equal(reason,c.Diagnostics[0].UnsupportedReason.ToString());return;}
            using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(assessed,a.Outcome.ToString());if(a.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(a.Value);if(reason is not null)Assert.Equal(reason,Assert.Single(a.Diagnostics).UnsupportedReason.ToString());return;}
            using var document=a.Value!;var value=document.ReadElement(path);Assert.Equal(NativeScalarKind.Integer,value.Kind);var text=Text(value);Assert.Equal(System.Numerics.BigInteger.Parse(XElement.Parse(xml).Value).ToString(System.Globalization.CultureInfo.InvariantCulture),text);var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));document.Dispose();using var reopened=s.Assess(exported,Context).Value!;Assert.Equal(text,Text(reopened.ReadElement(path)));
        }
        else
        {
            var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(compiled,c.Outcome.ToString());if(c.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(c.Value);Assert.Equal("BSK-NATIVE-SCHEMA-"+compiled!.ToUpperInvariant(),Assert.Single(c.Diagnostics).Code);if(reason is not null)Assert.Equal(reason,c.Diagnostics[0].UnsupportedReason.ToString());return;}
            using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(assessed,a.Outcome.ToString());if(a.Outcome!=NativeSchemaOutcome.Valid){Assert.Null(a.Value);if(reason is not null)Assert.Equal(reason,Assert.Single(a.Diagnostics).UnsupportedReason.ToString());return;}
            using var document=a.Value!;var value=document.HasRootScalar?document.ReadRootScalar():document.ReadScalar(0);Assert.Equal(NativeScalarKind.Integer,value.Kind);var text=Text(value);Assert.Equal(System.Numerics.BigInteger.Parse(XElement.Parse(xml).Value).ToString(System.Globalization.CultureInfo.InvariantCulture),text);var exported=document.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(exported)));document.Dispose();using var reopened=s.Assess(exported,Context).Value!;Assert.Equal(text,Text(reopened.HasRootScalar?reopened.ReadRootScalar():reopened.ReadScalar(0)));
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
        Assert.Same(fact.NativeDeclaration.Type,g);Assert.Equal("Derived",g.Name);Assert.Equal("Base",b.Name);Assert.NotSame(g,b);Assert.Null(c1.Name);Assert.Equal(Ns,c1.NamespaceUri);Assert.NotSame(c1,c0);Assert.Same(c0,c1.DerivationBase);Assert.True(c0.IsInteger);Assert.True(c1.IsInteger);Assert.False(g.IsInteger);Assert.Equal("Value",c0.Name);Assert.False(fact.HasNoTypedValue);Assert.Equal((Ns,"Derived"),node.SchemaTypeAnnotation!.Value);Assert.Same(fact,((ISchemaAnnotatedNode)node).OriginalSchemaFacts);Assert.True(f.Session.Advisor!.OwnsNode(node,fact,f.Input,f.Assessment.Origin));
        f.Run(op=>{var raw=fact.ReadTypedValue(default);Assert.Equal(SchemaResultKind.Supported,raw.Kind);Assert.Same(fact.Type,raw.Value.GoverningType);var guard=op.ReadTypedValue(f.Snapshot!,raw.Value);Assert.Equal(SchemaResultKind.Unsupported,guard.Kind);Assert.Equal("BOSAK-ORIGINAL-DATA-UNSUPPORTED",guard.DiagnosticCode);var value=f.Evaluate(op,node);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.NotSame(evidence.DeclaredType,evidence.ActualType);Assert.Same(f.Provider.TypeComponent(c1),evidence.ActualType);Assert.NotSame(f.Provider.TypeComponent(c0),evidence.ActualType);Assert.NotSame(f.Provider.TypeComponent(b),evidence.ActualType);Assert.True(f.Provider.Owns(evidence.ActualType));Assert.Same(f.Assessment.Origin,evidence.OriginalAssessment);return 0;});Assert.Equal("4",Text(f.Read(node)));
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
        using var f=new Fixture(schema,$"<t:r xmlns:t='{Ns}'><t:a>4</t:a><t:b>4</t:b></t:r>");var a=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!.NativeType;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!.NativeType;Assert.NotSame(a,b);Assert.NotSame(a.ContentType,b.ContentType);Assert.Same(a.DerivationBase,b.DerivationBase);Assert.Same(a.ContentType!.DerivationBase,b.ContentType!.DerivationBase);Assert.Equal("4",Text(f.Read(f.Element([0]))));Assert.Equal("4",Text(f.Read(f.Element([1]))));f.Run(op=>{Assert.True(SchemaAtomics.TryGetProvenance(f.Evaluate(op,f.Element([0])),out var av));Assert.True(SchemaAtomics.TryGetProvenance(f.Evaluate(op,f.Element([1])),out var bv));Assert.Same(f.Provider.TypeComponent(a.ContentType),av!.ActualType);Assert.Same(f.Provider.TypeComponent(b.ContentType),bv!.ActualType);Assert.NotSame(av.ActualType,bv.ActualType);return 0;});
    }
    /// <summary>Typed identity independently authenticates actual anonymous C1 without rebinding data-profile episodes.</summary>
    [Fact]
    public void TypedIdentity_HasItsOwnOriginalEvidence()
    {
        var compiled=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:1024).Value!;var provider=new NativeSchemaProvider(compiled);using var session=SchemaQuerySession.CompileXPath("$value",new(1,32,1,256,256),assessor:provider).Value;using var input=NativeSchemaInput.Capture(Xml()).Value!;var a=(NativeSchemaProvider.Assessment)session.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);Assert.Equal(SchemaAssessmentStatus.Valid,a.Status);var fact=a.ElementFacts[0];using var snapshot=session.OpenSnapshot(input,input,a.Origin).Value;
        new NativeSessionRunner(session).Run([snapshot],op=>{var raw=fact.ReadTypedValue(default);var admitted=op.ReadTypedValue(snapshot,raw.Value);Assert.Equal(SchemaResultKind.Supported,admitted.Kind);var value=op.Evaluate(new Bosak.XPath.Runtime.Vm.EvaluationContext().WithVariable("value",admitted.Value));Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(a.Origin,evidence.OriginalAssessment);Assert.NotSame(evidence.DeclaredType,evidence.ActualType);Assert.Same(provider.TypeComponent(fact.NativeType.ContentType!),evidence.ActualType);return 0;});
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
    {using var s=NativeSchemaNodeDataSession.Create(Schema(),Options).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess(Xml("5"),Context).Outcome);using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>s.Assess(Xml(),Context,cancel.Token));using var d=s.Assess(Xml(),Context).Value!;var value=d.ReadElement([]);Assert.Equal("4",Text(value));s.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadElement([]));Assert.Equal("4",Text(value));}
    /// <summary>Dependency cycles are refused eagerly even when their type has no root use.</summary>
    /// <param name="unused">Uses the valid base as root instead.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void Cycle_IsOutsideOneLevelProfile(bool unused)
    {
        var schema=Schema().Replace("<xs:restriction base='t:Base'><xs:maxExclusive value='5'","<xs:restriction base='t:Derived'><xs:maxExclusive value='5'",StringComparison.Ordinal);if(unused)schema=schema.Replace("<xs:element name='r' type='t:Derived'/>","<xs:element name='r' type='t:Base'/>",StringComparison.Ordinal);var r=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Unsupported,r.Outcome);Assert.Equal(NativeDecimalUnsupportedReason.DeclarationShape,r.UnsupportedReason);Assert.Null(r.Value);
    }
    /// <summary>Raw facet capacity is checked before whitespace collapse.</summary>
    /// <param name="length">Parsed raw value length.</param>
    [Theory][InlineData(255)][InlineData(256)][InlineData(257)]
    public void RawFacetCapacity_RemainsBounded(int length)
    {
        var schema=Schema("<xs:maxInclusive value='"+new string(' ',length-1)+"5'/>");var r=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024);Assert.Equal(length<=256?NativeOutcome.Valid:NativeOutcome.Unsupported,r.Outcome);if(length>256)Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,r.UnsupportedReason);
    }
    /// <summary>Query refusal rolls back cache accounting before another original target is read.</summary>
    [Fact]
    public void QueryCapacity_RollbackPreservesNextTarget()
    {
        var schema=Schema("",nested:true).Replace("name='v' type='t:Derived'","name='v' type='t:Derived' maxOccurs='2'",StringComparison.Ordinal);var xml=Xml("44",nested:true).Replace("</t:r>","<t:v>4</t:v></t:r>",StringComparison.Ordinal);using var s=NativeSchemaNodeDataSession.Create(schema,new(256,256,1024,65536,1,1,1,1,256)).Value!;using var d=s.Assess(xml,Context).Value!;Assert.Throws<NotSupportedException>(()=>d.ReadElement([0]));Assert.Equal("4",Text(d.ReadElement([1])));Assert.Equal("4",Text(d.ReadElement([1])));Assert.Contains("44",d.ExportXml());
    }

    /// <summary>Unqualified base references retain no-namespace identity in both existing routes.</summary>
    /// <param name="nested">Selects the nested terminal.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void NoTargetNamespace_UsesUnqualifiedComponents(bool nested)
    {
        var schema=Schema(nested:nested).Replace($" xmlns:t='{Ns}'", "",StringComparison.Ordinal).Replace($" targetNamespace='{Ns}'", "",StringComparison.Ordinal).Replace("t:Value","Value",StringComparison.Ordinal).Replace("t:Base","Base",StringComparison.Ordinal).Replace("t:Derived","Derived",StringComparison.Ordinal);var xml=nested?"<r><v>4</v></r>":"<r>4</r>";using var f=new Fixture(schema,xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=f.Element(nested?[0]:[]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.Equal("",fact.NativeType.NamespaceUri);Assert.Equal("",fact.NativeType.ContentType!.NamespaceUri);Assert.Equal("4",Text(f.Read(node)));f.Run(op=>{Assert.True(SchemaAtomics.TryGetProvenance(f.Evaluate(op,node),out var evidence));Assert.Same(f.Provider.TypeComponent(fact.NativeType.ContentType),evidence!.ActualType);return 0;});
    }
    /// <summary>Native coefficient and scale limits remain capacity refusals rather than invalid facets.</summary>
    /// <param name="scale">Selects scale instead of coefficient capacity.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void NativeFacetCapacity_RemainsUnsupported(bool scale)
    {
        var literal=scale?"00005":"1234567890";var schema=Schema("<xs:maxInclusive value='"+literal+"'/>");var r=NativeDecimalSchema.Compile(schema,new(4,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Unsupported,r.Outcome);Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,r.UnsupportedReason);Assert.Null(r.Value);
    }
    /// <summary>Reassessment reuses compiled C1 identity while creating a new original assessment episode.</summary>
    [Fact]
    public void NewEpisode_ReusesOnlyCompiledTypeIdentity()
    {
        using var f=new Fixture(Schema(),Xml());var node=f.Element([]);ISchemaAtomicProvenance? first=null;f.Run(op=>{Assert.True(SchemaAtomics.TryGetProvenance(f.Evaluate(op,node),out first));return 0;});using var next=NativeSchemaInput.Capture(Xml("4")).Value!;var assessment=(NativeSchemaProvider.Assessment)f.Session.Advisor!.Assess(new(next,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);Assert.Equal(SchemaAssessmentStatus.Valid,assessment.Status);using var snapshot=f.Session.OpenSnapshot(next,next,assessment.Origin).Value;new NativeSessionRunner(f.Session).Run([snapshot],op=>{var value=op.Evaluate(new Bosak.XPath.Runtime.Vm.EvaluationContext().WithVariable("node",XdmValue.FromNode(next.Root)));Assert.True(SchemaAtomics.TryGetProvenance(value,out var second));Assert.Same(first!.ActualType,second!.ActualType);Assert.Same(first.DeclaredType,second.DeclaredType);Assert.NotSame(first.OriginalAssessment,second.OriginalAssessment);Assert.Same(assessment.Origin,second.OriginalAssessment);return 0;});
    }

    /// <summary>Anonymous root/local owners retain actual integer content and original evidence.</summary>
    /// <param name="nested">Selects local owner.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void IntegerAnonymousOwners_AreNative(bool nested)
    {
        var x=XDocument.Parse(Schema(nested:nested));XNamespace xs=Xs;
        var derived=x.Root!.Elements(xs+"complexType").Single(e=>(string?)e.Attribute("name")=="Derived");
        var owner=x.Descendants(xs+"element").Single(e=>(string?)e.Attribute("type")=="t:Derived");
        owner.Attribute("type")!.Remove();var inline=new XElement(derived);inline.Attribute("name")!.Remove();owner.Add(inline);derived.Remove();
        using var f=new Fixture(x.ToString(),Xml(nested:nested));var node=f.Element(nested?[0]:[]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;
        Assert.Null(fact.NativeType.Name);Assert.Null(fact.NativeType.ContentType!.Name);Assert.True(fact.NativeType.ContentType.IsInteger);Assert.Same(fact.NativeType.DerivationBase!.ContentType,fact.NativeType.ContentType.DerivationBase);
        f.Run(op=>{var value=f.Evaluate(op,node);Assert.Equal(XdmValueKind.Integer,value.Kind);Assert.True(SchemaAtomics.TryGetProvenance(value,out var ev));Assert.Same(f.Provider.TypeComponent(fact.NativeType.ContentType),ev!.ActualType);Assert.Same(fact.Type,ev.DeclaredType);return 0;});
    }
    /// <summary>Primitive and named simple extension content retain the original C0 component.</summary>
    /// <param name="named">Uses a named integer simple restriction.</param>
    /// <param name="nested">Selects nested scalar.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void IntegerExtension_PreservesOriginalAtomicBase(bool named,bool nested)
    {
        var schema=Schema("",named?"<xs:minInclusive value='0'/>":"",nested).Replace("type='t:Derived'","type='t:Base'",StringComparison.Ordinal);
        if(!named)schema=schema.Replace("<xs:extension base='t:Value'","<xs:extension base='xs:integer'",StringComparison.Ordinal);
        using var f=new Fixture(schema,Xml(nested:nested));var node=f.Element(nested?[0]:[]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;var c0=fact.NativeType.ContentType!;
        Assert.True(c0.IsInteger);Assert.Equal(named?"Value":"integer",c0.Name);Assert.False(fact.NativeType.HasNoTypedValue);
        f.Run(op=>{var value=f.Evaluate(op,node);Assert.Equal(XdmValueKind.Integer,value.Kind);Assert.True(SchemaAtomics.TryGetProvenance(value,out var ev));Assert.Same(f.Provider.TypeComponent(c0),ev!.ActualType);Assert.Same(fact.Type,ev.DeclaredType);Assert.NotSame(ev.ActualType,ev.DeclaredType);return 0;});
    }
    /// <summary>Very large integer atomics retain exact values and actual component identity.</summary>
    /// <param name="nested">Selects nested terminal.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void IntegerAtomic_ExceedsDecimalAndInt64(bool nested)
    {
        const string literal="12345678901234567890123456789012345678901234567890";
        using var f=new Fixture(Schema("",nested:nested),Xml(literal,nested));var node=f.Element(nested?[0]:[]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;
        f.Run(op=>{var value=f.Evaluate(op,node);Assert.Equal(XdmValueKind.Integer,value.Kind);Assert.Equal(System.Numerics.BigInteger.Parse(literal),value.BigIntegerValue);Assert.True(SchemaAtomics.TryGetProvenance(value,out var ev));Assert.Same(f.Provider.TypeComponent(fact.NativeType.ContentType!),ev!.ActualType);return 0;});
        Assert.Equal(literal,Text(f.Read(node)));
    }
    /// <summary>Actual ancestry does not expand complex xsi:type substitution.</summary>
    /// <param name="mode">Same component, derived substitution or weakening.</param>
    [Theory][InlineData(0)][InlineData(1)][InlineData(2)]
    public void IntegerComplexSelection_RemainsBounded(int mode)
    {
        var schema=Schema();if(mode==1)schema=schema.Replace("type='t:Derived'","type='t:Base'",StringComparison.Ordinal);
        var xml=Xml().Replace("<t:r ","<t:r xmlns:xsi='"+Xsi+"' xsi:type='t:"+(mode==2?"Base":"Derived")+"' ",StringComparison.Ordinal);
        using var session=NativeSchemaNodeDataSession.Create(schema,Options).Value!;var result=session.Assess(xml,Context);Assert.Equal(mode==0?NativeSchemaOutcome.Valid:mode==1?NativeSchemaOutcome.Unsupported:NativeSchemaOutcome.Invalid,result.Outcome);if(mode==0)result.Value!.Dispose();else Assert.Null(result.Value);
    }
    /// <summary>Relative target namespaces retain original integer type identity.</summary>
    /// <param name="nested">Selects local terminal.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void RelativeNamespace_IntegerContext(bool nested)
    {using var f=new Fixture(Schema(nested:nested).Replace(Ns,"relative/integer",StringComparison.Ordinal),Xml(nested:nested).Replace(Ns,"relative/integer",StringComparison.Ordinal));var fact=f.Input.FactsFor((NativeSessionNode)f.Element(nested?[0]:[]))!;Assert.Equal("relative/integer",fact.NativeType.ContentType!.NamespaceUri);Assert.Equal("4",Text(f.Read(f.Element(nested?[0]:[]))));}

}
