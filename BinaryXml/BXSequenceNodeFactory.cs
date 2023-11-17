namespace BinaryXml
{
    internal static class BXSequenceNodeFactory
    {
        // Optimize
        private readonly static BXEndElementSequenceNode Cached_EndElement = new BXEndElementSequenceNode();

        public static BXSequenceNode Create(BXSequenceNodeType type)
        {
            switch (type)
            {
                case BXSequenceNodeType.Element: return new BXElementSequenceNode();
                case BXSequenceNodeType.EndElement: return Cached_EndElement;
                case BXSequenceNodeType.Attribute: return new BXAttributeSequenceNode();

                default: return null;
            }
        }
    }
}