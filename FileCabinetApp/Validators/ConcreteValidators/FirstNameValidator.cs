using System;
using FileCabinetApp.Records;

namespace FileCabinetApp.Validators.ConcreteValidators
{
    public class FirstNameValidator : IRecordValidator
    {
        private readonly int minLength;
        private readonly int maxLength;

        public FirstNameValidator(int minLength, int maxLength)
        {
            this.maxLength = maxLength;
            this.minLength = minLength;
        }

        public Tuple<bool, string> ValidateParameter(FileCabinetRecord parameter)
        {
            var firstname = parameter.FirstName;

            if (firstname == null)
            {
                return new Tuple<bool, string>(false, "first name is null");
            }

            if (string.IsNullOrWhiteSpace(firstname))
            {
                return new Tuple<bool, string>(false, "first name is WhiteSpace");
            }

            if (firstname.Length < this.maxLength || firstname.Length > this.maxLength)
            {
                return new Tuple<bool, string>(false, $"first name length less than {this.minLength} or greater than {this.maxLength}");
            }

            return new Tuple<bool, string>(true, "Done");
        }
    }
}
