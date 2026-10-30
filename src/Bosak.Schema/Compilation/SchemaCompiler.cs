// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 27 October 2026
// PURPOSE              : REQ-009 S1 compile pipeline: schema documents to immutable compiled models.
// SPECIAL NOTES        : Part of the REQ-009 XSD 1.1 staged support (ADR-005). Parses with XmlReader
//                        (never XmlSchemaSet); XSD 1.0 documents are untouched by the 1.1 paths; every
//                        rejection or unsupported construct is an explicit diagnostic.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |

//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 27-10-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 27-10-2026     | Digest made whitespace-insensitive (whitespace-only nodes dropped before hashing)        |
//                      | Charles Korthout | 0.3   | 27-10-2026     | Captures in-scope namespace bindings + xpathDefaultNamespace on assertions (REQ-009 S2)  |
//                      | Charles Korthout | 0.4   | 27-10-2026     | Records typed xs:element declarations for the S2 typed view (REQ-009 S2)                 |
//                      | Charles Korthout | 0.5   | 27-10-2026     | Records named xs:simpleType + owned xs:assertion facets (REQ-009 S3)                     |
//                      | Charles Korthout | 0.6   | 27-10-2026     | Records xs:openContent / xs:defaultOpenContent with XSD 1.1 rule validation (REQ-009 S5a)|
//                      | Charles Korthout | 0.7   | 27-10-2026     | Records xs:schema defaultAttributes + complexType defaultAttributesApply (REQ-009 S5b)   |
//                      | Charles Korthout | 0.8   | 27-10-2026     | Records xs:alternative with owner/static context + resolved type (REQ-009 S4a)          |
//                      | Charles Korthout | 0.9   | 27-10-2026     | Records owning type/element on xs:assert for S4b applicability filtering                 |
//                      | Charles Korthout | 0.10  | 29-10-2026     | Publish schema-only Apache-2.0 source; remove activation dependency                       |
//                      | Charles Korthout | 0.11  | 30-10-2026     | Records wildcard notNamespace / notQName with §3.10 representation checks (REQ-009 S5c)|
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Bosak.Schema;

/// <summary>
/// Default <see cref="IXsdSchemaCompiler"/> (REQ-009 S1). Produces immutable <see cref="CompiledSchema"/>
/// models with a stable digest and explicit diagnostics; assertion/alternative components are recorded,
/// their evaluation lands in phases S2/S3.
/// </summary>
public sealed class SchemaCompiler : IXsdSchemaCompiler
{
    private const string XmlSchemaNamespace = "http://www.w3.org/2001/XMLSchema";
    private const int MaxResolutionCountPerCompile = 64;

    /// <inheritdoc/>
    public ICompiledSchema Compile(XmlReader reader, Uri baseUri, SchemaCompileOptions options)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(baseUri);
        ArgumentNullException.ThrowIfNull(options);
        if (!baseUri.IsAbsoluteUri)
        {
            throw new ArgumentException("The base URI must be absolute.", nameof(baseUri));
        }

