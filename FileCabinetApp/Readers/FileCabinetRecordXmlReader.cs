using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileCabinetApp.Readers
{
    public class FileCabinetRecordXmlReader
    {
        private readonly FileStream fileStream;

        public FileCabinetRecordXmlReader(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<FileCabinetRecord>));
            return (List<FileCabinetRecord>)xmlSerializer.Deserialize(this.fileStream);
        }
    }
}
