using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workstation.ServiceModel.Ua;

namespace UaTypeGenerator
{
    public static class Extensions
    {
        public static string GetTypeName(this UADataType dt)
        {
            if (dt.SymbolicName != null)
            {
                return dt.SymbolicName;
            }
            else if (dt.BrowseName != null && QualifiedName.TryParse(dt.BrowseName, out var bname))
            {
                return bname.Name.ToNetIdentifier();
            }
            else
            {
                return null;
            }
        }

        public static string GetNamespace(this UADataType dt, string[] namespaceUris)
        {
            if (dt.BrowseName != null
                && QualifiedName.TryParse(dt.BrowseName, out var bname)
                && bname.NamespaceIndex < namespaceUris.Length)
            {
                return namespaceUris[bname.NamespaceIndex];
            }

            return null;
        }

        public static string ToNetIdentifier(this string browseName)
        {
            var name = browseName
                .Replace("\"", "")
                .Replace("-", "_")
                .Replace(" ", "_")
                .Replace(".", "._");

            if (char.IsDigit(name[0]))
            {
                name = "_" + name;
            }

            return name;
        }

        public static void Begin(this IndentedTextWriter writer, string opening)
        {
            writer.WriteLine(opening);
            writer.Indent++;
        }
        
        public static void End(this IndentedTextWriter writer, string closing)
        {
            writer.Indent--;
            writer.WriteLine(closing);
        }

        public static string[] WordWrap(this string text, int width)
        {
            text = text.Replace('\n', ' ').Replace('\r', ' ').Replace("  ", " ");
            var list = new List<string>();

            int index;
            while (true)
            {
                if (text.Length <= width)
                {
                    list.Add(text.Trim());
                    break;
                }

                index = text.LastIndexOf(' ', width);

                if (index >= 0)
                {
                    list.Add(text.Substring(0, index).Trim());
                    text = text.Substring(index);
                }
                else
                {
                    list.Add(text.Trim());
                    break;
                }
            }

            return list.ToArray();
        }
    }
}
