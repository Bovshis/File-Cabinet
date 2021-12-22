using System;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("purge", StringComparison.InvariantCultureIgnoreCase))
            {
                if (this.service is FileCabinetFilesystemService filesystemService)
                {
                    filesystemService.Purge();
                    return string.Empty;
                }
                else
                {
                    return "It is not Filesystem storage\n";
                }
            }

            return base.Handle(request);
        }
    }
}
