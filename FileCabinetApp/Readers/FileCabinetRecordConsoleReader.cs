using System;
using FileCabinetApp.Converters;
using FileCabinetApp.Records;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.ConcreteValidators;

namespace FileCabinetApp.Readers
{
    public static class FileCabinetRecordConsoleReader
    {
        public static RecordWithoutId ReadRecordFromConsole(IRecordValidator validator)
        {
            var recordWithoutId = new RecordWithoutId();

            do
            {
                Console.WriteLine("Write correct data: ");
                Console.Write("First name: ");
                recordWithoutId.FirstName = ReadInput(Converter.ConvertString);

                Console.Write("Last name: ");
                recordWithoutId.LastName = ReadInput(Converter.ConvertString);

                Console.Write("Date of birth: ");
                recordWithoutId.DateOfBirth = ReadInput(Converter.ConvertDate);

                Console.Write("Height: ");
                recordWithoutId.Height = ReadInput(Converter.ConvertShort);

                Console.Write("Weight: ");
                recordWithoutId.Weight = ReadInput(Converter.ConvertDecimal);

                Console.Write("Favorite Character: ");
                recordWithoutId.FavoriteCharacter = ReadInput(Converter.ConvertChar);
            }
            while (!validator.ValidateParameter(new FileCabinetRecord(1, recordWithoutId)).Item1);

            return recordWithoutId;
        }

        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter)
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

                return conversionResult.Item3;
            }
            while (true);
        }
    }
}
