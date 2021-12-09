using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Сonsole application for executing commands in the file cabinet.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Daniil Bovshis";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "get stat on records", "The 'stat' command get stat on records." },
            new string[] { "create", "create record", "The 'create' command create record." },
            new string[] { "list", "Write the list of records", "The 'list' command write the list of records." },
            new string[] { "edit", "edit record", "The 'edit' command edit record." },
            new string[] { "find", "find records", "The 'find' command find records." },
        };

        private static FileCabinetService fileCabinetService;

        /// <summary>
        /// Main method that determines which command to execute.
        /// </summary>
        public static void Main()
        {
            var isCorrectSettings = false;
            while (!isCorrectSettings)
            {
                try
                {
                    Console.Write("$ FileCabinetApp.exe ");
                    var settings = Console.ReadLine().Split(' ');
                    const int validationSettingIndex = 0;
                    SetValidationMode(settings[validationSettingIndex]);

                    isCorrectSettings = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void SetValidationMode(string settings)
        {
            if (settings.Length == 0 || string.IsNullOrWhiteSpace(settings))
            {
                fileCabinetService = new FileCabinetService(new DefaultValidator());
                Console.WriteLine("Using default validation rules.");
                return;
            }

            var validationSettings = settings.Split('=');
            const int command = 0;
            const int mode = 1;
            const string validationRulesCommand = "--validation-rules", shortValidationRulesCommand = "-v";
            const string validationRulesDefaultMode = "default", validationRulesCustomMode = "custom";
            if (validationSettings[command] == validationRulesCommand || validationSettings[command] == shortValidationRulesCommand)
            {
                if (validationSettings[mode].Equals(validationRulesDefaultMode, StringComparison.InvariantCultureIgnoreCase))
                {
                    fileCabinetService = new FileCabinetService(new DefaultValidator());
                    Console.WriteLine("Using default validation rules.");
                    return;
                }

                if (validationSettings[mode].Equals(validationRulesCustomMode, StringComparison.InvariantCultureIgnoreCase))
                {
                    fileCabinetService = new FileCabinetService(new CustomValidator());
                    Console.WriteLine("Using custom validation rules.");
                    return;
                }
            }

            throw new ArgumentException($"Bad validation rules command");
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            RecordWithoutId recordWithoutId = new ();
            bool isCorrectData = false;
            while (!isCorrectData)
            {
                try
                {
                    CheckDataForRecord(recordWithoutId);
                    var id = fileCabinetService.CreateRecord(recordWithoutId);
                    Console.WriteLine($"Record #{id} is created");
                    isCorrectData = true;
                }
                catch (Exception ex)
                {
                    Console.Write("Bad data format: ");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Enter correct data :");
                }
            }
        }

        private static void List(string parameters)
        {
            var recordsList = fileCabinetService.GetRecords();

            foreach (var record in recordsList)
            {
                Console.WriteLine(record.ToString());
            }
        }

        private static void Edit(string parameters)
        {
            RecordWithoutId recordWithoutId = new ();

            bool isCorrectData = false;
            while (!isCorrectData)
            {
                try
                {
                    int id;
                    var idParsed = int.TryParse(parameters, out id);
                    if (!idParsed || id < 0)
                    {
                        throw new ArgumentException($"Invalid parameters format", nameof(parameters));
                    }

                    if (id > fileCabinetService.GetStat())
                    {
                        Console.WriteLine($"#{id} record is not found.");
                        return;
                    }

                    CheckDataForRecord(recordWithoutId);
                    fileCabinetService.EditRecord(id, recordWithoutId);

                    Console.WriteLine($"Record #{id} is updated");
                    isCorrectData = true;
                }
                catch (Exception ex)
                {
                    Console.Write("Bad data format: ");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Enter correct data :");
                }
            }
        }

        private static void Find(string parameters)
        {
            var findParameters = parameters.Split(' ');
            const int property = 0;
            const int searchText = 1;
            var records = findParameters[property].ToUpper(CultureInfo.InvariantCulture) switch
            {
                "FIRSTNAME" => fileCabinetService.FindByFirstName(findParameters[searchText]),
                "LASTNAME" => fileCabinetService.FindByLastName(findParameters[searchText]),
                "DATEOFBIRTH" => fileCabinetService.FindByDateOfBirth(findParameters[searchText]),
                _ => null,
            };

            if (records == null || records.Length == 0)
            {
                Console.WriteLine($"There is no record for '{parameters}' parameters.");
            }
            else
            {
                foreach (var record in records)
                {
                    Console.WriteLine(record.ToString());
                }
            }
        }

        private static void CheckDataForRecord(RecordWithoutId recordWithoutId)
        {
            Console.Write("First name: ");
            recordWithoutId.FirstName = Console.ReadLine();

            Console.Write("Last name: ");
            recordWithoutId.LastName = Console.ReadLine();

            Console.Write("Date of birth: ");
            DateTime date;
            var dateParsed = DateTime.TryParse(Console.ReadLine(), out date);
            if (!dateParsed)
            {
                throw new ArgumentException($"Invalid date format");
            }

            recordWithoutId.DateOfBirth = date;

            Console.Write("Height: ");
            short height;
            var heightParsed = short.TryParse(Console.ReadLine(), out height);
            if (!heightParsed)
            {
                throw new ArgumentException($"Invalid height format");
            }

            recordWithoutId.Height = height;

            Console.Write("Weight: ");
            decimal weight;
            var weightParsed = decimal.TryParse(Console.ReadLine(), out weight);
            if (!weightParsed)
            {
                throw new ArgumentException($"Invalid weight format");
            }

            recordWithoutId.Weight = weight;

            Console.Write("Favorite Charachter: ");
            char favoriteCharacter;
            var charParsed = char.TryParse(Console.ReadLine(), out favoriteCharacter);

            if (!charParsed)
            {
                throw new ArgumentException($"Invalid char format");
            }

            recordWithoutId.FavoriteCharacter = favoriteCharacter;
        }
    }
}