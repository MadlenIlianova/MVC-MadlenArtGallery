using GalleryData.Models;

namespace MadlenArtGallery.Services.Interfaces
{
    public interface IFrameService
    {
        Task<List<Frame>> GetAllFramesWithSizesAsync();
    }
}
