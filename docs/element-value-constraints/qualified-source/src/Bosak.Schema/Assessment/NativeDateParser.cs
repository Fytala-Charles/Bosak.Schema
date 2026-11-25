// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 24 November 2026
// PURPOSE              : Validates bounded XSD1.1 date lexical and calendar mapping.
// SPECIAL NOTES        : Part of bounded native date validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 24-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Adopt qualified bounded native date transport |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Bosak.XPath.Core.Xdm;
namespace Bosak.Schema;
internal static class NativeDateParser
{
    private static readonly Regex Lexical=new(@"\A(?<y>-?(?:[1-9][0-9]{3,}|0[0-9]{3}))-(?<m>0[1-9]|1[0-2])-(?<d>0[1-9]|[12][0-9]|3[01])(?<z>Z|[+-](?:(?:0[0-9]|1[0-3]):[0-5][0-9]|14:00))?\z",RegexOptions.CultureInvariant,TimeSpan.FromSeconds(1));
    internal static XPathDateTime Parse(string text)
    {
        var lexical=string.Join(" ",text.Split(new[]{' ','\t','\r','\n'},StringSplitOptions.RemoveEmptyEntries));
        NativeDecimalSchema.Require(lexical.Length<=256,"Date lexical capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        var m=Lexical.Match(lexical);
        if(!m.Success)throw new NativeProfileException(NativeOutcome.Invalid,"Invalid date lexical form.");
        var year=BigInteger.Parse(m.Groups["y"].Value,CultureInfo.InvariantCulture);
        NativeDecimalSchema.Require(year>=-new BigInteger(long.MaxValue) && year<=long.MaxValue,"Date year capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        int month=int.Parse(m.Groups["m"].Value,CultureInfo.InvariantCulture),day=int.Parse(m.Groups["d"].Value,CultureInfo.InvariantCulture);
        bool leap=year%400==0 || year%4==0 && year%100!=0;
        int days=month==2?(leap?29:28):month is 4 or 6 or 9 or 11?30:31;
        if(day>days)throw new NativeProfileException(NativeOutcome.Invalid,"Invalid calendar day.");
        var z=m.Groups["z"].Value;int offset=0;
        if(z.Length>1)offset=(int.Parse(z.AsSpan(1,2),CultureInfo.InvariantCulture)*60+int.Parse(z.AsSpan(4,2),CultureInfo.InvariantCulture))*(z[0]=='-'?-1:1);
        return new((long)year,month,day,0,0,0,0,offset,z.Length!=0);
    }
}
