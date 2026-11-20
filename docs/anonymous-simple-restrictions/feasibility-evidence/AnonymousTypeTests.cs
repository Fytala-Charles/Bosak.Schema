// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Probes anonymous simple-type identity through public Core node and scalar transport.
// SPECIAL NOTES        : Synthetic issuer transport controls; no XML schema validation is performed.
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

using System.Numerics;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Runtime.Vm;
using Xunit;

namespace AttributeTransportProbe;

/// <summary>Exercises anonymous simple types without substituting named primitive handles.</summary>
public sealed class AnonymousTypeTests
{
    private static XdmValue Payload(int kind)=>kind switch
    {
        0=>XdmValue.FromString(""),1=>XdmValue.FromString("\U0001F600e\u0301 \r\n\t"),
        2=>XdmValue.FromDecimal(new XdmExactDecimal(0,0,256,256)),
        _=>XdmValue.FromDecimal(new XdmExactDecimal(BigInteger.Parse("12345678901234567890123456789012345678901234567890"),3,256,256))
    };
    private static ProbeValue Bind(Fixture f,ProbeAttributeNode node,XdmValue payload,Handle type,Handle declaration)
    {
        f.Provider.Extra.UnionWith([type,declaration]);
        var value=new ProbeValue(type,new ProbeEvidence(type,type,f.Provider.Advisor!.Origin),payload);f.Provider.Advisor.Values.Add(value);
        var facts=new ProbeFacts(type,declaration,f.Provider.Advisor.Origin,value);node.Facts=facts;f.Provider.Advisor.RegisterNode(node,facts);return value;
    }
    private static XdmValue Evaluate(Fixture f,ProbeAttributeNode node)=>f.Session.Run([f.Snapshot],op=>op.Evaluate(new EvaluationContext().WithVariable("node",XdmValue.FromNode(node))));
    private static ProbeAttributeNode Leaf(Fixture f)=>f.Input.Nodes.First(n=>n.LocalName=="amount");
    private static void Evidence(Fixture f,XdmValue value,Handle type)
    {
        Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));Assert.Same(type,evidence!.DeclaredType);Assert.Same(type,evidence.ActualType);Assert.Same(f.Provider.Advisor!.Origin,evidence.OriginalAssessment);
        Assert.NotSame(f.Provider.Declared,evidence.DeclaredType);Assert.NotSame(f.Provider.Actual,evidence.ActualType);
    }

    /// <summary>Anonymous node atomization retains original type identity and exact string/decimal payload.</summary>
    /// <param name="kind">String or decimal boundary payload.</param>
    [Theory][InlineData(0)][InlineData(1)][InlineData(2)][InlineData(3)]
    public void AnonymousNodeAtomization_PreservesUnnamedOriginalType(int kind)
    {
        using var f=new Fixture("data($node)",true);var node=Leaf(f);var type=new Handle(f.Provider);var payload=Payload(kind);Bind(f,node,payload,type,new(f.Provider));
        Assert.False(node.IsComplexType);Assert.False(node.HasNoTypedValue);Assert.Null(node.SchemaTypeAnnotation);Assert.Null(node.SchemaElementDeclaration);
        Assert.Same(type,node.OriginalSchemaFacts.Type);var result=Evaluate(f,node);TransportTests.SamePayload(payload,result);Evidence(f,result,type);
    }

    /// <summary>Direct typed reads retain the same original anonymous governing and actual type.</summary>
    /// <param name="kind">String or decimal boundary payload.</param>
    [Theory][InlineData(0)][InlineData(1)][InlineData(2)][InlineData(3)]
    public void AnonymousTypedRead_PreservesOriginalHandle(int kind)
    {
        using var f=new Fixture("$value",true);var type=new Handle(f.Provider);var payload=Payload(kind);var issued=Bind(f,Leaf(f),payload,type,new(f.Provider));
        Assert.Same(type,issued.GoverningType);
        f.Session.Run([f.Snapshot],op=>{var read=op.ReadTypedValue(f.Snapshot,issued);Assert.Equal(SchemaResultKind.Supported,read.Kind);var actual=op.Evaluate(new EvaluationContext().WithVariable("value",read.Value));TransportTests.SamePayload(payload,actual);Evidence(f,actual,type);return 0;});
    }

    /// <summary>Repeated original declarations share their anonymous type while equal distinct definitions do not.</summary>
    [Fact]
    public void RepeatedAndDistinctDefinitions_RetainSourceIdentity()
    {
        using var f=new Fixture("data($node)",true);var nodes=f.Input.Nodes.Where(n=>n.LocalName=="amount").ToArray();var other=f.Input.Nodes.First(n=>n.LocalName=="quantity");
        var shared=new Handle(f.Provider);var distinct=new Handle(f.Provider);var declaration=new Handle(f.Provider);var payload=Payload(3);
        foreach(var n in nodes)Bind(f,n,payload,shared,declaration);Bind(f,other,payload,distinct,new(f.Provider));
        foreach(var n in nodes){Assert.Same(shared,n.OriginalSchemaFacts.Type);Assert.Same(declaration,n.OriginalSchemaFacts.Declaration);Evidence(f,Evaluate(f,n),shared);}
        Evidence(f,Evaluate(f,other),distinct);Assert.NotSame(nodes[0].OriginalSchemaFacts.Type,other.OriginalSchemaFacts.Type);
    }

    /// <summary>A foreign original anonymous node is rejected by another session.</summary>
    [Fact]
    public void ForeignAnonymousNode_IsRefused()
    {
        using var a=new Fixture("data($node)",true);using var b=new Fixture("data($node)",true);var n=Leaf(a);Bind(a,n,Payload(1),new(a.Provider),new(a.Provider));
        Assert.Throws<ArgumentException>(()=>Evaluate(b,n));
    }

    /// <summary>Anonymous typed values cannot transfer authority to a different session.</summary>
    [Fact]
    public void ForeignAnonymousValue_IsRefused()
    {
        using var a=new Fixture("data($node)",true);using var b=new Fixture("$value",true);var n=Leaf(a);Bind(a,n,Payload(3),new(a.Provider),new(a.Provider));var value=Evaluate(a,n);
        Assert.Throws<ArgumentException>(()=>b.Session.Run([b.Snapshot],op=>op.Evaluate(new EvaluationContext().WithVariable("value",value))));
    }

    /// <summary>Snapshot closure revokes anonymous provenance while detached primitive payloads remain usable.</summary>
    /// <param name="kind">Decimal or string payload.</param>
    [Theory][InlineData(1)][InlineData(3)]
    public void AnonymousEvidence_ExpiresWithoutDestroyingDetachedPayload(int kind)
    {
        using var f=new Fixture("data($node)",true);var n=Leaf(f);Bind(f,n,Payload(kind),new(f.Provider),new(f.Provider));var value=Evaluate(f,n);Assert.True(SchemaAtomics.TryGetProvenance(value,out var evidence));
        var text=value.Kind==XdmValueKind.String?value.StringValue:value.ExactDecimalValue.ToInvariantString(256);f.Snapshot.Dispose();Assert.Throws<ObjectDisposedException>(()=>evidence!.ActualType);Assert.NotEmpty(text);
    }
}
