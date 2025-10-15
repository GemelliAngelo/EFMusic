using System.ComponentModel.DataAnnotations;

namespace EFMusic.Domain.Entities
{
    public class SongDetails
    {
        [Key]
        public int SongId { get; set; }
        public required DateTime ReleaseDate { get; set; }
        public short Length { get; set; }

        public int? ProducerId { get; set; }
        public Producer? Producer { get; set; }
    }
}
