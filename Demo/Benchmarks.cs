using BenchmarkDotNet.Attributes;
using BinaryXml;
using System.Xml.Linq;

namespace Demo
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        [Benchmark]
        public void Benchmark_XDocument()
        {
            Test_XDocument();
        }

        [Benchmark]
        public void Benchmark_BXmlDocument()
        {
            Test_BXmlDocument();
        }

        // TODO - 다양한 케이스 및 데이터로 테스트 해보면 좋을 듯?

        private static void Test_XDocument()
        {
            var xdoc = XDocument.Load(@"C:\Projects\BinaryXml\Demo\bin\Debug\net7.0\Sample.xml");

            foreach (var child in xdoc.Root.Elements())
            {
                var id = child.Attribute("ID").Value;
                var v = child.Value;
            }
        }

        private static void Test_BXmlDocument()
        {
            var bdoc = BXmlDocument.LoadFromFile(@"C:\Projects\BinaryXml\Demo\bin\Debug\net7.0\Sample.bxml");

            foreach (var child in bdoc.Root.GetChildren())
            {
                var id = child.GetAttribute("ID").Value;
                var v = child.Value;
            }
        }
    }
}
