using GalleryData.Models;

namespace MadlenArtGallery.Models
{
    public class HomeStatsViewModel
    {
        public int TotalArtworks { get; set; }
        public int TotalAuthors { get; set; }
        public int ActiveAuctions { get; set; }
        public string MostPopularTechnique { get; set; } = string.Empty;
        public Author? ArtistOfTheMonth { get; set; } 
        public int ArtistOfTheMonthArtworksCount { get; set; }

        public List<Artwork> FeaturedArtworks { get; set; } = new List<Artwork>();
        public List<Artwork> EndingSoonArtworks { get; set; } = new List<Artwork>();
    }
}
