using System;

namespace FileCabinetApp
{
    /// <summary>
    /// File cabinet with default settings.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        {
        }
    }
}
