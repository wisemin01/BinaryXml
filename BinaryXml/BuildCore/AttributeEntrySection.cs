using BinaryXml.Internal;
using System.Buffers;

namespace BinaryXml.BuildCore
{
    internal class AttributeEntrySection : ISection
    {
        private readonly List<BXmlAttributeEntry> _entries = new List<BXmlAttributeEntry>();

        public int CurrentOffset
        {
            get => _entries.Count * BXmlAttributeEntry.Size;
        }

        public void AddEntry(BXmlAttributeEntry entry)
        {
            _entries.Add(entry);
        }

        public void WriteTo(Stream stream)
        {
            var array = ArrayPool<byte>.Shared.Rent(BXmlAttributeEntry.Size);
            try
            {
                var span = array.AsSpan(0, BXmlAttributeEntry.Size);
                for (int i = 0; i < _entries.Count; ++i)
                {
                    _entries[i].WriteTo(span);
                    stream.Write(span);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        public void Dispose()
        {
            _entries.Clear();
        }
    }
}
