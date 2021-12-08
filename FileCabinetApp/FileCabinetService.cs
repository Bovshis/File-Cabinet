﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short heigth, decimal weight, char favoriteCharacter)
        {
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Height = heigth,
                Weight = weight,
                FavoriteCharacter = favoriteCharacter,
            };

            this.list.Add(record);
            AddElementToDictionary(firstName.ToUpper(CultureInfo.InvariantCulture), record, this.firstNameDictionary);
            AddElementToDictionary(lastName.ToUpper(CultureInfo.InvariantCulture), record, this.lastNameDictionary);
            AddElementToDictionary(dateOfBirth, record, this.dateOfBirthDictionary);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short heigth, decimal weight, char favoriteCharacter)
        {
            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Height = heigth,
                Weight = weight,
                FavoriteCharacter = favoriteCharacter,
            };
            var oldRecord = this.list[id - 1];
            this.list[id - 1] = record;

            this.firstNameDictionary[oldRecord.FirstName.ToUpper(CultureInfo.InvariantCulture)].Remove(oldRecord);
            AddElementToDictionary(firstName.ToUpper(CultureInfo.InvariantCulture), record, this.firstNameDictionary);

            this.lastNameDictionary[oldRecord.LastName.ToUpper(CultureInfo.InvariantCulture)].Remove(oldRecord);
            AddElementToDictionary(lastName.ToUpper(CultureInfo.InvariantCulture), record, this.lastNameDictionary);

            this.dateOfBirthDictionary[oldRecord.DateOfBirth].Remove(oldRecord);
            AddElementToDictionary(dateOfBirth, record, this.dateOfBirthDictionary);
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            var key = firstName.ToUpper(CultureInfo.InvariantCulture);
            if (this.firstNameDictionary.ContainsKey(key))
            {
                return this.firstNameDictionary[firstName.ToUpper(CultureInfo.InvariantCulture)].ToArray();
            }

            return null;
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            var key = lastName.ToUpper(CultureInfo.InvariantCulture);
            if (this.firstNameDictionary.ContainsKey(key))
            {
                return this.firstNameDictionary[lastName.ToUpper(CultureInfo.InvariantCulture)].ToArray();
            }

            return null;
        }

        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            DateTime dateOfBirthDate;
            var dateParsed = DateTime.TryParse(dateOfBirth, out dateOfBirthDate);
            if (dateParsed && this.dateOfBirthDictionary.ContainsKey(dateOfBirthDate))
            {
                return this.dateOfBirthDictionary[dateOfBirthDate].ToArray();
            }

            return null;
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
    }
}
