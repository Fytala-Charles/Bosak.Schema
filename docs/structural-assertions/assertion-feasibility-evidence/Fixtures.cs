// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Owns actual native assessment episodes for isolated original-node data probes.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
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
using System.Xml;
using Bosak.Schema;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests.NodeData;

internal sealed class Fixture : IDisposable
{
    internal readonly NativeSchemaProvider Provider;
    internal readonly SchemaQuerySession Session;
    internal readonly NativeSessionRunner Runner;
    internal readonly NativeSchemaInput Input;
    internal readonly NativeSchemaProvider.Assessment Assessment;
    internal readonly SchemaQuerySnapshot? Snapshot;
    internal readonly SchemaQueryLimits Limits;
    internal readonly string Fingerprint;
    internal Fixture(string schema,string xml,int cache=32,int digits=256,int scale=256,int documents=2)
    {
        Limits=new(documents,cache,1,digits,scale);
        var compiled=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Valid,compiled.Outcome);
        Fingerprint=CaptureFingerprint(compiled.Value!,Limits);
        Provider=new(compiled.Value!,true,Fingerprint);
        var session=SchemaQuerySession.CompileXPath("data($node)",Limits,assessor:Provider);Assert.Equal(SchemaResultKind.Supported,session.Kind);Session=session.Value;Runner=new(Session);
        Input=NativeSchemaInput.Capture(xml).Value!;
        Assessment=(NativeSchemaProvider.Assessment)Session.Advisor!.Assess(new(Input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
        if(Assessment.Status==SchemaAssessmentStatus.Valid){var opened=Session.OpenSnapshot(Input,Input,Assessment.Origin);Assert.Equal(SchemaResultKind.Supported,opened.Kind);Snapshot=opened.Value;}
    }
    internal static string CaptureFingerprint(NativeDecimalSchema schema,SchemaQueryLimits limits)
    {
        var recorder=new Recorder(new NativeSchemaProvider(schema));
        var attempt=SchemaQuerySession.CompileXPath("data($node)",limits,assessor:recorder);Assert.Equal(SchemaResultKind.Unsupported,attempt.Kind);Assert.False(string.IsNullOrEmpty(recorder.Fingerprint));return recorder.Fingerprint!;
    }
    internal IXdmNode Element(ImmutableArray<int> path)
    {
        if(path.IsDefault)throw new ArgumentException("Default path.",nameof(path));
        if(path.Length>3)throw new ArgumentOutOfRangeException(nameof(path));
        var segments=path.ToArray();IXdmNode node=Input.Root;
        foreach(var segment in segments){if(segment<0)throw new ArgumentOutOfRangeException(nameof(path));var children=new List<IXdmNode>();foreach(var item in node.Children(XdmNodeKind.Element))children.Add(item.NodeValue);if(segment>=children.Count)throw new ArgumentOutOfRangeException(nameof(path));node=children[segment];}
        return node;
    }
    internal IXdmNode Attribute(ImmutableArray<int> path,string ns,string local)
    {
        ArgumentNullException.ThrowIfNull(ns);ArgumentNullException.ThrowIfNull(local);
        try{XmlConvert.VerifyNCName(local);}catch(XmlException error){throw new ArgumentException("Invalid local name.",nameof(local),error);}
        var owner=Element(path);foreach(var item in owner.Attributes(local,ns))return item.NodeValue;throw new ArgumentException("Missing attribute.",nameof(local));
    }
    internal NativeScalarValue Read(IXdmNode node,CancellationToken token=default)
    {
        if(Snapshot is null)throw new NotSupportedException("No valid original assessment.");
        if(node is not NativeSessionNode original || Input.FactsFor(original) is null)throw new NotSupportedException("No original typed facts.");
        return Runner.Run([Snapshot],op=>NativeScalarValue.FromValue(op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(node)))),token);
    }
    internal T Run<T>(Func<SchemaQueryOperation,T> callback,CancellationToken token=default)
    {
        if(Snapshot is null)throw new NotSupportedException("No valid original assessment.");
        return Runner.Run([Snapshot],callback,token);
    }
    internal XdmValue Evaluate(SchemaQueryOperation operation,IXdmNode node)=>operation.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(node)));
    /// <inheritdoc/>
    public void Dispose(){Session.Dispose();Input.Dispose();}

    private sealed record Episode : ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    private sealed class Recorder(ISchemaAuthority authority) : ISchemaAssessor
    {
        internal string? Fingerprint;
        /// <inheritdoc/>
        public ISchemaAuthority Authority=>authority;
        /// <inheritdoc/>
        public SchemaLanguageVersion SchemaLanguage=>SchemaLanguageVersion.Xsd11;
        /// <inheritdoc/>
        public string AssessmentProfile=>"context-recorder-never-assesses";
        /// <inheritdoc/>
        public SchemaResult<ISchemaAssessmentAdvisor> Bind(IQuerySemanticEnvironment environment,CancellationToken token)
        {token.ThrowIfCancellationRequested();Fingerprint=environment.StaticContextFingerprint;return NativeCoreResult<ISchemaAssessmentAdvisor>.Unsupported("RECORDER-ONLY");}
    }
}
