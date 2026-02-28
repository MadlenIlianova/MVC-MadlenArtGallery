using GalleryData.Models;
using MadlenArtGallery.Models;

namespace MadlenArtGallery.Services.Interfaces
{
    public interface IArtworkService
    {
        Task<ArtworkSearchViewModel> SearchArtworksAsync(string technique, string style,
            int? yearFrom, int? yearTo, int? minPrice, int? maxPrice, string sortBy);
        Task<Artwork?> GetArtworkWithDetailsAsync(int id);
        Task<(Bid? HighestBid, int BidsCount)> GetArtworkBidInfoAsync(int artworkId);
        Task<List<Author>> GetAllAuthorsAsync();
        Task<(bool Success, string Message)> CreateArtworkAsync(Artwork artwork, IFormFile imageFile);
        Task<Artwork?> GetArtworkByIdAsync(int id);
        Task<(bool Success, string Message)> UpdateArtworkAsync(int id, Artwork artwork, IFormFile imageFile);
        Task<(bool Success, string Message)> DeleteArtworkAsync(int id);
        Task<(bool Success, string Message)> SetAuctionTimerAsync(int artworkId, DateTime endDate);
    }
}
