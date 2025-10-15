using EFMusic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFMusic.Infrastucture
{
    public class MusicDbContext : DbContext
    {
        public DbSet<Song> Songs { get; init; }
        public DbSet<SongDetails> SongsDetails { get; init; }
        public DbSet<Artist> Artist { get; init; }
        public DbSet<Producer> Producers { get; init; }
        public DbSet<Album> Albums { get; init; }
    }
}
