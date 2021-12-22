using System;
using FileCabinetApp.Readers;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        public EditCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("edit", StringComparison.InvariantCultureIgnoreCase))
            {
                var isParsed = int.TryParse(request.Parameters, out var id);

                if (!isParsed || id < 0)
                {
                    return $"#{id} record is not found.\n";
                }

                if (this.service.EditRecord(id, FileCabinetRecordConsoleReader.ReadRecordFromConsole(this.service.GetValidator())))
                {
                    return $"Record #{id} is edited\n";
                }

                return $"#{id} record is not found.\n";
            }

            return base.Handle(request);
        }
    }
}
