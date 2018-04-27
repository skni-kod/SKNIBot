using System;
using System.Data.Entity;
using SKNIBot.Core.Database.Models;

namespace SKNIBot.Core.Database
{
    public class StaticDBContext : DbContext
    {
        public virtual IDbSet<Command> Commands { get; set; }
        public virtual IDbSet<SimpleResponse> SimpleResponses { get; set; }

        public virtual IDbSet<Media> Media { get; set; }
        public virtual IDbSet<MediaName> MediaNames { get; set; }
        public virtual IDbSet<MediaCategory> MediaCategories { get; set; }

        public virtual IDbSet<HangmanCategory> HangmanCategories { get; set; }
        public virtual IDbSet<HangmanWord> HangmanWords { get; set; }

        public StaticDBContext() : base("StaticDatabaseConnectionString")
        {
#if DEBUG
            Database.Log = Console.Write;
#endif
        }
    }
}
