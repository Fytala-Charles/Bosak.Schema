// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Binds native schema assessment to original authority-owned Core facts.
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
//                      | Charles Korthout | 0.2   | 15-11-2026     | Reject uninitialized lookup names with explicit argument diagnostics                        |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Carry bounded Unsupported reasons without changing refusal outcomes                       |
//                      | Charles Korthout | 0.4   | 19-11-2026     | Adopt bounded expanded names and optional native assertions                              |
//                      | Charles Korthout | 0.5   | 19-11-2026     | Adopt bounded named complex types and shared expanded component identity                  |
//                      | Charles Korthout | 0.6   | 19-11-2026     | Adopt named decimal restrictions and original named scalar type identity                  |
//                      | Charles Korthout | 0.7   | 19-11-2026     | Share explicit mixed scalar profile with preserved decimal compatibility                  |
//                      | Charles Korthout | 0.8   | 19-11-2026     | Add bounded opt-in Boolean values while preserving decimal and string contracts             |
//                      | Charles Korthout | 0.9   | 19-11-2026     | Add bounded exact integer leaves with original typed provenance                           |
//                      | Charles Korthout | 1.0   | 19-11-2026     | Publish complete original attribute facts with separate element and attribute collections |
//                      | Charles Korthout | 1.1   | 20-11-2026     | Retain complex root governing identity with its distinct decimal atomic content type     |
//                      | Charles Korthout | 1.2   | 20-11-2026     | Register nested declaration components and project terminal scalar facts separately      |
//                      | Charles Korthout | 1.3   | 20-11-2026     | Publish exact original owner-aware attributes with separate root and descendant projections |
//                      | Charles Korthout | 1.4   | 20-11-2026     | Register actual decimal content types for nested original complex terminal facts         |
//                      | Charles Korthout | 1.5   | 20-11-2026     | Adopt opt-in original-node data binding; preserve scalar default          |
//                      | Charles Korthout | 1.6   | 21-11-2026     | Adopt bounded instance selection with original declaration and governing type identity |
//                      | Charles Korthout | 1.8   | 21-11-2026     | Adopt bounded complex restriction with original and atomic component identities |
//                      | Charles Korthout | 1.9   | 21-11-2026     | Probe empty complex typing in isolated copies only |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

/// <summary>Binds native schema assessment to original authority-owned Core facts.</summary>
internal sealed class NativeSchemaProvider : ISchemaAssessor, ISchemaAuthority
{
    private readonly NativeDecimalSchema schema;
    private readonly bool originalNodeData;
    private readonly string? expectedNodeFingerprint;
    private readonly Dictionary<object, Component> components = new(ReferenceEqualityComparer.Instance);
    internal Component RootDeclaration { get; }
    internal Component DecimalType { get; }

    internal NativeSchemaProvider(NativeDecimalSchema schema, bool originalNodeData = false, string? expectedNodeFingerprint = null)
    {
        this.schema = schema;
        this.originalNodeData = originalNodeData;
        this.expectedNodeFingerprint = expectedNodeFingerprint;
        RootDeclaration = Add(schema.Root);
        foreach (var plan in schema.Roots)
        {
            Add(plan.Root); Add(plan.Root.Type);
            if (plan.Root.Type.ContentType is { } contentType) Add(contentType);
            RegisterChildren(plan);
        }
        foreach (var type in schema.NamedTypes) Add(type);
        DecimalType = Add(schema.DecimalType);
        if (schema.StringType is { } stringType) Add(stringType);
        if (schema.BooleanType is { } booleanType) Add(booleanType);
        if (schema.IntegerType is { } integerType) Add(integerType);
    }

    private void RegisterChildren(NativeDecimalSchema plan)
    {
        foreach (var declaration in plan.Children.Concat(plan.AttributeUses))
        {
            var known = components.ContainsKey(declaration);
            Add(declaration); Add(declaration.Type);
            if (declaration.Type.ContentType is { } contentType) Add(contentType);
            if (!known && declaration.Container is { } child) RegisterChildren(child);
        }
    }

    // Typed lookup of an existing owned native component; never registers or fabricates authority.
    internal Component TypeComponent(NativeType type) => components[type];

    private Component Add(object value)
    {
        if (components.TryGetValue(value, out var existing)) return existing;
        var component = new Component(this);
        components.Add(value, component);
        if (value is NativeType type)
        {
            if (type.ContentType is { } content) Add(content);
            if (type.DerivationBase is { } parent) Add(parent);
        }
        return component;
    }

