using System.Runtime.CompilerServices;

namespace BinaryXml.Internal
{
    internal partial struct BXmlElementEntry
    {
        public readonly struct Reader
        {
            private readonly nint _ptr;

            public Reader(nint ptr)
            {
                _ptr = ptr;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private unsafe BXmlElementEntry* As()
            {
                return (BXmlElementEntry*)(void*)_ptr;
            }

            public unsafe int NameOffset
            {
                get { return RuntimeReadHelper.Read(As()->NameOffset); }
            }

            public unsafe int DataOffset
            {
                get { return RuntimeReadHelper.Read(As()->DataOffset); }
            }

            public unsafe int ChildOffset
            {
                get { return RuntimeReadHelper.Read(As()->ChildOffset); }
            }

            public unsafe int ChildCount
            {
                get { return RuntimeReadHelper.Read(As()->ChildCount); }
            }

            public unsafe int AttributeOffset
            {
                get { return RuntimeReadHelper.Read(As()->AttributeOffset); }
            }

            public unsafe int AttributeCount
            {
                get { return RuntimeReadHelper.Read(As()->AttributeCount); }
            }
        }
    }
}
