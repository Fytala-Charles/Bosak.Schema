// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Compiles bounded numeric lexical patterns without changing exact value facets.
// SPECIAL NOTES        : Internal finite numeric profile; lexical spelling remains separate from value equality.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed class NativeNumericPatterns(NativeNumericPatterns? parent, ImmutableArray<NativePattern> alternatives)
{
    internal bool Accepts(string lexical, NativePatternBudget budget) =>
        (parent is null || parent.Accepts(lexical, budget)) && (alternatives.IsEmpty || alternatives.Any(p => p.Match(lexical, budget)));

    internal static NativeNumericPatterns Compile(XElement restriction, NativeNumericPatterns? parent, NativePatternCompilation compilation)
    {
        var patterns = ImmutableArray.CreateBuilder<NativePattern>();
        foreach (var facet in NativeDecimalSchema.Elements(restriction).Where(f => f.Name == XName.Get("pattern", "http://www.w3.org/2001/XMLSchema")))
        {
            compilation.Check(); NativePattern.Capacity(patterns.Count < 8);
            NativeDecimalSchema.Attributes(facet, "value");
            NativeDecimalSchema.Require(NativeDecimalSchema.Elements(facet).Count == 0, "Numeric pattern children excluded.", NativeDecimalUnsupportedReason.DeclarationShape);
            if (facet.Attribute("value") is not { } value) throw new NativeProfileException(NativeOutcome.Invalid, "Numeric pattern value required.");
            patterns.Add(NativePattern.Compile(value.Value, compilation, numericAlphabet: true));
        }
        return new(parent, patterns.ToImmutable());
    }
}

internal sealed partial class NativeDecimalSchema
{
    private static NativeType CompileNumericPatternRestriction(XElement restriction, string? name, string ns, XName baseName,
        NativeType baseType, NativeNumericLimits limits, NativePatternCompilation compilation)
    {
        Require(!baseType.IsInteger || baseName.Namespace == Xs, "Named integer chains remain outside this profile.", NativeDecimalUnsupportedReason.ValueType);
        var values = Elements(restriction).Where(f => f.Name != Xs + "pattern").ToArray();
        Require(baseType.NumericPatterns is null || !values.Any(f => f.Name.LocalName is "enumeration" or "minInclusive" or "minExclusive" or "maxInclusive" or "maxExclusive"),
            "Value facets over patterned numeric bases await separate adoption.", NativeDecimalUnsupportedReason.ValueType);
        var patterns = NativeNumericPatterns.Compile(restriction, baseType.NumericPatterns, compilation);
        var valueRestriction = new XElement(restriction.Name, restriction.Attributes(), values);
        if (baseType.IsInteger)
            return new NativeType(name, false, ns, isInteger: true, integerFacets: baseName == Xs + "integer"
                ? NativeIntegerFacets.Compile(valueRestriction, limits) : NativeIntegerFacets.CompileDerived(valueRestriction, baseType.IntegerFacets, limits),
                baseName: baseName, derivationBase: baseName == Xs + "integer" ? null : baseType, numericPatterns: patterns);
        return new NativeType(name, false, ns, facets: baseName == Xs + "decimal"
            ? NativeDecimalFacets.Compile(valueRestriction, limits, allowEnumeration: true)
            : NativeDecimalFacets.CompileDerived(valueRestriction, baseType.Facets, limits, nativeDerivation: true),
            baseName: baseName, derivationBase: baseName == Xs + "decimal" ? null : baseType, numericPatterns: patterns);
    }
}
