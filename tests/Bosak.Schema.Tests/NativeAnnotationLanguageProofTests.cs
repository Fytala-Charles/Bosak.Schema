// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Proves opaque documentation language leaves native component and value authority intact.
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
namespace Bosak.Schema.Tests.AnnotationLanguage;
/// <summary>Checks language annotation admission without changing assessment authority.</summary>
public sealed class NativeAnnotationLanguageProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Annotation(string lang="en",string payload="human")=>"<xs:annotation><xs:documentation xml:lang='"+lang+"'>"+payload+"</xs:documentation></xs:annotation>";
    private static string Schema(string profile="decimal",string annotation="")
    {
        var definitions=profile=="list"?"<xs:simpleType name='T'><xs:list itemType='xs:decimal'/></xs:simpleType>":profile=="decimal"?"<xs:simpleType name='T'><xs:restriction base='xs:decimal'/></xs:simpleType>":"";
        var declaration=profile is "decimal" or "list"?"<xs:element name='a' type='T'/>":profile=="nil"?"<xs:element name='a' nillable='true'/>":"<xs:element name='a'><xs:complexType><xs:sequence><xs:element name='v' type='xs:decimal'/></xs:sequence></xs:complexType></xs:element>";
        return "<xs:schema xmlns:xs='"+X+"'>"+annotation+definitions+declaration+"</xs:schema>";
    }
    /// <summary>Existing scalar, list, nil and structural facts retain original authority and values.</summary>
    /// <param name="profile">Existing value family.</param>
    [Theory][InlineData("decimal")][InlineData("list")][InlineData("nil")][InlineData("sequence")]
    public void OriginalAuthority(string profile)
    {
        var xml=profile=="nil"?"<a xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true'/>":profile=="sequence"?"<a><v>01.20</v></a>":"<a>"+(profile=="list"?"01.20 2":"01.20")+"</a>";using var f=new Fixture(Schema(profile,Annotation(" en-US ")),xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var root=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Same(f.Provider.ResolveElementDeclaration(new("","a"),default).Value,root.Declaration);Assert.True(f.Provider.Owns(root.Type));if(profile=="sequence"){Assert.True(root.HasNoTypedValue);Assert.Equal("1.2",f.Read(f.Element([0])).DecimalValue.ToInvariantString(256));return;}var atoms=root.ReadTypedValue(default).Value.AtomicItems.ToArray();Assert.Equal(profile=="nil"?0:profile=="list"?2:1,atoms.Length);Assert.Equal(profile=="nil",root.IsNilled);if(profile=="nil"){Assert.True(root.NativeType.IsAnyType);Assert.Same(f.Provider.ResolveType(new(X,"anyType"),default).Value,root.Type);}else{Assert.Equal("T",root.NativeType.Name);Assert.All(atoms,atom=>Assert.Same(root.Type,atom.TypeEvidence.DeclaredType));Assert.Equal("1.2",atoms[0].Value.ExactDecimalValue.ToInvariantString(256));if(profile=="list")Assert.Equal("2",atoms[1].Value.ExactDecimalValue.ToInvariantString(256));}
    }
    /// <summary>Payload descendants remain opaque even when they look like malformed language or schema syntax.</summary>
    /// <param name="payload">Opaque application content.</param>
    [Theory]
    [InlineData("<xs:documentation xml:lang='bad_value'><xs:element name='fake' nillable='bad'/></xs:documentation>")]
    [InlineData("<xs:annotation id='dup'><xs:annotation id='dup'/><xs:simpleType name='Fake'/></xs:annotation>")]
    [InlineData("<foreign xmlns='urn:payload' xml:lang='bad_value'>raw</foreign>")]
    public void OpaquePayload(string payload)
    {
        using var f=new Fixture(Schema(annotation:Annotation("nl",payload)),"<a>01.20</a>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal("1.2",f.Read(f.Element([])).DecimalValue.ToInvariantString(256));Assert.NotEqual(SchemaResultKind.Supported,f.Provider.ResolveType(new("","Fake"),default).Kind);Assert.NotEqual(SchemaResultKind.Supported,f.Provider.ResolveElementDeclaration(new("","fake"),default).Kind);
    }
    /// <summary>Documentation metadata neither creates components nor consumes type/declaration budgets.</summary>
    [Fact]public void ComponentCatalog()
    {
        var plain=NativeDecimalSchema.Compile(Schema(),new(256,256),maxStringCharacters:1024).Value!;var tagged=NativeDecimalSchema.Compile(Schema(annotation:Annotation()),new(256,256),maxStringCharacters:1024).Value!;Assert.Equal(plain.NamedTypes.Select(t=>(t.NamespaceUri,t.Name)),tagged.NamedTypes.Select(t=>(t.NamespaceUri,t.Name)));Assert.Equal(plain.Roots.Select(r=>r.Root.ExpandedName),tagged.Roots.Select(r=>r.Root.ExpandedName));Assert.Equal(NativeOutcome.Valid,tagged.Assess("<a>1</a>").Outcome);Assert.Equal(NativeOutcome.Invalid,tagged.Assess("<a>bad</a>").Outcome);
    }
    /// <summary>The union accepts exact empty and collapsed language values, but rejects whitespace-only or bad unused metadata.</summary>
    [Fact]public void EagerLanguageValidation()
    {
        foreach(var value in new[]{""," en-US ","zz-ZZ"})Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(annotation:Annotation(value)),new(256,256),maxStringCharacters:1024).Outcome);foreach(var value in new[]{" ","bad_value","123"}){var source=Schema().Replace("</xs:schema>","<xs:simpleType name='Unused'>"+Annotation(value)+"<xs:restriction base='xs:string'/></xs:simpleType></xs:schema>");var result=NativeDecimalSchema.Compile(source,new(256,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Invalid,result.Outcome);Assert.Null(result.Value);}
    }
    /// <summary>Language metadata preserves the original annotation wrapper ceiling.</summary>
    [Fact]public void AnnotationCapacity()
    {
        Assert.Equal(NativeOutcome.Valid,NativeDecimalSchema.Compile(Schema(annotation:string.Concat(Enumerable.Repeat(Annotation(),16))),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(annotation:string.Concat(Enumerable.Repeat(Annotation(),17))),new(256,256),maxStringCharacters:1024).Outcome);Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(annotation:Annotation(new string('a',257))),new(256,256),maxStringCharacters:1024).Outcome);
    }
    /// <summary>Concrete facts retain cancellation, foreign ownership, detached values and expiry.</summary>
    [Fact]public void OwnershipAndLifetime()
    {
        using var f=new Fixture(Schema(annotation:Annotation()),"<a>01.20</a>");using var other=new Fixture(Schema(annotation:Annotation("nl")),"<a>01.20</a>");var local=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;var foreign=other.Input.FactsFor((NativeSessionNode)other.Element([]))!;Assert.False(f.Provider.Owns(foreign.Type));Assert.Throws<ArgumentException>(()=>f.Provider.IsSameComponent(local.Type,foreign.Type,default));using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>local.ReadTypedValue(c.Token));var detached=f.Read(f.Element([]));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>local.Declaration);Assert.Equal("1.2",detached.DecimalValue.ToInvariantString(256));
    }
    /// <summary>Legacy annotation grammar stays closed and cancellation retains precedence.</summary>
    [Fact]public void LegacyAndCancellation()
    {
        Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema(annotation:Annotation()),new(256,256)).Outcome);using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>NativeDecimalSchema.Compile(Schema(annotation:Annotation()),new(256,256),c.Token,1024));
    }
}
