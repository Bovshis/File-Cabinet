using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileCabinetApp.Records;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService service;

        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        public int CreateRecord(RecordWithoutId recordWithoutId)
        {
            var ticks = TickCounterService.GetTicks(this.service.CreateRecord, recordWithoutId, out var id);
            Console.WriteLine($"Create method execution duration is {ticks} ticks.");
            return id;
        }

        public void Insert(FileCabinetRecord record)
        {
            var ticks = TickCounterService.GetTicks(this.service.Insert, record);
            Console.WriteLine($"Insert method execution duration is {ticks} ticks.");
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var ticks = TickCounterService.GetTicks(this.service.GetRecords, out var records);
            Console.WriteLine($"GetRecords method execution duration is {ticks} ticks.");
            return records;
        }

        public int GetStat()
        {
            var ticks = TickCounterService.GetTicks(this.service.GetStat, out var stat);
            Console.WriteLine($"GetStat method execution duration is {ticks} ticks.");
            return stat;
        }

        public IRecordValidator GetValidator()
        {
            var ticks = TickCounterService.GetTicks(this.service.GetValidator, out var validator);
            Console.WriteLine($"GetValidator method execution duration is {ticks} ticks.");
            return validator;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var ticks = TickCounterService.GetTicks(this.service.MakeSnapshot, out var snapshot);
            Console.WriteLine($"MakeSnapshot method execution duration is {ticks} ticks.");
            return snapshot;
        }

        public int Restore(FileCabinetServiceSnapshot fileCabinetServiceSnapshot)
        {
            var ticks = TickCounterService.GetTicks(this.service.Restore, fileCabinetServiceSnapshot, out var amount);
            Console.WriteLine($"MakeSnapshot method execution duration is {ticks} ticks.");
            return amount;
        }

        public IList<int> Delete(params (string key, string value)[] @where)
        {
            var ticks = TickCounterService.GetTicks(this.service.Delete, @where, out var list);
            Console.WriteLine($"Delete method execution duration is {ticks} ticks.");
            return list;
        }

        public IList<int> Update(IList<(string, string)> replaceList, IList<(string, string)> whereList)
        {
            var ticks = TickCounterService.GetTicks(this.service.Update, replaceList, whereList, out var list);
            Console.WriteLine($"Update method execution duration is {ticks} ticks.");
            return list;
        }

        public IList<FileCabinetRecord> GetRecordsWhere(IList<(string, string)> whereList)
        {
            var ticks = TickCounterService.GetTicks(this.service.GetRecordsWhere, whereList, out var list);
            Console.WriteLine($"GetRecordsWhere method execution duration is {ticks} ticks.");
            return list;
        }
    }
}
