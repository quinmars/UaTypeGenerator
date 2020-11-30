using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Workstation.ServiceModel.Ua;

namespace UaTypeGenerator
{
    public class TypeSet
    {
        public const uint BooleanNumber = 1;
        public const uint SByteNumber = 2;
        public const uint ByteNumber = 3;
        public const uint Int16Number = 4;
        public const uint UInt16Number = 5;
        public const uint Int32Number = 6;
        public const uint UInt32Number = 7;
        public const uint Int64Number = 8;
        public const uint UInt64Number = 9;
        public const uint FloatNumber = 10;
        public const uint DoubleNumber = 11;
        public const uint StringNumber = 12;
        public const uint DateTimeNumber = 13;
        public const uint GuidNumber = 14;
        public const uint ByteStringNumber = 15;
        public const uint XmlElementNumber = 16;
        public const uint NodeIdNumber = 17;
        public const uint ExpandedNodeIdNumber = 18;
        public const uint StatusCodeNumber = 19;
        public const uint QualifiedNameNumber = 20;
        public const uint LocalizedTextNumber = 21;
        public const uint StructureNumber = 22;
        public const uint DataValueNumber = 23;
        public const uint BaseDataTypeNumber = 24;
        public const uint DiagnosticInfoNumber = 25;

        private readonly string[] _namespaceUris;

        private readonly IDictionary<string, NodeId> _alias;
        private readonly IDictionary<NodeId, NodeId> _binaryEncodings;
        private readonly IDictionary<NodeId, NodeId> _xmlEncodings;

        private readonly IDictionary<NodeId, NodeId> _parentIds;
        
        private readonly IDictionary<NodeId, TypeInfo> _internalTypes;
        private readonly IDictionary<NodeId, TypeInfo> _assemblyTypes;

        public IEnumerable<BaseDefinition> Definitions { get; }

        private readonly NodeId EnumerationId = NodeId.Parse(DataTypeIds.Enumeration);
        private readonly NodeId StringId = NodeId.Parse(DataTypeIds.String);
        private readonly NodeId UnionId = NodeId.Parse(DataTypeIds.Union);

