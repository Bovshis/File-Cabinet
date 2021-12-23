﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.Constants;
using FileCabinetApp.Enums;
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
        private FileStream fileStream;
        private int recordsAmount = 0;
        private int deletedRecordsAmount = 0;

        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">file stream to write.</param>
        /// <param name="validator">validator.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
        }

        /// <summary>
        /// Get validator.
        /// </summary>
        /// <returns>validator.</returns>
        public IRecordValidator GetValidator()
        {
            return this.validator;
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
        /// <returns>is record edited.</returns>
        public bool EditRecord(int id, RecordWithoutId recordWithoutId)
        {
            if (this.MoveCursorToRecord(id))
            {
                var byteRecord = new ByteRecord(new FileCabinetRecord(id, recordWithoutId));
                var byteWriter = new FileCabinetByteRecordWriter(this.fileStream);
                byteWriter.Write(byteRecord);
                return true;
            }

            return false;
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
                    records.Add(this.ReadRecord().ToFileCabinetRecord());
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
                    records.Add(this.ReadRecord().ToFileCabinetRecord());
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
                    records.Add(this.ReadRecord().ToFileCabinetRecord());
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
            return new ReadOnlyCollection<FileCabinetRecord>(
                this.GetByteRecords().Select(byteRecord => byteRecord.ToFileCabinetRecord()).ToArray());
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
        /// <returns>amount imported records.</returns>
        public int Restore(FileCabinetServiceSnapshot fileCabinetServiceSnapshot)
        {
            var amount = 0;
            foreach (var record in fileCabinetServiceSnapshot.Records)
            {
                if (this.validator.ValidateParameter(record).Item1)
                {
                    amount++;
                    this.AddRecord(record);
                }
                else
                {
                    Console.WriteLine($"Validation failed: {record.Id}.");
                }
            }

            this.recordsAmount += amount;
            return amount;
        }

        /// <summary>
        /// remove record.
        /// </summary>
        /// <param name="id">id removed record.</param>
        public void Remove(int id)
        {

            if (this.MoveCursorToRecord(id))
            {
                const int shortSize = 2;
                var statusBuffer = new byte[shortSize];
                this.fileStream.Read(statusBuffer, 0, statusBuffer.Length);
                if (BitConverter.ToInt16(statusBuffer) == (short)ByteRecordStatus.Deleted)
                {
                    Console.WriteLine("records has already been deleted");
                }
                else
                {
                    this.deletedRecordsAmount++;
                    this.fileStream.Seek(-1 * shortSize, SeekOrigin.Current);
                    this.fileStream.Write(BitConverter.GetBytes((short)ByteRecordStatus.Deleted));
                    Console.WriteLine($"Record #{id} is removed");
                }
            }
            else
            {
                Console.WriteLine($"Record #{id} is not found");
            }
        }

        /// <summary>
        /// Purge deleted records.
        /// </summary>
        public void Purge()
        {
            var filePath = this.fileStream.Name;
            var byteRecords = this.GetByteRecords();
            this.fileStream.Close();
            this.fileStream.Dispose();
            this.fileStream = new FileStream(filePath, FileMode.Create);
            foreach (var byteRecord in byteRecords)
            {
                if (BitConverter.ToInt16(byteRecord.Status) != (short)ByteRecordStatus.Deleted)
                {
                    this.AddRecord(byteRecord.ToFileCabinetRecord());
                }
            }

            Console.WriteLine($"Data file processing is completed: {this.deletedRecordsAmount} of {this.recordsAmount} records were purged.");
            this.recordsAmount -= this.deletedRecordsAmount;
            this.deletedRecordsAmount = 0;
        }

        private void AddRecord(FileCabinetRecord record)
        {
            var byteRecord = new ByteRecord(record);
            var byteWriter = new FileCabinetByteRecordWriter(this.fileStream);
            byteWriter.Write(byteRecord);
        }

        private ByteRecord ReadRecord()
        {
            var buffer = new byte[ByteOffsetConstants.Size];
            this.fileStream.Read(buffer, 0, buffer.Length);
            var byteRecord = new ByteRecord(buffer);
            return byteRecord;
        }

        private IEnumerable<ByteRecord> GetByteRecords()
        {
            var records = new List<ByteRecord>();

            var currentIndex = 0;
            this.fileStream.Seek(currentIndex, SeekOrigin.Begin);
            while (currentIndex < this.fileStream.Length)
            {
                records.Add(this.ReadRecord());
                currentIndex += ByteOffsetConstants.Size;
            }

            return records;
        }

        private bool MoveCursorToRecord(int id)
        {
            var currentIndex = ByteOffsetConstants.IdOffset;
            this.fileStream.Seek(currentIndex, SeekOrigin.Begin);

            const int sizeId = 4;
            while (currentIndex < this.fileStream.Length)
            {
                currentIndex += ByteOffsetConstants.Size;
                var buffer = new byte[sizeId];
                this.fileStream.Read(buffer, 0, buffer.Length);
                var currentId = BitConverter.ToInt32(buffer);
                if (currentId == id)
                {
                    this.fileStream.Seek(-1 * ByteOffsetConstants.FirstNameOffset, SeekOrigin.Current);
                    return true;
                }

                this.fileStream.Seek(ByteOffsetConstants.Size - sizeId, SeekOrigin.Current);
            }

            return false;
        }
    }
}
