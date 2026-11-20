// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Records actual native behavior before anonymous simple restriction implementation.
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
using System.Text.Json;
using Xunit;
namespace IntegerBeforeProbe;
/// <summary>Records actual unchanged native integer restriction refusals and existing support.</summary>
public sealed class BeforeTests
{
 private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,1,32,1,256,256);
 /// <summary>Original adopted rows currently refuse compilation.</summary>
 /// <param name="index">Original case row.</param>
 [Theory][InlineData(0)][InlineData(1)][InlineData(2)]
 public void OriginalRows_Refuse(int index){using var doc=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"original-rows.json")));var result=NativeSchemaSession.Create(doc.RootElement[index].GetProperty("schema").GetString()!,Options);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Equal(NativeSchemaUnsupportedReason.ValueType,result.Diagnostics[0].UnsupportedReason);Assert.Null(result.Value);}
 /// <summary>Primitive integer and named decimal/string restrictions remain admitted.</summary>
 /// <param name="family">Existing datatype family.</param>
 [Theory][InlineData("integer")][InlineData("decimal")][InlineData("string")]
 public void ExistingFamilies_Work(string family){var global=family=="integer"?"":$"<xs:simpleType name='T'><xs:restriction base='xs:{family}'/></xs:simpleType>";var type=family=="integer"?"xs:integer":"T";var schema=$"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>{global}<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='{type}'/></xs:sequence></xs:complexType></xs:element></xs:schema>";using var session=NativeSchemaSession.Create(schema,Options).Value!;var result=session.Assess("<r><v>12345678901234567890123456789012345678901234567890</v></r>",new(DateTimeOffset.UnixEpoch,0));Assert.Equal(NativeSchemaOutcome.Valid,result.Outcome);using var document=result.Value!;Assert.Equal(1,document.ScalarCount);}
}
