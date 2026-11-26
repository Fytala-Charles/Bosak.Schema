// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Parses bounded binary and namespace-sensitive QName values.
// SPECIAL NOTES        : Part of bounded native datatype validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 26-11-2026     | Support bounded text datatypes and inherited normalization facets |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
using System.Text;
using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

internal enum NativeInterchangeKind { HexBinary, Base64Binary, QName }

internal sealed record NativeInterchangeValue(NativeInterchangeKind Kind, string Lexical,
    ImmutableArray<byte> Octets, XsQName Name)
{
    internal bool ValueEquals(NativeInterchangeValue other) => Kind == other.Kind &&
        (Kind == NativeInterchangeKind.QName ? Name.Equals(other.Name) : Octets.AsSpan().SequenceEqual(other.Octets.AsSpan()));

    internal string Canonical => Kind switch
    {
        NativeInterchangeKind.HexBinary => Convert.ToHexString(Octets.AsSpan()),
        NativeInterchangeKind.Base64Binary => Convert.ToBase64String(Octets.AsSpan()),
        _ => Name.ToString()
    };
}

internal static class NativeInterchangeParser
{
    internal static NativeInterchangeValue Parse(string text, NativeInterchangeKind kind, int maxCharacters, XElement? scope)
    {
        NativeDecimalSchema.Require(text.Length <= 256 && text.Length <= maxCharacters,
            "Interchange lexical capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var lexical = NativeCalendarParser.Collapse(text);
        if (kind == NativeInterchangeKind.QName)
        {
            var parts = lexical.Split(':');
            if (parts.Length > 2 || parts.Any(string.IsNullOrEmpty)) Invalid();
            foreach (var part in parts) VerifyName(part);
            var prefix = parts.Length == 2 ? parts[0] : "";
            if (prefix == "xmlns") Invalid();
            var ns = prefix == "xml" ? XNamespace.Xml : prefix.Length == 0 ? scope?.GetDefaultNamespace() ?? XNamespace.None : scope?.GetNamespaceOfPrefix(prefix);
            if (ns is null) Invalid();
            NativeDecimalSchema.Require(ns!.NamespaceName.Length <= maxCharacters,
                "QName namespace capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
            return new(kind, lexical, [], new XsQName(parts[^1], ns.NamespaceName, prefix));
        }
        byte[] bytes;
        try
        {
            if (kind == NativeInterchangeKind.HexBinary)
            {
                if (lexical.Length % 2 != 0 || lexical.Any(c => !char.IsAsciiHexDigit(c))) Invalid();
                bytes = Convert.FromHexString(lexical);
            }
            else
            {
                var compact = string.Concat(lexical.Where(c => c is not (' ' or '\t' or '\r' or '\n')));
                bytes = Convert.FromBase64String(compact);
                // Convert accepts non-zero unused pad bits; XSD does not.
                if (Convert.ToBase64String(bytes) != compact) Invalid();
            }
        }
        catch (FormatException) { throw new NativeProfileException(NativeOutcome.Invalid, "Invalid binary lexical form."); }
        NativeDecimalSchema.Require(bytes.Length <= 128, "Binary octet capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        return new(kind, lexical, ImmutableArray.CreateRange(bytes), default);
    }

    private static void Invalid() => throw new NativeProfileException(NativeOutcome.Invalid, "Invalid binary or QName lexical form.");

    // XML 1.0 Fifth Edition NameStartChar/NameChar with colon removed for NCName.
    private static void VerifyName(string text) { if (!IsXmlName(text)) Invalid(); }
    internal static bool IsXmlName(string text, bool allowColon = false, bool token = false)
    {
        if (text.Length == 0) return false;
        var first = true;
        var remaining = text.AsSpan();
        while (!remaining.IsEmpty)
        {
            if (Rune.DecodeFromUtf16(remaining, out var rune, out var consumed) != System.Buffers.OperationStatus.Done) return false;
            var c = rune.Value;
            var start = allowColon && c == ':' || c is '_' or >= 'A' and <= 'Z' or >= 'a' and <= 'z' or
                >= 0xC0 and <= 0xD6 or >= 0xD8 and <= 0xF6 or >= 0xF8 and <= 0x2FF or
                >= 0x370 and <= 0x37D or >= 0x37F and <= 0x1FFF or >= 0x200C and <= 0x200D or
                >= 0x2070 and <= 0x218F or >= 0x2C00 and <= 0x2FEF or >= 0x3001 and <= 0xD7FF or
                >= 0xF900 and <= 0xFDCF or >= 0xFDF0 and <= 0xFFFD or >= 0x10000 and <= 0xEFFFF;
            if (!start && (first && !token || c is not ('-' or '.' or >= '0' and <= '9' or 0xB7 or >= 0x300 and <= 0x36F or >= 0x203F and <= 0x2040))) return false;
            first = false;
            remaining = remaining[consumed..];
        }
        return true;
    }
}
