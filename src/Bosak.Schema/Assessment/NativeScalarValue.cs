// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Carries detached checked scalar values without schema authority.
// SPECIAL NOTES        : Bounded opt-in scalar profile; existing decimal facade remains unchanged.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 19-11-2026     | Add bounded opt-in Boolean values while preserving decimal and string contracts             |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Add bounded exact integer leaves with original typed provenance                           |
//                      | Charles Korthout | 0.4   | 24-11-2026     | Probe revised bounded date transport in isolated source copy |
//                      | Charles Korthout | 0.5   | 24-11-2026     | Adopt qualified bounded native date transport |
//                      | Charles Korthout | 0.6   | 25-11-2026     | Preserve simple nillability and actual typed-empty values |
//                      | Charles Korthout | 0.7   | 25-11-2026     | Admit bounded exact native calendar values and original typed transport |
//                      | Charles Korthout | 0.8   | 25-11-2026     | Add bounded binary and namespace-sensitive QName transport |
//                      | Charles Korthout | 0.9   | 25-11-2026     | Transport ordered compound items with original member provenance |
//                      | Charles Korthout | 0.10   | 26-11-2026     | Preserve native URI and IEEE floating-point datatype values |
//                      | Charles Korthout | 0.11   | 26-11-2026     | Preserve bounded exact native duration values and provenance |
//                      | Charles Korthout | 0.12   | 26-11-2026     | Add bounded native Gregorian fragments with original typed provenance |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Core.Xdm;

using System.Numerics;
using System.Collections.Immutable;

namespace Bosak.Schema;

/// <summary>Identifies the detached scalar families admitted by this bounded profile.</summary>
public enum NativeScalarKind
{
    /// <summary>A preserved XML string value.</summary>
    String = 1,
    /// <summary>An exact finite decimal value.</summary>
    Decimal = 2,
    /// <summary>A two-valued Boolean with no lexical or schema authority attached.</summary>
    Boolean = 3,
    /// <summary>An exact integer under the explicit native digit capacity.</summary>
    Integer = 4,
    /// <summary>A checked date without schema authority.</summary>
    Date = 5,
    /// <summary>A supported empty typed sequence, distinct from an empty string or absent typed value.</summary>
    Empty = 6,
    /// <summary>An exact dateTime without schema authority.</summary>
    DateTime = 7,
    /// <summary>An exact time without schema authority.</summary>
    Time = 8,
    /// <summary>An exact dateTimeStamp with required timezone and no schema authority.</summary>
    DateTimeStamp = 9,
    /// <summary>An immutable hexadecimal binary value without schema authority.</summary>
    HexBinary = 10,
    /// <summary>An immutable base64 binary value without schema authority.</summary>
    Base64Binary = 11,
    /// <summary>An expanded QName without schema authority.</summary>
    QName = 12,
    /// <summary>An IEEE binary32 value without schema authority.</summary>
    Float = 13,
    /// <summary>An IEEE binary64 value without schema authority.</summary>
    Double = 14,
    /// <summary>A collapsed URI primitive value without schema authority.</summary>
    AnyUri = 15,
    /// <summary>A duration with exact months and seconds, without schema authority.</summary>
    Duration = 16,
    /// <summary>A year-month duration, without schema authority.</summary>
    YearMonthDuration = 17,
    /// <summary>A day-time duration, without schema authority.</summary>
    DayTimeDuration = 18,
    /// <summary>A bounded Gregorian year without schema authority.</summary>
    GYear = 19,
    /// <summary>A bounded Gregorian year-month without schema authority.</summary>
    GYearMonth = 20,
    /// <summary>A Gregorian month without schema authority.</summary>
    GMonth = 21,
    /// <summary>A Gregorian month-day without schema authority.</summary>
    GMonthDay = 22,
    /// <summary>A Gregorian day without schema authority.</summary>
    GDay = 23
}

