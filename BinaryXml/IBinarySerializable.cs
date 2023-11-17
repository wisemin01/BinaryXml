namespace BinaryXml
{
    public interface IBinarySerializable
    {
        void WriteTo(BinaryWriter writer);
        void ReadFrom(BinaryReader reader);
    }
}