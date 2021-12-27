using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using FileCabinetApp.Records;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// File cabinet with memory storage.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">validator.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
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
        /// Create record, adds to list and dictionaries.
        /// </summary>
        /// <param name="recordWithoutId">record data.</param>
        /// <returns>record number.</returns>
        public int CreateRecord(RecordWithoutId recordWithoutId)
        {
            var record = new FileCabinetRecord(this.list.Count + 1, recordWithoutId);
            this.list.Add(record);
            return record.Id;
        }

        /// <summary>
        /// Insert record.
        /// </summary>
        /// <param name="record">record.</param>
        public void Insert(FileCabinetRecord record)
        {
            this.list.Add(record);
        }

        /// <summary>
        /// Get records from the file cabinet list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Get count records from the file cabinet list..
        /// </summary>
        /// <returns>count of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Edit record.
        /// </summary>
        /// <param name="id">number of the edited record.</param>
        /// <param name="recordWithoutId">record data.</param>
        /// <returns>is record edited.</returns>
        public bool EditRecord(int id, RecordWithoutId recordWithoutId)
        {
            var position = this.list.FindIndex(x => x.Id == id);
            if (position != -1)
            {
                this.list[position] = new FileCabinetRecord(id, recordWithoutId);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Find list of the records by first name.
        /// </summary>
        /// <param name="firstName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            foreach (var record in this.list)
            {
                if (record.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Find list of the records by last Name.
        /// </summary>
        /// <param name="lastName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            foreach (var record in this.list)
            {
                if (record.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Find list of the records by date Of Birth.
        /// </summary>
        /// <param name="dateOfBirth">value to search.</param>
        /// <returns>Searched records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            foreach (var record in this.list)
            {
                if (record.DateOfBirth == DateTime.Parse(dateOfBirth))
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Make snapshot.
        /// </summary>
        /// <returns>FileCabinetService snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords());
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
                    this.list.Add(record);
                }
                else
                {
                    Console.WriteLine($"Validation failed: {record.Id}.");
                }
            }

            return amount;
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
                this.list.RemoveAll(x => x.Id == index);
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
            return this.list
                .Where(x => indices.Contains(x.Id))
                .ToList();
        }

        private void UpdateRecord(int index, IList<(string, string)> replaceList)
        {
            var id = this.list.FindIndex(x => x.Id == index);
            if (id == -1)
            {
                throw new ArgumentException(nameof(index));
            }

            var oldRecord = this.list[id];
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

            this.list[id] = oldRecord;
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

            return indices;
        }

        private IList<int> FindIndicesWhere((string, string) param, IList<int> indices)
        {
            var (key, value) = param;
            switch (key.ToLower(CultureInfo.InvariantCulture))
            {
                case "firstname":
                    return this.list
                        .Where(x => indices.Contains(x.Id))
                        .ToList()
                        .FindAll(x => x.FirstName.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                        .Select(x => x.Id)
                        .ToList();
                case "lastname":
                    return this.list
                        .Where(x => indices.Contains(x.Id))
                        .ToList()
                        .FindAll(x => x.LastName.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                        .Select(x => x.Id)
                        .ToList();
                case "dateofbirth":
                    return this.list
                        .Where(x => indices.Contains(x.Id))
                        .ToList()
                        .FindAll(x => x.DateOfBirth == Convert.ToDateTime(value))
                        .Select(x => x.Id)
                        .ToList();
                case "height":
                    return this.list
                            .Where(x => indices.Contains(x.Id))
                        .ToList()
                        .FindAll(x => x.Height == Convert.ToInt16(value))
                        .Select(x => x.Id)
                        .ToList();
                case "weight":
                    return this.list
                        .Where(x => indices.Contains(x.Id))
                        .ToList()
                        .FindAll(x => x.Weight == Convert.ToDecimal(value))
                        .Select(x => x.Id)
                        .ToList();
                case "favoritecharacter":
                    return this.list
                        .Where(x => indices.Contains(x.Id))
                        .ToList()
                        .FindAll(x => x.FavoriteCharacter == Convert.ToChar(value))
                        .Select(x => x.Id)
                        .ToList();
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
                    return new List<int>() { Convert.ToInt32(value) };
                }
            }

            return this.list
                .Select(item => item.Id)
                .ToList();
        }
    }
}
