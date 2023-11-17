using System.Xml;

namespace BinaryXml
{
    /// <summary>
    ///     Represents an BXML document.
    /// </summary>
    public sealed class BXmlDocument : IBinarySerializable
    {
        private readonly BXStringIndexer _indexer;
        private readonly BXSequence _sequence;
        
        internal BXStringIndexer Indexer { get =>  _indexer; }
        internal BXSequence Sequence { get => _sequence; }

        public BXmlElement Root
        {
            get => new BXmlElement(this, 0);
        }

        public BXmlDocument() : this(new BXStringIndexer(), new BXSequence())
        {
        }

        internal BXmlDocument(BXStringIndexer indexer, BXSequence sequence)
        {
            _indexer = indexer;
            _sequence = sequence;
        }

        public void ReadFrom(BinaryReader reader)
        {
            _indexer.ReadFrom(reader);
            _sequence.ReadFrom(reader);
        }

        public void WriteTo(BinaryWriter writer)
        {
            _indexer.WriteTo(writer);
            _sequence.WriteTo(writer);
        }

        public void Save(Stream stream)
        {
            using var writer = new BinaryWriter(stream);
            WriteTo(writer);
        }

        public void Save(string path)
        {
            using var fs = File.Open(path, FileMode.Create);
            Save(fs);
        }

        public static BXmlDocument LoadFromXml(XmlReader xmlReader)
        {
            var document = Xml2BXmlConverter.Convert(xmlReader);
            return document;
        }

        public static BXmlDocument LoadFromXmlFile(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
            using (var xmlReader = XmlReader.Create(fs))
            {
                return Xml2BXmlConverter.Convert(xmlReader);
            }
        }

        public static BXmlDocument Load(BinaryReader reader)
        {
            var document = new BXmlDocument();
            document.ReadFrom(reader);
            return document;
        }

        public static BXmlDocument LoadFromStream(Stream input)
        {
            var document = new BXmlDocument();

            using (var reader = new BinaryReader(input))
            {
                document.ReadFrom(reader);
            }

            return document;
        }

        public static BXmlDocument LoadFromFile(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
            {
                return LoadFromStream(fs);
            }
        }
    }
}