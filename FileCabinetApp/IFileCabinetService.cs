using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// File cabinet.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Create record.
        /// </summary>
        /// <returns>record id.</returns>
        public int CreateRecord();

        /// <summary>
        /// Get list of the records.
        /// </summary>
        /// <returns>lis of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Get count records.
        /// </summary>
        /// <returns>count records.</returns>
        public int GetStat();

        /// <summary>
        /// Edit record.
        /// </summary>
        /// <param name="id">number of the edited record.</param>
        public void EditRecord(int id);

        /// <summary>
        /// Find list of the records by first name.
        /// </summary>
        /// <param name="firstName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Find list of the records by last Name.
        /// </summary>
        /// <param name="lastName">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Find list of the records by date Of Birth.
        /// </summary>
        /// <param name="dateOfBirth">value to search.</param>
        /// <returns>List of the searched records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        /// <summary>
        /// Make Snapshot FileCabinetService.
        /// </summary>
        /// <returns>Snapshot FileCabinetService.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();
    }
}
