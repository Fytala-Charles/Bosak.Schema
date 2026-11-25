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
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks native binary and QName values through both public facades.</summary>
public sealed class NativeInterchangeConsumerTests
{
    /// <summary>Gets lexical acceptance and edge cases.</summary>
    /// <returns>Family, lexical form and expected assessment outcome.</returns>
    public static IEnumerable<object[]> Cases()
    {
        foreach (var data in new[] { false, true })
        foreach (var (family, lexical, valid) in new[] {
            ("hexBinary", "", true), ("hexBinary", "00afFE", true), ("hexBinary", " 61 ", true),
            ("hexBinary", "0", false), ("hexBinary", "00 ff", false), ("hexBinary", "GG", false),
            ("base64Binary", "", true), ("base64Binary", "Y Q = =", true), ("base64Binary", "YWI=", true),
            ("base64Binary", "YWJj", true), ("base64Binary", "AB==", false), ("base64Binary", "YQ", false),
            ("QName", "p:item", true), ("QName", "item", true), ("QName", "xml:lang", true),
            ("QName", "missing:item", false), ("QName", "a:b:c", false), ("QName", "1bad", false) })
            yield return [family, lexical, valid, data];
    }

    /// <summary>Reads original typed values and retains detached payloads after disposal.</summary>
    /// <param name="family">Schema primitive.</param><param name="lexical">Instance text.</param>
    /// <param name="valid">Expected validity.</param><param name="data">Node-data facade selector.</param>
    [Theory][MemberData(nameof(Cases))]
    public void Primitive(string family, string lexical, bool valid, bool data)
    {
        var schema = $"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:{family}'/></xs:schema>";
        var xml = $"<r xmlns:p='urn:test'>{lexical}</r>";
        var context = new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0);
        NativeScalarValue? scalar;
        if (data)
        {
            var compiled = NativeSchemaNodeDataSession.Create(schema, new NativeSchemaSessionOptions(256,256,256,65536,1,256,1,256,256));
            Assert.Equal(NativeSchemaOutcome.Valid, compiled.Outcome);
            using var session = compiled.Value!;
            var assessed = session.Assess(xml, context);
            Assert.Equal(valid ? NativeSchemaOutcome.Valid : NativeSchemaOutcome.Invalid, assessed.Outcome);
            using var document = assessed.Value;
            if (document is null) return;
            scalar = document.ReadElement([]);
        }
        else
        {
            var compiled = NativeSchemaSession.Create(schema, new NativeSchemaSessionOptions(256,256,256,65536,1,256,1,256,256));
            Assert.Equal(NativeSchemaOutcome.Valid, compiled.Outcome);
            using var session = compiled.Value!;
            var assessed = session.Assess(xml, context);
            Assert.Equal(valid ? NativeSchemaOutcome.Valid : NativeSchemaOutcome.Invalid, assessed.Outcome);
            using var document = assessed.Value;
            if (document is null) return;
            scalar = document.ReadRootScalar();
        }
        if (family == "QName")
        {
            Assert.Equal(NativeScalarKind.QName, scalar.Kind);
            Assert.Equal(lexical.Contains(':') ? lexical.Split(':')[1] : lexical, scalar.QNameValue.LocalName);
            Assert.Throws<InvalidOperationException>(() => scalar.BinaryValue);
        }
        else
        {
            Assert.Equal(family == "hexBinary" ? NativeScalarKind.HexBinary : NativeScalarKind.Base64Binary, scalar.Kind);
            Assert.NotNull(scalar.BinaryValue.ToArray());
            Assert.Throws<InvalidOperationException>(() => scalar.QNameValue);
        }
        Assert.Throws<InvalidOperationException>(() => scalar.StringValue);
    }
}
