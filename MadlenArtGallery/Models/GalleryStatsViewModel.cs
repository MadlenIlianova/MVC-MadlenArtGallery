namespace MadlenArtGallery.Models
{
    public class GalleryStatsViewModel
    {
        public int TotalArtworks { get; set; }
        public int ActiveAuctions { get; set; }
        public int TotalAuthors { get; set; }
        public AuthorStatViewModel AuthorOfMonth { get; set; }
    }
}
