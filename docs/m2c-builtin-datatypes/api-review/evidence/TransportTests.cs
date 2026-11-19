// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Probes typed scalar transport through published public Core APIs.
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

namespace DatatypeTransportProbe;

/// <summary>Qualifies public typed identity with a synthetic issuer, not an XSD validator.</summary>
public sealed class TransportTests
{
    private static XdmValue Plain(int kind)=>kind switch
    {
        0=>XdmValue.FromString(" INV-001 "),1=>XdmValue.FromString(""),2=>XdmValue.FromString("??e\u0301\r\n\t"),
        3=>XdmValue.FromBoolean(true),4=>XdmValue.FromBoolean(false),
        5=>XdmValue.FromInteger(BigInteger.Parse("12345678901234567890123456789012345678901234567890",CultureInfo.InvariantCulture)),
        6=>XdmValue.FromInteger(BigInteger.Parse("-12345678901234567890123456789012345678901234567890",CultureInfo.InvariantCulture)),
        _=>XdmValue.FromDecimal(new XdmExactDecimal(12500,3,256,256))
    };
    private static void SamePayload(XdmValue expected,XdmValue actual)
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
    internal readonly ProbeInput Input=new();
    internal readonly ProbeProvider Provider=new();
    internal readonly SchemaQuerySession Session;
    internal readonly SchemaQuerySnapshot Snapshot;
    internal Fixture()
    {
        Session=SchemaQuerySession.CompileXPath("$value",new(2,8,1,256,256),assessor:Provider).Value;
        Provider.Advisor!.Input=Input;Provider.Advisor.Origin=new(Input.Source);
        Snapshot=Session.OpenSnapshot(Input,Input,Provider.Advisor.Origin).Value;
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
{public ISchemaAuthority Authority=>provider;}
internal sealed class ProbeProvider:ISchemaAssessor,ISchemaAuthority
{
    internal readonly Handle Declared;internal readonly Handle Actual;internal ProbeAdvisor? Advisor;
    internal ProbeProvider(){Declared=new(this);Actual=new(this);}
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
    public bool Owns(ISchemaComponentHandle component)=>ReferenceEquals(component,Declared)||ReferenceEquals(component,Actual);
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
{public ISchemaSourceBinding Source=>source;}
internal sealed class ProbeAdvisor(ProbeProvider provider,IQuerySemanticEnvironment environment):ISchemaAssessmentAdvisor
{
    internal ProbeInput Input=null!;internal Origin Origin=null!;
    internal readonly HashSet<ISchemaTypedValue> Values=new(ReferenceEqualityComparer.Instance);
    /// <inheritdoc/>
    public ISchemaAssessor Assessor=>provider;
    /// <inheritdoc/>
    public IQuerySemanticEnvironment QueryEnvironment=>environment;
    /// <inheritdoc/>
    public bool OwnsAssessment(ISchemaAssessmentOrigin origin)=>ReferenceEquals(origin,Origin);
    /// <inheritdoc/>
    public bool OwnsTypedValue(ISchemaTypedValue value,ISchemaAssessmentInput input,ISchemaAssessmentOrigin? original)=>Values.Contains(value)&&ReferenceEquals(input,Input)&&ReferenceEquals(original,Origin);
    /// <inheritdoc/>
    public bool OwnsNodeFacts(ISchemaNodeFacts facts)=>false;
    /// <inheritdoc/>
    public bool OwnsNode(IXdmNode node,ISchemaNodeFacts facts,ISchemaAssessmentInput input,ISchemaAssessmentOrigin origin)=>false;
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
    private readonly IXdmNode root=new XDocumentNode(new XElement("probe"));
    /// <inheritdoc/>
    public IXdmNode Root{get{ThrowIfUnavailable(default);return root;}}
    /// <inheritdoc/>
    public ISchemaSourceBinding Source{get;}=new SourceBinding();
    /// <inheritdoc/>
    public ISchemaSnapshotBinding Snapshot{get;}=new SnapshotBinding();
    /// <inheritdoc/>
    public bool OwnsNode(IXdmNode node)=>ReferenceEquals(node,root);
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
