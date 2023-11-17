namespace BinaryXml
{
    internal sealed class BXSequence : IBinarySerializable
    {
        private readonly List<BXSequenceNode> _nodes = new List<BXSequenceNode>();

        public void Add(BXSequenceNode node)
        {
            _nodes.Add(node);
        }

        public void ReadFrom(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            _nodes.Clear();
            _nodes.Capacity = count;

            for (int i = 0; i < count; ++i)
            {
                var type = (BXSequenceNodeType)reader.ReadByte();
                var node = BXSequenceNodeFactory.Create(type);
                
                node.ReadFrom(reader);

                _nodes.Add(node);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write((Int32)_nodes.Count);

            for (int i = 0; i < _nodes.Count; ++i)
            {
                writer.Write((Byte)_nodes[i].Type);
                _nodes[i].WriteTo(writer);
            }
        }
    }
}