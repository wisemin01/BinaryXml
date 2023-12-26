using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace BinaryXml.Internal
{
    internal static class RuntimeReadHelper
    {
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
