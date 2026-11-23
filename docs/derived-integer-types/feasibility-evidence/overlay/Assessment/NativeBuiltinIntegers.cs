// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Issues finite built-in integer types with original parents and exact inherited bounds.
// SPECIAL NOTES        : BigInteger value-space checks; inherited fractionDigits zero remains fixed.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 22-11-2026     | Isolated native builtin integer feasibility |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Numerics;
using System.Xml.Linq;
namespace Bosak.Schema;
internal sealed partial class NativeDecimalSchema
{
    private static Dictionary<XName, NativeType> CreateBuiltinIntegerTypes(NativeType? integer)
    {
        var result = new Dictionary<XName, NativeType>();
        if (integer is null) return result;
        void Add(string name, string parentName, BigInteger? lower, BigInteger? upper)
        {
            var parent = parentName == "integer" ? integer : result[Xs + parentName];
            result.Add(Xs + name, new NativeType(name, false, Xs.NamespaceName, isInteger: true,
                integerFacets: new(lower, false, upper, false, null, parent.IntegerFacets), derivationBase: parent));
        }
        Add("nonPositiveInteger", "integer", null, BigInteger.Parse("0", System.Globalization.CultureInfo.InvariantCulture));
        Add("negativeInteger", "nonPositiveInteger", null, BigInteger.Parse("-1", System.Globalization.CultureInfo.InvariantCulture));
        Add("long", "integer", BigInteger.Parse("-9223372036854775808", System.Globalization.CultureInfo.InvariantCulture), BigInteger.Parse("9223372036854775807", System.Globalization.CultureInfo.InvariantCulture));
        Add("int", "long", BigInteger.Parse("-2147483648", System.Globalization.CultureInfo.InvariantCulture), BigInteger.Parse("2147483647", System.Globalization.CultureInfo.InvariantCulture));
        Add("short", "int", BigInteger.Parse("-32768", System.Globalization.CultureInfo.InvariantCulture), BigInteger.Parse("32767", System.Globalization.CultureInfo.InvariantCulture));
        Add("byte", "short", BigInteger.Parse("-128", System.Globalization.CultureInfo.InvariantCulture), BigInteger.Parse("127", System.Globalization.CultureInfo.InvariantCulture));
        Add("nonNegativeInteger", "integer", BigInteger.Parse("0", System.Globalization.CultureInfo.InvariantCulture), null);
        Add("unsignedLong", "nonNegativeInteger", BigInteger.Parse("0", System.Globalization.CultureInfo.InvariantCulture), BigInteger.Parse("18446744073709551615", System.Globalization.CultureInfo.InvariantCulture));
        Add("unsignedInt", "unsignedLong", BigInteger.Parse("0", System.Globalization.CultureInfo.InvariantCulture), BigInteger.Parse("4294967295", System.Globalization.CultureInfo.InvariantCulture));
        Add("unsignedShort", "unsignedInt", BigInteger.Parse("0", System.Globalization.CultureInfo.InvariantCulture), BigInteger.Parse("65535", System.Globalization.CultureInfo.InvariantCulture));
        Add("unsignedByte", "unsignedShort", BigInteger.Parse("0", System.Globalization.CultureInfo.InvariantCulture), BigInteger.Parse("255", System.Globalization.CultureInfo.InvariantCulture));
        Add("positiveInteger", "nonNegativeInteger", BigInteger.Parse("1", System.Globalization.CultureInfo.InvariantCulture), null);
        return result;
    }
}
