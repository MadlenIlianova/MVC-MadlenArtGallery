using GalleryData.Models;

namespace MadlenArtGallery.Models
{
    public class AuthorStatViewModel
    {
        public string AuthorName { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public Artwork FeaturedArtwork { get; set; }
    }
}
