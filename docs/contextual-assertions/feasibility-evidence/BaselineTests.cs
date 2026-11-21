// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Verifies isolated complex restriction identities, facets and original ownership.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 21-11-2026     | Assert original integer extension refusal before copied-source changes |
//                      | Charles Korthout | 0.3   | 21-11-2026     | Verify isolated contextual assertions and retained native baseline |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;

namespace ComplexProbe;

/// <summary>Authenticates unchanged baseline success and feature refusal.</summary>
public sealed class BaselineTests
{
    private static string Schema(string ns)=>$"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='{ns}' targetNamespace='{ns}'><xs:complexType name='B'><xs:simpleContent><xs:extension base='xs:decimal'/></xs:simpleContent></xs:complexType><xs:element name='r' type='t:B'/></xs:schema>";
    /// <summary>Original simple content continues to use its own actual primitive evidence.</summary>
    /// <param name="ns">Exact namespace.</param>
    [Theory][InlineData("urn:baseline")][InlineData("rel/baseline")]
    public void OriginalExtension_IsNativeAndOwned(string ns)
    {using var f=new Fixture(Schema(ns),$"<t:r xmlns:t='{ns}'>12.5</t:r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=f.Element([]);Assert.Equal("12.5",f.Read(node).DecimalValue.ToInvariantString(256));var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.Same(fact.NativeDeclaration.Type,fact.NativeType);Assert.True(f.Session.Advisor!.OwnsNode(node,fact,f.Input,f.Assessment.Origin));}
    /// <summary>Unchanged source refuses the prospective restriction grammar.</summary>
    [Fact]
    public void Restriction_RemainsUnsupported()
    {var schema=Schema("urn:baseline").Replace("xs:decimal","xs:integer",StringComparison.Ordinal).Replace("<xs:extension base='xs:integer'/>","<xs:extension base='xs:integer'><xs:assert test='$value eq 4'/></xs:extension>",StringComparison.Ordinal);var r=NativeSchemaSession.Create(schema,new(256,256,1024,65536,1,32,1,256,256));Assert.Equal(NativeSchemaOutcome.Unsupported,r.Outcome);Assert.Null(r.Value);}
}
