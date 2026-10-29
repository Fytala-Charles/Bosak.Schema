// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : Assertion component definitions for the REQ-009 compiled schema model.
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005); S1 records the components,
//                        S2/S3 add their evaluation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 27-10-2026     | Added NamespaceBindings/XPathDefaultNamespace (REQ-009 S2 assertion evaluation)          |
//                      | Charles Korthout | 0.3   | 27-10-2026     | TypeAlternativeDefinition gains owner context, static context, resolved type (S4a)     |
//                      | Charles Korthout | 0.4   | 27-10-2026     | AssertionDefinition gains owning type/element for S4b applicability filtering            |

//                      | Charles Korthout | 0.5   | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;

namespace Bosak.Schema;

/// <summary>
/// Distinguishes the two XSD 1.1 assertion contexts, which have distinct W3C-defined XPath
/// environments and must not be evaluated interchangeably.
/// </summary>
public enum AssertionContext
{
    /// <summary><c>xs:assert</c> on a complex type: the XPath context item is the element being validated.</summary>
    ComplexTypeAssert,

    /// <summary><c>xs:assertion</c> facet on a simple type: the XPath context item is the typed value being validated.</summary>
    SimpleTypeAssertion,
}

/// <summary>
/// A compiled XSD 1.1 assertion component (<c>xs:assert</c> or <c>xs:assertion</c>).
/// </summary>
/// <param name="TestExpression">The raw <c>test</c> attribute expression. Compilation into a constrained
/// XPath form and evaluation land in REQ-009 phases S2/S3.</param>
/// <param name="Context">The assertion context — <see cref="AssertionContext.ComplexTypeAssert"/> for
/// <c>xs:assert</c>, <see cref="AssertionContext.SimpleTypeAssertion"/> for <c>xs:assertion</c>.</param>
/// <param name="NodePath">Path of the originating node inside the schema document.</param>
/// <param name="NamespaceBindings">In-scope namespace prefix → URI bindings captured at the assertion's
/// location in the schema document; used as the static namespace context when the test expression is
/// compiled (REQ-009 S2).</param>
/// <param name="XPathDefaultNamespace">Resolved <c>xpathDefaultNamespace</c> (element or nearest ancestor
/// <c>xs:schema</c>): the default element namespace for the test expression, or null when unspecified.</param>
/// <param name="OwningTypeName">The resolved expanded name of the owning named <c>xs:complexType</c>, or null
/// when the assertion sits on an anonymous inline complex type (REQ-009 S4b applicability filtering).</param>
/// <param name="OwningElementName">The resolved expanded name of the owning <c>xs:element</c> declaration,
/// or null when unavailable (REQ-009 S4b: anonymous-owner assertions match candidates by this name).</param>
public sealed record AssertionDefinition(
    string TestExpression,
    AssertionContext Context,
    string? NodePath,
    IReadOnlyDictionary<string, string>? NamespaceBindings = null,
    string? XPathDefaultNamespace = null,
    XName? OwningTypeName = null,
    XName? OwningElementName = null);

/// <summary>
/// A compiled XSD 1.1 conditional type assignment component (<c>xs:alternative</c>).
/// </summary>
/// <param name="TestExpression">The raw <c>test</c> attribute expression, or null for the default alternative.</param>
/// <param name="TypeName">The <c>type</c> attribute value (a QName in its lexical form), or null when absent.</param>
/// <param name="ResolvedTypeName">The <c>type</c> attribute resolved to an expanded name (unprefixed type names
/// take the schema's target namespace, per the XSD rule), or null when absent or unresolvable.</param>
/// <param name="OwnerElementName">The raw lexical <c>name</c> of the owning <c>xs:element</c> declaration.</param>
/// <param name="OwnerElementResolvedName">The owning element declaration resolved to an expanded name (global
/// declarations and, per <c>elementFormDefault</c>, qualified locals take the target namespace), or null when
/// the owner name could not be resolved.</param>
/// <param name="NamespaceBindings">In-scope namespace prefix → URI bindings captured at the alternative's
/// location in the schema document; the static namespace context for the test expression (REQ-009 S4a).</param>
/// <param name="XPathDefaultNamespace">Resolved <c>xpathDefaultNamespace</c> (element or nearest ancestor
/// <c>xs:schema</c>): the default element namespace for the test expression, or null when unspecified.</param>
/// <param name="NodePath">Path of the originating node inside the schema document.</param>
public sealed record TypeAlternativeDefinition(
    string? TestExpression,
    string? TypeName,
    XName? ResolvedTypeName,
    string? OwnerElementName,
    XName? OwnerElementResolvedName,
    IReadOnlyDictionary<string, string>? NamespaceBindings,
    string? XPathDefaultNamespace,
    string? NodePath);
