// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Binds native decimal assessment to authority-owned Core session facts.
// SPECIAL NOTES        : Internal Core 1.81.2 adapter; finite profile only, no public native factory.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 15-11-2026     | Implement internal native decimal Core adapter                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

/// <summary>Binds native decimal assessment to authority-owned Core session facts.</summary>
internal sealed class NativeSchemaProvider : ISchemaAssessor, ISchemaAuthority
{
    private readonly NativeDecimalSchema schema;
    private readonly Dictionary<object, Component> components = new(ReferenceEqualityComparer.Instance);
    internal Component RootDeclaration { get; }
    internal Component DecimalType { get; }

    internal NativeSchemaProvider(NativeDecimalSchema schema)
    {
        this.schema = schema;
        RootDeclaration = Add(schema.Root);
        Add(schema.Root.Type);
        foreach (var declaration in schema.Children) { Add(declaration); Add(declaration.Type); }
        DecimalType = components[schema.Children[0].Type];
    }

    private Component Add(object value)
    {
        if (components.TryGetValue(value, out var existing)) return existing;
        var component = new Component(this);
        components.Add(value, component);
        return component;
    }

    public ISchemaAuthority Authority => this;
    public SchemaLanguageVersion SchemaLanguage => SchemaLanguageVersion.Xsd11;
    public string AssessmentProfile => "schema-native-decimal-session-v1";

    public SchemaResult<ISchemaAssessmentAdvisor> Bind(IQuerySemanticEnvironment environment, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(environment);
        // Only the released engine-issued scalar environment with an empty namespace context is adopted.
        if (environment.GetType().Assembly != typeof(SchemaQuerySession).Assembly ||
            environment.HostLanguage != QueryHostLanguage.XPath || environment.HostVersion != "3.1" ||
            environment.XdmSpecificationRevision.AbsoluteUri != "https://www.w3.org/TR/2017/REC-xpath-datamodel-31-20170321/" ||
            environment.BehaviorProfile != "bosak-typed-identity-v1" ||
            environment.DescriptorFormatVersion != "bosak-public-typed-identity-descriptor-v1" ||
            string.IsNullOrEmpty(environment.StaticContextFingerprint) ||
            environment.StaticContext.Namespaces.Count != 0)
            return NativeCoreResult<ISchemaAssessmentAdvisor>.Unsupported("BSK-NATIVE-ENVIRONMENT");
        return NativeCoreResult<ISchemaAssessmentAdvisor>.Supported(new Advisor(this, environment));
    }

