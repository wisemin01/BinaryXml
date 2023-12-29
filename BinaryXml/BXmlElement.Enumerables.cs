using BinaryXml.Internal;

namespace BinaryXml
{
    public readonly ref struct BXmlElementList
    {
        public ref struct Enumerator
        {
            private readonly BXmlElement _e;
            private int _index;

            public BXmlElement Current
            {
                get { return _e.InternalElement(_index); }
            }

            public Enumerator(BXmlElement e)
            {
                this._e = e;
                this._index = -1;
            }

            public bool MoveNext()
            {
                return ++_index < _e.ChildCount;
            }
        }

        private readonly BXmlElement _e;

        public int Count
        {
            get => this._e.ChildCount;
        }

        public BXmlElement this[int index]
        {
            get => this._e.InternalElement(index);
        }

        public BXmlElementList(BXmlElement e)
        {
            this._e = e;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_e);
        }
    }

    public readonly ref struct BXmlElementEnumerable
    {
        public ref struct Enumerator
        {
            private readonly BXmlElement _e;
            private readonly ReadOnlySpan<byte> _u8name;

            private int _index;
            private BXmlElement _current;

            public BXmlElement Current
            {
                get { return _current; }
            }

            public Enumerator(BXmlElement e, ReadOnlySpan<byte> u8name)
            {
                this._e = e;
                this._u8name = u8name;
            }

            public bool MoveNext()
            {
                while (_index < _e.ChildCount)
                {
                    _current = _e.InternalElement(_index);
                    ++_index;

                    if (_current.Name.SequenceEqual(_u8name))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private readonly BXmlElement _e;
        private readonly ReadOnlySpan<byte> _u8name;

        public BXmlElementEnumerable(BXmlElement e, ReadOnlySpan<byte> u8name)
        {
            this._e = e;
            this._u8name = u8name;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_e, _u8name);
        }
    }

    public readonly ref struct BXmlAttributeList
    {
        public ref struct Enumerator
        {
            private readonly BXmlElement _e;
            private int _index;

            public BXmlAttribute Current
            {
                get { return _e.InternalAttribute(_index); }
            }

            public Enumerator(BXmlElement e)
            {
                this._e = e;
                this._index = -1;
            }

            public bool MoveNext()
            {
                return ++_index < _e.AttributeCount;
            }
        }

        private readonly BXmlElement _e;

        public int Count
        {
            get => this._e.AttributeCount;
        }

        public BXmlAttribute this[int index]
        {
            get => this._e.InternalAttribute(index);
        }

        public BXmlAttributeList(BXmlElement e)
        {
            this._e = e;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_e);
        }
    }

    public readonly ref struct BXmlAttributeEnumerable
    {
        public ref struct Enumerator
        {
            private readonly BXmlElement _e;
            private readonly ReadOnlySpan<byte> _u8name;

            private int _index;
            private BXmlAttribute _current;

            public BXmlAttribute Current
            {
                get { return _current; }
            }

            public Enumerator(BXmlElement e, ReadOnlySpan<byte> u8name)
            {
                this._e = e;
                this._u8name = u8name;
            }

            public bool MoveNext()
            {
                while (_index < _e.ChildCount)
                {
                    _current = _e.InternalAttribute(_index);
                    ++_index;

                    if (_current.Name.SequenceEqual(_u8name))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private readonly BXmlElement _e;
        private readonly ReadOnlySpan<byte> _u8name;

        public BXmlAttributeEnumerable(BXmlElement e, ReadOnlySpan<byte> u8name)
        {
            this._e = e;
            this._u8name = u8name;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_e, _u8name);
        }
    }
}