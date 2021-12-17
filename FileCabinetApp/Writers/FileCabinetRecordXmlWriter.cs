using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Xml Writer for FileCabinetService.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write record.
        /// </summary>
        /// <param name="record">Record for write.</param>
        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteStartElement("record");
            this.writer.WriteAttributeString("id", record.Id.ToString());

            this.writer.WriteStartElement("name");
            this.writer.WriteAttributeString("first", record.FirstName);
            this.writer.WriteAttributeString("last", record.LastName);
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("dateOfBirth");
            this.writer.WriteString(record.DateOfBirth.ToString("yyyy-MMM-dd"));
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("height");
            this.writer.WriteString(record.Height.ToString());
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("weight");
            this.writer.WriteString(record.Weight.ToString());
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("favoriteCharacter");
            this.writer.WriteString(record.FavoriteCharacter.ToString());
            this.writer.WriteEndElement();

            this.writer.WriteEndElement();
        }
    }
}