        public TypeSet(UANodeSet nodeset, Assembly[] assemblies)
        {
            _namespaceUris = (nodeset.NamespaceUris ?? Enumerable.Empty<string>()).Prepend("http://opcfoundation.org/UA/").ToArray();
            _alias = nodeset.Aliases.ToDictionary(a => a.Alias, a => NodeId.Parse(a.Value));

            var table = from assembly in assemblies
                        from type in assembly.ExportedTypes
                        let info = type.GetTypeInfo()
                        let attr = info.GetCustomAttribute<DataTypeIdAttribute>(false)
                        where attr != null
                        select (attr.NodeId, type);

            _assemblyTypes = table.ToDictionary(
                t => ExpandedNodeId.ToNodeId(t.NodeId, _namespaceUris),
                t => new TypeInfo
                {
                    TypeName = t.type.FullName,
                    IsEnum = t.type.IsEnum,
                    IsReference = !t.type.IsValueType,
                    IsAbstract = t.type.IsAbstract,
                    IsClass = t.type.IsClass
                });

            _internalTypes = nodeset.Items
                .OfType<UADataType>()
                .Select(n => (Node: n, ParentId: GetParentId(n)))
                .ToDictionary(
                    n => ParseNodeId(n.Node.NodeId),
                    n => new TypeInfo
                    {
                        TypeName = n.Node.GetTypeName(),
                        IsEnum = n.ParentId == EnumerationId,
                        IsReference = n.ParentId != EnumerationId,
                        IsAbstract = n.ParentId != EnumerationId,
                        IsClass = n.ParentId != EnumerationId && n.ParentId != StringId
                    });

            var encodings = nodeset
                .Items
                .OfType<UAObject>()
                .Where(obj => obj.References.Any(r => r.ReferenceType == "HasEncoding"))
                .Where(obj => obj.References.Any(r => r.ReferenceType == "HasDescription"))
                .Select(obj => new
                {
                    EncodingId = ParseNodeId(obj.NodeId),
                    DataTypeId = obj.References
                        .Where(r => r.ReferenceType == "HasEncoding")
                        .Select(r => ParseNodeId(r.Value))
                        .First(),
                    Encoding = obj.SymbolicName
                });

            _binaryEncodings = encodings
                .Where(e => e.Encoding == "DefaultBinary")
                .ToDictionary(e => e.DataTypeId, e => e.EncodingId);

            _xmlEncodings = encodings
                .Where(e => e.Encoding == "DefaultXml")
                .ToDictionary(e => e.DataTypeId, e => e.EncodingId);

            _parentIds = nodeset.Items
                .OfType<UADataType>()
                .ToDictionary(dt => ParseNodeId(dt.NodeId), dt => GetParentId(dt)); 

            Definitions = nodeset.Items
                .OfType<UADataType>()
                .Select(dt =>
                {
                    var dataTypeId = ParseNodeId(dt.NodeId);
                    _binaryEncodings.TryGetValue(dataTypeId, out var binaryId);
                    _xmlEncodings.TryGetValue(dataTypeId, out var xmlId);

                    var parentId = GetParentId(dt);
                    var parentType = GetNetType(parentId);
                    if (parentId == EnumerationId)
                    {
                        return (BaseDefinition)new EnumDefinition
                        {
                            SymbolicName = dt.GetTypeName(),
                            Description = dt.Description?.FirstOrDefault()?.Value,
                            Documentation = dt.Documentation,
                            DataTypeId = ParseNodeId(dt.NodeId),
                            Items = dt.Definition.Field
                                .Select(f => new EnumDefinition.Item
                                {
                                    SymbolicName = f.SymbolicName ?? f.Name,
                                    Description = f.Description?.FirstOrDefault()?.Value,
                                    Value = f.Value
                                })
                                .ToArray()
                        };
                    }
                    else if (parentType.IsClass)
                    {
                        var fields = dt.Definition?.Field ?? Enumerable.Empty<DataTypeField>();
                        return (BaseDefinition)new ClassDefinition
                        {
                            BinaryEncodingId = binaryId,
                            XmlEncodingId = xmlId,
                            DataTypeId = dataTypeId,
                            SymbolicName = dt.GetTypeName(),
                            Description = dt.Description?.FirstOrDefault()?.Value,
                            Documentation = dt.Documentation,
                            ParentDataTypeId = parentId,
                            IsAbstract = dt.IsAbstract,
                            IsUnion = parentId == UnionId,
                            Namespace = dt.GetNamespace(_namespaceUris),
                            Properties = fields
                                .Select(f => new ClassDefinition.Property
                                {
                                    DataTypeId = ParseNodeId(f.DataType),
                                    SymbolicName = f.SymbolicName ?? f.Name.ToNetIdentifier(),
                                    OpcUaName = f.SymbolicName ?? f.Name,
                                    Description = f.Description?.FirstOrDefault()?.Value,
                                    IsOptional = f.IsOptional,
                                    Rank = f.ValueRank
                                })
                                .ToArray(),
                            OptionalPropertyCount = fields.Count(f => f.IsOptional)
                        };
                    }

                    return null;
                })
                .Where(d => d != null)
                .ToArray();
        }

        public bool TryGetExpandedNodeId(NodeId nodeId, out ExpandedNodeId eId)
        {
            if (nodeId == null)
            {
                eId = null;
                return false;
            }

            eId = NodeId.ToExpandedNodeId(nodeId, _namespaceUris);

            return eId != null;
        }

