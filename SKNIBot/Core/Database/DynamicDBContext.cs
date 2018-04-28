using System.Data.Entity;
using SKNIBot.Core.Database.Models;

namespace SKNIBot.Core.Database
{
    public class DynamicDBContext : DbContext
    {
        public virtual IDbSet<OnlineStats> OnlineStats { get; set; }

        public DynamicDBContext() : base("DynamicDatabaseConnectionString")
        {
#if DEBUG
            Database.Log = Console.Write;
#endif
        }
    }
}
