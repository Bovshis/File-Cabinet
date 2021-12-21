using System;
using FileCabinetApp.Records;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Class for validation parameters (default validation).
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        public Tuple<bool, string> ValidateFirstName(string firstname)
        {
            if (firstname == null)
            {
                return new Tuple<bool, string>(false, "first name is null");
            }

            if (string.IsNullOrWhiteSpace(firstname))
            {
                return new Tuple<bool, string>(false, "first name is WhiteSpace");
            }

            if (firstname.Length is < 2 or > 60)
            {
                return new Tuple<bool, string>(false, "first name length less than 2 or greater than 60");
            }

            return new Tuple<bool, string>(true, "Done");
        }

        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            if (lastName == null)
            {
                return new Tuple<bool, string>(false, "last name is null");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                return new Tuple<bool, string>(false, "last name is WhiteSpace");
            }

            if (lastName.Length is < 2 or > 60)
            {
                return new Tuple<bool, string>(false, "last name length less than 2 or greater than 60");
            }

            return new Tuple<bool, string>(true, "Done");
        }

        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                return new Tuple<bool, string>(false, "date of birth is less than 1 Jan 1950 or greater than current time");
            }

            return new Tuple<bool, string>(true, "Done");
        }

        public Tuple<bool, string> ValidateHeight(short height)
        {
            if (height < 0)
            {
                return new Tuple<bool, string>(false, "Height is less than 0");
            }

            return new Tuple<bool, string>(true, "Done");
        }

        public Tuple<bool, string> ValidateWeight(decimal weight)
        {
            if (weight < 0)
            {
                return new Tuple<bool, string>(false, "weight is less than 0");
            }

            return new Tuple<bool, string>(true, "Done");
        }

        public Tuple<bool, string> ValidateFavoriteCharacter(char favoriteCharacter)
        {
            return new Tuple<bool, string>(true, "Done");
        }

        public bool ValidateRecord(FileCabinetRecord record)
        {
            if (!this.ValidateFirstName(record.FirstName).Item1)
            {
                return false;
            }

            if (!this.ValidateLastName(record.LastName).Item1)
            {
                return false;
            }

            if (!this.ValidateDateOfBirth(record.DateOfBirth).Item1)
            {
                return false;
            }

            if (!this.ValidateHeight(record.Height).Item1)
            {
                return false;
            }

            if (!this.ValidateWeight(record.Weight).Item1)
            {
                return false;
            }

            if (!this.ValidateFavoriteCharacter(record.FavoriteCharacter).Item1)
            {
                return false;
            }

            return true;
        }
    }
}
