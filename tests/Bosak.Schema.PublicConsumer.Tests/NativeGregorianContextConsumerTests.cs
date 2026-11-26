// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies detached Gregorian components and closed imported types.
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
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
/// <summary>Verifies exact nullable fragment components, imports, read guards and original XML.</summary>
public sealed class NativeGregorianContextConsumerTests
{
    private static string Lexical(string kind)=>kind switch {"gYear"=>"-0000","gYearMonth"=>"10000-02","gMonth"=>"--02","gMonthDay"=>"--02-29",_=>"---31"};
    private static NativeSchemaSessionOptions Options=>new(1,0,256,65536,1,256,16,256,256);
    private static NativeSchemaAssessmentContext Context=>new(DateTimeOffset.UnixEpoch,-840);
    private static void Check(string kind,NativeScalarValue value)
    {
        Assert.Equal(Enum.Parse<NativeScalarKind>("G"+kind[1..]),value.Kind);Assert.Equal(Lexical(kind)+"Z",value.GregorianValue);Assert.Equal(0,value.GregorianTimezoneOffsetMinutes);
        Assert.Equal(kind=="gYear"?0L:kind=="gYearMonth"?10000L:(long?)null,value.GregorianYear);Assert.Equal(kind is "gYearMonth" or "gMonth" or "gMonthDay"?2:(int?)null,value.GregorianMonth);Assert.Equal(kind=="gMonthDay"?29:kind=="gDay"?31:(int?)null,value.GregorianDay);
        Assert.Throws<InvalidOperationException>(()=>value.StringValue);Assert.Throws<InvalidOperationException>(()=>value.DecimalValue);Assert.Throws<InvalidOperationException>(()=>value.IntegerValue);Assert.Throws<InvalidOperationException>(()=>value.BooleanValue);Assert.Throws<InvalidOperationException>(()=>value.FloatValue);Assert.Throws<InvalidOperationException>(()=>value.DoubleValue);Assert.Throws<InvalidOperationException>(()=>value.UriValue);Assert.Throws<InvalidOperationException>(()=>value.DateValue);Assert.Throws<InvalidOperationException>(()=>value.CalendarValue);Assert.Throws<InvalidOperationException>(()=>value.DurationValue);Assert.Throws<InvalidOperationException>(()=>value.BinaryValue);Assert.Throws<InvalidOperationException>(()=>value.QNameValue);
    }
    /// <summary>Checks imported primitive identity and exact detached nullable components without external I/O.</summary>
    /// <param name="kind">Primitive.</param><param name="data">Facade selector.</param>
    [Theory][InlineData("gYear",false)][InlineData("gYear",true)][InlineData("gYearMonth",false)][InlineData("gYearMonth",true)][InlineData("gMonth",false)][InlineData("gMonth",true)][InlineData("gMonthDay",false)][InlineData("gMonthDay",true)][InlineData("gDay",false)][InlineData("gDay",true)]
    public void Imported(string kind,bool data)
    {
        const string root="https://example.test/root.xsd";var catalog=new Dictionary<string,string>{[root]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:f='urn:foreign'><xs:import namespace='urn:foreign' schemaLocation='types.xsd'/><xs:element name='r' type='f:T'/></xs:schema>",["https://example.test/types.xsd"]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:foreign'><xs:simpleType name='T'><xs:restriction base='xs:"+kind+"'/></xs:simpleType></xs:schema>"};NativeScalarValue value;
        if(data){using var s=NativeSchemaNodeDataSession.Create(root,catalog,Options).Value!;using var d=s.Assess("<r>"+Lexical(kind)+"Z</r>",Context).Value!;value=d.ReadElement([]);}
        else{using var s=NativeSchemaSession.Create(root,catalog,Options).Value!;using var d=s.Assess("<r>"+Lexical(kind)+"Z</r>",Context).Value!;value=d.ReadRootScalar();}Check(kind,value);
    }
    /// <summary>Preserves original lexical XML and recovers an owner for exported-input re-assessment.</summary>
    /// <param name="kind">Primitive.</param><param name="data">Facade selector.</param>
    [Theory][InlineData("gYear",false)][InlineData("gYear",true)][InlineData("gYearMonth",false)][InlineData("gYearMonth",true)][InlineData("gMonth",false)][InlineData("gMonth",true)][InlineData("gMonthDay",false)][InlineData("gMonthDay",true)][InlineData("gDay",false)][InlineData("gDay",true)]
    public void ExportReopen(string kind,bool data)
    {
        var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:"+kind+"'/></xs:schema>";var xml="<r> "+Lexical(kind)+"Z </r>";NativeScalarValue detached;
        Assert.Equal(NativeDecimalOutcome.Unsupported,NativeDecimalSession.Create(schema,new(256,256,1,256,16,256,256)).Outcome);
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;using var d=s.Assess(xml,Context).Value!;detached=d.ReadElement([]);var exported=d.ExportXml();Assert.Equal(xml,exported);d.Dispose();using var reopened=s.Assess(exported,Context).Value!;Check(kind,reopened.ReadElement([]));}
        else{using var s=NativeSchemaSession.Create(schema,Options).Value!;using var d=s.Assess(xml,Context).Value!;detached=d.ReadRootScalar();var exported=d.ExportXml();Assert.Equal(xml,exported);d.Dispose();using var reopened=s.Assess(exported,Context).Value!;Check(kind,reopened.ReadRootScalar());}Check(kind,detached);
    }
    /// <summary>Refuses fabricated fragment components on other families.</summary>
    /// <param name="data">Facade selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void WrongFamily(bool data)
    {
        const string schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r' type='xs:string'/></xs:schema>";NativeScalarValue value;
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;using var d=s.Assess("<r>2000</r>",Context).Value!;value=d.ReadElement([]);}
        else{using var s=NativeSchemaSession.Create(schema,Options).Value!;using var d=s.Assess("<r>2000</r>",Context).Value!;value=d.ReadRootScalar();}
        Assert.Throws<InvalidOperationException>(()=>value.GregorianYear);Assert.Throws<InvalidOperationException>(()=>value.GregorianMonth);Assert.Throws<InvalidOperationException>(()=>value.GregorianDay);Assert.Throws<InvalidOperationException>(()=>value.GregorianTimezoneOffsetMinutes);Assert.Throws<InvalidOperationException>(()=>value.GregorianValue);
    }
    /// <summary>Distinguishes same-local imported types across two original namespaces.</summary>
    /// <param name="data">Facade selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void SameLocalNames(bool data)
    {
        const string root="https://example.test/root.xsd";var catalog=new Dictionary<string,string>{[root]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:a='urn:a' xmlns:b='urn:b'><xs:import namespace='urn:a' schemaLocation='a.xsd'/><xs:import namespace='urn:b' schemaLocation='b.xsd'/><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='a:T'/><xs:element name='b' type='b:T'/></xs:sequence></xs:complexType></xs:element></xs:schema>",["https://example.test/a.xsd"]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:a'><xs:simpleType name='T'><xs:restriction base='xs:gYear'/></xs:simpleType></xs:schema>",["https://example.test/b.xsd"]="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' targetNamespace='urn:b'><xs:simpleType name='T'><xs:restriction base='xs:gMonth'/></xs:simpleType></xs:schema>"};NativeScalarValue a,b;
        if(data){using var s=NativeSchemaNodeDataSession.Create(root,catalog,Options).Value!;using var d=s.Assess("<r><a>2000</a><b>--02</b></r>",Context).Value!;a=d.ReadElement([0]);b=d.ReadElement([1]);}
        else{using var s=NativeSchemaSession.Create(root,catalog,Options).Value!;using var d=s.Assess("<r><a>2000</a><b>--02</b></r>",Context).Value!;a=d.ReadScalar(0);b=d.ReadScalar(1);}
        Assert.Equal(NativeScalarKind.GYear,a.Kind);Assert.Equal(NativeScalarKind.GMonth,b.Kind);Assert.Equal(2000L,a.GregorianYear);Assert.Null(b.GregorianYear);Assert.Null(a.GregorianMonth);Assert.Equal(2,b.GregorianMonth);
    }
    /// <summary>Qualifies complex restrictions without confusing governing and detached atomic families.</summary>
    /// <param name="kind">Primitive.</param><param name="data">Facade selector.</param>
    [Theory][InlineData("gYear",false)][InlineData("gYear",true)][InlineData("gYearMonth",false)][InlineData("gYearMonth",true)][InlineData("gMonth",false)][InlineData("gMonth",true)][InlineData("gMonthDay",false)][InlineData("gMonthDay",true)][InlineData("gDay",false)][InlineData("gDay",true)]
    public void ComplexRestriction(string kind,bool data)
    {
        var lexical=Lexical(kind)+"Z";var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:complexType name='B'><xs:simpleContent><xs:extension base='xs:"+kind+"'/></xs:simpleContent></xs:complexType><xs:complexType name='T'><xs:simpleContent><xs:restriction base='B'><xs:enumeration value='"+lexical+"'/></xs:restriction></xs:simpleContent></xs:complexType><xs:element name='r' type='T'/></xs:schema>";
        if(data){using var s=NativeSchemaNodeDataSession.Create(schema,Options).Value!;using var d=s.Assess("<r>"+lexical+"</r>",Context).Value!;Check(kind,d.ReadElement([]));d.Dispose();Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>bad</r>",Context).Outcome);}
        else{using var s=NativeSchemaSession.Create(schema,Options).Value!;using var d=s.Assess("<r>"+lexical+"</r>",Context).Value!;Check(kind,d.ReadRootScalar());d.Dispose();Assert.Equal(NativeSchemaOutcome.Invalid,s.Assess("<r>bad</r>",Context).Outcome);}
    }
}
