"""Verify the immutable strict attribute wildcard qualification packet offline."""
from pathlib import Path
import hashlib
import json
import xml.etree.ElementTree as ET
import zipfile

D = Path(__file__).resolve().parent
R = D.parent.parent
N = {"t": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}


def read(name):
    return json.loads((D / name).read_text(encoding="utf-8"))


def digest(raw):
    return hashlib.sha256(raw).hexdigest()


for name, expected in read("PACKET-SHA256.json").items():
    assert digest((D / name).read_bytes()) == expected, name
for name, expected in read("BUILD-CONTEXT.json").items():
    raw = (R / name).read_bytes()
    lf = raw.replace(b"\r\n", b"\n")
    assert expected in {digest(raw), digest(lf), digest(lf.replace(b"\n", b"\r\n"))}, name

q = read("QUALIFICATION.json")
assert q["sourcePassed"] == 23953 and q["packagePublicPassed"] == 22127
assert q["sourceInternal"] == 1826 and q["sourcePublic"] == 22127
assert len(q["core"]) == 9 and all(x["version"] == "1.86.0" for x in q["core"])
assert q["unsignedLocalOnly"] and not q["newSchemaPublished"]
api = read("API-REVIEW.json")
assert api["exportedTypes"] == 55 and api["candidateRows"] == 663
assert not api["added"] and not api["removed"]
contracts = read("CONTRACTS.json")
assert contracts["count"] == len(contracts["contracts"]) == 74
tests = read("TEST-RESULTS.json")
assert tests["priorNamesPreserved"] == 23793 and tests["newNames"] == 160
with zipfile.ZipFile(D / "EXECUTION-EVIDENCE.zip") as z:
    for row in tests["lanes"]:
        raw = z.read("tests/" + row["lane"] + ".trx")
        assert digest(raw) == row["trxSha256"]
        counters = ET.fromstring(raw).find(".//t:Counters", N).attrib
        assert int(counters["passed"]) == row["passed"] and counters["failed"] == "0"
history = read("HISTORICAL-RECONCILIATION.json")
assert history["publicChanged"] == 4 and history["internalChanged"] == 6
for row in history["records"]:
    assert digest((R / row["originalFile"]).read_bytes()) == row["originalFileSha256"]
for row in read("CORPUS-DELTA.json"):
    assert row["counts"]["mismatch"] == row["counts"]["harnessError"] == 0
assert read("DATATYPE-RESULTS.json")["counts"].get("mismatch", 0) == 0
assert read("APPLICATION-TRIALS.json")["productionUserAcceptance"] is False
assert not read("REVIEW.json")["blockingFindings"]
print("PASS: sealed inputs, source/package results, API, Core identities and original fixtures")

for name, expected in history["originalInputsSha256"].items():
    assert digest((R / name).read_bytes()) == expected, name
