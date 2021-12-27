using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        private readonly IRecordValidator validator;

        private SortedList<int, int> recordsOffsetList;
        private FileStream fileStream;
        private int recordsAmount = 0;
        private int deletedRecordsAmount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">file stream to write.</param>
        /// <param name="validator">validator.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
            this.recordsOffsetList = new SortedList<int, int>();
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
            this.WriteRecord(new FileCabinetRecord(this.recordsAmount, recordWithoutId));
            return this.recordsAmount;
        }

        /// <summary>
        /// Insert record.
        /// </summary>
        /// <param name="record">record.</param>
        public void Insert(FileCabinetRecord record)
        {
            this.recordsAmount++;
            this.WriteRecord(record);
        }

        /// <summary>
        /// Get records from the file.
        /// </summary>
        /// <returns>Array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.GetRecords(this.GetAllIndices()).ToArray());
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
            var oldRecordsAmount = this.recordsAmount;
            foreach (var record in fileCabinetServiceSnapshot.Records)
            {
                if (this.validator.ValidateParameter(record).Item1)
                {
                    this.recordsAmount++;
                    this.WriteRecord(record);
                }
                else
                {
                    Console.WriteLine($"Validation failed: {record.Id}.");
                }
            }

            return this.recordsAmount - oldRecordsAmount;
        }

        /// <summary>
        /// delete record with given parameter.
        /// </summary>
        /// <param name="where">parameters for deleting.</param>
        /// <returns>List of deleted records.</returns>
        public IList<int> Delete(params (string key, string value)[] where)
        {
            var indices = this.GetIndicesWhere(where);
            foreach (var index in indices)
            {
                this.Remove(index);
            }

            return indices;
        }

        /// <summary>
        /// Update records.
        /// </summary>
        /// <param name="replaceList">List with data for updating record.</param>
        /// <param name="whereList">List with data for finding records.</param>
        /// <returns>list of indices updated records.</returns>
        public IList<int> Update(IList<(string, string)> replaceList, IList<(string, string)> whereList)
        {
            var indices = this.GetIndicesWhere(whereList.ToArray());
            foreach (var index in indices)
            {
                this.UpdateRecord(index, replaceList);
            }

            return indices;
        }

        /// <summary>
        /// Get records with parameters that contain in whereList.
        /// </summary>
        /// <param name="whereList">contain parameters for searching.</param>
        /// <returns>List of records.</returns>
        public IList<FileCabinetRecord> GetRecordsWhere(IList<(string, string)> whereList)
        {
            var indices = this.GetIndicesWhere(whereList.ToArray());
            return indices.Select(this.ReadRecord).ToList();
        }

        /// <summary>
        /// Purge deleted records.
        /// </summary>
        /// <returns>Amount deleted records.</returns>
        public int Purge()
        {
            if (this.deletedRecordsAmount > 0)
            {
                var filePath = this.fileStream.Name;
                var byteRecords = this.GetByteRecords();

                this.fileStream.Close();
                this.fileStream.Dispose();
                this.recordsOffsetList = new SortedList<int, int>();
                this.recordsAmount = 0;

                this.fileStream = new FileStream(filePath, FileMode.Create);

                foreach (var byteRecord in byteRecords)
                {
                    if (BitConverter.ToInt16(byteRecord.Status) != (short)ByteRecordStatus.Deleted)
                    {
                        this.recordsAmount++;
                        this.WriteRecord(byteRecord.ToFileCabinetRecord());
                    }
                }

                Console.WriteLine($"Data file processing is completed: {this.deletedRecordsAmount} of {this.recordsAmount} records were purged.");
                this.deletedRecordsAmount = 0;
            }

            return 0;
        }

        private void UpdateRecord(int index, IList<(string, string)> replaceList)
        {
            var oldRecord = this.ReadRecord(index);
            foreach (var (key, value) in replaceList)
            {
                switch (key.ToLower(CultureInfo.InvariantCulture))
                {
                    case "firstname":
                        oldRecord.FirstName = value;
                        break;
                    case "lastname":
                        oldRecord.LastName = value;
                        break;
                    case "dateofbirth":
                        oldRecord.DateOfBirth = Convert.ToDateTime(value);
                        break;
                    case "height":
                        oldRecord.Height = Convert.ToInt16(value);
                        break;
                    case "weight":
                        oldRecord.Weight = Convert.ToChar(value);
                        break;
                    case "favoritecharacter":
                        oldRecord.FavoriteCharacter = Convert.ToChar(value);
                        break;
                    default:
                        throw new ArgumentException($"There is no key: {key}");
                }
            }

            this.WriteRecord(oldRecord);
        }

        private IList<int> GetIndicesWhere(params (string key, string value)[] where)
        {
            var indices = this.GetAllIndices(where);
            foreach (var parameter in where)
            {
                if (parameter.key.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                indices = this.FindIndicesWhere(parameter, indices);
            }

            return indices.ToList();
        }

        private IList<int> FindIndicesWhere((string, string) param, IList<int> indices)
        {
            var (key, value) = param;
            switch (key.ToLower(CultureInfo.InvariantCulture))
            {
                case "firstname":
                    return this.FindWhereFirstName(value, indices).ToList();
                case "lastname":
                    return this.FindWhereLastName(value, indices).ToList();
                case "dateofbirth":
                    return this.FindWhereDateOfBirth(value, indices).ToList();
                case "height":
                case "weight":
                case "favoritecharacter":
                    throw new ArgumentException($"Not Implemented for FilesystemService");
                default:
                    throw new ArgumentException($"There is no key: {key}");
            }
        }

        private IList<int> GetAllIndices(params (string key, string value)[] where)
        {
            foreach (var (key, value) in where)
            {
                if (key.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                {
                    return new List<int> { Convert.ToInt32(value) };
                }
            }

            return this.recordsOffsetList.Keys;
        }

        private void Remove(int id)
        {
            if (!this.MoveCursorToRecord(id))
            {
                return;
            }

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
                this.recordsOffsetList.Remove(id);
                this.fileStream.Seek(-1 * shortSize, SeekOrigin.Current);
                this.fileStream.Write(BitConverter.GetBytes((short)ByteRecordStatus.Deleted));
            }
        }

        private void WriteRecord(FileCabinetRecord record)
        {
            if (!this.recordsOffsetList.ContainsKey(record.Id))
            {
                this.recordsOffsetList.Add(record.Id, ByteOffsetConstants.Size * (this.recordsAmount - 1));
            }

            this.MoveCursorToRecord(record.Id);
            var byteRecord = new ByteRecord(record);
            var byteWriter = new FileCabinetByteRecordWriter(this.fileStream);
            byteWriter.Write(byteRecord);
        }

        private ByteRecord ReadByteRecord()
        {
            var buffer = new byte[ByteOffsetConstants.Size];
            this.fileStream.Read(buffer, 0, buffer.Length);
            var byteRecord = new ByteRecord(buffer);
            return byteRecord;
        }

        private FileCabinetRecord ReadRecord(int index)
        {
            this.fileStream.Seek(this.recordsOffsetList[index], SeekOrigin.Begin);
            var buffer = new byte[ByteOffsetConstants.Size];
            this.fileStream.Read(buffer, 0, buffer.Length);
            return new ByteRecord(buffer).ToFileCabinetRecord();
        }

        private IEnumerable<ByteRecord> GetByteRecords()
        {
            var records = new List<ByteRecord>();

            var currentIndex = 0;
            this.fileStream.Seek(currentIndex, SeekOrigin.Begin);
            while (currentIndex < this.fileStream.Length)
            {
                records.Add(this.ReadByteRecord());
                currentIndex += ByteOffsetConstants.Size;
            }

            return records;
        }

        private IEnumerable<FileCabinetRecord> GetRecords(IEnumerable<int> indices)
        {
            return indices.Select(this.ReadRecord);
        }

        private bool MoveCursorToRecord(int id)
        {
            if (this.recordsOffsetList.ContainsKey(id))
            {
                this.fileStream.Seek(this.recordsOffsetList[id], SeekOrigin.Begin);
                return true;
            }

            return false;
        }

        private IEnumerable<int> FindWhereDateOfBirth(string dateOfBirth, IList<int> indices)
        {
            foreach (var index in indices)
            {
                this.fileStream.Seek(this.recordsOffsetList[index] + ByteOffsetConstants.YearOffset, SeekOrigin.Begin);
                var buffer = new byte[ByteOffsetConstants.DateCapacity];
                this.fileStream.Read(buffer, 0, buffer.Length);
                var date = new DateTime(
                    BitConverter.ToInt32(buffer[..4]),
                    BitConverter.ToInt32(buffer[4..8]),
                    BitConverter.ToInt32(buffer[8..]));
                if (date == DateTime.Parse(dateOfBirth))
                {
                    yield return index;
                }
            }
        }

        private IEnumerable<int> FindWhereFirstName(string firstName, IList<int> indices)
        {
            foreach (var index in indices)
            {
                this.fileStream.Seek(this.recordsOffsetList[index] + ByteOffsetConstants.FirstNameOffset, SeekOrigin.Begin);
                var buffer = new byte[ByteOffsetConstants.NameCapacity];
                this.fileStream.Read(buffer, 0, buffer.Length);
                if (firstName.Equals(Encoding.UTF8.GetString(buffer), StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return index;
                }
            }
        }

        private IEnumerable<int> FindWhereLastName(string lastName, IList<int> indices)
        {
            foreach (var index in indices)
            {
                this.fileStream.Seek(this.recordsOffsetList[index] + ByteOffsetConstants.LastNameOffset, SeekOrigin.Begin);
                var buffer = new byte[ByteOffsetConstants.NameCapacity];
                this.fileStream.Read(buffer, 0, buffer.Length);
                if (lastName.Equals(Encoding.UTF8.GetString(buffer), StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return index;
                }
            }
        }
    }
}
