// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Carries detached checked string, decimal or Boolean values without schema authority.
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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

/// <summary>Identifies the detached scalar families admitted by this bounded profile.</summary>
public enum NativeScalarKind
{
    /// <summary>A preserved XML string value.</summary>
    String = 1,
    /// <summary>An exact finite decimal value.</summary>
    Decimal = 2,
    /// <summary>A two-valued Boolean with no lexical or schema authority attached.</summary>
    Boolean = 3
}

/// <summary>Contains a detached immutable scalar with no transferable schema authority.</summary>
public sealed class NativeScalarValue
{
    private readonly string? text;
    private readonly XdmExactDecimal number;
    private readonly bool logical;
    private NativeScalarValue(string text) { this.text = text; Kind = NativeScalarKind.String; }
    private NativeScalarValue(XdmExactDecimal number) { this.number = number; Kind = NativeScalarKind.Decimal; }
    private NativeScalarValue(bool logical) { this.logical = logical; Kind = NativeScalarKind.Boolean; }
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
    internal static NativeScalarValue FromValue(XdmValue value) => value.Kind switch
    {
        XdmValueKind.String => new NativeScalarValue(value.StringValue),
        XdmValueKind.Decimal => new NativeScalarValue(value.ExactDecimalValue),
        XdmValueKind.Boolean => new NativeScalarValue(value.BooleanValue),
        _ => throw new NotSupportedException("The scalar family is outside the supported profile.")
    };
}
