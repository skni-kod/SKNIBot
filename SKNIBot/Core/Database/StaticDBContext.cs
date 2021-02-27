using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SKNIBot.Core.Database.Logger;
using SKNIBot.Core.Database.Models.StaticDB;

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

        public virtual DbSet<Role> Roles { get; set; }

        public StaticDBContext() : base(GetOptions("Data Source=StaticDatabase.sqlite"))
        {

        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder(), connectionString).Options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.UseLoggerFactory(new DbLoggerFactory());
#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
}
