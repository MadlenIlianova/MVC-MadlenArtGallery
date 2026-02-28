using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryData.Models
{
    public class Bid : BaseModel.BaseEntity
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ArtworkId { get; set; }
        public Artwork Artwork { get; set; }
    }
}
