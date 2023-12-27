using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace BinaryXml.Internal
{
    /// <summary>
    ///     Binary-XML Header Structure (16 Byte)
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct BXmlHeader
    {
        public const int Size = 16;

        [FieldOffset(0)] public int NameTableSectionOffset;
        [FieldOffset(4)] public int ElementEntrySectionOffset;
        [FieldOffset(8)] public int AttributeEntrySectionOffset;
        [FieldOffset(12)] public int DataSectionOffset;

        // @NOTE:
        //  BinaryXml 은 기본적으로 Little-Endian 방식으로 저장됩니다.
        //  현재 시스템이 Big-Endian 이라면 엔디안 변환을 거친 후 파일에 저장합니다.

        public BXmlHeader ReverseEndianness()
        {
            return new BXmlHeader()
            {
                NameTableSectionOffset = BinaryPrimitives.ReverseEndianness(NameTableSectionOffset),
                ElementEntrySectionOffset = BinaryPrimitives.ReverseEndianness(ElementEntrySectionOffset),
                AttributeEntrySectionOffset = BinaryPrimitives.ReverseEndianness(AttributeEntrySectionOffset),
                DataSectionOffset = BinaryPrimitives.ReverseEndianness(DataSectionOffset),
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
                        *((BXmlHeader*)ptr) = this;
                    }
                    else
                    {
                        *((BXmlHeader*)ptr) = ReverseEndianness();
                    }
                }
            }
        }

        public static BXmlHeader ParseFrom(ReadOnlySpan<byte> src)
        {
            unsafe
            {
                fixed (byte* ptr = src)
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        return *(BXmlHeader*)(void*)ptr;
                    }
                    else
                    {
                        return (*(BXmlHeader*)(void*)ptr).ReverseEndianness();
                    }
                }
            }
        }
    }
}
