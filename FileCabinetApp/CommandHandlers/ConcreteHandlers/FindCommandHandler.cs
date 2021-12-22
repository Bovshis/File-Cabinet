using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Printers;
using FileCabinetApp.Records;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<IEnumerable<FileCabinetRecord>> print;

        public FindCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> print)
            : base(service)
        {
            this.print = print;
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

                this.print(records);
                return string.Empty;
            }

            return base.Handle(request);
        }
    }
}
