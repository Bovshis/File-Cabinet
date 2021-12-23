using System;
using System.IO;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        public ExportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("export", StringComparison.InvariantCultureIgnoreCase))
            {
                var exportParameters = request.Parameters.Split(' ');
                const int amountParameters = 2;
                if (exportParameters.Length != amountParameters)
                {
                    return "Parameters amount is wrong\n";
                }

                const int fileType = 0;
                const int filePath = 1;

                // check file
                var fileInfo = new FileInfo(exportParameters[filePath]);
                if (!Directory.Exists(fileInfo.DirectoryName))
                {
                    return $"Export failed: can't open file {exportParameters[filePath]}\n";
                }

                var rewrite = false;
                if (fileInfo.Exists)
                {
                    Console.Write($"File is exist - rewrite {exportParameters[filePath]} [Y/n] ");
                    var answer = Console.ReadLine() ?? throw new ArgumentNullException();
                    if (answer.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    {
                        rewrite = false;
                    }
                    else if (answer.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                    {
                        rewrite = true;
                    }
                    else
                    {
                        return "What's your problem?\n";
                    }
                }

                // recording
                var streamWriter = new StreamWriter(exportParameters[filePath], rewrite, System.Text.Encoding.Default);
                if (exportParameters[fileType].Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    this.service.MakeSnapshot().SaveToCsv(streamWriter);
                    streamWriter.Flush();
                    streamWriter.Close();
                    return $"All records are exported to file {exportParameters[filePath]}\n";
                }

                if (exportParameters[fileType].Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    this.service.MakeSnapshot().SaveToXml(streamWriter);
                    return $"All records are exported to file {exportParameters[filePath]}\n";
                }

                return "Wrong type format!\n";
            }

            return base.Handle(request);
        }
    }
}
