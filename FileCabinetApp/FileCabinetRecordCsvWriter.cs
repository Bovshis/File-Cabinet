using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Csv Writer for FileCabinetService.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private const string NameString = "Id,First Name,Last Name,Date of Birth,Height,Weight,Favorite Character";
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            writer.WriteLine(NameString);
            this.writer = writer;
        }

        /// <summary>
        /// Write record
        /// </summary>
        /// <param name="record">Record for write.</param>
        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteLine(record.ToString());
        }
    }
}
