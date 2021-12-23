using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Printers;
using FileCabinetApp.Records;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<IEnumerable<FileCabinetRecord>> print;

        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> print)
            : base(service)
        {
            this.print = print;
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("list", StringComparison.InvariantCultureIgnoreCase))
            {
                var recordsList = this.service.GetRecords();
                this.print(recordsList);
                return string.Empty;
            }

            return base.Handle(request);
        }
    }
}
