// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 23 November 2026
// PURPOSE              : Verifies isolated all outcomes, physical original facts and omission semantics.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 23-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 23-11-2026     | Verify absent root typed-value diagnostic and caught FOTY0012 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.NodeData;
/// <summary>Checks real public all routes and actual original node evidence.</summary>
public sealed class NativeAllIdentityTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,1,256,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    /// <summary>Gets only all-phase payloads through both facades.</summary><returns>Planned controls.</returns>
    public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData","AllParticles","case-plan.json")));foreach(var r in d.RootElement.GetProperty("cases").EnumerateArray().Where(x=>x.GetProperty("phase").GetString()=="all-second"))foreach(bool data in new[]{false,true})yield return[r.GetProperty("id").GetString()!,r.GetRawText(),data];}
    private static string Text(NativeScalarValue v)=>v.Kind switch{NativeScalarKind.String=>v.StringValue,NativeScalarKind.Integer=>v.IntegerValue.ToString(CultureInfo.InvariantCulture),NativeScalarKind.Boolean=>v.BooleanValue?"true":"false",_=>v.DecimalValue.ToInvariantString(256)};
    /// <summary>Gets valid authored all cases for actual native identity checks.</summary><returns>Explicit valid controls.</returns>
    public static IEnumerable<object[]> Identities(){foreach(var x in Cases()){using var d=JsonDocument.Parse((string)x[1]);var r=d.RootElement;if(!(bool)x[2]&&r.GetProperty("expectedCompile").GetString()=="Valid"&&r.GetProperty("expectedAssessment").GetString()=="Valid")yield return x;}}
    /// <summary>Checks facts are attached in physical order with the selected declarations and types.</summary>
    /// <param name="id">Case identity.</param><param name="json">Original payload.</param><param name="data">Asserted false.</param>
    [Theory][MemberData(nameof(Identities))]
    public void PhysicalOriginalIdentity(string id,string json,bool data)
    {
        Assert.True(id.StartsWith("PT-",StringComparison.Ordinal)||id.StartsWith("AL-",StringComparison.Ordinal));Assert.False(data);using var d=JsonDocument.Parse(json);var r=d.RootElement;using var f=new Fixture(r.GetProperty("schema").GetString()!,r.GetProperty("instance").GetString()!);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var rootFact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.True(rootFact.HasNoTypedValue);Assert.False(rootFact.NativeType.EmptyContent);Assert.Equal(SchemaResultKind.Unsupported,rootFact.ReadTypedValue(default).Kind);Assert.Equal("FOTY0012",rootFact.ReadTypedValue(default).DiagnosticCode);
        var expected=XElement.Parse(r.GetProperty("instance").GetString()!).Elements().ToArray();Assert.Equal(expected.Length+1,f.Assessment.ElementFacts.Length);
        for(int i=0;i<expected.Length;i++)
        {
            var node=f.Element([i]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.Same(fact,f.Assessment.ElementFacts[i+1]);Assert.Same(node,f.Input.ElementNodes[i+1]);Assert.Equal(expected[i].Name.LocalName,fact.NativeDeclaration.Name);Assert.Equal(expected[i].Name.NamespaceName,fact.NativeDeclaration.NamespaceUri);Assert.False(fact.HasNoTypedValue);if(expected[i].Attribute(XName.Get("type","http://www.w3.org/2001/XMLSchema-instance")) is not null){Assert.Equal("integer",fact.NativeDeclaration.Type.Name);Assert.Equal("unsignedByte",fact.NativeType.Name);Assert.NotSame(fact.NativeDeclaration.Type,fact.NativeType);}Assert.True(f.Session.Advisor!.OwnsNode(node,fact,f.Input,f.Assessment.Origin));var raw=fact.ReadTypedValue(default).Value;Assert.False(raw.IsList);Assert.True(raw.TryGetSingleAtomic(out _));f.Run(op=>{var value=f.Evaluate(op,node);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(fact.Type,evidence.ActualType);Assert.Same(f.Assessment.Origin,evidence.OriginalAssessment);return 0;});Assert.Equal(expected[i].Value,Text(f.Read(node)));
        }
    }
}
