using GalleryData.Models;
using GalleryData;
using MadlenArtGallery.Models;
using MadlenArtGallery.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MadlenArtGallery.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public ArtworkService(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<ArtworkSearchViewModel> SearchArtworksAsync(string technique, string style,
            int? yearFrom, int? yearTo, int? minPrice, int? maxPrice, string sortBy)
        {
            var query = _context.Artworks
                .Include(a => a.Author)
                .Include(a => a.Bids)
                .AsQueryable();

            if (!string.IsNullOrEmpty(technique))
                query = query.Where(a => a.Technique == technique);

            if (!string.IsNullOrEmpty(style))
                query = query.Where(a => a.Style == style);

            if (yearFrom.HasValue)
                query = query.Where(a => a.Year >= yearFrom.Value);

            if (yearTo.HasValue)
                query = query.Where(a => a.Year <= yearTo.Value);

            if (minPrice.HasValue)
                query = query.Where(a => a.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(a => a.Price <= maxPrice.Value);

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(a => a.Price),
                "price_desc" => query.OrderByDescending(a => a.Price),
                "year_asc" => query.OrderBy(a => a.Year),
                "year_desc" => query.OrderByDescending(a => a.Year),
                _ => query.OrderByDescending(a => a.CreatedAt)
            };

            return new ArtworkSearchViewModel
            {
                Technique = technique,
                Style = style,
                YearFrom = yearFrom,
                YearTo = yearTo,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortBy = sortBy,
                Results = await query.ToListAsync()
            };
        }

        public async Task<Artwork?> GetArtworkWithDetailsAsync(int id)
        {
            return await _context.Artworks
                .Include(a => a.Author)
                .Include(a => a.Bids)
                    .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(Bid? HighestBid, int BidsCount)> GetArtworkBidInfoAsync(int artworkId)
        {
            var artwork = await _context.Artworks
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == artworkId);

            if (artwork == null)
                return (null, 0);

            var highestBid = artwork.Bids
                .OrderByDescending(b => b.Price)
                .FirstOrDefault();

            return (highestBid, artwork.Bids.Count);
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<(bool Success, string Message)> CreateArtworkAsync(Artwork artwork, IFormFile imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imageUrl = await _fileService.UploadImageAsync(imageFile, "artworks");
                    artwork.ImageUrl = imageUrl;
                }

                artwork.CreatedAt = DateTime.UtcNow;
                artwork.IsAuctionActive = true;

                _context.Artworks.Add(artwork);
                await _context.SaveChangesAsync();

                return (true, "Artwork created successfully!");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<Artwork?> GetArtworkByIdAsync(int id)
        {
            return await _context.Artworks.FindAsync(id);
        }

        public async Task<(bool Success, string Message)> UpdateArtworkAsync(int id, Artwork artwork, IFormFile imageFile)
        {
            try
            {
                var existingArtwork = await _context.Artworks.FindAsync(id);

                if (existingArtwork == null)
                    return (false, "Artwork not found!");

                existingArtwork.Title = artwork.Title;
                existingArtwork.Price = artwork.Price;
                existingArtwork.Technique = artwork.Technique;
                existingArtwork.Style = artwork.Style;
                existingArtwork.Year = artwork.Year;
                existingArtwork.Width = artwork.Width;
                existingArtwork.Height = artwork.Height;
                existingArtwork.Period = artwork.Period;
                existingArtwork.Condition = artwork.Condition;
                existingArtwork.Signature = artwork.Signature;
                existingArtwork.CountryOfOrigin = artwork.CountryOfOrigin;
                existingArtwork.AuthorId = artwork.AuthorId;
                existingArtwork.AuctionEndDate = artwork.AuctionEndDate;
                existingArtwork.IsAuctionActive = artwork.IsAuctionActive;
                existingArtwork.UpdatedAt = DateTime.UtcNow;

                if (imageFile != null && imageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingArtwork.ImageUrl))
                    {
                        await _fileService.DeleteImageAsync(existingArtwork.ImageUrl);
                    }

                    var imageUrl = await _fileService.UploadImageAsync(imageFile, "artworks");
                    existingArtwork.ImageUrl = imageUrl;
                }

                await _context.SaveChangesAsync();

                return (true, "Artwork updated successfully!");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteArtworkAsync(int id)
        {
            var artwork = await _context.Artworks.FindAsync(id);

            if (artwork == null)
                return (false, "Artwork not found!");

            _context.Artworks.Remove(artwork);
            await _context.SaveChangesAsync();

            return (true, "Artwork deleted successfully!");
        }

        public async Task<(bool Success, string Message)> SetAuctionTimerAsync(int artworkId, DateTime endDate)
        {
            var artwork = await _context.Artworks.FindAsync(artworkId);

            if (artwork == null)
                return (false, "Artwork not found!");

            if (endDate <= DateTime.Now)
                return (false, "End date must be in the future!");

            artwork.AuctionEndDate = endDate;
            artwork.IsAuctionActive = true;
            artwork.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return (true, $"Auction timer set for '{artwork.Title}'! Ends on {endDate:dd/MM/yyyy HH:mm}");
        }
    }
}

