"""Validate the contract without compiling or executing proposed semantics.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
from html.parser import HTMLParser
import json,hashlib,subprocess,re,xml.etree.ElementTree as ET
P=Path(__file__).resolve().parent;ROOT=P.parents[1]
read=lambda f:json.loads(f.read_text(encoding="utf-8-sig"))
sha=lambda b:hashlib.sha256(b).hexdigest()
prior=entries=0
for receipt in (ROOT/"docs").rglob("PACKET-SHA256.json"):
    if receipt.parent==P:continue
    for f,h in read(receipt).items():assert sha((receipt.parent/f).read_bytes())==h,(receipt,f);entries+=1
    prior+=1
spec=read(P/"SPECIFICATION-REVIEW.json")
for row in spec["sources"]:assert sha(Path(row["reviewBodyPath"]).read_bytes())==row["sha256"]
for row in read(P/"SOURCE-REVIEW.json")["rows"]:
    assert sha((P/"reviewed-schema"/row["path"]).read_bytes())==row["gitBlobSha256"]
    b=subprocess.check_output(["git","show",row["commit"]+":"+row["path"]],cwd=ROOT);assert sha(b)==row["gitBlobSha256"]
for row in read(P/"CORE-SOURCE-REVIEW.json")["rows"]:assert sha((P/row["copy"]).read_bytes())==row["gitBlobSha256"]
for f,h in read(P/"ENGINE-INPUTS.json").items():assert sha((ROOT/f).read_bytes())==h,f
class Text(HTMLParser):
    def __init__(self):super().__init__();self.parts=[]
    def handle_data(self,t):self.parts.append(t)
a=Text();body=Path(next(x["reviewBodyPath"] for x in spec["sources"] if x["id"]=="datatypes")).read_bytes();a.feed(body.decode("utf-8"));text=" ".join(" ".join(a.parts).replace("\u00b7"," ").split())
table=read(P/"BUILTIN-TYPES.json");assert table["bodySha256"]==sha(body) and len(table["types"])==12
mapping={x["name"]:x for x in table["types"]};assert len(mapping)==12
for row in table["types"]:
    term=row["section"]+" "+row["name"];positions=[m.start() for m in re.finditer(re.escape(term),text)];assert len(positions)>=2;at=positions[1];end=text.find("3.4."+str(int(row["section"].split(".")[-1])+1)+" ",at+len(term));section=text[at:end]
    assert sha(section.encode())==row["reviewedSectionTextSha256"]
    assert "base type of "+row["name"]+" is "+row["base"] in section
    for facet in ["minInclusive","maxInclusive"]:
        if row[facet] is not None:assert facet+" = "+row[facet] in section
    assert "fractionDigits = 0 (fixed)" in section and "whiteSpace = collapse (fixed)" in section
    seen=set();current=row["name"]
    while current in mapping:
        assert current not in seen;seen.add(current);current=mapping[current]["base"]
    assert current=="integer"
cases=read(P/"CASE-PLAN.json")["cases"];assert len(cases)==158 and len({x["id"] for x in cases})==158
for c in cases:
    ET.fromstring(c["schema"]);ET.fromstring(c["instance"]);assert c["status"].startswith("Proposed unexecuted")
    if c["expectedValue"] is not None:assert str(int(c["expectedValue"]))==c["expectedValue"]
assert len(read(P/"NATIVE-GATES.json")["gates"])==18
examples=read(P/"ORIGINAL-EXAMPLES.json")["rows"];assert len(examples)==3
for row in examples:
    for key,h in row["inputSha256"].items():assert sha((ROOT/"docs/expanded-conformance/cohort2/upstream"/row["case"][key]).read_bytes())==h
frozen=read(ROOT/"docs/conformance-pilot/CASES.json")
for f,h in frozen["files"].items():assert sha((ROOT/"tests/TestData/ConformancePilot/upstream"/f).read_bytes())==h
main=read(P/"MAIN-CI.json");assert main["headSha"]==spec["base"] and main["conclusion"]=="success"
merge=read(P/"PR41-MERGE.json");assert merge["state"]=="MERGED" and merge["mergeCommit"]["oid"]==spec["base"]
assert subprocess.check_output(["git","rev-parse",spec["base"]+"^{tree}"],cwd=ROOT)==subprocess.check_output(["git","rev-parse",merge["headRefOid"]+"^{tree}"],cwd=ROOT)
assert not subprocess.check_output(["git","diff","--name-only","--","src","tests","tools",".github","Directory.Build.props","global.json"],cwd=ROOT).strip()
assert "RESULT: ALL CHECKS PASSED" in (P/"DOCS-CHECK.log").read_text(encoding="utf-8-sig")
docs=read(P/"DOCUMENTATION-CHECKS.json");assert len(docs["pages"])==11 and docs["localLinks"]==667
checks=dict(status="Passed contract evidence validation; proposed semantics unexecuted",base=spec["base"],priorSeals=prior,priorEntries=entries,reviewedSpecificationBodies=4,reviewedSchemaFiles=10,reviewedCoreFiles=8,builtinDefinitions=12,plannedXmlPayloads=158,plannedNativeGates=18,originalExamples=3,frozenFiles=42,engineInputs=len(read(P/"ENGINE-INPUTS.json")),productSourceChanged=False,semanticAdoption=False,mainCi="success",reviewedTreePreserved=True,renderedPages=11,localLinks=667,visualAcceptance=False)
(P/"CHECKS.json").write_text(json.dumps(checks,indent=2)+"\n",encoding="utf-8");print(json.dumps(checks))
