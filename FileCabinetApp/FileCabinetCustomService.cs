using System;

namespace FileCabinetApp
{
    /// <summary>
    /// File cabinet with custom settings.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        public FileCabinetCustomService()
            : base(new DefaultValidator())
        {
        }
    }
}
