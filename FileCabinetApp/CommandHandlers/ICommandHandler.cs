namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Commands handler.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Link this handler with next handler.
        /// </summary>
        /// <param name="handler">Next handler.</param>
        /// <returns>Next handler for chain linking.</returns>
        public ICommandHandler SetNext(ICommandHandler handler);

        /// <summary>
        /// Execute request.
        /// </summary>
        /// <param name="request">Request for execution that contain command and parameters.</param>
        /// <returns>Execution result message.</returns>
        public object Handle(AppCommandRequest request);
    }
}
