using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace BinaryXml.Internal
{
    /// <summary>
    ///     Binary-XML Attribute Entry Structure (8 Byte)
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal partial struct BXmlAttributeEntry
    {
        public const int Size = 8;

        [FieldOffset(0)] public int NameOffset;
        [FieldOffset(4)] public int DataOffset;

        public BXmlAttributeEntry ReverseEndianness()
        {
            return new BXmlAttributeEntry()
            {
                NameOffset = BinaryPrimitives.ReverseEndianness(NameOffset),
                DataOffset = BinaryPrimitives.ReverseEndianness(DataOffset),
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
                        *((BXmlAttributeEntry*)ptr) = this;
                    }
                    else
                    {
                        *((BXmlAttributeEntry*)ptr) = ReverseEndianness();
                    }
                }
            }
        }
    }
}