    public ISchemaAuthority Authority => this;
    public SchemaLanguageVersion SchemaLanguage => SchemaLanguageVersion.Xsd11;
    public string AssessmentProfile => "schema-native-decimal-session-v1";

    public SchemaResult<ISchemaAssessmentAdvisor> Bind(IQuerySemanticEnvironment environment, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(environment);
        // Retain scalar default; original data mode requires the captured default-operation fingerprint.
        if (environment.GetType().Assembly != typeof(SchemaQuerySession).Assembly ||
            environment.HostLanguage != QueryHostLanguage.XPath || environment.HostVersion != "3.1" ||
            environment.XdmSpecificationRevision.AbsoluteUri != "https://www.w3.org/TR/2017/REC-xpath-datamodel-31-20170321/" ||
            environment.BehaviorProfile != (originalNodeData ? "bosak-original-node-data-v1" : "bosak-typed-identity-v1") ||
            environment.DescriptorFormatVersion != (originalNodeData ? "bosak-original-node-data-descriptor-v1" : "bosak-public-typed-identity-descriptor-v1") ||
            string.IsNullOrEmpty(environment.StaticContextFingerprint) ||
            environment.StaticContext.Namespaces.Count != 0 ||
            originalNodeData && (expectedNodeFingerprint is null || environment.StaticContextFingerprint != expectedNodeFingerprint))
            return NativeCoreResult<ISchemaAssessmentAdvisor>.Unsupported("BSK-NATIVE-ENVIRONMENT");
        return NativeCoreResult<ISchemaAssessmentAdvisor>.Supported(new Advisor(this, environment));
    }

