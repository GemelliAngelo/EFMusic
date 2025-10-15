using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMusic.Domain.Entities
{
    public class Album
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime ReleaseDate { get; set; }

        public ICollection<Song> Songs { get; set; } = [];
        public ICollection<Artist> Artists { get; set; } = [];
    }
}
