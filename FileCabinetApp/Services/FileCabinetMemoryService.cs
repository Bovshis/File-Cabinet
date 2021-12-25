using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// remove record.
        /// </summary>
        /// <param name="id">id removed record.</param>
        public void Remove(int id)
        {
            var record = this.list.Find(x => x.Id == id);

            if (record != null)
            {
                this.list.Remove(record);
                Console.WriteLine($"Record #{id} is removed");
            }
            else
            {
                Console.WriteLine($"Record #{id} doesn't consist");
            }
        }
    }
}
