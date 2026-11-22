// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Shares direct restriction parsing and bounds original local anonymous definitions.
// SPECIAL NOTES        : Internal bounded profile; anonymous identities never acquire invented QNames.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 20-11-2026     | Admit direct opt-in integer restrictions with exact original type identity                 |
//                      | Charles Korthout | 0.3   | 21-11-2026     | Adopt bounded instance selection with original declaration and governing type identity |
//                      | Charles Korthout | 0.4   | 22-11-2026     | Prototype bounded string patterns and per-assessment accounting |
//                      | Charles Korthout | 0.5   | 22-11-2026     | Adopt qualified bounded native string patterns |
//                      | Charles Korthout | 0.6   | 22-11-2026     | Probe simple scalar roots in isolated source copies only |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private static NativeType CompileRestriction(XElement definition, string? name, string namespaceUri,
        NativeNumericLimits limits, int? maxStringCharacters, NativePatternCompilation patternCompilation)
    {
        Attributes(definition, name is null ? [] : ["name"]);
        var restriction = Single(definition, "restriction");
        Attributes(restriction, "base");
        var baseName = ResolveTypeName(restriction, "base");
        if (baseName == Xs + "decimal")
            return new NativeType(name, false, namespaceUri, NativeDecimalFacets.Compile(restriction, limits), baseName: baseName);
        if (maxStringCharacters is not null && baseName == Xs + "string")
            return new NativeType(name, false, namespaceUri, isString: true,
                stringFacets: NativeStringFacets.Compile(restriction, maxStringCharacters.Value, patternCompilation), baseName: baseName);
        if (maxStringCharacters is not null && baseName == Xs + "integer")
            return new NativeType(name, false, namespaceUri, isInteger: true,
                integerFacets: NativeIntegerFacets.Compile(restriction, limits), baseName: baseName);
        throw new NativeProfileException(NativeOutcome.Unsupported, "Only admitted direct scalar restrictions are supported.", NativeDecimalUnsupportedReason.ValueType);
    }

    private static Dictionary<XElement, NativeType> CompileAnonymousRestrictions(XElement schema,
        NativeNumericLimits limits, int? maxStringCharacters, CancellationToken cancellationToken, NativePatternCompilation patternCompilation)
    {
        var result = new Dictionary<XElement, NativeType>();
        if (maxStringCharacters is null) return result;
        // Count source sites, including unused named definitions; shared references do not consume the budget twice.
        var definitions = schema.Descendants().Where(e => (e.Name == Xs + "simpleType" || e.Name == Xs + "complexType") &&
            e.Parent?.Name == Xs + "element" && (e.Parent.Parent?.Name == Xs + "sequence" || e.Name == Xs + "simpleType" && e.Parent.Parent?.Name == Xs + "schema")).ToArray();
        Require(definitions.Length <= 16, "Local anonymous type ceiling exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        foreach (var definition in definitions.Where(e => e.Name == Xs + "simpleType"))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Require(definition.Parent!.Attribute("type") is null, "Inline and named types cannot coexist.", NativeDecimalUnsupportedReason.DeclarationShape);
            result.Add(definition, CompileRestriction(definition, null, "", limits, maxStringCharacters, patternCompilation));
        }
        return result;
    }
    private static NativeDecimalSchema? TryCompileSimpleRoot(XElement declaration, string targetNamespace,
        NativeType decimalType, NativeType? stringType, NativeType? booleanType, NativeType? integerType,
        Dictionary<XName, NativeType> simple, Dictionary<XElement, NativeType> anonymousSimple, NativeNumericLimits limits)
    {
        if (stringType is null) return null; // Preserve the legacy facade grammar.
        NativeType? type = null;
        if (declaration.Attribute("type") is not null)
        {
            Require(Elements(declaration).Count == 0, "Inline and named types cannot coexist.", NativeDecimalUnsupportedReason.DeclarationShape);
            var name = ResolveTypeName(declaration);
            type = name == Xs + "decimal" ? decimalType : name == Xs + "string" ? stringType :
                name == Xs + "boolean" ? booleanType : name == Xs + "integer" ? integerType : simple.GetValueOrDefault(name);
        }
        else
        {
            var members = Elements(declaration);
            if (members.Count == 1 && members[0].Name == Xs + "simpleType") type = anonymousSimple[members[0]];
        }
        return type is null ? null : new NativeDecimalSchema(new NativeDeclaration(Name(declaration), type, 1, 1, targetNamespace), [], [], limits);
    }

}
