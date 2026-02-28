using GalleryData.Models;

namespace MadlenArtGallery.Services.Interfaces
{
    public interface IBidService
    {
        Task<List<Bid>> GetUserBidsAsync(string userId);
        Task<(bool Success, string Message)> PlaceBidAsync(int artworkId, int bidPrice, string userId, string userEmail);
        Task<List<Bid>> GetAllBidsAsync();
        Task<(bool Success, string Message, int ArtworkId)> DeleteBidAsync(int bidId);
        Task<List<Purchase>> GetUserWinsAsync(string userId);
    }
}
