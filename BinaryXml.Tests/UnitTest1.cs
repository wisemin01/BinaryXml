using NUnit.Framework;
using System.Xml.Linq;

namespace BinaryXml.Tests
{
    public class Tests
    {
        private string _sampleXml;

        // TODO - 유닛 테스트 작성.

        [SetUp]
        public void Setup()
        {
            _sampleXml = CreateSampleXml();
        }

        static string CreateSampleXml()
        {
            var doc = new XDocument();
            var root = new XElement("Sample");

            doc.Add(root);

            for (int i = 0; i < 1000; ++i)
            {
                var id = i + 1;

                var item = new XElement("Item");

                item.Add(new XAttribute("ID", id));
                item.Add(new XElement("Name", $"Item No. {id}"));
                item.Add(new XElement("Description", "Item Description."));
                item.Add(new XElement("Icon", $"Data/Icons/icon_{id}.png"));

                var options = new XElement("Options");

                options.Add(new XElement("Atk", 10));
                options.Add(new XElement("Def", 20));
                options.Add(new XElement("Hp", 30));
                options.Add(new XElement("Mp", 40));

                item.Add(options);

                root.Add(item);
            }

            return doc.ToString();
        }

        [Test]
        public void Test1()
        {
            var document = BXmlDocument.LoadFromXml(_sampleXml);

            Assert.That(document.Root.Name.SequenceEqual("Sample"u8), Is.True);
            Assert.That(document.Root.Name.ToString(), Is.EqualTo("Sample"));

            Assert.That(document.Root.Elements()[500].Attribute("ID"u8).Value.ToInt(), Is.EqualTo(501));
        }
    }
}