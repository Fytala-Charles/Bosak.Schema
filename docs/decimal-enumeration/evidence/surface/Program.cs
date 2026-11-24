// ===========================================================================================================================================================
// AUTHOR               : Charles Korthout
// CREATE DATE          : 19 November 2026
// PURPOSE              : Compares the delivered scalar surface against its proposal and prior public APIs.
// SPECIAL NOTES        : Bounded native decimal profile; published Core 1.81.2; no general XSD conformance claim.
//
// COPYRIGHT            : Fytala
// LICENSE              : LICENSE.md (Apache-2.0)
// SPDX-License-Identifier: Apache-2.0
// ===========================================================================================================================================================
// Change History:      |==================|=======|================|=========================================================================================
//                      |     Author       |Version|  Date          | Notes                                                                                    |
//                      |==================|=======|================|=========================================================================================
//                      | Charles Korthout | 0.1   | 19-11-2026     | Creation                                                                                 |
//                      |==================|=======|================|=========================================================================================
// ===========================================================================================================================================================

using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;
using Bosak.Schema;

internal static class Program
{
    private const BindingFlags Public=BindingFlags.Public|BindingFlags.Instance|BindingFlags.Static|BindingFlags.DeclaredOnly;
    private static string TypeName(Type t)=>t.IsByRef?TypeName(t.GetElementType()!)+"&":t.IsGenericParameter?t.Name:t.IsGenericType?t.GetGenericTypeDefinition().FullName+"<"+string.Join(",",t.GetGenericArguments().Select(TypeName))+">":t.FullName??t.Name;
    private static string Parameters(MethodBase m)=>string.Join(",",m.GetParameters().Select(p=>TypeName(p.ParameterType)+" "+p.Name+(p.HasDefaultValue?"="+(p.DefaultValue?.ToString()??"null"):"")));
    private static HashSet<string> Rows(Assembly a)
    {
        var rows=new HashSet<string>(StringComparer.Ordinal);
        foreach(var t in a.GetExportedTypes())
        {
            var name=TypeName(t);rows.Add("T|"+name+"|"+t.Attributes+"|"+(t.BaseType is {} b?TypeName(b):""));
            foreach(var c in t.GetConstructors(Public))rows.Add("C|"+name+"|"+Parameters(c));
            foreach(var m in t.GetMethods(Public).Where(m=>!m.IsSpecialName))rows.Add("M|"+name+"|"+m.Name+"|"+TypeName(m.ReturnType)+"|"+Parameters(m));
            foreach(var p in t.GetProperties(Public))rows.Add("P|"+name+"|"+p.Name+"|"+TypeName(p.PropertyType)+"|"+(p.GetMethod?.IsPublic??false)+"|"+(p.SetMethod?.IsPublic??false));
            foreach(var f in t.GetFields(Public).Where(f=>!f.IsSpecialName))rows.Add("F|"+name+"|"+f.Name+"|"+TypeName(f.FieldType)+"|"+(f.IsLiteral?f.GetRawConstantValue()?.ToString():""));
        }
        return rows;
    }
    private static int Main(string[] args)
    {
        try
        {
            var context=new AssemblyLoadContext("baseline-surface",true);var old=context.LoadFromAssemblyPath(Path.GetFullPath(args[0]));var current=typeof(NativeSchemaSession).Assembly;
            var before=Rows(old);var after=Rows(current);var missing=before.Except(after).ToArray();if(missing.Length!=0)throw new InvalidOperationException("Existing API changed: "+missing[0]);
            var newTypes=current.GetExportedTypes().Select(t=>t.FullName!).Except(old.GetExportedTypes().Select(t=>t.FullName!)).Order().ToArray();
            if(newTypes.Length!=0)throw new InvalidOperationException("Unexpected new public type.");
            string[] expectedAdded=[];
            if(!expectedAdded.Order().SequenceEqual(after.Except(before).Order()))throw new InvalidOperationException("Unexpected public surface delta.");
            var result=new {oldPublicTypes=old.GetExportedTypes().Length,currentPublicTypes=current.GetExportedTypes().Length,preservedRows=before.Count,removedOrChangedRows=missing,newPublicTypes=newTypes,addedRows=after.Except(before).Order().ToArray(),zeroPublicApiDelta=true};
            File.WriteAllText(args[2],JsonSerializer.Serialize(result,new JsonSerializerOptions{WriteIndented=true}));return 0;
        }
        catch(Exception ex){Console.Error.WriteLine("Caught API surface diagnostic: "+ex.GetType().Name+": "+ex.Message);return 1;}
    }
}
