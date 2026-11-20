// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Probes nested original node facts through published Core APIs.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 19-11-2026     | Probe public original attribute facts, typed reads and ownership refusal                   |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Keep synthetic attribute declaration identity distinct from type identities                |
//                      | Charles Korthout | 0.4   | 20-11-2026     | Qualify complex simple-content root facts with separate actual decimal types               |
//                      | Charles Korthout | 0.5   | 20-11-2026     | Probe nested navigation, container refusal and original leaf facts                         |
//                      | Charles Korthout | 0.6   | 20-11-2026     | Bind each original nested attribute fact to its exact captured owner node                  |
//                      | Charles Korthout | 0.7   | 20-11-2026     | Probe nested complex scalar terminals and depth-four original attribute owners             |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Xml.Linq;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Providers.Xml;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace AttributeTransportProbe;

/// <summary>Qualifies public typed identity with a synthetic issuer, not an XSD validator.</summary>
public sealed class TransportTests
{
    internal static XdmValue Plain(int kind)=>kind switch
    {
        0=>XdmValue.FromString(" INV-001 "),1=>XdmValue.FromString(""),2=>XdmValue.FromString("\U0001F600e\u0301\r\n\t"),
        3=>XdmValue.FromBoolean(true),4=>XdmValue.FromBoolean(false),
        5=>XdmValue.FromInteger(BigInteger.Parse("12345678901234567890123456789012345678901234567890",CultureInfo.InvariantCulture)),
        6=>XdmValue.FromInteger(BigInteger.Parse("-12345678901234567890123456789012345678901234567890",CultureInfo.InvariantCulture)),
        _=>XdmValue.FromDecimal(new XdmExactDecimal(12500,3,256,256))
    };
    internal static void SamePayload(XdmValue expected,XdmValue actual)
    {
        Assert.Equal(expected.Kind,actual.Kind);
        if(expected.Kind==XdmValueKind.String)Assert.Equal(expected.StringValue,actual.StringValue);
        else if(expected.Kind==XdmValueKind.Boolean)Assert.Equal(expected.BooleanValue,actual.BooleanValue);
        else if(expected.Kind==XdmValueKind.Decimal)Assert.Equal(expected.ExactDecimalValue,actual.ExactDecimalValue);
        else Assert.Equal(expected.BigIntegerValue,actual.BigIntegerValue);
    }

