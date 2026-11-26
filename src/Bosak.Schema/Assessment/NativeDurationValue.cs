// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Represents bounded exact duration months and seconds with REC partial ordering.
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
internal enum NativeDurationKind { Duration, YearMonthDuration, DayTimeDuration }
internal readonly record struct NativeDurationValue(NativeDurationKind Kind,BigInteger Months,XdmExactDecimal Seconds,string Lexical)
{
    private static readonly Regex Syntax=new(@"\A-?P(?:(?<Y>[0-9]+)Y)?(?:(?<M>[0-9]+)M)?(?:(?<D>[0-9]+)D)?(?:T(?:(?<H>[0-9]+)H)?(?:(?<N>[0-9]+)M)?(?:(?<S>(?:[0-9]+(?:\.[0-9]*)?|\.[0-9]+))S)?)?\z",RegexOptions.NonBacktracking|RegexOptions.CultureInvariant);
    internal string TypeName=>Kind switch { NativeDurationKind.YearMonthDuration=>"yearMonthDuration",NativeDurationKind.DayTimeDuration=>"dayTimeDuration",_=>"duration" };
    internal static NativeDurationValue Parse(string text,NativeDurationKind kind,NativeNumericLimits limits,int maxCharacters)
    {
        NativeDecimalSchema.Require(text.Length<=256&&text.Length<=maxCharacters,"Duration lexical capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        var lexical=NativeCalendarParser.Collapse(text);var match=Syntax.Match(lexical);
        if(!match.Success||lexical.EndsWith('T')||!new[]{"Y","M","D","H","N","S"}.Any(n=>match.Groups[n].Success))Invalid("Invalid duration lexical form.");
        if(kind==NativeDurationKind.YearMonthDuration&&(match.Groups["D"].Success||lexical.Contains('T'))||kind==NativeDurationKind.DayTimeDuration&&(match.Groups["Y"].Success||match.Groups["M"].Success))Invalid("Duration lexical form outside derived family.");
        var secondsText=match.Groups["S"].Value;
        NativeDecimalSchema.Require(!secondsText.StartsWith('.')&&!secondsText.EndsWith('.'),"Ambiguous REC decimal-point-only seconds syntax awaits resolution.",NativeDecimalUnsupportedReason.ValueType);
        NativeDecimalSchema.Require(lexical.Count(c=>c is >= '0' and <= '9')<=limits.MaxDigits,"Supplied duration digit capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        BigInteger Part(string name)=>match.Groups[name].Success?BigInteger.Parse(match.Groups[name].Value,CultureInfo.InvariantCulture):BigInteger.Zero;
        var months=Part("Y")*12+Part("M");var point=secondsText.IndexOf('.');var scale=point<0?0:secondsText.Length-point-1;
        NativeDecimalSchema.Require(scale<=limits.MaxScale,"Supplied duration fraction scale exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        var fraction=secondsText.Length==0?BigInteger.Zero:BigInteger.Parse(secondsText.Replace(".",""),CultureInfo.InvariantCulture);
        var coefficient=(Part("D")*86400+Part("H")*3600+Part("N")*60)*BigInteger.Pow(10,scale)+fraction;
        var sign=lexical.StartsWith('-')?-1:1;months*=sign;coefficient*=sign;
        NativeDecimalSchema.Require(BigInteger.Abs(months).ToString(CultureInfo.InvariantCulture).Length<=limits.MaxDigits,"Aggregate duration month capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
        try{return new(kind,months,new XdmExactDecimal(coefficient,scale,limits.MaxDigits,limits.MaxScale),lexical);}
        catch(NotSupportedException){throw new NativeProfileException(NativeOutcome.Unsupported,"Aggregate duration seconds capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);}
    }
    internal bool SameValue(NativeDurationValue other)=>Months==other.Months&&Seconds.Equals(other.Seconds);
    internal int? Compare(NativeDurationValue other)
    {
        if(SameValue(other))return 0;
        var scale=Math.Max(Seconds.Scale,other.Seconds.Scale);int? result=null;
        foreach(var (year,month) in new[]{(1696,9),(1697,2),(1903,3),(1903,7)})
        {
            var compared=Projected(year,month,scale).CompareTo(other.Projected(year,month,scale));
            if(result is not null&&result!=compared)return null;result=compared;
        }
        NativeDecimalSchema.Require(result != 0,"Distinct duration components with coincident Gregorian projections await qualification.",NativeDecimalUnsupportedReason.ValueType);
        return result;
    }
    private BigInteger Projected(int year,int month,int scale)
    {
        var total=(BigInteger)year*12+month-1+Months;var y=Floor(total,12);var m=(int)(total-y*12)+1;var prior=y-1;
        var days=365*prior+Floor(prior,4)-Floor(prior,100)+Floor(prior,400)+new[]{0,31,59,90,120,151,181,212,243,273,304,334}[m-1];
        if(m>2&&y%4==0&&(y%100!=0||y%400==0))days++;
        return days*86400*BigInteger.Pow(10,scale)+Seconds.Coefficient*BigInteger.Pow(10,scale-Seconds.Scale);
    }
    private static BigInteger Floor(BigInteger value,int divisor){var quotient=BigInteger.DivRem(value,divisor,out var remainder);return remainder.Sign<0?quotient-1:quotient;}
    internal XdmValue ToXdm()=>XdmValue.FromDuration(Lexical,TypeName);
    private static void Invalid(string text)=>throw new NativeProfileException(NativeOutcome.Invalid,text);
}
