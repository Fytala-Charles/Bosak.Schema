from pathlib import Path
import json,hashlib,zipfile,xml.etree.ElementTree as ET
D=Path(__file__).resolve().parent;R=D.parents[1]
def load(p):return json.loads(p.read_text(encoding="utf-8"))
def sha(b):return hashlib.sha256(b).hexdigest()
for n,h in load(D/"PACKET-SHA256.json").items():assert sha((D/n).read_bytes())==h,n
for n,h in load(D/"BASELINE-SHA256.json").items():assert sha((D/n).read_bytes())==h,n
ledger=load(D/"CONTRACTS.json");assert len(ledger["contracts"])==100 and len({r["id"] for r in ledger["contracts"]})==100 and all(r["status"]=="qualified" and r["evidence"] and r["tests"] for r in ledger["contracts"])
q=load(D/"QUALIFICATION.json");assert q["sourcePassed"]==11024 and q["packagePublicPassed"]==9516 and q["overlappingCoverage"] and q["unsignedLocalOnly"] and not q["newSchemaPublished"]
assert len(q["core"])==9 and all(c["version"]=="1.86.0" and c["signatureExitCode"]==0 for c in q["core"])
ns={"t":"http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
with zipfile.ZipFile(D/"QUALIFICATION-EVIDENCE.zip") as z:
 for n,h in q["sourceInputs"].items():assert sha(z.read("qualified-source/"+n))==h,n
 assert sha(z.read("packaged-NUGET_README.md"))==q["packageReadmeSha256"]
 for folder,want in [("signed-zero-qualified-source-results",11024),("signed-zero-qualified-package-results",9516)]:
  cs=[ET.fromstring(z.read(n)).find(".//t:Counters",ns).attrib for n in z.namelist() if n.startswith(folder+"/") and n.endswith(".trx")];assert cs and all(c["failed"]=="0" for c in cs) and sum(int(c["passed"]) for c in cs)==want
 cases=json.loads(z.read("contract-tests/cases.json"))["cases"];assert len(cases)==174 and {r["contract"] for r in cases}=={"CAL-"+str(i).zfill(3) for i in range(11,91)}
 for lane in ["pilot","expanded1","expanded2"]:
  result=json.loads(z.read("corpus/"+lane+"/RESULTS.json"));assert result["counts"]["mismatch"]==result["counts"]["harnessError"]==0
for n,h in load(D/"HISTORICAL-PRESERVATION.json").items():assert sha((R/n).read_bytes())==h,n
api=load(D/"API-REVIEW.json");assert api["exportedTypes"]==55 and not api["removed"] and len(api["added"])==5
print("100 bounded contracts, exact source/package receipts, preserved historical audit and additive API verify offline.")
