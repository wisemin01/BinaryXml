using BinaryXml.Internal;

namespace BinaryXml
{
    public readonly partial struct BXmlElement
    {
        public ref struct Enumerable
        {
            public ref struct Enumerator
            {
                private ref BXmlElement _e;
                private int _index;
                private BXmlElement _current;

                public BXmlElement Current { get => _current; }

                public Enumerator(BXmlElement e)
                {
                    this._e = e;
                }

                public bool MoveNext()
                {
                    while (_index++ < _e._reader.ChildCount)
                    {
                        _current = new BXmlElement(_e._document, _e._reader.ChildOffset + (_index * BXmlElementEntry.Size));
                        return true;
                    }

                    return false;
                }
            }

            private readonly BXmlElement _e;

            public Enumerable(BXmlElement e)
            {
                this._e = e;
            }

            public Enumerator GetEnumerator()
            {
                return new Enumerator(_e);
            }
        }
    }
}