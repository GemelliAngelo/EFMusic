namespace EFMusic.Domain.Entities
{
    public class Artist
    {
        public required int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Song> Songs { get; set; } = [];

        public ICollection<Album> Albums { get; set; } = [];
    }
}
