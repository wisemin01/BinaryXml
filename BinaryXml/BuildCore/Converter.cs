using BinaryXml.Internal;
using System.Xml;

namespace BinaryXml.BuildCore
{
    // Temp Class
    internal class BuildingNode
    {
        public int NameOffset;
        public int DataOffset;
        public int ChildOffset;
        public int ChildCount;
        public int AttributeOffset;
        public int AttributeCount;
        public string Data;

        internal BXmlElementEntry ToEntry()
        {
            return new BXmlElementEntry()
            {
                NameOffset = NameOffset,
                DataOffset = DataOffset,
                ChildOffset = ChildOffset,
                ChildCount = ChildCount,
                AttributeOffset = AttributeOffset,
                AttributeCount = AttributeCount
            };
        }
    }

    internal static class Converter
    {
        public static BXmlDocument Convert(XmlReader xmlReader)
        {
            using var headerSection = new HeaderSection();
            using var nameTableSection = new NameTableSection();
            using var elementEntrySection = new ElementEntrySection();
            using var attributeEntrySection = new AttributeEntrySection();
            using var dataSection = new DataSection();

            BuildingNode lastNode = null;

            var stack = new Stack<BuildingNode>();
            var childrenMapper = new Dictionary<BuildingNode, List<BuildingNode>>();

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                    {
                        var isEmptyElement = xmlReader.IsEmptyElement;

                        var nameOffset = nameTableSection.GetOffset(xmlReader.Name);
                        var node = new BuildingNode();

                        node.NameOffset = nameOffset; // Sequence node has an index instead of a name.

                        var attributeCount = xmlReader.AttributeCount;
                        if (attributeCount > 0)
                        {
                            node.AttributeOffset = attributeEntrySection.CurrentOffset;
                            node.AttributeCount = attributeCount;

                            for (int i = 0; i < attributeCount; ++i)
                            {
                                xmlReader.MoveToAttribute(i);

                                var attributeNameOffset = nameTableSection.GetOffset(xmlReader.Name);
                                var attributeDataOffset = dataSection.GetOffset(xmlReader.Value);

                                var entry = new BXmlAttributeEntry()
                                {
                                    NameOffset = attributeNameOffset,
                                    DataOffset = attributeDataOffset
                                };

                                attributeEntrySection.AddEntry(entry);
                            }
                        }

                        if (stack.TryPeek(out var parent))
                        {
                            if (!childrenMapper.TryGetValue(parent, out var children))
                            {
                                childrenMapper.Add(parent, children = new List<BuildingNode>());
                            }

                            children.Add(node);
                        }

                        if (!isEmptyElement)
                        {
                            stack.Push(node);
                        }

                        lastNode = node;
                        break;
                    }

                    case XmlNodeType.EndElement:
                    {
                        if (stack.TryPop(out var node))
                        {
                            if (childrenMapper.TryGetValue(node, out var children))
                            {
                                node.ChildOffset = elementEntrySection.CurrentOffset;
                                node.ChildCount = children.Count;

                                foreach (var child in children)
                                {
                                    var dataOffset = dataSection.GetOffset(child.Data);
                                    child.DataOffset = dataOffset;

                                    var entry = child.ToEntry();
                                    elementEntrySection.AddEntry(entry);
                                }

                                childrenMapper.Remove(node);
                            }

                            if (stack.Count == 0)
                            {
                                var dataOffset = dataSection.GetOffset(node.Data);
                                node.DataOffset = dataOffset;

                                var entry = node.ToEntry();
                                elementEntrySection.AddRootEntry(entry);
                            }
                        }
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

            using var outStream = new MemoryStream();

            outStream.Seek(BXmlHeader.Size, SeekOrigin.Begin);

            headerSection.MarkNameTable((int)outStream.Position);
            nameTableSection.WriteTo(outStream);

            headerSection.MarkElementEntry((int)outStream.Position);
            elementEntrySection.WriteTo(outStream);

            headerSection.MarkAttributeEntry((int)outStream.Position);
            attributeEntrySection.WriteTo(outStream);

            headerSection.MarkDataOffset((int)outStream.Position);
            dataSection.WriteTo(outStream);

            outStream.Seek(0, SeekOrigin.Begin);
            headerSection.WriteTo(outStream);

            return new BXmlDocument(outStream.ToArray());
        }
    }
}
