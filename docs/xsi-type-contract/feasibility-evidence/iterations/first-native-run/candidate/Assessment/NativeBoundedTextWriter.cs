// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Bounds XML export before appending output and observes cancellation.
// SPECIAL NOTES        : Bounded opt-in string/decimal profile; existing decimal facade remains unchanged.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Text;
using System.Xml;

namespace Bosak.Schema;

/// <summary>Bounds XML export before appending output and observes cancellation.</summary>
internal sealed class NativeBoundedTextWriter(int maximum, CancellationToken cancellationToken) : TextWriter
{
    private readonly StringBuilder output = new(Math.Min(maximum, 1024));
    private bool failed;
    /// <inheritdoc/>
    public override Encoding Encoding => Encoding.UTF8;
    private void Reserve(int count)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (failed || count > maximum - output.Length)
        {
            failed = true;
            throw new NotSupportedException("Serialized XML exceeds the configured output capacity.");
        }
    }
    /// <inheritdoc/>
    public override void Write(char value) { Reserve(1); output.Append(value); }
    /// <inheritdoc/>
    public override void Write(string? value) { Reserve(value?.Length ?? 0); output.Append(value); }
    /// <inheritdoc/>
    public override void Write(char[] buffer, int index, int count) => Write(buffer.AsSpan(index, count));
    /// <inheritdoc/>
    public override void Write(ReadOnlySpan<char> buffer) { Reserve(buffer.Length); output.Append(buffer); }
    internal string GetText() { Reserve(0); return output.ToString(); }
    internal static string Export(NativeSchemaInput input, int maximum, CancellationToken cancellationToken)
    {
        using var output = new NativeBoundedTextWriter(maximum, cancellationToken);
        using (var writer = XmlWriter.Create(output, new XmlWriterSettings
        {
            OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment,
            Indent = false, NewLineHandling = NewLineHandling.Entitize, CloseOutput = false
        }))
        {
            input.WriteRootTo(writer, cancellationToken);
            writer.Flush();
        }
        return output.GetText();
    }
}
