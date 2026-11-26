// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies actual anyType/untypedAtomic components, scope, authority and expiry.
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
//                      | Charles Korthout | 0.21  | 26-11-2026     | Reconcile measured unknown-lax admissions; original stimuli retained.                     |
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.AnyTypeTerminals;
/// <summary>Verifies actual anyType governing and untypedAtomic value components without scalar substitution.</summary>
public sealed class NativeAnyTypeTerminalProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(bool explicitType=false,bool nested=false)
    {
        var declaration="<xs:element name='"+(nested?"e":"r")+"'"+(explicitType?" type='xs:anyType'":"")+"/>";
        return "<xs:schema xmlns:xs='"+X+"'>"+(nested?"<xs:element name='r'><xs:complexType><xs:sequence>"+declaration+"</xs:sequence></xs:complexType></xs:element>":declaration)+"</xs:schema>";
    }
    /// <summary>Real builtin anyType governs one original untypedAtomic value at root or sequence terminal.</summary>
    /// <param name="explicitType">Explicit anyType selector.</param><param name="nested">Nested selector.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void OriginalAuthorityAndExpiry(bool explicitType,bool nested)
    {
        var xml=nested?"<r><e> 01.20<!--keep--><?p value?> </e></r>":"<r> 01.20<!--keep--><?p value?> </r>";using var f=new Fixture(Schema(explicitType,nested),xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=(NativeSessionNode)f.Element(nested?[0]:[]);var fact=f.Input.FactsFor(node)!;
        Assert.True(fact.NativeType.IsAnyType);Assert.True(fact.NativeType.MixedContent);Assert.True(node.IsComplexType);Assert.False(fact.HasNoTypedValue);Assert.Equal((X,"anyType"),node.SchemaTypeAnnotation);Assert.Same(f.Provider.ResolveType(new(X,"anyType"),default).Value,fact.Type);
        var raw=fact.ReadTypedValue(default).Value;var cell=Assert.Single(raw.AtomicItems);Assert.False(raw.IsList);Assert.Equal("untypedAtomic",cell.Value.SchemaTypeName);Assert.Same(fact.Type,cell.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"untypedAtomic"),default).Value,cell.TypeEvidence.ActualType);Assert.NotSame(fact.Type,cell.TypeEvidence.ActualType);Assert.NotSame(f.Provider.ResolveType(new(X,"string"),default).Value,cell.TypeEvidence.ActualType);
        Assert.True(f.Session.Advisor!.OwnsNode(node,fact,f.Input,f.Assessment.Origin));Assert.True(f.Session.Advisor.OwnsTypedValue(raw,f.Input,f.Assessment.Origin));var value=f.Run(op=>f.Evaluate(op,node));Assert.Equal("untypedAtomic",value.SchemaTypeName);Assert.Equal(" 01.20 ",value.StringValue);var detached=f.Read(node);f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));Assert.Equal(" 01.20 ",detached.StringValue);
    }
    /// <summary>Empty anyType is one empty atom, distinct from empty complex and element-only typed values.</summary>
    [Fact]
    public void EmptyValueIsNotEmptySequence()
    {
        using var any=new Fixture(Schema(),"<r/>");var af=any.Input.FactsFor((NativeSessionNode)any.Element([]))!;Assert.Equal("",Assert.Single(af.ReadTypedValue(default).Value.AtomicItems).Value.StringValue);Assert.Equal(NativeScalarKind.String,any.Read(any.Element([])).Kind);Assert.False(af.IsNilled);
        using var empty=new Fixture(Schema().Replace("<xs:element name='r'/>","<xs:element name='r'><xs:complexType/></xs:element>"),"<r/>");var ef=empty.Input.FactsFor((NativeSessionNode)empty.Element([]))!;Assert.True(ef.NativeType.EmptyContent);Assert.Empty(ef.ReadTypedValue(default).Value.AtomicItems);Assert.Equal(NativeScalarKind.Empty,empty.Read(empty.Element([])).Kind);
    }
    /// <summary>Different anyType uses share one real component within a schema and no foreign authority.</summary>
    [Fact]
    public void CanonicalTypeAndForeignAuthority()
    {
        var schema="<xs:schema xmlns:xs='"+X+"'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e'/><xs:element name='f' type='xs:anyType'/></xs:sequence></xs:complexType></xs:element></xs:schema>";using var a=new Fixture(schema,"<r><e>a</e><f>b</f></r>");using var b=new Fixture(schema,"<r><e>a</e><f>b</f></r>");var e=a.Input.FactsFor((NativeSessionNode)a.Element([0]))!;var f=a.Input.FactsFor((NativeSessionNode)a.Element([1]))!;Assert.Same(e.NativeType,f.NativeType);Assert.Same(e.Type,f.Type);var foreign=b.Input.FactsFor((NativeSessionNode)b.Element([0]))!;Assert.False(a.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>a.Provider.IsSameComponent(e.Type,foreign.Type,default));Assert.True(a.Input.FactsFor((NativeSessionNode)a.Element([]))!.HasNoTypedValue);
    }
    /// <summary>Unqualified lax element/attribute assessment publishes no original facts.</summary>
    [Fact]
    public void WildcardFailurePublishesNothing()
    {
        foreach(var xml in new[]{"<r><e>1</e></r>","<r a='1'/>"}){using var f=new Fixture(Schema(),xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.True(f.Input.IsAssessed);Assert.NotEmpty(f.Assessment.Facts);Assert.NotNull(f.Snapshot);}
    }
    /// <summary>Wildcard refusal, cancellation and owner disposal preserve later admissions and detached reads.</summary>
    [Fact]
    public void RecoveryAndCancellation()
    {
        using var s=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,1,256,256)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);using var a=s.Assess("<r>a</r>",context).Value!;var bad=s.Assess("<r><e/></r>",context);Assert.Equal(NativeSchemaOutcome.Valid,bad.Outcome);bad.Value!.Dispose();using var b=s.Assess("<r>b</r>",context).Value!;Assert.Equal("a",a.ReadRootScalar().StringValue);Assert.Equal("b",b.ReadRootScalar().StringValue);using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>s.Assess("<r/>",context,c.Token));Assert.Throws<OperationCanceledException>(()=>b.ReadRootScalar(c.Token));
    }
}
