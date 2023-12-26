using BinaryXml.Internal;
using System.Buffers;

namespace BinaryXml.BuildCore
{
    internal class ElementEntrySection : ISection
    {
        private readonly List<BXmlElementEntry> _entries = new List<BXmlElementEntry>();

        public int CurrentOffset
        {
            get => _entries.Count * BXmlElementEntry.Size;
        }

        public void ReserveForRoot()
        {
            _entries.Add(default);
        }

        public void AddEntry(BXmlElementEntry entry)
        {
            _entries.Add(entry);
        }

        public void AddRootEntry(BXmlElementEntry rootEntry)
        {
            _entries[0] = rootEntry;
        }

        public void WriteTo(Stream stream)
        {
            var array = ArrayPool<byte>.Shared.Rent(BXmlElementEntry.Size);
            try
            {
                var span = array.AsSpan(0, BXmlElementEntry.Size);
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