        public TypeInfo GetNetType(NodeId nodeId)
        {
            if (TryGetForwardId(nodeId, out NodeId forwardId))
            {
                nodeId = forwardId;
            }

            // first, check for the base types
            if (nodeId.NamespaceIndex == 0 && nodeId.IdType == IdType.Numeric)
            {
                switch ((uint)nodeId.Identifier)
                {
                    case BooleanNumber:        return new TypeInfo { TypeName = "bool" };
                    case SByteNumber:          return new TypeInfo { TypeName = "sbyte" };
                    case ByteNumber:           return new TypeInfo { TypeName = "byte" };
                    case Int16Number:          return new TypeInfo { TypeName = "short" };
                    case UInt16Number:         return new TypeInfo { TypeName = "ushort" };
                    case Int32Number:          return new TypeInfo { TypeName = "int" };
                    case UInt32Number:         return new TypeInfo { TypeName = "uint" };
                    case Int64Number:          return new TypeInfo { TypeName = "long" };
                    case UInt64Number:         return new TypeInfo { TypeName = "ulong" };
                    case FloatNumber:          return new TypeInfo { TypeName = "float" };
                    case DoubleNumber:         return new TypeInfo { TypeName = "double" };
                    case StringNumber:         return new TypeInfo { TypeName = "string", IsReference = true };
                    case DateTimeNumber:       return new TypeInfo { TypeName = "System.DateTime" };
                    case GuidNumber:           return new TypeInfo { TypeName = "System.Guid" };
                    case ByteStringNumber:     return new TypeInfo { TypeName = "byte[]", IsReference = true };
                    case XmlElementNumber:     return new TypeInfo { TypeName = "System.XmlElement", IsReference = true };
                }
            }

            // second check for internal types
            if (_internalTypes.TryGetValue(nodeId, out var value))
            {
                return value;
            }
            else if (_assemblyTypes.TryGetValue(nodeId, out value))
            {
                return value;
            }

            return new TypeInfo { TypeName = "upps" };
        }

        private bool TryGetForwardId(NodeId nodeId, out NodeId forwardId)
        {
            var curId = nodeId;
            while (_parentIds.TryGetValue(curId, out var parentId))
                curId = parentId;

            if (curId.NamespaceIndex != 0 || curId.IdType != IdType.Numeric)
            {
                forwardId = null;
                return false;
            }

            switch ((uint)curId.Identifier)
            {
                case 290: // Duration
                    forwardId = NodeId.Parse(DataTypeIds.Double);
                    return true;
                case 294:
                    forwardId = NodeId.Parse(DataTypeIds.DateTime);
                    return true;
                case BooleanNumber:
                case SByteNumber:
                case ByteNumber:
                case Int16Number:
                case UInt16Number:
                case Int32Number:
                case UInt32Number:
                case Int64Number:
                case UInt64Number:
                case FloatNumber:
                case DoubleNumber:
                case StringNumber:
                case DateTimeNumber:
                case GuidNumber:
                case ByteStringNumber:
                case XmlElementNumber:
                case NodeIdNumber:
                case ExpandedNodeIdNumber:
                case StatusCodeNumber:
                case QualifiedNameNumber:
                case LocalizedTextNumber:
                case DataValueNumber:
                case BaseDataTypeNumber:
                case DiagnosticInfoNumber:
                    forwardId = curId;
                    return forwardId != nodeId;
            }

            forwardId = null;
            return false;
        }

        private NodeId ParseNodeId(string txt)
        {
            if (_alias.TryGetValue(txt, out var nodeId))
            {
                return nodeId;
            }

            return NodeId.Parse(txt);
        }

        public NodeId GetParentId(UADataType dt)
            => dt.References
                .Where(r => r.ReferenceType == "HasSubtype")
                .Select(r => ParseNodeId(r.Value))
                .FirstOrDefault();

        public bool ParentHasOptionalProperties(ClassDefinition c)
        {
            var p = Definitions.OfType<ClassDefinition>().FirstOrDefault(d => d.DataTypeId == c.ParentDataTypeId);
            
            return p != null && (p.OptionalPropertyCount != 0 || ParentHasOptionalProperties(p));
        }
    }
}
