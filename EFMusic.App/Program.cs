using EFMusic.Domain.Entities;
using EFMusic.Infrastucture;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

                while (true)
                {
                    Console.WriteLine("1. Print Songs");
                    Console.WriteLine("2. Search Song");
                    Console.WriteLine("3. Add Song");
                    Console.WriteLine("4. Update Song");
                    Console.WriteLine("5. Delete Song");
                    Console.WriteLine("6. Exit");
                    var inputOption = Convert.ToInt16(Console.ReadLine());

                    switch (inputOption)
                    {
                        case 1:
                            Console.WriteLine(JsonSerializer.Serialize(GetSongs(), _options));
                            break;
                        case 2:
                            Console.WriteLine("Title, Artist or Genre: ");
                            var inputSearch = Console.ReadLine();

                            Console.WriteLine(JsonSerializer.Serialize(GetSongs(inputSearch ?? String.Empty), _options));
                            break;
                        case 3:
                            AddSong();
                            break;
                        case 4:
                            UpdateSong();
                            break;
                        case 5:
                            DeleteSong();
                            break;
                        case 6:
                            return;
                        default:
                            break;
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

        static List<Song> GetSongs(string input = "")
        {
            using var context = new MusicDbContext();

            var songs = context.Songs
                .Include(s => s.SongDetails)
                .Include(s => s.Artists)
                .Include(s => s.Album)
                .Include(s => s.Genres);

            if (!input.IsNullOrEmpty())
            {
                var filteredSongs = songs.Where(s => s.Title == input || s.Genres.Any(g => g.Name == input) || s.Artists.Any(a => a.Name == input));

                return [.. filteredSongs];
            }

            return [.. songs];
        }

        static void AddSong()
        {
            using var context = new MusicDbContext();
            var artists = context.Artists.ToList();
            var genres = context.Genres.ToList();
            var albums = context.Albums.ToList();
            var producers = context.Producers.ToList();

            Console.Write("Title: ");
            var inputTitle = Console.ReadLine() ?? "";

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

            Console.WriteLine("Available Producers: ");
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
        }

        static void UpdateSong()
        {
            using var context = new MusicDbContext();

            var songs = context.Songs
                .Include(s => s.SongDetails)
                .Include(s => s.Artists)
                .Include(s => s.Album)
                .Include(s => s.Genres);

            var artists = context.Artists.ToList();
            var genres = context.Genres.ToList();
            var albums = context.Albums.ToList();
            var producers = context.Producers.ToList();

            Console.WriteLine("Avilable songs:");
            foreach (var song in songs)
            {
                Console.WriteLine($"[{song.Id}] {song.Title} - {song.Album?.Name}");
            }
            Console.WriteLine("Song Id:");
            var inputSongId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Title: ");
            var inputTitle = Console.ReadLine() ?? "";

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

            Console.WriteLine("Available Producers: ");
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

            var inputSong = songs.First(s => s.Id == inputSongId);

            inputSong.Title = inputTitle;
            inputSong.SongDetails = new SongDetails() { Length = inputLength, ReleaseDate = inputReleaseDate, Producer = inputProducer};
            inputSong.Artists = inputArtists;
            inputSong.Album = inputAlbum;
            inputSong.Genres = inputGenres;

            //using var newContext = new MusicDbContext();

            context.Entry(inputSong).State = EntityState.Modified;
            //context.Update(inputSong);

            context.SaveChanges();


            //inputSong.ExecuteUpdate(setters => setters
            //    .SetProperty(s => s.Title, inputTitle ?? inputSong.First().Title)
            //    .SetProperty(s => s.SongDetails, new SongDetails() { Length = inputLength, ReleaseDate = inputReleaseDate, Producer = inputProducer ?? default })
            //    .SetProperty(s => s.Artists, inputArtists ?? inputSong.First().Artists)
            //    .SetProperty(s => s.Album, inputAlbum ?? inputSong.Firsit().Album)
            //    .SetProperty(s => s.Genres, inputGenres ?? inputSong.First().Genres));

        }

        static void DeleteSong()
        {
            using var context = new MusicDbContext();

            var songs = context.Songs;

            Console.WriteLine("Avilable songs:");
            foreach (var song in songs)
            {
                Console.WriteLine($"[{song.Id}] {song.Title} - {song.Album?.Name}");
            }
            Console.WriteLine("Song Id:");
            var inputSongId = Convert.ToInt32(Console.ReadLine());

            context.Songs.Where(s => s.Id == inputSongId).ExecuteDelete();
        }
    }
}
