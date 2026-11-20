// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Compiles bounded decimal and opt-in string assertions with exact comparison semantics.
// SPECIAL NOTES        : Internal finite native profile; no Core adapter or full XSD 1.1 conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 15-11-2026     | Implement bounded native decimal backend                                                 |
//                      | Charles Korthout | 0.2   | 19-11-2026     | Carry bounded Unsupported reasons without changing refusal outcomes                       |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Adopt bounded expanded names and optional native assertions                              |
//                      | Charles Korthout | 0.4   | 20-11-2026     | Add opt-in string equality, literal and sequence semantics under existing bounds          |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Numerics;
using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

/// <summary>Compiles bounded decimal and opt-in string assertions with exact comparison semantics.</summary>
internal sealed class NativeDecimalAssertion
{
    private readonly Predicate predicate;
    private NativeDecimalAssertion(Predicate predicate) => this.predicate = predicate;

    internal static NativeDecimalAssertion Compile(string expression, HashSet<string> names, NativeNumericLimits limits,
        Func<string, string?>? resolveName = null, HashSet<string>? stringNames = null)
    {
        try { return CompileCore(expression, names, limits, resolveName ?? (name => names.Contains(name) ? name : null), stringNames); }
        catch (NativeProfileException ex) when (ex.Outcome == NativeOutcome.Unsupported &&
            ex.UnsupportedReason != NativeDecimalUnsupportedReason.RepresentationCapacity)
        {
            throw new NativeProfileException(ex.Outcome, ex.Message, NativeDecimalUnsupportedReason.AssertionGrammar);
        }
    }

    private static NativeDecimalAssertion CompileCore(string expression, HashSet<string> names, NativeNumericLimits limits, Func<string, string?> resolveName, HashSet<string>? stringNames)
    {
        NativeDecimalSchema.Require(expression.Length <= 512, "Assertion exceeds 512 characters.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var tokens = new List<string>();
        for (var index = 0; index < expression.Length;)
        {
            if (expression[index] is ' ' or '\t' or '\r' or '\n') { index++; continue; }
            var start = index++;
            if (stringNames is not null && expression[start] is '\'' or '"')
            {
                var delimiter = expression[start];
                var closed = false;
                while (index < expression.Length)
                {
                    if (expression[index++] != delimiter) continue;
                    if (index < expression.Length && expression[index] == delimiter) { index++; continue; }
                    closed = true;
                    break;
                }
                NativeDecimalSchema.Require(closed, "Unterminated assertion string literal.");
            }
            else if (stringNames is not null && expression[start] is '=' or '!')
            {
                if (expression[start] == '!')
                {
                    NativeDecimalSchema.Require(index < expression.Length && expression[index] == '=', "Unsupported assertion token.");
                    index++;
                }
            }
            else if (expression[start] is not '(' and not ')')
                while (index < expression.Length && expression[index] is not (' ' or '\t' or '\r' or '\n' or '(' or ')') &&
                    (stringNames is null || expression[index] is not ('\'' or '"' or '=' or '!'))) index++;
            tokens.Add(expression[start..index]);
            NativeDecimalSchema.Require(tokens.Count <= 128, "Assertion exceeds 128 tokens.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        }
        var parser = new Parser(tokens, resolveName, limits, stringNames);
        var plan = parser.ParseOr();
        NativeDecimalSchema.Require(parser.AtEnd, "Trailing assertion tokens.");
        return new NativeDecimalAssertion(plan);
    }

    internal bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values, Dictionary<string, List<string>>? strings = null) => predicate.Evaluate(values, strings);

    private abstract class Predicate
    {
        internal abstract bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values, Dictionary<string, List<string>>? strings);
    }

    private sealed class Logical(Predicate left, Predicate right, bool and) : Predicate
    {
        internal override bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values, Dictionary<string, List<string>>? strings) =>
            and ? left.Evaluate(values, strings) && right.Evaluate(values, strings) : left.Evaluate(values, strings) || right.Evaluate(values, strings);
    }

