// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Probes instance-selected native types with original component identity.
// SPECIAL NOTES        : Isolated feasibility candidate only; production adoption remains pending.
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
using System.Xml;
using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private static readonly XNamespace Xsi = "http://www.w3.org/2001/XMLSchema-instance";
    private static readonly HashSet<string> KnownBuiltinNames = new(StringComparer.Ordinal)
    { "anySimpleType", "anyAtomicType", "string", "boolean", "decimal", "float", "double", "duration", "dateTime", "time", "date", "gYearMonth", "gYear", "gMonthDay", "gDay", "gMonth", "hexBinary", "base64Binary", "anyURI", "QName", "NOTATION", "normalizedString", "token", "language", "NMTOKEN", "NMTOKENS", "Name", "NCName", "ID", "IDREF", "IDREFS", "ENTITY", "ENTITIES", "integer", "nonPositiveInteger", "negativeInteger", "long", "int", "short", "byte", "nonNegativeInteger", "unsignedLong", "unsignedInt", "unsignedShort", "unsignedByte", "positiveInteger", "yearMonthDuration", "dayTimeDuration", "dateTimeStamp", "anyType", "error" };
    private ImmutableDictionary<XName, NativeType> instanceTypes = ImmutableDictionary<XName, NativeType>.Empty;

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
        if (IntegerType is { } integer) integer.BaseType = DecimalType;
        foreach (var type in all)
        {
            if (type.BaseName is { } parent) type.BaseType = named[parent];
            else if (type.ContentType is { } content) type.BaseType = content;
        }
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
        for (NativeType? current = selected; current is not null && visited.Add(current); current = current.BaseType)
            if (ReferenceEquals(current, declared)) { derives = true; break; }
        Check(derives, "Instance type does not derive from the declared type.");
        Require(!selected.IsComplex && !declared.IsComplex && selected.IsString == declared.IsString &&
            selected.IsBoolean == declared.IsBoolean && selected.IsInteger == declared.IsInteger,
            "Valid type derivation changes the admitted scalar representation.", NativeDecimalUnsupportedReason.ValueType);
        return selected;
    }

    private NativeElementState AssessElementScalar(XElement element, NativeDeclaration declaration)
    {
        var selected = ResolveInstanceType(element, declaration.Type);
        var governing = new NativeDeclaration(declaration.Name, selected, declaration.MinOccurs, declaration.MaxOccurs, declaration.NamespaceUri);
        return AssessScalar(element.Value, governing) with { Declaration = declaration, EffectiveType = selected };
    }
}
