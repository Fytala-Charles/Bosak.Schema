// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Verifies the packaged unsignedLong root example through both public facades.
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
//                      | Charles Korthout | 0.2   | 22-11-2026     | Verify packaged simple-root reads, export and whole-literal negative controls |
//                      | Charles Korthout | 0.3   | 22-11-2026     | Verify exact derived integer root, export and bounds |
//                      | Charles Korthout | 0.4   | 23-11-2026     | Verify exact packaged all example and optionality controls |
//                      | Charles Korthout | 0.5   | 24-11-2026     | Verify packaged integer enumeration and sibling facet semantics |
//                      | Charles Korthout | 0.2   | 24-11-2026     | Exercise exact packaged date examples and detached lifetime |
//                      | Charles Korthout | 0.3   | 24-11-2026     | Qualify exact packaged date simple-content example |
//                      | Charles Korthout | 0.6   | 24-11-2026     | Qualify exact packaged date enumeration example |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using System.Numerics;
using System.Globalization;
using System.Xml.Linq;
internal static class Program
{
    private static void Check(bool value){if(!value)throw new InvalidOperationException("Packaged counter example assertion failed.");}
    private static void AllExample()
    {
        string schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-all.xsd")),xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-all.xml"));
        var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);NativeScalarValue detached;
        using(var s=NativeSchemaSession.Create(schema,options).Value!)
        {
            using(var d=s.Assess(xml,context).Value!){Check(!d.HasRootScalar&&d.ScalarCount==2);Check(d.ReadScalar(0).IntegerValue==7);Check(d.ReadScalar(1).StringValue=="");detached=d.ReadScalar(0);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}
            using(var d=s.Assess("<r/>",context).Value!){Check(d.ScalarCount==0&&!d.HasRootScalar);}
            Check(s.Assess("<r><b>7</b></r>",context).Outcome==NativeSchemaOutcome.Invalid);
            Check(s.Assess("<r><b>7</b><b>8</b><a/></r>",context).Outcome==NativeSchemaOutcome.Invalid);
        }
        Check(detached.IntegerValue==7);
        using(var s=NativeSchemaNodeDataSession.Create(schema,options).Value!)
        {
            using(var d=s.Assess(xml,context).Value!){Check(d.ReadElement([0]).IntegerValue==7);Check(d.ReadElement([1]).StringValue=="");Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}
            using(var d=s.Assess("<r/>",context).Value!){Check(XNode.DeepEquals(XElement.Parse("<r/>"),XElement.Parse(d.ExportXml())));}
            Check(s.Assess("<r><b>7</b></r>",context).Outcome==NativeSchemaOutcome.Invalid);
        }
        Console.WriteLine("Exact packaged all example passes both facades, physical order, singleton empty string, optional empty group, invalid missing/duplicate members, export and detached lifetime.");
    }
    private static void EnumerationExample()
    {
        string schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-enum.xsd")),xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-enum.xml"));
        var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);NativeScalarValue detached;
        using(var s=NativeSchemaSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){Check(d.HasRootScalar&&d.ScalarCount==0);detached=d.ReadRootScalar();Check(detached.IntegerValue==1);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<r>2</r>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<r>0</r>",context).Outcome==NativeSchemaOutcome.Invalid);}
        Check(detached.IntegerValue==1);
        using(var s=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){Check(d.ReadElement([]).IntegerValue==1);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<r>2</r>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<r>0</r>",context).Outcome==NativeSchemaOutcome.Invalid);}
        Console.WriteLine("Exact packaged integer enumeration passes aliases, sibling facets, invalid membership, both facades, export and detached lifetime.");
    }
    private static void DateExample()
    {
        string schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-date.xsd")),xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-date.xml"));var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,120);NativeDateValue detached;
        using(var s=NativeSchemaSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){var v=d.ReadRootScalar();Check(v.Kind==NativeScalarKind.Date&&d.HasRootScalar&&d.ScalarCount==0);detached=v.DateValue;Check(detached.Year==0&&detached.Month==2&&detached.Day==29&&detached.TimezoneOffsetMinutes==840);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<date>2023-02-29</date>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<date>-9223372036854775808-01-01</date>",context).Outcome==NativeSchemaOutcome.Unsupported);using var d2=s.Assess("<date>10000-01-01</date>",context).Value!;Check(d2.ReadRootScalar().DateValue.TimezoneOffsetMinutes is null);}Check(detached.Year==0);
        using(var s=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){detached=d.ReadElement([]).DateValue;Check(detached.Year==0&&detached.Month==2&&detached.Day==29&&detached.TimezoneOffsetMinutes==840);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<date>2023-02-29</date>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<date>-9223372036854775808-01-01</date>",context).Outcome==NativeSchemaOutcome.Unsupported);using var d2=s.Assess("<date>10000-01-01</date>",context).Value!;Check(d2.ReadElement([]).DateValue.TimezoneOffsetMinutes is null);}Check(detached.Year==0);
        Console.WriteLine("Exact packaged date example passes both facades, year zero/extended year, timezone presence, invalid calendar/capacity, export and detached lifetime.");
    }
    private static void ExtensionExample()
    {
        var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-extension.xsd"));var xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-extension.xml"));var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,120);NativeDateValue date;NativeScalarValue label;
        using(var s=NativeSchemaSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){Check(d.HasRootScalar&&d.ScalarCount==0);date=d.ReadRootScalar().DateValue;label=d.ReadAttribute(0).Value;Check(date.Year==0&&date.Month==2&&date.Day==29&&date.TimezoneOffsetMinutes is null&&label.StringValue=="invoice");Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<issued>0000-02-29</issued>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<issued label='x' extra='x'>0000-02-29</issued>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<issued label='x'>1900-02-29</issued>",context).Outcome==NativeSchemaOutcome.Invalid);}Check(date.Year==0&&label.StringValue=="invoice");
        using(var s=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){date=d.ReadElement([]).DateValue;label=d.ReadAttribute([],"","label");Check(date.Year==0&&date.Month==2&&date.Day==29&&date.TimezoneOffsetMinutes is null&&label.StringValue=="invoice");Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<issued>0000-02-29</issued>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<issued label='x' extra='x'>0000-02-29</issued>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<issued label='x'>1900-02-29</issued>",context).Outcome==NativeSchemaOutcome.Invalid);}Check(date.Year==0&&label.StringValue=="invoice");
        Console.WriteLine("Exact packaged date-extension example passes both facades, typed metadata, date absence of timezone, invalid calendar/attributes, export and detached values.");
    }
    private static void DateEnumerationExample()
    {
        var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-date-enum.xsd"));var xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-date-enum.xml"));var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,840);NativeDateValue detached;
        using(var s=NativeSchemaSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){detached=d.ReadRootScalar().DateValue;Check(detached.Year==1999&&detached.Month==12&&detached.Day==31&&detached.TimezoneOffsetMinutes==-660);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<issued>2000-01-01</issued>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<issued>2001-01-01Z</issued>",context).Outcome==NativeSchemaOutcome.Invalid);}Check(detached.Year==1999&&detached.TimezoneOffsetMinutes==-660);
        using(var s=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){detached=d.ReadElement([]).DateValue;Check(detached.Year==1999&&detached.Month==12&&detached.Day==31&&detached.TimezoneOffsetMinutes==-660);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<issued>2000-01-01</issued>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<issued>2001-01-01Z</issued>",context).Outcome==NativeSchemaOutcome.Invalid);}Check(detached.Year==1999&&detached.TimezoneOffsetMinutes==-660);
        Console.WriteLine("Exact packaged date-enumeration example passes both facades, original local date/offset, nonmembers, export and detached lifetime.");
    }
    private static void EnumerationAcceptanceGates()
    {
        var baseSchema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example-date-enum.xsd"));var options=new NativeSchemaSessionOptions(256,256,1024,65536,2,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,840);
        foreach(bool data in new[]{false,true})
        {
            if(data){using var s=NativeSchemaNodeDataSession.Create(baseSchema,options).Value!;var good=s.Assess("<issued xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='Allowed'>1999-12-31-11:00</issued>",context);Check(good.Outcome==NativeSchemaOutcome.Valid);good.Value!.Dispose();Check(s.Assess("<issued xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='Allowed'>2001-01-01Z</issued>",context).Outcome==NativeSchemaOutcome.Invalid);}
            else{using var s=NativeSchemaSession.Create(baseSchema,options).Value!;var good=s.Assess("<issued xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='Allowed'>1999-12-31-11:00</issued>",context);Check(good.Outcome==NativeSchemaOutcome.Valid);good.Value!.Dispose();Check(s.Assess("<issued xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='Allowed'>2001-01-01Z</issued>",context).Outcome==NativeSchemaOutcome.Invalid);}
            foreach(int n in new[]{16,17})
            {
                var members=string.Concat(Enumerable.Range(1,n).Select(day=>"<xs:enumeration value='2000-01-"+day.ToString("00",System.Globalization.CultureInfo.InvariantCulture)+"'/>"));var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='D'><xs:restriction base='xs:date'>"+members+"</xs:restriction></xs:simpleType><xs:element name='r' type='D'/></xs:schema>";
                if(data){var c=NativeSchemaNodeDataSession.Create(schema,options);Check(c.Outcome==(n==16?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Unsupported));c.Value?.Dispose();}
                else{var c=NativeSchemaSession.Create(schema,options);Check(c.Outcome==(n==16?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Unsupported));c.Value?.Dispose();}
            }
            foreach(int n in new[]{256,257})
            {
                var lexical=new string('9',n-6)+"-01-01";Check(lexical.Length==n);var schema=baseSchema.Replace("2000-01-01+13:00",lexical);
                if(data){var c=NativeSchemaNodeDataSession.Create(schema,options);Check(c.Outcome==NativeSchemaOutcome.Unsupported);Check(c.Diagnostics[0].UnsupportedReason==NativeSchemaUnsupportedReason.RepresentationCapacity);}
                else{var c=NativeSchemaSession.Create(schema,options);Check(c.Outcome==NativeSchemaOutcome.Unsupported);Check(c.Diagnostics[0].UnsupportedReason==NativeSchemaUnsupportedReason.RepresentationCapacity);}
            }
            foreach(int n in new[]{32768,32769})
            {
                var schema=baseSchema.Replace("</xs:schema>",new string(' ',n-System.Text.Encoding.UTF8.GetByteCount(baseSchema))+"</xs:schema>");Check(System.Text.Encoding.UTF8.GetByteCount(schema)==n);
                if(data){var c=NativeSchemaNodeDataSession.Create(schema,options);Check(c.Outcome==(n==32768?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Unsupported));c.Value?.Dispose();}
                else{var c=NativeSchemaSession.Create(schema,options);Check(c.Outcome==(n==32768?NativeSchemaOutcome.Valid:NativeSchemaOutcome.Unsupported));c.Value?.Dispose();}
            }
        }
        Console.WriteLine("Additional packaged gates pass both facades: exact xsi:type membership, distinct16/17members,256/257normalized capacity,32768/32769schema UTF8 bytes.");
    }
    private static void DateBoundsExample(string schemaFile="example-bounds.xsd", string xmlFile="example-bounds.xml")
    {
        var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,schemaFile));var xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,xmlFile));var options=new NativeSchemaSessionOptions(256,256,1024,65536,2,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,840);NativeDateValue detached;
        using(var session=NativeSchemaSession.Create(schema,options).Value!){using(var d=session.Assess(xml,context).Value!){detached=d.ReadRootScalar().DateValue;Check(detached.Year==2000&&detached.Month==1&&detached.Day==2&&detached.TimezoneOffsetMinutes==0);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(session.Assess("<issued>2000-01-02</issued>",context).Outcome==NativeSchemaOutcome.Invalid);Check(session.Assess("<issued>2000-01-03Z</issued>",context).Outcome==NativeSchemaOutcome.Invalid);}Check(detached.Day==2);
        using(var session=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=session.Assess(xml,context).Value!){detached=d.ReadElement([]).DateValue;Check(detached.Year==2000&&detached.Month==1&&detached.Day==2&&detached.TimezoneOffsetMinutes==0);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(session.Assess("<issued>2000-01-02</issued>",context).Outcome==NativeSchemaOutcome.Invalid);Check(session.Assess("<issued>2000-01-03Z</issued>",context).Outcome==NativeSchemaOutcome.Invalid);}Check(detached.Day==2);
        Console.WriteLine("Exact packaged date bounds example passes both facades, bound/timezone refusals, export and detached lifetime.");
    }
    private static void BooleanPatternExample(string schemaFile="example-boolean.xsd",string xmlFile="example-boolean.xml") { var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,schemaFile));var xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,xmlFile));var options=new NativeSchemaSessionOptions(256,256,1024,65536,2,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);using(var session=NativeSchemaSession.Create(schema,options).Value!){using(var d=session.Assess(xml,context).Value!){Check(d.ReadRootScalar().BooleanValue);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(session.Assess("<enabled>1</enabled>",context).Outcome==NativeSchemaOutcome.Invalid);}using(var session=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=session.Assess(xml,context).Value!){Check(d.ReadElement([]).BooleanValue);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(session.Assess("<enabled>1</enabled>",context).Outcome==NativeSchemaOutcome.Invalid);}Console.WriteLine("Exact packaged Boolean pattern example passes both facades and lexical/value distinction.");}
    private static void DecimalEnumerationExample(string schemaFile="example-decimal-enum.xsd",string xmlFile="example-decimal-enum.xml") { var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,schemaFile));var xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,xmlFile));var options=new NativeSchemaSessionOptions(256,256,1024,65536,2,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);using(var session=NativeSchemaSession.Create(schema,options).Value!){using(var d=session.Assess(xml,context).Value!){Check(d.ReadRootScalar().DecimalValue.ToInvariantString(256)=="1.23");Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(session.Assess("<price>1.24</price>",context).Outcome==NativeSchemaOutcome.Invalid);}using(var session=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=session.Assess(xml,context).Value!){Check(d.ReadElement([]).DecimalValue.ToInvariantString(256)=="1.23");Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(session.Assess("<price>1.24</price>",context).Outcome==NativeSchemaOutcome.Invalid);}Console.WriteLine("Exact packaged decimal enumeration passes both facades and original exact values.");}
    private static int Main()
    {
        try
        {
            string schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"example.xsd")),xml="<counter>18446744073709551615</counter>";var expected=BigInteger.Parse("18446744073709551615",CultureInfo.InvariantCulture);var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);NativeScalarValue detached;
            using(var s=NativeSchemaSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){Check(d.HasRootScalar&&d.ScalarCount==0);detached=d.ReadRootScalar();Check(detached.IntegerValue==expected);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<counter>18446744073709551616</counter>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<counter>-1</counter>",context).Outcome==NativeSchemaOutcome.Invalid);using var zero=s.Assess("<counter>-0</counter>",context).Value!;Check(zero.ReadRootScalar().IntegerValue.IsZero);}Check(detached.IntegerValue==expected);
            using(var s=NativeSchemaNodeDataSession.Create(schema,options).Value!){using(var d=s.Assess(xml,context).Value!){detached=d.ReadElement([]);Check(detached.IntegerValue==expected);Check(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));}Check(s.Assess("<counter>18446744073709551616</counter>",context).Outcome==NativeSchemaOutcome.Invalid);Check(s.Assess("<counter>-1</counter>",context).Outcome==NativeSchemaOutcome.Invalid);using var zero=s.Assess("<counter>-0</counter>",context).Value!;Check(zero.ReadElement([]).IntegerValue.IsZero);}Check(detached.IntegerValue==expected);
            DateBoundsExample("example-complex-date.xsd","example-complex-date.xml");Console.WriteLine("Exact packaged date complex restriction example passes both facades, inherited constraints and original values.");DateBoundsExample("example-chains.xsd","example-chains.xml");Console.WriteLine("Exact packaged named date chain example passes both facades and inherited constraints.");BooleanPatternExample("example-boolean-chains.xsd","example-boolean-chains.xml");BooleanPatternExample();DecimalEnumerationExample();DateBoundsExample();DateBoundsExample("example-patterns.xsd","example-patterns.xml");EnumerationAcceptanceGates();DateEnumerationExample();ExtensionExample();DateExample();EnumerationExample();AllExample();Console.WriteLine("Packaged unsignedLong example passes both facades, exact BigInteger/export/detached lifetime, invalid bounds and signed zero.");return 0;
        }catch(Exception e){Console.Error.WriteLine("Caught counter README diagnostic: "+e.Message);return 1;}
    }
}
