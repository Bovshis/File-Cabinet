using System;
using System.Globalization;
using FileCabinetApp.Printers;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IRecordPrinter printer;

        public FindCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.printer = printer;
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("find", StringComparison.InvariantCultureIgnoreCase))
            {
                var findParameters = request.Parameters.Split(' ', 2);
                const int property = 0;
                const int searchText = 1;
                var records = findParameters[property].ToUpper(CultureInfo.InvariantCulture) switch
                {
                    "FIRSTNAME" => this.service.FindByFirstName(findParameters[searchText]),
                    "LASTNAME" => this.service.FindByLastName(findParameters[searchText]),
                    "DATEOFBIRTH" => this.service.FindByDateOfBirth(findParameters[searchText]),
                    _ => null,
                };

                if (records == null || records.Count == 0)
                {
                    return $"There is no record for '{request.Parameters}' parameters.\n";
                }

                this.printer.Print(records);
                return string.Empty;
            }

            return base.Handle(request);
        }
    }
}