    public bool Owns(ISchemaComponentHandle handle) => handle is Component component && ReferenceEquals(component.Provider, this) && components.ContainsValue(component);
    public SchemaResult<ISchemaComponentHandle> ResolveType(SchemaExpandedName name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateName(name);
        if (name.NamespaceUri == "http://www.w3.org/2001/XMLSchema" && name.LocalName == "decimal")
            return NativeCoreResult<ISchemaComponentHandle>.Supported(DecimalType);
        if (name.NamespaceUri == "http://www.w3.org/2001/XMLSchema" && name.LocalName == "string" && schema.StringType is { } stringType)
            return NativeCoreResult<ISchemaComponentHandle>.Supported(components[stringType]);
        if (name.NamespaceUri == "http://www.w3.org/2001/XMLSchema" && name.LocalName == "boolean" && schema.BooleanType is { } booleanType)
            return NativeCoreResult<ISchemaComponentHandle>.Supported(components[booleanType]);
        if (name.NamespaceUri == "http://www.w3.org/2001/XMLSchema" && name.LocalName == "integer" && schema.IntegerType is { } integerType)
            return NativeCoreResult<ISchemaComponentHandle>.Supported(components[integerType]);
        var type = schema.NamedTypes.FirstOrDefault(t => t.Name == name.LocalName && t.NamespaceUri == name.NamespaceUri);
        return type is not null ? NativeCoreResult<ISchemaComponentHandle>.Supported(components[type])
            : NativeCoreResult<ISchemaComponentHandle>.Unsupported("BSK-NATIVE-TYPE");
    }
    public SchemaResult<ISchemaComponentHandle> ResolveElementDeclaration(SchemaExpandedName name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateName(name);
        var root = schema.Roots.FirstOrDefault(p => p.Root.NamespaceUri == name.NamespaceUri && p.Root.Name == name.LocalName);
        return root is not null ? NativeCoreResult<ISchemaComponentHandle>.Supported(components[root.Root])
            : NativeCoreResult<ISchemaComponentHandle>.NotFound();
    }
    public SchemaResult<ISchemaComponentHandle> ResolveAttributeDeclaration(SchemaExpandedName name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ValidateName(name);
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
    private static void ValidateName(SchemaExpandedName name)
    {
        if (name.NamespaceUri is null || string.IsNullOrEmpty(name.LocalName))
            throw new ArgumentException("Lookup names require a namespace string and nonempty local name.", nameof(name));
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
                request.GoverningDeclaration is not null && !provider.schema.Roots.Any(p => ReferenceEquals(request.GoverningDeclaration, provider.components[p.Root])))
                return Assessment.Failure(SchemaAssessmentStatus.Unsupported, "BSK-NATIVE-MODE", "Only strict root assessment is supported.", request.Limits);
            if (input.IsAssessed)
                return Assessment.Failure(SchemaAssessmentStatus.Unsupported, "BSK-NATIVE-REASSESSMENT", "Original assessment cannot be replaced.", request.Limits);
            var result = provider.schema.Assess(input.Xml, cancellationToken);
            if (result.Outcome != NativeOutcome.Valid)
                return Assessment.Failure(result.Outcome == NativeOutcome.Invalid ? SchemaAssessmentStatus.Invalid : SchemaAssessmentStatus.Unsupported,
                    "BSK-NATIVE-" + result.Outcome.ToString().ToUpperInvariant(), result.Diagnostic!, request.Limits, result.UnsupportedReason);
            if (request.GoverningDeclaration is not null &&
                !ReferenceEquals(request.GoverningDeclaration, provider.components[result.Value!.Elements[0].Declaration]))
                return Assessment.Failure(SchemaAssessmentStatus.Unsupported, "BSK-NATIVE-MODE", "Governing declaration does not match root.", request.Limits);
            var origin = new Origin(this, input, date, timezone);
            var states = result.Value!.Elements;
            var facts = states.Select((state, index) => new Facts(input, input.ElementNodes[index], origin,
                provider.TypeComponent(state.GoverningType), provider.components[state.Declaration], state,
                provider.TypeComponent(state.GoverningType.ContentType ?? state.GoverningType))).ToImmutableArray();
            cancellationToken.ThrowIfCancellationRequested();
            var attributeFacts = result.Value.Attributes.Select(state => new Facts(input, input.ElementAttributeNode(state.OwnerElementIndex, state.OwnerElementPath, states[state.OwnerElementIndex].Declaration.ExpandedName, state.Declaration.ExpandedName), origin,
                provider.TypeComponent(state.GoverningType), provider.components[state.Declaration], state,
                provider.TypeComponent(state.GoverningType.ContentType ?? state.GoverningType))).ToImmutableArray();
            cancellationToken.ThrowIfCancellationRequested();
            if (!input.Publish(origin, facts.AddRange(attributeFacts)))
                return Assessment.Failure(SchemaAssessmentStatus.Unsupported, "BSK-NATIVE-REASSESSMENT", "Original assessment cannot be replaced.", request.Limits);
            return new Assessment(input, origin, facts, attributeFacts);
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
        internal Facts(NativeSchemaInput input, NativeSessionNode node, Origin origin, Component type, Component declaration, NativeElementState state, Component? actualType = null)
        {
            Input = input; Node = node; this.origin = origin; this.type = type; this.declaration = declaration; this.state = state;
            if (state.GoverningType.EmptyContent) value = new IssuedValue(input, origin, type, XdmValue.Undefined, empty: true);
            else if (state.TypedValue is { } scalar) value = new IssuedValue(input, origin, type, XdmValue.FromDecimal(scalar), actualType);
            else if (state.StringValue is { } text) value = new IssuedValue(input, origin, type, XdmValue.FromString(text), actualType);
            else if (state.BooleanValue is { } logical) value = new IssuedValue(input, origin, type, XdmValue.FromBoolean(logical), actualType);
            else if (state.IntegerValue is { } integer) value = new IssuedValue(input, origin, type, XdmValue.FromInteger(integer), actualType);
        }
        internal NativeSchemaInput Input { get; }
        internal NativeSessionNode Node { get; }
        internal string? NormalizedStringValue { get { Input.Check(); return state.NormalizedStringValue; } }
        internal ImmutableArray<int> OwnerElementPath { get { Input.Check(); return state.OwnerElementPath; } }
        internal NativeDeclaration NativeDeclaration { get { Input.Check(); return state.Declaration; } }
        internal NativeType NativeType { get { Input.Check(); return state.GoverningType; } }
        public ISchemaComponentHandle Type { get { Input.Check(); return type; } }
        public ISchemaComponentHandle Declaration { get { Input.Check(); return declaration; } }
        public ISchemaAssessmentOrigin Origin => origin;
        public bool HasNoTypedValue { get { Input.Check(); return state.GoverningType.HasNoTypedValue; } }
        public bool IsNilled { get { Input.Check(); return false; } }
        public SchemaResult<ISchemaTypedValue> ReadTypedValue(CancellationToken cancellationToken)
        {
            Input.Check(cancellationToken);
            return value is null ? NativeCoreResult<ISchemaTypedValue>.Unsupported("FOTY0012")
                : NativeCoreResult<ISchemaTypedValue>.Supported(value);
        }
    }

