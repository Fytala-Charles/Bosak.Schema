// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Owns actual native assessment episodes for isolated original-node data probes.
// SPECIAL NOTES        : Test-only prototype; no production facade or synthetic XML validation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 20-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 22-11-2026     | Adopt derived-integer expectations without changing original stimuli |
//                      | Charles Korthout | 0.3   | 24-11-2026     | Reconcile measured date outcomes in isolated test copy |
//                      | Charles Korthout | 0.4   | 24-11-2026     | Adopt qualified bounded native date transport |
//                      | Charles Korthout | 0.5   | 25-11-2026     | Verify admitted nil metadata and distinct supported empty typed sequences |
//                      | Charles Korthout | 0.6   | 25-11-2026     | Verify provided default/fixed values and empty augmentation refusal |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.SimpleRoots;
/// <summary>Verifies bounded native simple-root behavior and refusal boundaries.</summary>
public sealed class NativeSimpleRootBoundaryCoreTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static NativeSchemaSessionOptions Options=>new(256,256,16384,65536,1,32,1,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,0);
    private static string S(string content)=>$"<xs:schema xmlns:xs='{X}'>{content}</xs:schema>";
    private static string A(string name)=>$"<xs:element name='{name}'><xs:simpleType><xs:restriction base='xs:string'/></xs:simpleType></xs:element>";
    /// <summary>Global Root Ceiling.</summary>
    /// <param name="count">The count control input.</param>
    /// <param name="expected">The expected control input.</param>
    [Theory][InlineData(16,"Valid")][InlineData(17,"Unsupported")]
    public void GlobalRootCeiling(int count,string expected)
    {var schema=S(string.Concat(Enumerable.Range(0,count).Select(i=>$"<xs:element name='r{i}' type='xs:string'/>")));var result=NativeSchemaSession.Create(schema,Options);Assert.Equal(expected,result.Outcome.ToString());result.Value?.Dispose();}
    /// <summary>Combined Anonymous Budget.</summary>
    /// <param name="over">The over control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void CombinedAnonymousBudget(bool over)
    {var schema=S(string.Concat(Enumerable.Range(0,8).Select(i=>A("r"+i)))+"<xs:element name='box'><xs:complexType><xs:sequence>"+string.Concat(Enumerable.Range(0,over?9:8).Select(i=>A("v"+i)))+"</xs:sequence></xs:complexType></xs:element>");var result=NativeSchemaSession.Create(schema,Options);Assert.Equal(over?NativeSchemaOutcome.Unsupported:NativeSchemaOutcome.Valid,result.Outcome);if(over)Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(result.Diagnostics).UnsupportedReason);result.Value?.Dispose();}
    /// <summary>Namespaced Original Type.</summary>
    /// <param name="placement">The placement control input.</param>
    [Theory][InlineData("builtin")][InlineData("named")][InlineData("anonymous")]
    public void NamespacedOriginalType(string placement)
    {
        var definition=placement=="named"?"<xs:simpleType name='Code'><xs:restriction base='xs:string'/></xs:simpleType>":"";
        var root=placement=="anonymous"?A("r"):$"<xs:element name='r' type='{(placement=="named"?"t:Code":"xs:string")}'/>";
        var schema=$"<xs:schema xmlns:xs='{X}' xmlns:t='urn:r' targetNamespace='urn:r'>{definition}{root}</xs:schema>";
        using var f=new Fixture(schema,"<a:r xmlns:a='urn:r'> a </a:r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("urn:r",fact.NativeDeclaration.NamespaceUri);Assert.Equal(placement=="builtin"?X:placement=="named"?"urn:r":"",fact.NativeType.NamespaceUri);Assert.Equal(placement=="builtin"?"string":placement=="named"?"Code":null,fact.NativeType.Name);Assert.Equal(" a ",f.Read(f.Input.Root).StringValue);
        f.Run(op=>{Assert.True(SchemaAtomics.TryGetProvenance(f.Evaluate(op,f.Input.Root),out var origin));Assert.Same(fact.Type,origin!.DeclaredType);Assert.Same(fact.Type,origin.ActualType);return 0;});
    }
    /// <summary>Instance Selection.</summary>
    /// <param name="selected">The selected control input.</param>
    /// <param name="text">The text control input.</param>
    /// <param name="expected">The expected control input.</param>
    [Theory][InlineData("Code","ABC","Valid")][InlineData("Code","abc","Invalid")][InlineData("xs:integer","1","Invalid")][InlineData("missing","abc","Invalid")][InlineData("bad:t","abc","Invalid")][InlineData("xs:int","1","Invalid")]
    public void InstanceSelection(string selected,string text,string expected)
    {
        var schema=S("<xs:simpleType name='Code'><xs:restriction base='xs:string'><xs:pattern value='[A-Z]+'/></xs:restriction></xs:simpleType><xs:element name='r' type='xs:string'/>");
        using var f=new Fixture(schema,$"<r xmlns:xs='{X}' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='{selected}'>{text}</r>");Assert.Equal(expected,f.Assessment.Status.ToString());if(expected!="Valid"){Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);return;}
        var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;Assert.Equal("string",fact.NativeDeclaration.Type.Name);Assert.Equal("Code",fact.NativeType.Name);Assert.Same(fact.Declaration,f.Provider.ResolveElementDeclaration(new("","r"),default).Value);
        f.Run(op=>{Assert.True(SchemaAtomics.TryGetProvenance(f.Evaluate(op,f.Input.Root),out var origin));Assert.Same(fact.Type,origin!.DeclaredType);Assert.Same(fact.Type,origin.ActualType);return 0;});
    }
    /// <summary>Nested Catalog Order.</summary>
    /// <param name="reversed">The reversed control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void NestedCatalogOrder(bool reversed)
    {
        var scalar="<xs:element name='r' type='xs:string'/>";var nested="<xs:element name='n'><xs:complexType><xs:sequence><xs:element name='b'><xs:complexType><xs:sequence><xs:element name='v' type='xs:integer'/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element>";var schema=S(reversed?nested+scalar:scalar+nested);
        using var s=NativeSchemaSession.Create(schema,Options).Value!;using(var d=s.Assess("<r>abc</r>",Context).Value!){Assert.True(d.HasRootScalar);Assert.Equal(0,d.ScalarCount);Assert.Equal("abc",d.ReadRootScalar().StringValue);}using(var d=s.Assess("<n><b><v>7</v></b></n>",Context).Value!){Assert.False(d.HasRootScalar);Assert.Equal(1,d.ScalarCount);Assert.Equal(7,d.ReadScalar(0).IntegerValue);}
    }
    /// <summary>Excluded Declaration Attributes.</summary>
    /// <param name="attribute">The attribute control input.</param>
    [Theory][InlineData("default='abc'")][InlineData("fixed='abc'")][InlineData("nillable='true'")]
    public void ExcludedDeclarationAttributes(string attribute)
    {var result=NativeSchemaSession.Create(S($"<xs:element name='r' type='xs:string' {attribute}/>"),Options);if(attribute.StartsWith("nillable",StringComparison.Ordinal)){Assert.Equal(NativeSchemaOutcome.Valid,result.Outcome);using var session=result.Value!;using var document=session.Assess("<r xmlns:i='http://www.w3.org/2001/XMLSchema-instance' i:nil='true'/>",Context).Value!;Assert.True(document.IsRootNilled);Assert.Equal(NativeScalarKind.Empty,document.ReadRootScalar().Kind);}else{Assert.Equal(NativeSchemaOutcome.Valid,result.Outcome);using var session=result.Value!;using(var document=session.Assess("<r>abc</r>",Context).Value!){Assert.Equal("abc",document.ReadRootScalar().StringValue);Assert.False(document.IsRootNilled);}var other=session.Assess("<r>other</r>",Context);Assert.Equal(attribute.StartsWith("default",StringComparison.Ordinal)?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Invalid,other.Outcome);using(other.Value){if(other.Value is { } value)Assert.Equal("other",value.ReadRootScalar().StringValue);}var empty=session.Assess("<r/>",Context);Assert.Equal(NativeSchemaOutcome.Unsupported,empty.Outcome);Assert.Null(empty.Value);}}
    /// <summary>Other Builtin Types Stay Excluded.</summary>
    /// <param name="type">The type control input.</param>
    [Theory][InlineData("int")][InlineData("date")][InlineData("anyType")]
    public void OtherBuiltinTypesStayExcluded(string type)
    {var result=NativeSchemaSession.Create(S($"<xs:element name='r' type='xs:{type}'/>"),Options);Assert.Equal(type is "int" or "date"?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Unsupported,result.Outcome);result.Value?.Dispose();}
    /// <summary>Eager Unused Pattern.</summary>
    /// <param name="pattern">The pattern control input.</param>
    /// <param name="expected">The expected control input.</param>
    [Theory][InlineData("(","Invalid")][InlineData("\\p{L}","Unsupported")][InlineData("a{17}","Unsupported")]
    public void EagerUnusedPattern(string pattern,string expected)
    {var schema=S($"<xs:simpleType name='Unused'><xs:restriction base='xs:string'><xs:pattern value='{pattern}'/></xs:restriction></xs:simpleType><xs:element name='r' type='xs:string'/>");var result=NativeSchemaSession.Create(schema,Options);Assert.Equal(expected,result.Outcome.ToString());Assert.Null(result.Value);}
    /// <summary>Root Pattern Work Refusal And Recovery.</summary>
    /// <param name="data">The data control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void RootPatternWorkRefusalAndRecovery(bool data)
    {var wide="("+string.Join("|",Enumerable.Repeat("a",62))+")*";var schema=S($"<xs:simpleType name='W'><xs:restriction base='xs:string'><xs:pattern value='{wide}'/></xs:restriction></xs:simpleType><xs:element name='r' type='W'/>");var xml="<r>"+new string('a',16000)+"</r>";
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;var a=s.Assess(xml,Context);Assert.Equal(NativeSchemaOutcome.Unsupported,a.Outcome);Assert.Null(a.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(a.Diagnostics).UnsupportedReason);using var d=s.Assess("<r>a</r>",Context).Value!;Assert.Equal("a",d.ReadElement([]).StringValue);}
        else{using var s=NativeSchemaSession.Create(schema,Options).Value!;var a=s.Assess(xml,Context);Assert.Equal(NativeSchemaOutcome.Unsupported,a.Outcome);Assert.Null(a.Value);Assert.Equal(NativeSchemaUnsupportedReason.RepresentationCapacity,Assert.Single(a.Diagnostics).UnsupportedReason);using var d=s.Assess("<r>a</r>",Context).Value!;Assert.Equal("a",d.ReadRootScalar().StringValue);}
    }
    /// <summary>String Value Ceiling.</summary>
    /// <param name="max">The max control input.</param>
    /// <param name="text">The text control input.</param>
    /// <param name="expected">The expected control input.</param>
    [Theory][InlineData(2,"aa","Valid")][InlineData(2,"aaa","Unsupported")]
    public void StringValueCeiling(int max,string text,string expected)
    {var opts=new NativeSchemaSessionOptions(256,256,max,65536,1,32,1,256,256);using var s=NativeSchemaSession.Create(S("<xs:element name='r' type='xs:string'/>"),opts).Value!;var result=s.Assess($"<r>{text}</r>",Context);Assert.Equal(expected,result.Outcome.ToString());result.Value?.Dispose();}

    /// <summary>Original As050Admission.</summary>
    /// <param name="data">The data control input.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void OriginalAs050Admission(bool data)
    {using var doc=System.Text.Json.JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"TestData/SimpleRoots/AS-050-original.json")));var row=doc.RootElement;Assert.Equal("AS-050",row.GetProperty("id").GetString());Assert.Equal("Unsupported",row.GetProperty("expectedCompile").GetString());var schema=row.GetProperty("schema").GetString()!;var xml=row.GetProperty("instance").GetString()!;
        if(data){using var session=NativeSchemaNodeDataSession.Create(schema,Options).Value!;using var d=session.Assess(xml,Context).Value!;Assert.Equal("1",d.ReadElement([]).DecimalValue.ToInvariantString(256));}
        else{using var session=NativeSchemaSession.Create(schema,Options).Value!;using var d=session.Assess(xml,Context).Value!;Assert.Equal(0,d.ScalarCount);Assert.True(d.HasRootScalar);Assert.Equal("1",d.ReadRootScalar().DecimalValue.ToInvariantString(256));}
    }
}
