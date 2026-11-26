// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Validates bounded schema annotations before native declaration scans.
// SPECIAL NOTES        : Internal bounded annotation grammar; no public annotation PSVI projection.
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
using System.Xml;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private static void PrepareSchemaAnnotations(XElement schema, CancellationToken cancellationToken)
    {
        var parents = new HashSet<string>(StringComparer.Ordinal)
        {
            "schema", "element", "attribute", "simpleType", "complexType", "simpleContent", "complexContent",
            "restriction", "extension", "list", "union", "sequence", "choice", "all", "group", "attributeGroup",
            "include", "import", "assert", "length", "minLength", "maxLength", "pattern", "enumeration", "whiteSpace",
            "minInclusive", "maxInclusive", "minExclusive", "maxExclusive", "totalDigits", "fractionDigits", "explicitTimezone"
        };
        var count = 0; var identifiers = new HashSet<string>(StringComparer.Ordinal);
        void Visit(XElement owner)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var children = owner.Elements().ToArray();
            var annotations = children.Where(e => e.Name == Xs + "annotation").ToArray();
            if (annotations.Length != 0)
            {
                Require(owner.Name.Namespace == Xs && parents.Contains(owner.Name.LocalName), "Annotation parent is outside the profile.", NativeDecimalUnsupportedReason.DeclarationShape);
                Check(owner.Name == Xs + "schema" || annotations.Length == 1 && ReferenceEquals(children[0], annotations[0]), "Component annotation must occur once before other members.");
            }
            foreach (var child in children)
            {
                if (child.Name != Xs + "annotation") { Visit(child); continue; }
                Require(++count <= 16, "Annotation wrapper capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
                Attributes(child, "id");
                if (child.Attribute("id") is { } identifier)
                {
                    var text = Collapse(identifier.Value);
                    try { XmlConvert.VerifyNCName(text); }
                    catch (XmlException) { throw new NativeProfileException(NativeOutcome.Invalid, "Invalid annotation ID."); }
                    Check(identifiers.Add(text), "Duplicate annotation ID.");
                }
                Check(child.Nodes().All(n => n is XElement or XComment or XProcessingInstruction || n is XText t && IsWhitespace(t.Value)), "Annotation wrapper cannot contain character content.");
                foreach (var payload in child.Elements())
                {
                    Check(payload.Name == Xs + "appinfo" || payload.Name == Xs + "documentation", "Unexpected annotation payload wrapper.");
                    Attributes(payload, "source");
                    // No traversal into payloads: even XSD-named content remains opaque application data.
                }
                child.Remove();
            }
        }
        Visit(schema);
    }
}
