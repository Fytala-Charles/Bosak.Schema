// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Probes original attribute facts under distinct nested owners through public Core APIs.
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
using System.Xml.Linq;
using Bosak.XPath.Api.Schema;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Providers.Xml;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace AttributeTransportProbe;

/// <summary>Probes original nested attributes with a synthetic issuer, not an XSD validator.</summary>
public sealed class NestedAttributeTests
{
    private static Fixture Open(string expression="data($node)")=>new(expression,true,true);
    private static ProbeAttributeNode Attribute(Fixture f,int owner,XdmValue payload,string namespaceUri="")
    {
        var element=(XElement)f.Input.Nodes[owner==0?1:4].Raw.UnderlyingObject;
        var node=f.Input.Wrap(new XDocumentNode(element.Attribute(XName.Get("value",namespaceUri))!));
        var issued=f.Issue(payload);var facts=new ProbeFacts(f.Provider.Declared,f.Provider.AttributeDeclaration,f.Provider.Advisor!.Origin,issued);
        node.Facts=facts;f.Provider.Advisor.RegisterNode(node,facts);return node;
    }
    private static XdmValue Evaluate(Fixture f,ProbeAttributeNode node)=>f.Session.Run([f.Snapshot],op=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(node))));
    private static ImmutableArray<int> PathTo(IXdmNode owner)
    {
        var result=new List<int>();var node=owner;
        while(node.Parent is { } parent)
        {
            var i=0;var found=false;foreach(var child in parent.Children(XdmNodeKind.Element))
            {
                if(child.NodeValue.IsSameNode(node)){found=true;break;}i++;
            }
            Assert.True(found);result.Add(i);node=parent;
        }
        result.Reverse();return result.ToImmutableArray();
    }
    /// <summary>Repeated owners share use/type identity but retain distinct original attribute nodes and payloads.</summary>
    /// <param name="kind">Scalar payload family/case.</param>
    [Theory][InlineData(0)][InlineData(1)][InlineData(2)][InlineData(3)][InlineData(4)][InlineData(5)][InlineData(6)][InlineData(7)]
    public void RepeatedOwners_RetainOriginalAttributePayloadAndProvenance(int kind)
    {
        using var f=Open();var plain=TransportTests.Plain(kind);var a=Attribute(f,0,plain);var b=Attribute(f,1,plain);
        Assert.NotSame(a,b);Assert.False(a.IsSameNode(b));Assert.Same(a.OriginalSchemaFacts.Type,b.OriginalSchemaFacts.Type);Assert.Same(a.OriginalSchemaFacts.Declaration,b.OriginalSchemaFacts.Declaration);
        foreach(var node in new[]{a,b})
        {
            var value=Evaluate(f,node);TransportTests.SamePayload(plain,value);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(f.Provider.Declared,evidence!.DeclaredType);Assert.Same(f.Provider.Actual,evidence.ActualType);Assert.Same(f.Provider.Advisor!.Origin,evidence.OriginalAssessment);
        }
    }
    /// <summary>Namespaced/unqualified attributes retain the exact original owner through public navigation.</summary>
    /// <param name="owner">Repeated owner ordinal.</param>
    /// <param name="namespaceUri">Expanded attribute namespace.</param>
    [Theory][InlineData(0,"")][InlineData(1,"")][InlineData(0,"urn:attributes")][InlineData(1,"urn:attributes")]
    public void AttributeNavigation_RetainsExpandedNameAndOwner(int owner,string namespaceUri)
    {
        using var f=Open();var node=Attribute(f,owner,XdmValue.FromString("owner"+owner),namespaceUri);var element=f.Input.Nodes[owner==0?1:4];
        Assert.Equal(XdmNodeKind.Attribute,node.NodeKind);Assert.Equal(namespaceUri,node.NamespaceUri);Assert.Equal("value",node.LocalName);Assert.Same(element,node.Parent);
        var found=false;foreach(var value in element.Attributes("value",namespaceUri))if(value.NodeValue.IsSameNode(node))found=true;Assert.True(found);
        Assert.Equal("owner"+owner,Evaluate(f,node).StringValue);Assert.Equal(new[]{owner},PathTo(element));
    }
    /// <summary>Swapping same-authority authentic facts between equal-named attributes cannot authenticate another node.</summary>
    [Fact]
    public void SameAuthorityOtherOwnerFacts_AreRefused()
    {
        using var f=Open();var a=Attribute(f,0,XdmValue.FromString("EUR"));var b=Attribute(f,1,XdmValue.FromString("USD"));var original=b.Facts;b.Facts=a.Facts;
        Assert.Throws<ArgumentException>(()=>Evaluate(f,b));b.Facts=original;Assert.Equal("USD",Evaluate(f,b).StringValue);Assert.Equal("EUR",Evaluate(f,a).StringValue);
    }
    /// <summary>Foreign nodes and facts remain outside the selected input and authority.</summary>
    [Fact]
    public void ForeignNestedAttributeAndFacts_AreRefused()
    {
        using var a=Open();using var b=Open();var x=Attribute(a,0,XdmValue.FromString("EUR"));var y=Attribute(b,0,XdmValue.FromString("EUR"));
        Assert.Throws<ArgumentException>(()=>Evaluate(b,x));y.Facts=x.Facts;Assert.Throws<ArgumentException>(()=>Evaluate(b,y));
    }
    /// <summary>Metadata without issued nested attribute facts cannot manufacture validation.</summary>
    [Fact]
    public void MissingNestedAttributeFacts_AreRefused()
    {
        using var f=Open();var node=Attribute(f,0,XdmValue.FromString("EUR"));node.Facts=null;Assert.Throws<InvalidOperationException>(()=>Evaluate(f,node));
    }
    /// <summary>Snapshot closure revokes issued nested attribute provenance.</summary>
    [Fact]
    public void NestedAttributeEvidence_ExpiresWithSnapshot()
    {
        using var f=Open();var node=Attribute(f,0,XdmValue.FromString("EUR"));var value=Evaluate(f,node);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));f.Snapshot.Dispose();Assert.Throws<ObjectDisposedException>(()=>evidence!.ActualType);
    }
    /// <summary>Closed input cannot supply a new original nested attribute read.</summary>
    [Fact]
    public void ClosedNestedAttributeInput_IsRefused()
    {
        using var f=Open();var node=Attribute(f,0,XdmValue.FromString("EUR"));f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>Evaluate(f,node));
    }
    /// <summary>Detached index-path prototype and primitive values survive owner disposal without conveying authority.</summary>
    [Fact]
    public void DetachedPathPrototype_DistinguishesOwnersAndSurvivesClose()
    {
        using var f=Open();var a=Attribute(f,0,XdmValue.FromString("EUR"));var b=Attribute(f,1,XdmValue.FromString("USD"));
        var root=PathTo(f.Input.RootNode);var first=PathTo(a.Parent!);var second=PathTo(b.Parent!);var detached=Evaluate(f,b).StringValue;f.Dispose();
        Assert.False(root.IsDefault);Assert.Empty(root);Assert.Equal(new[]{0},first);Assert.Equal(new[]{1},second);Assert.Equal("USD",detached);Assert.False(first.IsDefault);
    }
    /// <summary>The facade's existing typed identity route can consume original nested attribute-issued values.</summary>
    [Fact]
    public void NestedAttributeIdentity_UsesExistingPublicRoute()
    {
        using var f=Open("$value");var node=Attribute(f,1,XdmValue.FromBoolean(false));
        f.Session.Run([f.Snapshot],op=>
        {
            var read=op.ReadTypedValue(f.Snapshot,node.OriginalSchemaFacts.ReadTypedValue(default).Value);Assert.Equal(SchemaResultKind.Supported,read.Kind);Assert.False(op.Evaluate(new EvaluationContext().WithVariable("value",read.Value)).BooleanValue);return 0;
        });
    }
}
