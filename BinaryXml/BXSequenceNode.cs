namespace BinaryXml
{
    // Base class.
    internal abstract class BXSequenceNode : IBinarySerializable
    {
        public abstract BXSequenceNodeHeaderFlag Flag { get; }

        public abstract void ReadFrom(BinaryReader reader);
        public abstract void WriteTo(BinaryWriter writer);
    }

    // Element
    internal sealed class BXElementSequenceNode : BXSequenceNode
    {
        private BXSequenceNodeHeaderFlag _flag;
        private int _childPtr = -1;
        private int _attributePtr = -1;

        public override BXSequenceNodeHeaderFlag Flag { get => _flag; }

        public int TypePtr { get; set; } = -1;
        public string Data { get; set; }
        public int ChildCount { get; set; }
        public int AttributeCount { get; set; }
        public int ChildPtr
        {
            get => _childPtr;
            internal set
            {
                _childPtr = value;
                if (_childPtr >= 0)
                {
                    _flag |= BXSequenceNodeHeaderFlag.HasChild;
                }
            }
        }
        public int AttributePtr
        {
            get { return _attributePtr; }
            internal set
            {
                _attributePtr = value;
                if (_attributePtr >= 0)
                {
                    _flag |= BXSequenceNodeHeaderFlag.HasAttribute;
                }
            }
        }
        public bool HasChild
        {
            get => (_flag & BXSequenceNodeHeaderFlag.HasChild) != 0;
        }
        public bool HasAttribute
        {
            get => (_flag & BXSequenceNodeHeaderFlag.HasAttribute) != 0;
        }

        public BXElementSequenceNode() : this(BXSequenceNodeHeaderFlag.Element)
        {
        }

        public BXElementSequenceNode(BXSequenceNodeHeaderFlag flag)
        {
            this._flag = flag;
        }

        public override void ReadFrom(BinaryReader reader)
        {
            this.TypePtr = reader.ReadInt32();
            this.Data = reader.ReadString();
            if (HasChild)
            {
                this._childPtr = reader.ReadInt32();
                this.ChildCount = reader.ReadInt32();
            }
            if (HasAttribute)
            {
                this._attributePtr = reader.ReadInt32();
                this.AttributeCount = reader.ReadInt32();
            }
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write((Int32)this.TypePtr);
            writer.Write((String)(this.Data ?? string.Empty));
            if (HasChild)
            {
                writer.Write((Int32)_childPtr);
                writer.Write((Int32)ChildCount);
            }
            if (HasAttribute)
            {
                writer.Write((Int32)_attributePtr);
                writer.Write((Int32)AttributeCount);
            }
        }
    }

    // Attribute
    internal sealed class BXAttributeSequenceNode : BXSequenceNode
    {
        public override BXSequenceNodeHeaderFlag Flag { get => BXSequenceNodeHeaderFlag.Attribute; }

        public int TypePtr { get; set; } = -1;
        public string Data { get; set; }

        public override void ReadFrom(BinaryReader reader)
        {
            this.TypePtr = reader.ReadInt32();
            this.Data = reader.ReadString();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            writer.Write((Int32)this.TypePtr);
            writer.Write((String)this.Data);
        }
    }
}