/// <summary>Contains a detached immutable scalar with no transferable schema authority.</summary>
public sealed class NativeScalarValue
{
    private readonly NativeGregorianValue? gregorian;
    private NativeScalarValue(NativeGregorianValue value) { gregorian=value; Kind=value.Kind switch { NativeGregorianKind.GYear=>NativeScalarKind.GYear,NativeGregorianKind.GYearMonth=>NativeScalarKind.GYearMonth,NativeGregorianKind.GMonth=>NativeScalarKind.GMonth,NativeGregorianKind.GMonthDay=>NativeScalarKind.GMonthDay,_=>NativeScalarKind.GDay }; }
    /// <summary>Gets the supplied year, or null when the Gregorian family has no year.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a Gregorian fragment family.</exception>
    public long? GregorianYear => gregorian.HasValue ? gregorian.Value.Year : throw new InvalidOperationException("The scalar is not a Gregorian fragment family.");
    /// <summary>Gets the supplied month, or null when the Gregorian family has no month.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a Gregorian fragment family.</exception>
    public int? GregorianMonth => gregorian.HasValue ? gregorian.Value.Month : throw new InvalidOperationException("The scalar is not a Gregorian fragment family.");
    /// <summary>Gets the supplied day, or null when the Gregorian family has no day.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a Gregorian fragment family.</exception>
    public int? GregorianDay => gregorian.HasValue ? gregorian.Value.Day : throw new InvalidOperationException("The scalar is not a Gregorian fragment family.");
    /// <summary>Gets supplied timezone minutes, or null when no timezone was supplied.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a Gregorian fragment family.</exception>
    public int? GregorianTimezoneOffsetMinutes => gregorian.HasValue ? gregorian.Value.Timezone : throw new InvalidOperationException("The scalar is not a Gregorian fragment family.");
    /// <summary>Gets collapsed original Gregorian text without canonical serialization.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a Gregorian fragment family.</exception>
    public string GregorianValue => gregorian.HasValue ? gregorian.Value.Lexical : throw new InvalidOperationException("The scalar is not a Gregorian fragment family.");
    private readonly NativeDurationValue? duration;
    private NativeScalarValue(NativeDurationValue value) { duration=value; Kind=value.Kind switch { NativeDurationKind.YearMonthDuration=>NativeScalarKind.YearMonthDuration, NativeDurationKind.DayTimeDuration=>NativeScalarKind.DayTimeDuration, _=>NativeScalarKind.Duration }; }
    /// <summary>Gets exact duration months, independently of seconds.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a duration family.</exception>
    public BigInteger DurationMonths => duration?.Months ?? throw new InvalidOperationException("The scalar is not a duration family.");
    /// <summary>Gets exact duration seconds, independently of months.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a duration family.</exception>
    public XdmExactDecimal DurationSeconds => duration?.Seconds ?? throw new InvalidOperationException("The scalar is not a duration family.");
    /// <summary>Gets collapsed original duration text; no canonical serialization is implied.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a duration family.</exception>
    public string DurationValue => duration?.Lexical ?? throw new InvalidOperationException("The scalar is not a duration family.");
    private readonly double floating;
    private NativeScalarValue(float value) { floating = value; Kind = NativeScalarKind.Float; }
    private NativeScalarValue(double value) { floating = value; Kind = NativeScalarKind.Double; }
    private NativeScalarValue(string value, NativeScalarKind kind) { text = value; Kind = kind; }
    /// <summary>Gets a detached IEEE binary32 value, preserving signed zero.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a float.</exception>
    public float FloatValue => Kind == NativeScalarKind.Float ? (float)floating : throw new InvalidOperationException("The scalar is not a float.");
    /// <summary>Gets a detached IEEE binary64 value, preserving signed zero.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a double.</exception>
    public double DoubleValue => Kind == NativeScalarKind.Double ? floating : throw new InvalidOperationException("The scalar is not a double.");
    /// <summary>Gets collapsed URI text without resolution, escaping or network access.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not an anyURI.</exception>
    public string UriValue => Kind == NativeScalarKind.AnyUri ? text! : throw new InvalidOperationException("The scalar is not an anyURI.");
    private readonly ImmutableArray<byte> binary;
    private readonly XsQName qname;
    private NativeScalarValue(XsQName value) { qname = value; Kind = NativeScalarKind.QName; }
    private NativeScalarValue(string value, bool base64) { binary = ImmutableArray.CreateRange(base64 ? Convert.FromBase64String(value) : Convert.FromHexString(value)); Kind = base64 ? NativeScalarKind.Base64Binary : NativeScalarKind.HexBinary; }
    /// <summary>Gets immutable decoded octets, usable after owner disposal.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not binary.</exception>
    public ImmutableArray<byte> BinaryValue => Kind is NativeScalarKind.HexBinary or NativeScalarKind.Base64Binary ? binary : throw new InvalidOperationException("The scalar is not binary.");
    /// <summary>Gets detached expanded QName components, usable after owner disposal.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a QName.</exception>
    public XsQName QNameValue => Kind == NativeScalarKind.QName ? qname : throw new InvalidOperationException("The scalar is not a QName.");
    private NativeScalarValue() { Kind = NativeScalarKind.Empty; }
    private readonly XdmExactCalendarValue calendar;
    private NativeScalarValue(XdmExactCalendarValue value)
    {
        calendar = value;
        Kind = value.Kind switch { XdmExactCalendarKind.DateTime => NativeScalarKind.DateTime, XdmExactCalendarKind.Time => NativeScalarKind.Time, XdmExactCalendarKind.DateTimeStamp => NativeScalarKind.DateTimeStamp, _ => throw new ArgumentException("Initialized calendar required.", nameof(value)) };
    }
    /// <summary>Gets exact detached calendar components, usable after owner disposal.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a calendar.</exception>
    public XdmExactCalendarValue CalendarValue => Kind is NativeScalarKind.DateTime or NativeScalarKind.Time or NativeScalarKind.DateTimeStamp ? calendar : throw new InvalidOperationException("The scalar is not a calendar.");
    private readonly NativeDateValue? date;
    private NativeScalarValue(XPathDateTime value) { date = new(value); Kind = NativeScalarKind.Date; }
    /// <summary>Gets the detached date components.</summary>
    /// <exception cref="InvalidOperationException">The scalar is not a date.</exception>
    public NativeDateValue DateValue => Kind == NativeScalarKind.Date ? date! : throw new InvalidOperationException("The scalar is not a date.");
    private readonly string? text;
    private readonly XdmExactDecimal number;
    private readonly bool logical;
    private readonly BigInteger integer;
    private NativeScalarValue(string text) { this.text = text; Kind = NativeScalarKind.String; }
    private NativeScalarValue(XdmExactDecimal number) { this.number = number; Kind = NativeScalarKind.Decimal; }
    private NativeScalarValue(bool logical) { this.logical = logical; Kind = NativeScalarKind.Boolean; }
    private NativeScalarValue(BigInteger integer) { this.integer = integer; Kind = NativeScalarKind.Integer; }
    /// <summary>Gets the admitted scalar family; consumers must handle future values.</summary>
    public NativeScalarKind Kind { get; }
    /// <summary>Gets the detached preserved string, usable after owner disposal.</summary>
    /// <exception cref="InvalidOperationException">This scalar is not a string.</exception>
    public string StringValue => Kind == NativeScalarKind.String ? text! : throw new InvalidOperationException("The scalar is not a string.");
    /// <summary>Gets the detached exact decimal, usable after owner disposal.</summary>
    /// <exception cref="InvalidOperationException">This scalar is not a decimal.</exception>
    public XdmExactDecimal DecimalValue => Kind == NativeScalarKind.Decimal ? number : throw new InvalidOperationException("The scalar is not a decimal.");
    /// <summary>Gets the detached Boolean, usable after owner disposal.</summary>
    /// <exception cref="InvalidOperationException">This scalar is not a Boolean.</exception>
    public bool BooleanValue => Kind == NativeScalarKind.Boolean ? logical : throw new InvalidOperationException("The scalar is not a Boolean.");
    /// <summary>Gets the detached exact integer, usable after owner disposal.</summary>
    /// <exception cref="InvalidOperationException">This scalar is not an integer.</exception>
    public BigInteger IntegerValue => Kind == NativeScalarKind.Integer ? integer : throw new InvalidOperationException("The scalar is not an integer.");
    internal static ImmutableArray<NativeScalarValue> FromItems(XdmValue value, CancellationToken token)
    {
        var result = ImmutableArray.CreateBuilder<NativeScalarValue>();
        if (value.Kind == XdmValueKind.Sequence)
        {
            foreach (var item in XdmSequence.FromSource(value.SequenceValue!)) { token.ThrowIfCancellationRequested(); result.Add(FromValue(item)); }
        }
        else if (value.Kind != XdmValueKind.Undefined) { token.ThrowIfCancellationRequested(); result.Add(FromValue(value)); }
        return result.ToImmutable();
    }
    internal static NativeScalarValue FromValue(XdmValue value) => value.IsExactCalendar ? new NativeScalarValue(value.ExactCalendarValue) : value.Kind switch
    {
        XdmValueKind.Undefined => new NativeScalarValue(),
        XdmValueKind.String when value.SchemaTypeName is "gYear" or "gYearMonth" or "gMonth" or "gMonthDay" or "gDay" => new NativeScalarValue(NativeGregorianValue.Parse(value.StringValue,NativeGregorianValue.KindFor(value.SchemaTypeName),256)),
        XdmValueKind.Duration => new NativeScalarValue(NativeDurationValue.Parse(value.DurationValue, value.SchemaTypeName == "yearMonthDuration" ? NativeDurationKind.YearMonthDuration : value.SchemaTypeName == "dayTimeDuration" ? NativeDurationKind.DayTimeDuration : NativeDurationKind.Duration, new(512,256),256)),
        XdmValueKind.Float => new NativeScalarValue((float)value.DoubleValue),
        XdmValueKind.Double => new NativeScalarValue(value.DoubleValue),
        XdmValueKind.String when value.SchemaTypeName == "anyURI" => new NativeScalarValue(value.StringValue, NativeScalarKind.AnyUri),
        XdmValueKind.QName => new NativeScalarValue(value.QNameValue),
        XdmValueKind.String when value.SchemaTypeName is "hexBinary" or "base64Binary" => new NativeScalarValue(value.StringValue, value.SchemaTypeName == "base64Binary"),
        XdmValueKind.String => new NativeScalarValue(value.StringValue),
        XdmValueKind.Decimal => new NativeScalarValue(value.ExactDecimalValue),
        XdmValueKind.Boolean => new NativeScalarValue(value.BooleanValue),
        XdmValueKind.Integer => new NativeScalarValue(value.BigIntegerValue),
        XdmValueKind.Date => new NativeScalarValue(value.DateXPathValue),
        _ => throw new NotSupportedException("The scalar family is outside the supported profile.")
    };
}
