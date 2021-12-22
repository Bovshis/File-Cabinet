using System;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        public virtual object Handle(AppCommandRequest request)
        {
            return this.nextHandler?.Handle(request);
        }

        public ICommandHandler SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
            return handler;
        }
    }
}
