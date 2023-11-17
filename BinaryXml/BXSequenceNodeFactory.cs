namespace BinaryXml
{
    internal static class BXSequenceNodeFactory
    {
        public static BXSequenceNode Create(BXSequenceNodeHeaderFlag flag)
        {
            if ((flag & BXSequenceNodeHeaderFlag.Element) != 0)
            {
                return new BXElementSequenceNode(flag);
            }

            if ((flag & BXSequenceNodeHeaderFlag.Attribute) != 0)
            {
                return new BXAttributeSequenceNode();
            }

            return null;
        }
    }
}