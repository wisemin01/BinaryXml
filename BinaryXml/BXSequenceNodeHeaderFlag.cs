namespace BinaryXml
{
    public enum BXSequenceNodeHeaderFlag : byte
    {
        Element = 1,
        Attribute = 2,
        HasChild = 4,
        HasAttribute = 8
        // (16)
        // (32)
        // (64)
        // (128)
    }
}