        var document = XDocument.Load(reader);
        return CompileCore(document, baseUri, options);
    }

    /// <inheritdoc/>
    public ICompiledSchema Compile(Stream stream, Uri baseUri, SchemaCompileOptions options)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(baseUri);
        ArgumentNullException.ThrowIfNull(options);
        if (!baseUri.IsAbsoluteUri)
        {
            throw new ArgumentException("The base URI must be absolute.", nameof(baseUri));
        }

        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null,
            MaxCharactersFromEntities = 1024,
        };
        using var reader = XmlReader.Create(stream, settings);
        return Compile(reader, baseUri, options);
    }

    private static ICompiledSchema CompileCore(XDocument document, Uri baseUri, SchemaCompileOptions options)
    {
        var digest = ComputeDigest(document);
        var state = new CompilationState(digest);

        var root = document.Root;
        if (root is null || root.Name != XName.Get("schema", XmlSchemaNamespace))
        {
            state.Diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.RootNotSchema,
                "The document root is not an xs:schema element in the XML Schema namespace.",
                null));
        }
        else
        {
            VisitDocument(root, baseUri, options, state, depth: 0);
        }

        return new CompiledSchema(
            SchemaId: "xsd-" + digest[..16],
            Sha256Digest: digest,
            LanguageVersion: options.Version,
            TargetNamespace: root?.Attribute("targetNamespace")?.Value,
            Assertions: state.Assertions,
            Alternatives: state.Alternatives,
            ElementDeclarations: state.ElementDeclarations,
            SimpleTypes: state.SimpleTypes
                .Select(c => new SimpleTypeDeclaration(c.TypeNamespace, c.TypeLocalName, c.BaseTypeNamespace, c.BaseTypeLocalName, c.Assertions, c.Path))
                .ToList(),
            OpenContents: state.OpenContents,
            DefaultOpenContents: state.DefaultOpenContents,
            DefaultAttributeReferences: state.DefaultAttributeReferences,
            DefaultAttributeOptOuts: state.DefaultAttributeOptOuts,
            Diagnostics: state.Diagnostics);
    }

    private static string ComputeDigest(XDocument document)
    {
        // Canonical form: formatting-insensitive re-serialization of the parsed tree. Whitespace-only
        // text nodes (indentation) are dropped first, so identical documents digest identically
        // regardless of source formatting or encoding; meaningful text content is preserved.
        var root = document.Root;
        if (root is null)
        {
            return Convert.ToHexString(SHA256.HashData([])).ToLowerInvariant();
        }

        var canonicalRoot = new XElement(root);
        foreach (var text in canonicalRoot.DescendantNodes().OfType<XText>().ToList())
        {
            if (string.IsNullOrWhiteSpace(text.Value))
            {
                text.Remove();
            }
        }

        var canonical = canonicalRoot.ToString(SaveOptions.DisableFormatting);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(canonical));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static void VisitDocument(XElement root, Uri baseUri, SchemaCompileOptions options, CompilationState state, int depth)
    {
        VisitElement(root, root.Name.LocalName, baseUri, options, state, depth, null);
    }

    private static void VisitElement(XElement element, string path, Uri baseUri, SchemaCompileOptions options, CompilationState state, int depth, SimpleTypeBuildContext? ownerType)
    {
        var xs = element.Name.NamespaceName == XmlSchemaNamespace;
        var local = element.Name.LocalName;

        if (xs)
        {
            switch (local)
            {
                case "assert":
                    if (options.Version == XsdLanguageVersion.Xsd10)
                    {
                        RequiresXsd11(state, path, "xs:assert");
                    }
                    else
                    {
                        state.Assertions.Add(new AssertionDefinition(
                            element.Attribute("test")?.Value ?? string.Empty,
                            AssertionContext.ComplexTypeAssert,
                            path,
                            CaptureNamespaceBindings(element),
                            ResolveXPathDefaultNamespace(element),
                            FindOwningComplexTypeName(element),
                            FindOwningElementName(element)));
                    }

                    break;

                case "assertion":
                    if (options.Version == XsdLanguageVersion.Xsd10)
                    {
                        RequiresXsd11(state, path, "xs:assertion");
                    }
                    else
                    {
                        var assertion = new AssertionDefinition(
                            element.Attribute("test")?.Value ?? string.Empty,
                            AssertionContext.SimpleTypeAssertion,
                            path,
                            CaptureNamespaceBindings(element),
                            ResolveXPathDefaultNamespace(element));
                        state.Assertions.Add(assertion);
                        ownerType?.Assertions.Add(assertion);
                    }

                    break;

                case "simpleType":
                    {
                        var context = CreateSimpleTypeContext(element, path, state);
                        if (context is not null)
                        {
                            foreach (var child in element.Elements())
                            {
                                VisitElement(child, ChildPath(path, child), baseUri, options, state, depth, context);
                            }

                            return;
                        }
                    }

                    break;

                case "alternative":
                    if (options.Version == XsdLanguageVersion.Xsd10)
                    {
                        RequiresXsd11(state, path, "xs:alternative");
                    }
                    else
                    {
                        RecordAlternative(element, path, state);
                    }

                    break;

                case "openContent":
                    if (options.Version == XsdLanguageVersion.Xsd10)
                    {
                        RequiresXsd11(state, path, "xs:openContent");
                    }
                    else
                    {
                        RecordOpenContent(element, path, state);
                    }

                    break;

                case "defaultOpenContent":
                    if (options.Version == XsdLanguageVersion.Xsd10)
                    {
                        RequiresXsd11(state, path, "xs:defaultOpenContent");
                    }
                    else
                    {
                        RecordDefaultOpenContent(element, path, state);
                    }

                    break;

                case "override":
                case "defaultAttributes":
                case "defaultAttributesApply":
                    RecognizedButUnsupported(state, path, "xs:" + local, options.Version);
                    break;

                case "any":
                case "anyAttribute":
                    if (element.Attribute("notQName") is not null || element.Attribute("notNamespace") is not null)
                    {
                        // The xs:any child of xs:openContent / xs:defaultOpenContent is recorded (and its
                        // negative constraints validated) by ParseOpenContentWildcard — only content-model
                        // and attribute wildcards, which have no recorded component yet, stay explicit diagnostics.
                        var recordedWildcard = local == "any" &&
                            (element.Parent?.Name == XName.Get("openContent", XmlSchemaNamespace) ||
                             element.Parent?.Name == XName.Get("defaultOpenContent", XmlSchemaNamespace));
                        if (!recordedWildcard)
                        {
                            RecognizedButUnsupported(state, path, "xs:" + local + " notQName/notNamespace", options.Version);
                        }
                    }

                    break;

                case "element":
                    RecordElementDeclaration(element, path, state);
                    break;

                case "schema":
                    RecordDefaultAttributes(element, path, state, options.Version);
                    break;

                case "complexType":
                    RecordDefaultAttributesApply(element, path, state, options.Version);
                    break;

                case "import":
                case "include":
                    ResolveReference(element, baseUri, options, state, depth, path);
                    break;
            }
        }

        foreach (var child in element.Elements())
        {
            VisitElement(child, ChildPath(path, child), baseUri, options, state, depth, ownerType);
        }
    }

    private static SimpleTypeBuildContext? CreateSimpleTypeContext(XElement element, string path, CompilationState state)
    {
        var typeName = element.Attribute("name")?.Value;
        var restriction = element.Element(XName.Get("restriction", XmlSchemaNamespace));
        var baseName = restriction?.Attribute("base")?.Value;
        if (typeName is null || baseName is null)
        {
            return null; // anonymous or non-restriction simple type: nothing to record for facet evaluation
        }

        if (!TryResolveTypeName(element, typeName, out var typeNamespace, out var typeLocalName) ||
            !TryResolveTypeName(element, baseName, out var baseNamespace, out var baseLocalName))
        {
            return null; // unresolvable prefix
        }

        var context = new SimpleTypeBuildContext(
            typeNamespace, typeLocalName, baseNamespace, baseLocalName, path);
        state.SimpleTypes.Add(context);
        return context;
    }

    private static bool TryResolveTypeName(XElement element, string lexical, out string ns, out string local)
    {
        // Type names differ from element names: an unprefixed type QName takes the schema's
        // target namespace (XSD rule; built-in types arrive xs:-prefixed).
        if (TryResolveQName(element, lexical, out ns, out local, out var prefixed) && !prefixed)
        {
            var schema = element.AncestorsAndSelf().LastOrDefault(e => e.Name == XName.Get("schema", XmlSchemaNamespace));
            ns = schema?.Attribute("targetNamespace")?.Value ?? string.Empty;
        }

        return !prefixed || ns.Length > 0;
    }

    private static void RequiresXsd11(CompilationState state, string path, string construct)
    {
        state.Diagnostics.Add(new SchemaDiagnostic(
            SchemaDiagnosticCodes.ConstructRequiresXsd11,
            $"{construct} is an XSD 1.1-only construct and was rejected because the schema was compiled against XSD 1.0.",
            path));
    }

    private static void RecognizedButUnsupported(CompilationState state, string path, string construct, XsdLanguageVersion version)
    {
        if (version == XsdLanguageVersion.Xsd10)
        {
            RequiresXsd11(state, path, construct);
            return;
        }

        state.Diagnostics.Add(new SchemaDiagnostic(
            SchemaDiagnosticCodes.UnsupportedConstruct,
            $"{construct} is recognized as XSD 1.1 but is not yet supported by this REQ-009 phase (explicit diagnostic, not silent acceptance).",
            path));
    }

    private static void RecordOpenContent(XElement element, string path, CompilationState state)
    {
        // Composition per the XSD 1.1 schema-for-schemas: openContent is allowed only as a child of
        // xs:complexType, or of xs:restriction / xs:extension within xs:complexContent.
        var parent = element.Parent;
        var parentName = parent?.Name;
        var complexContent = XName.Get("complexContent", XmlSchemaNamespace);
        var parentIsComplexContentDerivation =
            (parentName?.LocalName == "restriction" || parentName?.LocalName == "extension") &&
            parent?.Parent?.Name == complexContent;
        if (parent is null ||
            parentName is null ||
            parentName.NamespaceName != XmlSchemaNamespace ||
            (parentName.LocalName != "complexType" && !parentIsComplexContentDerivation))
        {
            InvalidOpenContent(state, path, "xs:openContent is only allowed as a child of xs:complexType or of xs:restriction/xs:extension within xs:complexContent.");
            return;
        }

        var openContentName = XName.Get("openContent", XmlSchemaNamespace);
        var siblings = parent.Elements(openContentName).ToList();
        if (siblings.Count > 1 && !ReferenceEquals(siblings[0], element))
        {
            InvalidOpenContent(state, path, "At most one xs:openContent may appear on a complex type or derivation.");
            return;
        }

        var outOfOrder = parent.Elements().TakeWhile(e => !ReferenceEquals(e, element))
            .Any(e => e.Name.NamespaceName == XmlSchemaNamespace && e.Name.LocalName != "annotation");
        if (outOfOrder)
        {
            InvalidOpenContent(state, path, "xs:openContent must precede the content model, attributes, and assertions.");
            return;
        }

        var mode = ParseOpenContentMode(element, path, state, allowNone: true);
        if (mode is null)
        {
            return;
        }

        var anyElements = element.Elements(XName.Get("any", XmlSchemaNamespace)).ToList();
        if (mode == OpenContentMode.None && anyElements.Count > 0)
        {
            InvalidOpenContent(state, path, "xs:openContent with mode 'none' must not have an xs:any child.");
            return;
        }

        if (mode != OpenContentMode.None && anyElements.Count == 0)
        {
            InvalidOpenContent(state, path, $"xs:openContent with mode '{element.Attribute("mode")?.Value ?? "interleave"}' requires an xs:any child.");
            return;
        }

        if (anyElements.Count > 1)
        {
            InvalidOpenContent(state, path, "xs:openContent accepts at most one xs:any child.");
            return;
        }

        OpenContentWildcard? wildcard = null;
        if (anyElements.Count == 1)
        {
            wildcard = ParseOpenContentWildcard(anyElements[0], path, state);
            if (wildcard is null)
            {
                return;
            }
        }

        state.OpenContents.Add(new OpenContentDefinition(mode.Value, wildcard, AppliesToEmpty: false, path));
    }

    private static void RecordDefaultOpenContent(XElement element, string path, CompilationState state)
    {
        // Composition per the XSD 1.1 schema-for-schemas: defaultOpenContent is allowed only as a direct
        // child of xs:schema, at most one per schema document, before any component declarations.
        var parent = element.Parent;
        if (parent is null || parent.Name != XName.Get("schema", XmlSchemaNamespace))
        {
            InvalidOpenContent(state, path, "xs:defaultOpenContent is only allowed as a direct child of xs:schema.");
            return;
        }

        var defaultOpenContentName = XName.Get("defaultOpenContent", XmlSchemaNamespace);
        var siblings = parent.Elements(defaultOpenContentName).ToList();
        if (siblings.Count > 1 && !ReferenceEquals(siblings[0], element))
        {
            InvalidOpenContent(state, path, "At most one xs:defaultOpenContent may appear in a schema document.");
            return;
        }

        var outOfOrder = parent.Elements().TakeWhile(e => !ReferenceEquals(e, element))
            .Any(e => e.Name.NamespaceName == XmlSchemaNamespace &&
                      e.Name.LocalName is not ("annotation" or "include" or "import" or "redefine" or "override"));
        if (outOfOrder)
        {
            InvalidOpenContent(state, path, "xs:defaultOpenContent must precede all component declarations (only include-family elements and annotations may precede it).");
            return;
        }

        var mode = ParseOpenContentMode(element, path, state, allowNone: false);
        if (mode is null)
        {
            return;
        }

        var appliesToEmpty = false;
        var appliesAttribute = element.Attribute("appliesToEmpty");
        if (appliesAttribute is not null)
        {
            try
            {
                appliesToEmpty = XmlConvert.ToBoolean(appliesAttribute.Value);
            }
            catch (FormatException)
            {
                InvalidOpenContent(state, path, $"The appliesToEmpty value '{appliesAttribute.Value}' is not a valid boolean.");
                return;
            }
        }

        var anyElements = element.Elements(XName.Get("any", XmlSchemaNamespace)).ToList();
        if (anyElements.Count != 1)
        {
            InvalidOpenContent(state, path, anyElements.Count == 0
                ? "xs:defaultOpenContent requires an xs:any child."
                : "xs:defaultOpenContent accepts exactly one xs:any child.");
            return;
        }

        var wildcard = ParseOpenContentWildcard(anyElements[0], path, state);
        if (wildcard is null)
        {
            return;
        }

        state.DefaultOpenContents.Add(new OpenContentDefinition(mode.Value, wildcard, appliesToEmpty, path));
    }

    private static OpenContentMode? ParseOpenContentMode(XElement element, string path, CompilationState state, bool allowNone)
    {
        var lexical = element.Attribute("mode")?.Value ?? "interleave";
        switch (lexical)
        {
            case "interleave":
                return OpenContentMode.Interleave;
            case "suffix":
                return OpenContentMode.Suffix;
            case "none" when allowNone:
                return OpenContentMode.None;
            default:
                InvalidOpenContent(state, path, allowNone
                    ? $"The mode value '{lexical}' is invalid; allowed: none, interleave, suffix."
                    : $"The mode value '{lexical}' is invalid on xs:defaultOpenContent; allowed: interleave, suffix.");
                return null;
        }
    }

    private static OpenContentWildcard? ParseOpenContentWildcard(XElement any, string path, CompilationState state)
    {
        var namespaceAttribute = any.Attribute("namespace");
        var notNamespaceAttribute = any.Attribute("notNamespace");
        if (namespaceAttribute is not null && notNamespaceAttribute is not null)
        {
            // W3C XML Schema 1.1 Part 1 §3.10.3 (Wildcard Representation OK): namespace and
            // notNamespace attributes must not both be present.
            InvalidWildcard(state, path, "The xs:any attributes 'namespace' and 'notNamespace' must not both be present (W3C XML Schema 1.1 Part 1 §3.10.3).");
            return null;
        }

        // With only notNamespace present the positive constraint has no tokens (§3.10.2.2 mapping:
        // the {namespaces} set belongs to the negative 'not' variety recorded in NotNamespace).
        var namespaceTokens = (namespaceAttribute?.Value ?? (notNamespaceAttribute is null ? "##any" : string.Empty))
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var token in namespaceTokens)
        {
            var knownToken = token is "##any" or "##other" or "##targetNamespace" or "##local";
            if (!knownToken && !Uri.TryCreate(token, UriKind.Absolute, out _))
            {
                InvalidOpenContent(state, path, $"The xs:any namespace token '{token}' is not ##any, ##other, ##targetNamespace, ##local, or an absolute URI.");
                return null;
            }
        }

        var notNamespaceTokens = SplitTokens(notNamespaceAttribute?.Value);
        foreach (var token in notNamespaceTokens)
        {
            var knownToken = token is "##targetNamespace" or "##local";
            if (!knownToken && !Uri.TryCreate(token, UriKind.Absolute, out _))
            {
                InvalidWildcard(state, path, $"The xs:any notNamespace token '{token}' is not ##targetNamespace, ##local, or an absolute URI (W3C XML Schema 1.1 Part 1 §3.10.2).");
                return null;
            }
        }

        var notQNameTokens = SplitTokens(any.Attribute("notQName")?.Value);
        foreach (var token in notQNameTokens)
        {
            if (token is "##defined" or "##definedSibling")
            {
                continue;
            }

            if (!IsQNameLexical(token))
            {
                InvalidWildcard(state, path, $"The xs:any notQName token '{token}' is not ##defined, ##definedSibling, or a QName (W3C XML Schema 1.1 Part 1 §3.10.2).");
                return null;
            }
        }

        var lexical = any.Attribute("processContents")?.Value ?? "strict";
        var processContents = lexical switch
        {
            "strict" => WildcardProcessContents.Strict,
            "lax" => WildcardProcessContents.Lax,
            "skip" => WildcardProcessContents.Skip,
            _ => (WildcardProcessContents?)null,
        };
        if (processContents is null)
        {
            InvalidOpenContent(state, path, $"The xs:any processContents value '{lexical}' is invalid; allowed: strict, lax, skip.");
            return null;
        }

        return new OpenContentWildcard(namespaceTokens, processContents.Value, path)
        {
            NotNamespace = notNamespaceTokens,
            NotQName = notQNameTokens,
        };
    }

    private static IReadOnlyList<string> SplitTokens(string? lexical)
    {
        if (string.IsNullOrWhiteSpace(lexical))
        {
            return [];
        }

        return lexical.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private static bool IsQNameLexical(string token)
    {
        // §3.10.2.2: an unprefixed QName member of notQName denotes the no-namespace expanded name;
        // a prefixed one must have exactly one colon with non-empty NCName parts. Tokens beginning
        // with '##' are only the two keywords validated earlier, so e.g. '##bogus' fails NCName here.
        var colon = token.IndexOf(':');
        try
        {
            if (colon < 0)
            {
                XmlConvert.VerifyNCName(token);
                return true;
            }

            if (colon == 0 || colon != token.LastIndexOf(':') || colon == token.Length - 1)
            {
                return false;
            }

            XmlConvert.VerifyNCName(token[..colon]);
            XmlConvert.VerifyNCName(token[(colon + 1)..]);
            return true;
        }
        catch (XmlException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private static void InvalidOpenContent(CompilationState state, string path, string message)
    {
        state.Diagnostics.Add(new SchemaDiagnostic(
            SchemaDiagnosticCodes.InvalidOpenContent,
            message,
            path));
    }

    private static void InvalidWildcard(CompilationState state, string path, string message)
    {
        state.Diagnostics.Add(new SchemaDiagnostic(
            SchemaDiagnosticCodes.InvalidWildcard,
            message,
            path));
    }

    private static void RecordDefaultAttributes(XElement element, string path, CompilationState state, XsdLanguageVersion version)
    {
        // xs:schema @defaultAttributes (XSD 1.1, Part 1 §3.4.2.4): a QName reference to a global
        // attribute group applied by default to the complex types of this schema document.
        var attribute = element.Attribute("defaultAttributes");
        if (attribute is null)
        {
            return;
        }

        if (version == XsdLanguageVersion.Xsd10)
        {
            RequiresXsd11(state, path, "defaultAttributes");
            return;
        }

        if (!TryResolveQName(element, attribute.Value, out var groupNamespace, out var groupLocalName, out _))
        {
            return; // unresolvable prefix: the declaration cannot drive default-attribute resolution
        }

        state.DefaultAttributeReferences.Add(new DefaultAttributesReference(groupNamespace, groupLocalName, path));
    }

    private static void RecordDefaultAttributesApply(XElement element, string path, CompilationState state, XsdLanguageVersion version)
    {
        // xs:complexType @defaultAttributesApply (XSD 1.1, Part 1 §3.4.2.4): boolean, default true;
        // false opts the type out of the schema's default attribute group.
        var attribute = element.Attribute("defaultAttributesApply");
        if (attribute is null)
        {
            return;
        }

        if (version == XsdLanguageVersion.Xsd10)
        {
            RequiresXsd11(state, path, "defaultAttributesApply");
            return;
        }

        bool applies;
        try
        {
            applies = XmlConvert.ToBoolean(attribute.Value);
        }
        catch (FormatException)
        {
            state.Diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.InvalidDefaultAttributes,
                $"The defaultAttributesApply value '{attribute.Value}' is not a valid boolean.",
                path));
            return;
        }

        if (!applies)
        {
            state.DefaultAttributeOptOuts.Add(new DefaultAttributesOptOut(element.Attribute("name")?.Value, path));
        }
    }

    private static void ResolveReference(XElement element, Uri baseUri, SchemaCompileOptions options, CompilationState state, int depth, string path)
    {
        var location = element.Attribute("schemaLocation")?.Value;
        if (location is null)
        {
            return; // namespace-only import: nothing to fetch
        }

        if (state.Resolutions >= MaxResolutionCountPerCompile)
        {
            state.Diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.ResolutionLimit,
                $"Schema resolution aborted: the per-compile limit of {MaxResolutionCountPerCompile} resolved documents was exceeded.",
                path));
            return;
        }

        Uri referenceUri;
        try
        {
            referenceUri = new Uri(baseUri, location);
        }
        catch (UriFormatException)
        {
            state.Diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.ResolutionUnavailable,
                $"The schema location '{location}' could not be resolved against the base URI '{baseUri}'.",
                path));
            return;
        }

        if (!state.Visited.Add(referenceUri.AbsoluteUri))
        {
            return; // already processed (cycle or duplicate include)
        }

        if (depth + 1 > options.MaxImportDepth)
        {
            state.Diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.ResolutionLimit,
                $"Schema resolution aborted: import depth {options.MaxImportDepth} was exceeded at '{referenceUri}'.",
                path));
            return;
        }

        if (options.Resolver is null)
        {
            state.Diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.ResolutionUnavailable,
                $"The schema document '{referenceUri}' was referenced but no schema resource resolver was supplied.",
                path));
            return;
        }

        state.Resolutions++;
        Stream? stream = null;
        try
        {
            stream = options.Resolver.Open(referenceUri);
            var referenced = XDocument.Load(stream);
            VisitDocument(referenced.Root!, referenceUri, options, state, depth + 1);
        }
        catch (SchemaResolutionException ex)
        {
            state.Diagnostics.Add(new SchemaDiagnostic(ex.Code, ex.Message, path));
        }
        catch (XmlException ex)
        {
            state.Diagnostics.Add(new SchemaDiagnostic(
                SchemaDiagnosticCodes.ResolutionUnavailable,
                $"The resolved schema document '{referenceUri}' is not well-formed XML: {ex.Message}",
                path));
        }
        finally
        {
            stream?.Dispose();
        }
    }

    private static IReadOnlyDictionary<string, string>? CaptureNamespaceBindings(XElement element)
    {
        Dictionary<string, string>? bindings = null;
        foreach (var scope in element.AncestorsAndSelf())
        {
            foreach (var attribute in scope.Attributes())
            {
                if (!attribute.IsNamespaceDeclaration || attribute.Name.LocalName == "xmlns")
                {
                    continue; // the default namespace declaration is not a prefix binding
                }

                bindings ??= new Dictionary<string, string>(StringComparer.Ordinal);
                if (!bindings.ContainsKey(attribute.Name.LocalName))
                {
                    bindings.Add(attribute.Name.LocalName, attribute.Value);
                }
            }
        }

        return bindings;
    }

    private static string? ResolveXPathDefaultNamespace(XElement element)
    {
        var schema = element.AncestorsAndSelf().FirstOrDefault(e => e.Name == XName.Get("schema", XmlSchemaNamespace));
        foreach (var scope in element.AncestorsAndSelf())
        {
            var attribute = scope.Attribute("xpathDefaultNamespace");
            if (attribute is null)
            {
                continue;
            }

            return attribute.Value switch
            {
                "##targetNamespace" => schema?.Attribute("targetNamespace")?.Value,
                "##local" => null,
                "##defaultNamespace" => InScopeDefaultNamespace(element),
                _ => attribute.Value,
            };
        }

        return null;
    }

    private static string? InScopeDefaultNamespace(XElement element)
    {
        foreach (var scope in element.AncestorsAndSelf())
        {
            var attribute = scope.Attributes().FirstOrDefault(a => a.IsNamespaceDeclaration && a.Name.LocalName == "xmlns");
            if (attribute is not null)
            {
                return attribute.Value;
            }
        }

        return null;
    }

    private static void RecordElementDeclaration(XElement element, string path, CompilationState state)
    {
        var name = element.Attribute("name")?.Value;
        var type = element.Attribute("type")?.Value;
        if (name is null || type is null)
        {
            return; // reference (ref="...") or typeless declaration: nothing to record
        }

        if (!TryResolveElementDeclarationName(element, name, out var elementNamespace, out var elementLocalName, out _) ||
            !TryResolveQName(element, type, out var typeNamespace, out var typeLocalName, out _))
        {
            return; // unresolvable prefix: the declaration cannot drive typed-view building
        }

        state.ElementDeclarations.Add(new ElementDeclaration(
            elementNamespace,
            elementLocalName,
            typeNamespace,
            typeLocalName,
            path));
    }

    private static bool TryResolveElementDeclarationName(XElement element, string lexical, out string ns, out string local, out bool prefixed)
    {
        if (!TryResolveQName(element, lexical, out ns, out local, out prefixed))
        {
            return false;
        }

        if (!prefixed)
        {
            // Unprefixed declaration names take the schema's target namespace for global declarations
            // and, for locals, only when elementFormDefault="qualified" (default is unqualified).
            var schemaName = XName.Get("schema", XmlSchemaNamespace);
            var schema = element.AncestorsAndSelf().LastOrDefault(e => e.Name == schemaName);
            var isGlobal = element.Parent?.Name == schemaName;
            if (isGlobal || schema?.Attribute("elementFormDefault")?.Value == "qualified")
            {
                ns = schema?.Attribute("targetNamespace")?.Value ?? string.Empty;
            }
        }

        return true;
    }

    private static XName? FindOwningComplexTypeName(XElement element)
    {
        // REQ-009 S4b applicability: an xs:assert is owned by its nearest enclosing named xs:complexType;
        // anonymous inline complex types leave the owner type null (the owning element identifies them).
        foreach (var ancestor in element.Ancestors())
        {
            if (ancestor.Name.NamespaceName != XmlSchemaNamespace)
            {
                continue;
            }

            if (ancestor.Name.LocalName is not ("complexType" or "simpleType"))
            {
                continue;
            }

            var name = ancestor.Attribute("name")?.Value;
            if (name is not null &&
                TryResolveTypeName(ancestor, name, out var ns, out var local))
            {
                return XName.Get(local, ns);
            }

            return null; // anonymous owner type
        }

        return null;
    }

    private static XName? FindOwningElementName(XElement element)
    {
        // REQ-009 S4b applicability: the nearest enclosing named xs:element declaration identifies
        // assertions on anonymous inline complex types.
        foreach (var ancestor in element.Ancestors())
        {
            if (ancestor.Name != XName.Get("element", XmlSchemaNamespace))
            {
                continue;
            }

            var name = ancestor.Attribute("name")?.Value;
            if (name is not null &&
                TryResolveElementDeclarationName(ancestor, name, out var ns, out var local, out _))
            {
                return XName.Get(local, ns);
            }
        }

        return null;
    }

    private static void RecordAlternative(XElement element, string path, CompilationState state)
    {
        // xs:alternative (XSD 1.1 conditional type assignment, Part 1 §3.4.2.4): records the alternative
        // together with its owner element declaration, the captured static context of its test expression,
        // and the resolved type name. CTA applicability evaluation itself is REQ-009 S4b.
        var test = element.Attribute("test")?.Value;
        var typeName = element.Attribute("type")?.Value;

        XName? resolvedTypeName = null;
        if (typeName is not null)
        {
            if (TryResolveTypeName(element, typeName, out var typeNs, out var typeLocal))
            {
                resolvedTypeName = XName.Get(typeLocal, typeNs);
            }
            else
            {
                state.Diagnostics.Add(new SchemaDiagnostic(
                    SchemaDiagnosticCodes.AlternativeTypeUnresolved,
                    $"The xs:alternative type '{typeName}' could not be resolved: the QName prefix is not declared in the schema document.",
                    path));
            }
        }

        string? ownerName = null;
        XName? ownerResolvedName = null;
        var parent = element.Parent;
        var parentElementName = parent?.Attribute("name")?.Value;
        if (parent is not null &&
            parent.Name == XName.Get("element", XmlSchemaNamespace) &&
            parentElementName is not null &&
            TryResolveElementDeclarationName(parent, parentElementName, out var ownerNs, out var ownerLocal, out _))
        {
            ownerName = parentElementName;
            ownerResolvedName = XName.Get(ownerLocal, ownerNs);
        }

        if (test is null)
        {
            var siblings = parent?.Elements(XName.Get("alternative", XmlSchemaNamespace)).ToList() ?? [];
            var earlierDefaults = siblings.TakeWhile(e => !ReferenceEquals(e, element)).Count(e => e.Attribute("test") is null);
            if (earlierDefaults > 0)
            {
                state.Diagnostics.Add(new SchemaDiagnostic(
                    SchemaDiagnosticCodes.InvalidAlternativeSet,
                    "The element declaration has more than one default xs:alternative (an alternative without a test attribute); at most one is allowed.",
                    path));
            }

            if (siblings.SkipWhile(e => !ReferenceEquals(e, element)).Skip(1).Any())
            {
                state.Diagnostics.Add(new SchemaDiagnostic(
                    SchemaDiagnosticCodes.InvalidAlternativeSet,
                    "The default xs:alternative (without a test attribute) must be the final alternative of its element declaration.",
                    path));
            }
        }

        state.Alternatives.Add(new TypeAlternativeDefinition(
            test,
            typeName,
            resolvedTypeName,
            ownerName,
            ownerResolvedName,
            CaptureNamespaceBindings(element),
            ResolveXPathDefaultNamespace(element),
            path));
    }

    private static bool TryResolveQName(XElement element, string lexical, out string ns, out string local, out bool prefixed)
    {
        ns = string.Empty;
        var colon = lexical.IndexOf(':');
        if (colon < 0)
        {
            // Unprefixed QName in a schema type/name attribute has no namespace (XSD rule).
            local = lexical;
            prefixed = false;
            return true;
        }

        prefixed = true;
        var prefix = lexical[..colon];
        local = lexical[(colon + 1)..];
        foreach (var scope in element.AncestorsAndSelf())
        {
            var attribute = scope.Attributes().FirstOrDefault(a => a.IsNamespaceDeclaration && a.Name.LocalName == prefix);
            if (attribute is not null)
            {
                ns = attribute.Value;
                return true;
            }
        }

        return false;
    }

    private static string ChildPath(string parentPath, XElement element)
    {
        var name = element.Name.LocalName;
        var parent = element.Parent;
        if (parent is null)
        {
            return name;
        }

        var sameName = parent.Elements(element.Name).ToList();
        if (sameName.Count > 1)
        {
            var index = sameName.FindIndex(e => ReferenceEquals(e, element)) + 1;
            return FormattableString.Invariant($"{parentPath}/{name}[{index}]");
        }

        return parentPath + "/" + name;
    }

    private sealed class CompilationState
    {
        internal CompilationState(string digest)
        {
            Digest = digest;
        }

        internal string Digest { get; }

        internal List<AssertionDefinition> Assertions { get; } = [];

        internal List<TypeAlternativeDefinition> Alternatives { get; } = [];

        internal List<ElementDeclaration> ElementDeclarations { get; } = [];

        internal List<SimpleTypeBuildContext> SimpleTypes { get; } = [];

        internal List<OpenContentDefinition> OpenContents { get; } = [];

        internal List<OpenContentDefinition> DefaultOpenContents { get; } = [];

        internal List<DefaultAttributesReference> DefaultAttributeReferences { get; } = [];

        internal List<DefaultAttributesOptOut> DefaultAttributeOptOuts { get; } = [];

        internal List<SchemaDiagnostic> Diagnostics { get; } = [];

        internal HashSet<string> Visited { get; } = new(StringComparer.OrdinalIgnoreCase);

        internal int Resolutions { get; set; }
    }

    internal sealed class SimpleTypeBuildContext
    {
        internal SimpleTypeBuildContext(string typeNamespace, string typeLocalName, string? baseTypeNamespace, string? baseTypeLocalName, string path)
        {
            TypeNamespace = typeNamespace;
            TypeLocalName = typeLocalName;
            BaseTypeNamespace = baseTypeNamespace;
            BaseTypeLocalName = baseTypeLocalName;
            Path = path;
        }

        internal string TypeNamespace { get; }

        internal string TypeLocalName { get; }

        internal string? BaseTypeNamespace { get; }

        internal string? BaseTypeLocalName { get; }

        internal string Path { get; }

        internal List<AssertionDefinition> Assertions { get; } = [];
    }
}
