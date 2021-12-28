using System;
using System.IO;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    /// <summary>
    /// Command handler for execution 'export' command.
    /// </summary>
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service <see cref="IFileCabinetService"/>.</param>
        public ExportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Execute 'export' request.
        /// </summary>
        /// <param name="request">Request for execution that contain command and parameters.</param>
        /// <returns>Execution result message.</returns>
        public override object Handle(AppCommandRequest request)
        {
            if (!request.Command.Equals("export", StringComparison.InvariantCultureIgnoreCase))
            {
                return base.Handle(request);
            }

            var exportParameters = request.Parameters.Split(' ', 2);
            const int fileType = 0;
            const int filePath = 1;

            var isRewrite = IsRewrite(exportParameters[filePath]);
            return this.ExportToFile(exportParameters[filePath], exportParameters[fileType], isRewrite);
        }

        private static bool IsRewrite(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                throw new ArgumentException($"Export failed: can't open file {filePath}\n");
            }

            if (!fileInfo.Exists)
            {
                return false;
            }

            Console.Write($"File is exist - rewrite {filePath} [Y/n] ");
            var answer = Console.ReadLine() ?? throw new ArgumentNullException();
            if (answer.Equals("y", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            else if (answer.Equals("n", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                throw new ArgumentException("What's your problem?\n");
            }
        }

        private string ExportToFile(string filePath, string fileType, bool isRewrite)
        {
            using var streamWriter = new StreamWriter(filePath, isRewrite, System.Text.Encoding.Default);
            if (fileType.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
            {
                this.service.MakeSnapshot().SaveToCsv(streamWriter);
                streamWriter.Flush();
                return $"All records are exported to file {filePath}\n";
            }

            if (fileType.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                this.service.MakeSnapshot().SaveToXml(streamWriter);
                return $"All records are exported to file {filePath}\n";
            }

            throw new ArgumentException("Wrong type format!\n");
        }
    }
}
