// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Compiles bounded contextual integer assertions for an isolated native experiment.
// SPECIAL NOTES        : Isolated prototype; no production semantic adoption or public authority issuance.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 21-11-2026     | Adopt bounded integer simple content with actual component identity |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;

namespace Bosak.Schema;

internal sealed class NativeIntegerAssertion
{
    internal sealed record Context(string Text, BigInteger Integer)
    {
        internal string TypeAnnotation => "{http://www.w3.org/2001/XMLSchema}anyType";
        internal string AtomizedKind => "untypedAtomic";
    }
    private sealed class DynamicError : Exception { }
    private sealed record Atom(string Kind, BigInteger Integer = default, double Number = 0, string Text = "");
    private sealed record Token(string Kind, string Text);
    private readonly Func<Context, bool> predicate;
    internal ImmutableDictionary<string, string> Namespaces { get; }
    internal string DefaultNamespace { get; }
    internal string BaseUri { get; }
    internal string Expression { get; }
    private NativeIntegerAssertion(Func<Context, bool> predicate, string expression,
        ImmutableDictionary<string, string> namespaces, string defaultNamespace, string baseUri)
    { this.predicate = predicate; Expression = expression; Namespaces = namespaces; DefaultNamespace = defaultNamespace; BaseUri = baseUri; }

