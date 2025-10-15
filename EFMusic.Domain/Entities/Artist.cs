namespace EFMusic.Domain.Entities
{
    public class Artist
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required short Age { get; set; }
    }
}
