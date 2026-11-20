// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Compiles and exercises packaged README examples against a local public package.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : license.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Globalization;
using Bosak.Schema;
using System.Text.Json;
internal static class Program
{
    private static NativeDecimalSessionOptions Options => new(256, 256, 1, 4, 1, 256, 256);
    private static NativeDecimalAssessmentContext Context => new(new DateTimeOffset(2026,11,19,0,0,0,TimeSpan.Zero),0);
    private static int Main(string[] args)
    {
        try
        {
            Directory.SetCurrentDirectory(args[0]);
            ExistingFactoriesCompile();
            var original = Console.Out; var output = new StringWriter(); Console.SetOut(output);
            try { Workflow(); } finally { Console.SetOut(original); }
            Require(output.ToString().StartsWith("12.5"+Environment.NewLine+"2"+Environment.NewLine+"1"+Environment.NewLine), "workflow output");
            Require(output.ToString().Contains("12.50"), "lexical export");
            string schema=File.ReadAllText("native-decimal.xsd");
            string xml=File.ReadAllText("native-decimal.xml");
            Check(NativeDecimalSession.Create(schema.Replace("<xs:schema ", "<xs:schema attributeFormDefault='qualified' "),Options), NativeDecimalUnsupportedReason.Namespace, "A schema namespace setting");
            Check(NativeDecimalSession.Create(schema.Replace("count(amount) eq 2 and tax gt 0", "sum(amount) eq 2"),Options), NativeDecimalUnsupportedReason.AssertionGrammar, "inspect its documented limits");
            using var session=NativeDecimalSession.Create(schema,Options).Value!;
            Check(session.Assess(xml.Replace("<order>","<order flag='PRIVATE-MARKER'>"),Context), NativeDecimalUnsupportedReason.Attributes, "An attribute");
            Check(session.Assess(new string(' ',65537),Context),NativeDecimalUnsupportedReason.RepresentationCapacity,"An explicit representation limit");
            Check(session.Assess("<!DOCTYPE order><order/>",Context),NativeDecimalUnsupportedReason.Unspecified,"inspect its documented limits");
            var valid=session.Assess(xml,Context); using var document=valid.Value!;
            Inspect(valid); Require(valid.Diagnostics.IsEmpty,"valid diagnostics");
            using (var nsSession=NativeDecimalSession.Create(File.ReadAllText("README-namespace.xsd"),Options).Value!)
            {
                var nsResult=nsSession.Assess(File.ReadAllText("README-namespace.xml"),Context);
                Require(nsResult.Outcome==NativeDecimalOutcome.Valid,"README namespace outcome");
                using var nsDocument=nsResult.Value!;
                Require(nsDocument.ReadDecimal(0).ToInvariantString(256)=="12.5","README namespace decimal");
                Require(System.Xml.Linq.XDocument.Parse(nsDocument.ExportXml()).Root!.Name==System.Xml.Linq.XName.Get("order","urn:orders"),"README namespace identity");
            }
            using (var named=NativeDecimalSession.Create(File.ReadAllText("README-named.xsd"),Options).Value!)
            {
                foreach(var rootName in new[]{"order","quote"})
                {
                    var xmlNamed=File.ReadAllText("README-namespace.xml").Replace("o:order","o:"+rootName,StringComparison.Ordinal);
                    var namedResult=named.Assess(xmlNamed,Context);Require(namedResult.Outcome==NativeDecimalOutcome.Valid,"named README outcome");
                    using var namedDoc=namedResult.Value!;Require(namedDoc.ReadDecimal(0).ToInvariantString(256)=="12.5","named README scalar");
                    Require(System.Xml.Linq.XDocument.Parse(namedDoc.ExportXml()).Root!.Name==System.Xml.Linq.XName.Get(rootName,"urn:orders"),"named README export");
                }
            }
            using (var restricted=NativeDecimalSession.Create(File.ReadAllText("README-restriction.xsd"),Options).Value!)
            {
                var text=File.ReadAllText("README-restriction.xml");
                var assessed=restricted.Assess(text,Context);Require(assessed.Outcome==NativeDecimalOutcome.Valid,"restriction example");
                using var doc=assessed.Value!;
                Require(doc.DecimalCount==3 && doc.ReadDecimal(0).ToInvariantString(256)=="12.5" && doc.ReadDecimal(1).ToInvariantString(256)=="2" && doc.ReadDecimal(2).ToInvariantString(256)=="1","restriction values");
                Require(restricted.Assess(text.Replace("12.50","-1",StringComparison.Ordinal),Context).Outcome==NativeDecimalOutcome.Invalid,"negative restriction violation");
                Require(restricted.Assess(text.Replace("12.50","12.501",StringComparison.Ordinal),Context).Outcome==NativeDecimalOutcome.Invalid,"fraction restriction violation");
            }
            var saved=Console.Out;var scalarOutput=new StringWriter();Console.SetOut(scalarOutput);
            try { ScalarWorkflow(); } finally { Console.SetOut(saved); }
            Require(scalarOutput.ToString().StartsWith(" INV-001 "+Environment.NewLine+"12.5"+Environment.NewLine),"new README scalar output");
            var proposedOutput=new StringWriter();Console.SetOut(proposedOutput);
            try { ProposedConsumer(); } finally { Console.SetOut(saved); }
            Require(proposedOutput.ToString()==" INV-001 "+Environment.NewLine+"12.5"+Environment.NewLine,"previous proposed consumer");
            var booleanOutput=new StringWriter();var booleanSaved=Console.Out;Console.SetOut(booleanOutput);
            try { BooleanWorkflow(); } finally { Console.SetOut(booleanSaved); }
            Require(booleanOutput.ToString().StartsWith("false"+Environment.NewLine) && booleanOutput.ToString().Contains("<paid>0</paid>"),"README false Boolean and lexical export");
            var integerSaved=Console.Out;var integerOutput=new StringWriter();Console.SetOut(integerOutput);
            try { IntegerWorkflow(); } finally { Console.SetOut(integerSaved); }
            Require(integerOutput.ToString().StartsWith("9223372036854775808"+Environment.NewLine) && integerOutput.ToString().Contains("+09223372036854775808"),"README exact integer and lexical export");
            var namedSaved=Console.Out;var namedOutput=new StringWriter();Console.SetOut(namedOutput);
            try { StringFacetWorkflow(); } finally { Console.SetOut(namedSaved); }
            Require(namedOutput.ToString().StartsWith("😀A"+Environment.NewLine) && namedOutput.ToString().Contains("<label>😀A</label>"),"README named Unicode string and lexical export");
            var attributeSaved=Console.Out;var attributeOutput=new StringWriter();Console.SetOut(attributeOutput);
            try { AttributeWorkflow(); RootAttributeProposedConsumer(); } finally { Console.SetOut(attributeSaved); }
            Require(attributeOutput.ToString().Contains("{}currency: String") && attributeOutput.ToString().Contains("{}paid: Boolean") && attributeOutput.ToString().Contains("{urn:orders}quantity: Integer") && attributeOutput.ToString().Contains("9223372036854775808"),"README root attributes and former proposed consumer");
            var rootSaved=Console.Out;var rootOutput=new StringWriter();Console.SetOut(rootOutput);
            try { SimpleContentWorkflow(); } finally { Console.SetOut(rootSaved); }
            Require(rootOutput.ToString().StartsWith("12.5"+Environment.NewLine+"0"+Environment.NewLine+"EUR"+Environment.NewLine) && rootOutput.ToString().Contains("12.50"),"README decimal simple-content root and unchanged zero leaf count");
            var nestedSaved=Console.Out;var nestedOutput=new StringWriter();Console.SetOut(nestedOutput);
            try { NestedWorkflow(); } finally { Console.SetOut(nestedSaved); }
            Require(nestedOutput.ToString().StartsWith("2"+Environment.NewLine+"ABC-0001"+Environment.NewLine+"12.5"+Environment.NewLine+"ORDER-1"+Environment.NewLine) && nestedOutput.ToString().Contains("12.50"),"README terminal-only nested sequence output");
            var ownerSaved=Console.Out;var ownerOutput=new StringWriter();Console.SetOut(ownerOutput);
            try { NestedAttributeWorkflow(); } finally { Console.SetOut(ownerSaved); }
            Require(ownerOutput.ToString().StartsWith("ROOT"+Environment.NewLine+"0: currency=EUR"+Environment.NewLine+"1: currency=USD"+Environment.NewLine+"2"+Environment.NewLine) && ownerOutput.ToString().Contains("12.50"),"README nested owner paths and unchanged scalar indexes");
            File.WriteAllText(args[1],JsonSerializer.Serialize(new {codeBlocksCompiled=11,workflowOutput=true,namespaceBranch=true,attributeBranch=true,capacityBranch=true,grammarDefault=true,unspecifiedDefault=true,validGuard=true,privateInputNotPrinted=true,packageVersion="0.6.0-alpha.0.14",namespaceXmlExample=true,namedTypeXmlExample=true,decimalRestrictionExample=true,mixedScalarExample=true,booleanExample=true,integerExample=true,namedStringFacetExample=true,rootAttributeExample=true,decimalSimpleContentExample=true,nestedSequenceExample=true,nestedAttributeExample=true,previousRootAttributeConsumerExecuted=true,previousProposedConsumerExecuted=true,published=false},new JsonSerializerOptions {WriteIndented=true}));
            return 0;
        }
        catch(Exception e) { Console.Error.WriteLine("Caught README diagnostic failure: "+e.GetType().Name+": "+e.Message); return 1; }
    }
    private static void Require(bool condition,string label) { if(!condition) throw new InvalidOperationException(label); }
    private static void Check<T>(NativeDecimalResult<T> result,NativeDecimalUnsupportedReason reason,string expected) where T:class
    {
        Require(result.Outcome==NativeDecimalOutcome.Unsupported && result.Value is null,"refusal");
        Require(result.Diagnostics[0].UnsupportedReason==reason,"reason");
        var original=Console.Error;var output=new StringWriter();Console.SetError(output);
        try { Inspect(result); } finally {Console.SetError(original);}
        Require(output.ToString().Contains(result.Diagnostics[0].Code) && output.ToString().Contains(expected) && !output.ToString().Contains("PRIVATE-MARKER"),"diagnostic text");
    }
    private static void Workflow()
    {
var options = new NativeDecimalSessionOptions(256, 256, 1, 4, 1, 256, 256);
var episode = new NativeDecimalAssessmentContext(
    new DateTimeOffset(2026, 11, 17, 12, 0, 0, TimeSpan.Zero), 60);
var compiled = NativeDecimalSession.Create(File.ReadAllText("native-decimal.xsd"), options);
if (compiled.Outcome != NativeDecimalOutcome.Valid)
{
    Console.Error.WriteLine(compiled.Diagnostics[0].Code);
    return;
}

using var session = compiled.Value!;
var result = session.Assess(File.ReadAllText("native-decimal.xml"), episode);
if (result.Outcome != NativeDecimalOutcome.Valid)
{
    Console.Error.WriteLine(result.Diagnostics[0].Code);
    return;
}

using var document = result.Value!;
for (var index = 0; index < document.DecimalCount; index++)
    Console.WriteLine(document.ReadDecimal(index).ToInvariantString(256));
Console.WriteLine(document.ExportXml());
    }