    /// <summary>Checks payload and original type/origin evidence through public read and evaluation.</summary>
    /// <param name="kind">Plain atomic test payload.</param>
    [Theory]
    [InlineData(0)][InlineData(1)][InlineData(2)][InlineData(3)][InlineData(4)][InlineData(5)][InlineData(6)][InlineData(7)]
    /// <inheritdoc/>
    public void PublicReadAndEvaluate_RetainsPayloadAndOriginalEvidence(int kind)
    {
        using var f=new Fixture();var plain=Plain(kind);var issued=f.Issue(plain);
        var detached=f.Session.Run([f.Snapshot],op=>
        {
            var read=op.ReadTypedValue(f.Snapshot,issued);Assert.Equal(SchemaResultKind.Supported,read.Kind);
            var value=op.Evaluate(new EvaluationContext().WithVariable("value",read.Value));SamePayload(plain,value);
            Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));
            Assert.Same(f.Provider.Declared,evidence!.DeclaredType);Assert.Same(f.Provider.Actual,evidence.ActualType);Assert.Same(f.Provider.Advisor!.Origin,evidence.OriginalAssessment);
            Assert.Same(f.Session.Environment,f.Provider.Advisor.QueryEnvironment);
            return value.Kind switch { XdmValueKind.String=>value.StringValue,XdmValueKind.Boolean=>value.BooleanValue?"true":"false",_=>value.Kind==XdmValueKind.Decimal?value.ExactDecimalValue.ToInvariantString(256):value.BigIntegerValue.ToString(CultureInfo.InvariantCulture) };
        });
        f.Dispose();Assert.NotNull(detached);
    }

    /// <summary>Rejects foreign issuance and a foreign operation's authenticated value.</summary>
    [Fact]
    /// <inheritdoc/>
    public void ForeignValueAndSnapshot_AreRefused()
    {
        using var a=new Fixture();using var b=new Fixture();var issued=a.Issue(Plain(0));
        var value=a.Session.Run([a.Snapshot],op=>op.ReadTypedValue(a.Snapshot,issued).Value);
        b.Session.Run([b.Snapshot],op=>
        {
            Assert.Throws<ArgumentException>(()=>op.ReadTypedValue(a.Snapshot,issued));
            Assert.Throws<ArgumentException>(()=>op.ReadTypedValue(b.Snapshot,issued));
            Assert.Throws<ArgumentException>(()=>op.Evaluate(new EvaluationContext().WithVariable("value",value)));return 0;
        });
    }

    /// <summary>Scalar evidence remains snapshot-bound and detached primitive strings survive.</summary>
    [Fact]
    /// <inheritdoc/>
    public void SnapshotEvidence_Expires_AndSessionDisposalRefusesWork()
    {
        using var f=new Fixture();var issued=f.Issue(Plain(0));ISchemaAtomicProvenance? evidence=null;string? detached=null;
        f.Session.Run([f.Snapshot],op=>
        {
            var v=op.ReadTypedValue(f.Snapshot,issued).Value;Assert.True(SchemaAtomics.TryGetProvenance(v,out evidence));detached=v.StringValue;return 0;
        });
        Assert.Same(f.Provider.Actual,evidence!.ActualType);
        f.Snapshot.Dispose();
        Assert.Throws<ObjectDisposedException>(()=>evidence!.ActualType);
        f.Session.Dispose();Assert.Throws<ObjectDisposedException>(()=>f.Session.Run([f.Snapshot],_=>0));Assert.Equal(" INV-001 ",detached);
    }

    /// <summary>Complete typed-container evidence is operation-local while its plain atomic is detached.</summary>
    [Fact]
    public void CompleteContainer_ViewExpiresAtOperationEnd()
    {
        using var f=new Fixture();var issued=f.Issue(Plain(2));ISchemaAtomicProvenance? evidence=null;string? text=null;
        f.Session.Run([f.Snapshot],op=>
        {
            var view=op.ReadTypedContainer(f.Snapshot,issued);Assert.Equal(SchemaResultKind.Supported,view.Kind);
            Assert.True(view.Value.TryGetSingleAtomic(out var item));evidence=item.TypeEvidence;text=item.Value.StringValue;
            Assert.Same(f.Provider.Actual,evidence.ActualType);return 0;
        });
        Assert.Throws<ObjectDisposedException>(()=>evidence!.ActualType);Assert.Equal(Plain(2).StringValue,text);
    }

    /// <summary>Checks transport atom admission and original input liveness without timing assumptions.</summary>
    [Fact]
    /// <inheritdoc/>
    public void RevokedInput_RefusesReads()
    {
        using var f=new Fixture();var issued=f.Issue(Plain(3));f.Input.Dispose();
        Assert.Throws<ObjectDisposedException>(()=>f.Session.Run([f.Snapshot],op=>op.ReadTypedValue(f.Snapshot,issued)));
    }

    /// <summary>Plain values cannot manufacture the issuer evidence required by typed identity.</summary>
    [Fact]
    /// <inheritdoc/>
    public void PlainAtomic_CannotAcquireAuthority()
    {
        using var f=new Fixture();f.Session.Run([f.Snapshot],op=>{Assert.Throws<ArgumentException>(()=>op.Evaluate(new EvaluationContext().WithVariable("value",Plain(0))));return 0;});
    }

    /// <summary>Identifies the actually loaded published packages rather than a source override.</summary>
    [Fact]
    /// <inheritdoc/>
    public void Runtime_UsesExpectedPublishedVersion()
    {
        var expected=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"expected-version.txt")).Trim();
        Assert.StartsWith(expected+"+",typeof(SchemaQuerySession).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion);
    }
}

