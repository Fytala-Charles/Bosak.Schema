// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Records the pre-adoption nested schema refusal through public Schema APIs.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 19-11-2026     | Probe public original attribute facts, typed reads and ownership refusal                   |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Keep synthetic attribute declaration identity distinct from type identities                |
//                      | Charles Korthout | 0.4   | 20-11-2026     | Qualify complex simple-content root facts with separate actual decimal types               |
//                      | Charles Korthout | 0.5   | 20-11-2026     | Probe nested navigation, container refusal and original leaf facts                         |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using Xunit;

/// <summary>Records baseline refusal before nested implementation.</summary>
public sealed class BeforeTests
{
    /// <summary>The new representative nested schema refuses on the unchanged delivered engine.</summary>
    [Fact]
    public void NewNestedFixture_IsUnsupportedBeforeImplementation()
    {
        var schema=File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"nested-order.xsd"));
        var result=NativeSchemaSession.Create(schema,new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256));
        Assert.Equal(NativeSchemaOutcome.Unsupported,result.Outcome);Assert.Null(result.Value);
        Assert.Equal("BSK-NATIVE-SCHEMA-UNSUPPORTED",Assert.Single(result.Diagnostics).Code);
    }
}
