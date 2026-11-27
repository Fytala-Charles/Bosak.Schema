// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Proves anyType nil typed-empty values, independent attributes and native authority.
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
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.AnyTypeNillability;
/// <summary>Checks nilled anyType evidence independently from ordinary mixed atoms and wildcard attributes.</summary>
public sealed class NativeAnyTypeNillabilityProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private const string I="http://www.w3.org/2001/XMLSchema-instance";
    private static string Schema(bool explicitType=false,string place="root",bool repeat=false)
    {
        var a="<xs:element name='a' nillable='true'"+(explicitType?" type='xs:anyType'":"")+(repeat&&place=="local"?" maxOccurs='2'":"")+"/>";
        var roots=place=="root"?a:place=="local"?"<xs:element name='r'><xs:complexType><xs:sequence>"+a+"</xs:sequence></xs:complexType></xs:element>":a+(place=="lax"?"<xs:element name='r'/>":"<xs:element name='r'><xs:complexType><xs:sequence><xs:element ref='a'"+(repeat?" maxOccurs='2'":"")+"/></xs:sequence></xs:complexType></xs:element>");
        return "<xs:schema xmlns:xs='"+X+"'>"+roots+"<xs:attribute name='g' type='xs:decimal'/></xs:schema>";
    }
    /// <summary>Actual nil facts retain canonical anyType/declaration authority and a supported empty typed sequence.</summary>
    /// <param name="explicitType">Explicit type selector.</param><param name="place">Existing assessment route.</param>
    [Theory][InlineData(false,"root")][InlineData(true,"root")][InlineData(false,"local")][InlineData(true,"local")][InlineData(false,"ref")][InlineData(true,"ref")][InlineData(false,"lax")][InlineData(true,"lax")]
    public void NilAuthority(bool explicitType,string place)
    {
        var node="<a xmlns:xsi='"+I+"' xsi:nil='true'><!--c--><?p data?></a>";using var f=new Fixture(Schema(explicitType,place),place=="root"?node:"<r>"+node+"</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);System.Collections.Immutable.ImmutableArray<int> path=place=="root"?[]:[0];var element=f.Element(path);var fact=f.Input.FactsFor((NativeSessionNode)element)!;Assert.True(fact.IsNilled);Assert.True(fact.NativeDeclaration.IsNillable);Assert.False(fact.HasNoTypedValue);Assert.True(fact.NativeType.IsAnyType);Assert.Same(f.Provider.ResolveType(new(X,"anyType"),default).Value,fact.Type);if(place!="local")Assert.Same(f.Provider.ResolveElementDeclaration(new("","a"),default).Value,fact.Declaration);var typed=fact.ReadTypedValue(default);Assert.Equal(SchemaResultKind.Supported,typed.Kind);Assert.Empty(typed.Value.AtomicItems);Assert.False(typed.Value.TryGetSingleAtomic(out _));Assert.Equal(NativeScalarKind.Empty,f.Read(element).Kind);Assert.Equal(XdmValueKind.Undefined,f.Run(op=>f.Evaluate(op,element)).Kind);
    }
    /// <summary>Nilled owners still validate known attributes and retain separate unknown attribute authority.</summary>
    /// <param name="unknown">Unknown attribute selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void AttributeAuthority(bool unknown)
    {
        var name=unknown?"extra":"g";using var f=new Fixture(Schema(),"<a xmlns:xsi='"+I+"' xsi:nil='true' "+name+"='"+(unknown?" raw ":"01.20")+"'/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var owner=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.True(owner.IsNilled);Assert.Empty(owner.ReadTypedValue(default).Value.AtomicItems);var attr=f.Input.FactsFor((NativeSessionNode)f.Attribute([],"",name))!;var atom=Assert.Single(attr.ReadTypedValue(default).Value.AtomicItems);if(unknown){Assert.Null(attr.Declaration);Assert.Same(f.Provider.ResolveType(new(X,"anySimpleType"),default).Value,attr.Type);Assert.Equal("untypedAtomic",atom.Value.SchemaTypeName);Assert.Same(f.Provider.ResolveType(new(X,"untypedAtomic"),default).Value,atom.TypeEvidence.ActualType);Assert.Equal(" raw ",f.Read(f.Attribute([],"",name)).StringValue);}else{Assert.Same(f.Provider.ResolveAttributeDeclaration(new("","g"),default).Value,attr.Declaration);Assert.Equal("1.2",f.Read(f.Attribute([],"",name)).DecimalValue.ToInvariantString(256));}using var bad=new Fixture(Schema(),"<a xmlns:xsi='"+I+"' xsi:nil='true' g='bad'/>");Assert.Equal(SchemaAssessmentStatus.Invalid,bad.Assessment.Status);Assert.False(bad.Input.IsAssessed);Assert.Empty(bad.Assessment.Facts);
    }
    /// <summary>Repeated nilled and ordinary anyType nodes share original authority and distinct actual values.</summary>
    [Fact]public void RepeatedOriginals()
    {
        using var f=new Fixture(Schema(place:"ref",repeat:true),"<r xmlns:xsi='"+I+"'><a xsi:nil='true'/><a/></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var a=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;Assert.Same(a.Type,b.Type);Assert.Same(a.Declaration,b.Declaration);Assert.True(a.IsNilled);Assert.False(b.IsNilled);Assert.Empty(a.ReadTypedValue(default).Value.AtomicItems);var atom=Assert.Single(b.ReadTypedValue(default).Value.AtomicItems);Assert.Equal("",atom.Value.StringValue);Assert.Equal("untypedAtomic",atom.Value.SchemaTypeName);Assert.Same(b.Type,atom.TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new(X,"untypedAtomic"),default).Value,atom.TypeEvidence.ActualType);
    }
    /// <summary>Invalid nil content or attributes publish no document; later nil and ordinary values recover.</summary>
    [Fact]public void RecoveryAndLifetime()
    {
        using var s=NativeSchemaSession.Create(Schema(),new(256,256,1024,65536,2,256,16,256,1024)).Value!;var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);foreach(var xml in new[]{"<a xmlns:xsi='"+I+"' xsi:nil='true'> </a>","<a xmlns:xsi='"+I+"' xsi:nil='true' g='bad'/>"}){var bad=s.Assess(xml,context);Assert.Equal(NativeSchemaOutcome.Invalid,bad.Outcome);Assert.Null(bad.Value);}using var nil=s.Assess("<a xmlns:xsi='"+I+"' xsi:nil='true' g='01.20'/>",context).Value!;Assert.True(nil.IsRootNilled);Assert.False(nil.HasRootScalar);Assert.Empty(nil.ReadRootItems());using var ordinary=s.Assess("<a>text</a>",context).Value!;var detached=ordinary.ReadRootScalar();Assert.False(ordinary.IsRootNilled);ordinary.Dispose();Assert.Throws<ObjectDisposedException>(()=>ordinary.ReadRootItems());Assert.Equal("text",detached.StringValue);nil.Dispose();Assert.Throws<ObjectDisposedException>(()=>nil.ReadRootItems());
    }
    /// <summary>Nilled facts preserve cancellation, foreign authority and expiry.</summary>
    [Fact]public void Ownership()
    {
        var xml="<a xmlns:xsi='"+I+"' xsi:nil='true'/>";using var f=new Fixture(Schema(),xml);using var other=new Fixture(Schema(),xml);var local=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.False(f.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(local.Type,foreign.Type,default));using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>local.ReadTypedValue(c.Token));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>local.IsNilled);Assert.Throws<ObjectDisposedException>(()=>local.ReadTypedValue(default));
    }
    /// <summary>Legacy grammar and existing runtime capacity remain closed under nil requests.</summary>
    [Fact]public void LegacyAndCapacity()
    {
        Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(),new(256,256)).Outcome);var c=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:1024).Value!;Assert.Equal(NativeOutcome.Unsupported,c.Assess("<a xmlns:xsi='"+I+"' xsi:nil='"+new string(' ',253)+"true'/>").Outcome);Assert.Equal(NativeOutcome.Invalid,c.Assess("<a xmlns:xsi='"+I+"' xsi:nil='true'><child/></a>").Outcome);
    }
}
