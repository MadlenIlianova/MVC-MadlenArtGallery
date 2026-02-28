using GalleryData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MadlenArtGallery.Models
{
    public class ArtworkSearchViewModel
    {    
        public string Technique { get; set; }
        public string Signature { get; set; }
        public string Country { get; set; }
        public string Style { get; set; }
        public string Condition { get; set; }

        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }

        public int? MinHeight { get; set; }
        public int? MinWidth { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string SortBy { get; set; }
        public List<Artwork> Results { get; set; } = new();
    }
}
