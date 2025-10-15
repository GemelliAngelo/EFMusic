using EFMusic.Domain.Entities;
using EFMusic.Infrastucture.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EFMusic.Infrastucture
{
    public class MusicDbContext : DbContext
    {
        public DbSet<Song> Songs { get; init; }
        public DbSet<SongDetails> SongsDetails { get; init; }
        public DbSet<Artist> Artists { get; init; }
        public DbSet<Producer> Producers { get; init; }
        public DbSet<Album> Albums { get; init; }
        public DbSet<Genre> Genres { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
                builder.UseSqlServer("Server=localhost;Database=EFMusicDb;User Id=sa;Password=bitspa.1;TrustServerCertificate=true");

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SongDetails>(entity =>
            {
                entity.HasKey(d => d.SongId);
                entity.HasOne(d => d.Song).WithOne(s => s.SongDetails).OnDelete(DeleteBehavior.Cascade);
                entity.Property(d => d.ReleaseDate).HasDefaultValueSql("getdate()");
            });

            builder.Entity<Album>(entity =>
            {
                entity.Property(a => a.ReleaseDate).HasDefaultValueSql("getdate()");
            });

            #region Data seeding

            builder.SeedData();

            #endregion
        }
    }
}
