using System.IO;
using FileCabinetApp.Records;

namespace FileCabinetApp.Writers
{
    /// <summary>
    /// Byte Writer for FileCabinetService.
    /// </summary>
    public class FileCabinetByteRecordWriter
    {
        private readonly FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetByteRecordWriter"/> class.
        /// </summary>
        /// <param name="fileStream">Writer.</param>
        public FileCabinetByteRecordWriter(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Write record.
        /// </summary>
        /// <param name="record">Record for write.</param>
        public void Write(ByteRecord record)
        {
            this.fileStream.Write(record.Status);
            this.fileStream.Write(record.Id);
            this.fileStream.Write(record.FirstName);
            this.fileStream.Write(record.LastName);
            this.fileStream.Write(record.Year);
            this.fileStream.Write(record.Month);
            this.fileStream.Write(record.Day);
            this.fileStream.Write(record.Height);
            this.fileStream.Write(record.Weight);
            this.fileStream.Write(record.FavoriteCharacter);
            this.fileStream.Flush();
        }
    }
}
