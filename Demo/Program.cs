using BenchmarkDotNet.Running;
using BinaryXml;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // BenchmarkRunner.Run<Benchmarks>();

            BXmlDocument.LoadFromXmlFile(@"C:\Projects\BinaryXml\Demo\bin\Debug\net7.0\Sample.xml")
                .Save(@"C:\Projects\BinaryXml\Demo\bin\Debug\net7.0\Sample.bxml");

            using var bdoc = BXmlDocument.LoadFromFile(@"C:\Projects\BinaryXml\Demo\bin\Debug\net7.0\Sample.bxml");

            foreach (var child in bdoc.Root.Elements())
            {
                var id = child.Attribute("ID").Value.ToLong();
                var v = child.Value;
            }
        }
    }
}