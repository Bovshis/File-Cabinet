namespace FileCabinetApp
{
    public interface IRecordValidator
    {
        void ValidateParameters(RecordWithoutId recordWithoutId);
    }
}
