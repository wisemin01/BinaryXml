using System.Runtime.CompilerServices;

namespace BinaryXml.Internal
{
    internal partial struct BXmlAttributeEntry
    {
        internal readonly struct Reader
        {
            private readonly nint _ptr;

            public Reader(nint ptr)
            {
                _ptr = ptr;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private unsafe BXmlAttributeEntry* As()
            {
                return (BXmlAttributeEntry*)(void*)_ptr;
            }

            public unsafe int NameOffset
            {
                get { return RuntimeReadHelper.Read(As()->NameOffset); }
            }

            public unsafe int DataOffset
            {
                get { return RuntimeReadHelper.Read(As()->DataOffset); }
            }
        }
    }
}