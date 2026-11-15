// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Verifies the owned native decimal facade returns detached results and cleans admissions.
// SPECIAL NOTES        : Internal Core 1.81.2 adapter; finite profile only, no public native factory.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 15-11-2026     | Qualify the internal synchronous facade prototype                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;
using Bosak.XPath.Api.Schema;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Verifies the owned native decimal facade returns detached results and cleans admissions.</summary>
public sealed class NativeDecimalSessionTests
{
    private static readonly DateTimeOffset Date = new(2026, 11, 15, 12, 0, 0, TimeSpan.Zero);
    private static string Schema => File.ReadAllText(Data("native-decimal.xsd"));
    private static string Instance => File.ReadAllText(Data("native-decimal.xml"));
    private static SchemaQueryLimits Limits => new(1, 4, 1, 256, 256);

    /// <summary>Checks the full owned native workflow exports detached exact values and XML.</summary>
    [Fact]
    public void CreateAssessRead_GoldenDocument_ReturnsDetachedValuesAndOwnedXml()
    {
        var session = Create();
        var result = session.Assess(Instance, Date, 60);
        Assert.Equal(NativeOutcome.Valid, result.Outcome);
        var document = result.Value!;
        Assert.Equal(3, document.DecimalCount);
        var price = document.ReadDecimal(0);
        Assert.Equal("12.5", price.ToInvariantString(256));
        Assert.Equal("2", document.ReadDecimal(1).ToInvariantString(256));
        Assert.Equal("1", document.ReadDecimal(2).ToInvariantString(256));
        var xml = XDocument.Parse(document.ExportXml());
        Assert.True(XNode.DeepEquals(XDocument.Load(Data("native-decimal.expected.xml")).Root, xml.Root));
        xml.Root!.RemoveAll();
        Assert.Equal(3, XDocument.Parse(document.ExportXml()).Root!.Elements().Count());
        document.Dispose();
        session.Dispose();
        Assert.Equal("12.5", price.ToInvariantString(256));
    }

    /// <summary>Checks nonvalid assessments create no document or lasting quota admission.</summary>
    [Fact]
    public void Assess_InvalidAndUnsupportedInputs_ReturnNoDocumentAndKeepSessionUsable()
    {
        using var session = Create();
        var invalid = session.Assess("<order><tax>1</tax></order>", Date, 60);
        Assert.Equal(NativeOutcome.Invalid, invalid.Outcome);
        Assert.Null(invalid.Value);
        var unsupported = session.Assess("<order flag='x'/>", Date, 60);
        Assert.Equal(NativeOutcome.Unsupported, unsupported.Outcome);
        Assert.Null(unsupported.Value);
        var malformed = session.Assess("<order", Date, 60);
        Assert.Equal(NativeOutcome.Invalid, malformed.Outcome);
        Assert.Null(malformed.Value);
        using var valid = session.Assess(Instance, Date, 60).Value!;
        Assert.Equal("1", valid.ReadDecimal(2).ToInvariantString(256));
    }

    /// <summary>Checks document close frees quota and session close revokes every remaining document.</summary>
    [Fact]
    public void Dispose_DocumentsAndSession_ReleaseAdmissionsAndRejectFurtherUse()
    {
        var session = Create();
        var first = session.Assess(Instance, Date, 60).Value!;
        Assert.Throws<InvalidOperationException>(() => session.Assess(Instance, Date, 60));
        first.Dispose();
        first.Dispose();
        Assert.Throws<ObjectDisposedException>(() => first.ExportXml());
        var second = session.Assess(Instance, Date, 60).Value!;
        var retainedOwner = second.Input;
        session.Dispose();
        session.Dispose();
        Assert.True(retainedOwner.ResourcesReleased);
        Assert.Throws<ObjectDisposedException>(() => second.ReadDecimal(0));
        Assert.Throws<ObjectDisposedException>(() => session.Assess(Instance, Date, 60));
        second.Dispose();
    }

