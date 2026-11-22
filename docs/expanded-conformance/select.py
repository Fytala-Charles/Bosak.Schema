"""Select a static expanded public sample before native execution.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import collections,hashlib,importlib.util,io,json,subprocess,xml.etree.ElementTree as E
P=Path(__file__).resolve().parent;ROOT=P.parents[1]
UP=Path("D:/Development/Bosak/.guard-tmp/w3c-xsdtests-20261120")
REV="7bc3365c652a322f3d762021b3879eb92dae7e30"
T="{http://www.w3.org/XML/2004/xml-schema-test-suite/}";X="{http://www.w3.org/2001/XMLSchema}";L="{http://www.w3.org/1999/xlink}href"
sha=lambda b:hashlib.sha256(b).hexdigest()
def dump(name,value):(P/name).write_text(json.dumps(value,indent=2)+"\n",encoding="utf-8")
spec=importlib.util.spec_from_file_location("pilot",ROOT/"tools/run-xsd-conformance-pilot.py");pilot=importlib.util.module_from_spec(spec);spec.loader.exec_module(pilot)
tokens={"1.1","xml-1.0","xml-1.0-5e"};rejected=collections.Counter();groups=[]
old=json.loads((ROOT/"docs/conformance-pilot/CASES.json").read_text(encoding="utf-8"));excluded={(r["metadata"],r["group"]) for r in old["cases"]}
def local(base,href):
    if ":" in href or href.startswith(("/","\\")):raise ValueError("remote-path")
    p=(base/href).resolve()
    if not p.is_relative_to(UP.resolve()) or not p.is_file():raise ValueError("missing-or-outside-file")
    return p

def features(path):
    raw=path.read_bytes().replace(b"\r\n",b"\n");tree=E.fromstring(raw);tags=collections.Counter(e.tag[len(X):] for e in tree.iter() if e.tag.startswith(X));found=set();builtin=set();scopes=[];pending=[]
    for event,node in E.iterparse(io.BytesIO(raw),events=("start-ns","start","end")):
        if event=="start-ns":pending.append(node)
        elif event=="start":
            ns=dict(scopes[-1]) if scopes else {};ns.update(pending);pending=[];scopes.append(ns)
            if node.tag.startswith(X):
                for key in ["base","type","itemType","memberTypes"]:
                    for value in node.get(key,"").split():
                        prefix,sep,name=value.partition(":");uri=ns.get(prefix if sep else "","");name=name if sep else prefix
                        if uri==X[1:-1]:builtin.add(name)
        else:scopes.pop()
    for tag in ["choice","all","pattern","list","union","include","import","redefine","override","alternative","openContent","defaultOpenContent","any","anyAttribute","key","keyref","unique","attributeGroup","group","complexContent","annotation"]:
        if tags[tag]:found.add(tag)
    for node in tree.iter():
        for key in ["default","fixed","nillable","abstract","substitutionGroup","defaultAttributes","defaultAttributesApply"]:
            if node.get(key) is not None:found.add(key)
        if node.get("maxOccurs")=="unbounded":found.add("unbounded")
        if node.get("ref") is not None:found.add("reference")
    for element in tree.findall(X+"element"):
        if element.find(X+"simpleType") is not None or element.get("type","").split(":")[-1] in builtin:found.add("global-simple-root")
    simple_names={e.get("name") for e in tree.findall(X+"simpleType")}
    if any(e.get("type","").split(":")[-1] in simple_names for e in tree.findall(X+"element")):found.add("global-simple-root")
    for value in builtin-{"decimal","string","boolean","integer"}:found.add("builtin:"+value)
    if tags["assertion"]:found.add("simple-assertion")
    if tags["assert"]:found.add("complex-assertion")
    if tags["attribute"]:found.add("attributes")
    return dict(tags=dict(tags),builtins=sorted(builtin),features=sorted(found),elements=sum(tags.values()),schemaBytes=len(raw))

suites=E.parse(UP/"suite.xml").getroot();refs=sorted({e.attrib[L] for e in suites.findall(T+"testSetRef")})
for name in refs:
    meta=UP/name
    try:ts=E.parse(meta).getroot()
    except (E.ParseError,OSError):rejected["metadata-parse"]+=1;continue
    if not pilot.applies(ts,tokens):rejected["testset-version"]+=1;continue
    for group in ts.findall(T+"testGroup"):
        if (name,group.get("name")) in excluded:rejected["original-frozen-group"]+=1;continue
        if not pilot.applies(group,tokens):rejected["group-version"]+=1;continue
        schemas=group.findall(T+"schemaTest")
        if len(schemas)!=1:rejected["not-one-schema-test"]+=1;continue
        st=schemas[0];current=st.find(T+"current")
        if current is None or current.get("status")!="accepted" or not pilot.applies(st,tokens):rejected["schema-status-or-version"]+=1;continue
        try:
            if pilot.expected_value(st,tokens)!="valid":rejected["schema-only-invalid"]+=1;continue
            docs=st.findall(T+"schemaDocument")
            if len(docs)!=1:raise ValueError("not-one-schema-document")
            schema=local(meta.parent,docs[0].attrib[L])
            if schema.stat().st_size>32768:raise ValueError("schema-over-32KiB")
            f=features(schema);instances=[]
            for it in group.findall(T+"instanceTest"):
                c=it.find(T+"current")
                if c is None or c.get("status")!="accepted" or not pilot.applies(it,tokens):continue
                documents=it.findall(T+"instanceDocument")
                if len(documents)!=1:continue
                try:
                    expected=pilot.expected_value(it,tokens);inst=local(meta.parent,documents[0].attrib[L])
                    if inst.stat().st_size>65536:continue
                    instances.append(dict(name=it.get("name"),expected=expected,path=inst.relative_to(UP).as_posix()))
                except (ValueError,OSError):continue
            if not instances:raise ValueError("no-eligible-instance")
            groups.append(dict(metadata=name,testSet=ts.get("name"),group=group.get("name"),schemaTest=st.get("name"),schema=schema.relative_to(UP).as_posix(),instances=instances,**f))
        except (ValueError,E.ParseError,OSError) as error:rejected[str(error) if isinstance(error,ValueError) else type(error).__name__]+=1

buckets=[("choice","SC06",{"choice"}),("all","SC06",{"all"}),("pattern","SC09",{"pattern"}),("date-time","SC07",{"builtin:date","builtin:dateTime","builtin:time"}),("floating-point","SC07",{"builtin:float","builtin:double"}),("qname-binary","SC07",{"builtin:QName","builtin:base64Binary","builtin:hexBinary"}),("integer-families","SC07",{"builtin:int","builtin:unsignedInt","builtin:positiveInteger"}),("list","SC10",{"list"}),("union","SC10",{"union"}),("default-fixed","SC11",{"default","fixed"}),("nil-abstract","SC11",{"nillable","abstract"}),("assembly","SC12",{"include","import"}),("simple-assertion","SC15",{"simple-assertion"}),("type-alternatives","SC16",{"alternative"}),("open-content","SC17",{"openContent","defaultOpenContent"}),("wildcards","SC18",{"any","anyAttribute"}),("default-attributes","SC19",{"defaultAttributes","defaultAttributesApply"}),("identity-substitution","SC20",{"key","keyref","unique","substitutionGroup"})]
selected=[];used=set();inventory=[]
for bucket,area,target in buckets:
    eligible=[g for g in groups if target.intersection(g["features"])]
    ordered=sorted(eligible,key=lambda g:(len(set(g["features"])-target),g["elements"],g["schemaBytes"],g["metadata"],g["group"]))
    chosen=[]
    for g in ordered:
        key=(g["metadata"],g["group"])
        if key in used:continue
        chosen.append(g);used.add(key)
        if len(chosen)==3:break
    inventory.append(dict(bucket=bucket,area=area,targets=sorted(target),eligibleGroups=len(eligible),chosenGroups=[g["metadata"]+"/"+g["group"] for g in chosen]))
    for g in chosen:
        # Prefer one accepted valid and one accepted invalid original instance when available.
        picks=[]
        for validity in ["valid","invalid"]:
            values=sorted((i for i in g["instances"] if i["expected"]==validity),key=lambda i:i["name"])
            if values:picks.append(values[0])
        for it in picks:
            selected.append(dict(id=g["testSet"]+"/"+g["group"]+"/"+it["name"],metadata=g["metadata"],testSet=g["testSet"],group=g["group"],schemaTest=g["schemaTest"],instanceTest=it["name"],schema=g["schema"],instance=it["path"],upstreamSchemaExpected="valid",upstreamExpected=it["expected"],schemaStatus="accepted",instanceStatus="accepted",bucket=bucket,area=area,staticFeatures=g["features"],schemaBytes=g["schemaBytes"]))
files={"suite.xml","00COPYRIGHT"}
for row in selected:files.update(row[k] for k in ["metadata","schema","instance"])
# Keep source fixture bytes exact; imports/includes are measured as closed-schema unsupported,
# not fetched or flattened. No external acquisition is attempted by the native driver.
manifest=dict(date="2026-11-22",sourceCommit="677b5628be9b22c993dddb6ff8dce1b268a282db",upstream=dict(repository="https://github.com/w3c/xsdtests",revision=REV,originalFetchDate="2026-11-20"),configurationTokens=sorted(tokens),selection="Static before execution:18feature buckets;3groups per bucket; minimum unrelated static features, element count, LF-normalized bytes, metadata/group ordering; one accepted valid and invalid instance if available; original15frozen groups excluded",coverageDenominator=None,conformancePercentage=None,schemaOnlyInvalidTests="Excluded; Unsupported is never Invalid",resolution="Caller supplies one original schema; no imports/includes resolved, flattened or fetched",cases=selected,files={})
for name in sorted(files):
    blob=subprocess.check_output(["git","-c","safe.directory="+str(UP),"-C",str(UP),"show",REV+":"+name]);assert blob.replace(b"\r\n",b"\n")== (UP/name).read_bytes().replace(b"\r\n",b"\n"),name
    dest=P/"upstream"/name;dest.parent.mkdir(parents=True,exist_ok=True);dest.write_bytes(blob);manifest["files"][name]=sha(blob)
pilot.verify(manifest,P/"upstream")
dump("INVENTORY.json",dict(metadataSets=len(refs),eligibleGroups=len(groups),selectionRejected=dict(rejected),buckets=inventory,selectedGroups=[g for g in groups if (g["metadata"],g["group"]) in used]))
dump("CASES.json",manifest)
dump("SELECTION-CHECKS.json",dict(status="Passed static selection and original metadata/hash validation before execution",selectedCases=len(selected),selectedGroups=len(used),buckets=len(buckets),originalFrozenGroupsExcluded=len(excluded),files=len(files),manifestSha256=sha((P/"CASES.json").read_bytes()),engineExecuted=False))
print(json.dumps(dict(eligibleGroups=len(groups),selectedGroups=len(used),selectedCases=len(selected),files=len(files),buckets=len(buckets))))
