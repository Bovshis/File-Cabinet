using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FileCabinetApp.Records;
using FileCabinetApp.Validators;
using FileCabinetApp.Writers;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Logger for <see cref="IFileCabinetService"/>.
    /// </summary>
    public class ServiceLogger : IFileCabinetService
    {
        private readonly IFileCabinetService service;
        private readonly LogsWriter logWriter = new LogsWriter(new StreamWriter("history.log", false));

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">Service <see cref="IFileCabinetService"/>.</param>
        public ServiceLogger(IFileCabinetService service)
        {
            this.service = service;
        }

        public IRecordValidator GetValidator()
        {
            this.logWriter.Write(this.service.GetValidator, out var validator);
            return validator;
        }

        public int CreateRecord(RecordWithoutId recordWithoutId)
        {
            this.logWriter.Write(this.service.CreateRecord, recordWithoutId, out var id);
            return id;
        }

        public void Insert(FileCabinetRecord record)
        {
            this.service.Insert(record);
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.logWriter.Write(this.service.GetRecords, out var records);
            return records;
        }

        public int GetStat()
        {
            this.logWriter.Write(this.service.GetStat, out var stat);
            return stat;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.logWriter.Write(this.service.MakeSnapshot, out var fileCabinetService);
            return fileCabinetService;
        }

        public int Restore(FileCabinetServiceSnapshot fileCabinetServiceSnapshot)
        {
            this.logWriter.Write(this.service.Restore, fileCabinetServiceSnapshot, out var amount);
            return amount;
        }

        public IList<int> Delete(params (string key, string value)[] @where)
        {
            this.logWriter.Write(this.service.Delete, @where, out var list);
            return list;
        }

        public IList<int> Update(IList<(string, string)> replaceList, IList<(string, string)> whereList)
        {
            this.logWriter.Write(this.service.Update, replaceList, whereList, out var list);
            return list;
        }

        public IList<FileCabinetRecord> GetRecordsWhere(IList<(string, string)> whereList)
        {
            this.logWriter.Write(this.service.GetRecordsWhere, whereList, out var list);
            return list;
        }
    }
}
