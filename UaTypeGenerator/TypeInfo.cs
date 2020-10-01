using System;
using System.Collections.Generic;
using System.Text;

namespace UaTypeGenerator
{
    public class TypeInfo
    {
        public string TypeName { get; set; }
        public bool IsReference { get; set; }
        public bool IsEnum { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsClass { get; set; }
    }
}
