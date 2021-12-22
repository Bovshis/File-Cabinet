using System;
using System.Text;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        public ListCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("list", StringComparison.InvariantCultureIgnoreCase))
            {
                var recordsList = this.service.GetRecords();
                var sb = new StringBuilder();

                foreach (var record in recordsList)
                {
                    sb.Append(record).Append('\n');
                }

                return sb.ToString();
            }

            return base.Handle(request);
        }
    }
}
