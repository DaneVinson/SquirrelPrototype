using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdminConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                UpdateNuspec("1.0.0");
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} - {1}", ex.GetType(), ex.Message);
                Console.WriteLine(ex.StackTrace ?? String.Empty);
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("...");
                Console.ReadKey();
            }
        }

        private static XElement[] GetFileElements()
        {
            List<string> files = new List<string>();
            var extensions = new string[] { ".dll", ".exe", ".config" };
            var directory = new DirectoryInfo(@"C:\Users\dvinson\Documents\GitHub\SquirrelPrototype\Client.Bootstrap\bin\Release");
            foreach (var file in directory.GetFiles().OrderBy(f => f.Name))
            {
                if (extensions.Any(e => e.Equals(file.Extension, StringComparison.OrdinalIgnoreCase)))
                {
                    files.Add(file.Name);
                }
            }

            return files.Select(f => new XElement("file", 
                                        new XAttribute("src", $"{directory.FullName}\\{f}"),
                                        new XAttribute("target", $"lib\\net45\\{f}")))
                            .ToArray();
        }

        private static void UpdateNuspec(string version)
        {
            var element = new XElement("package",
                            //new XAttribute("xmlns", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"),
                            new XElement("metadata",
                                new XElement("id", "Client"),
                                new XElement("version", version),
                                new XElement("authors", "Dane Vinson"),
                                new XElement("requireLicenseAcceptance", false),
                                new XElement("description", "Test client application for prototyping of Squirrel deployments.")),
                            new XElement("files", GetFileElements()));

            using (StreamWriter writer = new StreamWriter($"c:\\temp\\Client.{version}.nuspec", false))
            {
                element.Save(writer);
            }
        }
    }
}
