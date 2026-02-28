using GalleryData.Models;
using GalleryData;
using MadlenArtGallery.Hubs;
using MadlenArtGallery.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MadlenArtGallery.Services
{
    public class BidService : IBidService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<BidHub> _hubContext;

        public BidService(AppDbContext context, IHubContext<BidHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<List<Bid>> GetUserBidsAsync(string userId)
        {
            return await _context.Bids
                .Include(b => b.Artwork)
                    .ThenInclude(a => a.Author)
                .Include(b => b.Artwork.Bids)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Date)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> PlaceBidAsync(int artworkId, int bidPrice, string userId, string userEmail)
        {
            var artwork = await _context.Artworks
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == artworkId);

            if (artwork == null)
                return (false, "Artwork not found!");

            if (artwork.AuctionEndDate.HasValue && artwork.AuctionEndDate.Value <= DateTime.UtcNow)
                return (false, "This auction has ended! Bidding is no longer allowed.");

            var highestBid = artwork.Bids
                .OrderByDescending(b => b.Price)
                .FirstOrDefault();

            var minimumBid = highestBid != null ? highestBid.Price + 10 : artwork.Price;

            if (bidPrice < minimumBid)
                return (false, $"Your bid must be at least €{minimumBid}");

            var bid = new Bid
            {
                ArtworkId = artworkId,
                UserId = userId,
                Price = bidPrice,
                Date = DateTime.Now
            };

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveNewBid", artworkId, userEmail, bidPrice);

            return (true, "Your bid has been placed successfully!");
        }

        public async Task<List<Bid>> GetAllBidsAsync()
        {
            return await _context.Bids
                .Include(b => b.Artwork)
                    .ThenInclude(a => a.Author)
                .Include(b => b.User)
                .OrderByDescending(b => b.Date)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message, int ArtworkId)> DeleteBidAsync(int bidId)
        {
            var bid = await _context.Bids.FindAsync(bidId);

            if (bid == null)
                return (false, "Bid not found!", 0);

            var artworkId = bid.ArtworkId;

            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("BidDeleted", artworkId, bidId);

            return (true, "Bid deleted successfully!", artworkId);
        }

        public async Task<List<Purchase>> GetUserWinsAsync(string userId)
        {
            return await _context.Purchases
                .Include(p => p.Artwork)
                    .ThenInclude(a => a.Author)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}

