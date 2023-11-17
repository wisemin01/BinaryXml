namespace BinaryXml
{
    // Base class.
    internal abstract class BXSequenceNode : IBinarySerializable
    {
        public abstract BXSequenceNodeType Type { get; }

        public abstract void ReadFrom(BinaryReader reader);
        public abstract void WriteTo(BinaryWriter writer);
    }

    // Element
    internal sealed class BXElementSequenceNode : BXSequenceNode
    {
        public override BXSequenceNodeType Type { get => BXSequenceNodeType.Element; }

        public int TypeIndex { get; set; }
        public string Data { get; set; }

        public override void ReadFrom(BinaryReader reader)
        {
            this.TypeIndex = reader.ReadInt32();
            this.Data = reader.ReadString();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write((Int32)this.TypeIndex);
            writer.Write((String)(this.Data ?? string.Empty));
        }
    }

    // EndElement
    internal sealed class BXEndElementSequenceNode : BXSequenceNode
    {
        public override BXSequenceNodeType Type { get => BXSequenceNodeType.EndElement; }

        public override void ReadFrom(BinaryReader reader)
        {
        }

        public override void WriteTo(BinaryWriter writer)
        {
        }
    }

    // Attribute
    internal sealed class BXAttributeSequenceNode : BXSequenceNode
    {
        public override BXSequenceNodeType Type { get => BXSequenceNodeType.Attribute; }

        public int TypeIndex { get; set; }
        public string Data { get; set; }

        public override void ReadFrom(BinaryReader reader)
        {
            this.TypeIndex = reader.ReadInt32();
            this.Data = reader.ReadString();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write((Int32)this.TypeIndex);
            writer.Write((String)this.Data);
        }
    }
}