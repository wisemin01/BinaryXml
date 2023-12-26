using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace BinaryXml.Internal
{
    /// <summary>
    ///     Binary-XML Element Entry Structure (24 Byte)
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal partial struct BXmlElementEntry
    {
        public const int Size = 24;

        [FieldOffset(0)] public int NameOffset;
        [FieldOffset(4)] public int DataOffset;
        [FieldOffset(8)] public int ChildOffset;
        [FieldOffset(12)] public int ChildCount;
        [FieldOffset(16)] public int AttributeOffset;
        [FieldOffset(20)] public int AttributeCount;

        public BXmlElementEntry ReverseEndianness()
        {
            return new BXmlElementEntry()
            {
                NameOffset = BinaryPrimitives.ReverseEndianness(NameOffset),
                DataOffset = BinaryPrimitives.ReverseEndianness(DataOffset),
                ChildOffset = BinaryPrimitives.ReverseEndianness(ChildOffset),
                ChildCount = BinaryPrimitives.ReverseEndianness(ChildCount),
                AttributeOffset = BinaryPrimitives.ReverseEndianness(AttributeOffset),
                AttributeCount = BinaryPrimitives.ReverseEndianness(AttributeCount),
            };
        }

        public void WriteTo(Span<byte> dest)
        {
            unsafe
            {
                fixed (byte* ptr = dest)
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        *((BXmlElementEntry*)ptr) = this;
                    }
                    else
                    {
                        *((BXmlElementEntry*)ptr) = ReverseEndianness();
                    }
                }
            }
        }
    }
}
