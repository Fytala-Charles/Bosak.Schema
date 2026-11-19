// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Compiles the finite decimal assertion grammar and evaluates exact comparisons.
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
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Numerics;
using Bosak.XPath.Core.Xdm;

namespace Bosak.Schema;

/// <summary>Compiles the finite decimal assertion grammar and evaluates exact comparisons.</summary>
internal sealed class NativeDecimalAssertion
{
    private readonly Predicate predicate;
    private NativeDecimalAssertion(Predicate predicate) => this.predicate = predicate;

    internal static NativeDecimalAssertion Compile(string expression, HashSet<string> names, NativeNumericLimits limits,
        Func<string, string?>? resolveName = null)
    {
        try { return CompileCore(expression, names, limits, resolveName ?? (name => names.Contains(name) ? name : null)); }
        catch (NativeProfileException ex) when (ex.Outcome == NativeOutcome.Unsupported &&
            ex.UnsupportedReason != NativeDecimalUnsupportedReason.RepresentationCapacity)
        {
            throw new NativeProfileException(ex.Outcome, ex.Message, NativeDecimalUnsupportedReason.AssertionGrammar);
        }
    }

    private static NativeDecimalAssertion CompileCore(string expression, HashSet<string> names, NativeNumericLimits limits, Func<string, string?> resolveName)
    {
        NativeDecimalSchema.Require(expression.Length <= 512, "Assertion exceeds 512 characters.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        var tokens = new List<string>();
        for (var index = 0; index < expression.Length;)
        {
            if (expression[index] is ' ' or '\t' or '\r' or '\n') { index++; continue; }
            var start = index++;
            if (expression[start] is not '(' and not ')')
                while (index < expression.Length && expression[index] is not (' ' or '\t' or '\r' or '\n' or '(' or ')')) index++;
            tokens.Add(expression[start..index]);
            NativeDecimalSchema.Require(tokens.Count <= 128, "Assertion exceeds 128 tokens.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        }
        var parser = new Parser(tokens, resolveName, limits);
        var plan = parser.ParseOr();
        NativeDecimalSchema.Require(parser.AtEnd, "Trailing assertion tokens.");
        return new NativeDecimalAssertion(plan);
    }

    internal bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values) => predicate.Evaluate(values);

    private abstract class Predicate
    {
        internal abstract bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values);
    }

    private sealed class Logical(Predicate left, Predicate right, bool and) : Predicate
    {
        internal override bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values) =>
            and ? left.Evaluate(values) && right.Evaluate(values) : left.Evaluate(values) || right.Evaluate(values);
    }

    private sealed class Presence(string name, bool exists) : Predicate
    {
        internal override bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values) =>
            exists == (values[name].Count != 0);
    }

    private sealed class Comparison(Operand left, Operand right, string operation) : Predicate
    {
        internal override bool Evaluate(Dictionary<string, List<XdmExactDecimal>> values)
        {
            var a = left.Get(values);
            var b = right.Get(values);
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

    private sealed class Operand(string? name, bool count, XdmExactDecimal? constant)
    {
        internal XdmExactDecimal? Get(Dictionary<string, List<XdmExactDecimal>> values)
        {
            if (name is null) return constant;
            var items = values[name];
            if (count) return new XdmExactDecimal(items.Count, 0, 2, 0);
            if (items.Count > 1) throw new NativeProfileException(NativeOutcome.Invalid, "Assertion scalar operand has multiple items (XPTY0004).");
            return items.Count == 0 ? null : items[0];
        }
    }

    private sealed class Parser(List<string> tokens, Func<string, string?> resolveName, NativeNumericLimits limits)
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
            NativeDecimalSchema.Require(operation is "eq" or "ne" or "lt" or "le" or "gt" or "ge", "Unsupported comparison operator.");
            return new Comparison(left, ParseOperand(), operation);
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
            var resolved = resolveName(Peek());
            if (resolved is not null) { Next(); return new Operand(resolved, false, null); }
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
