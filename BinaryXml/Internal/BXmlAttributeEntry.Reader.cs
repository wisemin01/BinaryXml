using System.Runtime.CompilerServices;

namespace BinaryXml.Internal
{
    internal partial struct BXmlAttributeEntry
    {
        // @NOTE:
        //  Reader는 해당 Entry에 대한 포인터 객체입니다.
        //  포인터로 유지하는 이유:
        //    1. 특정 필드를 읽어오는데 전체를 스택에 복사할 필요는 없기 때문에.
        //    2. Endian 변환을 내부에서 처리하기 위해.
        internal readonly struct Reader
        {
            private readonly nint _ptr;

            public bool IsNullPtr => _ptr == IntPtr.Zero;

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