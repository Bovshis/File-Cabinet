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
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.firstNameDictionary[firstName.ToUpper(CultureInfo.InvariantCulture)].ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            var res = from p in this.list
                      where p.LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase)
                      select p;
            return res.ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            var res = from p in this.list
                      where p.DateOfBirth.ToString("yyyy-MMM-dd").Equals(dateOfBirth, StringComparison.InvariantCultureIgnoreCase)
                      select p;
            return res.ToArray();
        }

        private static void AddElementToDictionary(string key, FileCabinetRecord record, Dictionary<string, List<FileCabinetRecord>> dictionary)
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
