using System;
using FileCabinetApp.Records;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validation parameters.
    /// </summary>
    public interface IRecordValidator
    {
        Tuple<bool, string> ValidateParameter(FileCabinetRecord parameters);
    }
}
