using System;

namespace FileCabinetApp.CommandHandlers.ConcreteHandlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        private readonly Action<bool> exit;

        public ExitCommandHandler(Action<bool> exit)
        {
            this.exit = exit;
        }

        public override object Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                this.exit(false);
                return "Exiting an application...\n";
            }

            return base.Handle(request);
        }
    }
}
