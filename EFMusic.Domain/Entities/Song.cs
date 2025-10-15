namespace EFMusic.Domain.Entities
{
    public class Song
    {
        public required int Id { get; set; }
        public required int Title { get; set; }

        public ICollection<Artist> Artists { get; set; } = [];

        public int AlbumId { get; set; }
        public Album? Album { get; set; }
    }
}
