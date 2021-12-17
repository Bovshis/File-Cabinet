using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Csv Writer for FileCabinetService.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write record.
        /// </summary>
        /// <param name="record">Record for write.</param>
        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteLine(record.ToString());
        }
    }
}