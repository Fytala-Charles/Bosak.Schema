// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Parses bounded IEEE floating values and preserves primitive family identity.
// SPECIAL NOTES        : Part of bounded native datatype validation.
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
using System.Globalization;
using System.Text.RegularExpressions;
using Bosak.XPath.Core.Xdm;
namespace Bosak.Schema;

internal enum NativeFloatingKind { Float, Double }
internal readonly record struct NativeFloatingValue(NativeFloatingKind Kind,double Number)
{
    private static readonly Regex Lexical=new(@"\A[+-]?(?:[0-9]+(?:\.[0-9]*)?|\.[0-9]+)(?:[eE][+-]?[0-9]+)?\z",RegexOptions.CultureInvariant|RegexOptions.NonBacktracking);
    internal static NativeFloatingValue Parse(string text,NativeFloatingKind kind,int maxCharacters)
    {
        NativeDecimalSchema.Require(text.Length<=256&&text.Length<=maxCharacters,"Floating lexical capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        var lexical=NativeCalendarParser.Collapse(text);double number;
        if(lexical is "INF" or "+INF")number=double.PositiveInfinity;
        else if(lexical=="-INF")number=double.NegativeInfinity;
        else if(lexical=="NaN")number=double.NaN;
        else
        {
            if(!Lexical.IsMatch(lexical))throw new NativeProfileException(NativeOutcome.Invalid,"Invalid XSD floating lexical form.");
            const NumberStyles styles=NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint|NumberStyles.AllowExponent;
            if(kind==NativeFloatingKind.Float)
            {
                if(!float.TryParse(lexical,styles,CultureInfo.InvariantCulture,out var parsed))throw new NativeProfileException(NativeOutcome.Invalid,"Invalid floating conversion.");
                number=parsed;
            }
            else if(!double.TryParse(lexical,styles,CultureInfo.InvariantCulture,out number))throw new NativeProfileException(NativeOutcome.Invalid,"Invalid floating conversion.");
            if(number==0&&lexical.StartsWith('-'))number=BitConverter.Int64BitsToDouble(long.MinValue);
        }
        return new(kind,number);
    }
    internal bool SameSchemaValue(NativeFloatingValue other)=>Kind==other.Kind&&(Number==other.Number||double.IsNaN(Number)&&double.IsNaN(other.Number));
    internal XdmValue ToXdm()=>Kind==NativeFloatingKind.Float?XdmValue.FromFloat((float)Number):XdmValue.FromDouble(Number);
}
