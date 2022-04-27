using FileCabinetApp.Records;
using Microsoft.EntityFrameworkCore;

namespace FileCabinetApp.DatabaseContexts
{
    public class FileCabinetServiceContext : DbContext
    {
        public FileCabinetServiceContext()
        {
            this.Database.EnsureCreated();
        }

        public DbSet<FileCabinetRecord> Records { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=cabinet-records.db");
        }
    }
}
