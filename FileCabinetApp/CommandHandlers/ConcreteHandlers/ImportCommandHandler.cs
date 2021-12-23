using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FileCabinetApp.Records;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        public ImportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("import", StringComparison.InvariantCultureIgnoreCase))
            {
                var importParameters = request.Parameters.Split(' ');
                const int amountParameters = 2;
                if (importParameters.Length != amountParameters)
                {
                    return "Parameters amount is wrong\n";
                }

                const int fileType = 0;
                const int filePath = 1;

                // check file
                var fileInfo = new FileInfo(importParameters[filePath]);
                if (!Directory.Exists(fileInfo.DirectoryName))
                {
                    return $"Export failed: can't open file {importParameters[filePath]}\n";
                }

                using var fileStream = new FileStream(importParameters[filePath], FileMode.Open);
                var fileCabinetServiceSnapshot = new FileCabinetServiceSnapshot(
                    new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>()));
                if (importParameters[fileType].Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    fileCabinetServiceSnapshot.LoadFromCsv(new StreamReader(fileStream));
                }
                else if (importParameters[fileType].Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    fileCabinetServiceSnapshot.LoadFromXml(fileStream);
                }
                else
                {
                    return "Wrong type format!\n";
                }

                var importedAmount = this.service.Restore(fileCabinetServiceSnapshot);
                return $"{importedAmount} records were imported from {importParameters[filePath]}.\n";
            }

            return base.Handle(request);
        }
    }
}
