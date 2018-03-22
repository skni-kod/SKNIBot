using System.Data.Entity;
using SKNIBot.Core.Database.Models;

namespace SKNIBot.Core.Database
{
    public class DatabaseContext : DbContext
    {
        public virtual IDbSet<MontyPythonVideo> MontyPythonVideos { get; set; }
        public virtual IDbSet<Joke> Jokes { get; set; }

        public DatabaseContext() : base("SQLiteConnectionString")
        {

        }
    }
}
