// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies exact calendar contracts through both public facades.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
using Fixture = Bosak.Schema.Tests.NodeData.Fixture;
using Xunit;
namespace Bosak.Schema.Tests.NativeCalendars;
/// <summary>Verifies calendar facts and authority against actual Core sessions.</summary>
public sealed class NativeCalendarIdentityTests
{
    private const string Start="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>";
    private static string Value(string kind)=>(kind=="time"?"":"2000-01-01T")+"12:00:00.123456789Z";
    /// <summary>Checks exact calendar payloads and actual type proof for primitive and named atoms.</summary>
    /// <param name="kind">Calendar family.</param><param name="named">Named restriction selector.</param>
    [Theory][InlineData("dateTime",false)][InlineData("time",false)][InlineData("dateTimeStamp",false)][InlineData("dateTime",true)][InlineData("time",true)][InlineData("dateTimeStamp",true)]
    public void ExactFacts(string kind,bool named)
    {
        var schema=Start+(named?"<xs:simpleType name='N'><xs:restriction base='xs:"+kind+"'/></xs:simpleType>":"")+"<xs:element name='r' type='"+(named?"N":"xs:"+kind)+"'/></xs:schema>";
        using var f=new Fixture(schema,"<r>"+Value(kind)+"</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.TryGetSingleAtomic(out var atomic));Assert.True(atomic.Value.IsExactCalendar);Assert.Equal(Value(kind),atomic.Value.ExactCalendarValue.ToInvariantString(256));
        Assert.Equal(Enum.Parse<NativeScalarKind>(char.ToUpperInvariant(kind[0])+kind[1..]),f.Read(f.Input.Root).Kind);Assert.Same(fact.Type,typed.GoverningType);Assert.Same(f.Provider.TypeComponent(fact.NativeType),atomic.TypeEvidence.ActualType);Assert.True(f.Provider.Owns(atomic.TypeEvidence.ActualType));
        f.Run(op=>{var atom=f.Evaluate(op,f.Input.Root);Assert.True(atom.IsExactCalendar);Assert.True(SchemaAtomics.TryGetProvenance(atom,out var provenance));Assert.Same(fact.Type,provenance!.DeclaredType);Assert.Same(atomic.TypeEvidence.ActualType,provenance.ActualType);Assert.Same(f.Assessment.Origin,provenance.OriginalAssessment);Assert.Equal(Value(kind),atom.ExactCalendarValue.ToInvariantString(256));return true;});
        using var other=new Fixture(schema,"<r>"+Value(kind)+"</r>");Assert.Throws<ArgumentException>(()=>other.Provider.IsSameComponent(fact.Type,other.Provider.DecimalType,default));
    }
    /// <summary>Checks anonymous, shared named and canonical global calendar identities.</summary>
    [Fact]
    public void ComponentIdentity()
    {
        var anonymous=Start+"<xs:element name='r'><xs:simpleType><xs:restriction base='xs:dateTime'/></xs:simpleType></xs:element></xs:schema>";
        using(var f=new Fixture(anonymous,"<r>"+Value("dateTime")+"</r>"))Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Input.Root)!.NativeType.Name);
        var schema=Start+"<xs:simpleType name='N'><xs:restriction base='xs:dateTime'/></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='g'><xs:complexType><xs:sequence><xs:element ref='d'/></xs:sequence></xs:complexType></xs:element><xs:element name='h'><xs:complexType><xs:sequence><xs:element ref='d'/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element><xs:element name='d' type='N'/></xs:schema>";
        using var shared=new Fixture(schema,"<r><g><d>"+Value("dateTime")+"</d></g><h><d>"+Value("dateTime")+"</d></h></r>");
        var a=shared.Input.FactsFor((NativeSessionNode)shared.Element([0,0]))!;var b=shared.Input.FactsFor((NativeSessionNode)shared.Element([1,0]))!;
        Assert.Same(a.Type,b.Type);Assert.Same(a.Declaration,b.Declaration);Assert.Same(a.NativeDeclaration,b.NativeDeclaration);Assert.NotSame(a.Node,b.Node);
    }
    /// <summary>Checks selected stamp proof and absence of facts after invalid calendar input.</summary>
    [Fact]
    public void SelectionAndInvalidity()
    {
        var schema=Start+"<xs:element name='r' type='xs:dateTime'/></xs:schema>";
        using(var f=new Fixture(schema,"<r xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xs='http://www.w3.org/2001/XMLSchema' xsi:type='xs:dateTimeStamp'>"+Value("dateTimeStamp")+"</r>"))
        {
            var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("dateTime",fact.NativeDeclaration.Type.Name);Assert.Equal("dateTimeStamp",fact.NativeType.Name);Assert.NotSame(fact.NativeDeclaration.Type,fact.NativeType);Assert.Equal(NativeScalarKind.DateTimeStamp,f.Read(f.Input.Root).Kind);
        }
        using var invalid=new Fixture(schema,"<r>not a dateTime</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,invalid.Assessment.Status);Assert.Empty(invalid.Assessment.Facts);Assert.Null(invalid.Snapshot);Assert.Null(invalid.Input.FactsFor((NativeSessionNode)invalid.Input.Root));
    }
    /// <summary>Checks owned calendar derivation while preserving unsupported noncalendar answers.</summary>
    [Fact]
    public void CAL098_OwnedDerivation()
    {
        using var f=new Fixture(Start+"<xs:simpleType name='N'><xs:restriction base='xs:dateTimeStamp'/></xs:simpleType><xs:element name='r' type='N'/></xs:schema>","<r>"+Value("dateTimeStamp")+"</r>");
        var n=f.Provider.ResolveType(new("","N"),default).Value;var dt=f.Provider.ResolveType(new("http://www.w3.org/2001/XMLSchema","dateTime"),default).Value;var time=f.Provider.ResolveType(new("http://www.w3.org/2001/XMLSchema","time"),default).Value;
        Assert.True(f.Provider.IsDerivedFrom(n,dt,default).Value);Assert.False(f.Provider.IsDerivedFrom(dt,n,default).Value);Assert.False(f.Provider.IsDerivedFrom(n,time,default).Value);Assert.Equal(SchemaResultKind.Unsupported,f.Provider.IsDerivedFrom(n,f.Provider.DecimalType,default).Kind);
        using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>f.Provider.IsDerivedFrom(n,dt,cancel.Token));
    }
    /// <summary>Checks guarded comparisons using actual native issued exact calendars.</summary>
    /// <param name="kind">Calendar type.</param><param name="expression">Closed comparison.</param><param name="expected">Expected Boolean result.</param>
    [Theory][InlineData("dateTime","$left eq $right",true)][InlineData("dateTime","$left ne $right",false)][InlineData("dateTime","$left lt $right",false)][InlineData("dateTime","$left le $right",true)][InlineData("dateTime","$left gt $right",false)][InlineData("dateTime","$left ge $right",true)][InlineData("time","$left eq $right",true)][InlineData("time","$left ne $right",false)][InlineData("time","$left lt $right",false)][InlineData("time","$left le $right",true)][InlineData("time","$left gt $right",false)][InlineData("time","$left ge $right",true)][InlineData("dateTimeStamp","$left eq $right",true)][InlineData("dateTimeStamp","$left ne $right",false)][InlineData("dateTimeStamp","$left lt $right",false)][InlineData("dateTimeStamp","$left le $right",true)][InlineData("dateTimeStamp","$left gt $right",false)][InlineData("dateTimeStamp","$left ge $right",true)]
    public void CAL099_GuardedComparison(string kind,string expression,bool expected)
    {
        using var f=new GuardedFixture(kind,expression);
        var result=f.Session.Run([f.Snapshot],op=>
        {
            var left=op.ReadTypedValue(f.Snapshot,f.Assessment.ElementFacts[1].ReadTypedValue(default).Value).Value;
            var right=op.ReadTypedValue(f.Snapshot,f.Assessment.ElementFacts[2].ReadTypedValue(default).Value).Value;
            return op.Evaluate(new EvaluationContext().WithVariable("left",left).WithVariable("right",right)).BooleanValue;
        });Assert.Equal(expected,result);
    }
    /// <summary>Checks foreign authority, fingerprint mismatch and revoked inputs remain refused.</summary>
    [Fact]
    public void CAL100_ForeignAndRevoked()
    {
        using var f=new GuardedFixture("dateTime","$left eq $right");using var other=new GuardedFixture("dateTime","$left eq $right");
        var foreign=other.Assessment.ElementFacts[1].ReadTypedValue(default).Value;
        f.Session.Run([f.Snapshot],op=>{Assert.Throws<ArgumentException>(()=>op.ReadTypedValue(f.Snapshot,foreign));return true;});
        var mismatch=SchemaQuerySession.CompileCalendarComparison("$left ne $right",new(1,16,1,256,256),f.Provider,0);Assert.Equal(SchemaResultKind.Unsupported,mismatch.Kind);
        f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>f.Session.Run([f.Snapshot],_=>true));
    }

    private sealed class GuardedFixture : IDisposable
    {
        internal NativeSchemaProvider Provider { get; }
        internal SchemaQuerySession Session { get; }
        internal NativeSchemaInput Input { get; }
        internal NativeSchemaProvider.Assessment Assessment { get; }
        internal SchemaQuerySnapshot Snapshot { get; }
        internal GuardedFixture(string kind,string expression)
        {
            var schema=Start+"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='xs:"+kind+"'/><xs:element name='b' type='xs:"+kind+"'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
            var compiled=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:256);Assert.Equal(NativeOutcome.Valid,compiled.Outcome);
            var recorder=new Recorder(new NativeSchemaProvider(compiled.Value!));var limits=new SchemaQueryLimits(1,16,1,256,256);
            Assert.Equal(SchemaResultKind.Unsupported,SchemaQuerySession.CompileCalendarComparison(expression,limits,recorder,0).Kind);Assert.NotNull(recorder.Fingerprint);
            Provider=new NativeSchemaProvider(compiled.Value!,calendarComparisonFingerprint:recorder.Fingerprint);
            var opened=SchemaQuerySession.CompileCalendarComparison(expression,limits,Provider,0);Assert.Equal(SchemaResultKind.Supported,opened.Kind);Session=opened.Value;
            var left=Value(kind);var right=(kind=="time"?"":"2000-01-01T")+"14:00:00.123456789+02:00";
            Input=NativeSchemaInput.Capture("<r><a>"+left+"</a><b>"+right+"</b></r>").Value!;
            Assessment=(NativeSchemaProvider.Assessment)Session.Advisor!.Assess(new(Input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);Assert.Equal(SchemaAssessmentStatus.Valid,Assessment.Status);
            var snapshot=Session.OpenSnapshot(Input,Input,Assessment.Origin);Assert.Equal(SchemaResultKind.Supported,snapshot.Kind);Snapshot=snapshot.Value;
        }
        /// <inheritdoc/>
        public void Dispose(){Session.Dispose();Input.Dispose();}
    }
    private sealed record Episode : ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime => DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes => 0;
    }
    private sealed class Recorder(ISchemaAuthority authority) : ISchemaAssessor
    {
        internal string? Fingerprint { get; private set; }
        /// <inheritdoc/>
        public ISchemaAuthority Authority=>authority;
        /// <inheritdoc/>
        public SchemaLanguageVersion SchemaLanguage=>SchemaLanguageVersion.Xsd11;
        /// <inheritdoc/>
        public string AssessmentProfile=>"calendar-fingerprint-recorder-never-assesses";
        /// <inheritdoc/>
        public SchemaResult<ISchemaAssessmentAdvisor> Bind(IQuerySemanticEnvironment environment,CancellationToken token){token.ThrowIfCancellationRequested();Fingerprint=environment.StaticContextFingerprint;return NativeCoreResult<ISchemaAssessmentAdvisor>.Unsupported("RECORDER-ONLY");}
    }
}
