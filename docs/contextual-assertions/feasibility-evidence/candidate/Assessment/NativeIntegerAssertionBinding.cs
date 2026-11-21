// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 21 November 2026
// PURPOSE              : Binds bounded integer assertions to native simple-content assessment.
// SPECIAL NOTES        : Isolated prototype; no production semantic adoption or public authority issuance.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 21-11-2026     | Creation                                                                                 |
//                      | Charles Korthout | 0.2   | 21-11-2026     | Refine isolated contextual assertion prototype; no production adoption |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================


using System.Collections.Immutable;
using System.Xml.Linq;

namespace Bosak.Schema;

internal sealed partial class NativeDecimalSchema
{
    private ImmutableArray<NativeIntegerAssertion> integerAssertions = [];
    internal ImmutableArray<NativeIntegerAssertion> IntegerAssertions => integerAssertions;
    private static ImmutableArray<NativeIntegerAssertion> CompileIntegerAssertions(
        IEnumerable<XElement> elements, string targetNamespace, string defaultNamespace,
        NativeNumericLimits limits, CancellationToken token)
    {
        var result = ImmutableArray.CreateBuilder<NativeIntegerAssertion>();
        foreach (var element in elements)
        {
            token.ThrowIfCancellationRequested();
            Require(element.Name == Xs + "assert", "Expected bounded integer assertion.", NativeDecimalUnsupportedReason.AssertionGrammar);
            Attributes(element, "test", "xpathDefaultNamespace");
            Require(Elements(element).Count == 0, "Assertion children are excluded.", NativeDecimalUnsupportedReason.AssertionGrammar);
            var expression = (string?)element.Attribute("test");
            Require(expression is not null, "Assertion expression required.", NativeDecimalUnsupportedReason.AssertionGrammar);
            var namespaces = ImmutableDictionary.CreateBuilder<string,string>(StringComparer.Ordinal);
            namespaces["xml"] = XNamespace.Xml.NamespaceName;
            foreach (var scope in element.AncestorsAndSelf().Reverse())
                foreach (var attribute in scope.Attributes().Where(a => a.IsNamespaceDeclaration))
                    namespaces[attribute.Name.LocalName == "xmlns" ? "" : attribute.Name.LocalName] = attribute.Value;
            result.Add(NativeIntegerAssertion.Compile(expression!, limits, namespaces.ToImmutable(),
                AssertionNamespace(element, targetNamespace, defaultNamespace), element.BaseUri));
            Require(result.Count <= 8, "Effective integer assertion capacity exceeded.", NativeDecimalUnsupportedReason.RepresentationCapacity);
        }
        return result.ToImmutable();
    }
    private void AssessIntegerAssertions(XElement element, NativeElementState state, CancellationToken token)
    {
        if (integerAssertions.IsEmpty) return;
        var context = new NativeIntegerAssertion.Context(element.Value, state.IntegerValue!.Value);
        foreach (var assertion in integerAssertions)
        {
            token.ThrowIfCancellationRequested();
            Check(assertion.Evaluate(context), "Integer assertion evaluated false or raised a dynamic/type error.");
        }
    }
}