    private static void Inspect<T>(NativeDecimalResult<T> result) where T:class
    {
if (result.Outcome == NativeDecimalOutcome.Unsupported)
{
    var diagnostic = result.Diagnostics[0];
    Console.Error.WriteLine(diagnostic.Code);
    switch (diagnostic.UnsupportedReason)
    {
        case NativeDecimalUnsupportedReason.Namespace:
            Console.Error.WriteLine("A schema namespace setting is outside the profile.");
            break;
        case NativeDecimalUnsupportedReason.Attributes:
            Console.Error.WriteLine("An attribute is outside the profile.");
            break;
        case NativeDecimalUnsupportedReason.RepresentationCapacity:
            Console.Error.WriteLine("An explicit representation limit was exceeded.");
            break;
        default:
            Console.Error.WriteLine("The input is outside the profile; inspect its documented limits.");
            break;
    }
}
    }
    private static void ExistingFactoriesCompile()
    {
var compiler = SchemaServices.CreateSchemaCompiler();
var validator = SchemaServices.CreateValidationService();
var xsltCompiler = SchemaAwareXslt.CreateCompiler();
    }

    private static void ScalarWorkflow()
    {
var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="r"><xs:complexType><xs:sequence>
    <xs:element name="id" type="xs:string"/>
    <xs:element name="amount" type="xs:decimal"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 4, 1, 256, 256);
var context = new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("<r><id> INV-001 </id><amount>12.50</amount></r>", context);
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
for (var index = 0; index < document.ScalarCount; index++)
{
    var scalar = document.ReadScalar(index);
    switch (scalar.Kind)
    {
        case NativeScalarKind.String:
            Console.WriteLine(scalar.StringValue);
            break;
        case NativeScalarKind.Decimal:
            Console.WriteLine(scalar.DecimalValue.ToInvariantString(256));
            break;
        default:
            throw new NotSupportedException("This application does not handle this scalar kind.");
    }
}
Console.WriteLine(document.ExportXml());
    }

    private static void ProposedConsumer()
    {


var options = new NativeSchemaSessionOptions(
    maxNativeCoefficientDigits: 256, maxNativeScale: 256,
    maxStringCharacters: 256, maxExportCharacters: 65536,
    maxOpenDocuments: 1, maxCachedScalarValuesPerDocument: 4,
    maxTypedResultAtoms: 1, maxQueryCoefficientDigits: 256, maxQueryScale: 256);
var context = new NativeSchemaAssessmentContext(
    new DateTimeOffset(2026, 11, 19, 12, 0, 0, TimeSpan.Zero), 0);
var compiled = NativeSchemaSession.Create(File.ReadAllText("string-order.xsd"), options);
if (compiled.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(compiled.Diagnostics[0].Code);
    return;
}
using var session = compiled.Value!;
var assessed = session.Assess(File.ReadAllText("string-order.xml"), context);
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
for (var index = 0; index < document.ScalarCount; index++)
{
    var value = document.ReadScalar(index);
    switch (value.Kind)
    {
        case NativeScalarKind.String:
            Console.WriteLine(value.StringValue);
            break;
        case NativeScalarKind.Decimal:
            Console.WriteLine(value.DecimalValue.ToInvariantString(256));
            break;
        default:
            throw new NotSupportedException("This application does not handle this scalar kind.");
    }
}

    }

    private static void BooleanWorkflow()
    {
var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="r"><xs:complexType><xs:sequence>
    <xs:element name="paid" type="xs:boolean"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 1, 65536, 1, 4, 1, 1, 0);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("<r><paid>0</paid></r>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
var scalar = document.ReadScalar(0);
if (scalar.Kind != NativeScalarKind.Boolean)
    throw new InvalidOperationException("Expected a Boolean scalar.");
Console.WriteLine(scalar.BooleanValue ? "true" : "false");
Console.WriteLine(document.ExportXml());
    }

    private static void IntegerWorkflow()
    {
var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="r"><xs:complexType><xs:sequence>
    <xs:element name="quantity" type="xs:integer"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 0, 1, 65536, 1, 4, 1, 256, 0);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("<r><quantity>+09223372036854775808</quantity></r>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
var scalar = document.ReadScalar(0);
if (scalar.Kind != NativeScalarKind.Integer)
    throw new InvalidOperationException("Expected an integer scalar.");
Console.WriteLine(scalar.IntegerValue.ToString(CultureInfo.InvariantCulture));
Console.WriteLine(document.ExportXml());
    }

    private static void StringFacetWorkflow()
    {
var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:simpleType name="Label"><xs:restriction base="xs:string">
    <xs:length value="2"/>
    <xs:enumeration value="😀A"/>
  </xs:restriction></xs:simpleType>
  <xs:element name="r"><xs:complexType><xs:sequence>
    <xs:element name="label" type="Label"/>
  </xs:sequence></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 3, 65536, 1, 4, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("<r><label>😀A</label></r>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
Console.WriteLine(document.ReadScalar(0).StringValue);
Console.WriteLine(document.ExportXml());

    }
    private static void AttributeWorkflow()
    {
var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:orders"
           targetNamespace="urn:orders" elementFormDefault="qualified" attributeFormDefault="unqualified">
  <xs:simpleType name="Currency"><xs:restriction base="xs:string">
    <xs:length value="3"/><xs:enumeration value="EUR"/><xs:enumeration value="USD"/>
  </xs:restriction></xs:simpleType>
  <xs:complexType name="Order"><xs:sequence>
    <xs:element name="id" type="xs:string"/><xs:element name="amount" type="xs:decimal"/>
  </xs:sequence>
    <xs:attribute name="currency" type="t:Currency" use="required"/>
    <xs:attribute name="paid" type="xs:boolean"/>
    <xs:attribute name="quantity" type="xs:integer" form="qualified" use="required"/>
  </xs:complexType>
  <xs:element name="order" type="t:Order"/>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 32, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("""
<o:order xmlns:o="urn:orders" o:quantity="9223372036854775808" paid="0" currency="EUR"><o:id> INV-001 </o:id><o:amount>12.50</o:amount></o:order>
""", new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
Console.WriteLine(document.ReadScalar(0).StringValue);
Console.WriteLine(document.ReadScalar(1).DecimalValue.ToInvariantString(256));
for (var index = 0; index < document.AttributeCount; index++)
{
    var attribute = document.ReadAttribute(index);
    Console.WriteLine("{" + attribute.NamespaceUri + "}" + attribute.LocalName + ": " + attribute.Value.Kind);
}
Console.WriteLine(document.ReadAttribute(2).Value.IntegerValue.ToString(CultureInfo.InvariantCulture));
Console.WriteLine(document.ExportXml());
    }

    private static void RootAttributeProposedConsumer()
    {
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 32, 1, 256, 256);
var created = NativeSchemaSession.Create(File.ReadAllText("future-order.xsd"), options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess(File.ReadAllText("future-order.xml"),
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
// Existing leaf indexes remain id=0, amount=1.
Console.WriteLine(document.ReadScalar(0).StringValue);
Console.WriteLine(document.ReadScalar(1).DecimalValue.ToInvariantString(256));
for (var index = 0; index < document.AttributeCount; index++)
{
    var attribute = document.ReadAttribute(index);
    Console.WriteLine($"{{{attribute.NamespaceUri}}}{attribute.LocalName}: {attribute.Value.Kind}");
}
Console.WriteLine(document.ExportXml());
    }

    private static void SimpleContentWorkflow()
    {
var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:t="urn:price" targetNamespace="urn:price">
  <xs:complexType name="PriceType"><xs:simpleContent><xs:extension base="xs:decimal">
    <xs:attribute name="currency" type="xs:string" use="required"/>
  </xs:extension></xs:simpleContent></xs:complexType>
  <xs:element name="price" type="t:PriceType"/>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 8, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(created.Diagnostics[0].Code);
    return;
}
using var session = created.Value!;
var assessed = session.Assess("<p:price xmlns:p='urn:price' currency='EUR'>12.50</p:price>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
{
    Console.Error.WriteLine(assessed.Diagnostics[0].Code);
    return;
}
using var document = assessed.Value!;
if (!document.HasRootScalar)
    throw new InvalidOperationException("Expected a root decimal value.");
Console.WriteLine(document.ReadRootScalar().DecimalValue.ToInvariantString(256));
Console.WriteLine(document.ScalarCount);
Console.WriteLine(document.ReadAttribute(0).Value.StringValue);
Console.WriteLine(document.ExportXml());
    }

    private static void NestedWorkflow()
    {
var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:complexType name="Line"><xs:sequence>
    <xs:element name="sku" type="xs:string"/>
    <xs:element name="amount" type="xs:decimal"/>
  </xs:sequence></xs:complexType>
  <xs:element name="order"><xs:complexType><xs:sequence>
    <xs:element name="line" type="Line" maxOccurs="2"/>
  </xs:sequence><xs:attribute name="id" type="xs:string" use="required"/></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 8, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
    throw new InvalidOperationException(created.Diagnostics[0].Code);
using var session = created.Value!;
var assessed = session.Assess("<order id='ORDER-1'><line><sku>ABC-0001</sku><amount>12.50</amount></line></order>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
    throw new InvalidOperationException(assessed.Diagnostics[0].Code);
using var document = assessed.Value!;
Console.WriteLine(document.ScalarCount);
Console.WriteLine(document.ReadScalar(0).StringValue);
Console.WriteLine(document.ReadScalar(1).DecimalValue.ToInvariantString(256));
Console.WriteLine(document.ReadAttribute(0).Value.StringValue);
Console.WriteLine(document.ExportXml());

    }
    private static void NestedAttributeWorkflow()
    {
var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:complexType name="Line"><xs:sequence>
    <xs:element name="amount" type="xs:decimal"/>
  </xs:sequence><xs:attribute name="currency" type="xs:string" use="required"/></xs:complexType>
  <xs:element name="order"><xs:complexType><xs:sequence>
    <xs:element name="line" type="Line" maxOccurs="2"/>
  </xs:sequence><xs:attribute name="id" type="xs:string"/></xs:complexType></xs:element>
</xs:schema>
""";
var options = new NativeSchemaSessionOptions(256, 256, 256, 65536, 1, 16, 1, 256, 256);
var created = NativeSchemaSession.Create(schema, options);
if (created.Outcome != NativeSchemaOutcome.Valid)
    throw new InvalidOperationException(created.Diagnostics[0].Code);
using var session = created.Value!;
var assessed = session.Assess("<order id='ROOT'><line currency='EUR'><amount>12.50</amount></line><line currency='USD'><amount>3.75</amount></line></order>",
    new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch, 0));
if (assessed.Outcome != NativeSchemaOutcome.Valid)
    throw new InvalidOperationException(assessed.Diagnostics[0].Code);
using var document = assessed.Value!;
Console.WriteLine(document.ReadAttribute(0).Value.StringValue);
for (var i = 0; i < document.DescendantAttributeCount; i++)
{
    var attribute = document.ReadDescendantAttribute(i);
    Console.WriteLine($"{string.Join(".", attribute.OwnerElementPath)}: {attribute.LocalName}={attribute.Value.StringValue}");
}
Console.WriteLine(document.ScalarCount);
Console.WriteLine(document.ExportXml());

    }
}
