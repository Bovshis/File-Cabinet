using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.Records;

namespace FileCabinetApp.Validators
{
    public class CompositeValidator : IRecordValidator
    {
        public CompositeValidator(List<IRecordValidator> validators)
        {
            this.Validators = validators;
        }

        public List<IRecordValidator> Validators { get; }

        public Tuple<bool, string> ValidateParameter(FileCabinetRecord parameter)
        {
            foreach (var result in this.Validators.Select(validator => validator.ValidateParameter(parameter)).Where(result => !result.Item1))
            {
                return result;
            }

            return new Tuple<bool, string>(true, "Done");
        }
    }
}
