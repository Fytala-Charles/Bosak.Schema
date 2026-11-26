// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies numeric lexical patterns, original type authority, budgets and expiry.
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
using System.Xml.Linq;
using Bosak.Schema.Tests.NodeData;
using Bosak.XPath.Core.Schema;
using Xunit;
namespace Bosak.Schema.Tests.NumericPatterns;
/// <summary>Checks original numeric components, failure publication and shared pattern budgets.</summary>
public sealed class NativeNumericPatternProofTests
{
    private const string X="http://www.w3.org/2001/XMLSchema";
    private static string Schema(bool decimalValue=false,bool attribute=false)
    {
        var definition="<xs:simpleType name='N'><xs:restriction base='xs:"+(decimalValue?"decimal":"integer")+"'><xs:pattern value='"+(decimalValue?"[0-9]{2}[.][0-9]{2}":"[0-9]{3}")+"'/></xs:restriction></xs:simpleType>";
        var root=attribute?"<xs:element name='r'><xs:complexType><xs:attribute name='a' type='N'/></xs:complexType></xs:element>":"<xs:element name='r' type='N'/>";return "<xs:schema xmlns:xs='"+X+"'>"+definition+root+"</xs:schema>";
    }
    /// <summary>Numeric pattern validation preserves actual governing/atomic handles and exact values after collapse.</summary>
    /// <param name="decimalValue">Decimal selector.</param><param name="attribute">Attribute selector.</param>
    [Theory][InlineData(false,false)][InlineData(false,true)][InlineData(true,false)][InlineData(true,true)]
    public void OriginalAuthorityAndExpiry(bool decimalValue,bool attribute)
    {
        var lexical=decimalValue?" 01.20 ":" 001 ";var xml=attribute?"<r a='"+lexical+"'/>":"<r>"+lexical+"</r>";using var f=new Fixture(Schema(decimalValue,attribute),xml);Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var node=(NativeSessionNode)(attribute?f.Attribute([],"","a"):f.Element([]));var fact=f.Input.FactsFor(node)!;Assert.NotNull(fact.NativeType.NumericPatterns);Assert.Equal("N",fact.NativeType.Name);Assert.Same(f.Provider.TypeComponent(fact.NativeType),fact.Type);var raw=fact.ReadTypedValue(default).Value;var item=Assert.Single(raw.AtomicItems);Assert.Same(fact.Type,item.TypeEvidence.DeclaredType);Assert.Same(fact.Type,item.TypeEvidence.ActualType);Assert.True(f.Session.Advisor!.OwnsTypedValue(raw,f.Input,f.Assessment.Origin));Assert.True(f.Session.Advisor.OwnsNode(node,fact,f.Input,f.Assessment.Origin));var detached=f.Read(node);if(decimalValue)Assert.Equal("1.2",detached.DecimalValue.ToInvariantString(256));else Assert.Equal("1",detached.IntegerValue.ToString());f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));
    }
    /// <summary>Both primitive-invalid and lexical-pattern-invalid input publish no original facts.</summary>
    /// <param name="lexical">Original invalid spelling.</param>
    [Theory][InlineData("1")][InlineData("٠٠١")]
    public void FailurePublishesNothing(string lexical)
    {using var f=new Fixture(Schema(),"<r>"+lexical+"</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.False(f.Input.IsAssessed);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Assessment.Evidence);}
    /// <summary>Numeric Nd escapes consume shared compilation and matching budgets and honor cancellation.</summary>
    [Fact]
    public void SharedBudgetsAndCancellation()
    {
        var compilation=new NativePatternCompilation(default);var pattern=NativePattern.Compile(@"\p{Nd}{1,3}",compilation,numericAlphabet:true);Assert.True(compilation.States>0);Assert.True(pattern.Match("001",new(default)));Assert.False(pattern.Match("١٢٣",new(default)));
        var error=Assert.Throws<NativeProfileException>(()=>pattern.Match("001",new(default,0)));Assert.Equal(NativeOutcome.Unsupported,error.Outcome);Assert.Equal(NativeDecimalUnsupportedReason.RepresentationCapacity,error.UnsupportedReason);
        using var cancelled=new CancellationTokenSource();cancelled.Cancel();Assert.Throws<OperationCanceledException>(()=>pattern.Match("001",new(cancelled.Token)));var nonnumeric=Assert.Throws<NativeProfileException>(()=>NativePattern.Compile(@"\p{Nd}",new(default)));Assert.Equal(NativeOutcome.Unsupported,nonnumeric.Outcome);
    }
    /// <summary>Inherited decimal patterns retain actual base components and conjunctive lexical checks.</summary>
    [Fact]
    public void DecimalInheritanceAndRecovery()
    {
        var schema=Schema(true).Replace("<xs:element name='r' type='N'/>","<xs:simpleType name='D'><xs:restriction base='N'><xs:pattern value='0[0-9][.][0-9]{2}'/></xs:restriction></xs:simpleType><xs:element name='r' type='D'/>");using var f=new Fixture(schema,"<r>01.20</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Element([]))!;Assert.Equal("D",fact.NativeType.Name);Assert.Equal("N",fact.NativeType.DerivationBase!.Name);Assert.NotNull(fact.NativeType.DerivationBase.NumericPatterns);Assert.Equal("1.2",f.Read(f.Element([])).DecimalValue.ToInvariantString(256));
        var compiled=NativeDecimalSchema.Compile(schema,new(256,256),maxStringCharacters:1024).Value!;Assert.Equal(NativeOutcome.Invalid,compiled.Assess("<r>11.20</r>").Outcome);Assert.Equal(NativeOutcome.Valid,compiled.Assess("<r>01.20</r>").Outcome);
        var complex=Schema(true).Replace("<xs:element name='r' type='N'/>","<xs:complexType name='B'><xs:simpleContent><xs:extension base='N'/></xs:simpleContent></xs:complexType><xs:complexType name='D'><xs:simpleContent><xs:restriction base='B'/></xs:simpleContent></xs:complexType><xs:element name='r' type='D'/>");
        Assert.Equal(NativeOutcome.Unsupported,NativeDecimalSchema.Compile(complex,new(256,256),maxStringCharacters:1024).Outcome);
    }
}
