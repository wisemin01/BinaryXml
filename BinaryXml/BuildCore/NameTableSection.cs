using System.Buffers;
using System.Text;

namespace BinaryXml.BuildCore
{
    internal class NameTableSection : ISection
    {
        private readonly MemoryStream _memoryStream = new MemoryStream();
        private readonly Dictionary<string, int> _offsetCache = new Dictionary<string, int>();

        public int GetOffset(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return -1;
            }

            if (_offsetCache.TryGetValue(text, out var offset))
            {
                return offset;
            }
            else
            {
                offset = (int)_memoryStream.Position;

                var byteCount = Encoding.UTF8.GetByteCount(text);
                var array = ArrayPool<byte>.Shared.Rent(byteCount);
                try
                {
                    unsafe
                    {
                        fixed (char* chars = text)
                        fixed (byte* bytes = array)
                        {
                            Encoding.UTF8.GetBytes(chars, text.Length, bytes, byteCount);
                        }
                    }

                    BitEncodingHelper.Write7BitEncodedInt(_memoryStream, byteCount);

                    _memoryStream.Write(array);
                    _offsetCache.Add(text, offset);

                    return offset;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(array);
                }
            }
        }

        public void WriteTo(Stream stream)
        {
            _memoryStream.WriteTo(stream);
        }

        public void Dispose()
        {
            _memoryStream.Dispose();
            _offsetCache.Clear();
        }
    }
}
