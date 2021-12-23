using System;
using FileCabinetApp.Records;

namespace FileCabinetApp.Validators.ConcreteValidators
{
    public class WeightValidator : IRecordValidator
    {
        private readonly decimal min;
        private readonly decimal max;

        public WeightValidator(decimal min, decimal max)
        {
            this.min = min;
            this.max = max;
        }

        public Tuple<bool, string> ValidateParameter(FileCabinetRecord parameter)
        {
            var weight = parameter.Weight;

            if (weight < this.min || weight > this.max)
            {
                return new Tuple<bool, string>(false, $"Weight is less than {this.min} and greater than {this.max}");
            }

            return new Tuple<bool, string>(true, "Done");
        }
    }
}
