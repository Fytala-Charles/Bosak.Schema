"""Verify caught calendar transport preflight receipts; never launch diagnostic binaries.
Copyright Fytala; SPDX-License-Identifier: Apache-2.0.
"""
from pathlib import Path
import json,hashlib
P=Path(__file__).resolve().parent;ROOT=P.parents[1];sha=lambda b:hashlib.sha256(b).hexdigest();read=lambda p:json.loads(p.read_text(encoding='utf-8-sig'))
packets=entries=0
for p in (ROOT/'docs').rglob('PACKET-SHA256.json'):
 if p.parent==P:continue
 for rel,h in read(p).items():assert sha((p.parent/rel).read_bytes())==h,(p,rel)
 packets+=1;entries+=len(read(p))
assert (packets,entries)==(59,3856),(packets,entries)
inputs=read(ROOT/'docs/date-time-transport/ENGINE-INPUTS.json');assert len(inputs)==323
for rel,h in inputs.items():assert sha((ROOT/rel).read_bytes())==h,rel
for lane in ['minimum','current']:
 d=read(P/lane/'RESULTS.json');assert len(d['rows'])==32
 assert sum(r['componentTransport']=='Pass' for r in d['rows'])==32
 assert sum(r['formatting']=='Pass' for r in d['rows'])==28
 failures=[r for r in d['rows'] if r['formatting']=='Fail'];assert len(failures)==4
 assert all(r['year']==-9223372036854775808 and r['error'].startswith('OverflowException:') for r in failures)
 assert {r['zone'] for r in failures}=={None,0,-840,840}
 assert d['baselineScalar']==d['baselineNodeData']=='Unsupported'
 s=(P/lane/'Probe.csproj').read_text(encoding='utf-8');assert 'ProjectReference' not in s and 'Microsoft.NET.Test.Sdk' not in s
 assert 'NuGetAudit>false' not in (P/'Directory.Build.props').read_text(encoding='utf-8')
a=read(P/'minimum/RESULTS.json');b=read(P/'current/RESULTS.json');assert a['rows']==b['rows']
i=read(P/'IDENTITY.json');assert len(i['lanes'])==2
for lane in i['lanes']:
 assert lane['coreSignatureFreshlyVerified'] and lane['packageOnly'] and len(lane['core'])==9
 assert lane['schemaDllSha256']=='49c3ec63f3082f3b8c857a852c750d8d50df23f40a3c924dbf8490bb460e57bd'
 for row in lane['core'].values():assert row['archiveUnchanged'] and row['signatureVerified'] and row['payloadIdentical']
r=read(P/'BOUND-REVISION-PROPOSAL.json');assert r['proposedYearMinimum']==-9223372036854775807 and r['originalPlanUnchanged'] and r['nativeGatesRemaining']
assert len(read(P/'DOCUMENTATION-CHECKS.json')['pages'])==12
print('Verified59 prior packets/3856 entries,323 unchanged production inputs;32 exact components/28 expected strings/4 caught long.MinValue failures per pin;both native baseline refusals;18 unchanged signed archive/runtime identities and fresh Core signatures. Native validation/facade/facts/lifetime gates remain pending.')
