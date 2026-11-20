// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Qualifies actual native original-node atomization in an isolated candidate.
// SPECIAL NOTES        : Native facts with signed public Core; Internal authority controls; no full XSD conformance claim.
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
using Bosak.Schema;
using Bosak.XPath.Api;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;

namespace Bosak.Schema.Tests.NodeData;

/// <summary>Exercises actual native data profiles and original tuple guards.</summary>
public sealed class NativeDataTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal' maxOccurs='2'/></xs:sequence><xs:attribute name='id' type='xs:string'/></xs:complexType></xs:element></xs:schema>";
    private const string Xml="<r id='A'><v>123</v><v>1</v></r>";
    private static string Text(NativeScalarValue value)=>value.Kind switch{NativeScalarKind.String=>value.StringValue,NativeScalarKind.Boolean=>value.BooleanValue?"true":"false",NativeScalarKind.Integer=>value.IntegerValue.ToString(CultureInfo.InvariantCulture),_=>value.DecimalValue.ToInvariantString(256)};
    private static ImmutableArray<int> Path(JsonElement value)=>value.EnumerateArray().Select(v=>v.GetInt32()).ToImmutableArray();
    /// <summary>Gets actual compiled-native payload and owner cases.</summary>
    /// <returns>Stable identity and original XML/schema payloads.</returns>
    public static IEnumerable<object[]> Cases(){using var data=JsonDocument.Parse(File.ReadAllText(System.IO.Path.Combine(AppContext.BaseDirectory,"TestData", "NodeData", "cases.json")));foreach(var row in data.RootElement.EnumerateArray())yield return [row.GetProperty("id").GetString()!,row.GetRawText()];}
    /// <summary>Original element/attribute atomization preserves exact values and original type evidence.</summary>
    /// <param name="id">Native case identity.</param>
    /// <param name="payload">Schema, instance and selected original targets.</param>
    [Theory][MemberData(nameof(Cases))]
    public void ActualNativeFacts_AtomizeAndDetach(string id,string payload)
    {
        Assert.StartsWith("ND-",id);using var data=JsonDocument.Parse(payload);var row=data.RootElement;using var f=new Fixture(row.GetProperty("schema").GetString()!,row.GetProperty("xml").GetString()!);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var element=f.Element(Path(row.GetProperty("path")));var value=f.Read(element);Assert.Equal(row.GetProperty("kind").GetString(),value.Kind.ToString().ToLowerInvariant());Assert.Equal(row.GetProperty("expected").GetString(),Text(value));
        var facts=f.Input.FactsFor((NativeSessionNode)element)!;
        if(row.GetProperty("mode").GetString()=="anonymous")Assert.Null(element.SchemaTypeAnnotation);
        f.Run(op=>{var typed=f.Evaluate(op,element);Assert.True(SchemaAtomics.TryGetProvenance(typed,out var evidence));Assert.Same(facts.Type,evidence!.DeclaredType);if(row.GetProperty("mode").GetString()!="complex")Assert.Same(facts.Type,evidence.ActualType);else Assert.NotSame(facts.Type,evidence.ActualType);Assert.Same(f.Assessment.Origin,evidence.OriginalAssessment);return 0;});
        foreach(var attr in row.GetProperty("attrs").EnumerateArray()){var node=f.Attribute(Path(attr.GetProperty("path")),attr.GetProperty("namespace").GetString()!,attr.GetProperty("name").GetString()!);Assert.Equal(attr.GetProperty("expected").GetString(),Text(f.Read(node)));Assert.Same(f.Element(Path(attr.GetProperty("path"))),node.Parent);}
        f.Dispose();Assert.Equal(row.GetProperty("expected").GetString(),Text(value));
    }
    /// <summary>The candidate mode accepts only its exact data profile and unchanged scalar defaults.</summary>
    [Fact]
    public void ProviderModes_KeepSeparateEnvironments()
    {
        using var f=new Fixture(Schema,Xml);Assert.Equal("bosak-original-node-data-v1",f.Session.Environment.BehaviorProfile);Assert.Equal("bosak-original-node-data-descriptor-v1",f.Session.Environment.DescriptorFormatVersion);Assert.Empty(f.Session.Environment.StaticContext.Namespaces);
        Assert.Equal(SchemaResultKind.Unsupported,SchemaQuerySession.CompileXPath("$value",f.Limits,assessor:f.Provider).Kind);
        var compiled=NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:1024).Value!;var scalar=new NativeSchemaProvider(compiled);Assert.Equal(SchemaResultKind.Unsupported,SchemaQuerySession.CompileXPath("data($node)",f.Limits,assessor:scalar).Kind);using var ordinary=SchemaQuerySession.CompileXPath("$value",f.Limits,assessor:scalar).Value;
    }
    /// <summary>General path/predicate/arithmetic syntax remains unavailable in the bounded public data profile.</summary>
    /// <param name="expression">Unavailable expression.</param>
    [Theory][InlineData("data($node/v)")][InlineData("data($node)[1]")][InlineData("$node/v")][InlineData("data($node) + 1")][InlineData("nilled($node)")]
    public void GeneralQueries_AreUnsupported(string expression){using var f=new Fixture(Schema,Xml);Assert.Equal(SchemaResultKind.Unsupported,SchemaQuerySession.CompileXPath(expression,f.Limits,assessor:f.Provider).Kind);}
    /// <summary>Static namespace and default-element context changes do not match the pinned operation.</summary>
    /// <param name="defaultNamespace">Selects otherwise hidden default namespace alteration.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void AlteredStaticContext_IsRefused(bool defaultNamespace)
    {
        using var f=new Fixture(Schema,Xml);var options=defaultNamespace?new CompileOptions{DefaultElementNamespace="urn:changed"}:new CompileOptions{Namespaces=new Dictionary<string,string>{{"x","urn:changed"}}};Assert.Equal(SchemaResultKind.Unsupported,SchemaQuerySession.CompileXPath("data($node)",f.Limits,options,assessor:f.Provider).Kind);
    }
    /// <summary>A forged descriptive environment is rejected before its getters can act as authority.</summary>
    [Fact]
    public void ForgedEnvironment_IsRefusedBeforeGetters(){using var f=new Fixture(Schema,Xml);Assert.Equal(SchemaResultKind.Unsupported,f.Provider.Bind(new BombEnvironment(),default).Kind);}
    private sealed class BombEnvironment : IQuerySemanticEnvironment
    {
        /// <inheritdoc/>
        public QueryHostLanguage HostLanguage=>throw new InvalidOperationException("Foreign getter.");
        /// <inheritdoc/>
        public string HostVersion=>throw new InvalidOperationException("Foreign getter.");
        /// <inheritdoc/>
        public Uri XdmSpecificationRevision=>throw new InvalidOperationException("Foreign getter.");
        /// <inheritdoc/>
        public string BehaviorProfile=>throw new InvalidOperationException("Foreign getter.");
        /// <inheritdoc/>
        public IQueryStaticContext StaticContext=>throw new InvalidOperationException("Foreign getter.");
        /// <inheritdoc/>
        public string DescriptorFormatVersion=>throw new InvalidOperationException("Foreign getter.");
        /// <inheritdoc/>
        public string StaticContextFingerprint=>throw new InvalidOperationException("Foreign getter.");
    }
    /// <summary>Foreign original nodes and copied XML cannot transfer original assessment ownership.</summary>
    [Fact]
    public void ForeignAndCopiedNodes_AreRejected()
    {
        using var a=new Fixture(Schema,Xml);using var b=new Fixture(Schema,Xml);var node=a.Element([0]);Assert.Throws<ArgumentException>(()=>b.Run(op=>b.Evaluate(op,node)));
        using var copy=NativeSchemaInput.Capture(Xml).Value!;Assert.Throws<ArgumentException>(()=>a.Run(op=>a.Evaluate(op,copy.ElementNodes[1])));Assert.Equal("123",Text(b.Read(b.Element([0]))));
    }
    /// <summary>Scalar-session documents cannot be admitted as original data-session episodes.</summary>
    [Fact]
    public void OldScalarEpisode_CannotBeRebound()
    {
        using var f=new Fixture(Schema,Xml);var p=new NativeSchemaProvider(NativeDecimalSchema.Compile(Schema,new(256,256),maxStringCharacters:1024).Value!);using var scalar=SchemaQuerySession.CompileXPath("$value",f.Limits,assessor:p).Value;using var input=NativeSchemaInput.Capture(Xml).Value!;var assessment=(NativeSchemaProvider.Assessment)scalar.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
        Assert.Throws<ArgumentException>(()=>f.Session.OpenSnapshot(input,input,assessment.Origin));
    }
    private sealed record Episode : ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    /// <summary>Default, negative, deep and missing targets refuse without disturbing later reads.</summary>
    [Fact]
    public void InvalidTargets_RefuseAndRecover()
    {
        using var f=new Fixture(Schema,Xml);Assert.Throws<ArgumentException>(()=>f.Element(default));Assert.Throws<ArgumentOutOfRangeException>(()=>f.Element([-1]));Assert.Throws<ArgumentOutOfRangeException>(()=>f.Element([9]));Assert.Throws<ArgumentOutOfRangeException>(()=>f.Element([0,0,0,0]));Assert.Throws<ArgumentException>(()=>f.Attribute([],"","missing"));Assert.Throws<ArgumentException>(()=>f.Attribute([],"","x:y"));Assert.Equal("123",Text(f.Read(f.Element([0]))));
    }
    /// <summary>A preserved hint is not a typed application attribute or a lexical fallback.</summary>
    [Fact]
    public void UnassessedHintAttribute_StaysUnsupported()
    {
        using var f=new Fixture(Schema,"<r xmlns:h='http://www.w3.org/2001/XMLSchema-instance' h:noNamespaceSchemaLocation='missing'><v>1</v></r>");var node=f.Attribute([],"http://www.w3.org/2001/XMLSchema-instance","noNamespaceSchemaLocation");Assert.Throws<NotSupportedException>(()=>f.Read(node));Assert.Equal("1",Text(f.Read(f.Element([0]))));
    }
    /// <summary>Element-only targets retain no typed value while scalar targets remain usable.</summary>
    [Fact]
    public void ElementOnlyContainer_ReportsFoty0012()
    {
        using var f=new Fixture(Schema,Xml);var error=Assert.Throws<InvalidOperationException>(()=>f.Read(f.Element([])));Assert.Contains("FOTY0012",error.Message);Assert.Equal("123",Text(f.Read(f.Element([0]))));
    }
    /// <summary>Native-invalid XML does not publish a snapshot or partial original facts.</summary>
    [Fact]
    public void InvalidInstance_DoesNotAdmitSnapshot()
    {
        using var f=new Fixture(Schema,"<r><v>NaN</v></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Null(f.Snapshot);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.ElementFacts);Assert.Throws<NotSupportedException>(()=>f.Read(f.Element([0])));
    }
    /// <summary>Repeated targets reuse a bounded cache and a distinct target refuses without poisoning the first.</summary>
    [Fact]
    public void CacheLimit_ReusesSameTargetAndRefusesDistinct()
    {
        using var f=new Fixture(Schema,Xml,cache:1);var first=f.Element([0]);Assert.Equal("123",Text(f.Read(first)));Assert.Equal("123",Text(f.Read(first)));Assert.Throws<NotSupportedException>(()=>f.Read(f.Element([1])));Assert.Equal("123",Text(f.Read(first)));
    }
    /// <summary>Failed large-value query admission rolls back its cache charge so a small target still works.</summary>
    [Fact]
    public void NumericRefusal_RollsBackCacheCharge()
    {
        using var f=new Fixture(Schema,Xml,cache:1,digits:2);Assert.Throws<NotSupportedException>(()=>f.Read(f.Element([0])));Assert.Equal("1",Text(f.Read(f.Element([1]))));
    }
    /// <summary>Root and descendant application attributes are distinct cached original targets.</summary>
    [Fact]
    public void ElementAndAttribute_HaveSeparateCacheEntries()
    {
        using var f=new Fixture(Schema,Xml,cache:1);Assert.Equal("A",Text(f.Read(f.Attribute([],"","id"))));Assert.Throws<NotSupportedException>(()=>f.Read(f.Element([0])));Assert.Equal("A",Text(f.Read(f.Attribute([],"","id"))));
    }
    /// <summary>Cancellation and overlapping runner work refuse without poisoning an original data read.</summary>
    [Fact]
    public void CancellationAndOverlap_PreserveRecovery()
    {
        using var f=new Fixture(Schema,Xml);using var cancelled=new CancellationTokenSource();cancelled.Cancel();Assert.Throws<OperationCanceledException>(()=>f.Read(f.Element([0]),cancelled.Token));f.Run(op=>{Assert.Throws<NotSupportedException>(()=>f.Read(f.Element([0])));return 0;});Assert.Equal("123",Text(f.Read(f.Element([0]))));
    }
    /// <summary>Snapshot revocation expires provenance but detached scalar values survive owner closure.</summary>
    [Fact]
    public void SnapshotClosure_RevokesEvidenceAndPreservesDetachedValues()
    {
        using var f=new Fixture(Schema,Xml);var node=f.Element([0]);var detached=f.Read(node);var value=f.Run(op=>f.Evaluate(op,node));Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));f.Snapshot!.Dispose();Assert.Throws<ObjectDisposedException>(()=>evidence!.ActualType);Assert.Throws<ObjectDisposedException>(()=>f.Read(node));f.Dispose();Assert.Equal("123",Text(detached));
    }
    /// <summary>Admission quotas are enforced before a second capture can borrow data-session resources.</summary>
    [Fact]
    public void AdmissionQuota_RefusesAnotherSnapshot()
    {
        using var f=new Fixture(Schema,Xml,documents:1);using var input=NativeSchemaInput.Capture(Xml).Value!;var result=(NativeSchemaProvider.Assessment)f.Session.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);Assert.Throws<InvalidOperationException>(()=>f.Session.OpenSnapshot(input,input,result.Origin));Assert.Equal("123",Text(f.Read(f.Element([0]))));
    }
    /// <summary>Authentic facts cannot be associated with a different original node even inside one input.</summary>
    [Fact]
    public void AliasedFacts_AreRejectedByNativeIssuer()
    {
        using var f=new Fixture(Schema,Xml);var a=f.Element([0]);var b=f.Element([1]);var facts=f.Input.FactsFor((NativeSessionNode)a)!;var advisor=(NativeSchemaProvider.Advisor)f.Session.Advisor!;Assert.True(advisor.OwnsNode(a,facts,f.Input,f.Assessment.Origin));Assert.False(advisor.OwnsNode(b,facts,f.Input,f.Assessment.Origin));
    }
    /// <summary>Equal paths in two native captures select only the admitted original snapshot.</summary>
    [Fact]
    public void SameSessionCaptures_RequireSelectedOriginalSnapshot()
    {
        using var f=new Fixture(Schema,Xml);using var input=NativeSchemaInput.Capture(Xml).Value!;var result=(NativeSchemaProvider.Assessment)f.Session.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);using var snapshot=f.Session.OpenSnapshot(input,input,result.Origin).Value;var node=input.ElementNodes[1];Assert.Throws<ArgumentException>(()=>f.Run(op=>f.Evaluate(op,node)));var value=f.Runner.Run([snapshot],op=>NativeScalarValue.FromValue(f.Evaluate(op,node)));Assert.Equal("123",Text(value));Assert.NotSame(f.Assessment.Origin,result.Origin);
    }
    /// <summary>A scale refusal also rolls back its original-target cache charge.</summary>
    [Fact]
    public void ScaleRefusal_RollsBackCacheCharge()
    {
        using var f=new Fixture(Schema,"<r><v>1.23</v><v>1</v></r>",cache:1,scale:1);Assert.Throws<NotSupportedException>(()=>f.Read(f.Element([0])));Assert.Equal("1",Text(f.Read(f.Element([1]))));
    }
    /// <summary>Failed bounded XML export preserves original data reads and lexical export.</summary>
    [Fact]
    public void FailedExport_DoesNotPoisonOriginalData()
    {
        using var f=new Fixture(Schema,Xml);Assert.Throws<NotSupportedException>(()=>NativeBoundedTextWriter.Export(f.Input,10,default));Assert.Equal("123",Text(f.Read(f.Element([0]))));Assert.Contains("123",NativeBoundedTextWriter.Export(f.Input,65536,default));
    }
}
