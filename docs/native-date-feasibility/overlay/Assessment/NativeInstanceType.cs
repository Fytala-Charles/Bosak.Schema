// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Resolves bounded instance-selected types with original component identity.
// SPECIAL NOTES        : Internal mixed profile; immutable component graph and closed type lookup.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 21-11-2026     | Adopt bounded complex restriction with original and atomic component identities |
//                      | Charles Korthout | 0.3   | 22-11-2026     | Prototype bounded string patterns and per-assessment accounting |
//                      | Charles Korthout | 0.4   | 22-11-2026     | Adopt qualified bounded native string patterns |
//                      | Charles Korthout | 9.0   | 24-11-2026     | Probe revised bounded date transport in isolated source copy |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private static readonly XNamespace Xsi = "http://www.w3.org/2001/XMLSchema-instance";
    private static readonly ImmutableHashSet<string> KnownBuiltinNames = ImmutableHashSet.Create(StringComparer.Ordinal,
      "anySimpleType", "anyAtomicType", "string", "boolean", "decimal", "float", "double", "duration", "dateTime", "time", "date", "gYearMonth", "gYear", "gMonthDay", "gDay", "gMonth", "hexBinary", "base64Binary", "anyURI", "QName", "NOTATION", "normalizedString", "token", "language", "NMTOKEN", "NMTOKENS", "Name", "NCName", "ID", "IDREF", "IDREFS", "ENTITY", "ENTITIES", "integer", "nonPositiveInteger", "negativeInteger", "long", "int", "short", "byte", "nonNegativeInteger", "unsignedLong", "unsignedInt", "unsignedShort", "unsignedByte", "positiveInteger", "yearMonthDuration", "dayTimeDuration", "dateTimeStamp", "anyType", "error");
    private ImmutableDictionary<XName, NativeType> instanceTypes = ImmutableDictionary<XName, NativeType>.Empty;

    private ImmutableDictionary<NativeType, NativeType> instanceBases = ImmutableDictionary<NativeType, NativeType>.Empty;

    // Constructed once, before the compiled schema is exposed to any assessor.
    private void PrepareInstanceTypes()
    {
        var named = NamedTypes.Concat(new NativeType?[] { DecimalType, StringType, BooleanType, IntegerType }.OfType<NativeType>())
            .Where(type => type.Name is not null).ToImmutableDictionary(type => XName.Get(type.Name!, type.NamespaceUri));
        instanceTypes = named;
        var all = new HashSet<NativeType>(ReferenceEqualityComparer.Instance);
        var seenPlans = new HashSet<NativeDecimalSchema>(ReferenceEqualityComparer.Instance);
        void AddType(NativeType type)
        {
            if (!all.Add(type)) return;
            if (type.ContentType is { } content) AddType(content);
            if (type.DerivationBase is { } parent) AddType(parent);
        }
        void Visit(NativeDecimalSchema plan)
        {
            if (!seenPlans.Add(plan)) return;
            AddType(plan.Root.Type);
            foreach (var declaration in plan.Children.Concat(plan.AttributeUses))
            {
                AddType(declaration.Type);
                if (declaration.Container is { } child) Visit(child);
            }
        }
        foreach (var type in named.Values) AddType(type);
        foreach (var plan in Roots) Visit(plan);
        var bases = ImmutableDictionary.CreateBuilder<NativeType, NativeType>();
        if (IntegerType is { } integer) bases.Add(integer, DecimalType);
        foreach (var type in all)
        {
            if (type.DerivationBase is { } direct) bases.Add(type, direct);
            else if (type.BaseName is { } parent) bases.Add(type, named[parent]);
            else if (type.ContentType is { } content) bases.Add(type, content);
        }
        instanceBases = bases.ToImmutable();
    }

    // Admission of the special attribute is separate from hint ignoring. Every assessed
    // element invokes ResolveInstanceType before this helper can admit its attributes.
    private bool IsAdmittedInstanceAttribute(XAttribute attribute) => IsNamespaceOrLocationHint(attribute) ||
        maxStringCharacters is not null && attribute.Name == Xsi + "type";

    private NativeType ResolveInstanceType(XElement element, NativeType declared)
    {
        var request = element.Attribute(Xsi + "type");
        if (request is null) return declared;
        Require(maxStringCharacters is not null, "Instance type selection is outside the decimal-only profile.", NativeDecimalUnsupportedReason.Attributes);
        Require(request.Value.Length <= 256, "Instance type QName capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var token = Collapse(request.Value);
        var parts = token.Split(':');
        Check(parts.Length is 1 or 2 && parts.All(part => part.Length != 0), "Invalid instance type QName.");
        try { foreach (var part in parts) XmlConvert.VerifyNCName(part); }
        catch (Exception error) when (error is XmlException or ArgumentException)
        { throw new NativeProfileException(NativeOutcome.Invalid, "Invalid instance type QName."); }
        var ns = parts.Length == 1 ? element.GetDefaultNamespace().NamespaceName : element.GetNamespaceOfPrefix(parts[0])?.NamespaceName;
        Check(ns is not null, "Instance type prefix is unbound.");
        if (!instanceTypes.TryGetValue(XName.Get(parts[^1], ns!), out var selected))
        {
            if (ns == Xs.NamespaceName && KnownBuiltinNames.Contains(parts[^1]))
                throw new NativeProfileException(NativeOutcome.Unsupported, "Known instance type is outside the native profile.", NativeDecimalUnsupportedReason.ValueType);
            throw new NativeProfileException(NativeOutcome.Invalid, "Instance type is absent from the closed schema.");
        }
        if (ReferenceEquals(selected, declared)) return selected;
        var derives = false;
        var visited = new HashSet<NativeType>(ReferenceEqualityComparer.Instance);
        for (NativeType? current = selected; current is not null && visited.Add(current); current = instanceBases.GetValueOrDefault(current))
            if (ReferenceEquals(current, declared)) { derives = true; break; }
        Check(derives, "Instance type does not derive from the declared type.");
        Require(!selected.IsComplex && !declared.IsComplex && selected.IsString == declared.IsString &&
            selected.IsDate == declared.IsDate && selected.IsBoolean == declared.IsBoolean && selected.IsInteger == declared.IsInteger,
            "Valid type derivation changes the admitted scalar representation.", NativeDecimalUnsupportedReason.ValueType);
        return selected;
    }

    private NativeElementState AssessElementScalar(XElement element, NativeDeclaration declaration, NativePatternBudget patternBudget)
    {
        var selected = ResolveInstanceType(element, declaration.Type);
        var governing = new NativeDeclaration(declaration.Name, selected, declaration.MinOccurs, declaration.MaxOccurs, declaration.NamespaceUri);
        return AssessScalar(element.Value, governing, patternBudget) with { Declaration = declaration, EffectiveType = selected };
    }
}
