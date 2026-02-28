using GalleryData.Models;
using MadlenArtGallery.Models;

namespace MadlenArtGallery.Services.Interfaces
{
    public interface IAdminService
    {
        Task<DashboardViewModel> GetDashboardStatsAsync();
        Task<List<Artwork>> GetAllArtworksWithAuthorsAsync();
        Task<List<UserWithRoleViewModel>> GetAllUsersWithRolesAsync();
        Task<(bool Success, string Message)> MakeUserAdminAsync(string userId);
        Task<(bool Success, string Message)> RemoveAdminRoleAsync(string userId);
        Task<(bool Success, string Message)> DeleteUserAsync(string userId);
        Task<List<Purchase>> GetAuctionWinnersAsync();
    }
}

