using System;
using System.Collections.Generic;
using FileCabinetApp.Validators.ConcreteValidators;

namespace FileCabinetApp.Validators
{
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new ();

        public ValidatorBuilder ValidateFirstName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));
            return this;
        }

        public ValidatorBuilder ValidateLastName(int minLength, int maxLength)
        {
            this.validators.Add(new LastNameValidator(minLength, maxLength));
            return this;
        }

        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        public ValidatorBuilder ValidateHeight(short min, short max)
        {
            this.validators.Add(new HeightValidator(min, max));
            return this;
        }

        public ValidatorBuilder ValidateWeight(decimal min, decimal max)
        {
            this.validators.Add(new WeightValidator(min, max));
            return this;
        }

        public CompositeValidator Create()
        {
            return new CompositeValidator(this.validators);
        }

        public CompositeValidator CreateDefault()
        {
            return this.ValidateFirstName(2, 60).ValidateLastName(2, 60)
                .ValidateDateOfBirth(new DateTime(1950, 1, 1), DateTime.Now).
                ValidateHeight(0,400).ValidateWeight(0, 1000).Create();
        }

        public CompositeValidator CreateCustom()
        {
            return this.ValidateFirstName(1, 20).ValidateLastName(1, 20)
                .ValidateDateOfBirth(new DateTime(1900, 1, 1), new DateTime(2050, 1, 1)).
                ValidateHeight(0, 300).ValidateWeight(0, 200).Create();
        }
    }
}
