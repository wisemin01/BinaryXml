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

        public BXmlElement Element(ReadOnlySpan<byte> name)
        {
            var count = _reader.ChildCount;
            for (int i = 0; i < count; ++i)
            {
                var e = InternalElement(i);
                if (e.Name.SequenceEqual(name))
                {
                    return e;
                }
            }
            return default;
        }

        public BXmlElement Element(string name)
        {
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

        public BXmlAttribute Attribute(ReadOnlySpan<byte> name)
        {
            var count = _reader.AttributeCount;
            for (int i = 0; i < count; ++i)
            {
                var e = InternalAttribute(i);
                if (e.Name.SequenceEqual(name))
                {
                    return e;
                }
            }
            return default;
        }

        public BXmlAttribute Attribute(string name)
        {
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