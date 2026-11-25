// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Verifies relative namespace names and independent schema form declarations.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Globalization;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Exercises real native component and original-episode authority on public Core.</summary>
public sealed class NativeNamespaceFormCoreTests
{
    private const string Xs="http://www.w3.org/2001/XMLSchema";
    private const string Xsi="http://www.w3.org/2001/XMLSchema-instance";
    private static NativeSchemaSessionOptions Options => new(256,256,1024,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context => new(DateTimeOffset.UnixEpoch,0);
    private static string Text(NativeScalarValue value)=>value.Kind switch
    {NativeScalarKind.String=>value.StringValue,NativeScalarKind.Boolean=>value.BooleanValue?"true":"false",NativeScalarKind.Integer=>value.IntegerValue.ToString(CultureInfo.InvariantCulture),_=>value.DecimalValue.ToInvariantString(256)};
    private static (string Schema,string Xml) Payload(string ns="rel/orders",string family="decimal",string form="unqualified")
    {
        var facet=family=="string"?"<xs:maxLength value='1'/>":"<xs:maxInclusive value='10'/>";
        var schema=$"<xs:schema xmlns:xs='{Xs}' xmlns:t='{ns}' targetNamespace='{ns}' elementFormDefault='qualified' attributeFormDefault='{form}'><xs:simpleType name='T'><xs:restriction base='xs:{family}'>{facet}</xs:restriction></xs:simpleType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:{family}'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        var xml=$"<q:r xmlns:q='{ns}' xmlns:i='{Xsi}'><q:v i:type='q:T'>{(family=="string"?"A":"9")}</q:v></q:r>";
        return (schema,xml);
    }
    /// <summary>Gets exact namespace strings and existing scalar families.</summary>
    /// <returns>Relative namespace and admitted scalar family.</returns>
    public static IEnumerable<object[]> Names()=>from ns in new[]{"rel/orders","../orders","#orders","orders%2Fv1","a/../b"} from family in new[]{"decimal","string","integer"} select new object[]{ns,family};
    /// <summary>Selected relative components retain exact names and original authority through signed Core.</summary>
    /// <param name="ns">Namespace spelling.</param>
    /// <param name="family">Scalar family.</param>
    [Theory][MemberData(nameof(Names))]
    public void RelativeComponents_AtomizeWithOriginalAuthority(string ns,string family)
    {
        var p=Payload(ns,family);using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var node=f.Element([0]);var fact=f.Input.FactsFor((NativeSessionNode)node)!;
        Assert.Equal(ns,fact.NativeDeclaration.NamespaceUri);Assert.Equal(ns,fact.NativeType.NamespaceUri);Assert.Equal((ns,"T"),node.SchemaTypeAnnotation!.Value);var component=f.Provider.ResolveType(new(ns,"T"),default);Assert.Equal(SchemaResultKind.Supported,component.Kind);Assert.Same(component.Value,fact.Type);Assert.True(f.Session.Advisor!.OwnsNode(node,fact,f.Input,f.Assessment.Origin));
        f.Run(op=>{var value=f.Evaluate(op,node);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(fact.Type,evidence.ActualType);Assert.Same(f.Assessment.Origin,evidence.OriginalAssessment);return 0;});
        Assert.Equal(family=="string"?"A":"9",Text(f.Read(node)));
    }
    /// <summary>Namespace names remain exact strings without URI base/dot/escape/case normalization.</summary>
    /// <param name="ns">Declared namespace.</param>
    /// <param name="other">Different instance namespace.</param>
    [Theory][InlineData("../orders","https://example.org/orders")][InlineData("orders%2Fv1","orders/v1")][InlineData("Orders","orders")][InlineData("a/../b","b")]
    public void DistinctSpellings_AreNotInterchangeable(string ns,string other)
    {
        var p=Payload(ns);using var f=new Fixture(p.Schema,p.Xml.Replace(ns,other,StringComparison.Ordinal));Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);Assert.False(f.Input.IsAssessed);Assert.Null(f.Snapshot);
    }
    /// <summary>Form defaults are legal without any application attribute declarations.</summary>
    /// <param name="form">Legal default.</param>
    [Theory][InlineData("qualified")][InlineData("unqualified")]
    public void FormWithoutUses_IsAdmitted(string form)
    {
        var p=Payload("urn:forms",form:form);using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Empty(f.Assessment.AttributeFacts);Assert.Empty(f.Assessment.DescendantAttributeFacts);Assert.Equal("9",Text(f.Read(f.Element([0]))));
    }
    /// <summary>Invalid form values remain an explicit compile refusal.</summary>
    [Fact]
    public void InvalidForm_IsNotIgnored()
    {var p=Payload(form:"other");var result=NativeDecimalSchema.Compile(p.Schema,new(256,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Unsupported,result.Outcome);Assert.Equal(NativeDecimalUnsupportedReason.Namespace,result.UnsupportedReason);}
    /// <summary>Legacy decimal-only compilation retains both prior exclusions.</summary>
    /// <param name="relative">Selects the relative-name exclusion.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void LegacyProfile_RetainsItsLimits(bool relative)
    {
        var p=Payload(relative?"rel/orders":"urn:forms");var schema=relative?p.Schema.Replace(" attributeFormDefault='unqualified'","",StringComparison.Ordinal):p.Schema;var result=NativeDecimalSchema.Compile(schema,new(256,256));Assert.Equal(NativeOutcome.Unsupported,result.Outcome);Assert.Equal(NativeDecimalUnsupportedReason.Namespace,result.UnsupportedReason);if(relative)Assert.Equal("Only absolute namespace URIs without whitespace are admitted.",result.Diagnostic);
    }
    /// <summary>Explicit assertion namespace strings retain their absolute-only policy.</summary>
    [Fact]
    public void ExplicitRelativeAssertionDefault_StillRefuses()
    {
        var p=Payload();var result=NativeDecimalSchema.Compile(p.Schema.Replace("<xs:schema ","<xs:schema xpathDefaultNamespace='rel/orders' ",StringComparison.Ordinal),new(256,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Unsupported,result.Outcome);Assert.Equal(NativeDecimalUnsupportedReason.Namespace,result.UnsupportedReason);Assert.Equal("Only absolute namespace URIs without whitespace are admitted.",result.Diagnostic);
    }
    /// <summary>Symbolic target binding uses the captured relative namespace without resolution.</summary>
    [Fact]
    public void SymbolicTargetAssertion_UsesExactRelativeName()
    {
        var p=Payload();p.Schema=p.Schema.Replace("<xs:schema ","<xs:schema xpathDefaultNamespace='##targetNamespace' ",StringComparison.Ordinal).Replace("</xs:sequence>","</xs:sequence><xs:assert test='v eq 9'/>",StringComparison.Ordinal);using var f=new Fixture(p.Schema,p.Xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal("9",Text(f.Read(f.Element([0]))));
    }
    /// <summary>Equal source spellings do not transfer original node authority between sessions.</summary>
    [Fact]
    public void ForeignSession_StillRefusesOriginalNode()
    {
        var p=Payload();using var a=new Fixture(p.Schema,p.Xml);using var b=new Fixture(p.Schema,p.Xml);var node=a.Element([0]);var fact=a.Input.FactsFor((NativeSessionNode)node)!;Assert.False(b.Session.Advisor!.OwnsNode(node,fact,a.Input,a.Assessment.Origin));Assert.Throws<ArgumentException>(()=>b.Run(op=>b.Evaluate(op,node)));
    }
    /// <summary>Both public owned facades export and independently reassess relative namespace identity.</summary>
    /// <param name="data">Selects original-node data.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void PublicFacades_ExportReassessAndDetach(bool data)
    {
        var p=Payload();NativeScalarValue detached;
        if(data){using var s=NativeSchemaNodeDataSession.Create(p.Schema,Options).Value!;using var d=s.Assess(p.Xml,Context).Value!;detached=d.ReadElement([0]);var xml=d.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(p.Xml),XElement.Parse(xml)));d.Dispose();using var again=s.Assess(xml,Context).Value!;Assert.Equal("9",Text(again.ReadElement([0])));}
        else{using var s=NativeSchemaSession.Create(p.Schema,Options).Value!;using var d=s.Assess(p.Xml,Context).Value!;detached=d.ReadScalar(0);var xml=d.ExportXml();Assert.True(XNode.DeepEquals(XElement.Parse(p.Xml),XElement.Parse(xml)));d.Dispose();using var again=s.Assess(xml,Context).Value!;Assert.Equal("9",Text(again.ReadScalar(0)));}
        Assert.Equal("9",Text(detached));
    }
    /// <summary>Relative Boolean declarations preserve actual values and reject an invalid lexical form.</summary>
    /// <param name="invalid">Selects an invalid Boolean lexical value.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void RelativeBoolean_UsesActualTypedFacts(bool invalid)
    {
        const string ns="ElemDecl/typeDef";
        var schema=$"<xs:schema xmlns:xs='{Xs}' targetNamespace='{ns}'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:boolean'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
        using var f=new Fixture(schema,$"<q:r xmlns:q='{ns}'><v>{(invalid?"TRUE":"false")}</v></q:r>");Assert.Equal(invalid?SchemaAssessmentStatus.Invalid:SchemaAssessmentStatus.Valid,f.Assessment.Status);
        if(invalid){Assert.Empty(f.Assessment.Facts);Assert.Null(f.Snapshot);Assert.False(f.Input.IsAssessed);}
        else{Assert.Equal("false",Text(f.Read(f.Element([0]))));Assert.False(f.Read(f.Element([0])).BooleanValue);}
    }

}
