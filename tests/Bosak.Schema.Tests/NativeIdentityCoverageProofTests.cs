// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies bounded validation-root identity and reference contracts.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.IdentityCoverage;

/// <summary>Verifies actual identity member provenance, absence of failed facts and assessment-local state.</summary>
public sealed class NativeIdentityCoverageProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(string aliasType="xs:ID",string extra="")=>$"<xs:schema xmlns:xs='{X}'>{extra}<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:string'/></xs:sequence><xs:attribute name='id' type='xs:ID'/><xs:attribute name='alias' type='{aliasType}'/><xs:attribute name='ref' type='xs:IDREF'/><xs:attribute name='refs' type='xs:IDREFS'/></xs:complexType></xs:element></xs:schema>";
    /// <summary>Built-in ID keeps original governing/atomic identity and normalized detached strings.</summary>
    [Fact]
    public void IdAuthorityAndExpiry()
    {
        using var f=new Fixture(Schema(),"<r id=' a ' ref='a'><v>x</v></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var node=(NativeSessionNode)f.Attribute([],"","id");var fact=f.Input.FactsFor(node)!;Assert.Equal("ID",fact.NativeType.Name);Assert.Equal(NativeTextKind.ID,fact.NativeType.TextKind);Assert.Equal("NCName",fact.NativeType.DerivationBase!.Name);
        var raw=fact.ReadTypedValue(default).Value;Assert.False(raw.IsList);var item=Assert.Single(raw.AtomicItems);Assert.Same(fact.Type,item.TypeEvidence.ActualType);Assert.True(f.Session.Advisor!.OwnsTypedValue(raw,f.Input,f.Assessment.Origin));Assert.True(f.Session.Advisor.OwnsNode(node,fact,f.Input,f.Assessment.Origin));
        var detached=f.Read(node).StringValue;Assert.Equal("a",detached);f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal("a",detached);
    }
    /// <summary>Only the actual selected union member supplies ID participation and atomic authority.</summary>
    /// <param name="idFirst">Member order selector.</param>
    [Theory][InlineData(true)][InlineData(false)]
    public void ActualUnionMember(bool idFirst)
    {
        var members=idFirst?"xs:ID xs:string":"xs:string xs:ID";var extra="<xs:simpleType name='U'><xs:union memberTypes='"+members+"'/></xs:simpleType>";
        using var f=new Fixture(Schema("U",extra),"<r alias='a'><v>x</v></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","alias"))!;
        Assert.Equal("U",fact.NativeType.Name);var item=Assert.Single(fact.ReadTypedValue(default).Value.AtomicItems);Assert.Same(fact.Type,item.TypeEvidence.DeclaredType);Assert.Same(f.Provider.TypeComponent(fact.NativeType.UnionMembers[0]),item.TypeEvidence.ActualType);Assert.NotSame(fact.Type,item.TypeEvidence.ActualType);
    }
    /// <summary>Reference-list items retain the actual IDREF handles and ordered repeated values.</summary>
    [Fact]
    public void ListMemberAuthority()
    {
        using var f=new Fixture(Schema(),"<r id='a' alias='b' refs='a b a'><v>x</v></r>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","refs"))!;Assert.Equal("IDREFS",fact.NativeType.Name);Assert.Equal("IDREF",fact.NativeType.ListItemType!.Name);
        var raw=fact.ReadTypedValue(default).Value;Assert.True(raw.IsList);Assert.False(raw.TryGetSingleAtomic(out _));var items=raw.AtomicItems.ToArray();Assert.Equal(new[]{"a","b","a"},items.Select(x=>x.Value.StringValue));Assert.All(items,x=>Assert.Same(f.Provider.TypeComponent(fact.NativeType.ListItemType),x.TypeEvidence.ActualType));
        var cursor=raw.AtomicItems.GetEnumerator();f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>cursor.MoveNext());
    }
    /// <summary>Late graph failure publishes no original facts, evidence or assessed input.</summary>
    /// <param name="xml">Unresolved reference or conflicting owners.</param>
    [Theory][InlineData("<r ref='missing'><v>x</v></r>")][InlineData("<r id='a' ref='missing'><v>x</v></r>")]
    public void FailureHasNoFacts(string xml)
    {
        using var f=new Fixture(Schema(),xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);
    }
    /// <summary>Bindings cannot leak from an active document to another admission in the same session.</summary>
    [Fact]
    public void RecoveryAndScopeIsolation()
    {
        var c=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,16,256,256));Assert.Equal(NativeSchemaOutcome.Valid,c.Outcome);using var s=c.Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
        using var first=s.Assess("<r id='a'><v>x</v></r>",context).Value!;var bad=s.Assess("<r ref='a'><v>x</v></r>",context);Assert.Equal(NativeSchemaOutcome.Invalid,bad.Outcome);Assert.Null(bad.Value);
        using var second=s.Assess("<r id='b' ref='b'><v>x</v></r>",context).Value!;Assert.Equal("b",second.ReadAttribute(0).Value.StringValue);Assert.Equal("a",first.ReadAttribute(0).Value.StringValue);
        using var cancelled=new CancellationTokenSource();cancelled.Cancel();Assert.Throws<OperationCanceledException>(()=>s.Assess("<r id='c'><v>x</v></r>",context,cancelled.Token));
    }
    /// <summary>The same immutable compiled schema has independent binding tables in parallel assessments.</summary>
    /// <returns>Completion.</returns>
    [Fact]
    public async Task ParallelTablesAreLocal()
    {
        var compiled=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:1024).Value!;
        await Task.WhenAll(Enumerable.Range(0,8).Select(_=>Task.Run(()=>{var a=compiled.Assess("<r id='a' ref='a'><v>x</v></r>");Assert.Equal(NativeOutcome.Valid,a.Outcome);var b=compiled.Assess("<r ref='a'><v>x</v></r>");Assert.Equal(NativeOutcome.Invalid,b.Outcome);Assert.Null(b.Value);}))); 
    }
}
