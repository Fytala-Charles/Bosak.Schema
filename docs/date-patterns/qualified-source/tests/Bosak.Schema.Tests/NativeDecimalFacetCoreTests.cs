// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Verifies named decimal facet facts and atomic provenance through actual Core.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks original named decimal facts and authority through actual Core sessions.</summary>
public sealed class NativeDecimalFacetCoreTests
{
    private const string Schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='urn:orders' targetNamespace='urn:orders' elementFormDefault='qualified'><xs:simpleType name='Money'><xs:restriction base='xs:decimal'><xs:minInclusive value='0'/><xs:fractionDigits value='2'/></xs:restriction></xs:simpleType><xs:complexType name='Order'><xs:sequence><xs:element name='amount' type='t:Money' maxOccurs='2'/></xs:sequence></xs:complexType><xs:element name='order' type='t:Order'/></xs:schema>";
    private static NativeSchemaProvider Provider()=>new(NativeDecimalSchema.Compile(Schema,new(256,256)).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider p)=>SchemaQuerySession.CompileXPath("$value",new(1,4,1,256,256),assessor:p).Value;
    private static NativeSchemaInput Input(string value="12.5000")=>NativeSchemaInput.Capture($"<o:order xmlns:o='urn:orders'><o:amount>{value}</o:amount><o:amount>2</o:amount></o:order>").Value!;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession s,NativeSchemaInput i)=>(NativeSchemaProvider.Assessment)s.Advisor!.Assess(new(i,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>new(2026,11,19,0,0,0,TimeSpan.Zero);
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }

    /// <summary>Retains original named handles through annotations, scalar reads and actual evaluation.</summary>
    [Fact]
    public void NamedLeaf_TransportPreservesDeclaredAndActualType()
    {
        var p=Provider();using var s=Session(p);using var input=Input();var result=Assess(s,input);
        Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);var money=p.ResolveType(new("urn:orders","Money"),default).Value;
        Assert.Same(money,result.Facts[1].Type);Assert.Same(money,result.Facts[2].Type);
        Assert.Equal(("urn:orders","Money"),input.ElementNodes[1].SchemaTypeAnnotation!.Value);
        var scalar=result.Facts[1].ReadTypedValue(default).Value;Assert.True(scalar.TryGetSingleAtomic(out var item));
        Assert.Same(money,item.TypeEvidence.DeclaredType);Assert.Same(money,item.TypeEvidence.ActualType);Assert.Same(result.Origin,item.TypeEvidence.OriginalAssessment);
        using var snapshot=s.OpenSnapshot(input,input,result.Origin).Value;
        var text=new NativeSessionRunner(s).Run([snapshot],op=>
        {
            var read=op.ReadTypedValue(snapshot,scalar);Assert.Equal(SchemaResultKind.Supported,read.Kind);
            var evaluated=op.Evaluate(new EvaluationContext().WithVariable("value",read.Value));
            Assert.True(SchemaAtomics.TryGetProvenance(evaluated,out var evidence));Assert.Same(money,evidence!.ActualType);
            return evaluated.ExactDecimalValue.ToInvariantString(256);
        });
        Assert.Equal("12.5",text);Assert.Equal("FOTY0012",result.Facts[0].ReadTypedValue(default).DiagnosticCode);
        input.Dispose();Assert.Throws<ObjectDisposedException>(()=>input.ElementNodes[1].SchemaTypeAnnotation);
    }

    /// <summary>Primitive decimal remains distinct even when every leaf has a named restriction.</summary>
    [Fact]
    public void PrimitiveLookup_IsSeparateFromFirstNamedLeaf()
    {
        var p=Provider();var money=p.ResolveType(new("urn:orders","Money"),default).Value;
        Assert.Same(p.DecimalType,p.ResolveType(new("http://www.w3.org/2001/XMLSchema","decimal"),default).Value);
        Assert.NotSame(money,p.DecimalType);Assert.True(p.Owns(p.DecimalType));
    }

    /// <summary>A failed facet publishes neither facts nor original evidence.</summary>
    [Fact]
    public void FacetFailure_AdmitsNoSnapshotOrFacts()
    {
        using var s=Session(Provider());using var input=Input("-1");var result=Assess(s,input);
        Assert.Equal(SchemaAssessmentStatus.Invalid,result.Status);Assert.Null(result.Evidence);Assert.Null(result.ValidatedRoot);Assert.Empty(result.Facts);
        Assert.False(input.IsAssessed);
    }

    /// <summary>Equal named restrictions in independent schemas grant no foreign authority.</summary>
    [Fact]
    public void NamedRestriction_ForeignAuthorityAndDisposalRemainGuarded()
    {
        var p=Provider();var q=Provider();var own=p.ResolveType(new("urn:orders","Money"),default).Value;var foreign=q.ResolveType(new("urn:orders","Money"),default).Value;
        Assert.False(p.Owns(foreign));Assert.Throws<ArgumentException>(()=>p.IsSameComponent(own,foreign,default));
        using var a=Session(p);using var b=Session(q);using var x=Input();using var y=Input();var ax=Assess(a,x);var by=Assess(b,y);
        Assert.False(a.Advisor!.OwnsNode(ax.ValidatedRoot!,by.Facts[1],x,ax.Origin));Assert.False(a.Advisor.OwnsAssessment(by.Origin));
        x.Dispose();Assert.Throws<ObjectDisposedException>(()=>ax.Facts[1].ReadTypedValue(default));
    }
}
