using BinaryXml.Internal;
using System.Buffers;

namespace BinaryXml.BuildCore
{
    internal class HeaderSection : ISection
    {
        private int _nameTableOffset;
        private int _elementEntryOffset;
        private int _attributeEntryOffset;
        private int _dataOffset;

        public void MarkNameTable(int offset)
        {
            _nameTableOffset = offset;
        }

        public void MarkElementEntry(int offset)
        {
            _elementEntryOffset = offset;
        }

        public void MarkAttributeEntry(int offset)
        {
            _attributeEntryOffset = offset;
        }

        public void MarkDataOffset(int offset)
        {
            _dataOffset = offset;
        }

        public void WriteTo(Stream stream)
        {
            var array = ArrayPool<byte>.Shared.Rent(BXmlHeader.Size);
            try
            {
                var header = new BXmlHeader()
                {
                    NameTableSectionOffset = _nameTableOffset,
                    ElementEntrySectionOffset = _elementEntryOffset,
                    AttributeEntrySectionOffset = _attributeEntryOffset,
                    DataSectionOffset = _dataOffset
                };

                var span = array.AsSpan(0, BXmlHeader.Size);

                header.WriteTo(span);
                stream.Write(span);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        public void Dispose()
        {
        }
    }
}
