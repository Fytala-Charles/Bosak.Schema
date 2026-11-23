// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated derived integer values and original type evidence.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 22-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Collections.Immutable;
using System.Numerics;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.DerivedIntegers;
/// <summary>Verifies proposed derived integer cases and actual original type identity.</summary>
public sealed class NativeDerivedIntegerIdentityCoreTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static NativeSchemaSessionOptions Options(int digits=256)=>new(digits,256,1024,65536,1,256,1,256,256);
    /// <summary>Gets proposed payloads and facade selectors.</summary><returns>Explicit payload controls.</returns>
    public static IEnumerable<object[]> Cases(){using var d=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData/DerivedIntegers/cases.json")));foreach(var row in d.RootElement.GetProperty("cases").EnumerateArray())foreach(bool data in new[]{false,true})yield return[row.GetProperty("id").GetString()!,row.GetRawText(),data];}
    private static void Check(NativeScalarValue value,string expected){Assert.Equal(NativeScalarKind.Integer,value.Kind);Assert.Equal(BigInteger.Parse(expected,CultureInfo.InvariantCulture),value.IntegerValue);}
    /// <summary>Gets original identity controls for all types and placements.</summary><returns>Identity controls.</returns>
    public static IEnumerable<object[]> Identities(){foreach(var x in Cases()){using var d=JsonDocument.Parse((string)x[1]);if(!(bool)x[2]&&d.RootElement.GetProperty("purpose").GetString()!.EndsWith("preserves original derived integer identity",StringComparison.Ordinal))yield return x;}}
    /// <summary>Checks original native issuance through the public Core original-node route.</summary>
    /// <param name="id">Case identity.</param><param name="json">Original payload.</param><param name="data">Unused facade selector, asserted false.</param>
    [Theory][MemberData(nameof(Identities))]
    public void OriginalIdentity(string id,string json,bool data)
    {
        Assert.False(data);using var doc=JsonDocument.Parse(json);var r=doc.RootElement;var placement=r.GetProperty("placement").GetString();var family=r.GetProperty("declaredBuiltin").GetString()!;var path=r.GetProperty("elementPath").EnumerateArray().Select(x=>x.GetInt32()).ToImmutableArray();var attribute=r.GetProperty("attribute").GetString();using var f=new Fixture(r.GetProperty("schema").GetString()!,r.GetProperty("instance").GetString()!);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var node=attribute is null?f.Element(path):f.Attribute(path,"",attribute);var fact=f.Input.FactsFor((NativeSessionNode)node)!;Assert.False(fact.HasNoTypedValue);Assert.False(fact.IsNilled);Assert.True(f.Session.Advisor!.OwnsNode(node,fact,f.Input,f.Assessment.Origin));Assert.Equal("",fact.NativeDeclaration.NamespaceUri);
        var name=placement=="namedRestriction"?"N":placement is "anonymousRestriction" or "simpleContent"?null:family;Assert.Equal(name,fact.NativeType.Name);Assert.Equal(placement is "namedRestriction" or "anonymousRestriction" or "simpleContent"?"":X,fact.NativeType.NamespaceUri);if(name is null)Assert.Null(node.SchemaTypeAnnotation);else Assert.Equal((fact.NativeType.NamespaceUri,name),node.SchemaTypeAnnotation!.Value);
        var actual=placement=="simpleContent"?f.Provider.ResolveType(new(X,family),default).Value:fact.Type;var raw=fact.ReadTypedValue(default).Value;Assert.False(raw.IsList);Assert.True(raw.TryGetSingleAtomic(out var atom));Assert.Equal(XdmValueKind.Integer,atom.Value.Kind);Assert.Same(fact.Type,raw.GoverningType);Assert.Same(fact.Type,atom.TypeEvidence.DeclaredType);Assert.Same(actual,atom.TypeEvidence.ActualType);
        var expected=BigInteger.Parse(r.GetProperty("expectedValue").GetString()!,CultureInfo.InvariantCulture);Assert.Equal(expected,atom.Value.BigIntegerValue);
        if(placement is "root" or "leaf" or "nestedLeaf" or "attribute")Assert.Same(fact.Type,f.Provider.ResolveType(new(X,family),default).Value);
        f.Run(op=>{var v=f.Evaluate(op,node);Assert.Equal(expected,v.BigIntegerValue);Assert.True(SchemaAtomics.TryGetProvenance(v,out var origin));Assert.Same(fact.Type,origin!.DeclaredType);Assert.Same(actual,origin.ActualType);Assert.Same(f.Assessment.Origin,origin.OriginalAssessment);File.WriteAllText(Path.Combine(AppContext.BaseDirectory,id+"-identity.json"),JsonSerializer.Serialize(new{id,placement,family,observedDescriptiveType=v.SchemaTypeName,actualOriginalTypeVerified=true,bigInteger=expected.ToString(CultureInfo.InvariantCulture),generalTypedExecutionClaim=false}));return 0;});Check(f.Read(node),expected.ToString(CultureInfo.InvariantCulture));
    }
}
