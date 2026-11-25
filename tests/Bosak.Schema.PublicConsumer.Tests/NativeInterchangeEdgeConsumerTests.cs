// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Tests bounded binary and namespace-sensitive QName parsing.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
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
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;

/// <summary>Checks imported namespace literals, finite facets and original owners.</summary>
public sealed class NativeInterchangeEdgeConsumerTests
{
    private static NativeSchemaSessionOptions Options => new(256,256,256,65536,1,256,1,256,256);
    private static NativeSchemaAssessmentContext Context => new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Checks facet namespace capture across an actual closed import.</summary>
    /// <param name="data">Original-node facade selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void ImportedQName(bool data)
    {
        var catalog = new Dictionary<string,string>
        {
            ["https://example.test/root.xsd"] = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='urn:types' xmlns:p='urn:wrong'><xs:import namespace='urn:types' schemaLocation='types.xsd'/><xs:element name='r' type='t:N'/></xs:schema>",
            ["https://example.test/types.xsd"] = "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:p='urn:correct' targetNamespace='urn:types'><xs:simpleType name='N'><xs:restriction base='xs:QName'><xs:enumeration value='p:item'/></xs:restriction></xs:simpleType></xs:schema>"
        };
        const string good = "<r xmlns:q='urn:correct'>q:item</r>", bad = "<r xmlns:q='urn:wrong'>q:item</r>";
        if(data)
        {
            var compiled=NativeSchemaNodeDataSession.Create("https://example.test/root.xsd",catalog,Options);Assert.Equal(NativeSchemaOutcome.Valid,compiled.Outcome);
            using var session=compiled.Value!;Assert.Equal(NativeSchemaOutcome.Invalid,session.Assess(bad,Context).Outcome);
            using var document=session.Assess(good,Context).Value!;Assert.Equal("urn:correct",document.ReadElement([]).QNameValue.NamespaceUri);
        }
        else
        {
            var compiled=NativeSchemaSession.Create("https://example.test/root.xsd",catalog,Options);Assert.Equal(NativeSchemaOutcome.Valid,compiled.Outcome);
            using var session=compiled.Value!;Assert.Equal(NativeSchemaOutcome.Invalid,session.Assess(bad,Context).Outcome);
            using var document=session.Assess(good,Context).Value!;Assert.Equal("urn:correct",document.ReadRootScalar().QNameValue.NamespaceUri);
        }
    }

    /// <summary>Checks original restriction context within complex simple content.</summary>
    /// <param name="data">Original-node facade selector.</param>
    /// <param name="kind">Interchange type.</param><param name="facet">Enumeration literal.</param><param name="xml">Original input.</param>
    [Theory]
    [InlineData(false,"QName","p:item","<r xmlns:q='urn:x'>q:item</r>")]
    [InlineData(true,"QName","p:item","<r xmlns:q='urn:x'>q:item</r>")]
    [InlineData(false,"hexBinary","61","<r>61</r>")]
    [InlineData(true,"base64Binary","YQ==","<r>Y Q = =</r>")]
    public void ComplexRestriction(bool data,string kind,string facet,string xml)
    {
        var schema=$"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:p='urn:x'><xs:complexType name='B'><xs:simpleContent><xs:extension base='xs:{kind}'/></xs:simpleContent></xs:complexType><xs:complexType name='D'><xs:simpleContent><xs:restriction base='B'><xs:enumeration value='{facet}'/></xs:restriction></xs:simpleContent></xs:complexType><xs:element name='r' type='D'/></xs:schema>";
        if(data){var c=NativeSchemaNodeDataSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.Contains(d.ReadElement([]).Kind,new[]{NativeScalarKind.QName,NativeScalarKind.HexBinary,NativeScalarKind.Base64Binary});}
        else{var c=NativeSchemaSession.Create(schema,Options);Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var a=s.Assess(xml,Context);Assert.Equal(NativeSchemaOutcome.Valid,a.Outcome);using var d=a.Value!;Assert.Contains(d.ReadRootScalar().Kind,new[]{NativeScalarKind.QName,NativeScalarKind.HexBinary,NativeScalarKind.Base64Binary});}
    }
}
