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
    QName = 12
}

/// <summary>Contains a detached immutable scalar with no transferable schema authority.</summary>
public sealed class NativeScalarValue
{
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
