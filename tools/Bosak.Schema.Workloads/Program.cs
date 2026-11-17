// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 17 November 2026
// PURPOSE              : Measures bounded public Schema workflows with explicit correctness and recovery checks.
// SPECIAL NOTES        : Diagnostic workload tool; local measurements do not establish beta or performance acceptance.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 17-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.XPath.Providers.Xml;

internal static class Program
{
    private static readonly NativeDecimalAssessmentContext Context = new(new DateTimeOffset(2026, 11, 17, 12, 0, 0, TimeSpan.Zero), 60);
    private const string SmallSchema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='order'><xs:complexType><xs:sequence><xs:element name='amount' type='xs:decimal'/></xs:sequence><xs:assert test='amount ge 0'/></xs:complexType></xs:element></xs:schema>";
    private const string SmallInstance = "<order><amount>12.50</amount></order>";

    private static int Main(string[] args)
    {
        try { return Run(args); }
        catch (Exception error)
        {
            Console.Error.WriteLine(JsonSerializer.Serialize(new { success = false, exceptionType = error.GetType().FullName, message = "Workload diagnostic failed; no unhandled exception was launched." }));
            return 1;
        }
    }

    private static int Run(string[] args)
    {
        if (args.Length != 3 || !int.TryParse(args[1], out var iterations) || iterations is < 1 or > 1000 ||
            !int.TryParse(args[2], out var warmup) || warmup is < 0 or > 20) return 2;
        var names = new[] { "native-small", "native-max-bytes", "legacy-facets", "restricted-xslt", "refusal-recovery" };
        if (!names.Contains(args[0], StringComparer.Ordinal)) return 2;
        var directory = Path.Combine(Path.GetTempPath(), "bosak-workload-" + Guid.NewGuid().ToString("N"));
        var fixture = Path.Combine(directory, "schema.xsd");
        Directory.CreateDirectory(directory);
        try
        {
            File.WriteAllText(fixture, "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:workload'><xs:element name='value' type='xs:decimal'/></xs:schema>");
            var maximumSchema = MaximumSchema();
            var maximumInstance = MaximumInstance();
            Action action = args[0] switch
            {
                "native-small" => () => Native(SmallSchema, SmallInstance, 1, _ => "12.5"),
                "native-max-bytes" => () => Native(maximumSchema, maximumInstance, 16, i => i % 2 == 0 ? new string('9', 256) : "0." + new string('1', 255)),
                "legacy-facets" => Legacy,
                "restricted-xslt" => () => RestrictedXslt(directory),
                _ => Recovery,
            };
            for (var i = 0; i < warmup; i++) action();
            var times = new List<double>();
            var allocations = new List<long>();
            for (var i = 0; i < iterations; i++)
            {
                var allocated = GC.GetAllocatedBytesForCurrentThread();
                var started = Stopwatch.GetTimestamp();
                action();
                times.Add(Stopwatch.GetElapsedTime(started).TotalMilliseconds);
                allocations.Add(GC.GetAllocatedBytesForCurrentThread() - allocated);
            }
            using var process = Process.GetCurrentProcess();
            process.Refresh();
            var assembly = typeof(NativeDecimalSession).Assembly.Location;
            Console.WriteLine(JsonSerializer.Serialize(new
            {
                success = true,
                workload = args[0], iterations, warmup,
                elapsedMilliseconds = times,
                callingThreadAllocatedBytes = allocations,
                peakWorkingSetBytes = process.PeakWorkingSet64,
                runtime = RuntimeInformation.FrameworkDescription,
                os = RuntimeInformation.OSDescription,
                architecture = RuntimeInformation.ProcessArchitecture.ToString(),
                processors = Environment.ProcessorCount,
                serverGc = GCSettings.IsServerGC,
                schemaAssemblySha256 = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(assembly))).ToLowerInvariant(),
                schemaBytes = args[0] == "native-max-bytes" ? Encoding.UTF8.GetByteCount(maximumSchema) : (int?)null,
                instanceBytes = args[0] == "native-max-bytes" ? Encoding.UTF8.GetByteCount(maximumInstance) : (int?)null,
                assertionsChecked = true,
                cancellationMeasurement = args[0] == "refusal-recovery" ? "Pre-cancelled calls only; no mid-work latency claim" : null,
            }));
            return 0;
        }
        finally
        {
            File.Delete(fixture);
            Directory.Delete(directory, false);
        }
    }

    private static NativeDecimalSessionOptions Options() => new(256, 256, 1, 16, 1, 256, 256);
    private static void Check(bool condition)
    {
        if (!condition) throw new InvalidOperationException("A workload correctness check failed.");
    }

    private static void Native(string schema, string instance, int count, Func<int, string> expected)
    {
        var created = NativeDecimalSession.Create(schema, Options());
        Check(created.Outcome == NativeDecimalOutcome.Valid && created.Value is not null);
        using var session = created.Value!;
        var assessed = session.Assess(instance, Context);
        Check(assessed.Outcome == NativeDecimalOutcome.Valid && assessed.Value is not null);
        using var document = assessed.Value!;
        Check(document.DecimalCount == count);
        for (var i = 0; i < count; i++) Check(document.ReadDecimal(i).ToInvariantString(512) == expected(i));
        var exported = document.ExportXml();
        Check(XDocument.Parse(exported).Root!.Elements().Count() == count);
    }

    private static string MaximumSchema()
    {
        var leaves = string.Concat(Enumerable.Range(0, 16).Select(i => $"<xs:element name='v{i}' type='xs:decimal'/>"));
        var assertions = string.Concat(Enumerable.Range(0, 8).Select(i => $"<xs:assert test='v{i} eq v{i}'/>"));
        var schema = $"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='order'><xs:complexType><xs:sequence>{leaves}</xs:sequence>{assertions}</xs:complexType></xs:element></xs:schema>";
        return new string(' ', 32768 - Encoding.UTF8.GetByteCount(schema)) + schema;
    }

    private static string MaximumInstance()
    {
        var instance = "<order>" + string.Concat(Enumerable.Range(0, 16).Select(i => $"<v{i}>{(i % 2 == 0 ? new string('9', 256) : "." + new string('1', 255))}</v{i}>")) + "</order>";
        return new string(' ', 65536 - Encoding.UTF8.GetByteCount(instance)) + instance;
    }

    private static void Legacy()
    {
        const string xml = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='Positive'><xs:restriction base='xs:decimal'><xs:assertion test='$value gt 0'/></xs:restriction></xs:simpleType></xs:schema>";
        using var input = new MemoryStream(Encoding.UTF8.GetBytes(xml));
        var model = SchemaServices.CreateSchemaCompiler().Compile(input, new Uri("file:///workload.xsd"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
        Check(model.Diagnostics.Count == 0);
        var service = SchemaServices.CreateValidationService();
        Check(service.ValidateValue(model, XName.Get("Positive"), "2").IsValid);
        Check(!service.ValidateValue(model, XName.Get("Positive"), "-2").IsValid);
        using var invalid = new MemoryStream(Encoding.UTF8.GetBytes("<not-schema/>"));
        var bad = SchemaServices.CreateSchemaCompiler().Compile(invalid, new Uri("file:///workload.xsd"), SchemaCompileOptions.Default(XsdLanguageVersion.Xsd11));
        Check(!service.Validate(bad, new XElement("x")).IsValid);
    }

    private static void RestrictedXslt(string directory)
    {
        var compiler = SchemaAwareXslt.CreateRestrictedCompiler(directory, 4096);
        const string stylesheet = "<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='3.0'><xsl:import-schema namespace='urn:workload' schema-location='schema.xsd'/><xsl:template name='main'><out>ok</out></xsl:template></xsl:stylesheet>";
        var executable = compiler.Compile(stylesheet, new Uri(Path.Combine(directory, "main.xsl")).AbsoluteUri);
        var result = executable.TransformToString(new XDocumentNode(XDocument.Parse("<root/>")), initialTemplate: "main");
        Check(result.Contains("<out>ok</out>", StringComparison.Ordinal));
        Check(compiler.ResourcePolicy!.Receipts.Count > 0);
    }

    private static void Expect<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException("An expected workload refusal was not observed.");
    }

    private static void Recovery()
    {
        using var cancelled = new CancellationTokenSource();
        cancelled.Cancel();
        Expect<OperationCanceledException>(() => NativeDecimalSession.Create(SmallSchema, Options(), cancelled.Token));
        using var session = NativeDecimalSession.Create(SmallSchema, Options()).Value!;
        Expect<OperationCanceledException>(() => session.Assess(SmallInstance, Context, cancelled.Token));
        Check(session.Assess("<order><amount>bad</amount></order>", Context).Outcome == NativeDecimalOutcome.Invalid);
        Check(session.Assess("<order flag='unsupported'><amount>1</amount></order>", Context).Outcome == NativeDecimalOutcome.Unsupported);
        Check(session.Assess(new string(' ', 65537), Context).Outcome == NativeDecimalOutcome.Unsupported);
        var document = session.Assess(SmallInstance, Context).Value!;
        try
        {
            Expect<InvalidOperationException>(() => session.Assess(SmallInstance, Context));
            Expect<OperationCanceledException>(() => document.ReadDecimal(0, cancelled.Token));
            Expect<OperationCanceledException>(() => document.ExportXml(cancelled.Token));
            Check(document.ReadDecimal(0).ToInvariantString(32) == "12.5");
        }
        finally { document.Dispose(); }
        document.Dispose();
        Expect<ObjectDisposedException>(() => document.ReadDecimal(0));
        using var recovered = session.Assess(SmallInstance, Context).Value!;
        Check(recovered.DecimalCount == 1);
    }
}
