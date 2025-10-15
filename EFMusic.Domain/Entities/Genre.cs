namespace EFMusic.Domain.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<Song>? Songs { get; set; } = [];
    }
}
