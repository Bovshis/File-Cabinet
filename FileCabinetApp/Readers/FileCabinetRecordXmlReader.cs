using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FileCabinetApp.Records;

namespace FileCabinetApp.Readers
{
    /// <summary>
    /// Reader for importing data from xml file.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private readonly FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="fileStream">file stream for reading data.</param>
        public FileCabinetRecordXmlReader(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Get records list from file.
        /// </summary>
        /// <returns>records list.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<FileCabinetRecord>));
            return (List<FileCabinetRecord>)xmlSerializer.Deserialize(this.fileStream);
        }
    }
}
