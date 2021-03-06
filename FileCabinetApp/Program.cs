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
            StartProgram();
            var commands = CreateCommandHandlers();
            do
            {
                ExecuteCommand(commands);
            }
            while (isRunning);
        }

        private static void StartProgram()
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
        }

        private static void ExecuteCommand(ICommandHandler commands)
        {
            Console.Write("> ");
            var appCommandRequest = ReadCommandRequest();
            if (appCommandRequest == null)
            {
                return;
            }

            try
            {
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
            var search = new AdvancedSearch(command);
            var res = search.GetSimilarCommand();
            Console.WriteLine($"there is no {command} command. Similar command:\n {string.Join("\n", res)}");
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var helpHandler = new HelpCommandHandler();
            var exitHandler = new ExitCommandHandler(RunOrExit);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var insertHandler = new InsertCommandHandler(fileCabinetService);
            var deleteHandler = new DeleteCommandHandler(fileCabinetService);
            var updateHandler = new UpdateCommandHandler(fileCabinetService);
            var selectHandler = new SelectCommandHandler(fileCabinetService, DefaultRecordPrinter.Print);

            helpHandler
                .SetNext(exitHandler)
                .SetNext(statHandler)
                .SetNext(createHandler)
                .SetNext(updateHandler)
                .SetNext(exportHandler)
                .SetNext(importHandler)
                .SetNext(purgeHandler)
                .SetNext(insertHandler)
                .SetNext(deleteHandler)
                .SetNext(selectHandler);

            return helpHandler;
        }

        private static void RunOrExit(bool flag)
        {
            isRunning = flag;
        }
    }
}