internal sealed class Fixture:IDisposable
{
    internal readonly ProbeInput Input;
    internal readonly ProbeProvider Provider=new();
    internal readonly SchemaQuerySession Session;
    internal readonly SchemaQuerySnapshot Snapshot;
    internal Fixture(string expression="$value", bool nested=false, bool nestedAttributes=false, bool simpleTerminals=false)
    {
        Input=new(nested,nestedAttributes,simpleTerminals);
        Session=SchemaQuerySession.CompileXPath(expression,new(2,8,1,256,256),assessor:Provider).Value;
        Provider.Advisor!.Input=Input;Provider.Advisor.Origin=new(Input.Source);
        Snapshot=Session.OpenSnapshot(Input,Input,Provider.Advisor.Origin).Value;
    }
    internal ProbeAttributeNode RootValue(XdmValue value)
    {
        var facts=new ProbeFacts(Provider.Declared,Provider.RootDeclaration,Provider.Advisor!.Origin,Issue(value));
        Input.RootNode.Facts=facts;Provider.Advisor.RegisterNode(Input.RootNode,facts);return Input.RootNode;
    }
    internal ProbeAttributeNode Attribute(XdmValue value)
    {
        var issued=Issue(value);var facts=new ProbeFacts(Provider.Declared,Provider.AttributeDeclaration,Provider.Advisor!.Origin,issued);
        Input.Attribute.Facts=facts;Provider.Advisor.RegisterNode(Input.Attribute,facts);return Input.Attribute;
    }
    internal ProbeValue Issue(XdmValue value)
    {
        var issued=new ProbeValue(Provider.Declared,new ProbeEvidence(Provider.Declared,Provider.Actual,Provider.Advisor!.Origin),value);
        Provider.Advisor.Values.Add(issued);return issued;
    }
    /// <inheritdoc/>
    public void Dispose(){Session.Dispose();Input.Dispose();}
}
internal sealed class Outcome<T>(SchemaResultKind kind,T value):SchemaResult<T>
{
    /// <inheritdoc/>
    public override SchemaResultKind Kind=>kind;
    /// <inheritdoc/>
    public override T Value=>kind==SchemaResultKind.Supported?value:throw new InvalidOperationException("No supported value.");
    /// <inheritdoc/>
    public override string? DiagnosticCode=>null;
    internal static Outcome<T> Ok(T value)=>new(SchemaResultKind.Supported,value);
    internal static Outcome<T> Unsupported()=>new(SchemaResultKind.Unsupported,default!);
}
internal sealed class Handle(ProbeProvider provider):ISchemaComponentHandle
{
    /// <inheritdoc/>
    public ISchemaAuthority Authority=>provider;
}
internal sealed class ProbeProvider:ISchemaAssessor,ISchemaAuthority
{
    internal readonly HashSet<ISchemaComponentHandle> Extra=new(ReferenceEqualityComparer.Instance);
    internal readonly Handle Declared;internal readonly Handle Actual;internal readonly Handle AttributeDeclaration;internal readonly Handle RootDeclaration;internal ProbeAdvisor? Advisor;
    internal ProbeProvider(){Declared=new(this);Actual=new(this);AttributeDeclaration=new(this);RootDeclaration=new(this);}
    /// <inheritdoc/>
    public ISchemaAuthority Authority=>this;
    /// <inheritdoc/>
    public SchemaLanguageVersion SchemaLanguage=>SchemaLanguageVersion.Xsd11;
    /// <inheritdoc/>
    public string AssessmentProfile=>"synthetic-transport-only-not-a-validator";
    /// <inheritdoc/>
    public SchemaResult<ISchemaAssessmentAdvisor> Bind(IQuerySemanticEnvironment environment,CancellationToken token)
    {token.ThrowIfCancellationRequested();Advisor=new(this,environment);return Outcome<ISchemaAssessmentAdvisor>.Ok(Advisor);}
    /// <inheritdoc/>
    public bool Owns(ISchemaComponentHandle component)=>Extra.Contains(component)||ReferenceEquals(component,Declared)||ReferenceEquals(component,Actual)||ReferenceEquals(component,AttributeDeclaration)||ReferenceEquals(component,RootDeclaration);
    /// <inheritdoc/>
    public SchemaResult<ISchemaComponentHandle> ResolveType(SchemaExpandedName name,CancellationToken token)=>Outcome<ISchemaComponentHandle>.Unsupported();
    /// <inheritdoc/>
    public SchemaResult<ISchemaComponentHandle> ResolveElementDeclaration(SchemaExpandedName name,CancellationToken token)=>Outcome<ISchemaComponentHandle>.Unsupported();
    /// <inheritdoc/>
    public SchemaResult<ISchemaComponentHandle> ResolveAttributeDeclaration(SchemaExpandedName name,CancellationToken token)=>Outcome<ISchemaComponentHandle>.Unsupported();
    /// <inheritdoc/>
    public SchemaResult<bool> IsSameComponent(ISchemaComponentHandle a,ISchemaComponentHandle b,CancellationToken token)=>Outcome<bool>.Ok(Owns(a)&&Owns(b)&&ReferenceEquals(a,b));
    /// <inheritdoc/>
    public SchemaResult<bool> IsDerivedFrom(ISchemaComponentHandle a,ISchemaComponentHandle b,CancellationToken token)=>Outcome<bool>.Unsupported();
    /// <inheritdoc/>
    public SchemaResult<bool> IsAffiliatedWith(ISchemaComponentHandle a,ISchemaComponentHandle b,CancellationToken token)=>Outcome<bool>.Unsupported();
    /// <inheritdoc/>
    public SchemaResult<ImmutableArray<ISchemaComponentHandle>> GetComponentClosure(ISchemaComponentHandle c,CancellationToken token)=>Outcome<ImmutableArray<ISchemaComponentHandle>>.Unsupported();
}
internal sealed class Origin(ISchemaSourceBinding source):ISchemaAssessmentOrigin
{
    /// <inheritdoc/>
    public ISchemaSourceBinding Source=>source;
}
internal sealed class ProbeAdvisor(ProbeProvider provider,IQuerySemanticEnvironment environment):ISchemaAssessmentAdvisor
{
    internal ProbeInput Input=null!;internal Origin Origin=null!;
    internal readonly HashSet<ISchemaTypedValue> Values=new(ReferenceEqualityComparer.Instance);
    internal readonly HashSet<ISchemaNodeFacts> Facts=new(ReferenceEqualityComparer.Instance);
    private readonly Dictionary<ISchemaNodeFacts,IXdmNode> issuedNodes=new(ReferenceEqualityComparer.Instance);
    internal void RegisterNode(IXdmNode node,ISchemaNodeFacts facts){Facts.Add(facts);issuedNodes.Add(facts,node);}
    /// <inheritdoc/>
    public ISchemaAssessor Assessor=>provider;
    /// <inheritdoc/>
    public IQuerySemanticEnvironment QueryEnvironment=>environment;
    /// <inheritdoc/>
    public bool OwnsAssessment(ISchemaAssessmentOrigin origin)=>ReferenceEquals(origin,Origin);
    /// <inheritdoc/>
    public bool OwnsTypedValue(ISchemaTypedValue value,ISchemaAssessmentInput input,ISchemaAssessmentOrigin? original)=>Values.Contains(value)&&ReferenceEquals(input,Input)&&ReferenceEquals(original,Origin);
    /// <inheritdoc/>
    public bool OwnsNodeFacts(ISchemaNodeFacts facts)=>Facts.Contains(facts);
    /// <inheritdoc/>
    public bool OwnsNode(IXdmNode node,ISchemaNodeFacts facts,ISchemaAssessmentInput input,ISchemaAssessmentOrigin origin)=>ReferenceEquals(input,Input)&&ReferenceEquals(origin,Origin)&&Input.OwnsNode(node)&&node is ProbeAttributeNode n&&ReferenceEquals(n.Facts,facts)&&Facts.Contains(facts)&&issuedNodes.TryGetValue(facts,out var original)&&ReferenceEquals(node,original);
    /// <inheritdoc/>
    public void ThrowIfUnavailable(CancellationToken token)=>token.ThrowIfCancellationRequested();
    /// <inheritdoc/>
    public ISchemaAssessmentResult Assess(SchemaAssessmentRequest request,CancellationToken token)=>throw new NotSupportedException("This probe does not validate XML.");
    /// <inheritdoc/>
    public SchemaResult<ISchemaTypedValue> ConstructSimpleValue(XdmValue input,ISchemaComponentHandle targetType,ISchemaAssessmentEpisode episode,CancellationToken token)=>Outcome<ISchemaTypedValue>.Unsupported();
}
internal sealed record ProbeEvidence(ISchemaComponentHandle DeclaredType,ISchemaComponentHandle ActualType,ISchemaAssessmentOrigin? OriginalAssessment):ISchemaAtomicProvenance;
internal sealed class ProbeValue(ISchemaComponentHandle governing,ProbeEvidence evidence,XdmValue value):ISchemaTypedValue
{
    /// <inheritdoc/>
    public ISchemaComponentHandle GoverningType=>governing;
    /// <inheritdoc/>
    public bool IsList=>false;
    /// <inheritdoc/>
    public IEnumerable<SchemaAtomicItem> AtomicItems{get{yield return new(value,evidence);}}
    /// <inheritdoc/>
    public bool TryGetSingleAtomic(out SchemaAtomicItem item){item=new(value,evidence);return true;}
}
internal sealed class ProbeInput:ISchemaAssessmentInput,IQueryOperationLifetime,IDisposable
{
    private bool closed;
    private readonly IXdmNode root;
    internal readonly ProbeAttributeNode Attribute;
    internal readonly ProbeAttributeNode RootNode;
    internal readonly List<ProbeAttributeNode> Nodes=[];
    internal ProbeAttributeNode Wrap(XDocumentNode raw)
    {
        ThrowIfUnavailable(default);
        var found=Nodes.FirstOrDefault(n=>n.Raw.IsSameNode(raw));
        if(found is not null)return found;
        var node=new ProbeAttributeNode(this,raw);Nodes.Add(node);return node;
    }
    internal ProbeInput(bool nested=false,bool nestedAttributes=false, bool simpleTerminals=false)
    {
        var element=new XElement(XName.Get("price","urn:price"),new XAttribute(XNamespace.Xmlns+"p","urn:attribute-probe"),new XAttribute(XName.Get("value","urn:attribute-probe"),"synthetic lexical placeholder"));
        if(nested)element=XElement.Parse("<order><line><amount>12.50</amount><quantity>2</quantity></line><line><amount>3.75</amount><quantity>1</quantity></line><approved>false</approved></order>");
        if(nestedAttributes)
        {
            element.SetAttributeValue("id","ROOT");
            foreach(var line in element.Elements("line"))
            {
                line.SetAttributeValue("value","synthetic lexical placeholder");
                line.SetAttributeValue(XName.Get("value","urn:attributes"),"synthetic lexical placeholder");
            }
        }
        if(simpleTerminals)
        {
            foreach(var line in element.Elements("line"))
            {
                var amount=line.Element("amount")!;amount.SetAttributeValue("currency","EUR");amount.ReplaceWith(new XElement("group",amount));
            }
        }
        RootNode=new(this,new XDocumentNode(element),true,nested);root=RootNode;Nodes.Add(RootNode);
        if(nested)
        {
            foreach(var e in element.Descendants())Nodes.Add(new(this,new XDocumentNode(e),e.HasElements || simpleTerminals && e.Name.LocalName=="amount",e.HasElements));
            Attribute=RootNode;
        }
        else {Attribute=new(this,new XDocumentNode(element.Attribute(XName.Get("value","urn:attribute-probe"))!));Nodes.Add(Attribute);}
    }
    /// <inheritdoc/>
    public IXdmNode Root{get{ThrowIfUnavailable(default);return root;}}
    /// <inheritdoc/>
    public ISchemaSourceBinding Source{get;}=new SourceBinding();
    /// <inheritdoc/>
    public ISchemaSnapshotBinding Snapshot{get;}=new SnapshotBinding();
    /// <inheritdoc/>
    public bool OwnsNode(IXdmNode node)=>Nodes.Any(n=>ReferenceEquals(node,n));
    /// <inheritdoc/>
    public void ThrowIfUnavailable(CancellationToken token){token.ThrowIfCancellationRequested();ObjectDisposedException.ThrowIf(closed,this);}
    /// <inheritdoc/>
    public IQueryOperationLease Enter(CancellationToken token){ThrowIfUnavailable(token);return new Lease(this);}
    /// <inheritdoc/>
    public void Dispose()=>closed=true;
    private sealed class SourceBinding:ISchemaSourceBinding;
    private sealed class SnapshotBinding:ISchemaSnapshotBinding;
    private sealed class Lease(ProbeInput input):IQueryOperationLease
    {
        private bool closed;
    /// <inheritdoc/>
        public void ThrowIfUnavailable(CancellationToken token){ObjectDisposedException.ThrowIf(closed,this);input.ThrowIfUnavailable(token);}
    /// <inheritdoc/>
        public void Dispose()=>closed=true;
    }
}

