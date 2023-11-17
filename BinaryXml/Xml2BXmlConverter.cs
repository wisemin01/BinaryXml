using System.Xml;

namespace BinaryXml
{
    /// <summary>
    ///     Supports converting .xml to .bxml.
    /// </summary>
    internal static class Xml2BXmlConverter
    {
        /// <summary>
        ///     Reads the file from the corresponding <paramref name="xmlReader"/> and converts it to a .bxml file.
        /// </summary>
        public static BXmlDocument Convert(XmlReader xmlReader)
        {
            var indexer = new BXStringIndexer();
            var sequence = new BXSequence();

            BXElementSequenceNode lastNode = null;

            // stack: Stack to know the parent node in the current context.
            // childrenMapper: Since child nodes with the same parent must be stored consecutively, a dictionary to be temporarily stored is created to satisfy the condition.
            var stack = new Stack<BXElementSequenceNode>();
            var childrenMapper = new Dictionary<BXElementSequenceNode, List<BXElementSequenceNode>>();

            sequence.Add(null); // reserve to root element.

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                    {
                        var typePtr = indexer.GetOrCreateIndex(xmlReader.Name); // Indexing.
                        var node = new BXElementSequenceNode();

                        node.TypePtr = typePtr; // Sequence node has an index instead of a name.

                        var attributeCount = xmlReader.AttributeCount;
                        if (attributeCount > 0)
                        {
                            node.AttributePtr = sequence.Count;
                            node.AttributeCount = attributeCount;

                            for (int i = 0; i < attributeCount; ++i)
                            {
                                xmlReader.MoveToAttribute(i);

                                var attrTypePtr = indexer.GetOrCreateIndex(xmlReader.Name); // Indexing.
                                var attrNode = new BXAttributeSequenceNode()
                                {
                                    TypePtr = attrTypePtr,
                                    Data = xmlReader.Value
                                };

                                sequence.Add(attrNode);
                            }
                        }

                        if (stack.TryPeek(out var parent))
                        {
                            if (!childrenMapper.TryGetValue(parent, out var children))
                            {
                                childrenMapper.Add(parent, children = new List<BXElementSequenceNode>());
                            }
                            children.Add(node);
                        }
                        stack.Push(node);
                        lastNode = node;

                        break;
                    }

                    case XmlNodeType.EndElement:
                    {
                        if (stack.TryPop(out var node))
                        {
                            if (childrenMapper.TryGetValue(node, out var children))
                            {
                                node.ChildPtr = sequence.Count;
                                node.ChildCount = children.Count;

                                foreach (var child in children)
                                {
                                    sequence.Add(child);
                                }

                                childrenMapper.Remove(node);
                            }

                            if (stack.Count == 0)
                            {
                                sequence[0] = node; // set root element.
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

            return new BXmlDocument(indexer, sequence);
        }
    }
}