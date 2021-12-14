﻿using System;
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
        private readonly ReadOnlyCollection<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Records for initializing.</param>
        public FileCabinetServiceSnapshot(ReadOnlyCollection<FileCabinetRecord> records)
        {
            this.records = records;
        }

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
    }
}
