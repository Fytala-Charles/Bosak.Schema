// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Carries a detached validated root attribute expanded name and scalar value.
// SPECIAL NOTES        : Bounded opt-in root attribute result; no transferable schema authority.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

namespace Bosak.Schema;

/// <summary>Contains a detached immutable root attribute name and typed scalar without schema authority.</summary>
public sealed class NativeAttributeValue
{
    internal NativeAttributeValue(string namespaceUri, string localName, NativeScalarValue value)
    {
        NamespaceUri = namespaceUri; LocalName = localName; Value = value;
    }
    /// <summary>Gets the expanded-name namespace URI, including empty for an unqualified attribute.</summary>
    public string NamespaceUri { get; }
    /// <summary>Gets the expanded-name local name without a prefix.</summary>
    public string LocalName { get; }
    /// <summary>Gets the detached typed scalar, usable after document and session disposal.</summary>
    public NativeScalarValue Value { get; }
}
