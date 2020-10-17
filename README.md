# UaTypeGenerator

The _UaTypeGenerator_ generates C# types for OPC UA data types defined in a Nodeset2.xml file. The generated classes and enums are intended to be used with _Converter Systems_ [UaClient](https://github.com/convertersystems/opc-ua-client) library. The generator features following OPC UA types: enumerations, structures, abstract structures, structures with optional fields and unions.

Usage

```sh
dotnet UaTypeGenerator.dll -f Some.NodeSet2.xml -n Ua.Some -a .\Ua.DI.dll
```

This will create a `Some.NodeSet2.cs` file with the namespace `Ua.Some` that can use the types of `Ua.DI.dll`.