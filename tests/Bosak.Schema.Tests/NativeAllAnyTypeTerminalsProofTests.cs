// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Verifies original all-group anyType member values and authority.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.AllAnyTypeTerminals;
/// <summary>Proves original all member authority and physical-order anyType values.</summary>
public sealed class NativeAllAnyTypeTerminalsProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(bool named=false,bool explicitType=false,bool optional=false)
    {
        var body="<xs:all><xs:element name='a'"+(explicitType?" type='xs:anyType'":"")+(optional?" minOccurs='0'":"")+"/><xs:element name='d' type='xs:decimal' minOccurs='0'/></xs:all>";
        return "<xs:schema xmlns:xs='"+X+"'>"+(named?"<xs:complexType name='T'>"+body+"</xs:complexType><xs:element name='r' type='T'/><xs:element name='s' type='T'/>":"<xs:element name='r'><xs:complexType>"+body+"</xs:complexType></xs:element>")+"</xs:schema>";
    }
    /// <summary>Original declarations and canonical anyType governing versus actual untypedAtomic evidence remain.</summary>
    /// <param name="named">Named root owner selector.</param><param name="explicitType">Explicit rather than default anyType.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void Authority(bool named,bool explicitType)
    {
        using var f=new Fixture(Schema(named,explicitType),"<r><a> 001 </a><d>01.20</d></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var root=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(root.HasNoTypedValue);Assert.Equal(SchemaResultKind.Unsupported,root.ReadTypedValue(default).Kind);var a=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.NotNull(a.Declaration);Assert.True(a.NativeType.IsAnyType);Assert.Same(f.Provider.ResolveType(new(X,"anyType"),default).Value,a.Type);var atom=Assert.Single(a.ReadTypedValue(default).Value.AtomicItems);Assert.Equal("untypedAtomic",atom.Value.SchemaTypeName);Assert.Equal(" 001 ",atom.Value.StringValue);Assert.Same(a.Type,atom.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"untypedAtomic"),default).Value,atom.TypeEvidence.ActualType);Assert.NotSame(a.Type,atom.TypeEvidence.ActualType);Assert.Equal("1.2",f.Read(f.Element([1])).DecimalValue.ToInvariantString(256));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>a.Declaration);Assert.Throws<ObjectDisposedException>(()=>a.ReadTypedValue(default));
    }
    /// <summary>Physical ordering follows original XML while unordered matching selects correct declarations.</summary>
    [Fact]public void PhysicalOrder()
    {
        using var f=new Fixture(Schema(),"<r><d>01.20</d><a>x<!--ignored--><?p ignored?>y</a></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal("1.2",f.Read(f.Element([0])).DecimalValue.ToInvariantString(256));Assert.Equal("xy",f.Read(f.Element([1])).StringValue);Assert.True(f.Input.FactsFor((NativeSessionNode)f.Element([1]))!.NativeType.IsAnyType);
    }
    /// <summary>Optional absence has no child facts; present empty anyType yields one empty string atom.</summary>
    [Fact]public void OptionalEmpty()
    {
        using var absent=new Fixture(Schema(optional:true),"<r/>");Assert.Equal(SchemaAssessmentStatus.Valid,absent.Assessment.Status);using var present=new Fixture(Schema(optional:true),"<r><a/></r>");var fact=present.Input.FactsFor((NativeSessionNode)present.Element([0]))!;Assert.False(fact.HasNoTypedValue);Assert.False(fact.NativeType.EmptyContent);Assert.Equal("",Assert.Single(fact.ReadTypedValue(default).Value.AtomicItems).Value.StringValue);
    }
    /// <summary>Named aliases retain the same local declaration and governing anyType component.</summary>
    [Fact]public void Aliases()
    {
        var c=NativeDecimalSchema.Compile(Schema(named:true),new(256,256),maxStringCharacters:1024).Value!;var roots=c.Roots.Where(p=>p.Root.Name is "r" or "s").ToArray();Assert.Same(roots[0].Root.Type,roots[1].Root.Type);Assert.Same(roots[0].Children[0],roots[1].Children[0]);using var s=NativeSchemaSession.Create(Schema(named:true),new(256,256,1024,65536,2,256,16,256,1024)).Value!;foreach(var name in new[]{"r","s"}){using var d=s.Assess("<"+name+"><a> raw </a></"+name+">",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.False(d.HasRootScalar);Assert.Equal(" raw ",d.ReadScalar(0).StringValue);}
    }
    /// <summary>UTF16 text capacity refuses without publication and permits later bounded assessment.</summary>
    [Fact]public void Capacity()
    {
        var c=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:3).Value!;var failed=c.Assess("<r><a>😀ab</a></r>");Assert.Equal(NativeOutcome.Unsupported,failed.Outcome);Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,failed.UnsupportedReason);Assert.Null(failed.Value);Assert.Equal(NativeOutcome.Valid,c.Assess("<r><a>abc</a></r>").Outcome);
    }
    /// <summary>Duplicate members and unsupported terminal breadth publish no partial facts.</summary>
    [Fact]public void FailureRecovery()
    {
        foreach(var xml in new[]{"<r><a/><a/></r>","<r><a u='x'/></r>","<r><a><u/></a></r>"}){using var f=new Fixture(Schema(),xml);Assert.NotEqual(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);}using var s=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,16,256,1024)).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r><a/><a/></r>",new(DateTimeOffset.UnixEpoch,0)).Outcome);using var d=s.Assess("<r><a>x</a></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;Assert.Equal("x",d.ReadScalar(0).StringValue);
    }
    /// <summary>Local facts retain cancellation and foreign authority boundaries.</summary>
    [Fact]public void Ownership()
    {
        using var f=new Fixture(Schema(),"<r><a>x</a></r>");using var other=new Fixture(Schema(),"<r><a>x</a></r>");var local=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([0]))!;Assert.False(f.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(local.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>local.ReadTypedValue(cancel.Token));
    }
    /// <summary>Scalar-only all members retain decimal rather than anyType evidence.</summary>
    [Fact]public void ScalarControl()
    {
        using var f=new Fixture(Schema(explicitType:true).Replace("type='xs:anyType'","type='xs:decimal'"),"<r><a>01.20</a></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.False(f.Input.FactsFor((NativeSessionNode)f.Element([0]))!.NativeType.IsAnyType);Assert.Equal("1.2",f.Read(f.Element([0])).DecimalValue.ToInvariantString(256));
    }
    /// <summary>The legacy decimal-only compiler remains outside the all-group opt-in profile.</summary>
    [Fact]public void LegacyRefusal()=>Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(),new(256,256)).Outcome);
}
