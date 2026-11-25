// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 25 November 2026
// PURPOSE              : Verifies actual list and union item authority, identity and revocation.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 25-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Core.Xdm;
using Fixture=Bosak.Schema.Tests.NodeData.Fixture;
using Xunit;
namespace Bosak.Schema.Tests;

/// <summary>Checks retained list boundaries and selected-member provenance through actual Core.</summary>
public sealed class NativeCompoundProofTests
{
    private const string Start="<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>";
    private const string Types="<xs:simpleType name='A'><xs:restriction base='xs:integer'/></xs:simpleType><xs:simpleType name='B'><xs:restriction base='xs:boolean'/></xs:simpleType><xs:simpleType name='U'><xs:union memberTypes='A B'/></xs:simpleType><xs:simpleType name='L'><xs:list itemType='U'/></xs:simpleType>";
    /// <summary>Checks ordered actual member types and governing list proof, including zero/singleton lists.</summary>
    /// <param name="text">List text.</param><param name="count">Expected item count.</param>
    [Theory][InlineData("",0)][InlineData("1",1)][InlineData("1 true",2)]
    public void ListProof(string text,int count)
    {
        using var f=new Fixture(Start+Types+"<xs:element name='r' type='L'/></xs:schema>","<r>"+text+"</r>",atoms:16);
        Assert.Equal(SchemaAssessmentStatus.Valid,f.Assessment.Status);
        var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;var typed=fact.ReadTypedValue(default).Value;
        Assert.Same(f.Input.Root,fact.Node);Assert.True(typed.IsList);Assert.False(typed.TryGetSingleAtomic(out _));Assert.Same(fact.Type,typed.GoverningType);
        var cells=typed.AtomicItems.ToArray();Assert.Equal(count,cells.Length);
        for(int i=0;i<count;i++){Assert.Same(fact.Type,cells[i].TypeEvidence.DeclaredType);Assert.Same(f.Provider.ResolveType(new("",i==0?"A":"B"),default).Value,cells[i].TypeEvidence.ActualType);Assert.Same(f.Assessment.Origin,cells[i].TypeEvidence.OriginalAssessment);}
        f.Run(op=>
        {
            var value=f.Evaluate(op,f.Input.Root);var values=new List<XdmValue>();
            if(value.Kind==XdmValueKind.Sequence){foreach(var atom in XdmSequence.FromSource(value.SequenceValue!))values.Add(atom);}
            else if(value.Kind!=XdmValueKind.Undefined)values.Add(value);
            Assert.Equal(count,values.Count);
            for(int i=0;i<count;i++){Assert.True(SchemaAtomics.TryGetProvenance(values[i],out var proof));Assert.Same(cells[i].TypeEvidence.ActualType,proof!.ActualType);Assert.Same(fact.Type,proof.DeclaredType);}
            return true;
        });
        var enumerator=typed.AtomicItems.GetEnumerator();f.Input.Dispose();Assert.Throws<ObjectDisposedException>(()=>enumerator.MoveNext());Assert.Throws<ObjectDisposedException>(()=>typed.TryGetSingleAtomic(out _));Assert.Throws<ObjectDisposedException>(()=>fact.ReadTypedValue(default));
    }
    /// <summary>Checks union governing identity and active basic member, including nested unions.</summary>
    /// <param name="nested">Nested member selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void UnionProof(bool nested)
    {
        var schema=Start+Types+(nested?"<xs:simpleType name='N'><xs:union memberTypes='U xs:string'/></xs:simpleType>":"")+"<xs:element name='r' type='"+(nested?"N":"U")+"'/></xs:schema>";
        using var f=new Fixture(schema,"<r>1</r>");var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;var typed=fact.ReadTypedValue(default).Value;
        Assert.False(typed.IsList);Assert.True(typed.TryGetSingleAtomic(out var cell));Assert.Same(fact.Type,typed.GoverningType);Assert.NotSame(fact.Type,cell.TypeEvidence.ActualType);Assert.Same(f.Provider.ResolveType(new("","A"),default).Value,cell.TypeEvidence.ActualType);
        f.Run(op=>{var atom=f.Evaluate(op,f.Input.Root);Assert.True(SchemaAtomics.TryGetProvenance(atom,out var proof));Assert.Same(fact.Type,proof!.DeclaredType);Assert.Same(cell.TypeEvidence.ActualType,proof.ActualType);return true;});
        using var other=new Fixture(schema,"<r>1</r>");Assert.Throws<ArgumentException>(()=>other.Provider.IsSameComponent(cell.TypeEvidence.ActualType,other.Provider.DecimalType,default));Assert.False(other.Session.Advisor!.OwnsTypedValue(typed,other.Input,null));
    }
    /// <summary>Checks a union selecting a list retains union governing type and per-item actual proof.</summary>
    [Fact]
    public void SelectedListProof()
    {
        using var f=new Fixture(Start+Types+"<xs:simpleType name='V'><xs:union memberTypes='L xs:boolean'/></xs:simpleType><xs:element name='r' type='V'/></xs:schema>","<r>1 true</r>",atoms:16);
        var fact=f.Input.FactsFor((NativeSessionNode)f.Input.Root)!;var typed=fact.ReadTypedValue(default).Value;Assert.True(typed.IsList);Assert.Same(fact.Type,typed.GoverningType);var cells=typed.AtomicItems.ToArray();Assert.Equal(2,cells.Length);Assert.All(cells,c=>Assert.Same(fact.Type,c.TypeEvidence.DeclaredType));Assert.NotSame(cells[0].TypeEvidence.ActualType,cells[1].TypeEvidence.ActualType);
    }
    /// <summary>Checks shared named members stay canonical across original nodes.</summary>
    [Fact]
    public void SharedIdentity()
    {
        using var f=new Fixture(Start+Types+"<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='a' type='U'/><xs:element name='b' type='U'/></xs:sequence></xs:complexType></xs:element></xs:schema>","<r><a>1</a><b>2</b></r>");
        var a=f.Input.FactsFor((NativeSessionNode)f.Element([0]))!;var b=f.Input.FactsFor((NativeSessionNode)f.Element([1]))!;Assert.Same(a.Type,b.Type);Assert.NotSame(a.Node,b.Node);Assert.Same(a.ReadTypedValue(default).Value.AtomicItems.Single().TypeEvidence.ActualType,b.ReadTypedValue(default).Value.AtomicItems.Single().TypeEvidence.ActualType);
    }
    /// <summary>Checks complete invalid lists/unions publish no partial facts.</summary>
    /// <param name="type">Compound type.</param><param name="text">Invalid text after a valid prefix.</param>
    [Theory][InlineData("L","1 nope")][InlineData("U","nope")]
    public void InvalidFacts(string type,string text)
    {
        using var f=new Fixture(Start+Types+"<xs:element name='r' type='"+type+"'/></xs:schema>","<r>"+text+"</r>");Assert.Equal(SchemaAssessmentStatus.Invalid,f.Assessment.Status);Assert.Empty(f.Assessment.Facts);Assert.Null(f.Snapshot);Assert.Null(f.Input.FactsFor((NativeSessionNode)f.Input.Root));
    }
    /// <summary>Checks a selected string member preserves whitespace in original node string facts.</summary>
    /// <param name="content">Simple-content owner selector.</param>
    [Theory][InlineData(false)][InlineData(true)]
    public void SelectedStringNormalization(bool content)
    {
        var definition="<xs:simpleType name='U'><xs:union memberTypes='xs:string xs:integer'/></xs:simpleType>";
        var root=content?"<xs:element name='r'><xs:complexType><xs:simpleContent><xs:extension base='U'/></xs:simpleContent></xs:complexType></xs:element>":"<xs:element name='r' type='U'/>";
        using var f=new Fixture(Start+definition+root+"</xs:schema>","<r> a </r>");
        Assert.Equal(" a ",f.Input.Root.StringValue);Assert.Equal(" a ",f.Read(f.Input.Root).StringValue);
    }
}
