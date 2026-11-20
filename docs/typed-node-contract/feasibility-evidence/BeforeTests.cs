// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Records native scalar behavior and node-data refusal before the isolated candidate.
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

using Bosak.Schema;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace NativeDataBefore;
/// <summary>Verifies actual native behavior on unchanged production source.</summary>
public sealed class BeforeTests
{
 private static string Schema(string family)=>$"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:{family}'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
 /// <summary>Original native provider does not bind the node-data environment.</summary>
 /// <param name="expression">Node-data or unavailable navigation expression.</param>
 [Theory][InlineData("data($node)")][InlineData("fn:data($node)")][InlineData("data($node/v)")]
 public void NativeNodeData_RemainsUnsupported(string expression){var provider=new NativeSchemaProvider(NativeDecimalSchema.Compile(Schema("decimal"),new(256,256),maxStringCharacters:1024).Value!);var result=SchemaQuerySession.CompileXPath(expression,new(2,16,1,256,256),assessor:provider);Assert.Equal(SchemaResultKind.Unsupported,result.Kind);}
 /// <summary>The owned scalar route still supports each current family.</summary>
 /// <param name="family">Existing primitive family.</param>
 [Theory][InlineData("string")][InlineData("decimal")][InlineData("integer")][InlineData("boolean")]
 public void NativeScalars_StillWork(string family){using var session=NativeSchemaSession.Create(Schema(family),new(256,256,1024,65536,1,16,1,256,256)).Value!;using var document=session.Assess("<r><v>0</v></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.Equal(1,document.ScalarCount);Assert.NotNull(document.ReadScalar(0));}
 /// <summary>The legacy decimal facade still refuses integer schemas.</summary>
 [Fact]
 public void LegacyInteger_RemainsUnsupported(){Assert.Equal(NativeDecimalOutcome.Unsupported,NativeDecimalSession.Create(Schema("integer"),new(256,256,1,16,1,256,256)).Outcome);}
}
