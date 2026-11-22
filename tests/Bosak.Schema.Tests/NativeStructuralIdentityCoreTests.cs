// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 22 November 2026
// PURPOSE              : Verifies isolated empty typed-value representation through original native assessments.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 22-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 22-11-2026     | Adapt native controls for bounded integer simple content |
//                      | Charles Korthout | 0.3   | 22-11-2026     | Verify isolated contextual assertions and retained native baseline |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Collections.Immutable;
using System.Xml.Linq;
using Bosak.Schema;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Xunit;
namespace Bosak.Schema.Tests.Structural;
/// <summary>Checks named empty-content invalidity, original upstream data and owner guards.</summary>
public sealed class NativeStructuralIdentityCoreTests
{
    private static string A(string expr)=>new XElement(XName.Get("assert","http://www.w3.org/2001/XMLSchema"),new XAttribute("test",expr)).ToString(SaveOptions.DisableFormatting);
    /// <summary>A named empty root must preserve empty-content rules after plan copying.</summary>
    /// <param name="sequence">Explicit empty sequence.</param><param name="text">Forbidden content.</param>
    [Theory][InlineData(false," ")][InlineData(true," ")][InlineData(false,"<t:x/>")][InlineData(true,"<t:x/>")]
    public void NamedEmptyInvalid(bool sequence,string text)
    {var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='urn:named' targetNamespace='urn:named'><xs:complexType name='E'>"+(sequence?"<xs:sequence/>":"")+A("namespace-uri()='urn:named'")+"</xs:complexType><xs:element name='r' type='t:E'/></xs:schema>";using var f=new Fixture(schema,"<t:r xmlns:t='urn:named'>"+text+"</t:r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);}
    /// <summary>Measures the original remaining fixture with native assessment; this is not a frozen-pilot run.</summary>
    [Fact]
    public void OriginalAssert027()
    {var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"original-ns3.xsd"));var xml=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"original-ns3.xml"));using var f=new Fixture(schema,xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var outer=f.Element([]);var inner=f.Element([0]);Assert.True(outer.HasNoTypedValue);Assert.False(inner.HasNoTypedValue);Assert.Equal("http://www.example.org",inner.NamespaceUri);Assert.Equal(XdmValueKind.Undefined,f.Run(op=>f.Evaluate(op,inner)).Kind);}
    /// <summary>Namespace assertions retain authority, foreign/copy refusal, cancellation and recovery.</summary>
    [Fact]
    public void ContextOwnershipGuards()
    {var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType>"+A("namespace-uri(.)=''")+"</xs:complexType></xs:element></xs:schema>";using var a=new Fixture(schema,"<r/>");using var b=new Fixture(schema,"<r/>");var n=a.Element([]);Assert.Throws<ArgumentException>(()=>b.Run(op=>b.Evaluate(op,n)));using var copy=NativeSchemaInput.Capture("<r/>").Value!;Assert.Throws<ArgumentException>(()=>a.Run(op=>a.Evaluate(op,copy.Root)));using var token=new CancellationTokenSource();token.Cancel();Assert.ThrowsAny<OperationCanceledException>(()=>a.Run(op=>a.Evaluate(op,n),token.Token));Assert.Equal(XdmValueKind.Undefined,a.Run(op=>a.Evaluate(op,n)).Kind);a.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>n.TypedValue);}
    /// <summary>Legacy assertions retain namespace-uri spelling inside literals or child names.</summary>
    /// <param name="literal">Text compared using the established child string grammar.</param>
    [Theory][InlineData("namespace-uri")][InlineData("namespace-uri()")][InlineData("f:namespace-uri()")]
    public void LegacyLiteralRouting(string literal)
    {var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v' type='xs:string'/></xs:sequence>"+A("v = '"+literal+"'")+"</xs:complexType></xs:element></xs:schema>";using var f=new Fixture(schema,"<r><v>"+literal+"</v></r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal(literal,f.Read(f.Element([0])).StringValue);}
    /// <summary>Existing bounded xsi:type self-selection retains the original empty type and name context.</summary>
    [Fact]
    public void EmptyXsiTypeSelfSelection()
    {var schema="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:t='urn:e' targetNamespace='urn:e'><xs:complexType name='E'>"+A("namespace-uri()='urn:e'")+"</xs:complexType><xs:element name='r' type='t:E'/></xs:schema>";using var f=new Fixture(schema,"<t:r xmlns:t='urn:e' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='t:E'/>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);Assert.Equal(("urn:e","E"),f.Element([]).SchemaTypeAnnotation);Assert.Equal(XdmValueKind.Undefined,f.Run(op=>f.Evaluate(op,f.Element([]))).Kind);}
}
