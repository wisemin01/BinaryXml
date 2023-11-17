namespace BinaryXml
{
    internal sealed class BXSequence : IBinarySerializable
    {
        private readonly List<BXSequenceNode> _nodes = new List<BXSequenceNode>();

        internal int Count { get => _nodes.Count; }

        internal BXSequenceNode this[int index]
        {
            get => _nodes[index];
            set => _nodes[index] = value;
        }

        internal void Add(BXSequenceNode node)
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
                var flag = (BXSequenceNodeHeaderFlag)reader.ReadByte();
                var node = BXSequenceNodeFactory.Create(flag);
                
                node.ReadFrom(reader);

                _nodes.Add(node);
            }
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write((Int32)_nodes.Count);

            for (int i = 0; i < _nodes.Count; ++i)
            {
                writer.Write((Byte)_nodes[i].Flag);
                _nodes[i].WriteTo(writer);
            }
        }
    }
}