// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Verifies contextual integer assertions through the public owned facades.
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
           xmlns:t="urn:assertions" targetNamespace="urn:assertions">
  <xs:complexType name="Allowed">
    <xs:simpleContent><xs:extension base="xs:integer">
      <xs:assert test="$value = (1 to 10, 20, 30)"/>
    </xs:extension></xs:simpleContent>
  </xs:complexType>
  <xs:complexType name="EvenAllowed">
    <xs:simpleContent><xs:restriction base="t:Allowed">
      <xs:assert test="$value mod 2 eq 0"/>
    </xs:restriction></xs:simpleContent>
  </xs:complexType>
  <xs:element name="value" type="t:EvenAllowed"/>
</xs:schema>
""";
    var options=new NativeSchemaSessionOptions(256,256,1024,65536,1,32,1,256,256);
    var context=new NativeSchemaAssessmentContext(DateTimeOffset.UnixEpoch,0);
    string Xml(string number)=>"<t:value xmlns:t='urn:assertions'>"+number+"</t:value>";
    using(var session=NativeSchemaSession.Create(schema,options).Value ?? throw new InvalidOperationException("Compile failed"))
    {
        using(var doc=session.Assess(Xml("20"),context).Value ?? throw new InvalidOperationException("Valid rejected"))
            if(doc.ReadRootScalar().IntegerValue!=new BigInteger(20))throw new InvalidOperationException("Wrong exact value");
        foreach(var invalid in new[]{"80","5"})if(session.Assess(Xml(invalid),context).Outcome!=NativeSchemaOutcome.Invalid)throw new InvalidOperationException("Assertion failure admitted");
    }
    using(var session=NativeSchemaNodeDataSession.Create(schema,options).Value ?? throw new InvalidOperationException("Compile failed"))
    {
        using(var doc=session.Assess(Xml("20"),context).Value ?? throw new InvalidOperationException("Valid rejected"))
            if(doc.ReadElement([]).IntegerValue!=new BigInteger(20))throw new InvalidOperationException("Wrong exact value");
        foreach(var invalid in new[]{"80","5"})if(session.Assess(Xml(invalid),context).Outcome!=NativeSchemaOutcome.Invalid)throw new InvalidOperationException("Assertion failure admitted");
    }
    Console.WriteLine("Exact packaged README assertion example passes both facades, inherited rejection80 and local rejection5");return 0;
}
catch(Exception error){Console.Error.WriteLine("Caught example failure: "+error.GetType().Name);return 1;}
