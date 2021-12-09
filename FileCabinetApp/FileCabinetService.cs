﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Сlass for working with a list of records.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();

        private IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// validation rules.
        /// </summary>
        /// <param name="recordValidator">parameters validator.</param>
        public FileCabinetService(IRecordValidator recordValidator)
        {
            this.validator = recordValidator;
        }

        /// <summary>
        /// Method that creates record, adds to list and dictionaries.
        /// </summary>
        /// <param name="recordWithoutId">Person's data without id.</param>
        /// <returns>record number.</returns>
        public int CreateRecord(RecordWithoutId recordWithoutId)
        {
            this.validator.ValidateParameters(recordWithoutId);
            var record = CreateRecord(this.list.Count + 1, recordWithoutId);
            this.list.Add(record);
            this.AddElementToDictionaries(record);

            return record.Id;
        }

        /// <summary>
        /// Get records from the file cabinet list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
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
        /// Method that edit record.
        /// </summary>
        /// <param name="id">number of the edited record.</param>
        /// <param name="recordWithoutId">data of the edited record without id.</param>
        public void EditRecord(int id, RecordWithoutId recordWithoutId)
        {
            this.validator.ValidateParameters(recordWithoutId);
            var oldRecord = this.list[id - 1];
            this.firstNameDictionary[oldRecord.FirstName.ToUpper(CultureInfo.InvariantCulture)].Remove(oldRecord);
            this.lastNameDictionary[oldRecord.LastName.ToUpper(CultureInfo.InvariantCulture)].Remove(oldRecord);
            this.dateOfBirthDictionary[oldRecord.DateOfBirth].Remove(oldRecord);

            var record = CreateRecord(id, recordWithoutId);
            this.list[id - 1] = record;
            this.AddElementToDictionaries(record);
        }

        /// <summary>
        /// Find list of the records by first name.
        /// </summary>
        /// <param name="firstName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            var key = firstName.ToUpper(CultureInfo.InvariantCulture);
            if (this.firstNameDictionary.ContainsKey(key))
            {
                return this.firstNameDictionary[firstName.ToUpper(CultureInfo.InvariantCulture)].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Find list of the records by last Name.
        /// </summary>
        /// <param name="lastName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            var key = lastName.ToUpper(CultureInfo.InvariantCulture);
            if (this.firstNameDictionary.ContainsKey(key))
            {
                return this.firstNameDictionary[lastName.ToUpper(CultureInfo.InvariantCulture)].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Find list of the records by date Of Birth.
        /// </summary>
        /// <param name="dateOfBirth">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            DateTime dateOfBirthDate;
            var dateParsed = DateTime.TryParse(dateOfBirth, out dateOfBirthDate);
            if (dateParsed && this.dateOfBirthDictionary.ContainsKey(dateOfBirthDate))
            {
                return this.dateOfBirthDictionary[dateOfBirthDate].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        private static void AddElementToDictionary<T>(T key, FileCabinetRecord record, Dictionary<T, List<FileCabinetRecord>> dictionary)
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

        private static FileCabinetRecord CreateRecord(int id, RecordWithoutId recordWithoutId)
        {
            return new FileCabinetRecord
            {
                Id = id,
                FirstName = recordWithoutId.FirstName,
                LastName = recordWithoutId.LastName,
                DateOfBirth = recordWithoutId.DateOfBirth,
                Height = recordWithoutId.Height,
                Weight = recordWithoutId.Weight,
                FavoriteCharacter = recordWithoutId.FavoriteCharacter,
            };
        }

        private void AddElementToDictionaries(FileCabinetRecord record)
        {
            AddElementToDictionary(record.FirstName.ToUpper(CultureInfo.InvariantCulture), record, this.firstNameDictionary);
            AddElementToDictionary(record.LastName.ToUpper(CultureInfo.InvariantCulture), record, this.lastNameDictionary);
            AddElementToDictionary(record.DateOfBirth, record, this.dateOfBirthDictionary);
        }
    }
}
