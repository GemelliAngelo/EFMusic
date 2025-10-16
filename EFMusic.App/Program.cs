using EFMusic.Domain.Entities;
using EFMusic.Infrastucture;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EFMusic.App
{
    internal class Program
    {
        private static readonly JsonSerializerOptions _options = new() { WriteIndented = true, ReferenceHandler = ReferenceHandler.IgnoreCycles };

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information($"App info: {Assembly.GetExecutingAssembly().FullName}");

                using var context = new MusicDbContext();
                var albums = context.Albums.ToList();
                var artists = context.Artists.ToList();
                var producers = context.Producers.ToList();
                var genres = context.Genres.ToList();

                while (true)
                {
                    Console.WriteLine("1. Print Songs");
                    Console.WriteLine("2. Search Song");
                    Console.WriteLine("3. Add Song");
                    Console.WriteLine("4. Exit");
                    var inputOption = Convert.ToInt16(Console.ReadLine());

                    switch (inputOption)
                    {
                        case 1:
                            var songs = context.Songs
                                .Include(s => s.SongDetails)
                                .Include(s => s.Artists)
                                .Include(s => s.Album)
                                .Include(s => s.Genres);

                            Console.WriteLine(JsonSerializer.Serialize(songs, _options));
                            break;
                        case 2:
                            Console.WriteLine("Title, Artist or Genre: ");
                            var inputSearch = Console.ReadLine();
                            var searchSongs = context.Songs.Where(s => s.Title == inputSearch || s.Genres.Contains(new Genre() { Name = inputSearch }) || s.Artists.Contains(new Artist() { Name = inputSearch }))
                                .Include(s => s.SongDetails)
                                .Include(s => s.Artists)
                                .Include(s => s.Album)
                                .Include(s => s.Genres);

                            Console.WriteLine(JsonSerializer.Serialize(searchSongs, _options));
                            break;
                        case 3:
                            Console.Write("Title: ");
                            var inputTitle = Console.ReadLine();

                            Console.WriteLine("How many Artists: ");
                            var inputArtistsCount = Convert.ToInt16(Console.ReadLine());

                            Console.WriteLine("Avilable Artists: ");
                            foreach (var artist in artists)
                            {
                                Console.WriteLine($"[{artist.Id}] {artist.Name}");
                            }
                            ICollection<int> inputArtistsIds = [];

                            for (int i = 0; i < inputArtistsCount; i++)
                            {
                                Console.Write("Artist Id: ");
                                inputArtistsIds.Add(Convert.ToInt32(Console.ReadLine()));
                            }
                            var inputArtists = context.Artists.Where(a => inputArtistsIds.Contains(a.Id)).ToList();

                            Console.WriteLine("How many Genres: ");
                            var inputGenresCount = Convert.ToInt16(Console.ReadLine());
                            foreach (var genre in genres)
                            {
                                Console.WriteLine($"[{genre.Id}] {genre.Name}");
                            }
                            ICollection<int> inputGenresIds = [];

                            for (int i = 0; i < inputGenresCount; i++)
                            {
                                Console.Write("Genres Id: ");
                                inputGenresIds.Add(Convert.ToInt32(Console.ReadLine()));
                            }
                            var inputGenres = context.Genres.Where(g => inputGenresIds.Contains(g.Id)).ToList();

                            Console.Write("Length: ");
                            var inputLength = Convert.ToInt16(Console.ReadLine());

                            Console.Write("Available Producers: ");
                            foreach (var producer in producers)
                            {
                                Console.WriteLine($"[{producer.Id}] {producer.Name}");
                            }
                            Console.Write("Producer Id:");
                            var inputProducerId = Convert.ToInt16(Console.ReadLine());
                            var inputProducer = context.Producers.Where(p => p.Id == inputProducerId).First();

                            Console.Write("Release Date: ");
                            var inputReleaseDate = Convert.ToDateTime(Console.ReadLine());

                            Console.WriteLine("Available Albums: ");
                            foreach (var album in albums)
                            {
                                Console.WriteLine($"[{album.Id}] {album.Name}");
                            }

                            Console.Write("Album Id: ");
                            var inputAlbumId = Convert.ToInt32(Console.ReadLine());
                            var inputAlbum = context.Albums.Where(a => a.Id == inputAlbumId).First();

                            var song = new Song()
                            {
                                Title = inputTitle,
                                SongDetails = new SongDetails() { Length = inputLength, ReleaseDate = inputReleaseDate, Producer = inputProducer },
                                Artists = inputArtists,
                                Album = inputAlbum,
                                Genres = inputGenres
                            };

                            context.Songs.Add(song);
                            context.SaveChanges();

                            break;
                        case 4:
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
