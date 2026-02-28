using GalleryData.Models;

namespace MadlenArtGallery.Models
{
    public class FrameSelectionViewModel
    {
        public int PurchaseId { get; set; }
        public string ArtworkTitle { get; set; } = string.Empty;
        public string ArtworkArtist { get; set; } = string.Empty;
        public string ArtworkImageUrl { get; set; } = string.Empty;
        public decimal ArtworkPrice { get; set; }

        public List<Frame> AvailableFrames { get; set; } = new List<Frame>();
    }
}
