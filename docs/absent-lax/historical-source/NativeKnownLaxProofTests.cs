// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies original global declarations and actual typed provenance through known lax matches.
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
using Xunit;
namespace Bosak.Schema.Tests.KnownLax;
/// <summary>Verifies no wildcard-use declarations are fabricated and known invalid values prevent publication.</summary>
public sealed class NativeKnownLaxProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema()=>"<xs:schema xmlns:xs='"+X+"'><xs:element name='d' type='xs:decimal'/><xs:element name='e'/><xs:attribute name='a' type='xs:integer'/><xs:element name='r'/></xs:schema>";
    /// <summary>Known matches retain actual original global declaration, type, owner and expiry.</summary>
    /// <param name="attribute">Attribute rather than child selector.</param><param name="nested">Nested anyType owner selector.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void OriginalGlobalAuthority(bool attribute,bool nested)
    {
        var content=attribute?"<e a='001'/>":"<e><d>01.20</d></e>";var xml=nested?"<r>"+content+"</r>":attribute?"<r a='001'/>":"<r><d>01.20</d></r>";using var f=new Fixture(Schema(),xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var node=(NativeSessionNode)(attribute?f.Attribute(nested?[0]:[],"","a"):f.Element(nested?[0,0]:[0]));var fact=f.Input.FactsFor(node)!;var declaration=attribute?f.Provider.ResolveAttributeDeclaration(new("","a"),default):f.Provider.ResolveElementDeclaration(new("","d"),default);Assert.Same(declaration.Value,fact.Declaration);Assert.Equal(attribute?"a":"d",fact.NativeDeclaration.Name);Assert.Equal(attribute,fact.NativeDeclaration.IsAttribute);Assert.Same(fact.NativeDeclaration,fact.NativeDeclaration.CanonicalDeclaration);
        var raw=fact.ReadTypedValue(default).Value;var item=Assert.Single(raw.AtomicItems);Assert.Same(fact.Type,item.TypeEvidence.DeclaredType);Assert.Same(fact.Type,item.TypeEvidence.ActualType);Assert.Same(f.Provider.ResolveType(new(X,attribute?"integer":"decimal"),default).Value,fact.Type);Assert.True(f.Session.Advisor!.OwnsNode(node,fact,f.Input,f.Assessment.Origin));Assert.True(f.Session.Advisor.OwnsTypedValue(raw,f.Input,f.Assessment.Origin));var detached=f.Read(node);if(attribute)Assert.Equal("1",detached.IntegerValue.ToString());else Assert.Equal("1.2",detached.DecimalValue.ToInvariantString(256));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));
    }
    /// <summary>Known IDREFS members keep actual global declaration and IDREF handles and resolve forward.</summary>
    [Fact]
    public void GlobalListIdentityAndScope()
    {
        var schema=Schema().Replace("<xs:attribute name='a' type='xs:integer'/>","<xs:attribute name='refs' type='xs:IDREFS'/><xs:attribute name='id' type='xs:ID'/>");using var f=new Fixture(schema,"<r refs='v v'><e id='v'/></r>",atoms:16);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","refs"))!;Assert.Same(f.Provider.ResolveAttributeDeclaration(new("","refs"),default).Value,fact.Declaration);var raw=fact.ReadTypedValue(default).Value;Assert.True(raw.IsList);Assert.Equal(2,raw.AtomicItems.Count());Assert.All(raw.AtomicItems,x=>{Assert.Equal("v",x.Value.StringValue);Assert.Same(f.Provider.ResolveType(new(X,"IDREF"),default).Value,x.TypeEvidence.ActualType);});
        var compiled=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024).Value!;Assert.Equal(NativeOutcome.Invalid,compiled.Assess("<r refs='v'/>").Outcome);Assert.Equal(NativeOutcome.Valid,compiled.Assess("<r refs='x'><e id='x'/></r>").Outcome);var cursor=raw.AtomicItems.GetEnumerator();f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>cursor.MoveNext());
    }
    /// <summary>Unknown and late invalid known matches publish no facts and permit recovery/cancellation.</summary>
    [Fact]
    public void FailurePublishesNothing()
    {
        foreach(var (xml,status) in new[]{("<r><d>1</d><d>bad</d></r>",SchemaAssessmentStatus.Invalid),("<r a='bad'/>",SchemaAssessmentStatus.Invalid),("<r><d>1</d><missing/></r>",SchemaAssessmentStatus.Unsupported),("<r unknown='x'/>",SchemaAssessmentStatus.Unsupported)}){using var f=new Fixture(Schema(),xml);Assert.Equal(status,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);}
        using var s=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,1,256,256)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r><d>bad</d></r>",context).Outcome);using var d=s.Assess("<r><d>01.20</d></r>",context).Value!;Assert.Equal("1.2",d.ReadScalar(0).DecimalValue.ToInvariantString(256));using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>s.Assess("<r/>",context,c.Token));Assert.Throws<OperationCanceledException>(()=>d.ReadScalar(0,c.Token));
    }
    /// <summary>Dynamic known wildcard paths remain within actual depth and value-state capacities.</summary>
    [Fact]
    public void DynamicCapacity()
    {
        var compiled=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:1024).Value!;Assert.Equal(NativeOutcome.Valid,compiled.Assess("<r><e><e><e>x</e></e></e></r>").Outcome);var deep=compiled.Assess("<r><e><e><e><e>x</e></e></e></e></r>");Assert.Equal(NativeOutcome.Unsupported,deep.Outcome);Assert.Null(deep.Value);Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,deep.UnsupportedReason);
        var tooMany=compiled.Assess("<r>"+string.Concat(Enumerable.Repeat("<d>1</d>",16))+"</r>");Assert.Equal(NativeOutcome.Unsupported,tooMany.Outcome);Assert.Null(tooMany.Value);Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,tooMany.UnsupportedReason);
    }
    /// <summary>Repeated known occurrences share global components while attribute order stays source-stable.</summary>
    [Fact]
    public void CanonicalOccurrencesAndOrder()
    {
        var schema=Schema().Replace("<xs:attribute name='a' type='xs:integer'/>","<xs:attribute name='a' type='xs:integer'/><xs:attribute name='b' type='xs:token'/>");using var f=new Fixture(schema,"<r b=' x ' a='001'><d>01.20</d><d>2</d></r>");var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"","a"))!;Assert.Same(a,f.Assessment.AttributeFacts[0]);Assert.Equal("b",f.Assessment.AttributeFacts[1].NativeDeclaration.Name);var first=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;var second=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;Assert.Same(first.Declaration,second.Declaration);Assert.Same(first.Type,second.Type);Assert.Same(f.Provider.ResolveElementDeclaration(new("","d"),default).Value,first.Declaration);var root=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal("untypedAtomic",Assert.Single(root.ReadTypedValue(default).Value.AtomicItems).Value.SchemaTypeName);Assert.Equal("01.202",f.Read(f.Element([])).StringValue);
    }
}
