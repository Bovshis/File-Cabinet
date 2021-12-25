using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileCabinetApp.Records;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// File cabinet.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Get validator.
        /// </summary>
        /// <returns>validator.</returns>
        public IRecordValidator GetValidator();

        /// <summary>
        /// Create record.
        /// </summary>
        /// <param name="recordWithoutId">record data.</param>
        /// <returns>record id.</returns>
        public int CreateRecord(RecordWithoutId recordWithoutId);

        /// <summary>
        /// Insert record.
        /// </summary>
        /// <param name="record">record.</param>
        public void Insert(FileCabinetRecord record);

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
        /// <param name="recordWithoutId">record data.</param>
        /// <returns>is record edited.</returns>
        public bool EditRecord(int id, RecordWithoutId recordWithoutId);

        /// <summary>
        /// Find list of the records by first name.
        /// </summary>
        /// <param name="firstName">value to search.</param>
        /// <returns>Searched records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Find list of the records by last Name.
        /// </summary>
        /// <param name="lastName">value to search.</param>
        /// <returns>searched records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Find list of the records by date Of Birth.
        /// </summary>
        /// <param name="dateOfBirth">value to search.</param>
        /// <returns>Searched records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        /// <summary>
        /// Make snapshot.
        /// </summary>
        /// <returns>FileCabinetService snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Restore records.
        /// </summary>
        /// <param name="fileCabinetServiceSnapshot">snapshot that contains data.</param>
        /// <returns>amount imported records.</returns>
        public int Restore(FileCabinetServiceSnapshot fileCabinetServiceSnapshot);

        /// <summary>
        /// remove record.
        /// </summary>
        /// <param name="id">id removed record.</param>
        public void Remove(int id);
    }
}
