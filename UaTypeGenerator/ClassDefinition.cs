using System;
using System.Collections.Generic;
using System.Text;
using Workstation.ServiceModel.Ua;

namespace UaTypeGenerator
{
    public class ClassDefinition : BaseDefinition
    {
        public class Property : BaseDefinition
        {
            public NodeId DataTypeId { get; set; }
            public bool IsOptional { get; set; }
            public int Rank { get; set; }
        }

        public string Namespace { get; set; }
        public NodeId DataTypeId { get; set; }
        public NodeId BinaryEncodingId { get; set; }
        public NodeId XmlEncodingId { get; set; }

        public bool IsAbstract { get; set; }
        public bool IsUnion { get; set; }

        public NodeId ParentDataTypeId { get; set; }

        public Property[] Properties { get; set; }
        public int OptionalPropertyCount { get; set; }
    }
}
