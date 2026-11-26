// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Verifies measured historical text outcomes without editing original fixtures.
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
using System.Collections.Immutable;
using System.Xml.Linq;
using Xunit;
namespace Bosak.Schema.PublicConsumer.Tests;
internal static class NativeTextLegacyVerification
{
    internal static void Check(string schema,string xml,NativeSchemaSessionOptions options,NativeSchemaOutcome compile,NativeSchemaOutcome outcome,string? expected=null,bool root=false,bool numeric=false)
    {
        var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
        var c=NativeSchemaSession.Create(schema,options);Assert.Equal(compile,c.Outcome);using var s=c.Value;
        if(s is null)return;
        var a=s.Assess(xml,context);Assert.Equal(outcome,a.Outcome);using var d=a.Value;if(d is null)return;
        var value=root?d.ReadRootScalar():d.ReadScalar(0);Assert.Equal(expected,numeric?value.DecimalValue.ToInvariantString(256):value.StringValue);
        Assert.True(XNode.DeepEquals(XElement.Parse(xml),XElement.Parse(d.ExportXml())));
        var n=NativeSchemaNodeDataSession.Create(schema,options);Assert.Equal(compile,n.Outcome);using var ns=n.Value!;using var nd=ns.Assess(xml,context).Value!;
        var other=nd.ReadElement(root?ImmutableArray<int>.Empty:[0]);Assert.Equal(expected,numeric?other.DecimalValue.ToInvariantString(256):other.StringValue);
    }
}
