using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for validation parameters (custom validation).
    /// </summary>
    public class CustomValidator : IRecordValidator
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

            if (!char.IsUpper(firstname[0]))
            {
                return new Tuple<bool, string>(false, "first name doesn't start with upper letter");
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

            if (!char.IsUpper(lastName[0]))
            {
                return new Tuple<bool, string>(false, "last name doesn't start with upper letter");
            }

            return new Tuple<bool, string>(true, "Done");
        }

        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < new DateTime(1899, 1, 1))
            {
                return new Tuple<bool, string>(false, "date of birth is less than 1 Jan 1899");
            }

            return new Tuple<bool, string>(true, "Done");
        }

        public Tuple<bool, string> ValidateHeight(short height)
        {
            if (height < 50 || height > 300)
            {
                return new Tuple<bool, string>(false, "height is less than 50 or greater than 300");
            }

            return new Tuple<bool, string>(true, "Done");
        }

        public Tuple<bool, string> ValidateWeight(decimal weight)
        {
            if (weight < 10 || weight > 200)
            {
                return new Tuple<bool, string>(false, "weight is less than 10 or greater than 200");
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
