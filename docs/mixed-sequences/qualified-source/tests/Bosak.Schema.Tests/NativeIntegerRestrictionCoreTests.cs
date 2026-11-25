// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Verifies original restricted integer facts, exact payloads and authority lifetime.
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

using System.Globalization;
using System.Numerics;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace Bosak.Schema.Tests;

/// <summary>Checks actual restricted integer identity independently of synthetic Core transport.</summary>
public sealed class NativeIntegerRestrictionCoreTests
{
    private static string Schema(bool named)
    {
        const string body="<xs:restriction base='xs:integer'><xs:totalDigits value='60'/></xs:restriction>";
        var global=named?"<xs:simpleType name='Count'>"+body+"</xs:simpleType>":"";
        var v=named?"<xs:element name='v' type='Count'/>":"<xs:element name='v'><xs:simpleType>"+body+"</xs:simpleType></xs:element>";
        return "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>"+global+"<xs:complexType name='Box'><xs:sequence>"+v+"<xs:element name='w'><xs:simpleType>"+body+"</xs:simpleType></xs:element></xs:sequence>"+(named?"<xs:attribute name='code' type='Count'/>":"")+"</xs:complexType><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='Box'/><xs:element name='b' type='Box'/></xs:sequence>"+(named?"<xs:attribute name='id' type='Count'/>":"")+"</xs:complexType></xs:element></xs:schema>";
    }
    private const string Number="123456789012345678901234567890123456789012345678901234567890";
    private static string Xml(bool named)=>"<r"+(named?" id='1'":"")+"><a"+(named?" code='1'":"")+"><v>"+Number+"</v><w>1</w></a><b"+(named?" code='2'":"")+"><v>2</v><w>1</w></b></r>";
    private static NativeSchemaProvider Provider(bool named)=>new(NativeDecimalSchema.Compile(Schema(named),new(256,0),maxStringCharacters:1024).Value!);
    private static SchemaQuerySession Session(NativeSchemaProvider p)=>SchemaQuerySession.CompileXPath("$value",new(2,32,1,256,0),assessor:p).Value;
    private static NativeSchemaProvider.Assessment Assess(SchemaQuerySession s,NativeSchemaInput i)=>(NativeSchemaProvider.Assessment)s.Advisor!.Assess(new(i,SchemaAssessmentMode.Strict,null,null,new Episode(),null),default);
    private sealed record Episode:ISchemaAssessmentEpisode
    {
        /// <inheritdoc/>
        public DateTimeOffset CurrentDateTime=>DateTimeOffset.UnixEpoch;
        /// <inheritdoc/>
        public int ImplicitTimezoneOffsetMinutes=>0;
    }
    /// <summary>Restricted integer payloads keep original named/anonymous handles, sharing and attribute identity.</summary>
    /// <param name="named">Selects the named restriction route.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void IntegerFacts_PreserveExactOriginalTypeAndOwner(bool named)
    {
        var p=Provider(named);using var s=Session(p);using var input=NativeSchemaInput.Capture(Xml(named)).Value!;var result=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Valid,result.Status);Assert.Equal(4,result.ScalarFacts.Length);var fact=result.ScalarFacts[0];Assert.Same(fact.Type,result.ScalarFacts[2].Type);Assert.Same(fact.Declaration,result.ScalarFacts[2].Declaration);Assert.NotSame(fact.Type,result.ScalarFacts[1].Type);Assert.NotSame(p.ResolveType(new("http://www.w3.org/2001/XMLSchema","integer"),default).Value,fact.Type);
        if(named){Assert.Equal(("","Count"),fact.Node.SchemaTypeAnnotation!.Value);Assert.Same(p.ResolveType(new("","Count"),default).Value,fact.Type);Assert.Same(fact.Type,result.AttributeFacts[0].Type);Assert.All(result.DescendantAttributeFacts,x=>Assert.Same(fact.Type,x.Type));Assert.Equal(new[]{0},result.DescendantAttributeFacts[0].OwnerElementPath);}
        else{Assert.Null(fact.Node.SchemaTypeAnnotation);Assert.Null(fact.NativeType.Name);Assert.Empty(fact.NativeType.NamespaceUri);}
        Assert.Equal(BigInteger.Parse(Number,CultureInfo.InvariantCulture),fact.Node.TypedValue.BigIntegerValue);using var snapshot=s.OpenSnapshot(input,input,result.Origin).Value;
        foreach(var item in result.ScalarFacts.Concat(result.AttributeFacts).Concat(result.DescendantAttributeFacts))new NativeSessionRunner(s).Run([snapshot],op=>{var value=op.ReadTypedValue(snapshot,item.ReadTypedValue(default).Value).Value;Assert.Equal(XdmValueKind.Integer,value.Kind);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(item.Type,evidence!.DeclaredType);Assert.Same(item.Type,evidence.ActualType);Assert.Same(result.Origin,evidence.OriginalAssessment);return 0;});
    }
    /// <summary>Original integer restriction values cannot transfer authority and expire with their snapshot.</summary>
    /// <param name="named">Selects named or anonymous identity.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void IntegerAuthority_RejectsForeignAndExpiredValues(bool named)
    {
        var pa=Provider(named);var pb=Provider(named);using var a=Session(pa);using var b=Session(pb);using var ia=NativeSchemaInput.Capture(Xml(named)).Value!;using var ib=NativeSchemaInput.Capture(Xml(named)).Value!;var ar=Assess(a,ia);var br=Assess(b,ib);using var sa=a.OpenSnapshot(ia,ia,ar.Origin).Value;using var sb=b.OpenSnapshot(ib,ib,br.Origin).Value;var issued=ar.ScalarFacts[0].ReadTypedValue(default).Value;
        new NativeSessionRunner(b).Run([sb],op=>{Assert.Throws<ArgumentException>(()=>op.ReadTypedValue(sb,issued));return 0;});ISchemaAtomicProvenance? evidence=null;new NativeSessionRunner(a).Run([sa],op=>{Assert.True(SchemaAtomics.TryGetProvenance(op.ReadTypedValue(sa,issued).Value,out evidence));return 0;});sa.Dispose();Assert.Throws<ObjectDisposedException>(()=>evidence!.ActualType);
    }
    /// <summary>Invalid restricted application attributes never publish partial facts.</summary>
    /// <param name="root">Selects root or descendant attribute failure.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void InvalidIntegerAttributes_DoNotPublishFacts(bool root)
    {
        var p=Provider(true);using var s=Session(p);var xml=Xml(true).Replace(root?"id='1'":"code='1'",root?"id='1.0'":"code='1.0'",StringComparison.Ordinal);using var input=NativeSchemaInput.Capture(xml).Value!;var result=Assess(s,input);Assert.Equal(SchemaAssessmentStatus.Invalid,result.Status);Assert.Empty(result.ElementFacts);Assert.Empty(result.AttributeFacts);Assert.False(input.IsAssessed);
    }
}
