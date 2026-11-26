// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Maps bounded Gregorian fragments and exact REC timezone-aware timelines.
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
using System.Numerics;
using System.Text.RegularExpressions;
using Bosak.XPath.Core.Xdm;
namespace Bosak.Schema;
internal enum NativeGregorianKind { GYear, GYearMonth, GMonth, GMonthDay, GDay }
internal readonly record struct NativeGregorianValue(NativeGregorianKind Kind,long? Year,int? Month,int? Day,int? Timezone,string Lexical)
{
    private const string YearPattern=@"(?<y>-?(?:[1-9][0-9]{3,}|0[0-9]{3}))";
    private const string MonthPattern=@"(?<m>0[1-9]|1[0-2])";
    private const string DayPattern=@"(?<d>0[1-9]|[12][0-9]|3[01])";
    private const string ZonePattern=@"(?<z>Z|[+-](?:(?:0[0-9]|1[0-3]):[0-5][0-9]|14:00))?\z";
    private static readonly Dictionary<NativeGregorianKind,Regex> Syntax=Enum.GetValues<NativeGregorianKind>().ToDictionary(k=>k,k=>new Regex(@"\A"+(k switch { NativeGregorianKind.GYear=>YearPattern,NativeGregorianKind.GYearMonth=>YearPattern+"-"+MonthPattern,NativeGregorianKind.GMonth=>"--"+MonthPattern,NativeGregorianKind.GMonthDay=>"--"+MonthPattern+"-"+DayPattern,_=>"---"+DayPattern })+ZonePattern,RegexOptions.NonBacktracking|RegexOptions.CultureInvariant));
    internal string TypeName=>TypeNameFor(Kind);
    internal static string TypeNameFor(NativeGregorianKind kind)=>"g"+kind.ToString()[1..];
    internal static NativeGregorianKind KindFor(string name)=>name switch { "gYear"=>NativeGregorianKind.GYear,"gYearMonth"=>NativeGregorianKind.GYearMonth,"gMonth"=>NativeGregorianKind.GMonth,"gMonthDay"=>NativeGregorianKind.GMonthDay,"gDay"=>NativeGregorianKind.GDay,_=>throw new InvalidOperationException("Unknown Gregorian family.") };
    internal static NativeGregorianValue Parse(string text,NativeGregorianKind kind,int maxCharacters)
    {
        NativeDecimalSchema.Require(text.Length<=256&&text.Length<=maxCharacters,"Gregorian lexical capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        var lexical=NativeCalendarParser.Collapse(text);var match=Syntax[kind].Match(lexical);if(!match.Success)Invalid("Invalid Gregorian lexical form.");
        long? year=null;int? month=null,day=null,zone=null;
        if(match.Groups["y"].Success){var y=BigInteger.Parse(match.Groups["y"].Value,CultureInfo.InvariantCulture);NativeDecimalSchema.Require(y>=-new BigInteger(long.MaxValue)&&y<=long.MaxValue,"Gregorian year capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);year=(long)y;}
        if(match.Groups["m"].Success)month=int.Parse(match.Groups["m"].Value,CultureInfo.InvariantCulture);
        if(match.Groups["d"].Success)day=int.Parse(match.Groups["d"].Value,CultureInfo.InvariantCulture);
        if(month is {} m&&day is {} d&&d>DaysInMonth(year??1972,m))Invalid("Invalid Gregorian month/day combination.");
        var z=match.Groups["z"].Value;if(z.Length!=0)zone=z=="Z"?0:(int.Parse(z.AsSpan(1,2),CultureInfo.InvariantCulture)*60+int.Parse(z.AsSpan(4,2),CultureInfo.InvariantCulture))*(z[0]=='-'?-1:1);
        return new(kind,year,month,day,zone,lexical);
    }
    internal bool SameValue(NativeGregorianValue other)=>Kind==other.Kind&&Timezone.HasValue==other.Timezone.HasValue&&Point(Timezone??0)==other.Point(other.Timezone??0);
    internal bool IdenticalValue(NativeGregorianValue other)=>Kind==other.Kind&&Year==other.Year&&Month==other.Month&&Day==other.Day&&Timezone==other.Timezone;
    internal int? Compare(NativeGregorianValue other)
    {
        NativeDecimalSchema.Require(Kind==other.Kind,"Gregorian primitive spaces cannot be ordered together.",NativeDecimalUnsupportedReason.ValueType);
        if(Timezone.HasValue==other.Timezone.HasValue)return Point(Timezone??0).CompareTo(other.Point(other.Timezone??0));
        var leftMin=Point(Timezone??840);var leftMax=Point(Timezone??-840);var rightMin=other.Point(other.Timezone??840);var rightMax=other.Point(other.Timezone??-840);
        return leftMax<rightMin?-1:leftMin>rightMax?1:null;
    }
    internal BigInteger Point(int zone)
    {
        // REC D.2.1 uses1972-12-31, with the month's last permitted day when month is present and day absent.
        var y=(BigInteger)(Year??1972);var m=Month??12;var d=Day??(Month is not null?DaysInMonth(Year??1972,m):31);var prior=y-1;
        var days=365*prior+Floor(prior,4)-Floor(prior,100)+Floor(prior,400)+new[]{0,31,59,90,120,151,181,212,243,273,304,334}[m-1]+d-1;
        if(m>2&&Leap(y))days++;return days*1440-zone;
    }
    private static bool Leap(BigInteger year)=>year%400==0||year%4==0&&year%100!=0;
    private static int DaysInMonth(long year,int month)=>month==2?(Leap(year)?29:28):month is 4 or 6 or 9 or 11?30:31;
    private static BigInteger Floor(BigInteger value,int divisor){var q=BigInteger.DivRem(value,divisor,out var r);return r.Sign<0?q-1:q;}
    internal XdmValue ToXdm()=>XdmValue.FromString(Lexical,TypeName);
    private static void Invalid(string text)=>throw new NativeProfileException(NativeOutcome.Invalid,text);
}
