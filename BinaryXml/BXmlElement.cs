using BinaryXml.Internal;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace BinaryXml
{
    /// <summary>
    ///     Represents an BXML element.
    /// </summary>
    public readonly partial struct BXmlElement
    {
        private readonly BXmlDocument _document;
        private readonly BXmlElementEntry.Reader _reader;

        public bool IsNull
        {
            get => _reader.IsNullPtr;
        }

        public RawString Name
        {
            get { return new RawString(_document.GetNameSpan(_reader.NameOffset)); }
        }

        public RawString Value
        {
            get { return new RawString(_document.GetDataSpan(_reader.DataOffset)); }
        }

        public int ChildCount
        {
            get => _reader.ChildCount;
        }

        public int AttributeCount
        {
            get => _reader.AttributeCount;
        }

        internal BXmlElement(BXmlDocument document, int offset)
        {
            this._document = document;
            this._reader = document.GetElementReader(offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BXmlElement InternalElement(int index)
        {
            return new BXmlElement(_document, _reader.ChildOffset + (index * BXmlElementEntry.Size));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BXmlAttribute InternalAttribute(int index)
        {
            return new BXmlAttribute(_document, _reader.AttributeOffset + (index * BXmlAttributeEntry.Size));
        }

        public BXmlElementList Elements()
        {
            return new BXmlElementList(this);
        }

        public BXmlElementEnumerable Elements(ReadOnlySpan<byte> u8name)
        {
            return new BXmlElementEnumerable(this, u8name);
        }

        public BXmlElement Element(ReadOnlySpan<byte> u8name)
        {
            var count = _reader.ChildCount;
            for (int i = 0; i < count; ++i)
            {
                var e = InternalElement(i);
                if (e.Name.SequenceEqual(u8name))
                {
                    return e;
                }
            }
            return default;
        }

        public BXmlElement Element(string name)
        {
            // TODO - stackalloc 및 ArrayPool 사용 패턴이 여러번 사용되어 객체 or 함수화하면 좋을 것 같음.

            int byteCount = Encoding.UTF8.GetByteCount(name);
            if (byteCount <= 128)
            {
                unsafe
                {
                    var bytes = stackalloc byte[128];
                    fixed (char* chars = name)
                    {
                        Encoding.UTF8.GetBytes(chars, name.Length, bytes, byteCount);
                        return Element(new ReadOnlySpan<byte>(bytes, byteCount));
                    }
                }
            }
            else
            {
                var array = ArrayPool<byte>.Shared.Rent(byteCount);
                try
                {
                    unsafe
                    {
                        fixed (byte* bytes = array)
                        fixed (char* chars = name)
                        {
                            Encoding.UTF8.GetBytes(chars, name.Length, bytes, byteCount);
                            return Element(new ReadOnlySpan<byte>(bytes, byteCount));
                        }
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(array);
                }
            }
        }

        public BXmlAttributeList Attributes()
        {
            return new BXmlAttributeList(this);
        }

        public BXmlAttributeEnumerable Attributes(ReadOnlySpan<byte> u8name)
        {
            return new BXmlAttributeEnumerable(this, u8name);
        }

        public BXmlAttribute Attribute(ReadOnlySpan<byte> u8name)
        {
            var count = _reader.AttributeCount;
            for (int i = 0; i < count; ++i)
            {
                var e = InternalAttribute(i);
                if (e.Name.SequenceEqual(u8name))
                {
                    return e;
                }
            }
            return default;
        }

        public BXmlAttribute Attribute(string name)
        {
            // TODO - stackalloc 및 ArrayPool 사용 패턴이 여러번 사용되어 객체 or 함수화하면 좋을 것 같음.

            int byteCount = Encoding.UTF8.GetByteCount(name);
            if (byteCount <= 128)
            {
                unsafe
                {
                    var bytes = stackalloc byte[128];
                    fixed (char* chars = name)
                    {
                        Encoding.UTF8.GetBytes(chars, name.Length, bytes, byteCount);
                        return Attribute(new ReadOnlySpan<byte>(bytes, byteCount));
                    }
                }
            }
            else
            {
                var array = ArrayPool<byte>.Shared.Rent(byteCount);
                try
                {
                    unsafe
                    {
                        fixed (byte* bytes = array)
                        fixed (char* chars = name)
                        {
                            Encoding.UTF8.GetBytes(chars, name.Length, bytes, byteCount);
                            return Attribute(new ReadOnlySpan<byte>(bytes, byteCount));
                        }
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(array);
                }
            }
        }
    }
}