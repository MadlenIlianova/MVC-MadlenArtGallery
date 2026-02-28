using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryData.Models
{
    public class Author : BaseModel.BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Artwork> Artworks { get; set; }
    }
}
