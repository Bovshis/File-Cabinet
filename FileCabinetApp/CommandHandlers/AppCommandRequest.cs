namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command request.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="parameters">Parameters.</param>
        public AppCommandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets or sets command.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets parameters.
        /// </summary>
        public string Parameters { get; set; }
    }
}
