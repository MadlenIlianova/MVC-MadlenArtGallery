using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryData.Models
{
    public class Artwork : BaseModel.BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public bool SoldWithFrame { get; set; }
        public string Technique { get; set; }
        public string Signature { get; set; }
        public string CountryOfOrigin { get; set; }
        public int Year { get; set; }
        public string Condition { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public string Style { get; set; }
        public string Period { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? AuctionEndDate { get; set; }
        public bool IsAuctionActive { get; set; } = true;
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public ICollection<UserLikedArtwork> UserLikedArtworks { get; set; }
    }
}
