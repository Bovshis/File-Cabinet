using System;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        public StatCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("stat", StringComparison.InvariantCultureIgnoreCase))
            {
                var recordsCount = this.service.GetStat();
                return $"{recordsCount} record(s).\n";
            }

            return base.Handle(request);
        }
    }
}
