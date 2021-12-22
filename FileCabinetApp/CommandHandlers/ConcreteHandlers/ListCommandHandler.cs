using System;
using System.Text;
using FileCabinetApp.Printers;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IRecordPrinter printer;

        public ListCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.printer = printer;
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("list", StringComparison.InvariantCultureIgnoreCase))
            {
                var recordsList = this.service.GetRecords();
                this.printer.Print(recordsList);
                return string.Empty;
            }

            return base.Handle(request);
        }
    }
}
