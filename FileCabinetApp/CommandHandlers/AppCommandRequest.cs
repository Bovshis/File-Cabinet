namespace FileCabinetApp.CommandHandlers
{
    public class AppCommandRequest
    {
        public string Command { get; set; }

        public string Parameters { get; set; }

        public AppCommandRequest(string command, string property)
        {
            this.Command = command;
            this.Parameters = property;
        }
    }
}
