using BinaryXml.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml;

namespace BinaryXml
{
    internal readonly struct SectionSegment
    {
        private readonly nint _ptr;
        private readonly int _length;

        internal nint Ptr
        {
            get { return _ptr; }
        }

        internal int Length
        {
            get { return _length; }
        }

        public SectionSegment(nint ptr, int length)
        {
            this._ptr = ptr;
            this._length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ReadOnlySpan<byte> AsSpan()
        {
            return new ReadOnlySpan<byte>((void*)_ptr, _length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ReadOnlySpan<byte> AsSpan(int offset)
        {
            return new ReadOnlySpan<byte>((void*)(_ptr + offset), _length - offset);
        }
    }

    /// <summary>
    ///     Represents an BXML document.
    /// </summary>
    public sealed class BXmlDocument : IDisposable
    {
        private readonly byte[] _data;
        private readonly GCHandle _ptr;

        private readonly SectionSegment _nameTableSection;
        private readonly SectionSegment _elementEntrySection;
        private readonly SectionSegment _attributeEntrySection;
        private readonly SectionSegment _dataSection;

        public BXmlElement Root
        {
            get { return new BXmlElement(this, 0); }
        }

        internal BXmlDocument(byte[] data)
        {
            _data = data;
            _ptr = GCHandle.Alloc(data, GCHandleType.Pinned);

            unsafe
            {
                var ptr = _ptr.AddrOfPinnedObject();
                var header = BXmlHeader.ParseFrom(new ReadOnlySpan<byte>((void*)ptr, BXmlHeader.Size));

                var nameTableLength = header.ElementEntrySectionOffset - header.NameTableSectionOffset;
                var elementEntryLength = header.AttributeEntrySectionOffset - header.ElementEntrySectionOffset;
                var attributeEntryLength = header.DataSectionOffset - header.AttributeEntrySectionOffset;
                var dataLength = _data.Length - header.DataSectionOffset;

                _nameTableSection = new SectionSegment(ptr + header.NameTableSectionOffset, nameTableLength);
                _elementEntrySection = new SectionSegment(ptr + header.ElementEntrySectionOffset, elementEntryLength);
                _attributeEntrySection = new SectionSegment(ptr + header.AttributeEntrySectionOffset, attributeEntryLength);
                _dataSection = new SectionSegment(ptr + header.DataSectionOffset, dataLength);
            }
        }

        ~BXmlDocument()
        {
            if (_ptr.IsAllocated)
            {
                _ptr.Free();
            }
        }

        internal unsafe BXmlElementEntry.Reader GetElementReader(int offset)
        {
            return new BXmlElementEntry.Reader(_elementEntrySection.Ptr + offset);
        }

        internal unsafe BXmlAttributeEntry.Reader GetAttributeReader(int offset)
        {
            return new BXmlAttributeEntry.Reader(_attributeEntrySection.Ptr + offset);
        }

        internal unsafe ReadOnlySpan<byte> GetNameSpan(int offset)
        {
            if (offset < 0)
            {
                return ReadOnlySpan<byte>.Empty;
            }

            var span = _nameTableSection.AsSpan(offset);
            var length = BitEncodingHelper.Read7BitEncodedInt(span, out var consumed);

            return span.Slice(consumed, length);
        }

        internal unsafe ReadOnlySpan<byte> GetDataSpan(int offset)
        {
            if (offset < 0)
            {
                return ReadOnlySpan<byte>.Empty;
            }

            var span = _dataSection.AsSpan(offset);
            var length = BitEncodingHelper.Read7BitEncodedInt(span, out var consumed);

            return span.Slice(consumed, length);
        }

        public static BXmlDocument Load(byte[] data)
        {
            return new BXmlDocument(data);
        }

        public static BXmlDocument LoadFromFile(string path)
        {
            return new BXmlDocument(File.ReadAllBytes(path));
        }

        public static BXmlDocument LoadFromXmlFile(string path)
        {
            using (var fs = File.OpenRead(path))
            using (var xmlReader = XmlReader.Create(fs))
            {
                return BuildCore.Converter.Convert(xmlReader);
            }
        }

        public void Dispose()
        {
            if (_ptr.IsAllocated)
            {
                _ptr.Free();
                GC.SuppressFinalize(this);
            }
        }

        public void Save(string path)
        {
            File.WriteAllBytes(path, _data);
        }
    }
}