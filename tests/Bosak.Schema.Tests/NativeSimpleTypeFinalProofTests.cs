// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 November 2026
// PURPOSE              : Proves final derivation refusal and unchanged native value authority.
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
namespace Bosak.Schema.Tests.SimpleTypeFinal;
/// <summary>Checks direct derivation methods and original native authority.</summary>
public sealed class NativeSimpleTypeFinalProofTests
{
    private static string Schema(string final,string body="",string type="A") => "<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'><xs:simpleType name='A' final='"+final+"'><xs:restriction base='xs:decimal'/></xs:simpleType>"+body+"<xs:element name='r' type='"+type+"'/></xs:schema>";
    /// <summary>Each prohibited direct derivation refuses before publishing a compiled schema.</summary>
    /// <param name="method">Blocked derivation method.</param>
    /// <param name="body">Original derivation source.</param>
    [Theory]
    [InlineData("restriction","<xs:simpleType name='B'><xs:restriction base='A'/></xs:simpleType>")]
    [InlineData("list","<xs:simpleType name='B'><xs:list itemType='A'/></xs:simpleType>")]
    [InlineData("union","<xs:simpleType name='B'><xs:union memberTypes='xs:boolean A'/></xs:simpleType>")]
    [InlineData("extension","<xs:complexType name='B'><xs:simpleContent><xs:extension base='A'/></xs:simpleContent></xs:complexType>")]
    public void Prohibited(string method,string body)
    {
        var result=NativeDecimalSchema.Compile(Schema(method,body),new(256,256),maxStringCharacters:1024);Assert.Equal(NativeOutcome.Invalid,result.Outcome);Assert.Null(result.Value);
    }
    /// <summary>Final value use preserves governing declaration, named type and original atomic normalization.</summary>
    [Fact]public void OriginalAuthority()
    {
        using var f=new Fixture(Schema("#all"),"<r>01.20</r>");Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Same(f.Provider.ResolveElementDeclaration(new("","r"),default).Value,fact.Declaration);Assert.Equal("1.2",f.Read(f.Element([])).DecimalValue.ToInvariantString(256));Assert.Equal("A",fact.NativeType.Name);
    }
    /// <summary>Legacy grammar remains closed and cancellation takes precedence.</summary>
    [Fact]public void LegacyAndCancellation()
    {
        Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(Schema("list"),new(256,256)).Outcome);using var c=new CancellationTokenSource();c.Cancel();Assert.Throws<OperationCanceledException>(()=>NativeDecimalSchema.Compile(Schema("list"),new(256,256),c.Token,1024));
    }
}
