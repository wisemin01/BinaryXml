using BinaryXml.Internal;
using System.Buffers;
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

        public RawString Name
        {
            get { return new RawString(_document.GetNameSpan(_reader.NameOffset)); }
        }

        public RawString Value
        {
            get { return new RawString(_document.GetDataSpan(_reader.DataOffset)); }
        }

        internal BXmlElement(BXmlDocument document, int offset)
        {
            this._document = document;
            this._reader = document.GetElementReader(offset);
        }

        public IEnumerable<BXmlElement> Elements()
        {
            var count = _reader.ChildCount;
            for (int i = 0; i < count; ++i)
            {
                var e = new BXmlElement(_document, _reader.ChildOffset + (i * BXmlElementEntry.Size));
                yield return e;
            }
        }

        public BXmlElement Element(ReadOnlySpan<byte> utf8name)
        {
            var count = _reader.ChildCount;
            for (int i = 0; i < count; ++i)
            {
                var e = new BXmlElement(_document, _reader.ChildOffset + (i * BXmlElementEntry.Size));
                if (e.Name.SequenceEqual(utf8name))
                {
                    return e;
                }
            }
            throw new Exception();
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
    }
}