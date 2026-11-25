// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Maps bounded calendar lexical forms to exact signed Core values.
// SPECIAL NOTES        : Part of bounded native exact calendar validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 25-11-2026     | Map signed-zero XSD years independently of the narrower Core helper |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Bosak.XPath.Core.Xdm;
namespace Bosak.Schema;
internal static class NativeCalendarParser
{
    private static readonly Regex Token = new(@"\A(?:(?<y>-?[0-9]{4,})-[0-9]{2}-[0-9]{2}T)?[0-9]{2}:[0-9]{2}:(?<s>[0-9]{2}(?:\.[0-9]+)?)(?:Z|[+-][0-9]{2}:[0-9]{2})?\z", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);
    internal static string Collapse(string text) => string.Join(" ", text.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries));
    internal static XdmExactCalendarValue Parse(string text, XdmExactCalendarKind kind, NativeNumericLimits limits, int maxCharacters)
    {
        NativeDecimalSchema.Require(text.Length <= 256 && text.Length <= maxCharacters, "Calendar lexical capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var lexical = Collapse(text);
        var match = Token.Match(lexical);
        if (!match.Success || match.Groups["y"].Success == (kind == XdmExactCalendarKind.Time)) Invalid("Invalid calendar lexical form.");
        if (match.Groups["y"].Success)
        {
            var year = BigInteger.Parse(match.Groups["y"].Value, CultureInfo.InvariantCulture);
            NativeDecimalSchema.Require(year >= -new BigInteger(long.MaxValue) && year <= long.MaxValue, "Calendar year capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        }
        var seconds = match.Groups["s"].Value;
        var scale = seconds.Length > 2 ? seconds.Length - 3 : 0;
        NativeDecimalSchema.Require(2 + scale <= limits.MaxDigits && scale <= limits.MaxScale, "Calendar seconds capacity exceeded before normalization.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        // XSD yearFragValue maps -0000 to integer zero. Core 1.86.0's
        // narrower lexical helper refuses that spelling; keep original text
        // for schema patterns/export and normalize only the component input.
        var componentLexical = match.Groups["y"].Value == "-0000" ? lexical[1..] : lexical;
        try { return XdmExactCalendarValue.Parse(componentLexical, kind, limits.MaxDigits, limits.MaxScale); }
        catch (Exception error) when (error is FormatException or ArgumentOutOfRangeException) { throw new NativeProfileException(NativeOutcome.Invalid, "Invalid calendar lexical or component value."); }
        catch (NotSupportedException) { throw new NativeProfileException(NativeOutcome.Unsupported, "Calendar representation capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity); }
    }
    private static void Invalid(string message) => throw new NativeProfileException(NativeOutcome.Invalid, message);
}
