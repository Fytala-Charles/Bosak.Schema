// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Checks bounded validation-root ID and reference bindings before fact publication.
// SPECIAL NOTES        : Internal assessment bookkeeping; no public ID table or mutable XML exposure.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 26-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================
using System.Collections.Immutable;
using System.Xml.Linq;
namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    internal static bool IsIdentityAtomic(NativeType type) => type.IsString && type.TextKind is NativeTextKind.ID or NativeTextKind.IDREF;

    private static bool ContainsIdentityType(NativeType type)
    {
        var seen=new HashSet<NativeType>(ReferenceEqualityComparer.Instance);
        bool Visit(NativeType t) => seen.Add(t) && (IsIdentityAtomic(t) || t.ContentType is { } content && Visit(content) || t.ListItemType is { } item && Visit(item) || t.IsUnion && t.UnionMembers.Any(Visit));
        return Visit(type);
    }

    internal NativeResult<NativeAssessedDocument> Assess(string instance, CancellationToken cancellationToken = default)
    {
        var result=AssessCore(instance,cancellationToken);
        if(result.Outcome!=NativeOutcome.Valid || result.Value is null)return result;
        try
        {
            result.Value.CheckIdentity(cancellationToken);
            return result;
        }
        catch(NativeProfileException error)
        {
            return NativeResult<NativeAssessedDocument>.Failure(error.Outcome,error.Message,error.UnsupportedReason);
        }
    }

    internal static void CheckIdentity(XDocument document, ImmutableArray<NativeElementState> elements,
        ImmutableArray<NativeElementState> attributes, CancellationToken token)
    {
        var bindings=new Dictionary<string,XElement?>(StringComparer.Ordinal);
        var occurrences=0;
        void Atomic(NativeElementState state,XElement? owner)
        {
            token.ThrowIfCancellationRequested();
            if(state.IsNilled)return;
            if(state.IsListValue)
            {
                foreach(var item in state.Items)Atomic(item,owner);
                return;
            }
            var type=state.ActualAtomicType ?? state.GoverningType.ContentType ?? state.GoverningType;
            if(!IsIdentityAtomic(type))return;
            Require(++occurrences<=256,"Identity occurrence capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
            var value=state.StringValue!;
            if(!bindings.ContainsKey(value))
            {
                Require(bindings.Count<128,"Distinct identity string capacity exceeded.",NativeDecimalUnsupportedReason.RepresentationCapacity);
                bindings.Add(value,null);
            }
            if(type.TextKind!=NativeTextKind.ID || owner is null)return;
            var existing=bindings[value];
            Check(existing is null || ReferenceEquals(existing,owner),"ID has multiple element bindings.");
            bindings[value]=owner;
        }
        // State order is the same original physical element order used by the fact issuer.
        var nodes=document.Root!.DescendantsAndSelf().ToArray();
        Require(nodes.Length==elements.Length,"Identity state/capture alignment unavailable.",NativeDecimalUnsupportedReason.DeclarationShape);
        for(var i=0;i<elements.Length;i++)Atomic(elements[i],nodes[i].Parent);
        foreach(var attribute in attributes)
        {
            token.ThrowIfCancellationRequested();
            var owner=document.Root!;
            foreach(var segment in attribute.OwnerElementPath)owner=owner.Elements().ElementAt(segment);
            Atomic(attribute,owner);
        }
        token.ThrowIfCancellationRequested();
        Check(bindings.Values.All(owner=>owner is not null),"ID or IDREF has no element binding in the validation root.");
    }
}