/// <summary>Checks original attribute-node admission with an intentionally synthetic issuer.</summary>
public sealed class AttributeFactsTests
{
    /// <summary>Original attribute data keeps the actual atomic payload and original type evidence.</summary>
    /// <param name="kind">Scalar payload family/case.</param>
    [Theory][InlineData(0)][InlineData(1)][InlineData(2)][InlineData(3)][InlineData(4)][InlineData(5)][InlineData(6)][InlineData(7)]
    public void OriginalAttributeData_PreservesPayloadAndOriginalEvidence(int kind)
    {
        using var f=new Fixture("data($node)");var plain=TransportTests.Plain(kind);var node=f.Attribute(plain);
        Assert.Same(f.Provider.AttributeDeclaration,node.OriginalSchemaFacts.Declaration);Assert.NotSame(node.OriginalSchemaFacts.Type,node.OriginalSchemaFacts.Declaration);
        Assert.Equal(XdmNodeKind.Attribute,node.NodeKind);Assert.Equal("urn:attribute-probe",node.NamespaceUri);Assert.Equal("value",node.LocalName);
        var detached=f.Session.Run([f.Snapshot],op=>
        {
            var actual=op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(node)));TransportTests.SamePayload(plain,actual);
            Assert.True(SchemaAtomics.TryGetProvenance(actual,out var evidence));Assert.Same(f.Provider.Declared,evidence!.DeclaredType);Assert.Same(f.Provider.Actual,evidence.ActualType);Assert.Same(f.Provider.Advisor!.Origin,evidence.OriginalAssessment);
            return actual.Kind==XdmValueKind.String?actual.StringValue:"detached";
        });
        f.Dispose();Assert.NotNull(detached);
    }
    /// <summary>Attribute-issued facts also work through the scalar facade's established identity profile.</summary>
    [Fact]
    public void AttributeFacts_UseExistingTypedReadRoute()
    {
        using var f=new Fixture();var node=f.Attribute(XdmValue.FromBoolean(false));
        f.Session.Run([f.Snapshot],op=>
        {
            var read=op.ReadTypedValue(f.Snapshot,node.OriginalSchemaFacts.ReadTypedValue(default).Value);Assert.Equal(SchemaResultKind.Supported,read.Kind);
            var actual=op.Evaluate(new EvaluationContext().WithVariable("value",read.Value));Assert.False(actual.BooleanValue);
            Assert.True(SchemaAtomics.TryGetProvenance(actual,out var evidence));Assert.Same(f.Provider.Advisor!.Origin,evidence!.OriginalAssessment);return 0;
        });
    }
    /// <summary>Equal names and payloads cannot authenticate foreign attribute nodes or facts.</summary>
    [Fact]
    public void ForeignAttributeNodeAndFacts_AreRefused()
    {
        using var a=new Fixture("data($node)");using var b=new Fixture("data($node)");var x=a.Attribute(XdmValue.FromString("x"));var y=b.Attribute(XdmValue.FromString("x"));
        b.Session.Run([b.Snapshot],op=>{Assert.Throws<ArgumentException>(()=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(x))));return 0;});
        var saved=y.Facts;y.Facts=x.Facts;
        b.Session.Run([b.Snapshot],op=>{Assert.Throws<ArgumentException>(()=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(y))));return 0;});y.Facts=saved;
    }
    /// <summary>Missing original attribute facts cannot be replaced by raw node metadata.</summary>
    [Fact]
    public void AttributeWithoutIssuedFacts_IsRefused()
    {
        using var f=new Fixture("data($node)");
        f.Session.Run([f.Snapshot],op=>{Assert.Throws<InvalidOperationException>(()=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(f.Input.Attribute))));return 0;});
    }
    /// <summary>Original attribute provenance expires when its selected snapshot is disposed.</summary>
    [Fact]
    public void AttributeEvidence_ExpiresWithSnapshot()
    {
        using var f=new Fixture("data($node)");var node=f.Attribute(XdmValue.FromString("detached"));ISchemaAtomicProvenance? evidence=null;
        var text=f.Session.Run([f.Snapshot],op=>{var v=op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(node)));Assert.True(SchemaAtomics.TryGetProvenance(v,out evidence));return v.StringValue;});
        f.Snapshot.Dispose();Assert.Throws<ObjectDisposedException>(()=>evidence!.ActualType);Assert.Equal("detached",text);
    }
    /// <summary>Revocation prevents original attribute reads and detachment through a closed input.</summary>
    [Fact]
    public void ClosedAttributeInput_RefusesOriginalData()
    {
        using var f=new Fixture("data($node)");var node=f.Attribute(XdmValue.FromString("x"));f.Input.Dispose();
        Assert.Throws<ObjectDisposedException>(()=>f.Session.Run([f.Snapshot],op=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(node)))));
    }
}
internal sealed class ProbeFacts(ISchemaComponentHandle type,ISchemaComponentHandle declaration,ISchemaAssessmentOrigin origin,ISchemaTypedValue? value):ISchemaNodeFacts
{
    /// <inheritdoc/>
    public ISchemaComponentHandle Type=>type;
    /// <inheritdoc/>
    public ISchemaComponentHandle? Declaration=>declaration;
    /// <inheritdoc/>
    public ISchemaAssessmentOrigin Origin=>origin;
    /// <inheritdoc/>
    public bool HasNoTypedValue=>value is null;
    /// <inheritdoc/>
    public bool IsNilled=>false;
    /// <inheritdoc/>
    public SchemaResult<ISchemaTypedValue> ReadTypedValue(CancellationToken token){token.ThrowIfCancellationRequested();return value is null?Outcome<ISchemaTypedValue>.Unsupported():Outcome<ISchemaTypedValue>.Ok(value);}
}
internal sealed class ProbeAttributeNode(ProbeInput input,XDocumentNode raw,bool complexRoot=false,bool noTyped=false):ISchemaAnnotatedNode
{
    internal XDocumentNode Raw=>raw;
    internal ISchemaNodeFacts? Facts;
    /// <inheritdoc/>
    public ISchemaNodeFacts OriginalSchemaFacts{get{input.ThrowIfUnavailable(default);return Facts??throw new InvalidOperationException("No issued attribute facts.");}}
    /// <inheritdoc/>
    public XdmNodeKind NodeKind=>raw.NodeKind;
    /// <inheritdoc/>
    public bool IsComplexType=>complexRoot;
    /// <inheritdoc/>
    public bool HasNoTypedValue=>noTyped;
    /// <inheritdoc/>
    public (string NamespaceUri,string LocalName)? SchemaTypeAnnotation=>complexRoot?("urn:price","PriceType"):null;
    /// <inheritdoc/>
    public (string NamespaceUri,string LocalName)? SchemaElementDeclaration=>complexRoot&&ReferenceEquals(this,input.RootNode)?(raw.NamespaceUri,raw.LocalName):null;
    /// <inheritdoc/>
    public string LocalName=>raw.LocalName;
    /// <inheritdoc/>
    public string NamespaceUri=>raw.NamespaceUri;
    /// <inheritdoc/>
    public string Prefix=>raw.Prefix;
    /// <inheritdoc/>
    public string StringValue=>raw.StringValue;
    /// <inheritdoc/>
    public XdmValue TypedValue=>throw new InvalidOperationException("Use original issued attribute facts.");
    /// <inheritdoc/>
    public IXdmNode? Parent=>raw.Parent is XDocumentNode n?input.Wrap(n):null;
    /// <inheritdoc/>
    public IXdmNode? Document=>raw.Document is XDocumentNode n?input.Wrap(n):null;
    /// <inheritdoc/>
    public long DocumentOrder=>raw.DocumentOrder;
    /// <inheritdoc/>
    public string BaseUri=>raw.BaseUri;
    /// <inheritdoc/>
    public string DocumentUri=>raw.DocumentUri;
    /// <inheritdoc/>
    public XdmSequence Children(XdmNodeKind kind=XdmNodeKind.All)=>Project(raw.Children(kind));
    /// <inheritdoc/>
    public XdmSequence Attributes(string? localName=null,string? namespaceUri=null)=>Project(raw.Attributes(localName,namespaceUri));
    /// <inheritdoc/>
    public XdmSequence Axis(XdmAxis axis)=>Project(raw.Axis(axis));
    /// <inheritdoc/>
    public bool IsSameNode(IXdmNode other)=>ReferenceEquals(this,other);
    private sealed class Items(List<XdmValue> values):IXdmSequence
    {
        /// <inheritdoc/>
        public bool TryGetLength(out int length){length=values.Count;return true;}
        /// <inheritdoc/>
        public IXdmSequenceEnumerator GetEnumerator()=>new Cursor(values);
        private sealed class Cursor(List<XdmValue> values):IXdmSequenceEnumerator
        {
            private int index=-1;
            /// <inheritdoc/>
            public bool MoveNext()=>++index<values.Count;
            /// <inheritdoc/>
            public XdmValue Current=>values[index];
        }
    }
    private XdmSequence Project(XdmSequence sequence)
    {
        var values=new List<XdmValue>();foreach(var value in sequence)values.Add(value.IsNode&&value.NodeValue is XDocumentNode n?XdmValue.FromNode(input.Wrap(n)):value);
        return XdmSequence.FromSource(new Items(values));
    }
    /// <inheritdoc/>
    public string ToXmlString()=>raw.ToXmlString();
}

