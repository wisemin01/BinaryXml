using BinaryXml.Internal;

namespace BinaryXml
{
    /// <summary>
    ///     Represents an BXML attribute.
    /// </summary>
    public readonly struct BXmlAttribute
    {
        private readonly BXmlDocument _document;
        private readonly BXmlAttributeEntry.Reader _reader;

        public bool IsNull
        {
            get => _reader.IsNullPtr;
        }

        public RawString Name
        {
            get { return new RawString(_document.GetNameSpan(_reader.NameOffset)); }
        }

        public RawString Value
        {
            get { return new RawString(_document.GetDataSpan(_reader.DataOffset)); }
        }

        internal BXmlAttribute(BXmlDocument document, int offset)
        {
            this._document = document;
            this._reader = document.GetAttributeReader(offset);
        }
    }
}