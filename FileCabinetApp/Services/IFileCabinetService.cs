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
        /// delete record with given parameter.
        /// </summary>
        /// <param name="where">parameters for deleting.</param>
        /// <returns>List of deleted records.</returns>
        public IList<int> Delete(params (string key, string value)[] where);

        /// <summary>
        /// Update records.
        /// </summary>
        /// <param name="replaceList">List with data for updating record.</param>
        /// <param name="whereList">List with data for finding records.</param>
        /// <returns>list of indices updated records.</returns>
        public IList<int> Update(IList<(string, string)> replaceList, IList<(string, string)> whereList);

        /// <summary>
        /// Get records with parameters that contain in whereList.
        /// </summary>
        /// <param name="whereList">contain parameters for searching.</param>
        /// <returns>List of records.</returns>
        public IList<FileCabinetRecord> GetRecordsWhere(IList<(string, string)> whereList);
    }
}
