using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Сlass for working with a list of records.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();

        private readonly IRecordValidator validator;
        private readonly IConverter converter = new Converter();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// validation rules.
        /// </summary>
        /// <param name="recordValidator">parameters validator.</param>
        public FileCabinetMemoryService(IRecordValidator recordValidator)
        {
            this.validator = recordValidator;
        }

        /// <summary>
        /// Create record, adds to list and dictionaries.
        /// </summary>
        /// <returns>record number.</returns>
        public int CreateRecord()
        {
            var record = CreateRecord(this.list.Count + 1, this.ReadRecordFromConsole());
            this.list.Add(record);
            this.AddElementToDictionaries(record);

            return record.Id;
        }

        /// <summary>
        /// Get records from the file cabinet list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Get count records from the file cabinet list..
        /// </summary>
        /// <returns>count of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Edit record.
        /// </summary>
        /// <param name="id">number of the edited record.</param>
        public void EditRecord(int id)
        {
            var oldRecord = this.list[id - 1];
            this.firstNameDictionary[oldRecord.FirstName.ToUpper(CultureInfo.InvariantCulture)].Remove(oldRecord);
            this.lastNameDictionary[oldRecord.LastName.ToUpper(CultureInfo.InvariantCulture)].Remove(oldRecord);
            this.dateOfBirthDictionary[oldRecord.DateOfBirth].Remove(oldRecord);

            var record = CreateRecord(id, this.ReadRecordFromConsole());
            this.list[id - 1] = record;
            this.AddElementToDictionaries(record);
        }

        /// <summary>
        /// Find list of the records by first name.
        /// </summary>
        /// <param name="firstName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var key = firstName.ToUpper(CultureInfo.InvariantCulture);
            if (this.firstNameDictionary.ContainsKey(key))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName.ToUpper(CultureInfo.InvariantCulture)]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Find list of the records by last Name.
        /// </summary>
        /// <param name="lastName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var key = lastName.ToUpper(CultureInfo.InvariantCulture);
            if (this.firstNameDictionary.ContainsKey(key))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[lastName.ToUpper(CultureInfo.InvariantCulture)]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Find list of the records by date Of Birth.
        /// </summary>
        /// <param name="dateOfBirth">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var dateParsed = DateTime.TryParse(dateOfBirth, out var dateOfBirthDate);
            if (dateParsed && this.dateOfBirthDictionary.ContainsKey(dateOfBirthDate))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[dateOfBirthDate]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Make snapshot.
        /// </summary>
        /// <returns>FileCabinetService snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords());
        }

        private static void AddElementToDictionary<T>(T key, FileCabinetRecord record, Dictionary<T, List<FileCabinetRecord>> dictionary)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(record);
            }
            else
            {
                dictionary[key] = new List<FileCabinetRecord>() { record };
            }
        }

        private static FileCabinetRecord CreateRecord(int id, RecordWithoutId recordWithoutId)
        {
            return new FileCabinetRecord
            {
                Id = id,
                FirstName = recordWithoutId.FirstName,
                LastName = recordWithoutId.LastName,
                DateOfBirth = recordWithoutId.DateOfBirth,
                Height = recordWithoutId.Height,
                Weight = recordWithoutId.Weight,
                FavoriteCharacter = recordWithoutId.FavoriteCharacter,
            };
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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

        private void AddElementToDictionaries(FileCabinetRecord record)
        {
            AddElementToDictionary(record.FirstName.ToUpper(CultureInfo.InvariantCulture), record, this.firstNameDictionary);
            AddElementToDictionary(record.LastName.ToUpper(CultureInfo.InvariantCulture), record, this.lastNameDictionary);
            AddElementToDictionary(record.DateOfBirth, record, this.dateOfBirthDictionary);
        }

        private RecordWithoutId ReadRecordFromConsole()
        {
            RecordWithoutId recordWithoutId = new ();
            Console.Write("First name: ");
            recordWithoutId.FirstName = ReadInput(this.converter.ConvertString, this.validator.ValidateFirstName);

            Console.Write("Last name: ");
            recordWithoutId.LastName = ReadInput(this.converter.ConvertString, this.validator.ValidateLastName);

            Console.Write("Date of birth: ");
            recordWithoutId.DateOfBirth = ReadInput(this.converter.ConvertDate, this.validator.ValidateDateOfBirth);

            Console.Write("Height: ");
            recordWithoutId.Height = ReadInput(this.converter.ConvertShort, this.validator.ValidateHeight);

            Console.Write("Weight: ");
            recordWithoutId.Weight = ReadInput(this.converter.ConvertDecimal, this.validator.ValidateWeight);

            Console.Write("Favorite Character: ");
            recordWithoutId.FavoriteCharacter = ReadInput(this.converter.ConvertChar, this.validator.ValidateFavoriteCharacter);

            return recordWithoutId;
        }
    }
}
