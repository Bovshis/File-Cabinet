using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FileCabinetApp.Constants;
using FileCabinetApp.Records;
using FileCabinetApp.Validators;
using FileCabinetApp.Writers;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// File cabinet with file storage.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly FileStream fileStream;
        private int recordsAmount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">file stream to write.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Create record, adds to file.
        /// </summary>
        /// <param name="recordWithoutId">record data.</param>
        /// <returns>record number.</returns>
        public int CreateRecord(RecordWithoutId recordWithoutId)
        {
            this.recordsAmount++;
            this.AddRecord(new FileCabinetRecord(this.recordsAmount, recordWithoutId));
            return this.recordsAmount;
        }

        /// <summary>
        /// Edit record.
        /// </summary>
        /// <param name="id">number of the edited record.</param>
        /// <param name="recordWithoutId">record data.</param>
        public void EditRecord(int id, RecordWithoutId recordWithoutId)
        {
            long offset = ByteOffsetConstants.Size * (id - 1);
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            var byteRecord = new ByteRecord(new FileCabinetRecord(id, recordWithoutId));
            var byteWriter = new FileCabinetByteRecordWriter(this.fileStream);
            byteWriter.Write(byteRecord);
        }

        /// <summary>
        /// Find list of the records by date Of Birth.
        /// </summary>
        /// <param name="dateOfBirth">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var records = new List<FileCabinetRecord>();

            var currentIndex = ByteOffsetConstants.YearOffset;
            this.fileStream.Seek(currentIndex, SeekOrigin.Begin);

            while (currentIndex < this.fileStream.Length)
            {
                currentIndex += ByteOffsetConstants.Size;
                var buffer = new byte[ByteOffsetConstants.DateCapacity];
                this.fileStream.Read(buffer, 0, buffer.Length);
                var date = new DateTime(
                    BitConverter.ToInt32(buffer[..4]),
                    BitConverter.ToInt32(buffer[4..8]),
                    BitConverter.ToInt32(buffer[8..])).ToString("yyyy-MMM-dd");
                if (dateOfBirth.Equals(date, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek(-1 * ByteOffsetConstants.HeightOffset, SeekOrigin.Current);
                    var recordBuffer = new byte[ByteOffsetConstants.Size];
                    this.fileStream.Read(recordBuffer, 0, recordBuffer.Length);
                    var byteRecord = new ByteRecord(recordBuffer);
                    records.Add(byteRecord.ToFileCabinetRecord());
                    this.fileStream.Seek(ByteOffsetConstants.YearOffset, SeekOrigin.Current);
                }
                else
                {
                    this.fileStream.Seek(ByteOffsetConstants.Size - ByteOffsetConstants.DateCapacity, SeekOrigin.Current);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records.ToArray());
        }

        /// <summary>
        /// Find list of the records by first name.
        /// </summary>
        /// <param name="firstName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var records = new List<FileCabinetRecord>();

            var currentIndex = ByteOffsetConstants.FirstNameOffset;
            this.fileStream.Seek(currentIndex, SeekOrigin.Begin);

            while (currentIndex < this.fileStream.Length)
            {
                currentIndex += ByteOffsetConstants.Size;
                var buffer = new byte[ByteOffsetConstants.NameCapacity];
                this.fileStream.Read(buffer, 0, buffer.Length);
                if (firstName.Equals(Encoding.UTF8.GetString(buffer), StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek(-1 * ByteOffsetConstants.LastNameOffset, SeekOrigin.Current);
                    var recordBuffer = new byte[ByteOffsetConstants.Size];
                    this.fileStream.Read(recordBuffer, 0, recordBuffer.Length);
                    var byteRecord = new ByteRecord(recordBuffer);
                    records.Add(byteRecord.ToFileCabinetRecord());
                    this.fileStream.Seek(ByteOffsetConstants.FirstNameOffset, SeekOrigin.Current);
                }
                else
                {
                    this.fileStream.Seek(ByteOffsetConstants.Size - ByteOffsetConstants.FirstNameOffset, SeekOrigin.Current);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records.ToArray());
        }

        /// <summary>
        /// Find list of the records by last Name.
        /// </summary>
        /// <param name="lastName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var records = new List<FileCabinetRecord>();

            var currentIndex = ByteOffsetConstants.LastNameOffset;
            this.fileStream.Seek(currentIndex, SeekOrigin.Begin);

            while (currentIndex < this.fileStream.Length)
            {
                currentIndex += ByteOffsetConstants.Size;
                var buffer = new byte[ByteOffsetConstants.NameCapacity];
                this.fileStream.Read(buffer, 0, buffer.Length);
                if (lastName.Equals(Encoding.UTF8.GetString(buffer), StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek(-1 * ByteOffsetConstants.YearOffset, SeekOrigin.Current);
                    var recordBuffer = new byte[ByteOffsetConstants.Size];
                    this.fileStream.Read(recordBuffer, 0, recordBuffer.Length);
                    var byteRecord = new ByteRecord(recordBuffer);
                    records.Add(byteRecord.ToFileCabinetRecord());
                    this.fileStream.Seek(ByteOffsetConstants.LastNameOffset, SeekOrigin.Current);
                }
                else
                {
                    this.fileStream.Seek(ByteOffsetConstants.Size - ByteOffsetConstants.LastNameOffset, SeekOrigin.Current);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records.ToArray());
        }

        /// <summary>
        /// Get records from the file.
        /// </summary>
        /// <returns>Array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var records = new List<FileCabinetRecord>();

            var currentIndex = 0;
            this.fileStream.Seek(currentIndex, SeekOrigin.Begin);

            while (currentIndex < this.fileStream.Length)
            {
                var buffer = new byte[ByteOffsetConstants.Size];
                this.fileStream.Read(buffer, 0, buffer.Length);
                var byteRecord = new ByteRecord(buffer);
                records.Add(byteRecord.ToFileCabinetRecord());
                currentIndex += ByteOffsetConstants.Size;
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records.ToArray());
        }

        /// <summary>
        /// Get count records from the file.
        /// </summary>
        /// <returns>count of records.</returns>
        public int GetStat()
        {
            return this.recordsAmount;
        }

        /// <summary>
        /// Make snapshot.
        /// </summary>
        /// <returns>FileCabinetService snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Restore records.
        /// </summary>
        /// <param name="fileCabinetServiceSnapshot">snapshot that contains data.</param>
        /// <param name="validator">for validating data.</param>
        /// <returns>amount imported records.</returns>
        public int Restore(FileCabinetServiceSnapshot fileCabinetServiceSnapshot, IRecordValidator validator)
        {
            var amount = 0;
            foreach (var record in fileCabinetServiceSnapshot.Records)
            {
                if (validator.ValidateRecord(record))
                {
                    amount++;
                    this.AddRecord(record);
                }
                else
                {
                    Console.WriteLine($"Validation failed: {record.Id}.");
                }
            }

            return amount;
        }

        /// <summary>
        /// Add record to file.
        /// </summary>
        /// <param name="record">record for writing.</param>
        public void AddRecord(FileCabinetRecord record)
        {
            var byteRecord = new ByteRecord(record);
            var byteWriter = new FileCabinetByteRecordWriter(this.fileStream);
            byteWriter.Write(byteRecord);
        }
    }
}
