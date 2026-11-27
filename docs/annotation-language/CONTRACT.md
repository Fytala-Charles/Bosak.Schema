# Bounded documentation annotation language

27 November2026. Recorded before implementation at04de2632a342b42bddf77c713e796dbca5b00dee. ADOPTION.json pins exact immutable sources, including dated2009 XML namespace schema.

Admit the XML namespace lang attribute only on xs:documentation wrappers within already admitted annotation positions. Existing unqualified source and namespace declarations remain admitted. Other wrapper attributes/locations retain their existing Unsupported boundary; do not broaden the common schema Attributes helper.

Validate the XML namespace schema union correctly: the xs:language branch uses XML whitespace collapse followed by ASCII groups [a-zA-Z]{1,8}(-[a-zA-Z0-9]{1,8})*. The xs:string empty branch accepts the exact empty parsed attribute value only. Thus padded valid language tags are accepted, an exact empty undeclaration is accepted, but whitespace-only is Invalid. No IANA/BCP47 registration lookup. The pre-collapse parsed attribute-value ceiling is256UTF16 units; excess is Unsupported. Invalid admitted lexicals make compilation Invalid, including unused definitions, as the selected implementation-defined annotation-descendant policy.

Documentation/appinfo contents stay opaque: do not inspect descendant language values, XSD-named fake definitions, declarations, nil flags or nested annotations in payloads. Existing annotation ownership/leading placement/unique IDs/count/schema byte limits remain. No language component or declaration is created. Source language metadata has no effect on instance validity, actual type/member/value/nil/declaration facts, original XML export or authority. No external source URI is fetched during compilation.

xml:lang on schema/annotation/appinfo/other wrappers, arbitrary foreign wrapper attributes, language inheritance projection and public annotation PSVI remain outside this bounded slice. Legacy grammar, all semantic type/value/assembly/finite/session/cancellation/foreign/expiry and no-augmentation boundaries remain. No full XSD/PSVI/C45 or production acceptance claim.

Qualify happy/edge cases through both facades and native identity/value evidence, original unchanged samples, full source/exact fresh package/API/signed Core/invoice/docs/release controls. Preserve all old input bytes/names and explicitly reconcile any measured expectations. Reviewed commit/push/PR/green merge/main CI precede completion. No publication included.
