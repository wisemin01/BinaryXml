using System.Buffers;
using System.Buffers.Text;
using System.Text;

namespace BinaryXml
{
    /// <summary>
    ///     Representing the position and length of a UTF-8 encoded string.
    /// </summary>
    public readonly ref struct RawString
    {
        private readonly ReadOnlySpan<byte> _span;

        public RawString(ReadOnlySpan<byte> span)
        {
            _span = span;
        }

        public bool SequenceEqual(ReadOnlySpan<byte> u8name)
        {
            return _span.SequenceEqual(u8name);
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(_span);
        }

        public bool TryToBoolean(out bool value)
        {
            return Utf8Parser.TryParse(_span, out value, out _);
        }

        public bool TryToInt(out int value)
        {
            return Utf8Parser.TryParse(_span, out value, out _);
        }

        public bool TryToLong(out long value)
        {
            return Utf8Parser.TryParse(_span, out value, out _);
        }

        public bool TryToFloat(out float value)
        {
            return Utf8Parser.TryParse(_span, out value, out _);
        }

        public bool TryToDouble(out double value)
        {
            return Utf8Parser.TryParse(_span, out value, out _);
        }

        public bool TryToEnum<TEnum>(bool ignoreCase, out TEnum value)
            where TEnum : struct
        {
            // TODO - stackalloc 및 ArrayPool 사용 패턴이 여러번 사용되어 객체 or 함수화하면 좋을 것 같음.

            int charCount = Encoding.UTF8.GetCharCount(_span);
            if (charCount <= 32)
            {
                unsafe
                {
                    var chars = stackalloc char[32];
                    fixed (byte* bytes = _span)
                    {
                        Encoding.UTF8.GetChars(bytes, _span.Length, chars, charCount);
                        return Enum.TryParse(new ReadOnlySpan<char>(chars, charCount), ignoreCase, out value);
                    }
                }
            }
            else
            {
                var array = ArrayPool<char>.Shared.Rent(charCount);
                try
                {
                    unsafe
                    {
                        fixed (byte* bytes = _span)
                        fixed (char* chars = array)
                        {
                            Encoding.UTF8.GetChars(bytes, _span.Length, chars, charCount);
                            return Enum.TryParse(new ReadOnlySpan<char>(chars, charCount), ignoreCase, out value);
                        }
                    }
                }
                finally
                {
                    ArrayPool<char>.Shared.Return(array);
                }
            }
        }

        public bool ToBoolean()
        {
            return TryToBoolean(out var value) ? value : throw new FormatException("Incorrect format.");
        }

        public long ToInt()
        {
            return TryToInt(out var value) ? value : throw new FormatException("Incorrect format.");
        }

        public long ToLong()
        {
            return TryToLong(out var value) ? value : throw new FormatException("Incorrect format.");
        }

        public float ToFloat()
        {
            return TryToFloat(out var value) ? value : throw new FormatException("Incorrect format.");
        }

        public double ToDouble()
        {
            return TryToDouble(out var value) ? value : throw new FormatException("Incorrect format.");
        }

        public TEnum ToEnum<TEnum>(bool ignoreCase)
            where TEnum : struct
        {
            return TryToEnum<TEnum>(ignoreCase, out var value) ? value : throw new FormatException("Incorrect format.");
        }
    }
}