    private sealed class IssuedValue(NativeSchemaInput input, Origin origin, Component type, XdmValue scalar, Component? actualType = null, bool empty = false) : ISchemaTypedValue
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
            if (!empty) yield return Item();
        }
        private SchemaAtomicItem Item() => new(scalar, new Provenance(input, origin, type, actualType ?? type));
        public bool TryGetSingleAtomic(out SchemaAtomicItem item)
        {
            input.Check();
            if (empty) { item = default; return false; }
            item = Item();
            return true;
        }
    }

    private sealed class Provenance(NativeSchemaInput input, Origin origin, Component type, Component actualType) : ISchemaAtomicProvenance
    {
        public ISchemaComponentHandle DeclaredType { get { input.Check(); return type; } }
        public ISchemaComponentHandle ActualType { get { input.Check(); return actualType; } }
        public ISchemaAssessmentOrigin OriginalAssessment { get { input.Check(); return origin; } }
    }

    internal sealed class Assessment : ISchemaAssessmentResult, ISchemaAssessmentEvidence
    {
        private readonly NativeSchemaInput? input;
        private readonly Origin? origin;
        internal Assessment(NativeSchemaInput input, Origin origin, ImmutableArray<Facts> facts, ImmutableArray<Facts> attributeFacts = default)
        {
            this.input = input; this.origin = origin; ElementFacts = facts;
            var allAttributes = attributeFacts.IsDefault ? [] : attributeFacts;
            AttributeFacts = allAttributes.Where(f => f.OwnerElementPath.IsEmpty).ToImmutableArray();
            DescendantAttributeFacts = allAttributes.Where(f => !f.OwnerElementPath.IsEmpty).ToImmutableArray();
            ScalarFacts = facts.Skip(1).Where(f => !f.HasNoTypedValue && !f.NativeType.EmptyContent).ToImmutableArray();
            Facts = facts.AddRange(allAttributes); Status = SchemaAssessmentStatus.Valid;
        }
        private Assessment(SchemaAssessmentStatus status, ImmutableArray<SchemaAssessmentDiagnostic> diagnostics, bool truncated,
            NativeDecimalUnsupportedReason reason)
        {
            Status = status; Diagnostics = diagnostics; DiagnosticsTruncated = truncated; UnsupportedReason = reason;
        }
        internal NativeDecimalUnsupportedReason UnsupportedReason { get; }
        internal ImmutableArray<Facts> Facts { get; } = [];
        internal ImmutableArray<Facts> ElementFacts { get; } = [];
        internal ImmutableArray<Facts> ScalarFacts { get; } = [];
        internal ImmutableArray<Facts> AttributeFacts { get; } = [];
        internal ImmutableArray<Facts> DescendantAttributeFacts { get; } = [];
        public SchemaAssessmentStatus Status { get; }
        public ISchemaAssessmentEvidence? Evidence => origin is null ? null : this;
        public IXdmNode? ValidatedRoot { get { input?.Check(); return input?.Root; } }
        public ImmutableArray<SchemaAssessmentDiagnostic> Diagnostics { get; } = [];
        public bool DiagnosticsTruncated { get; }
        public ISchemaAssessmentOrigin Origin => origin ?? throw new InvalidOperationException("No original evidence.");
        public bool IsComplete => Status == SchemaAssessmentStatus.Valid;
        internal static Assessment Failure(SchemaAssessmentStatus status, string code, string message, SchemaAssessmentLimits? limits,
            NativeDecimalUnsupportedReason reason = NativeDecimalUnsupportedReason.Unspecified)
        {
            var budget = 256L;
            if (limits is not null)
            {
                if (limits.MaxDiagnostics <= 0 || limits.MaxDiagnosticCharacters <= 0) throw new ArgumentException("Diagnostic limits must be positive.", nameof(limits));
                budget = Math.Min(budget, limits.MaxDiagnosticCharacters);
            }
            if (budget < code.Length) return new Assessment(status, [], true, reason);
            var length = (int)Math.Min(message.Length, budget - code.Length);
            return new Assessment(status, [new SchemaAssessmentDiagnostic(code, message[..length], null)], length < message.Length, reason);
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
