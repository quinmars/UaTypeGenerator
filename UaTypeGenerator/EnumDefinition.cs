using System;
using System.Collections.Generic;
using System.Text;
using Workstation.ServiceModel.Ua;

namespace UaTypeGenerator
{
    public class EnumDefinition : BaseDefinition
    {
        public class Item : BaseDefinition
        {
            public int? Value { get; set; }
        }

        public NodeId DataTypeId { get; set; }
        public Item[] Items { get; set; }
    }
}
