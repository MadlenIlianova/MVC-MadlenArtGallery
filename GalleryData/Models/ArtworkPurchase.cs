using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryData.Models
{
    public class ArtworkPurchase
    {
        public int Id { get; set; }

        public int ArtworkId { get; set; }
        public Artwork Artwork { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ArtworkPrice { get; set; }

        public int? FrameSizeId { get; set; }
        public FrameSize FrameSize { get; set; }

        public int FramePrice { get; set; }
        public int TotalPrice { get; set; }
    }
}