    internal static NativeIntegerAssertion Compile(string expression, NativeNumericLimits limits,
        ImmutableDictionary<string, string> namespaces, string defaultNamespace, string baseUri)
    {
        Need(expression.Length <= 512, true);
        var tokens = new List<Token>();
        for (var index = 0; index < expression.Length;)
        {
            var c = expression[index];
            if (c is ' ' or '\t' or '\r' or '\n') { index++; continue; }
            var start = index++;
            if (c is '\'' or '"')
            {
                var text = new System.Text.StringBuilder(); var closed = false;
                while (index < expression.Length)
                {
                    var next = expression[index++];
                    if (next != c) { text.Append(next); continue; }
                    if (index < expression.Length && expression[index] == c) { text.Append(c); index++; continue; }
                    closed = true; break;
                }
                Need(closed); tokens.Add(new("string", text.ToString()));
            }
            else if (c is >= '0' and <= '9')
            {
                while (index < expression.Length && expression[index] is >= '0' and <= '9') index++;
                tokens.Add(new("integer", expression[start..index]));
            }
            else if (char.IsAsciiLetter(c) || c is '$' or '_')
            {
                while (index < expression.Length && (char.IsAsciiLetterOrDigit(expression[index]) || expression[index] is '_' or ':' or '-')) index++;
                tokens.Add(new("word", expression[start..index]));
            }
            else
            {
                Need(c is '.' or '(' or ')' or ',' or '=' or '!' or '+' or '-');
                if (c == '!') { Need(index < expression.Length && expression[index] == '='); index++; }
                tokens.Add(new("symbol", expression[start..index]));
            }
            Need(tokens.Count <= 128, true);
        }
        var parser = new Parser(tokens, limits); var predicate = parser.ParseOr(); Need(parser.End);
        return new(predicate, expression, namespaces, defaultNamespace, baseUri);
    }
    internal bool Evaluate(Context context)
    {
        try { return predicate(context); }
        catch (DynamicError) { return false; }
    }
    private static void Need(bool condition, bool capacity = false) => NativeDecimalSchema.Require(condition,
        capacity ? "Integer assertion capacity exceeded." : "Integer assertion grammar is outside the profile.",
        capacity ? NativeDecimalUnsupportedReason.RepresentationCapacity : NativeDecimalUnsupportedReason.AssertionGrammar);
    private static double AsDouble(Atom atom)
    {
        if (atom.Kind == "double") return atom.Number;
        var text = atom.Kind == "integer" ? atom.Integer.ToString(CultureInfo.InvariantCulture) : atom.Text;
        if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var number)) throw new DynamicError();
        return number;
    }
    private static Atom Remainder(Atom left, BigInteger divisor)
    {
        if (left.Kind == "integer")
        { if (divisor.IsZero) throw new DynamicError(); return new("integer", left.Integer % divisor); }
        if (left.Kind is "untyped" or "double") return new("double", Number: AsDouble(left) % AsDouble(new("integer", divisor)));
        throw new DynamicError();
    }
    private static bool Compare(Atom left, Atom right, string operation, bool general)
    {
        if (general)
        {
            if (left.Kind == "untyped") left = new("double", Number: AsDouble(left));
            if (right.Kind == "untyped") right = new("double", Number: AsDouble(right));
        }
        else
        {
            if (left.Kind == "untyped") left = new("string", Text: left.Text);
            if (right.Kind == "untyped") right = new("string", Text: right.Text);
        }
        if (left.Kind == "string" || right.Kind == "string")
        {
            if (left.Kind != "string" || right.Kind != "string" || operation is not ("eq" or "ne")) throw new DynamicError();
            var same = string.Equals(left.Text, right.Text, StringComparison.Ordinal);
            return operation == "eq" ? same : !same;
        }
        int comparison;
        if (left.Kind == "double" || right.Kind == "double")
        {
            var a = AsDouble(left); var b = AsDouble(right);
            if (double.IsNaN(a) || double.IsNaN(b)) return operation == "ne";
            comparison = a.CompareTo(b);
        }
        else comparison = left.Integer.CompareTo(right.Integer);
        return operation switch { "eq" => comparison == 0, "ne" => comparison != 0, "lt" => comparison < 0,
            "le" => comparison <= 0, "gt" => comparison > 0, "ge" => comparison >= 0, _ => throw new DynamicError() };
    }
    private sealed class Parser(List<Token> tokens, NativeNumericLimits limits)
    {
        private int position, depth;
        internal bool End => position == tokens.Count;
        private string Peek => End ? "" : tokens[position].Text;
        private bool Take(string token) { if (Peek != token || End || tokens[position].Kind == "string") return false; position++; return true; }
        private void Expect(string token) => Need(Take(token));
        private void Enter() { depth++; Need(depth <= 16, true); }
        internal Func<Context, bool> ParseOr()
        {
            var plan = ParseAnd();
            while (Take("or")) { var left = plan; var right = ParseAnd(); plan = c => left(c) || right(c); }
            return plan;
        }
        private Func<Context, bool> ParseAnd()
        {
            var plan = Primary();
            while (Take("and")) { var left = plan; var right = Primary(); plan = c => left(c) && right(c); }
            return plan;
        }
        private Func<Context, bool> Primary()
        {
            if (Take("(")) { Enter(); var inner = ParseOr(); Expect(")"); depth--; return inner; }
            var left = Operand(); Need(!End); var operation = Peek; position++;
            if (operation is "=" or "!=")
            {
                var sequence = Sequence(); var op = operation == "=" ? "eq" : "ne";
                return c => { var a = left(c); foreach (var b in sequence) if (Compare(a, new("integer", b), op, true)) return true; return false; };
            }
            Need(operation is "eq" or "ne" or "lt" or "le" or "gt" or "ge");
            var right = Operand(); return c => Compare(left(c), right(c), operation, false);
        }
        private Func<Context, Atom> Operand()
        {
            Need(!End); Func<Context, Atom> value;
            if (Take(".")) value = c => new("untyped", Text: c.Text);
            else if (Take("$value")) value = c => new("integer", c.Integer);
            else if (tokens[position].Kind == "string") { var text = tokens[position++].Text; value = _ => new("string", Text: text); }
            else { var integer = Integer(); value = _ => new("integer", integer); }
            if (Take("mod")) { var dividend = value; var divisor = Integer(); value = c => Remainder(dividend(c), divisor); }
            return value;
        }
        private BigInteger Integer()
        {
            var negative = Take("-"); if (!negative) Take("+"); Need(!End && tokens[position].Kind == "integer");
            var text = tokens[position++].Text; Need(text.Length <= 256 && text.Length <= limits.MaxDigits, true);
            var value = BigInteger.Parse(text, NumberStyles.None, CultureInfo.InvariantCulture); return negative ? -value : value;
        }
        private BigInteger[] Sequence()
        {
            var result = new List<BigInteger>(); var grouped = Take("(");
            if (grouped) { Enter(); if (Take(")")) { depth--; return []; } }
            do
            {
                var first = Integer(); var last = Take("to") ? Integer() : first;
                var count = BigInteger.Max(BigInteger.Zero, last - first + 1); Need(count <= 32 - result.Count, true);
                for (var n = first; n <= last; n++) result.Add(n);
            } while (grouped && Take(","));
            if (grouped) { Expect(")"); depth--; }
            return result.ToArray();
        }
    }
}
