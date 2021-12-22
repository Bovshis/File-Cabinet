using System;
using FileCabinetApp.Converters;
using FileCabinetApp.Records;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Readers
{
    public static class FileCabinetRecordConsoleReader
    {
        public static RecordWithoutId ReadRecordFromConsole(IRecordValidator validator)
        {
            var recordWithoutId = new RecordWithoutId();
            Console.Write("First name: ");
            recordWithoutId.FirstName = ReadInput(Converter.ConvertString, validator.ValidateFirstName);

            Console.Write("Last name: ");
            recordWithoutId.LastName = ReadInput(Converter.ConvertString, validator.ValidateLastName);

            Console.Write("Date of birth: ");
            recordWithoutId.DateOfBirth = ReadInput(Converter.ConvertDate, validator.ValidateDateOfBirth);

            Console.Write("Height: ");
            recordWithoutId.Height = ReadInput(Converter.ConvertShort, validator.ValidateHeight);

            Console.Write("Weight: ");
            recordWithoutId.Weight = ReadInput(Converter.ConvertDecimal, validator.ValidateWeight);

            Console.Write("Favorite Character: ");
            recordWithoutId.FavoriteCharacter = ReadInput(Converter.ConvertChar, validator.ValidateFavoriteCharacter);

            return recordWithoutId;
        }

        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
