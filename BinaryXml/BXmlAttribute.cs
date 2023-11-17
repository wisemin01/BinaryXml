using System.Xml.Linq;

namespace BinaryXml
{
    /// <summary>
    ///     Represents an BXML attribute.
    /// </summary>
    public readonly struct BXmlAttribute
    {
        private readonly BXmlDocument _docRef;
        private readonly int _ptr;

        internal BXmlAttribute(BXmlDocument docRef, int ptr)
        {
            this._docRef = docRef;
            this._ptr = ptr;
        }

        private BXAttributeSequenceNode GetSelfSequenceNode()
        {
            return (BXAttributeSequenceNode)_docRef.Sequence[_ptr];
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
    }
}