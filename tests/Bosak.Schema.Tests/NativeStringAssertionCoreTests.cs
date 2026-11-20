// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks original native facts and no-publication failures after string assertions.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Xml.Linq;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Api;
using Bosak.XPath.Providers.Xml;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks original facts and compatibility under the admitted string assertion profile.</summary>
public sealed class NativeStringAssertionCoreTests
{
    private static string Schema(string mode)
    {
        var named=mode=="named"?"<xs:simpleType name='Label'><xs:restriction base='xs:string'/></xs:simpleType>":"";
        var child=mode=="anonymous"?"<xs:element name='v'><xs:simpleType><xs:restriction base='xs:string'/></xs:simpleType></xs:element>":"<xs:element name='v' type='"+(mode=="named"?"Label":"xs:string")+"'/>";
        return "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+named+"<xs:element name='r'><xs:complexType><xs:sequence>"+child+"</xs:sequence><xs:assert test=\"v eq 'A'\"/></xs:complexType></xs:element></xs:schema>";
    }
    /// <summary>Successful assertions retain actual original named or anonymous type/provenance identity.</summary>
    /// <param name="mode">Primitive, named or anonymous string declaration.</param>
    [Theory][InlineData("primitive")][InlineData("named")][InlineData("anonymous")]
    public void ValidAssertion_RetainsOriginalNativeFacts(string mode)
    {
        using var f=new Fixture(Schema(mode),"<r><v>A</v></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=f.Element([0]);var facts=f.Input.FactsFor((NativeSessionNode)node)!;
        Assert.Equal("A",f.Read(node).StringValue);if(mode=="anonymous")Assert.Null(node.SchemaTypeAnnotation);
        f.Run(operation=>{var value=f.Evaluate(operation,node);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(f.Assessment.Origin,evidence!.OriginalAssessment);Assert.Same(facts.Type,evidence.DeclaredType);Assert.Same(facts.Type,evidence.ActualType);return 0;});
        Assert.True(f.Session.Advisor!.OwnsNode(node,facts,f.Input,f.Assessment.Origin));
    }
    /// <summary>False assertions cannot publish typed facts or admit a snapshot.</summary>
    /// <param name="mode">Primitive, named or anonymous declaration.</param>
    [Theory][InlineData("primitive")][InlineData("named")][InlineData("anonymous")]
    public void InvalidAssertion_PublishesNoNativeFacts(string mode)
    {
        using var f=new Fixture(Schema(mode),"<r><v>B</v></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);Assert.False(f.Input.IsAssessed);Assert.Null(f.Snapshot);Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Element([0])));
    }
    /// <summary>The decimal-only facade keeps its previous literal and comparison grammar.</summary>
    /// <param name="test">New mixed-profile string grammar.</param>
    [Theory][InlineData("'A' eq 'A'")][InlineData("'A' = 'A'")]
    public void DecimalOnlyFacade_DoesNotAdoptStringGrammar(string test)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='n' type='xs:decimal'/></xs:sequence><xs:assert test=\""+test+"\"/></xs:complexType></xs:element></xs:schema>";
        var legacy=NativeDecimalSession.Create(schema,new(256,256,1,32,1,256,256));Assert.Equal(NativeDecimalOutcome.Unsupported,legacy.Outcome);Assert.Null(legacy.Value);
        using var mixed=NativeSchemaSession.Create(schema,new(256,256,1024,65536,1,32,1,256,256)).Value!;using var doc=mixed.Assess("<r><n>1</n></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.Equal("1",doc.ReadScalar(0).DecimalValue.ToInvariantString(256));
    }
    /// <summary>Published Bosak and native assessment agree on the admitted pure string comparison subset.</summary>
    /// <param name="expression">Pure string comparison or count/presence composition.</param>
    /// <param name="content">Original child XML.</param>
    /// <param name="expected">Expected effective Boolean value.</param>
    [Theory]
    [InlineData("v = 'A'","<v>A</v><v>B</v>",true)]
    [InlineData("v != 'A'","<v>A</v><v>B</v>",true)]
    [InlineData("v eq 'A'","<v>A</v>",true)]
    [InlineData("v ne 'A'","<v>B</v>",true)]
    [InlineData("v ne 'A'","",false)]
    [InlineData("v != 'A'","",false)]
    [InlineData("v eq ''","<v/>",true)]
    [InlineData("v eq 'can''t'","<v>can't</v>",true)]
    [InlineData("exists(v) and count(v) eq 2","<v>A</v><v>B</v>",true)]
    public void PublishedCoreAndNative_StringSubsetAgree(string expression,string content,bool expected)
    {
        var xml="<r>"+content+"</r>";
        var core=XPath31Expression.Compile(expression).Evaluate(new XDocumentNode(XDocument.Parse(xml).Root!)).GetEffectiveBooleanValue();
        Assert.Equal(expected,core);
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:string' minOccurs='0' maxOccurs='2'/></xs:sequence><xs:assert test=\""+System.Security.SecurityElement.Escape(expression)+"\"/></xs:complexType></xs:element></xs:schema>";
        var compiled=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Valid,compiled.Outcome);
        Assert.Equal(expected?NativeOutcome.Valid:NativeOutcome.Invalid,compiled.Value!.Assess(xml).Outcome);
    }

}
