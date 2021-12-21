using System.Collections.Generic;
using System.IO;
using FileCabinetApp.Records;

namespace FileCabinetApp.Readers
{
    /// <summary>
    /// Reader for importing data from csv file.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">Stream reader for reading data.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Get records list from file.
        /// </summary>
        /// <returns>records list.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();
            string line;
            while ((line = this.reader.ReadLine()) != null)
            {
                records.Add(new FileCabinetRecord(line));
            }

            return records;
        }
    }
}
