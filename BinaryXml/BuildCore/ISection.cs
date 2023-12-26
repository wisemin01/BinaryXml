namespace BinaryXml.BuildCore
{
    internal interface ISection : IDisposable
    {
        public void WriteTo(Stream stream);
    }
}
