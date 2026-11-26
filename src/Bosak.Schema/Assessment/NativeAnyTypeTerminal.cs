// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Preserves canonical anyType identity and original text-only mixed terminal values.
// SPECIAL NOTES        : Canonical anyType and anySimpleType components for bounded lax assessment.
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
//                      | Charles Korthout | 0.21  | 26-11-2026     | Preserve absent lax declarations and original node identity.                              |
// ===========================================================================================================================================================
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed class NativeAnyTypeCompilation
{
    internal NativeType? Type { get; private set; }
    internal NativeType Use() => Type ??= new NativeType("anyType", false, "http://www.w3.org/2001/XMLSchema", isMixed: true, isAnyType: true);
}

internal sealed partial class NativeDecimalSchema
{
    private static bool IsAnyTypeElement(XElement declaration)
    {
        if (declaration.Attribute("ref") is not null || Elements(declaration).Count != 0) return false;
        if (declaration.Attribute("type") is null) return true;
        var lexical = Collapse(declaration.Attribute("type")!.Value);
        return lexical.Split(':')[^1] == "anyType" && ResolveTypeName(declaration) == Xs + "anyType";
    }

    private static NativeDecimalSchema CompileAnyTypeTerminal(XElement source, string name, int min, int max, string ns, NativeNumericLimits limits)
    {
        var root = source.AncestorsAndSelf().Last();
        var context = root.Annotation<NativeAnyTypeCompilation>();
        if (context is null) { context = new NativeAnyTypeCompilation(); root.AddAnnotation(context); }
        var declaration = ElementDeclaration(source, name, context.Use(), min, max, ns);
        return new NativeDecimalSchema(declaration, [], [], limits) { NestedProfile = true };
    }

    private void CompleteAnyTypeMetadata(XElement schema)
    {
        if (schema.Annotation<NativeAnyTypeCompilation>()?.Type is not { } type) return;
        Require(!NamedTypes.Any(t => t.Name == "anyType" && t.NamespaceUri == Xs.NamespaceName),
            "Reserved anyType component redeclaration is outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
        Require(!NamedTypes.Any(t => t.Name == "anySimpleType" && t.NamespaceUri == Xs.NamespaceName), "Reserved anySimpleType component redeclaration.", NativeDecimalUnsupportedReason.DeclarationShape);
        LaxAttributeType = new NativeType("anySimpleType", false, Xs.NamespaceName, isString: true);
        NamedTypes = NamedTypes.Add(type).Add(LaxAttributeType);
        UntypedAtomicType ??= new NativeType("untypedAtomic", false, Xs.NamespaceName, isString: true);
    }
}
