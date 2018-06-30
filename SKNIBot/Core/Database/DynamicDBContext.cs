using Microsoft.EntityFrameworkCore;
using SKNIBot.Core.Database.Models;

namespace SKNIBot.Core.Database
{
    public class DynamicDBContext : DbContext
    {
        public virtual DbSet<OnlineStats> OnlineStats { get; set; }

        public DynamicDBContext() : base(GetOptions("Data Source=DynamicDatabase.sqlite"))
        {/*
#if DEBUG
            Database.Log = System.Console.Write;
#endif*/
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder(), connectionString).Options;
        }
    }
}
