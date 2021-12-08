using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

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
            this.list[id - 1] = new FileCabinetRecord
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

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            var res = from p in this.list
                      where p.FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase)
                      select p;
            return res.ToArray();
        }
    }
}
