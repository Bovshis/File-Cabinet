using System;
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
            var record = CreateRecord(this.list.Count + 1, firstName, lastName, dateOfBirth, heigth, weight, favoriteCharacter);
            this.list.Add(record);
            this.AddElementToDictionaries(record);

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
            var oldRecord = this.list[id - 1];
            this.firstNameDictionary[oldRecord.FirstName.ToUpper(CultureInfo.InvariantCulture)].Remove(oldRecord);
            this.lastNameDictionary[oldRecord.LastName.ToUpper(CultureInfo.InvariantCulture)].Remove(oldRecord);
            this.dateOfBirthDictionary[oldRecord.DateOfBirth].Remove(oldRecord);

            var record = CreateRecord(id, firstName, lastName, dateOfBirth, heigth, weight, favoriteCharacter);
            this.list[id - 1] = record;
            this.AddElementToDictionaries(record);
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

        private static FileCabinetRecord CreateRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short heigth, decimal weight, char favoriteCharacter)
        {
            return new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Height = heigth,
                Weight = weight,
                FavoriteCharacter = favoriteCharacter,
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
