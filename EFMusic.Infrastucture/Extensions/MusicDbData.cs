using EFMusic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFMusic.Infrastucture.Extensions
{
    public static class EFExtensions
    {
        public static void SeedData(this ModelBuilder builder)
        {
            Genre[] genres = [
                new() { Id = 1, Name = "Hip Hop"},
                new() { Id = 2, Name = "Electronic"},
                new() { Id = 3, Name = "Jazz"},
                new() { Id = 4, Name = "Rock"},
                new() { Id = 5, Name = "Classic"},
                new() { Id = 6, Name = "Reggae"},
                new() { Id = 7, Name = "Rap"}
                ];

            builder.Entity<Genre>().HasData(genres);

            Artist[] artists = [
                new() { Id = 1, Name = "Travis Scott"},
                new() { Id = 2, Name = "Rihanna"},
                new() { Id = 3, Name = "Bad Bunny"},
                new() { Id = 4, Name = "Linkin Park"},
                new() { Id = 5, Name = "Adele"},
                new() { Id = 6, Name = "Miles Davis"},
                new() { Id = 7, Name = "Daft Punk"}
                ];

            builder.Entity<Artist>().HasData(artists);

            Producer[] producers = [
                new() { Id = 1, Name = "Rick Rubin"},
                new() { Id = 2, Name = "Calvin Harris"},
                new() { Id = 3, Name = "Metro Boomin"},
                new() { Id = 4, Name = "Max Martin"},
                new() { Id = 5, Name = "Brian Eno"}
                ];

            builder.Entity<Producer>().HasData(producers);

            Album[] albums = [
                new() { Id = 1, Name = "Astroworld", ReleaseDate = Convert.ToDateTime("2018-1-1")},
                new() { Id = 2, Name = "Anti", ReleaseDate = Convert.ToDateTime("2016-1-1")},
                new() { Id = 3, Name = "Un Verano Sin Ti", ReleaseDate = Convert.ToDateTime("2022-1-1")},
                new() { Id = 4, Name = "Hybrid Theory", ReleaseDate = Convert.ToDateTime("2000-1-1")},
                new() { Id = 5, Name = "21", ReleaseDate = Convert.ToDateTime("2011-1-1")},
                new() { Id = 6, Name = "Kind of Blue", ReleaseDate = Convert.ToDateTime("1959-1-1")},
                new() { Id = 7, Name = "Discovery", ReleaseDate = Convert.ToDateTime("2001-1-1")}
                ];

            builder.Entity<Album>().HasData(albums);

        }
    }
}
