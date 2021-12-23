using System;
using System.IO;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CommandHandlers.ConcreteHandlers;
using FileCabinetApp.Printers;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Console application for executing commands in the file cabinet.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Daniil Bovshis";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static bool isRunning = true;
        private static IFileCabinetService fileCabinetService;

        /// <summary>
        /// Main method that determines which command to execute.
        /// </summary>
        public static void Main()
        {
            SetSettings();
            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            Console.WriteLine(HintMessage);
            Console.WriteLine();

            var commands = CreateCommandHandlers();
            do
            {
                Console.Write("> ");
                var appCommandRequest = ReadCommandRequest();
                if (appCommandRequest == null)
                {
                    continue;
                }

                var result = commands.Handle(appCommandRequest);
                if (result != null)
                {
                    Console.WriteLine(result.ToString());
                }
                else
                {
                    PrintMissedCommandInfo(appCommandRequest.Command);
                }
            }
            while (isRunning);
        }

        private static AppCommandRequest ReadCommandRequest()
        {
            var inputs = Console.ReadLine()?.Split(' ', 2);

            const int commandIndex = 0;
            var command = inputs?[commandIndex];
            if (string.IsNullOrEmpty(command))
            {
                Console.WriteLine(HintMessage);
                return null;
            }

            const int parametersIndex = 1;
            var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;

            return new AppCommandRequest(command, parameters);
        }

        private static void SetSettings()
        {
            var isCorrectSettings = false;
            while (!isCorrectSettings)
            {
                try
                {
                    Console.Write("$ FileCabinetApp.exe ");
                    var settings = Console.ReadLine()?.Split(' ');

                    if (settings == null)
                    {
                        throw new ArgumentNullException(nameof(settings));
                    }

                    if (settings.Length % 2 != 0)
                    {
                        throw new ArgumentException("Bad settings format!");
                    }

                    IRecordValidator validator;
                    var validationModeIndex = Array.FindIndex(settings, x => x is "--validation-rules" or "-v") + 1;
                    if (validationModeIndex != 0)
                    {
                        validator = GetValidationMode(settings[validationModeIndex]);
                    }
                    else
                    {
                        Console.WriteLine("Using default validation rules.");
                        validator = new ValidatorBuilder().CreateDefault();
                    }

                    var storageModeIndex = Array.FindIndex(settings, x => x is "--storage" or "-s") + 1;
                    if (storageModeIndex != 0)
                    {
                        SetStorageMode(settings[storageModeIndex], validator);
                    }
                    else
                    {
                        Console.WriteLine("Using memory cabinet.");
                        fileCabinetService = new FileCabinetMemoryService(validator);
                    }

                    isCorrectSettings = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static IRecordValidator GetValidationMode(string validationMode)
        {
            const string validationRulesDefaultMode = "default", validationRulesCustomMode = "custom";

            if (validationMode.Equals(validationRulesDefaultMode, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Using default validation rules.");
                return new ValidatorBuilder().CreateDefault();
            }

            if (validationMode.Equals(validationRulesCustomMode, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Using custom validation rules.");
                return new ValidatorBuilder().CreateCustom();
            }

            throw new ArgumentException($"Bad validation rules command");
        }

        private static void SetStorageMode(string storageMode, IRecordValidator validator)
        {
            const string memoryMode = "memory", fileMode = "file";

            if (storageMode.Equals(memoryMode, StringComparison.InvariantCultureIgnoreCase))
            {
                fileCabinetService = new FileCabinetMemoryService(validator);
                Console.WriteLine("Using memory cabinet.");
                return;
            }

            if (storageMode.Equals(fileMode, StringComparison.InvariantCultureIgnoreCase))
            {
                fileCabinetService = new FileCabinetFilesystemService(new FileStream("cabinet-records.db", FileMode.Create), validator);
                Console.WriteLine("Using file cabinet.");
                return;
            }

            throw new ArgumentException($"Bad validation rules command");
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var helpHandler = new HelpCommandHandler();
            var exitHandler = new ExitCommandHandler(RunOrExit);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService, DefaultRecordPrinter.Print);
            var editHandler = new EditCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService, DefaultRecordPrinter.Print);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);

            helpHandler.SetNext(exitHandler).SetNext(statHandler).SetNext(createHandler)
                .SetNext(listHandler).SetNext(editHandler).SetNext(findHandler).SetNext(exportHandler)
                .SetNext(importHandler).SetNext(removeHandler).SetNext(purgeHandler);

            return helpHandler;
        }

        private static void RunOrExit(bool flag)
        {
            isRunning = flag;
        }
    }
}