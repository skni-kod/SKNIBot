using Microsoft.EntityFrameworkCore;
using SKNIBot.Core.Database.Models;

namespace SKNIBot.Core.Database
{
    public class StaticDBContext : DbContext
    {
        public virtual DbSet<Command> Commands { get; set; }
        public virtual DbSet<SimpleResponse> SimpleResponses { get; set; }

        public virtual DbSet<Media> Media { get; set; }
        public virtual DbSet<MediaName> MediaNames { get; set; }
        public virtual DbSet<MediaCategory> MediaCategories { get; set; }

        public virtual DbSet<HangmanCategory> HangmanCategories { get; set; }
        public virtual DbSet<HangmanWord> HangmanWords { get; set; }

        public virtual DbSet<SpotifyEarWorm> SpotifyEarWorms { get; set; }
        public virtual DbSet<JavaThing> JavaThings { get; set; }

        public StaticDBContext() : base(GetOptions("Data Source=StaticDatabase.sqlite"))
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
