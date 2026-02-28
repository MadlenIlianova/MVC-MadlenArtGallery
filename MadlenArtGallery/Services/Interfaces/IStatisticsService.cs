using MadlenArtGallery.Models;

namespace MadlenArtGallery.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<GalleryStatsViewModel> GetGalleryStatsAsync();
        Task<AuthorStatViewModel> GetAuthorOfMonthAsync();
    }
}
