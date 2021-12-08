using System;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
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

        private static FileCabinetService fileCabinetService = new FileCabinetService();

        public static void Main(string[] args)
        {
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
            string firstName;
            string lastName;
            DateTime dateTime;
            short height;
            decimal weight;
            char favoriteCharacter;

            bool isCorrectData = false;
            while (!isCorrectData)
            {
                try
                {
                    CheckPersonData(out firstName, out lastName, out dateTime, out height, out weight, out favoriteCharacter);

                    var id = fileCabinetService.CreateRecord(firstName, lastName, dateTime, height, weight, favoriteCharacter);
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
            string firstName;
            string lastName;
            DateTime dateTime;
            short height;
            decimal weight;
            char favoriteCharacter;

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
                    }

                    CheckPersonData(out firstName, out lastName, out dateTime, out height, out weight, out favoriteCharacter);

                    fileCabinetService.EditRecord(id, firstName, lastName, dateTime, height, weight, favoriteCharacter);
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
            var records = findParameters[0].ToUpper(CultureInfo.InvariantCulture) switch
            {
                "FIRSTNAME" => fileCabinetService.FindByFirstName(findParameters[1]),
                "LASTNAME" => fileCabinetService.FindByLastName(findParameters[1]),
                _ => null,
            };

            if (records == null || records.Count() == 0)
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

        private static void CheckPersonData(out string firstName, out string lastName, out DateTime dateTime, out short height, out decimal weight, out char favoriteCharacter)
        {
            Console.Write("First name: ");
            firstName = Console.ReadLine();

            Console.Write("Last name: ");
            lastName = Console.ReadLine();

            Console.Write("Date of birth: ");
            var dateParsed = DateTime.TryParse(Console.ReadLine(), out dateTime);
            if (!dateParsed)
            {
                throw new ArgumentException($"Invalid date format", nameof(dateTime));
            }

            Console.Write("Height: ");
            var heightParsed = short.TryParse(Console.ReadLine(), out height);
            if (!heightParsed)
            {
                throw new ArgumentException($"Invalid height format", nameof(height));
            }

            Console.Write("Weight: ");
            var weightParsed = decimal.TryParse(Console.ReadLine(), out weight);
            if (!weightParsed)
            {
                throw new ArgumentException($"Invalid weight format", nameof(weight));
            }

            Console.Write("Favorite Charachter: ");
            var charParsed = char.TryParse(Console.ReadLine(), out favoriteCharacter);

            if (!charParsed)
            {
                throw new ArgumentException($"Invalid char format", nameof(favoriteCharacter));
            }
        }
    }
}