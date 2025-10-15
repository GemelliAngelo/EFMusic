namespace EFMusic.Domain.Entities
{
    public class Producer
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Song> Songs { get; set; } = [];
    }
}
