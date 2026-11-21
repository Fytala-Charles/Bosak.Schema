// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Verifies integer complex content through the public owned facades.
// SPECIAL NOTES        : Unit tests verifying correctness of the underlying implementation.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.Schema;
using System.Numerics;
try
{
    var schema = """
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           xmlns:t="urn:quantity" targetNamespace="urn:quantity">
  <xs:complexType name="Quantity">
    <xs:simpleContent><xs:extension base="xs:integer"/></xs:simpleContent>
  </xs:complexType>
  <xs:complexType name="SmallQuantity">
    <xs:simpleContent><xs:restriction base="t:Quantity">
      <xs:maxExclusive value="100000000000000000000"/>
    </xs:restriction></xs:simpleContent>
  </xs:complexType>
  <xs:element name="quantity" type="t:SmallQuantity"/>
</xs:schema>
""";
    using var session=NativeSchemaSession.Create(schema,new(256,256,1024,65536,1,32,1,256,256)).Value ?? throw new InvalidOperationException("Compile failed");
    using var doc=session.Assess("<t:quantity xmlns:t='urn:quantity'>9223372036854775808</t:quantity>",new(DateTimeOffset.UnixEpoch,0)).Value ?? throw new InvalidOperationException("Assessment failed");
    if(doc.ReadRootScalar().IntegerValue != BigInteger.Parse("9223372036854775808"))throw new InvalidOperationException("Exact value differs");
    if(session.Assess("<t:quantity xmlns:t='urn:quantity'>4.5</t:quantity>",new(DateTimeOffset.UnixEpoch,0)).Outcome!=NativeSchemaOutcome.Invalid)throw new InvalidOperationException("Fraction accepted");
    Console.WriteLine("README integer example passed exact BigInteger and fractional refusal");return 0;
}
catch(Exception error){Console.Error.WriteLine("Caught example failure: "+error.GetType().Name);return 1;}