    /// <summary>Checks indexing and cancellation failures cannot strand the shared facade gate.</summary>
    [Fact]
    public void ReadDecimal_InvalidIndexOrCancellation_LeavesDocumentUsable()
    {
        using var session = Create();
        using var document = session.Assess(Instance, Date, 60).Value!;
        Assert.Throws<ArgumentOutOfRangeException>(() => document.ReadDecimal(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => document.ReadDecimal(document.DecimalCount));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Throws<OperationCanceledException>(() => document.ReadDecimal(0, cancellation.Token));
        Assert.Equal("1", document.ReadDecimal(2).ToInvariantString(256));
    }

    /// <summary>Checks query admission failure preserves the document and permits a supported later read.</summary>
    [Fact]
    public void ReadDecimal_QueryCapacityExceeded_RefusesWithoutCoercionOrGateLeak()
    {
        using var session = Create(new SchemaQueryLimits(1, 4, 1, 1, 0));
        using var document = session.Assess(Instance, Date, 60).Value!;
        Assert.Throws<NotSupportedException>(() => document.ReadDecimal(0));
        Assert.Equal("1", document.ReadDecimal(2).ToInvariantString(256));
    }

    /// <summary>Checks unsuccessful compilation and invalid explicit capacities never return a session.</summary>
    [Fact]
    public void Create_InvalidSchemaOrCapacities_RefusesOrThrowsWithoutSession()
    {
        var malformed = NativeDecimalSession.Create("<xs:schema", new(256, 256), Limits);
        Assert.Equal(NativeOutcome.Invalid, malformed.Outcome);
        Assert.Null(malformed.Value);
        var unsupported = NativeDecimalSession.Create(Schema.Replace("xs:decimal", "xs:string", StringComparison.Ordinal), new(256, 256), Limits);
        Assert.Equal(NativeOutcome.Unsupported, unsupported.Outcome);
        Assert.Null(unsupported.Value);
        Assert.Throws<ArgumentOutOfRangeException>(() => NativeDecimalSession.Create(Schema, default, Limits));
        Assert.Throws<ArgumentException>(() => NativeDecimalSession.Create(Schema, new(256, 256), default));
    }

    /// <summary>Checks malformed assessment context releases input ownership before a subsequent valid capture.</summary>
    [Fact]
    public void Assess_InvalidTimezone_ThrowsAndKeepsAdmissionAvailable()
    {
        using var session = Create();
        Assert.Throws<ArgumentException>(() => session.Assess(Instance, Date, 841));
        using var document = session.Assess(Instance, Date, 60).Value!;
        Assert.Equal(3, document.DecimalCount);
    }

    /// <summary>Checks an all-optional empty root remains valid without a fabricated scalar.</summary>
    [Fact]
    public void Assess_AllOptionalAbsent_ReturnsEmptyDecimalViewWithoutSyntheticValue()
    {
        const string schema = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='order'><xs:complexType><xs:sequence><xs:element name='amount' type='xs:decimal' minOccurs='0'/></xs:sequence><xs:assert test='empty(amount)'/></xs:complexType></xs:element></xs:schema>";
        using var session = NativeDecimalSession.Create(schema, new(256, 256), Limits).Value!;
        using var document = session.Assess("<order/>", Date, 60).Value!;
        Assert.Equal(0, document.DecimalCount);
        Assert.Throws<ArgumentOutOfRangeException>(() => document.ReadDecimal(0));
        Assert.Empty(XDocument.Parse(document.ExportXml()).Root!.Elements());
    }

    private static NativeDecimalSession Create(SchemaQueryLimits? limits = null)
    {
        var result = NativeDecimalSession.Create(Schema, new(256, 256), limits ?? Limits);
        Assert.True(result.Outcome == NativeOutcome.Valid, result.Diagnostic);
        return result.Value!;
    }
    private static string Data(string file) => Path.Combine(AppContext.BaseDirectory, "TestData", "Schemas", file);
}
