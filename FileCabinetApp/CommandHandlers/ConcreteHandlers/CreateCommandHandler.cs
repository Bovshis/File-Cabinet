using System;
using FileCabinetApp.Readers;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        public CreateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("create", StringComparison.InvariantCultureIgnoreCase))
            {
                var id = this.service.CreateRecord(FileCabinetRecordConsoleReader.ReadRecordFromConsole(this.service.GetValidator()));
                return $"Record #{id} is created\n";
            }

            return base.Handle(request);
        }
    }
}
