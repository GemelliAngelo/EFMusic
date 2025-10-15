using System.ComponentModel.DataAnnotations.Schema;

namespace EFMusic.Domain.Entities
{
    public class SongDetails
    {
        public int SongId { get; set; }
        public Song? Song { get; set; }

        public required DateTime ReleaseDate { get; set; }
        public short Length { get; set; }

        public int? ProducerId { get; set; }
        public Producer? Producer { get; set; }
    }
}
