namespace BinaryXml
{
    /// <summary>
    ///     Represents an BXML element.
    /// </summary>
    public readonly struct BXmlElement
    {
        private readonly BXmlDocument _docRef;
        private readonly int _ptr;

        internal BXmlElement(BXmlDocument docRef, int ptr)
        {
            this._docRef = docRef;
            this._ptr = ptr;
        }

        private BXElementSequenceNode GetSelfSequenceNode()
        {
            return (BXElementSequenceNode)_docRef.Sequence[_ptr];
        }

        public string Name
        {
            get
            {
                return _docRef.Indexer.GetString(TypePtr);
            }
        }

        internal int TypePtr
        {
            get
            {
                var sequenceNode = GetSelfSequenceNode();
                return sequenceNode.TypePtr;
            }
        }

        public string Value
        {
            get
            {
                var sequenceNode = GetSelfSequenceNode();
                return sequenceNode.Data;
            }
        }

        public BXmlElement? GetChild(int index)
        {
            var sequenceNode = GetSelfSequenceNode();
            if (index < 0 || index >= sequenceNode.ChildCount)
            {
                return null;
            }

            return new BXmlElement(_docRef, sequenceNode.ChildPtr + index);
        }

        public IEnumerable<BXmlElement> GetChildren()
        {
            var sequenceNode = GetSelfSequenceNode();
            for (int i = 0; i < sequenceNode.ChildCount; ++i)
            {
                yield return GetChild(i).Value;
            }
        }

        public IEnumerable<BXmlElement> GetChildren(string name)
        {
            var sequenceNode = GetSelfSequenceNode();
            var typePtr = _docRef.Indexer.GetIndex(name);

            for (int i = 0; i < sequenceNode.ChildCount; ++i)
            {
                var e = GetChild(i).Value;
                if (e.TypePtr == typePtr)
                {
                    yield return e;
                }
            }
        }

        public BXmlElement? GetChild(string name)
        {
            var sequenceNode = GetSelfSequenceNode();
            var typePtr = _docRef.Indexer.GetIndex(name);

            for (int i = 0; i < sequenceNode.ChildCount; ++i)
            {
                var e = GetChild(i).Value;
                if (e.TypePtr == typePtr)
                {
                    return e;
                }
            }
            return null;
        }

        public IEnumerable<BXmlAttribute> GetAttributes()
        {
            var sequenceNode = GetSelfSequenceNode();
            var ptr = sequenceNode.AttributePtr;
            for (int i = 0; i < sequenceNode.AttributeCount; ++i)
            {
                yield return new BXmlAttribute(_docRef, ptr + i);
            }
        }

        public IEnumerable<BXmlAttribute> GetAttributes(string name)
        {
            var sequenceNode = GetSelfSequenceNode();
            var ptr = sequenceNode.AttributePtr;

            var typePtr = _docRef.Indexer.GetIndex(name);

            for (int i = 0; i < sequenceNode.AttributeCount; ++i)
            {
                var e = new BXmlAttribute(_docRef, ptr + i);
                if (e.TypePtr == typePtr)
                {
                    yield return e;
                }
            }
        }

        public BXmlAttribute? GetAttribute(string name)
        {
            var sequenceNode = GetSelfSequenceNode();
            var ptr = sequenceNode.AttributePtr;

            var typePtr = _docRef.Indexer.GetIndex(name);

            for (int i = 0; i < sequenceNode.AttributeCount; ++i)
            {
                var e = new BXmlAttribute(_docRef, ptr + i);
                if (e.TypePtr == typePtr)
                {
                    return e;
                }
            }

            return null;
        }
    }
}