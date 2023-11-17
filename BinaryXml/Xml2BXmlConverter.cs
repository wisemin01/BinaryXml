using System.Xml;

namespace BinaryXml
{
    internal static class Xml2BXmlConverter
    {
        public static BXmlDocument Convert(XmlReader xmlReader)
        {
            var indexer = new BXStringIndexer();
            var sequence = new BXSequence();

            BXElementSequenceNode lastNode = null;

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                    {
                        var index = indexer.GetOrCreateIndex(xmlReader.Name);
                        var node = new BXElementSequenceNode
                        {
                            TypeIndex = index
                        };
                        sequence.Add(node);

                        if (xmlReader.AttributeCount > 0)
                        {
                            for (int i = 0; i < xmlReader.AttributeCount; ++i)
                            {
                                xmlReader.MoveToAttribute(i);

                                var attrIndex = indexer.GetOrCreateIndex(xmlReader.Name);
                                var attrNode = new BXAttributeSequenceNode()
                                {
                                    TypeIndex = attrIndex,
                                    Data = xmlReader.Value
                                };

                                sequence.Add(attrNode);
                            }
                        }

                        lastNode = node;

                        break;
                    }

                    case XmlNodeType.EndElement:
                    {
                        sequence.Add(new BXEndElementSequenceNode());
                        break;
                    }

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    {
                        if (lastNode != null)
                        {
                            lastNode.Data = xmlReader.Value;
                        }
                        break;
                    }
                }
            }

            return new BXmlDocument(indexer, sequence);
        }
    }
}