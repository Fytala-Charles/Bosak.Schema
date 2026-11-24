// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Carries a detached validated attribute name, owner path and scalar value.
// SPECIAL NOTES        : Bounded opt-in attribute result; no transferable schema authority.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 20-11-2026     | Carry detached immutable owner paths for root and descendant attribute results           |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;

namespace Bosak.Schema;

/// <summary>Contains a detached attribute name, owner path and typed scalar without schema authority.</summary>
public sealed class NativeAttributeValue
{
    internal NativeAttributeValue(string namespaceUri, string localName, NativeScalarValue value, ImmutableArray<int> ownerElementPath)
    {
        NamespaceUri = namespaceUri; LocalName = localName; Value = value;
        OwnerElementPath = ownerElementPath.IsDefault ? [] : ownerElementPath;
    }
    /// <summary>Gets a nondefault immutable path of zero-based element-child indexes from the assessed root to the owner.</summary>
    /// <remarks>Empty for the root. This detached document-relative locator carries no schema authority or cross-document identity.</remarks>
    public ImmutableArray<int> OwnerElementPath { get; }
    /// <summary>Gets the expanded-name namespace URI, including empty for an unqualified attribute.</summary>
    public string NamespaceUri { get; }
    /// <summary>Gets the expanded-name local name without a prefix.</summary>
    public string LocalName { get; }
    /// <summary>Gets the detached typed scalar, usable after document and session disposal.</summary>
    public NativeScalarValue Value { get; }
}
