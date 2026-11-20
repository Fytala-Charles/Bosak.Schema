// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Checks original anonymous simple-type facts and native atomic provenance.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks actual native anonymous facts independently of synthetic transport probes.</summary>
public sealed class NativeAnonymousRestrictionCoreTests
{
    private static string Schema(string family)=>$"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:complexType name='Box'><xs:sequence><xs:element name='v'><xs:simpleType><xs:restriction base='xs:{family}'/></xs:simpleType></xs:element><xs:element name='w'><xs:simpleType><xs:restriction base='xs:{family}'/></xs:simpleType></xs:element></xs:sequence></xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='Box'/><xs:element name='b' type='Box'/></xs:sequence></xs:complexType></xs:element></xs:schema>";
    private const string Xml="<r><a><v>12.50</v><w>12.50</w></a><b><v>12.50</v><w>12.50</w></b></r>";
    private static NativeSchemaProvider Provider(string family)=>new(NativeDecimalSchema.Compile(Schema(family),new(256,256),maxStringCharacters:1024).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider provider,string expression="$value")=>SchemaQuerySession.CompileXPath(expression,new(2,16,1,256,256),assessor:provider).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession session,NativeSchemaInput input)=>(NativeSchemaProvider.Assessment)session.Advisor!.Assess(new(input,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    /// <summary>Source definitions retain absent QNames and shared/distinct original component identity.</summary>
    /// <param name="family">Primitive storage family.</param>
    [Theory][InlineData("decimal")][InlineData("string")]
    public void OriginalAnonymousFacts_RetainIdentityAndTypedProvenance(string family)
    {
        var provider=Provider(family);using var session=Session(provider);using var input=NativeSchemaInput.Capture(Xml).Value!;var result=Assess(session,input);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);Assert.Equal(4,result.ScalarFacts.Length);
        Assert.Same(result.ScalarFacts[0].Type,result.ScalarFacts[2].Type);Assert.Same(result.ScalarFacts[0].Declaration,result.ScalarFacts[2].Declaration);Assert.NotSame(result.ScalarFacts[0].Type,result.ScalarFacts[1].Type);
        using var snapshot=session.OpenSnapshot(input,input,result.Origin).Value;
        foreach(var fact in result.ScalarFacts)
        {
            Assert.False(fact.Node.IsComplexType);Assert.False(fact.HasNoTypedValue);Assert.Null(fact.NativeType.Name);Assert.Empty(fact.NativeType.NamespaceUri);Assert.Null(fact.Node.SchemaTypeAnnotation);Assert.Null(fact.Node.SchemaElementDeclaration);Assert.NotSame(fact.Type,fact.Declaration);Assert.NotSame(provider.DecimalType,fact.Type);
            var atom=fact.Node.TypedValue;Assert.Equal(family=="string"?XdmValueKind.String:XdmValueKind.Decimal,atom.Kind);
            new NativeSessionRunner(session).Run([snapshot],op=>{var read=op.ReadTypedValue(snapshot,fact.ReadTypedValue(default).Value);Assert.Equal(SchemaResultKind.Supported,read.Kind);var value=op.Evaluate(new EvaluationContext().WithVariable("value",read.Value));Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(fact.Type,evidence!.DeclaredType);Assert.Same(fact.Type,evidence.ActualType);Assert.Same(result.Origin,evidence.OriginalAssessment);return 0;});
        }
        Assert.Equal(SchemaResultKind.Unsupported,provider.ResolveType(new("","anonymous"),default).Kind);
    }
    /// <summary>The native advisor retains its scalar-read profile instead of admitting general node queries.</summary>
    /// <param name="family">Decimal or string anonymous type.</param>
    [Theory][InlineData("decimal")][InlineData("string")]
    public void NativeAdvisor_PreservesScalarReadOnlyProfile(string family)
    {
        var provider=Provider(family);var result=SchemaQuerySession.CompileXPath("data($node)",new(2,16,1,256,256),assessor:provider);Assert.Equal(SchemaResultKind.Unsupported,result.Kind);
    }
    /// <summary>Foreign anonymous issuance is refused and closing the snapshot revokes provenance.</summary>
    /// <param name="family">Primitive storage family.</param>
    [Theory][InlineData("decimal")][InlineData("string")]
    public void OriginalAnonymousAuthority_RejectsForeignAndExpiredValues(string family)
    {
        var pa=Provider(family);var pb=Provider(family);using var a=Session(pa);using var b=Session(pb);using var ia=NativeSchemaInput.Capture(Xml).Value!;using var ib=NativeSchemaInput.Capture(Xml).Value!;var ar=Assess(a,ia);var br=Assess(b,ib);using var sa=a.OpenSnapshot(ia,ia,ar.Origin).Value;using var sb=b.OpenSnapshot(ib,ib,br.Origin).Value;
        var issued=ar.ScalarFacts[0].ReadTypedValue(default).Value;new NativeSessionRunner(b).Run([sb],op=>{Assert.Throws<ArgumentException>(()=>op.ReadTypedValue(sb,issued));return 0;});
        ISchemaAtomicProvenance? evidence=null;new NativeSessionRunner(a).Run([sa],op=>{var value=op.ReadTypedValue(sa,issued).Value;Assert.True(SchemaAtomics.TryGetProvenance(value,out evidence));return 0;});sa.Dispose();Assert.Throws<ObjectDisposedException>(()=>evidence!.ActualType);
    }
}
