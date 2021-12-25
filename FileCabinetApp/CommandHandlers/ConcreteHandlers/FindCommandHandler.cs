using System;
using System.Collections.Generic;
using System.Globalization;
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
                try
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
                    this.print(records);
                    return string.Empty;
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }

            return base.Handle(request);
        }
    }
}
