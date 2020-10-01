using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Workstation.ServiceModel.Ua;

namespace UaTypeGenerator
{
    public class TypeSetWriter
    {
        private readonly TypeSet _typeSet;
        private readonly string _netNamespace;
        
        public TypeSetWriter(TypeSet typeSet, string netNamespace)
        {
            _typeSet = typeSet;
            _netNamespace = netNamespace;
        }

        public void Write(IndentedTextWriter writer)
        {
            WriteNamespaceBegin(writer);
            WriteDefinitions(writer);
            WriteNamespaceEnd(writer);
        }

        private void WriteNamespaceBegin(IndentedTextWriter writer)
        {
            writer.WriteLine($"namespace {_netNamespace}");
            writer.WriteLine("{");
            writer.Indent++;
        }
        
        private void WriteNamespaceEnd(IndentedTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine("}");
        }

        private void WriteDefinitions(IndentedTextWriter writer)
        {
            foreach (var d in _typeSet.Definitions)
            {
                switch (d)
                {
                    case ClassDefinition c when c.IsUnion:
                        WriteUnion(writer, c);
                        break;
                    case ClassDefinition c:
                        WriteClass(writer, c);
                        break;
                    case EnumDefinition e:
                        WriteEnumeration(writer, e);
                        break;
                }
                writer.WriteLine();
            }
        }

        private void WriteClass(IndentedTextWriter writer, ClassDefinition c)
        {
            bool isDerived = false;

            WriteDocumentation(writer, c);
            WriteAttributes(writer, c);

            if (c.ParentDataTypeId != null)
            {
                isDerived = true;
                var netType = _typeSet.GetNetType(c.ParentDataTypeId);
                var absmodifier = c.IsAbstract ? "abstract " : "";
                writer.WriteLine($"public {absmodifier}class {c.SymbolicName} : {netType.TypeName}");
            }
            else
            {
                writer.WriteLine($"public class {c.SymbolicName} : Workstation.ServiceModel.Ua.IEncodable");
            }

            writer.WriteLine("{");
            writer.Indent++;

            foreach (var p in c.Properties)
            {
                var netType = _typeSet.GetNetType(p.DataTypeId);
                var r = p.Rank switch
                {
                    2 => "[,]",
                    1 => "[]",
                    _ => ""
                };
                writer.WriteLine($"public {netType.TypeName}{r} {p.SymbolicName} {{ get; set; }}");
            }

            var modifier = isDerived ? "override" : "virtual";

            if (c.Properties.Any() || !isDerived)
            {
                writer.WriteLine();
                WriteInheritDoc(writer);
                writer.WriteLine($"public {modifier} void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)");
                writer.WriteLine("{");
                writer.Indent++;
                if (isDerived)
                {
                    writer.WriteLine("base.Encode(encoder);");
                }

                writer.WriteLine($"encoder.PushNamespace(\"{c.Namespace}\");");
                foreach (var p in c.Properties)
                {
                    var netType = _typeSet.GetNetType(p.DataTypeId);
                    var suffix = RenderMethodSuffix(netType, p.Rank);

                    writer.WriteLine($"encoder.Write{suffix}(\"{p.SymbolicName}\", {p.SymbolicName});");
                }
                writer.WriteLine("encoder.PopNamespace();");
                writer.Indent--;
                writer.WriteLine("}");

                writer.WriteLine();
                WriteInheritDoc(writer);
                writer.WriteLine($"public {modifier} void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)");
                writer.WriteLine("{");
                writer.Indent++;
                if (isDerived)
                {
                    writer.WriteLine("base.Decode(decoder);");
                }

                writer.WriteLine($"decoder.PushNamespace(\"{c.Namespace}\");");
                foreach (var p in c.Properties)
                {
                    var netType = _typeSet.GetNetType(p.DataTypeId);
                    var suffix = RenderMethodSuffix(netType, p.Rank);

                    writer.WriteLine($"{p.SymbolicName} = decoder.Read{suffix}(\"{p.SymbolicName}\");");
                }
                writer.WriteLine("decoder.PopNamespace();");
                writer.Indent--;
                writer.WriteLine("}");
            }
            writer.Indent--;
            writer.WriteLine("}");
        }

