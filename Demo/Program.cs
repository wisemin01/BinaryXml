using BinaryXml;
using System.Xml.Linq;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var xdoc = new XDocument();
            xdoc.Add(new XElement("Elements"));

            for (int i = 0; i < 10000; ++i)
            {
                xdoc.Root.Add(new XElement("Element", "Hello world!"));
            }

            xdoc.Save("Sample.xml");

            var bdoc = BXmlDocument.LoadFromXmlFile(@"Sample.xml");
            bdoc.Save("Sample.bxml");

            var bdoc2 = BXmlDocument.LoadFromFile(@"Sample.bxml");
            bdoc2.Save("Sample2.bxml");
        }
    }
}