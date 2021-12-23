namespace FileCabinetApp.CommandHandlers
{
    public interface ICommandHandler
    {
        public ICommandHandler SetNext(ICommandHandler handler);

        public object Handle(AppCommandRequest request);
    }
}