/// <summary>Tests public transport of complex simple-content roots with actual decimal atomic values.</summary>
public sealed class RootFactsTests
{
    private static XdmValue Decimal(string lexical)=>XdmValue.FromDecimal(new XdmExactDecimal(System.Numerics.BigInteger.Parse(lexical.Replace(".","",StringComparison.Ordinal),CultureInfo.InvariantCulture),lexical.Contains('.')?lexical.Length-lexical.IndexOf('.')-1:0,256,256));
    /// <summary>Complex root annotations and declared type differ from the selected decimal atomic type.</summary>
    /// <param name="lexical">Exact finite decimal test payload.</param>
    [Theory][InlineData("12.50")][InlineData("0")][InlineData("-12.50")][InlineData("12345678901234567890123456789012345678901234567890")][InlineData("+0.000")]
    public void ComplexRootData_RetainsDecimalAndDistinctTypeEvidence(string lexical)
    {
        using var f=new Fixture("data($node)");var value=Decimal(lexical);var root=f.RootValue(value);
        Assert.Equal(XdmNodeKind.Element,root.NodeKind);Assert.True(root.IsComplexType);Assert.False(root.HasNoTypedValue);Assert.Equal(("urn:price","PriceType"),root.SchemaTypeAnnotation!.Value);
        Assert.Same(f.Provider.Declared,root.OriginalSchemaFacts.Type);Assert.NotSame(root.OriginalSchemaFacts.Type,root.OriginalSchemaFacts.Declaration);
        f.Session.Run([f.Snapshot],op=>
        {
            var actual=op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(root)));Assert.Equal(value.ExactDecimalValue,actual.ExactDecimalValue);
            Assert.True(SchemaAtomics.TryGetProvenance(actual,out var evidence));Assert.Same(f.Provider.Declared,evidence!.DeclaredType);Assert.Same(f.Provider.Actual,evidence.ActualType);Assert.NotSame(evidence.DeclaredType,evidence.ActualType);Assert.Same(f.Provider.Advisor!.Origin,evidence.OriginalAssessment);return 0;
        });
    }
    /// <summary>The facade's existing identity read route also supports distinct complex/atomic type handles.</summary>
    [Fact]
    public void ComplexRootIdentity_ReadsOriginalDecimal()
    {
        using var f=new Fixture();var root=f.RootValue(Decimal("12.50"));var detached=f.Session.Run([f.Snapshot],op=>
        {
            var read=op.ReadTypedValue(f.Snapshot,root.OriginalSchemaFacts.ReadTypedValue(default).Value);Assert.Equal(SchemaResultKind.Supported,read.Kind);var value=op.Evaluate(new EvaluationContext().WithVariable("value",read.Value));
            Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.NotSame(evidence!.DeclaredType,evidence.ActualType);return value.ExactDecimalValue;
        });f.Dispose();Assert.Equal("12.5",detached.ToInvariantString(256));
    }
    /// <summary>Same-shaped foreign roots/facts cannot authenticate a selected input.</summary>
    [Fact]
    public void ForeignRootAndFacts_AreRefused()
    {
        using var a=new Fixture("data($node)");using var b=new Fixture("data($node)");var x=a.RootValue(Decimal("1"));var y=b.RootValue(Decimal("1"));
        b.Session.Run([b.Snapshot],op=>{Assert.Throws<ArgumentException>(()=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(x))));return 0;});var saved=y.Facts;y.Facts=x.Facts;
        b.Session.Run([b.Snapshot],op=>{Assert.Throws<ArgumentException>(()=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(y))));return 0;});y.Facts=saved;
    }
    /// <summary>Metadata without issued root facts cannot manufacture validation.</summary>
    [Fact]
    public void RootMetadataAlone_IsRefused()
    {
        using var f=new Fixture("data($node)");f.Session.Run([f.Snapshot],op=>{Assert.Throws<InvalidOperationException>(()=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(f.Input.RootNode))));return 0;});
    }
    /// <summary>Original root provenance is revoked with its selected snapshot.</summary>
    [Fact]
    public void RootEvidence_ExpiresWithSnapshot()
    {
        using var f=new Fixture("data($node)");var root=f.RootValue(Decimal("1"));ISchemaAtomicProvenance? evidence=null;
        f.Session.Run([f.Snapshot],op=>{var value=op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(root)));Assert.True(SchemaAtomics.TryGetProvenance(value,out evidence));return 0;});f.Snapshot.Dispose();Assert.Throws<ObjectDisposedException>(()=>evidence!.ActualType);
    }
    /// <summary>Closed original root input cannot supply a new typed read.</summary>
    [Fact]
    public void ClosedRootInput_RefusesRead()
    {
        using var f=new Fixture("data($node)");var root=f.RootValue(Decimal("1"));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>f.Session.Run([f.Snapshot],op=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(root)))));
    }
}
