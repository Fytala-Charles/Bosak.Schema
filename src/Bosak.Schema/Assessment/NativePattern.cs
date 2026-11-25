// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Compiles bounded Unicode-scalar pattern automata with explicit work accounting.
// SPECIAL NOTES        : Internal finite native profile; no Core adapter or full XSD 1.1 conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 22-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 22-11-2026     | Prototype bounded string patterns and per-assessment accounting |
//                      | Charles Korthout | 0.3   | 22-11-2026     | Adopt qualified bounded native string patterns |
//                      | Charles Korthout | 0.4   | 24-11-2026     | Share schema base-literal pattern work and cancellation across date types |
//                      | Charles Korthout | 0.5   | 25-11-2026     | Preserve bounded list/union types and ordered selected member facts |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Text;
namespace Bosak.Schema;

internal sealed class NativePatternBudget(CancellationToken token, int limit = 4_194_304)
{
    private int consumed;
    internal int Consumed => Volatile.Read(ref consumed);
    private int compoundAttempts;
    internal void CompoundAttempt()
    {
        Tick();
        NativeDecimalSchema.Require(++compoundAttempts <= 4096, "Compound attempt budget exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
    }
    internal void Tick()
    {
        token.ThrowIfCancellationRequested();
        NativePattern.Capacity(consumed < limit);
        consumed++;
    }
}
internal sealed class NativePatternCompilation(CancellationToken token)
{
    private readonly NativePatternBudget matchingBudget = new(token);
    internal NativePatternBudget MatchingBudget => matchingBudget;
    internal int States { get; private set; }
    internal void Check() => token.ThrowIfCancellationRequested();
    internal void Reserve(int states)
    { Check(); NativePattern.Capacity(states <= 1024 - States); States += states; }
}
internal sealed class NativePattern
{
    internal static void Capacity(bool value) => NativeDecimalSchema.Require(value,
        "Pattern representation/work capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
    private static void Valid(bool value)
    { if (!value) throw new NativeProfileException(NativeOutcome.Invalid, "Malformed bounded XSD pattern.", NativeDecimalUnsupportedReason.ValueType); }
    private static void Unsupported() => throw new NativeProfileException(NativeOutcome.Unsupported,
        "Pattern syntax is outside the finite subset.", NativeDecimalUnsupportedReason.ValueType);
    private sealed record Member(int Low, int High, int Mode = 0)
    { internal bool Single => Mode == 0 && Low == High; }
    private sealed record Set(ImmutableArray<Member> Members, bool Negative = false)
    {
        internal bool Match(int value, NativePatternBudget budget)
        {
            bool found = false;
            foreach (var member in Members)
            {
                budget.Tick(); bool space = value is 0x20 or 9 or 10 or 13;
                if (member.Mode == 1 ? space : member.Mode == 2 ? !space : value >= member.Low && value <= member.High)
                { found = true; break; }
            }
            return Negative ? !found : found;
        }
    }
    private abstract record Tree;
    private sealed record Empty : Tree;
    private sealed record Atom(Set Characters) : Tree;
    private sealed record Sequence(ImmutableArray<Tree> Items) : Tree;
    private sealed record Alternative(ImmutableArray<Tree> Items) : Tree;
    private sealed record Repeat(Tree Item, int Minimum, int Maximum) : Tree;
    private sealed record State(Set? Characters, int Next, int Other = -1);
    private readonly ImmutableArray<State> states;
    private readonly int start;
    internal int StateCount => states.Length;
    private NativePattern(ImmutableArray<State> states, int start) { this.states = states; this.start = start; }
    internal static NativePattern Compile(string expression, NativePatternCompilation compilation)
    {
        compilation.Check(); Capacity(expression.Length <= 256);
        var parser = new Parser(expression, compilation); var tree = parser.Parse();
        var states = new List<State>(128) { new(null, -1) }; // State0 is the sole accept state.
        int Emit(State state) { compilation.Check(); Capacity(states.Count < 128); states.Add(state); return states.Count - 1; }
        int Build(Tree node, int next)
        {
            compilation.Check();
            switch (node)
            {
                case Empty: return next;
                case Atom atom: return Emit(new(atom.Characters, next));
                case Sequence sequence:
                    for (int i = sequence.Items.Length - 1; i >= 0; i--) next = Build(sequence.Items[i], next);
                    return next;
                case Alternative alternative:
                    var entries = alternative.Items.Select(item => Build(item, next)).ToArray();
                    int entry = entries[^1];
                    for (int i = entries.Length - 2; i >= 0; i--) entry = Emit(new(null, entries[i], entry));
                    return entry;
                case Repeat repeat:
                    if (repeat.Maximum < 0)
                    {
                        int split = Emit(new(null, -1, next)); int body = Build(repeat.Item, split);
                        states[split] = new(null, body, next); next = split;
                    }
                    else for (int i = repeat.Minimum; i < repeat.Maximum; i++) next = Emit(new(null, Build(repeat.Item, next), next));
                    for (int i = 0; i < repeat.Minimum; i++) next = Build(repeat.Item, next);
                    return next;
                default: throw new InvalidOperationException("Unknown pattern tree.");
            }
        }
        int first = Build(tree, 0); compilation.Reserve(states.Count);
        return new(states.ToImmutableArray(), first);
    }
    internal bool Match(string text, NativePatternBudget budget)
    {
        bool[] Closure(IEnumerable<int> seeds)
        {
            var visited = new bool[states.Length]; var stack = new Stack<int>(seeds);
            while (stack.TryPop(out int index))
            {
                budget.Tick(); if (visited[index]) continue; visited[index] = true;
                var state = states[index];
                if (index != 0 && state.Characters is null)
                { if (state.Next >= 0) stack.Push(state.Next); if (state.Other >= 0) stack.Push(state.Other); }
            }
            return visited;
        }
        var active = Closure([start]);
        foreach (var rune in text.EnumerateRunes())
        {
            var next = new List<int>(states.Length);
            for (int i = 1; i < states.Length; i++)
            {
                budget.Tick();
                if (!active[i] || states[i].Characters is not { } set) continue;
                if (set.Match(rune.Value, budget)) next.Add(states[i].Next);
            }
            if (next.Count == 0) return false;
            active = Closure(next);
        }
        return active[0];
    }
    private sealed class Parser(string text, NativePatternCompilation compilation)
    {
        private int index, atoms, depth;
        private char Current => index < text.Length ? text[index] : '\0';
        private bool Take(char value) { if (Current != value || index == text.Length) return false; index++; return true; }
        internal Tree Parse() { var result = Or(); Valid(index == text.Length); return result; }
        private Tree Or()
        {
            var values = ImmutableArray.CreateBuilder<Tree>(); values.Add(Branch());
            while (Take('|')) values.Add(Branch());
            return values.Count == 1 ? values[0] : new Alternative(values.ToImmutable());
        }
        private Tree Branch()
        {
            var values = ImmutableArray.CreateBuilder<Tree>();
            while (index < text.Length && Current is not ')' and not '|')
            {
                compilation.Check(); var value = Unit();
                if (Take('?')) value = new Repeat(value, 0, 1);
                else if (Take('*')) value = new Repeat(value, 0, -1);
                else if (Take('+')) value = new Repeat(value, 1, -1);
                else if (Take('{'))
                {
                    int minimum = Number(); int maximum = minimum;
                    if (Take(',')) maximum = Current == '}' ? -1 : Number();
                    Valid(Take('}') && (maximum < 0 || minimum <= maximum)); value = new Repeat(value, minimum, maximum);
                }
                Valid(Current is not '?' and not '*' and not '+' and not '{'); values.Add(value);
            }
            return values.Count switch { 0 => new Empty(), 1 => values[0], _ => new Sequence(values.ToImmutable()) };
        }
        private int Number()
        {
            Valid(Current is >= '0' and <= '9'); int value = 0;
            while (Current is >= '0' and <= '9') { compilation.Check(); value = value * 10 + text[index++] - '0'; Capacity(value <= 16); }
            return value;
        }
        private Tree Unit()
        {
            Capacity(++atoms <= 64); compilation.Check();
            if (Take('(')) { Capacity(++depth <= 8); var value = Or(); Valid(Take(')')); depth--; return value; }
            if (Take('.')) return new Atom(new([new(10, 10), new(13, 13)], true));
            if (Take('[')) return new Atom(Class());
            Valid(Current is not ')' and not ']' and not '?' and not '*' and not '+' and not '{' and not '}');
            return new Atom(new([Character().Value]));
        }
        private readonly record struct Part(Member Value, bool RawDash = false);
        private Part Character()
        {
            compilation.Check(); Valid(index < text.Length);
            if (Take('\\'))
            {
                Valid(index < text.Length); char code = text[index++];
                if (code is 'd' or 'D' or 'w' or 'W' or 'i' or 'I' or 'c' or 'C' or 'p' or 'P') Unsupported();
                if (code is 's' or 'S') return new(new(0, 0, code == 's' ? 1 : 2));
                int value = code switch { 'n' => 10, 'r' => 13, 't' => 9, _ => code };
                Valid("nrt\\|.-^?*+{}()[]".Contains(code)); return new(new(value, value));
            }
            Valid(Current is not '[' and not ']');
            Valid(Rune.TryGetRuneAt(text, index, out var rune)); index += rune.Utf16SequenceLength;
            return new(new(rune.Value, rune.Value), rune.Value == '-');
        }
        private Set Class()
        {
            bool negative = Take('^'); var members = ImmutableArray.CreateBuilder<Member>();
            while (index < text.Length && Current != ']')
            {
                compilation.Check(); if (Current == '-' && index + 1 < text.Length && text[index + 1] == '[') Unsupported();
                var first = Character();
                if (first.Value.Single && Current == '-' && index + 1 < text.Length && text[index + 1] != ']')
                {
                    if (text[index + 1] == '[' || text[index + 1] == '-' && index + 2 < text.Length && text[index + 2] == '[') Unsupported();
                    index++; var last = Character();
                    Valid(first.Value.Single && last.Value.Single && !first.RawDash && !last.RawDash && first.Value.Low <= last.Value.Low);
                    members.Add(new(first.Value.Low, last.Value.Low));
                }
                else members.Add(first.Value);
                Capacity(members.Count <= 16);
            }
            Valid(members.Count != 0 && Take(']')); return new(members.ToImmutable(), negative);
        }
    }
}
