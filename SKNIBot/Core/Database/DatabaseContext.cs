using System.Data.Entity;

namespace SKNIBot.Core.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("SQLiteConnectionString")
        {

        }
    }
}
