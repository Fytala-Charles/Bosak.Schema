// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 20 November 2026
// PURPOSE              : Records actual native behavior before anonymous simple restriction implementation.
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

using Bosak.Schema;
using Xunit;

namespace AnonymousBeforeProbe;

/// <summary>Distinguishes actual baseline refusal from public Core transport feasibility.</summary>
public sealed class BeforeTests
{
    private static NativeSchemaSessionOptions Options=>new(256,256,1024,65536,1,32,1,256,256);
    private static string Read(string name)=>File.ReadAllText(Path.Combine(AppContext.BaseDirectory,name));
    private static string Schema(string family,bool anonymous)=>$"<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>{(anonymous?"":$"<xs:simpleType name='T'><xs:restriction base='xs:{family}'/></xs:simpleType>")}<xs:element name='r'><xs:complexType><xs:sequence><xs:element name='v'{(anonymous?"": " type='T'")}>{(anonymous?$"<xs:simpleType><xs:restriction base='xs:{family}'/></xs:simpleType>":"")}</xs:element></xs:sequence></xs:complexType></xs:element></xs:schema>";

    /// <summary>The two targeted anonymous families still refuse on the unchanged engine.</summary>
    /// <param name="family">Primitive restriction base.</param>
    [Theory][InlineData("decimal")][InlineData("string")]
    public void AnonymousLocalType_StillUnsupported(string family)
    {
        var result=NativeSchemaSession.Create(Schema(family,true),Options);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);Assert.Equal(NativeSchemaUnsupportedReason.DeclarationShape,result.Diagnostics[0].UnsupportedReason);
    }
    /// <summary>Existing direct named restrictions remain admitted and return their original scalar family.</summary>
    /// <param name="family">Admitted named restriction family.</param>
    [Theory][InlineData("decimal")][InlineData("string")]
    public void NamedRestrictions_StillWork(string family)
    {
        using var session=NativeSchemaSession.Create(Schema(family,false),Options).Value!;using var document=session.Assess("<r><v>12.50</v></r>",new(DateTimeOffset.UnixEpoch,0)).Value!;
        Assert.Equal(1,document.ScalarCount);Assert.Equal(family=="decimal"?NativeScalarKind.Decimal:NativeScalarKind.String,document.ReadScalar(0).Kind);
    }
    /// <summary>The pinned prior case remains a compile refusal until implementation.</summary>
    [Fact]
    public void OriginalNs051_RemainsUnsupported()
    {
        var result=NativeSchemaSession.Create(Read("ns051.xsd"),Options);Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);
    }
    /// <summary>The original upstream hint case retains its separately measured validity match.</summary>
    [Fact]
    public void OriginalHintCase_RemainsValid()
    {
        using var session=NativeSchemaSession.Create(Read("ctB073.xsd"),Options).Value!;var result=session.Assess(Read("ctB073.xml"),new(DateTimeOffset.UnixEpoch,0));Assert.Equal(NativeSchemaOutcome.Valid,result.Outcome);using var document=result.Value!;Assert.Equal(1,document.ScalarCount);
    }
}
