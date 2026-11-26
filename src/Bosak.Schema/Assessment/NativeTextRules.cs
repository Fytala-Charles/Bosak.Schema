// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 26 November 2026
// PURPOSE              : Validates native text lexical families and whitespace normalization.
// SPECIAL NOTES        : Part of bounded native datatype validation.
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
using System.Xml.Linq;
namespace Bosak.Schema;

internal enum NativeTextKind { String, NormalizedString, Token, Language, Name, NCName, NMTOKEN }
internal static class NativeTextRules
{
    internal static string Normalize(string text, int mode) => mode == 0 ? text : mode == 1 ? text.Replace('\t',' ').Replace('\r',' ').Replace('\n',' ') : NativeCalendarParser.Collapse(text);
    internal static bool Accepts(string text, NativeTextKind kind) => kind switch
    {
        NativeTextKind.Language => Language(text),
        NativeTextKind.Name => NativeInterchangeParser.IsXmlName(text, true),
        NativeTextKind.NCName => NativeInterchangeParser.IsXmlName(text),
        NativeTextKind.NMTOKEN => NativeInterchangeParser.IsXmlName(text, true, true),
        _ => true
    };
    private static bool Language(string text)
    {
        var parts=text.Split('-');
        return parts.All(p=>p.Length is >=1 and <=8) && parts[0].All(char.IsAsciiLetter) && parts.Skip(1).All(p=>p.All(char.IsAsciiLetterOrDigit));
    }
}
internal sealed partial class NativeDecimalSchema
{
    private static void RegisterTextBuiltins(Dictionary<XName,NativeType> simple,NativeType primitive)
    {
        NativeType Add(string name,NativeTextKind kind,NativeType parent)
        {
            var type=new NativeType(name,false,Xs.NamespaceName,isString:true,textKind:kind,derivationBase:parent);
            simple.Add(Xs+name,type);return type;
        }
        var normalized=Add("normalizedString",NativeTextKind.NormalizedString,primitive);
        var token=Add("token",NativeTextKind.Token,normalized);
        Add("language",NativeTextKind.Language,token);
        var name=Add("Name",NativeTextKind.Name,token);
        Add("NCName",NativeTextKind.NCName,name);
        var nmtoken=Add("NMTOKEN",NativeTextKind.NMTOKEN,token);
        simple.Add(Xs+"NMTOKENS",new NativeType("NMTOKENS",false,Xs.NamespaceName,listItemType:nmtoken,
            compoundFacets:new NativeCompoundFacets(null,null,1,null,[],[])));
    }
}
