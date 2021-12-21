using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp.Readers
{
    public class FileCabinetRecordCsvReader
    {
        private readonly StreamReader reader;

        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

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
