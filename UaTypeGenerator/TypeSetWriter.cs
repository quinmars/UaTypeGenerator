﻿using Org.BouncyCastle.Security;
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
            WriteFileHeader(writer);

            writer.WriteLine($"namespace {_netNamespace}");
            writer.Begin("{");

            WriteCommonTypes(writer);
            
            WriteDefinitions(writer);
            
            writer.End("}");
        }

        private void WriteFileHeader(IndentedTextWriter writer)
        {
            writer.WriteLine("// ------------------------------------------------------------------------------");
            writer.WriteLine("// <auto-generated>");
            writer.WriteLine("//     This code was generated by a tool.");
            writer.WriteLine("//");
            writer.WriteLine("//     Changes to this file may cause incorrect behavior and will be lost if");
            writer.WriteLine("//     the code is regenerated");
            writer.WriteLine("// </auto-generated>");
            writer.WriteLine("// ------------------------------------------------------------------------------");
            writer.WriteLine();
        }

        private void WriteCommonTypes(IndentedTextWriter writer)
        {
            writer.WriteLine("// Ideally this should go into Workstation.UaClient");
            writer.WriteLine("public interface IOptionalFields");
            writer.Begin("{");
            writer.WriteLine("int OptionalFieldCount { get; }");
            writer.WriteLine("uint EncodingMask { get; }");
            writer.End("}");
            writer.WriteLine();
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

        /*
         * Class and unions
         */
        private void WriteClass(IndentedTextWriter writer, ClassDefinition c)
        {
            bool isDerived = c.ParentDataTypeId != null;

            WriteDocumentation(writer, c, isUnion: false);
            WriteAttributes(writer, c);

            var hasOptionalFields = c.OptionalPropertyCount != 0;
            var parentHasOptionalFields = _typeSet.ParentHasOptionalProperties(c);

            WriteClassHeader(writer, c, parentHasOptionalFields);

            writer.Begin("{");

            if (hasOptionalFields)
            {
                WriteOptionalFieldsImplementation(writer, c, parentHasOptionalFields);
            }

            var optional = 0;
            foreach (var p in c.Properties)
            {
                if (p.IsOptional)
                {
                    WriteOptionalProperty(writer, p, optional, parentHasOptionalFields);
                    optional++;
                }
                else
                {
                    WriteProperty(writer, p);
                }
                writer.WriteLine();
            }

            if (c.Properties.Any() || !isDerived)
            {
                WriteEncodeMethod(writer, c, isDerived, parentHasOptionalFields);
                writer.WriteLine();
                WriteDecodeMethod(writer, c, isDerived, parentHasOptionalFields);
            }
            writer.End("}");
        }

        private void WriteDocumentation(IndentedTextWriter writer, ClassDefinition c, bool isUnion)
        {
            string summary = c.Description is null
                ? $"Class for the {c.SymbolicName} data type."
                : c.Description;
            writer.WriteLine("/// <summary>");
            foreach (var line in summary.WordWrap(76 - 4 * writer.Indent))
            {
                writer.WriteLine($"/// {line}");
            }
            writer.WriteLine("/// </summary>");

            if (isUnion)
            {
                var remarks = "This class is an implementation of the union type. That means "
                    + "only one of its properties is accessible. Which properity is accessible can "
                    + "be tested by the <see cref=\"SwitchField\" /> property.";
                writer.WriteLine("/// <remarks>");
                foreach (var line in remarks.WordWrap(76 - 4 * writer.Indent))
                {
                    writer.WriteLine($"/// {line}");
                }
                writer.WriteLine("/// </remarks>");
            }

            if (!string.IsNullOrEmpty(c.Documentation))
            {
                writer.WriteLine($"/// <seealso href=\"{c.Documentation}\" />");
            }
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

        private void WriteClassHeader(IndentedTextWriter writer, ClassDefinition c, bool parentHasOptionalFields)
        {
            var hasOptionalFields = c.OptionalPropertyCount != 0;
            string optionalInterface = (hasOptionalFields && !parentHasOptionalFields) ? "," : "";
            if (c.ParentDataTypeId != null)
            {
                var netType = _typeSet.GetNetType(c.ParentDataTypeId);
                var absmodifier = c.IsAbstract ? "abstract " : "";

                writer.WriteLine($"public {absmodifier}class {c.SymbolicName} : {netType.TypeName}{optionalInterface}");
            }
            else
            {
                writer.WriteLine($"public class {c.SymbolicName} : Workstation.ServiceModel.Ua.IEncodable{optionalInterface}");
            }
            if (hasOptionalFields && !parentHasOptionalFields)
            {
                writer.Indent++;
                writer.Indent++;
                writer.WriteLine("IOptionalFields");
                writer.Indent--;
                writer.Indent--;
            }
        }
        
        private void WriteEncodeMethod(IndentedTextWriter writer, ClassDefinition c, bool isDerived, bool parentHasOptionalFields)
        {
            var hasOptionalFields = c.OptionalPropertyCount != 0;
            var modifier = isDerived ? "override" : "virtual";

            WriteInheritDoc(writer);
            writer.WriteLine($"public {modifier} void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)");
            writer.Begin("{");
            if (isDerived)
            {
                writer.WriteLine("base.Encode(encoder);");
            }

            writer.WriteLine($"encoder.PushNamespace(\"{c.Namespace}\");");
            writer.WriteLine();
            if (hasOptionalFields && !parentHasOptionalFields)
            {
                writer.WriteLine("encoder.WriteUInt32(\"EncodingMask\", EncodingMask);");
            }

            var index = 0;
            foreach (var p in c.Properties)
            {
                var netType = _typeSet.GetNetType(p.DataTypeId);
                var suffix = RenderMethodSuffix(netType, p.Rank);

                if (p.IsOptional)
                {
                    writer.WriteLine($"if ({p.SymbolicName} is {{}} opt{index})");
                    writer.Begin("{");
                    writer.WriteLine($"encoder.Write{suffix}(\"{p.SymbolicName}\", opt{index});");
                    writer.End("}");
                    index++;
                }
                else
                {
                    writer.WriteLine($"encoder.Write{suffix}(\"{p.SymbolicName}\", {p.SymbolicName});");
                }
            }
            writer.WriteLine();
            writer.WriteLine("encoder.PopNamespace();");
            writer.End("}");
        }

        private void WriteDecodeMethod(IndentedTextWriter writer, ClassDefinition c, bool isDerived, bool parentHasOptionalFields)
        {
            var hasOptionalFields = c.OptionalPropertyCount != 0;
            var modifier = isDerived ? "override" : "virtual";

            WriteInheritDoc(writer);
            writer.WriteLine($"public {modifier} void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)");
            writer.Begin("{");
            if (hasOptionalFields && parentHasOptionalFields)
            {
                writer.WriteLine("int offset = base.OptionalFieldCount;");
                writer.WriteLine();
            }
            if (isDerived)
            {
                writer.WriteLine("base.Decode(decoder);");
            }

            writer.WriteLine($"decoder.PushNamespace(\"{c.Namespace}\");");
            writer.WriteLine();
            if (hasOptionalFields)
            {
                if (!parentHasOptionalFields)
                {
                    writer.WriteLine("var encodingMask = decoder.ReadUInt32(null);");
                    writer.WriteLine("EncodingMask = encodingMask;");
                }
                else
                {
                    writer.WriteLine("var encodingMask = EncodingMask;");
                }
                writer.WriteLine();
            }

            var index = 0;
            foreach (var p in c.Properties)
            {
                var netType = _typeSet.GetNetType(p.DataTypeId);
                var suffix = RenderMethodSuffix(netType, p.Rank);

                if (p.IsOptional)
                {
                    if (parentHasOptionalFields)
                    {
                        writer.WriteLine($"{p.SymbolicName} = (encodingMask & (1u << ({index} + offset))) != 0");
                    }
                    else
                    {
                        writer.WriteLine($"{p.SymbolicName} = (encodingMask & (1u << {index})) != 0");
                    }
                    writer.Indent++;
                    writer.WriteLine($"? decoder.Read{suffix}(\"{p.SymbolicName}\")");
                    writer.WriteLine($": default;");
                    writer.Indent--;
                    index++;
                }
                else
                {
                    writer.WriteLine($"{p.SymbolicName} = decoder.Read{suffix}(\"{p.SymbolicName}\");");
                }
            }
            writer.WriteLine();
            writer.WriteLine("decoder.PopNamespace();");
            writer.End("}");
        }

        
        private void WriteOptionalFieldsImplementation(IndentedTextWriter writer, ClassDefinition c, bool parentHasOptionalFields)
        {
            if (parentHasOptionalFields)
            {
                WriteInheritDoc(writer);
                writer.WriteLine("public override int OptionalFieldCount => base.OptionalFieldCount + {0};", c.OptionalPropertyCount);
            }
            else
            {
                WriteInheritDoc(writer);
                writer.WriteLine("public virtual int OptionalFieldCount => {0};", c.OptionalPropertyCount);
                WriteInheritDoc(writer);
                writer.WriteLine("public uint EncodingMask { get; protected set; }");
            }
            writer.WriteLine();
        }

        private void WriteProperty(IndentedTextWriter writer, ClassDefinition.Property p)
        {
            WritePropertyDocumentation(writer, p, isUnion: false);
            var netType = _typeSet.GetNetType(p.DataTypeId);
            var r = p.Rank switch
            {
                2 => "[,]",
                1 => "[]",
                _ => ""
            };
            writer.WriteLine($"public {netType.TypeName}{r} {p.SymbolicName} {{ get; set; }}");
        }

        private void WriteOptionalProperty(IndentedTextWriter writer, ClassDefinition.Property p, int index, bool parentHasOptionalFields)
        {
            WritePropertyDocumentation(writer, p, isUnion: false);
            var netType = _typeSet.GetNetType(p.DataTypeId);
            var r = p.Rank switch
            {
                2 => "[,]",
                1 => "[]",
                _ => ""
            };
            var q = (netType.IsReference || p.Rank > 0) ? "" : "?";
            var field = GetFieldName(p.SymbolicName);

            writer.WriteLine($"public {netType.TypeName}{r}{q} {p.SymbolicName}");
            writer.Begin("{");
            writer.WriteLine($"get => {field};");
            writer.WriteLine("set");
            writer.Begin("{");
            if (parentHasOptionalFields)
            {
                writer.WriteLine($"uint flag = 1u << ({index} + base.OptionalFieldCount);");
            }
            else
            {
                writer.WriteLine($"uint flag = 1u << {index};");
            }
            writer.WriteLine();
            writer.WriteLine($"{field} = value;");
            writer.WriteLine("EncodingMask = value is null");
            writer.Indent++;
            writer.WriteLine("? EncodingMask & ~flag");
            writer.WriteLine(": EncodingMask | flag;");
            writer.Indent--;
            writer.End("}");
            writer.End("}");
            writer.WriteLine($"private {netType.TypeName}{r}{q} {field};");
        }
        
        private void WritePropertyDocumentation(IndentedTextWriter writer, ClassDefinition.Property p, bool isUnion)
        {
            string summary = p.Description is null
                ? $"The {p.SymbolicName} property."
                : p.Description;
            writer.WriteLine("/// <summary>");
            foreach (var line in summary.WordWrap(76 - 4 * writer.Indent))
            {
                writer.WriteLine($"/// {line}");
            }
            writer.WriteLine("/// </summary>");

            if (isUnion)
            {
                var remarks = "The value of this property may only be retrieved, when "
                    + $"the <see cref=\"SwitchField\" /> property.is set to <c>UnionField.{p.SymbolicName}</c>. "
                    + "Otherwise the behavior is undefined and can lead to invalid data or "
                    + "an <see cref=\"System.InvalidCastException\" /> exeption.";
                writer.WriteLine("/// <remarks>");
                foreach (var line in remarks.WordWrap(76 - 4 * writer.Indent))
                {
                    writer.WriteLine($"/// {line}");
                }
                writer.WriteLine("/// </remarks>");
            }

            if (!string.IsNullOrEmpty(p.Documentation))
            {
                writer.WriteLine($"/// <seealso href=\"{p.Documentation}\" />");
            }

        }

        /*
         * Union
         */
        private void WriteUnion(IndentedTextWriter writer, ClassDefinition c)
        {
            WriteDocumentation(writer, c, isUnion: true);
            WriteAttributes(writer, c);

            WriteUnionHeader(writer, c);

            writer.Begin("{");

            WriteUnionEnum(writer, c);
            writer.WriteLine();
            WriteUnionFields(writer);

            writer.WriteLine();
            foreach (var p in c.Properties)
            {
                WriteUnionProperty(writer, p);
                writer.WriteLine();
            }

            WriteUnionEncodeMethod(writer, c);
            writer.WriteLine();
            WriteUnionDecodeMethod(writer, c);

            writer.End("}");
        }

        private void WriteUnionHeader(IndentedTextWriter writer, ClassDefinition c)
        {
            writer.WriteLine($"public sealed class {c.SymbolicName} : Workstation.ServiceModel.Ua.Union");
        }

        private void WriteUnionEnum(IndentedTextWriter writer, ClassDefinition c)
        {
            writer.WriteLine("public enum UnionField");
            writer.Begin("{");
            writer.WriteLine("Null = 0,");
            var i = 1;
            foreach (var p in c.Properties)
            {
                writer.WriteLine($"{p.SymbolicName} = {i},");
                i++;
            }
            writer.End("}");
        }

        private void WriteUnionFields(IndentedTextWriter writer)
        {
            writer.WriteLine("private object _field;");
            writer.WriteLine();
            writer.WriteLine("public UnionField SwitchField { get; private set; }");
        }

        private void WriteUnionProperty(IndentedTextWriter writer, ClassDefinition.Property p)
        {
            WritePropertyDocumentation(writer, p, isUnion: true);
            var netType = _typeSet.GetNetType(p.DataTypeId);
            var r = p.Rank switch
            {
                2 => "[,]",
                1 => "[]",
                _ => ""
            };
            var type = $"{netType.TypeName}{r}";
            writer.WriteLine($"public {type} {p.SymbolicName}");
            writer.Begin("{");
            writer.WriteLine($"get => ({type})_field;");
            writer.WriteLine("set");
            writer.Begin("{");
            writer.WriteLine($"SwitchField = UnionField.{p.SymbolicName};");
            writer.WriteLine($"_field = value;");
            writer.End("}");
            writer.End("}");
        }

        private void WriteUnionEncodeMethod(IndentedTextWriter writer, ClassDefinition c)
        {
            WriteInheritDoc(writer);
            writer.WriteLine($"public override void Encode(Workstation.ServiceModel.Ua.IEncoder encoder)");
            writer.Begin("{");

            writer.WriteLine($"encoder.PushNamespace(\"{c.Namespace}\");");
            writer.WriteLine();
            writer.WriteLine($"encoder.WriteUInt32(\"SwitchField\", (uint)SwitchField);");
            writer.WriteLine($"switch (SwitchField)");
            writer.Begin("{");
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

            writer.End("}");

            writer.WriteLine("encoder.PopNamespace();");
            writer.End("}");
        }

        private void WriteUnionDecodeMethod(IndentedTextWriter writer, ClassDefinition c)
        {
            WriteInheritDoc(writer);
            writer.WriteLine($"public override void Decode(Workstation.ServiceModel.Ua.IDecoder decoder)");
            writer.Begin("{");

            writer.WriteLine($"decoder.PushNamespace(\"{c.Namespace}\");");
            writer.WriteLine();
            writer.WriteLine($"var switchField = (UnionField)decoder.ReadUInt32(null);");
            writer.WriteLine($"switch (switchField)");
            writer.Begin("{");
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

            writer.End("}");

            writer.WriteLine("decoder.PopNamespace();");
            writer.End("}");
        }

        /*
         * Enumeration
         */
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
            writer.Begin("{");

            foreach (var item in e.Items)
            {
                if (item.Description != null)
                {
                    writer.WriteLine("/// <summary>");
                    foreach (var line in item.Description.WordWrap(76 - 4 * writer.Indent))
                    {
                        writer.WriteLine($"/// {line}");
                    }
                    writer.WriteLine("/// </summary>");
                }
                writer.WriteLine($"{item.SymbolicName} = {item.Value},");
            }

            writer.End("}");
        }
        
        private void WriteDocumentation(IndentedTextWriter writer, EnumDefinition e)
        {
            string summary = e.Description is null
                ? $"The {e.SymbolicName} enumeration."
                : e.Description;
            writer.WriteLine("/// <summary>");
            foreach (var line in summary.WordWrap(76 - 4 * writer.Indent))
            {
                writer.WriteLine($"/// {line}");
            }
            writer.WriteLine("/// </summary>");

            if (!string.IsNullOrEmpty(e.Documentation))
            {
                writer.WriteLine($"/// <seealso href=\"{e.Documentation}\" />");
            }
        }

        /*
         * Helper
         */
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
        
        private string GetFieldName(string name)
        {
            return "_" + Char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
        
        private void WriteInheritDoc(IndentedTextWriter writer)
        {
            writer.WriteLine("/// <<inheritdoc/>");
        }
    }
}
