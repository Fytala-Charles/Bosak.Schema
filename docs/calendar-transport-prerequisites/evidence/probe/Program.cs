// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Measures current signed calendar transport without changing production semantics.
// SPECIAL NOTES        : Handled package-only prerequisite diagnostic; no semantic admission.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Security.Cryptography;
using System.Text.Json;
using Bosak.Schema;
using Bosak.XPath.Core.Xdm;

internal static class Program
{
    private static int Main(string[] args)
    {
        try
        {
            var rows = new List<object>();
            var zones = new[] { (false, 0), (true, 0), (true, 840), (true, -840) };
            var errors = new List<string>();
            void Check(bool ok, string label) { if (!ok) errors.Add(label); }
            foreach (var family in new[] { "dateTime", "time" })
            {
                foreach (var (present, offset) in zones)
                {
                    var raw = new XPathDateTime(2000, 1, 2, 3, 4, 5, 123, offset, present);
                    var value = family == "time" ? XdmValue.FromTime(raw, present) : XdmValue.FromDateTime(raw, present);
                    var read = family == "time" ? value.TimeXPathValue : value.DateTimeXPathValue;
                    bool ok = read.Year == raw.Year && read.Month == raw.Month && read.Day == raw.Day && read.Hour == raw.Hour && read.Minute == raw.Minute && read.Second == raw.Second && read.Millisecond == 123 && read.HasTimezone == present && read.TimezoneOffsetMinutes == offset;
                    Check(ok, "component " + family);
                    rows.Add(new { kind = "component", family, present, offset, passed = ok });
                    foreach (long ticks in new long[] { 1230000, 1234567, 1234568, 1239999 })
                    {
                        var input = new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.FromMinutes(offset)).AddTicks(ticks);
                        var v = family == "time" ? XdmValue.FromTime(input, present) : XdmValue.FromDateTime(input, present);
                        var dto = family == "time" ? v.TimeValue : v.DateTimeValue;
                        var x = family == "time" ? v.TimeXPathValue : v.DateTimeXPathValue;
                        long output = dto.Ticks % TimeSpan.TicksPerSecond;
                        bool expected = output == ticks / 10000 * 10000 && x.Millisecond == 123 && x.HasTimezone == present && x.TimezoneOffsetMinutes == offset;
                        Check(expected, "fraction " + family);
                        rows.Add(new { kind = "fraction", family, present, offset, inputTicks = ticks, outputTicks = output, exact = ticks == output, passed = expected });
                    }
                }
                long Read(long ticks)
                {
                    var dto = new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.Zero).AddTicks(ticks);
                    var v = family == "time" ? XdmValue.FromTime(dto, true) : XdmValue.FromDateTime(dto, true);
                    return (family == "time" ? v.TimeValue : v.DateTimeValue).Ticks % TimeSpan.TicksPerSecond;
                }
                bool collision = Read(1234567) == Read(1234568);
                Check(collision, "collision " + family);
                rows.Add(new { kind = "collision", family, distinctInputTicks = new[] { 1234567, 1234568 }, collapsed = collision, passed = collision });
            }
            foreach (var type in new[] { "dateTime", "time", "dateTimeStamp" })
            {
                string schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:" + type + "'/></xs:schema>";
                var options = new NativeSchemaSessionOptions(128, 64, 1024, 8192, 4, 32, 32, 128, 64);
                var scalar = NativeSchemaSession.Create(schema, options);
                var node = NativeSchemaNodeDataSession.Create(schema, options);
                foreach (var (facade, outcome, noValue) in new[] { ("scalar", scalar.Outcome.ToString(), scalar.Value is null), ("node-data", node.Outcome.ToString(), node.Value is null) })
                {
                    bool refused = outcome == "Unsupported" && noValue;
                    Check(refused, facade + type);
                    rows.Add(new { kind = "native-refusal", facade, type, outcome, noValue, assessed = false, passed = refused });
                }
            }
            var assembly = typeof(XdmValue).Assembly;
            var report = new { observations = rows.Count, rows, errors, coreVersion = assembly.GetName().Version?.ToString(), coreDllSha256 = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(assembly.Location))), schemaDllSha256 = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(typeof(NativeSchemaSession).Assembly.Location))) };
            File.WriteAllText(args[0], JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine($"{rows.Count} handled observations; {errors.Count} unexpected results.");
            return errors.Count == 0 && rows.Count == 48 ? 0 : 1;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Caught diagnostic failure: " + ex);
            return 1;
        }
    }
}
