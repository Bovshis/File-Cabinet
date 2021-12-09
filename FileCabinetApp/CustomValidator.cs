using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class CustomValidator : IRecordValidator
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

            if (!char.IsUpper(recordWithoutId.FirstName[0]))
            {
                throw new ArgumentException($"first name doesn't start with upper letter", nameof(recordWithoutId));
            }

            if (recordWithoutId.LastName == null)
            {
                throw new ArgumentNullException(nameof(recordWithoutId));
            }

            if (string.IsNullOrWhiteSpace(recordWithoutId.FirstName))
            {
                throw new ArgumentException($"last name is WhiteSpace", nameof(recordWithoutId));
            }

            if (!char.IsUpper(recordWithoutId.LastName[0]))
            {
                throw new ArgumentException($"last name doesn't start with upper letter", nameof(recordWithoutId));
            }

            if (recordWithoutId.DateOfBirth < new DateTime(1899, 1, 1))
            {
                throw new ArgumentException($"date of birth is less than 1 Jan 1899", nameof(recordWithoutId));
            }

            if (recordWithoutId.Height < 50 || recordWithoutId.Height > 300)
            {
                throw new ArgumentException($"height is less than 50 or greater than 300", nameof(recordWithoutId));
            }

            if (recordWithoutId.Weight < 10 || recordWithoutId.Weight > 200)
            {
                throw new ArgumentException($"weight is less than 10 or greater than 200", nameof(recordWithoutId));
            }
        }
    }
}