        private void WriteUnion(IndentedTextWriter writer, ClassDefinition c)
        {
            WriteDocumentation(writer, c);
            WriteAttributes(writer, c);

            writer.WriteLine($"public sealed class {c.SymbolicName} : Workstation.ServiceModel.Ua.Union");

            writer.WriteLine("{");
            writer.Indent++;

            writer.WriteLine("public enum UnionField");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("Null = 0,");
            var i = 1;
            foreach (var p in c.Properties)
            {
                writer.WriteLine($"{p.SymbolicName} = {i},");
                i++;
            }
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("private object _field;");
            writer.WriteLine();
            writer.WriteLine("public UnionField SwitchField { get; private set; }");
            writer.WriteLine();
            foreach (var p in c.Properties)
            {
                var netType = _typeSet.GetNetType(p.DataTypeId);
                var r = p.Rank switch
                {
                    2 => "[,]",
                    1 => "[]",
                    _ => ""
                };
                var type = $"{netType.TypeName}{r}";
                writer.WriteLine($"public {type} {p.SymbolicName}");
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine($"get => ({type})_field;");
                writer.WriteLine("set");
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine($"SwitchField = UnionField.{p.SymbolicName};");
                writer.WriteLine($"_field = value;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
            }

            writer.WriteLine();
            WriteInheritDoc(writer);
            writer.WriteLine($"public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)");
            writer.WriteLine("{");
            writer.Indent++;

            writer.WriteLine($"encoder.PushNamespace(\"{c.Namespace}\");");
            writer.WriteLine($"encoder.WriteUInt32(\"SwitchField\", (uint)SwitchField);");
            writer.WriteLine();
            writer.WriteLine($"switch (SwitchField)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine($"case UnionField.Null:");
            writer.Indent++;
            writer.WriteLine("break;");
            writer.Indent--;
            foreach (var p in c.Properties)
            {
                var netType = _typeSet.GetNetType(p.DataTypeId);
                var suffix = RenderMethodSuffix(netType, p.Rank);

                writer.WriteLine($"case UnionField.{p.SymbolicName}:");
                writer.Indent++;
                writer.WriteLine($"encoder.Write{suffix}(\"{p.SymbolicName}\", {p.SymbolicName});");
                writer.WriteLine("break;");
                writer.Indent--;
            }
            writer.WriteLine("default:");
            writer.Indent++;
            writer.WriteLine("throw new Workstation.ServiceModel.Ua.ServiceResultException(Workstation.ServiceModel.Ua.StatusCodes.BadEncodingError);");
            writer.Indent--;

            writer.Indent--;
            writer.WriteLine("}");

            writer.WriteLine("encoder.PopNamespace();");
            writer.Indent--;
            writer.WriteLine("}");

            writer.WriteLine();
            WriteInheritDoc(writer);
            writer.WriteLine($"public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)");
            writer.WriteLine("{");
            writer.Indent++;

            writer.WriteLine($"decoder.PushNamespace(\"{c.Namespace}\");");
            writer.WriteLine();
            writer.WriteLine($"var switchField = (UnionField)decoder.ReadUInt32(null);");
            writer.WriteLine($"switch (switchField)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("case UnionField.Null:");
            writer.Indent++;
            writer.WriteLine("_field = null;");
            writer.WriteLine("break;");
            writer.Indent--;
            foreach (var p in c.Properties)
            {
                var netType = _typeSet.GetNetType(p.DataTypeId);
                var suffix = RenderMethodSuffix(netType, p.Rank);
                
                writer.WriteLine($"case UnionField.{p.SymbolicName}:");
                writer.Indent++;
                writer.WriteLine($"{p.SymbolicName} = decoder.Read{suffix}(\"{p.SymbolicName}\");");
                writer.WriteLine("break;");
                writer.Indent--;
            }
            writer.WriteLine("default:");
            writer.Indent++;
            writer.WriteLine("throw new Workstation.ServiceModel.Ua.ServiceResultException(Workstation.ServiceModel.Ua.StatusCodes.BadEncodingError);");
            writer.Indent--;

            writer.Indent--;
            writer.WriteLine("}");

            writer.WriteLine("decoder.PopNamespace();");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
        }

        private void WriteDocumentation(IndentedTextWriter writer, ClassDefinition c)
        {
            string summary = c.Description is null
                ? $"Class for {c.SymbolicName}"
                : c.Description;
            writer.WriteLine("/// <summary>");
            writer.WriteLine($"/// {summary}");
            writer.WriteLine("/// </summary>");

            if (!string.IsNullOrEmpty(c.Documentation))
            {
                writer.WriteLine($"/// <seealso href=\"{c.Documentation}\" />");
            }
        }
        
        private void WriteDocumentation(IndentedTextWriter writer, EnumDefinition e)
        {
            string summary = e.Description is null
                ? $"{e.SymbolicName} enumeration"
                : e.Description;
            writer.WriteLine("/// <summary>");
            writer.WriteLine($"/// {summary}");
            writer.WriteLine("/// </summary>");

            if (!string.IsNullOrEmpty(e.Documentation))
            {
                writer.WriteLine($"/// <seealso href=\"{e.Documentation}\" />");
            }
        }

        private void WriteInheritDoc(IndentedTextWriter writer)
        {
            writer.WriteLine("/// <<inheritdoc/>");
        }

        private void WriteAttributes(IndentedTextWriter writer, ClassDefinition c)
        {
            if (_typeSet.TryGetExpandedNodeId(c.BinaryEncodingId, out ExpandedNodeId eId))
            {
                writer.WriteLine($"[Workstation.ServiceModel.Ua.BinaryEncodingId(\"{eId}\")]");
            }

            if (_typeSet.TryGetExpandedNodeId(c.XmlEncodingId, out eId))
            {
                writer.WriteLine($"[Workstation.ServiceModel.Ua.XmlEncodingId(\"{eId}\")]");
            }
            
            if (_typeSet.TryGetExpandedNodeId(c.DataTypeId, out eId))
            {
                writer.WriteLine($"[Workstation.ServiceModel.Ua.DataTypeId(\"{eId}\")]");
            }
            else
            {
                throw new InvalidDataException($"No data type id for class {c.SymbolicName}.");
            }
        }

        private string RenderMethodSuffix(TypeInfo netType, int rank)
        {
            if (rank > 0)
            {
                switch (netType.TypeName)
                {
                    case "bool": return "BooleanArray";
                    case "sbyte": return "SByteArray";
                    case "byte": return "ByteString";
                    case "short": return "Int16Array";
                    case "ushort": return "UInt16Array";
                    case "int": return "Int32Array";
                    case "uint": return "UInt32Array";
                    case "long": return "Int64Array";
                    case "ulong": return "UInt64Array";
                    case "float": return "FloatArray";
                    case "double": return "DoubleArray";
                    case "string": return "StringArray";
                    case "System.DateTime": return "DateTimeArray";
                    case "System.Guid": return "GuidArray";
                    case "System.XElement": return "XElementArray";
                    case "Workstation.ServiceModel.Ua.NodeId": return "NodeIdArray";
                    case "Workstation.ServiceModel.Ua.ExpandedNodeId": return "ExpandedNodeIdArray";
                    case "Workstation.ServiceModel.Ua.StatusCode": return "StatusCodeArray";
                    case "Workstation.ServiceModel.Ua.QualifiedName": return "QualifiedNameArray";
                    case "Workstation.ServiceModel.Ua.LocalizedText": return "LocalizedTextArray";
                    case "Workstation.ServiceModel.Ua.DataValue": return "DataValueArray";
                    case "Workstation.ServiceModel.Ua.Variant": return "VariantArray";
                    case "Workstation.ServiceModel.Ua.DiagnosticInfo": return "DiagnosticInfoArray";
                    case "Workstation.ServiceModel.Ua.ExtensionObject": return "ExtensionObjectArray";
                }
            }
            else
            {
                switch (netType.TypeName)
                {
                    case "byte[]": return "ByteString";
                    case "bool": return "Boolean";
                    case "sbyte": return "SByte";
                    case "byte": return "Byte";
                    case "short": return "Int16";
                    case "ushort": return "UInt16";
                    case "int": return "Int32";
                    case "uint": return "UInt32";
                    case "long": return "Int64";
                    case "ulong": return "UInt64";
                    case "float": return "Float";
                    case "double": return "Double";
                    case "string": return "String";
                    case "System.DateTime": return "DateTime";
                    case "System.Guid": return "Guid";
                    case "System.XElement": return "XElement";
                    case "Workstation.ServiceModel.Ua.NodeId": return "NodeId";
                    case "Workstation.ServiceModel.Ua.ExpandedNodeId": return "ExpandedNodeId";
                    case "Workstation.ServiceModel.Ua.StatusCode": return "StatusCode";
                    case "Workstation.ServiceModel.Ua.QualifiedName": return "QualifiedName";
                    case "Workstation.ServiceModel.Ua.LocalizedText": return "LocalizedText";
                    case "Workstation.ServiceModel.Ua.DataValue": return "DataValue";
                    case "Workstation.ServiceModel.Ua.Variant": return "Variant";
                    case "Workstation.ServiceModel.Ua.DiagnosticInfo": return "DiagnosticInfo";
                    case "Workstation.ServiceModel.Ua.ExtensionObject": return "ExtensionObject";
                }
            }

            var array = rank > 0 ? "Array" : "";
            if (netType.IsEnum)
            {
                return $"Enumeration{array}<{netType.TypeName}>";
            }
            else if (netType.IsAbstract)
            {
                return $"ExtensionObject{array}<{netType.TypeName}>";
            }

            return $"Encodable{array}<{netType.TypeName}>";
        }

        private void WriteEnumeration(IndentedTextWriter writer, EnumDefinition e)
        {
            WriteDocumentation(writer, e);

            if (_typeSet.TryGetExpandedNodeId(e.DataTypeId, out var eId))
            {
                writer.WriteLine($"[Workstation.ServiceModel.Ua.DataTypeId(\"{eId}\")]");
            }
            else
            {
                throw new InvalidDataException($"No data type id for enum {e.SymbolicName}.");
            }

            writer.WriteLine($"public enum {e.SymbolicName}");
            writer.WriteLine("{");
            writer.Indent++;

            foreach (var item in e.Items)
            {
                if (item.Description != null)
                {
                    writer.WriteLine($"/// <summary>{item.Description}</summary>");
                }
                writer.WriteLine($"{item.SymbolicName} = {item.Value},");
            }

            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
