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

        // @NOTE:
        //  BinaryXml 은 기본적으로 Little-Endian 방식으로 저장됩니다.
        //  현재 시스템이 Big-Endian 이라면 엔디안 변환을 거친 후 파일에 저장합니다.

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
