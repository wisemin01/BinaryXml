using BenchmarkDotNet.Attributes;
using BinaryXml;
using System.Xml.Linq;

namespace Demo
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        [GlobalSetup]
        public void Setup()
        {
            CreateSamples();
        }

        static void CreateSamples()
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

            doc.Save("Sample.xml");
            BXmlDocument.LoadFromXmlFile("Sample.xml").Save("Sample.bxml");
        }

        // TODO - 다양한 케이스 및 데이터로 테스트 해보면 좋을 듯?
        
        // POD struct.
        public struct SampleItem
        {
            public int ID;
            public string Name;
            public string Description;
            public string Icon;

            public int Atk;
            public int Def;
            public int Hp;
            public int Mp;

            public override string ToString()
            {
                return @$"{nameof(ID)}: {ID}
{nameof(Name)}: {Name}
{nameof(Description)}: {Description}
{nameof(Icon)}: {Icon}
{nameof(Atk)}: {Atk}
{nameof(Def)}: {Def}
{nameof(Hp)}: {Hp}
{nameof(Mp)}: {Mp}";
            }
        }

        [Benchmark]
        public void Benchmark_XDocument()
        {
            var document = XDocument.Load(@"Sample.xml");

            foreach (var e in document.Root.Elements())
            {
                var item = new SampleItem();

                item.ID = int.Parse(e.Attribute("ID").Value);

                item.Name = e.Element("Name").Value;
                item.Description = e.Element("Description").Value;
                item.Icon = e.Element("Icon").Value;

                var options = e.Element("Options");

                item.Atk = int.Parse(options.Element("Atk").Value);
                item.Def = int.Parse(options.Element("Def").Value);
                item.Hp = int.Parse(options.Element("Hp").Value);
                item.Mp = int.Parse(options.Element("Mp").Value);
            }
        }

        [Benchmark]
        public void Benchmark_BXmlDocument()
        {
            using var document = BXmlDocument.LoadFromFile(@"Sample.bxml");

            foreach (var e in document.Root.Elements())
            {
                var item = new SampleItem();

                item.ID = e.Attribute("ID").Value.ToInt();

                item.Name = e.Element("Name").Value.ToString();
                item.Description = e.Element("Description").Value.ToString();
                item.Icon = e.Element("Icon").Value.ToString();

                var options = e.Element("Options");

                item.Atk = options.Element("Atk").Value.ToInt();
                item.Def = options.Element("Def").Value.ToInt();
                item.Hp = options.Element("Hp").Value.ToInt();
                item.Mp = options.Element("Mp").Value.ToInt();
            }
        }
    }
}
