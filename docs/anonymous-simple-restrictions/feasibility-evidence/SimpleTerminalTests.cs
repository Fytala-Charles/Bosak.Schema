// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Probes nested complex decimal terminals through public Core APIs.
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

using System.Collections.Immutable;
using System.Numerics;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Providers.Xml;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace AttributeTransportProbe;

/// <summary>Probes nested complex/decimal facts without native XML schema validation.</summary>
public sealed class SimpleTerminalTests
{
    private static Fixture Open()=>new("data($node)",true,false,true);
    private static ProbeAttributeNode Price(Fixture f,string digits)
    {
        var n=f.Input.Nodes.First(n=>n.LocalName=="amount");var value=XdmValue.FromDecimal(new XdmExactDecimal(BigInteger.Parse(digits),0,256,256));
        var facts=new ProbeFacts(f.Provider.Declared,f.Provider.AttributeDeclaration,f.Provider.Advisor!.Origin,f.Issue(value));n.Facts=facts;f.Provider.Advisor.RegisterNode(n,facts);return n;
    }
    private static XdmValue Evaluate(Fixture f,ProbeAttributeNode n)=>f.Session.Run([f.Snapshot],op=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(n))));
    /// <summary>Complex terminal annotations retain exact decimal values and distinct actual atomic provenance.</summary>
    /// <param name="digits">Exact integer-shaped decimal coefficient.</param>
    [Theory][InlineData("0")][InlineData("12")][InlineData("-12")][InlineData("12345678901234567890123456789012345678901234567890")][InlineData("99999999999999999999999999999999999999999999999999")]
    public void NestedComplexScalar_PreservesDeclaredAndActualTypes(string digits)
    {
        using var f=Open();var n=Price(f,digits);Assert.True(n.IsComplexType);Assert.False(n.HasNoTypedValue);Assert.Null(n.SchemaElementDeclaration);var value=Evaluate(f,n);Assert.Equal(BigInteger.Parse(digits),value.ExactDecimalValue.Coefficient);
        Assert.True(SchemaAtomics.TryGetProvenance(value,out var e));Assert.Same(f.Provider.Declared,e!.DeclaredType);Assert.Same(f.Provider.Actual,e.ActualType);Assert.NotSame(e.DeclaredType,e.ActualType);Assert.Same(f.Provider.Advisor!.Origin,e.OriginalAssessment);
    }
    /// <summary>Depth-four scalar-bearing owners carry their original attribute node and a three-segment detached locator.</summary>
    [Fact]
    public void DepthFourAttributeOwner_HasOriginalParentAndThreeSegments()
    {
        using var f=Open();var price=f.Input.Nodes.First(n=>n.LocalName=="amount");ProbeAttributeNode? attr=null;foreach(var v in price.Attributes("currency",""))attr=(ProbeAttributeNode)v.NodeValue;Assert.NotNull(attr);Assert.Same(price,attr!.Parent);
        var fact=new ProbeFacts(f.Provider.Declared,f.Provider.AttributeDeclaration,f.Provider.Advisor!.Origin,f.Issue(XdmValue.FromString("EUR")));attr.Facts=fact;f.Provider.Advisor.RegisterNode(attr,fact);Assert.Equal("EUR",Evaluate(f,attr).StringValue);
        var segments=new List<int>();IXdmNode n=price;while(n.Parent is {} parent){var i=0;foreach(var child in parent.Children(XdmNodeKind.Element)){if(child.NodeValue.IsSameNode(n))break;i++;}segments.Add(i);n=parent;}segments.Reverse();var detached=segments.ToImmutableArray();f.Dispose();Assert.Equal(new[]{0,0,0},detached);
    }
    /// <summary>Authentic complex-terminal facts from another owner authority are refused.</summary>
    [Fact]
    public void ForeignComplexTerminal_IsRefused()
    {
        using var a=Open();using var b=Open();var x=Price(a,"12");Assert.Throws<ArgumentException>(()=>Evaluate(b,x));
    }
    /// <summary>Snapshot closure revokes nested complex/decimal provenance.</summary>
    [Fact]
    public void ComplexTerminalEvidence_ExpiresWithSnapshot()
    {
        using var f=Open();var n=Price(f,"12");var v=Evaluate(f,n);Assert.True(SchemaAtomics.TryGetProvenance(v,out var e));f.Snapshot.Dispose();Assert.Throws<ObjectDisposedException>(()=>e!.ActualType);
    }
}
