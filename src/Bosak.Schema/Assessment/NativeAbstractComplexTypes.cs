// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Freezes abstract complex type identities and checks resolved governing types.
// SPECIAL NOTES        : Abstractness belongs to original complex types, separate from declarations and atoms.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private ImmutableHashSet<NativeType> abstractComplexTypes = ImmutableHashSet<NativeType>.Empty;

    private void CompleteAbstractComplexMetadata(XElement schema, CancellationToken token)
    {
        var types = ImmutableHashSet.CreateBuilder<NativeType>();
        var ns = Collapse((string?)schema.Attribute("targetNamespace") ?? "");
        foreach (var definition in schema.Elements(Xs + "complexType"))
        {
            token.ThrowIfCancellationRequested();
            if (definition.Attribute("abstract") is not { } flag) continue;
            Require(flag.Value.Length <= 256, "Abstract complex-type Boolean source capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            var value = Collapse(flag.Value);
            Check(value is "true" or "false" or "1" or "0", "Invalid abstract complex-type Boolean.");
            if (value is "true" or "1") types.Add(instanceTypes[XName.Get(Name(definition), ns)]);
        }
        abstractComplexTypes = types.ToImmutable();
    }

    private NativeType ResolveInstanceType(XElement element, NativeType declared)
    {
        var actual = ResolveInstanceTypeCore(element, declared);
        Check(!abstractComplexTypes.Contains(actual), "An abstract complex type cannot govern a validated element.");
        return actual;
    }
}
