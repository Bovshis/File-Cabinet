using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Records;

namespace FileCabinetApp.Validators.ConcreteValidators
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime from;
        private readonly DateTime to;

        public DateOfBirthValidator(DateTime @from, DateTime to)
        {
            this.@from = @from;
            this.to = to;
        }

        public Tuple<bool, string> ValidateParameter(FileCabinetRecord parameter)
        {
            var dateOfBirth = parameter.DateOfBirth;

            if (dateOfBirth < this.@from || dateOfBirth > this.to)
            {
                return new Tuple<bool, string>(false, $"date of birth is less than {this.@from} or greater than {this.to}");
            }

            return new Tuple<bool, string>(true, "Done");
        }
    }
}
