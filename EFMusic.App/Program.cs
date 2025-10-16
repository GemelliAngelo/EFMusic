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

                            Console.WriteLine(JsonSerializer.Serialize(songs,_options));
                            break;
                        case 2:
                            Console.WriteLine("Title, Artist or Genre: ");
                            var inputSearch = Console.ReadLine();
                            var searchSongs = context.Songs.Where(s => s.Title == inputSearch || s.Genres.Contains(new Genre() { Name = inputSearch }) || s.Artists.Contains(new Artist() { Name = inputSearch }))
                                .Include(s => s.SongDetails)
                                .Include(s => s.Artists)
                                .Include(s => s.Album)
                                .Include(s => s.Genres);

                            Console.WriteLine(JsonSerializer.Serialize(searchSongs,_options));
                            break;
                        case 3:
                            Console.Write("Title: ");
                            var inputTitle = Console.ReadLine();
                            Console.Write("Artist: ");
                            var inputArtist = Console.ReadLine();
                            Console.Write("Genres: ");
                            var inputGenres = Console.ReadLine();
                            Console.Write("Length: ");
                            var inputLength = Console.ReadLine();
                            Console.Write("Producer: ");
                            var inputProducer = Console.ReadLine();
                            Console.Write("Release Date: ");
                            var inputReleaseDate = Console.ReadLine();
                            Console.Write("Album: ");
                            var inputAlbum = Console.ReadLine();
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
