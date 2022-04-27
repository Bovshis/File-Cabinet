using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using FileCabinetApp.DatabaseContexts;
using FileCabinetApp.Records;
using FileCabinetApp.Validators;
using Microsoft.EntityFrameworkCore;

namespace FileCabinetApp.Services
{
    public class FileCabinetDatabaseService : IFileCabinetService
    {
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetDatabaseService"/> class.
        /// </summary>
        /// <param name="validator">validator.</param>
        public FileCabinetDatabaseService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Create record, adds to list and dictionaries.
        /// </summary>
        /// <param name="recordWithoutId">record data.</param>
        /// <returns>record number.</returns>
        public int CreateRecord(RecordWithoutId recordWithoutId)
        {
            using FileCabinetServiceContext db = new FileCabinetServiceContext();
            var lastRecord = db.Records.OrderBy(x => x.Id).LastOrDefault();
            if (lastRecord != null)
            {
                var nextId = lastRecord.Id + 1;
                db.Records.Add(new FileCabinetRecord
                {
                    Id = nextId,
                    FirstName = recordWithoutId.FirstName,
                    LastName = recordWithoutId.LastName,
                    Height = recordWithoutId.Height,
                    Weight = recordWithoutId.Weight,
                    FavoriteCharacter = recordWithoutId.FavoriteCharacter,
                });
                db.SaveChanges();
                return nextId;
            }

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Records.Add(new FileCabinetRecord
            {
                FirstName = recordWithoutId.FirstName,
                LastName = recordWithoutId.LastName,
                Height = recordWithoutId.Height,
                Weight = recordWithoutId.Weight,
                FavoriteCharacter = recordWithoutId.FavoriteCharacter,
            });
            db.SaveChanges();
            return 1;
        }

        /// <summary>
        /// delete record with given parameter.
        /// </summary>
        /// <param name="where">parameters for deleting.</param>
        /// <returns>List of deleted records.</returns>
        public IList<int> Delete(params (string key, string value)[] where)
        {
            using FileCabinetServiceContext db = new FileCabinetServiceContext();
            var records = db.Records.Where(x => this.IsSatisfy(x, where.ToList()));
            foreach (var record in records)
            {
                db.Records.Remove(record);
            }

            db.SaveChanges();
            return records.Select(x => x.Id).ToList();
        }

        /// <summary>
        /// Get records from the file cabinet list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            using FileCabinetServiceContext db = new FileCabinetServiceContext();
            return new ReadOnlyCollection<FileCabinetRecord>(db.Records.ToList());
        }

        /// <summary>
        /// Get records with parameters that contain in whereList.
        /// </summary>
        /// <param name="whereList">contain parameters for searching.</param>
        /// <returns>List of records.</returns>
        public IList<FileCabinetRecord> GetRecordsWhere(IList<(string, string)> whereList)
        {
            using FileCabinetServiceContext db = new FileCabinetServiceContext();
            return db.Records.Where(x => this.IsSatisfy(x, whereList)).ToList();
        }

        /// <summary>
        /// Get count records from the file cabinet list..
        /// </summary>
        /// <returns>count of records.</returns>
        public int GetStat()
        {
            using FileCabinetServiceContext db = new FileCabinetServiceContext();
            return db.Records.Count();
        }

        /// <summary>
        /// Get validator.
        /// </summary>
        /// <returns>validator.</returns>
        public IRecordValidator GetValidator()
        {
            return this.validator;
        }

        /// <summary>
        /// Insert record.
        /// </summary>
        /// <param name="record">record.</param>
        public void Insert(FileCabinetRecord record)
        {
            using var db = new FileCabinetServiceContext();
            var oldRecord = db.Records.Find(record.Id);
            if (oldRecord == null)
            {
                db.Records.Add(record);
            }
            else
            {
                oldRecord.FirstName = record.FirstName;
                oldRecord.LastName = record.LastName;
                oldRecord.Height = record.Height;
                oldRecord.Weight = record.Weight;
                oldRecord.FavoriteCharacter = record.FavoriteCharacter;
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Make snapshot.
        /// </summary>
        /// <returns>FileCabinetService snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords());
        }

        /// <summary>
        /// Restore records.
        /// </summary>
        /// <param name="fileCabinetServiceSnapshot">snapshot that contains data.</param>
        /// <returns>amount imported records.</returns>
        public int Restore(FileCabinetServiceSnapshot fileCabinetServiceSnapshot)
        {
            var amount = 0;
            foreach (var record in fileCabinetServiceSnapshot.Records)
            {
                if (this.validator.ValidateParameter(record).Item1)
                {
                    amount++;
                    this.Insert(record);
                }
                else
                {
                    Console.WriteLine($"Validation failed: {record.Id}.");
                }
            }

            return amount;
        }

        public IList<int> Update(IList<(string, string)> replaceList, IList<(string, string)> whereList)
        {
            using FileCabinetServiceContext db = new FileCabinetServiceContext();
            var records = db.Records.Where(x => this.IsSatisfy(x, whereList));
            foreach (var record in records)
            {
                this.UpdateRecord(record, replaceList);
            }

            db.SaveChanges();
            return records.Select(x => x.Id).ToList();
        }

        private void UpdateRecord(FileCabinetRecord record, IList<(string, string)> replaceList)
        {
            foreach (var query in replaceList)
            {
                var (key, value) = query;
                switch (key.ToLower(CultureInfo.InvariantCulture))
                {
                    case "firstname":
                        record.FirstName = value;
                        break;
                    case "lastname":
                        record.LastName = value;
                        break;
                    case "dateofbirth":
                        record.DateOfBirth = Convert.ToDateTime(value);
                        break;
                    case "height":
                        record.Height = Convert.ToInt16(value);
                        break;

                    case "weight":
                        record.Weight = Convert.ToDecimal(value);
                        break;
                    case "favoritecharacter":
                        record.FavoriteCharacter = Convert.ToChar(value);
                        break;
                    default:
                        throw new ArgumentException($"There is no key: {key}");
                }
            }
        }

        private bool IsSatisfy(FileCabinetRecord record, IList<(string, string)> whereList)
        {
            foreach (var query in whereList)
            {
                var (key, value) = query;
                switch (key.ToLower(CultureInfo.InvariantCulture))
                {
                    case "firstname":
                        if (record.FirstName != value)
                        {
                            return false;
                        }

                        break;
                    case "lastname":
                        if (record.LastName != value)
                        {
                            return false;
                        }

                        break;
                    case "dateofbirth":
                        if (record.DateOfBirth != Convert.ToDateTime(value))
                        {
                            return false;
                        }

                        break;
                    case "height":
                        if (record.Height != Convert.ToInt16(value))
                        {
                            return false;
                        }

                        break;
                    case "weight":
                        if (record.Weight != Convert.ToDecimal(value))
                        {
                            return false;
                        }

                        break;
                    case "favoritecharacter":
                        if (record.FavoriteCharacter != Convert.ToChar(value))
                        {
                            return false;
                        }

                        break;
                    default:
                        throw new ArgumentException($"There is no key: {key}");
                }
            }

            return true;
        }
    }
}
