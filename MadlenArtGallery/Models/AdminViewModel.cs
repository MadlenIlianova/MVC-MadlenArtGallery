using GalleryData.Models;

namespace MadlenArtGallery.Models
{
    public class AdminViewModel
    {
        public int TotalArtworks { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBids { get; set; }
        public int TotalPurchases { get; set; }

        public List<Artwork> RecentArtworks { get; set; } = new();
        public List<Bid> RecentBids { get; set; } = new();
        public List<Artwork> TopPricedArtworks { get; set; } = new();
    }

    public class RolesViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}

