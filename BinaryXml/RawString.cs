using System.Text;

namespace BinaryXml
{
    public readonly ref struct RawString
    {
        private readonly ReadOnlySpan<byte> _span;

        public RawString(ReadOnlySpan<byte> span)
        {
            _span = span;
        }

        public bool SequenceEqual(ReadOnlySpan<byte> utf8Bytes)
        {
            return _span.SequenceEqual(utf8Bytes);
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(_span);
        }
    }
}
