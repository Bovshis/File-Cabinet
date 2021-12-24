using System;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CommandHandlers.ConcreteHandlers;
using FileCabinetApp.Printers;
using FileCabinetApp.Services;
using FileCabinetApp.Settings;

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
            Console.Write("$ FileCabinetApp.exe ");
            var launcher = new Launcher();
            do
            {
                var settings = Console.ReadLine()?.Split(' ');
                if (settings == null)
                {
                    Console.WriteLine("Bad settings format!");
                    continue;
                }

                var (service, message) = launcher.SetSettings(settings);
                fileCabinetService = service;
                Console.WriteLine(message);
            }
            while (fileCabinetService == null);

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