// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 15 November 2026
// PURPOSE              : Projects guarded native nodes without exposing mutable XML storage.
// SPECIAL NOTES        : Internal Core 1.81.2 adapter; finite profile only, no public native factory.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 15-11-2026     | Implement internal native decimal Core adapter                                                 |
//                      | Charles Korthout | 0.2   | 19-11-2026     | Retain expanded root declaration identity in guarded node annotations                      |
//                      | Charles Korthout | 0.3   | 19-11-2026     | Adopt bounded named complex types and shared expanded component identity                  |
//                      | Charles Korthout | 0.4   | 19-11-2026     | Expose original local attribute declaration metadata under the existing owner guard      |
//                      | Charles Korthout | 0.5   | 20-11-2026     | Decouple complex annotation from typed-value absence and retain normalized root text     |
//                      | Charles Korthout | 0.6   | 20-11-2026     | Keep local complex declarations distinct from global element metadata                     |
//                      | Charles Korthout | 0.7   | 20-11-2026     | Expose guarded original-node data interface; exact published facts and lifetime guard  |
//                      | Charles Korthout | 0.8   | 21-11-2026     | Probe empty complex typing in isolated copies only |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using Bosak.XPath.Core.Xdm;
using Bosak.XPath.Core.Schema;
using Bosak.XPath.Providers.Xml;
using Bosak.XPath.Runtime.Vm;

namespace Bosak.Schema;

/// <summary>Projects guarded native nodes without exposing mutable XML storage.</summary>
internal sealed class NativeSessionNode(NativeSchemaInput input, XDocumentNode raw) : ISchemaAnnotatedNode
{
    internal NativeSchemaInput Input => input;
    private T Read<T>(Func<T> read) { input.Check(); return read(); }
    private NativeSchemaProvider.Facts? Facts => input.FactsFor(this);
    public XdmNodeKind NodeKind => Read(() => raw.NodeKind);
    /// <inheritdoc/>
    public ISchemaNodeFacts OriginalSchemaFacts => Read<ISchemaNodeFacts>(() => Facts ?? throw new NotSupportedException("No original native facts."));
    public string LocalName => Read(() => raw.LocalName);
    public string EncodedLocalName => Read(() => raw.EncodedLocalName);
    public string NamespaceUri => Read(() => raw.NamespaceUri);
    public string Prefix => Read(() => raw.Prefix);
    public string EncodedPrefix => Read(() => raw.EncodedPrefix);
    public string StringValue => Read(() => Facts?.NormalizedStringValue ?? raw.StringValue);
    public XdmValue TypedValue
    {
        get
        {
            input.Check();
            var facts = Facts;
            if (facts is null) return raw.TypedValue;
            if (facts.HasNoTypedValue)
                throw new XPathErrorException("http://www.w3.org/2005/xqt-errors", "FOTY0012", "err", "Element-only root has no typed value.");
            var value = facts.ReadTypedValue(default).Value;
            if (!value.TryGetSingleAtomic(out var item))
            {
                if (facts.NativeType.EmptyContent) return XdmValue.Undefined;
                throw new NotSupportedException("Non-scalar typed value outside the finite profile.");
            }
            return item.Value;
        }
    }
    public bool HasNoTypedValue => Read(() => Facts?.HasNoTypedValue ?? raw.HasNoTypedValue);
    public bool IsComplexType => Read(() => Facts?.NativeType.IsComplex ?? raw.IsComplexType);
    public bool IsId => Read(() => false);
    public bool IsIdref => Read(() => false);
    public (string NamespaceUri, string LocalName)? SchemaTypeAnnotation =>
        Read<(string, string)?>(() => Facts?.NativeType is { Name: { } name } type ? (type.NamespaceUri, name) : null);
    public (string NamespaceUri, string LocalName)? SchemaElementDeclaration =>
        Read<(string, string)?>(() => Facts?.NativeType.IsComplex == true && ReferenceEquals(this, input.Root) ? (raw.NamespaceUri, raw.LocalName) : null);
    public (string NamespaceUri, string LocalName)? SchemaAttributeDeclaration =>
        Read<(string, string)?>(() => Facts?.NativeDeclaration is { IsAttribute: true } d ? (d.NamespaceUri, d.Name) : null);
    public bool IsNilled => Read(() => false);
    public bool IsConstructedElement => Read(() => false);
    public IXdmNode? Parent => Read(() => raw.Parent is XDocumentNode parent ? input.Wrap(parent) : null);
    public IXdmNode? Document => Read(() => raw.Document is XDocumentNode document ? input.Wrap(document) : null);
    public long DocumentOrder => Read(() => raw.DocumentOrder);
    public string BaseUri => Read(() => raw.BaseUri);
    public string DocumentUri => Read(() => raw.DocumentUri);
    public bool HasDocumentType => Read(() => false);
    public string DocumentTypeName => Read(() => raw.DocumentTypeName);
    public string PublicId => Read(() => raw.PublicId);
    public string SystemId => Read(() => raw.SystemId);
    public string InternalSubset => Read(() => raw.InternalSubset);
    public bool TryGetUnparsedEntity(string name, out string systemId, out string publicId)
    {
        input.Check();
        systemId = ""; publicId = ""; return false;
    }
    public string ToXmlString() => Read(raw.ToXmlString);
    public bool IsSameNode(IXdmNode other) => Read(() => ReferenceEquals(this, other));
    public XdmSequence Children(XdmNodeKind kind) => Read(() => Project(raw.Children(kind)));
    public XdmSequence Attributes(string? localName = null, string? namespaceUri = null) => Read(() => Project(raw.Attributes(localName, namespaceUri)));
    public XdmSequence Axis(XdmAxis axis) => Read(() => Project(raw.Axis(axis)));
    private XdmSequence Project(XdmSequence sequence) => XdmSequence.FromSource(new GuardedSequence(input, sequence));

    private sealed class GuardedSequence(NativeSchemaInput input, XdmSequence source) : IXdmSequence
    {
        public bool TryGetLength(out int length) { input.Check(); return source.TryGetLength(out length); }
        public IXdmSequenceEnumerator GetEnumerator() { input.Check(); return new Cursor(input, source.GetEnumerator()); }
    }
    private sealed class Cursor(NativeSchemaInput input, XdmSequence.Enumerator enumerator) : IXdmSequenceEnumerator
    {
        private XdmSequence.Enumerator enumerator = enumerator;
        public bool MoveNext() { input.Check(); return enumerator.MoveNext(); }
        public XdmValue Current
        {
            get
            {
                input.Check();
                var value = enumerator.Current;
                return value.IsNode && value.NodeValue is XDocumentNode node ? XdmValue.FromNode(input.Wrap(node)) : value;
            }
        }
    }
}
