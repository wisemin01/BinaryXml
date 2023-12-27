using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace BinaryXml.Internal
{
    internal static class RuntimeReadHelper
    {
        // @NOTE: 파일에는 Little-Endian 으로 저장합니다.
        //   시스템이 Big-Endian 이라면 변환하여 읽습니다.

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Read(int v)
        {
            if (BitConverter.IsLittleEndian)
            {
                return v;
            }

            return BinaryPrimitives.ReverseEndianness(v);
        }
    }
}
