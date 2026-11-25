// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Tests bounded binary and namespace-sensitive QName parsing.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Xml.Linq;
using Xunit;
namespace Bosak.Schema.Tests;

/// <summary>Checks binary encodings and QName namespace contexts before native integration.</summary>
public sealed class NativeInterchangeParserTests
{
    /// <summary>Checks canonical encoding and immutable decoded payloads.</summary>
    [Theory]
    [InlineData(false, "", "")]
    [InlineData(false, " 00afFE\t", "00AFFE")]
    [InlineData(true, "", "")]
    [InlineData(true, "Y Q = =", "YQ==")]
    [InlineData(true, "YWI=", "YWI=")]
    /// <param name="base64">Encoding selector.</param>
    /// <param name="lexical">Supplied lexical form.</param>
    /// <param name="expected">Expected canonical encoding.</param>
    [InlineData(true, "YWJj", "YWJj")]
    public void BinaryCanonical(bool base64, string lexical, string expected)
    {
        var result = NativeInterchangeParser.Parse(lexical, base64 ? NativeInterchangeKind.Base64Binary : NativeInterchangeKind.HexBinary, 256, null);
        Assert.Equal(expected, result.Canonical);
    }

    /// <summary>Rejects malformed encodings including unused pad bits.</summary>
    [Theory]
    [InlineData(false, "0")]
    [InlineData(false, "00 ff")]
    [InlineData(false, "GG")]
    [InlineData(false, "+0")]
    [InlineData(true, "AB==")]
    [InlineData(true, "AAB=")]
    [InlineData(true, "YQ")]
    [InlineData(true, "YQ===")]
    /// <param name="base64">Encoding selector.</param>
    /// <param name="lexical">Malformed lexical form.</param>
    [InlineData(true, "YQ==\u00a0")]
    public void InvalidBinary(bool base64, string lexical) => Assert.Equal(NativeOutcome.Invalid,
        Assert.Throws<NativeProfileException>(() => NativeInterchangeParser.Parse(lexical,
            base64 ? NativeInterchangeKind.Base64Binary : NativeInterchangeKind.HexBinary, 256, null)).Outcome);

    /// <summary>Checks per-owner bindings and prefix-independent value equality.</summary>
    [Fact]
    public void QNameContextAndEquality()
    {
        var a = XElement.Parse("<a xmlns='urn:default' xmlns:p='urn:x'><b xmlns:p='urn:y'/></a>");
        var first = NativeInterchangeParser.Parse(" p:item ", NativeInterchangeKind.QName, 256, a);
        var other = NativeInterchangeParser.Parse("q:item", NativeInterchangeKind.QName, 256, XElement.Parse("<a xmlns:q='urn:x'/>"));
        Assert.True(first.ValueEquals(other));
        Assert.False(first.ValueEquals(NativeInterchangeParser.Parse("p:item", NativeInterchangeKind.QName, 256, a.Elements().Single())));
        Assert.Equal("urn:default", NativeInterchangeParser.Parse("item", NativeInterchangeKind.QName, 256, a).Name.NamespaceUri);
        Assert.Equal(XNamespace.Xml.NamespaceName, NativeInterchangeParser.Parse("xml:lang", NativeInterchangeKind.QName, 256, a).Name.NamespaceUri);
    }

    /// <summary>Checks QName lexical and binding refusals.</summary>
    [Theory]
    [InlineData("")]
    [InlineData("p:x")]
    [InlineData("a:b:c")]
    [InlineData(":x")]
    [InlineData("x:")]
    [InlineData("1x")]
    [InlineData("xmlns:x")]
    /// <param name="lexical">Malformed or unbound QName.</param>
    [InlineData("a b")]
    public void InvalidQName(string lexical) => Assert.Equal(NativeOutcome.Invalid,
        Assert.Throws<NativeProfileException>(() => NativeInterchangeParser.Parse(lexical, NativeInterchangeKind.QName, 256, new XElement("a"))).Outcome);

    /// <summary>Checks that capacity refusal does not become invalid-value fallback.</summary>
    [Fact]
    public void CapacityAndPrimitiveEquality()
    {
        Assert.Equal(NativeOutcome.Unsupported, Assert.Throws<NativeProfileException>(() =>
            NativeInterchangeParser.Parse(new string('0', 258), NativeInterchangeKind.HexBinary, 512, null)).Outcome);
        var hex = NativeInterchangeParser.Parse("61", NativeInterchangeKind.HexBinary, 256, null);
        var base64 = NativeInterchangeParser.Parse("YQ==", NativeInterchangeKind.Base64Binary, 256, null);
        Assert.False(hex.ValueEquals(base64));
        Assert.Equal(NativeOutcome.Unsupported, Assert.Throws<NativeProfileException>(() =>
            NativeInterchangeParser.Parse("x", NativeInterchangeKind.QName, 2, XElement.Parse("<a xmlns='urn:long'/>"))).Outcome);
    }
}
