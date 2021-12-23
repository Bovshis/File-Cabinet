﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();

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
            this.AddRecord(record);
            return record.Id;
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
                var oldRecord = this.list[position];
                this.firstNameDictionary[oldRecord.FirstName.ToUpper(CultureInfo.InvariantCulture)].Remove(oldRecord);
                this.lastNameDictionary[oldRecord.LastName.ToUpper(CultureInfo.InvariantCulture)].Remove(oldRecord);
                this.dateOfBirthDictionary[oldRecord.DateOfBirth].Remove(oldRecord);

                var record = new FileCabinetRecord(id, recordWithoutId);
                this.list[position] = record;
                this.AddElementToDictionaries(record);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Find list of the records by first name.
        /// </summary>
        /// <param name="firstName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var key = firstName.ToUpper(CultureInfo.InvariantCulture);
            if (this.firstNameDictionary.ContainsKey(key))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName.ToUpper(CultureInfo.InvariantCulture)]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Find list of the records by last Name.
        /// </summary>
        /// <param name="lastName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var key = lastName.ToUpper(CultureInfo.InvariantCulture);
            if (this.firstNameDictionary.ContainsKey(key))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[lastName.ToUpper(CultureInfo.InvariantCulture)]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Find list of the records by date Of Birth.
        /// </summary>
        /// <param name="dateOfBirth">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var dateParsed = DateTime.TryParse(dateOfBirth, out var dateOfBirthDate);
            if (dateParsed && this.dateOfBirthDictionary.ContainsKey(dateOfBirthDate))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[dateOfBirthDate]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
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
        /// remove record.
        /// </summary>
        /// <param name="id">id removed record.</param>
        public void Remove(int id)
        {
            var record = this.list.Find(x => x.Id == id);

            if (record != null)
            {
                this.list.Remove(record);
                this.RemoveElementFromDictionaries(record);
                Console.WriteLine($"Record #{id} is removed");
            }
            else
            {
                Console.WriteLine($"Record #{id} doesn't consist");
            }
        }

        private static void AddElementToDictionary<T>(T key, FileCabinetRecord record, IDictionary<T, List<FileCabinetRecord>> dictionary)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(record);
            }
            else
            {
                dictionary[key] = new List<FileCabinetRecord>() { record };
            }
        }

        private void AddRecord(FileCabinetRecord record)
        {
            this.list.Add(record);
            this.AddElementToDictionaries(record);
        }

        private void AddElementToDictionaries(FileCabinetRecord record)
        {
            AddElementToDictionary(record.FirstName.ToUpper(CultureInfo.InvariantCulture), record, this.firstNameDictionary);
            AddElementToDictionary(record.LastName.ToUpper(CultureInfo.InvariantCulture), record, this.lastNameDictionary);
            AddElementToDictionary(record.DateOfBirth, record, this.dateOfBirthDictionary);
        }

        private void RemoveElementFromDictionaries(FileCabinetRecord record)
        {
            this.firstNameDictionary[record.FirstName.ToUpper(CultureInfo.InvariantCulture)].Remove(record);
            this.lastNameDictionary[record.LastName.ToUpper(CultureInfo.InvariantCulture)].Remove(record);
            this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
        }
    }
}
