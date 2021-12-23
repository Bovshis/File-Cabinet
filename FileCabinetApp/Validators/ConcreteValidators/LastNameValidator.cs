using System;
using FileCabinetApp.Records;

namespace FileCabinetApp.Validators.ConcreteValidators
{
    public class LastNameValidator : IRecordValidator
    {
        private readonly int minLength;
        private readonly int maxLength;

        public LastNameValidator(int minLength, int maxLength)
        {
            this.maxLength = maxLength;
            this.minLength = minLength;
        }

        public Tuple<bool, string> ValidateParameter(FileCabinetRecord parameter)
        {
            var lastName = parameter.LastName;

            if (lastName == null)
            {
                return new Tuple<bool, string>(false, "first name is null");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                return new Tuple<bool, string>(false, "first name is WhiteSpace");
            }

            if (lastName.Length < this.minLength || lastName.Length > this.maxLength)
            {
                return new Tuple<bool, string>(false, $"first name length less than {this.minLength} or greater than {this.maxLength}");
            }

            return new Tuple<bool, string>(true, "Done");
        }
    }
}
