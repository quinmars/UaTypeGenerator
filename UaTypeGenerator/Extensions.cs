using System;
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
                return bname.Name;
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

    }
}
