using Mono.Options;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Schema;
using System.Xml.Serialization;
using Workstation.ServiceModel.Ua;

namespace UaTypeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var shouldShowHelp = false;
            
            var nodesetFile = default(string);
            var ns = default(string);
            var assembliesNames = new List<string>();

            var p = new Mono.Options.OptionSet {
                { "f|nodeset=",   "the nodeset file",            f =>nodesetFile = f },
                { "n|namespace=", "the .NET namespace.",         n => ns = n },
                { "a|additional-assemblies=", "the .NET namespace.", a => assembliesNames.Add(a) },
                { "h|help",       "show this message and exit",  h => shouldShowHelp = h != null },
            };

            try
            {
                p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("UaTypeGenerator.exe: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `UaTypeGenerator.exe --help' for more information.");
                return;
            }

            if (shouldShowHelp)
            {
                ShowHelp(p);
                return;
            }

            var assemblies = assembliesNames
                .Select(n => Assembly.LoadFrom(n))
                .Prepend(typeof(OpenSecureChannelRequest).Assembly)
                .ToArray();

            var nodeset = ReadNodeSet(nodesetFile);

            var typeset = new TypeSet(nodeset, assemblies);
            var typewriter = new TypeSetWriter(typeset, ns);

            var outputFile = Path.ChangeExtension(nodesetFile, ".cs");
            using (var writer = File.CreateText(outputFile))
            {
                var intendedWriter = new IndentedTextWriter(writer);
                typewriter.Write(intendedWriter);
            }
        }

        private static UANodeSet ReadNodeSet(string nodesetFile)
        {
            var serializer = new XmlSerializer(typeof(UANodeSet));
            var file = new StreamReader(nodesetFile);

            var set = (UANodeSet)serializer.Deserialize(file);

            file.Close();

            return set;
        }

        private static void ShowHelp(Mono.Options.OptionSet p)
        {
            Console.WriteLine("Usage: UaTypeGenerator.exe [OPTIONS]+");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
