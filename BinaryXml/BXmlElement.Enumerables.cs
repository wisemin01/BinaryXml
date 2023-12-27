using BinaryXml.Internal;

namespace BinaryXml
{
    public readonly ref struct BXmlElementList
    {
        public ref struct Enumerator
        {
            private ref BXmlElement _e;
            private int _index;

            public BXmlElement Current
            {
                get { return _e.InternalElement(_index); }
            }

            public Enumerator(BXmlElement e)
            {
                this._e = e;
            }

            public bool MoveNext()
            {
                return _index++ < _e.ChildCount;
            }
        }

        private readonly BXmlElement _e;

        public BXmlElementList(BXmlElement e)
        {
            this._e = e;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_e);
        }
    }

    public readonly ref struct BXmlAttributeList
    {
        public ref struct Enumerator
        {
            private ref BXmlElement _e;
            private int _index;

            public BXmlAttribute Current
            {
                get { return _e.InternalAttribute(_index); }
            }

            public Enumerator(BXmlElement e)
            {
                this._e = e;
            }

            public bool MoveNext()
            {
                return _index++ < _e.AttributeCount;
            }
        }

        private readonly BXmlElement _e;

        public BXmlAttributeList(BXmlElement e)
        {
            this._e = e;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_e);
        }
    }
}