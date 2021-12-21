using FileCabinetApp.Readers;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Snapshot FileCabinetService.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private ReadOnlyCollection<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Records for initializing.</param>
        public FileCabinetServiceSnapshot(ReadOnlyCollection<FileCabinetRecord> records)
        {
            this.records = records;
        }

        /// <summary>
        /// Gets records.
        /// </summary>
        public ReadOnlyCollection<FileCabinetRecord> Records => this.records;

        /// <summary>
        /// Save records to csv file.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write.</param>
        public void SaveToCsv(StreamWriter streamWriter)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(streamWriter);
            const string nameString = "Id,First Name,Last Name,Date of Birth,Height,Weight,Favorite Character";
            streamWriter.WriteLine(nameString);
            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }

        /// <summary>
        /// Save records to xml file.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write.</param>
        public void SaveToXml(StreamWriter streamWriter)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
            };
            var writer = XmlWriter.Create(streamWriter, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("records");

            var xmlWriter = new FileCabinetRecordXmlWriter(writer);
            foreach (var record in this.records)
            {
                xmlWriter.Write(record);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// load data from csv file.
        /// </summary>
        /// <param name="streamReader">file for reading.</param>
        public void LoadFromCsv(StreamReader streamReader)
        {
            var reader = new FileCabinetRecordCsvReader(streamReader);
            this.records = new ReadOnlyCollection<FileCabinetRecord>(reader.ReadAll());
        }

        /// <summary>
        /// load data from xml file.
        /// </summary>
        /// <param name="fileStream">file stream for reading.</param>
        public void LoadFromXml(FileStream fileStream)
        {
            var reader = new FileCabinetRecordXmlReader(fileStream);
            this.records = new ReadOnlyCollection<FileCabinetRecord>(reader.ReadAll());
        }
    }
}
