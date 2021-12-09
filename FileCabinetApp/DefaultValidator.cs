using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class DefaultValidator : IRecordValidator
    {
        public void ValidateParameters(RecordWithoutId recordWithoutId)
        {
            if (recordWithoutId.FirstName == null)
            {
                throw new ArgumentNullException(nameof(recordWithoutId));
            }

            if (string.IsNullOrWhiteSpace(recordWithoutId.FirstName))
            {
                throw new ArgumentException($"first name is WhiteSpace", nameof(recordWithoutId));
            }

            if (recordWithoutId.FirstName.Length < 2 || recordWithoutId.FirstName.Length > 60)
            {
                throw new ArgumentException($"first name lenght less than 2 or greater than 60", nameof(recordWithoutId));
            }

            if (recordWithoutId.LastName == null)
            {
                throw new ArgumentNullException(nameof(recordWithoutId));
            }

            if (string.IsNullOrWhiteSpace(recordWithoutId.FirstName))
            {
                throw new ArgumentException($"last name is WhiteSpace", nameof(recordWithoutId));
            }

            if (recordWithoutId.LastName.Length < 2 || recordWithoutId.LastName.Length > 60)
            {
                throw new ArgumentException($"last name lenght less than 2 or greater than 60", nameof(recordWithoutId));
            }

            if (recordWithoutId.DateOfBirth < new DateTime(1950, 1, 1) || recordWithoutId.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"date of birth is less than 1 Jan 1950 or greater than current time", nameof(recordWithoutId));
            }

            if (recordWithoutId.Height < 0)
            {
                throw new ArgumentException($"height is less than 0", nameof(recordWithoutId));
            }

            if (recordWithoutId.Weight < 0)
            {
                throw new ArgumentException($"weight is less than 0", nameof(recordWithoutId));
            }
        }
    }
}
