using BenchmarkDotNet.Running;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarks>();

            //BXmlDocument.LoadFromXmlFile(@"C:\Projects\BinaryXml\Demo\bin\Debug\net7.0\Sample.xml")
            //    .Save(@"C:\Projects\BinaryXml\Demo\bin\Debug\net7.0\Sample.bxml");

            //var bdoc = BXmlDocument.LoadFromFile(@"C:\Projects\BinaryXml\Demo\bin\Debug\net7.0\Sample.bxml");

            //foreach (var child in bdoc.Root.Elements())
            //{
            //    Console.WriteLine(child.Name.ToString());
            //    Console.WriteLine(child.Value.ToString());
            //}
        }
    }
}