    private sealed class Presence(string name, bool exists) : Predicate
    {
        internal override bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values, Dictionary<string, List<string>>? strings) =>
            exists == ((strings is not null && strings.TryGetValue(name, out var text) ? text.Count : values[name].Count) != 0);
    }

    private sealed class Comparison(Operand left, Operand right, string operation) : Predicate
    {
        internal override bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values, Dictionary<string, List<string>>? strings)
        {
            var a = left.Get(values, strings);
            var b = right.Get(values, strings);
            if (a is null || b is null) return false;
            var scale = Math.Max(a.Value.Scale, b.Value.Scale);
            var coefficientA = a.Value.Coefficient * BigInteger.Pow(10, scale - a.Value.Scale);
            var coefficientB = b.Value.Coefficient * BigInteger.Pow(10, scale - b.Value.Scale);
            var comparison = coefficientA.CompareTo(coefficientB);
            return operation switch
            {
                "eq" => comparison == 0, "ne" => comparison != 0,
                "lt" => comparison < 0, "le" => comparison <= 0,
                "gt" => comparison > 0, "ge" => comparison >= 0,
                _ => throw new InvalidOperationException("Uncompiled comparison.")
            };
        }
    }

    private sealed class StringComparison(Operand left, Operand right, string operation) : Predicate
    {
        internal override bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values, Dictionary<string, List<string>>? strings)
        {
            var a = left.GetStrings(strings);
            var b = right.GetStrings(strings);
            if (operation is "eq" or "ne")
            {
                if (a.Count > 1 || b.Count > 1)
                    throw new NativeProfileException(NativeOutcome.Invalid, "Assertion scalar operand has multiple items (XPTY0004).");
                if (a.Count == 0 || b.Count == 0) return false;
            }
            var equal = operation is "eq" or "=";
            foreach (var first in a)
                foreach (var second in b)
                    if (string.Equals(first, second, System.StringComparison.Ordinal) == equal) return true;
            return false;
        }
    }

    private sealed class Operand(string? name, bool count, XdmExactDecimal? constant, bool isString = false, string? stringConstant = null)
    {
        internal bool IsString => isString;
        internal IReadOnlyList<string> GetStrings(Dictionary<string, List<string>>? strings) =>
            name is null ? [stringConstant!] : strings![name];
        internal XdmExactDecimal? Get(Dictionary<string, List<XdmExactDecimal>> values, Dictionary<string, List<string>>? strings)
        {
            if (name is null) return constant;
            var items = values[name];
            if (count) return new XdmExactDecimal(strings is not null && strings.TryGetValue(name, out var text) ? text.Count : items.Count, 0, 2, 0);
            if (items.Count > 1) throw new NativeProfileException(NativeOutcome.Invalid, "Assertion scalar operand has multiple items (XPTY0004).");
            return items.Count == 0 ? null : items[0];
        }
    }

    private sealed class Parser(List<string> tokens, Func<string, string?> resolveName, NativeNumericLimits limits, HashSet<string>? stringNames)
    {
        private int position;
        private int depth;
        internal bool AtEnd => position == tokens.Count;

        internal Predicate ParseOr()
        {
            var result = ParseAnd();
            while (Take("or")) result = new Logical(result, ParseAnd(), false);
            return result;
        }

        private Predicate ParseAnd()
        {
            var result = ParsePrimary();
            while (Take("and")) result = new Logical(result, ParsePrimary(), true);
            return result;
        }

        private Predicate ParsePrimary()
        {
            if (Take("("))
            {
                Enter();
                var result = ParseOr();
                Expect(")");
                depth--;
                return result;
            }
            if ((Peek() is "exists" or "empty") && Peek(1) == "(")
            {
                var exists = Next() == "exists";
                Expect("(");
                Enter();
                var name = ChildName();
                Expect(")");
                depth--;
                return new Presence(name, exists);
            }
            var left = ParseOperand();
            var operation = Next();
            NativeDecimalSchema.Require(operation is "eq" or "ne" or "lt" or "le" or "gt" or "ge" ||
                stringNames is not null && operation is "=" or "!=", "Unsupported comparison operator.");
            var right = ParseOperand();
            if (left.IsString || right.IsString)
            {
                NativeDecimalSchema.Require(left.IsString && right.IsString && operation is "eq" or "ne" or "=" or "!=",
                    "Only string equality operands are admitted.");
                return new StringComparison(left, right, operation);
            }
            NativeDecimalSchema.Require(operation is "eq" or "ne" or "lt" or "le" or "gt" or "ge", "Unsupported comparison operator.");
            return new Comparison(left, right, operation);
        }

        private Operand ParseOperand()
        {
            if (Peek() == "count" && Peek(1) == "(")
            {
                Next();
                Expect("(");
                Enter();
                var name = ChildName();
                Expect(")");
                depth--;
                return new Operand(name, true, null);
            }
            if (stringNames is not null && Peek().Length >= 2 && Peek()[0] is '\'' or '"')
            {
                var literal = Next();
                var delimiter = literal[0];
                return new Operand(null, false, null, true, literal[1..^1].Replace(new string(delimiter, 2), delimiter.ToString(), System.StringComparison.Ordinal));
            }
            var resolved = resolveName(Peek());
            if (resolved is not null) { Next(); return new Operand(resolved, false, null, stringNames?.Contains(resolved) == true); }
            return new Operand(null, false, NativeDecimalSchema.ParseDecimal(Next(), limits, NativeOutcome.Unsupported));
        }

        private string ChildName()
        {
            var name = resolveName(Next());
            NativeDecimalSchema.Require(name is not null, "Assertion references an unknown child.");
            return name!;
        }

        private string Peek(int offset = 0) => position + offset < tokens.Count ? tokens[position + offset] : "";
        private string Next()
        {
            NativeDecimalSchema.Require(!AtEnd, "Incomplete assertion expression.");
            return tokens[position++];
        }
        private bool Take(string token)
        {
            if (Peek() != token) return false;
            position++;
            return true;
        }
        private void Expect(string token) => NativeDecimalSchema.Require(Take(token), "Unexpected assertion token.");
        private void Enter()
        {
            depth++;
            NativeDecimalSchema.Require(depth <= 16, "Assertion nesting exceeds sixteen.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        }
    }
}
