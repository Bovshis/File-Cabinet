using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FileCabinetApp.Records;
using FileCabinetApp.Validators;
using FileCabinetApp.Writers;

namespace FileCabinetApp.Services
{
    public class ServiceLogger : IFileCabinetService
    {
        private readonly IFileCabinetService service;
        private readonly LogsWriter logWriter = new LogsWriter(new StreamWriter("history.log", false));

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

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.logWriter.Write(this.service.FindByFirstName, firstName, out var records);
            return records;
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.logWriter.Write(this.service.FindByLastName, lastName, out var records);
            return records;
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            this.logWriter.Write(this.service.FindByDateOfBirth, dateOfBirth, out var records);
            return records;
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
    }
}
