using System;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        public RemoveCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("remove", StringComparison.InvariantCultureIgnoreCase))
            {
                var isParsed = int.TryParse(request.Parameters, out var id);
                if (!isParsed)
                {
                    return $"Record #{request.Parameters} doesn't exists.\n";
                }

                this.service.Remove(id);
                return string.Empty;
            }

            return base.Handle(request);
        }
    }
}
