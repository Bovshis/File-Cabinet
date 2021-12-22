using System;
using FileCabinetApp.Records;

namespace FileCabinetApp.Validators.ConcreteValidators
{
    public class HeightValidator : IRecordValidator
    {
        private readonly short min;
        private readonly short max;

        public HeightValidator(short min, short max)
        {
            this.min = min;
            this.max = max;
        }

        public Tuple<bool, string> ValidateParameter(FileCabinetRecord parameter)
        {
            var height = parameter.Height;

            if (height < this.min || height > this.max)
            {
                return new Tuple<bool, string>(false, $"Height is less than {this.min} and greater than {this.max}");
            }

            return new Tuple<bool, string>(true, "Done");
        }
    }
}
