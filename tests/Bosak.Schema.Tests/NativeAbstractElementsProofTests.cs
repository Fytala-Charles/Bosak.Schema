// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Verifies canonical abstract declarations without poisoning shared types or locals.
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
namespace Bosak.Schema.Tests.AbstractElements;
/// <summary>Proves declaration-scoped abstract enforcement across existing assessment paths.</summary>
public sealed class NativeAbstractElementsProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(string profile="decimal",bool flag=true,string place="root",bool optional=false)
    {
        var defs=profile switch{"empty"=>"<xs:complexType name='T'/>","sequence"=>"<xs:complexType name='T'><xs:sequence><xs:element name='c' type='xs:decimal'/></xs:sequence></xs:complexType>","all"=>"<xs:complexType name='T'><xs:all><xs:element name='c' type='xs:decimal'/></xs:all></xs:complexType>","choice"=>"<xs:complexType name='T'><xs:choice><xs:element name='c' type='xs:decimal'/></xs:choice></xs:complexType>",_=>""};
        var type=profile=="decimal"?" type='xs:decimal'":profile=="anyType"?"":" type='T'";
        var roots="<xs:element name='a' abstract='"+(flag?"true":"false")+"'"+type+"/><xs:element name='b'"+type+"/>";
        if(place=="lax")roots+="<xs:element name='r'/>";
        else if(place!="root")roots+="<xs:element name='r'><xs:complexType><xs:"+(place=="local"?"sequence":place)+"><xs:element "+(place=="local"?"name='a' type='xs:decimal'":"ref='a'")+(optional?" minOccurs='0'":"")+"/></xs:"+(place=="local"?"sequence":place)+"></xs:complexType></xs:element>";
        return "<xs:schema xmlns:xs='"+X+"'>"+defs+roots+"</xs:schema>";
    }
    private static string Content(string profile)=>profile=="decimal"?"01.20":profile=="anyType"?" raw ":profile=="empty"?"":"<c>01.20</c>";
    /// <summary>Admitted root families reject true flags and retain original concrete facts for false flags.</summary>
    /// <param name="profile">Existing profile family.</param>
    [Theory][InlineData("decimal")][InlineData("anyType")][InlineData("empty")][InlineData("sequence")]
    public void RootAuthority(string profile)
    {
        using var bad=new Fixture(Schema(profile),"<a>"+Content(profile)+"</a>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);Assert.Empty(bad.Assessment.Facts);using var good=new Fixture(Schema(profile,false),"<a>"+Content(profile)+"</a>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);var root=good.Input.FactsFor((NativeSessionNode)good.Element([]))!;Assert.Same(good.Provider.ResolveElementDeclaration(new("","a"),default).Value,root.Declaration);if(profile=="decimal")Assert.Equal("1.2",good.Read(good.Element([])).DecimalValue.ToInvariantString(256));if(profile=="anyType")Assert.Equal(" raw ",good.Read(good.Element([])).StringValue);if(profile=="empty")Assert.Empty(root.ReadTypedValue(default).Value.AtomicItems);if(profile=="sequence")Assert.True(root.HasNoTypedValue);
    }
    /// <summary>Shared types do not transfer abstractness from one declaration to another.</summary>
    [Fact]public void SharedType()
    {
        var c=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:1024).Value!;var a=c.Roots.Single(p=>p.Root.Name=="a").Root;var b=c.Roots.Single(p=>p.Root.Name=="b").Root;Assert.Same(a.Type,b.Type);Assert.NotSame(a,b);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a>1</a>").Outcome);Assert.Equal(NativeOutcome.Valid,c.Assess("<b>1</b>").Outcome);using var f=new Fixture(Schema(),"<b>01.20</b>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Same(f.Provider.ResolveElementDeclaration(new("","b"),default).Value,fact.Declaration);Assert.NotSame(f.Provider.ResolveElementDeclaration(new("","a"),default).Value,fact.Declaration);
    }
    /// <summary>A same-named local particle is not the abstract global declaration.</summary>
    [Fact]public void LocalCollision()
    {
        using var f=new Fixture(Schema(place:"local"),"<r><a>01.20</a></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var local=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;Assert.NotSame(f.Provider.ResolveElementDeclaration(new("","a"),default).Value,local.Declaration);Assert.Equal("1.2",f.Read(f.Element([0])).DecimalValue.ToInvariantString(256));
    }
    /// <summary>Scalar references consult canonical globals in sequences, all and choice.</summary>
    [Fact]public void ScalarReferences()
    {
        foreach(var model in new[]{"sequence","all","choice"}){using var bad=new Fixture(Schema(place:model),"<r><a>1</a></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);using var good=new Fixture(Schema(flag:false,place:model),"<r><a>01.20</a></r>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);Assert.Same(good.Provider.ResolveElementDeclaration(new("","a"),default).Value,good.Input.FactsFor((NativeSessionNode)good.Element([0]))!.Declaration);}
    }
    /// <summary>Complex and anyType references cannot bypass abstract global authority.</summary>
    [Fact]public void ComplexReferences()
    {
        foreach(var profile in new[]{"empty","sequence","anyType"}){using var f=new Fixture(Schema(profile,place:"sequence"),"<r><a>"+Content(profile)+"</a></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);}
    }
    /// <summary>Known lax matches reject abstract declarations before scalar/container dispatch.</summary>
    [Fact]public void KnownLax()
    {
        foreach(var profile in new[]{"decimal","anyType","empty","sequence","all","choice"}){using var f=new Fixture(Schema(profile,place:"lax"),"<r><a>"+Content(profile)+"</a></r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);}using var good=new Fixture(Schema(flag:false,place:"lax"),"<r><a>01.20</a></r>");Assert.Equal(SchemaAssessmentStatus.Valid,good.Assessment.Status);Assert.Same(good.Provider.ResolveElementDeclaration(new("","a"),default).Value,good.Input.FactsFor((NativeSessionNode)good.Element([0]))!.Declaration);
    }
    /// <summary>Type selection and nil requests cannot legalize an abstract declaration.</summary>
    [Fact]public void TypeAndNil()
    {
        var schema=Schema().Replace("abstract='true' type='xs:decimal'","abstract='true' nillable='true' type='xs:decimal'");foreach(var xml in new[]{"<a xmlns:xs='"+X+"' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='xs:decimal'>1</a>","<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>"}){using var f=new Fixture(schema,xml);Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);}
    }
    /// <summary>Unused optional abstract references do not invalidate their concrete owner.</summary>
    [Fact]public void OptionalUnused()
    {
        using var f=new Fixture(Schema(place:"sequence",optional:true),"<r/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.True(f.Input.FactsFor((NativeSessionNode)f.Element([]))!.HasNoTypedValue);
    }
    /// <summary>Invalid abstract assessment recovers to a concrete alias and preserves detached lifetime.</summary>
    [Fact]public void RecoveryAndLifetime()
    {
        using var s=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,16,256,1024)).Value!;Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<a>1</a>",new(DateTimeOffset.UnixEpoch,0)).Outcome);using var d=s.Assess("<b>01.20</b>",new(DateTimeOffset.UnixEpoch,0)).Value!;var value=d.ReadRootScalar();Assert.Equal("1.2",value.DecimalValue.ToInvariantString(256));d.Dispose();Assert.Throws<ObjectDisposedException>(()=>d.ReadRootScalar());Assert.Equal("1.2",value.DecimalValue.ToInvariantString(256));Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<a>1</a>",new(DateTimeOffset.UnixEpoch,0)).Outcome);
    }
    /// <summary>Concrete facts retain foreign authority, cancellation and expiry.</summary>
    [Fact]public void Ownership()
    {
        using var f=new Fixture(Schema(),"<b>1</b>");using var other=new Fixture(Schema(),"<b>1</b>");var local=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.False(f.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(local.Type,foreign.Type,default));using var cancel=new CancellationTokenSource();cancel.Cancel();Assert.Throws<OperationCanceledException>(()=>local.ReadTypedValue(cancel.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>local.Declaration);
    }
    /// <summary>Legacy decimal-only compilation keeps its existing attribute grammar.</summary>
    [Fact]public void LegacyRefusal()=>Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(),new(256,256)).Outcome);
}
