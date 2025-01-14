using Microsoft.EntityFrameworkCore;
using Spot.Data.Entities;

namespace Spot.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<SongTag> SongTags { get; set; }

        public DbSet<SongSongTagMap> SongSongTagMaps { get; set; }

        public DbSet<SongTagCategory> SongTagCategories { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Song>()
                .HasMany(e => e.SongTags)
                .WithMany(e => e.Songs)
                .UsingEntity<SongSongTagMap>();

            modelBuilder.Entity<Song>()
                .Navigation(e => e.SongTags)
                .AutoInclude();

            modelBuilder.Entity<Song>()
                .Navigation(e => e.SongSongTagMaps)
                .AutoInclude();

            modelBuilder.Entity<SongTag>()
                .Navigation(e => e.SongSongTagMaps)
                .AutoInclude();

            // cant have both
            //modelBuilder.Entity<SongTag>()
            //  .Navigation(e => e.Songs)
            //  .AutoInclude();
        }
    }
}
