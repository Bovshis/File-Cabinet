namespace FileCabinetApp
{
    /// <summary>
    /// Inteface for validation parameters.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Method for validation parameters.
        /// </summary>
        /// <param name="recordWithoutId">parameters.</param>
        void ValidateParameters(RecordWithoutId recordWithoutId);
    }
}
