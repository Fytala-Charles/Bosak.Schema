// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies original mixed type identity, raw aggregates and strict attribute authority.
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
namespace Bosak.Schema.Tests.MixedWildcards;
/// <summary>Proves original mixed governing identities and independent strict wildcard values.</summary>
public sealed class NativeMixedWildcardsProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(bool nested=false,bool named=false,bool optional=false,bool mixed=true)
    {
        var body="<xs:sequence><xs:element name='c' type='xs:decimal'"+(optional?" minOccurs='0'":"")+"/></xs:sequence><xs:anyAttribute notQName='unused'/>";
        var type="<xs:complexType mixed='"+(mixed?"true":"false")+"'>"+body+"</xs:complexType>";
        var root=nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e'>"+type+"</xs:element></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r'>"+type+"</xs:element>";
        if(named)root="<xs:complexType name='T' mixed='"+(mixed?"true":"false")+"'>"+body+"</xs:complexType>"+(nested?"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='e' type='T'/></xs:sequence></xs:complexType></xs:element>":"<xs:element name='r' type='T'/><xs:element name='s' type='T'/>");
        return "<xs:schema xmlns:xs='"+X+"'><xs:attribute name='a' type='xs:integer'/><xs:attribute name='b' type='xs:token'/><xs:attribute name='id' type='xs:ID'/><xs:attribute name='ref' type='xs:IDREF'/>"+root+"</xs:schema>";
    }
    /// <summary>Original complex type, actual untypedAtomic and independent attribute/child evidence remain.</summary>
    /// <param name="nested">Nested owner selector.</param><param name="named">Named type selector.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void OriginalAuthority(bool nested,bool named)
    {
        using var f=new Fixture(Schema(nested,named),nested?"<r><e a='001' b='  x  '>pre<c>01.20</c>post</e></r>":"<r a='001' b='  x  '>pre<c>01.20</c>post</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var path=nested?new[]{0}:Array.Empty<int>();var owner=f.Input.FactsFor((NativeSessionNode)f.Element([..path]))!;Assert.True(owner.NativeType.MixedContent);Assert.False(owner.HasNoTypedValue);Assert.False(owner.NativeType.IsAnyType);Assert.NotNull(owner.Declaration);if(named)Assert.Same(f.Provider.ResolveType(new("","T"),default).Value,owner.Type);
        var atom=Assert.Single(owner.ReadTypedValue(default).Value.AtomicItems);Assert.Equal("untypedAtomic",atom.Value.SchemaTypeName);Assert.Equal("pre01.20post",atom.Value.StringValue);Assert.Same(owner.Type,atom.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"untypedAtomic"),default).Value,atom.TypeEvidence.ActualType);Assert.NotSame(owner.Type,atom.TypeEvidence.ActualType);
        var a=f.Input.FactsFor((NativeSessionNode)f.Attribute([..path],"","a"))!;Assert.Same(f.Provider.ResolveAttributeDeclaration(new("","a"),default).Value,a.Declaration);Assert.Equal("1",f.Read(f.Attribute([..path],"","a")).IntegerValue.ToString());Assert.Equal("x",f.Read(f.Attribute([..path],"","b")).StringValue);Assert.Equal("1.2",f.Read(f.Element([..path,0])).DecimalValue.ToInvariantString(256));Assert.Equal("pre01.20post",f.Read(f.Element([..path])).StringValue);
        f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>owner.ReadTypedValue(default));Assert.Throws<ObjectDisposedException>(()=>a.Declaration);
    }
    /// <summary>Named root aliases retain the same actual mixed type and wildcard metadata.</summary>
    [Fact]public void NamedAliases()
    {
        var schema=Schema(named:true);var compiled=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024).Value!;var roots=compiled.Roots.Where(p=>p.Root.Name is "r" or "s").ToArray();Assert.Equal(2,roots.Length);Assert.Same(roots[0].Root.Type,roots[1].Root.Type);Assert.Same(roots[0].CompiledAttributeWildcard,roots[1].CompiledAttributeWildcard);Assert.NotNull(roots[0].CompiledAttributeWildcard);
        using var session=NativeSchemaSession.Create(schema,new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);foreach(var name in new[]{"r","s"}){using var d=session.Assess("<"+name+" a='1'>pre<c>01.20</c>post</"+name+">",context).Value!;Assert.True(d.HasRootScalar);Assert.Equal("pre01.20post",d.ReadRootScalar().StringValue);Assert.Equal("1.2",d.ReadScalar(0).DecimalValue.ToInvariantString(256));}
    }
    /// <summary>Optional absent children yield one original string atom, including the empty string.</summary>
    /// <param name="text">Original direct text.</param>
    [Theory][InlineData("")][InlineData(" text ")]
    public void OptionalAbsent(string text)
    {
        using var f=new Fixture(Schema(optional:true),"<r a='1'>"+text+"</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.False(owner.HasNoTypedValue);Assert.False(owner.NativeType.EmptyContent);var atom=Assert.Single(owner.ReadTypedValue(default).Value.AtomicItems);Assert.Equal("untypedAtomic",atom.Value.SchemaTypeName);Assert.Equal(text,atom.Value.StringValue);
    }
    /// <summary>Attribute and child invalidity prevent partial facts and later assessment recovers.</summary>
    [Fact]public void FailureAndRecovery()
    {
        foreach(var xml in new[]{"<r a='bad'>pre<c>1</c>post</r>","<r a='1'>pre<c>bad</c>post</r>","<r unknown='1'><c>1</c></r>"}){using var f=new Fixture(Schema(),xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);}
        using var s=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r a='bad'><c>1</c></r>",context).Outcome);using var d=s.Assess("<r a='1'>pre<c>01.20</c>post</r>",context).Value!;Assert.Equal("pre01.20post",d.ReadRootScalar().StringValue);
    }
    /// <summary>Identity, foreign authority and cancelled reads retain existing boundaries.</summary>
    [Fact]public void IdentityAndOwnership()
    {
        using var good=new Fixture(Schema(),"<r id='v' ref='v'>pre<c>1</c>post</r>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);using var bad=new Fixture(Schema(),"<r ref='missing'><c>1</c></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);using var other=new Fixture(Schema(),"<r id='v'><c>1</c></r>");var local=good.Input.FactsFor((NativeSessionNode)good.Attribute([],"","id"))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Attribute([],"","id"))!;Assert.False(good.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>good.Provider.IsSameComponent(local.Type,foreign.Type,default));using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>local.ReadTypedValue(c.Token));
    }
    /// <summary>Aggregate capacity refusal produces no document and a bounded later input recovers.</summary>
    [Fact]public void AggregateCapacity()
    {
        var compiled=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:6).Value!;var failed=compiled.Assess("<r a='1'>x<c>01.20</c>y</r>");Assert.Equal(NativeOutcome.Unsupported,failed.Outcome);Assert.Null(failed.Value);Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,failed.UnsupportedReason);Assert.Equal(NativeOutcome.Valid,compiled.Assess("<r a='1'>x<c>1</c>y</r>").Outcome);
    }
    /// <summary>The equivalent element-only model retains no typed value.</summary>
    [Fact]public void ElementOnlyControl()
    {
        using var f=new Fixture(Schema(mixed:false),"<r a='1'><c>1</c></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(owner.HasNoTypedValue);Assert.Equal(SchemaResultKind.Unsupported,owner.ReadTypedValue(default).Kind);
    }
    /// <summary>Known anyType matches retain the actual mixed child declaration and aggregate.</summary>
    [Fact]public void KnownLaxContainer()
    {
        var schema=Schema().Replace("<xs:element name='r'>","<xs:element name='s'>").Replace("</xs:schema>","<xs:element name='r'/></xs:schema>");using var f=new Fixture(schema,"<r><s a='1'>pre<c>01.20</c>post</s></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.Same(f.Provider.ResolveElementDeclaration(new("","s"),default).Value,owner.Declaration);Assert.False(owner.NativeType.IsAnyType);Assert.Equal("pre01.20post",f.Read(f.Element([0])).StringValue);Assert.Equal("1.2",f.Read(f.Element([0,0])).DecimalValue.ToInvariantString(256));
    }
}
