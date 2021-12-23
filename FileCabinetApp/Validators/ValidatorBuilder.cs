using System;
using System.Collections.Generic;
using FileCabinetApp.Readers;
using FileCabinetApp.Validators.ConcreteValidators;
using FileCabinetApp.Validators.Rules;

namespace FileCabinetApp.Validators
{
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators = new ();

        private readonly ValidationRules rules = ValidationRulesReader.ReadRulesFromConfig();

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
            return this.ValidateFirstName(this.rules.Default.FirstName.MinValue, this.rules.Default.FirstName.MaxValue)
                .ValidateLastName(this.rules.Default.LastName.MinValue, this.rules.Default.LastName.MaxValue)
                .ValidateDateOfBirth(this.rules.Default.DateOfBirth.MinValue, this.rules.Default.DateOfBirth.MaxValue)
                .ValidateHeight(this.rules.Default.Height.MinValue, this.rules.Default.Height.MaxValue)
                .ValidateWeight(this.rules.Default.Weight.MinValue, this.rules.Default.Weight.MaxValue)
                .Create();
        }

        public CompositeValidator CreateCustom()
        {
            return this.ValidateFirstName(this.rules.Custom.FirstName.MinValue, this.rules.Custom.FirstName.MaxValue)
                .ValidateLastName(this.rules.Custom.LastName.MinValue, this.rules.Custom.LastName.MaxValue)
                .ValidateDateOfBirth(this.rules.Custom.DateOfBirth.MinValue, this.rules.Custom.DateOfBirth.MaxValue)
                .ValidateHeight(this.rules.Custom.Height.MinValue, this.rules.Custom.Height.MaxValue)
                .ValidateWeight(this.rules.Custom.Weight.MinValue, this.rules.Custom.Weight.MaxValue)
                .Create();
        }
    }
}