    public bool Owns(ISchemaComponentHandle handle) => handle is Component component && ReferenceEquals(component.Provider, this) && components.ContainsValue(component);
    public SchemaResult<ISchemaComponentHandle> ResolveType(SchemaExpandedName name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return name.NamespaceUri == "http://www.w3.org/2001/XMLSchema" && name.LocalName == "decimal"
            ? NativeCoreResult<ISchemaComponentHandle>.Supported(DecimalType)
            : NativeCoreResult<ISchemaComponentHandle>.Unsupported("BSK-NATIVE-TYPE");
    }
    public SchemaResult<ISchemaComponentHandle> ResolveElementDeclaration(SchemaExpandedName name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return name.NamespaceUri.Length == 0 && name.LocalName == schema.Root.Name
            ? NativeCoreResult<ISchemaComponentHandle>.Supported(RootDeclaration)
            : NativeCoreResult<ISchemaComponentHandle>.NotFound();
    }
    public SchemaResult<ISchemaComponentHandle> ResolveAttributeDeclaration(SchemaExpandedName name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return NativeCoreResult<ISchemaComponentHandle>.Unsupported("BSK-NATIVE-ATTRIBUTES");
    }
    public SchemaResult<bool> IsSameComponent(ISchemaComponentHandle left, ISchemaComponentHandle right, CancellationToken cancellationToken)
    {
        Validate(left, cancellationToken);
        Validate(right, cancellationToken);
        return NativeCoreResult<bool>.Supported(ReferenceEquals(left, right));
    }
    public SchemaResult<bool> IsDerivedFrom(ISchemaComponentHandle derived, ISchemaComponentHandle baseType, CancellationToken cancellationToken)
    {
        Validate(derived, cancellationToken); Validate(baseType, cancellationToken);
        return NativeCoreResult<bool>.Unsupported("BSK-NATIVE-DERIVATION");
    }
    public SchemaResult<bool> IsAffiliatedWith(ISchemaComponentHandle member, ISchemaComponentHandle head, CancellationToken cancellationToken)
    {
        Validate(member, cancellationToken); Validate(head, cancellationToken);
        return NativeCoreResult<bool>.Unsupported("BSK-NATIVE-AFFILIATION");
    }
    public SchemaResult<ImmutableArray<ISchemaComponentHandle>> GetComponentClosure(ISchemaComponentHandle component, CancellationToken cancellationToken)
    {
        Validate(component, cancellationToken);
        return NativeCoreResult<ImmutableArray<ISchemaComponentHandle>>.Unsupported("BSK-NATIVE-CLOSURE");
    }
    private void Validate(ISchemaComponentHandle component, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!Owns(component)) throw new ArgumentException("Foreign or unissued component.", nameof(component));
    }

    internal sealed class Component(NativeSchemaProvider provider) : ISchemaComponentHandle
    {
        internal NativeSchemaProvider Provider => provider;
        public ISchemaAuthority Authority => provider;
    }

    internal sealed class Advisor(NativeSchemaProvider provider, IQuerySemanticEnvironment environment) : ISchemaAssessmentAdvisor
    {
        public ISchemaAssessor Assessor => provider;
        public IQuerySemanticEnvironment QueryEnvironment => environment;
        public void ThrowIfUnavailable(CancellationToken cancellationToken) => cancellationToken.ThrowIfCancellationRequested();
        public bool OwnsAssessment(ISchemaAssessmentOrigin origin) =>
            origin is Origin issued && ReferenceEquals(issued.Advisor, this) && issued.Input.IsIssued(issued);
        public bool OwnsNodeFacts(ISchemaNodeFacts facts) => facts is Facts issued && OwnsAssessment(issued.Origin) && issued.Input.OwnsFacts(issued);
        public bool OwnsNode(IXdmNode node, ISchemaNodeFacts facts, ISchemaAssessmentInput input, ISchemaAssessmentOrigin origin) =>
            facts is Facts issued && OwnsNodeFacts(facts) && OwnsAssessment(origin) && ReferenceEquals(issued.Origin, origin) &&
            ReferenceEquals(issued.Node, node) && ReferenceEquals(issued.Input, input);
        public bool OwnsTypedValue(ISchemaTypedValue value, ISchemaAssessmentInput input, ISchemaAssessmentOrigin? originalAssessment) =>
            value is IssuedValue issued && OwnsAssessment(issued.Origin) &&
            ReferenceEquals(issued.Input, input) && ReferenceEquals(issued.Origin, originalAssessment);

        public ISchemaAssessmentResult Assess(SchemaAssessmentRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(request);
            if (request.Limits is { } limits && (limits.MaxDiagnostics <= 0 || limits.MaxDiagnosticCharacters <= 0))
                throw new ArgumentException("Diagnostic limits must be positive.", nameof(request));
            // Check concrete host issuance before any foreign input getters.
            if (request.Input is not NativeSchemaInput input)
                return Assessment.Failure(SchemaAssessmentStatus.Unsupported, "BSK-NATIVE-INPUT", "Foreign capture is unsupported.", request.Limits);
            using var lease = input.Enter(cancellationToken);
            if (request.Episode is null) throw new ArgumentException("An assessment episode is required.", nameof(request));
            var date = request.Episode.CurrentDateTime;
            var timezone = request.Episode.ImplicitTimezoneOffsetMinutes;
            if (timezone is < -840 or > 840) throw new ArgumentException("Invalid implicit timezone.", nameof(request));
            if (request.RequestedType is not null) provider.Validate(request.RequestedType, cancellationToken);
            if (request.GoverningDeclaration is not null) provider.Validate(request.GoverningDeclaration, cancellationToken);
            if (request.Mode != SchemaAssessmentMode.Strict || request.RequestedType is not null ||
                request.GoverningDeclaration is not null && !ReferenceEquals(request.GoverningDeclaration, provider.RootDeclaration))
                return Assessment.Failure(SchemaAssessmentStatus.Unsupported, "BSK-NATIVE-MODE", "Only strict root assessment is supported.", request.Limits);
            if (input.IsAssessed)
                return Assessment.Failure(SchemaAssessmentStatus.Unsupported, "BSK-NATIVE-REASSESSMENT", "Original assessment cannot be replaced.", request.Limits);
            var result = provider.schema.Assess(input.Xml, cancellationToken);
            if (result.Outcome != NativeOutcome.Valid)
                return Assessment.Failure(result.Outcome == NativeOutcome.Invalid ? SchemaAssessmentStatus.Invalid : SchemaAssessmentStatus.Unsupported,
                    "BSK-NATIVE-" + result.Outcome.ToString().ToUpperInvariant(), result.Diagnostic!, request.Limits);
            var origin = new Origin(this, input, date, timezone);
            var states = result.Value!.Elements;
            var facts = states.Select((state, index) => new Facts(input, input.ElementNodes[index], origin,
                provider.components[state.Declaration.Type], provider.components[state.Declaration], state)).ToImmutableArray();
            cancellationToken.ThrowIfCancellationRequested();
            if (!input.Publish(origin, facts))
                return Assessment.Failure(SchemaAssessmentStatus.Unsupported, "BSK-NATIVE-REASSESSMENT", "Original assessment cannot be replaced.", request.Limits);
            return new Assessment(input, origin, facts);
        }

        public SchemaResult<ISchemaTypedValue> ConstructSimpleValue(XdmValue input, ISchemaComponentHandle targetType,
            ISchemaAssessmentEpisode episode, CancellationToken cancellationToken)
        {
            provider.Validate(targetType, cancellationToken);
            return NativeCoreResult<ISchemaTypedValue>.Unsupported("BSK-NATIVE-CONSTRUCTION");
        }
    }

    internal sealed class Origin(Advisor advisor, NativeSchemaInput input, DateTimeOffset date, int timezone) : ISchemaAssessmentOrigin
    {
        internal Advisor Advisor => advisor;
        internal NativeSchemaInput Input => input;
        internal DateTimeOffset Date { get; } = date;
        internal int Timezone { get; } = timezone;
        public ISchemaSourceBinding Source => input.Source;
    }

    internal sealed class Facts : ISchemaNodeFacts
    {
        private readonly ISchemaComponentHandle type;
        private readonly ISchemaComponentHandle declaration;
        private readonly Origin origin;
        private readonly NativeElementState state;
        private readonly IssuedValue? value;
        internal Facts(NativeSchemaInput input, NativeSessionNode node, Origin origin, Component type, Component declaration, NativeElementState state)
        {
            Input = input; Node = node; this.origin = origin; this.type = type; this.declaration = declaration; this.state = state;
            if (state.TypedValue is { } scalar) value = new IssuedValue(input, origin, type, scalar);
        }
        internal NativeSchemaInput Input { get; }
        internal NativeSessionNode Node { get; }
        public ISchemaComponentHandle Type { get { Input.Check(); return type; } }
        public ISchemaComponentHandle Declaration { get { Input.Check(); return declaration; } }
        public ISchemaAssessmentOrigin Origin => origin;
        public bool HasNoTypedValue { get { Input.Check(); return state.Declaration.Type.HasNoTypedValue; } }
        public bool IsNilled { get { Input.Check(); return false; } }
        public SchemaResult<ISchemaTypedValue> ReadTypedValue(CancellationToken cancellationToken)
        {
            Input.Check(cancellationToken);
            return value is null ? NativeCoreResult<ISchemaTypedValue>.Unsupported("FOTY0012")
                : NativeCoreResult<ISchemaTypedValue>.Supported(value);
        }
    }

    private sealed class IssuedValue(NativeSchemaInput input, Origin origin, Component type, XdmExactDecimal scalar) : ISchemaTypedValue
    {
        internal NativeSchemaInput Input => input;
        internal Origin Origin => origin;
        public ISchemaComponentHandle GoverningType { get { input.Check(); return type; } }
        public bool IsList { get { input.Check(); return false; } }
        public IEnumerable<SchemaAtomicItem> AtomicItems
        {
            get { input.Check(); return Enumerate(); }
        }
        private IEnumerable<SchemaAtomicItem> Enumerate()
        {
            input.Check();
            yield return Item();
        }
        private SchemaAtomicItem Item() => new(XdmValue.FromDecimal(scalar), new Provenance(input, origin, type));
        public bool TryGetSingleAtomic(out SchemaAtomicItem item)
        {
            input.Check();
            item = Item();
            return true;
        }
    }

    private sealed class Provenance(NativeSchemaInput input, Origin origin, Component type) : ISchemaAtomicProvenance
    {
        public ISchemaComponentHandle DeclaredType { get { input.Check(); return type; } }
        public ISchemaComponentHandle ActualType { get { input.Check(); return type; } }
        public ISchemaAssessmentOrigin OriginalAssessment { get { input.Check(); return origin; } }
    }

    internal sealed class Assessment : ISchemaAssessmentResult, ISchemaAssessmentEvidence
    {
        private readonly NativeSchemaInput? input;
        private readonly Origin? origin;
        internal Assessment(NativeSchemaInput input, Origin origin, ImmutableArray<Facts> facts)
        {
            this.input = input; this.origin = origin; Facts = facts; Status = SchemaAssessmentStatus.Valid;
        }
        private Assessment(SchemaAssessmentStatus status, ImmutableArray<SchemaAssessmentDiagnostic> diagnostics, bool truncated)
        {
            Status = status; Diagnostics = diagnostics; DiagnosticsTruncated = truncated;
        }
        internal ImmutableArray<Facts> Facts { get; } = [];
        public SchemaAssessmentStatus Status { get; }
        public ISchemaAssessmentEvidence? Evidence => origin is null ? null : this;
        public IXdmNode? ValidatedRoot { get { input?.Check(); return input?.Root; } }
        public ImmutableArray<SchemaAssessmentDiagnostic> Diagnostics { get; } = [];
        public bool DiagnosticsTruncated { get; }
        public ISchemaAssessmentOrigin Origin => origin ?? throw new InvalidOperationException("No original evidence.");
        public bool IsComplete => Status == SchemaAssessmentStatus.Valid;
        internal static Assessment Failure(SchemaAssessmentStatus status, string code, string message, SchemaAssessmentLimits? limits)
        {
            var budget = 256L;
            if (limits is not null)
            {
                if (limits.MaxDiagnostics <= 0 || limits.MaxDiagnosticCharacters <= 0) throw new ArgumentException("Diagnostic limits must be positive.", nameof(limits));
                budget = Math.Min(budget, limits.MaxDiagnosticCharacters);
            }
            if (budget < code.Length) return new Assessment(status, [], true);
            var length = (int)Math.Min(message.Length, budget - code.Length);
            return new Assessment(status, [new SchemaAssessmentDiagnostic(code, message[..length], null)], length < message.Length);
        }
    }
}

internal sealed class NativeCoreResult<T> : SchemaResult<T>
{
    private readonly T value;
    private NativeCoreResult(SchemaResultKind kind, T value, string? code) { Kind = kind; this.value = value; DiagnosticCode = code; }
    public override SchemaResultKind Kind { get; }
    public override T Value => Kind == SchemaResultKind.Supported ? value : throw new InvalidOperationException("Only Supported has a value.");
    public override string? DiagnosticCode { get; }
    internal static NativeCoreResult<T> Supported(T value) => new(SchemaResultKind.Supported, value, null);
    internal static NativeCoreResult<T> Unsupported(string code) => new(SchemaResultKind.Unsupported, default!, code);
    internal static NativeCoreResult<T> NotFound() => new(SchemaResultKind.NotFound, default!, null);
}
