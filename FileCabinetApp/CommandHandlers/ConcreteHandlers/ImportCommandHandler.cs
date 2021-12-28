using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FileCabinetApp.Records;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    /// <summary>
    /// Command handler for execution 'import' command.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service <see cref="IFileCabinetService"/>.</param>
        public ImportCommandHandler(IFileCabinetService service)
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
            if (!request.Command.Equals("import", StringComparison.InvariantCultureIgnoreCase))
            {
                return base.Handle(request);
            }

            try
            {
                var importParameters = request.Parameters.Split(' ', 2);
                const int fileType = 0;
                const int filePath = 1;
                var fileCabinetServiceSnapshot = ImportFromFile(importParameters[filePath], importParameters[fileType]);
                var importedAmount = this.service.Restore(fileCabinetServiceSnapshot);
                return $"{importedAmount} records were imported from {importParameters[filePath]}.\n";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static FileCabinetServiceSnapshot ImportFromFile(string filePath, string fileType)
        {
            var fileInfo = new FileInfo(filePath);
            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                throw new ArgumentException($"Export failed: can't open file {filePath}\n");
            }

            using var fileStream = new FileStream(filePath, FileMode.Open);
            var fileCabinetServiceSnapshot = new FileCabinetServiceSnapshot(
                new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>()));
            if (fileType.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
            {
                fileCabinetServiceSnapshot.LoadFromCsv(new StreamReader(fileStream));
            }
            else if (fileType.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                fileCabinetServiceSnapshot.LoadFromXml(fileStream);
            }
            else
            {
                throw new ArgumentException("Wrong type format!\n");
            }

            return fileCabinetServiceSnapshot;
        }
    }
}
