using System;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "get stat on records", "The 'stat' command get stat on records." },
            new string[] { "create", "create record", "The 'create' command create record." },
            new string[] { "export", "export records", "The 'export' command export records." },
            new string[] { "import", "import records", "The 'import' command import records." },
            new string[] { "purge", "purge records", "The 'purge' command purge records." },
            new string[] { "insert", "insert record", "The 'insert' command insert record." },
            new string[] { "delete", "delete records", "The 'delete' command delete records." },
            new string[] { "update", "update records", "The 'update' command update records." },
            new string[] { "select", "select records", "The 'select' command select records." },
        };

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("help", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!string.IsNullOrEmpty(request.Parameters))
                {
                    var index = Array.FindIndex(HelpMessages, 0, HelpMessages.Length, i => string.Equals(i[CommandHelpIndex], request.Parameters, StringComparison.InvariantCultureIgnoreCase));
                    if (index >= 0)
                    {
                        return HelpMessages[index][ExplanationHelpIndex] + "\n";
                    }
                    else
                    {
                        return $"There is no explanation for '{request.Parameters}' command.\n";
                    }
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.Append("Available commands:\n");
                    foreach (var helpMessage in HelpMessages)
                    {
                        sb.Append(helpMessage[CommandHelpIndex]).Append(" - ").Append(helpMessage[DescriptionHelpIndex]).Append('\n');
                    }

                    return sb.ToString();
                }
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
