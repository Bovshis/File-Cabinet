namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Commands handler.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        /// <summary>
        /// Execute request.
        /// </summary>
        /// <param name="request">Request for execution that contain command and parameters.</param>
        /// <returns>Execution result message.</returns>
        public virtual object Handle(AppCommandRequest request)
        {
            return this.nextHandler?.Handle(request);
        }

        /// <summary>
        /// Link this handler with next handler.
        /// </summary>
        /// <param name="handler">Next handler.</param>
        /// <returns>Next handler for chain linking.</returns>
        public ICommandHandler SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
            return handler;
        }
    }
}
