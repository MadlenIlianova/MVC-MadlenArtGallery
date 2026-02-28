using GalleryData.Models;
using GalleryData;
using Microsoft.EntityFrameworkCore;

namespace MadlenArtGallery.Services
{
    public class AuctionService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuctionService> _logger;

        public AuctionService(IServiceProvider serviceProvider, ILogger<AuctionService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Auction Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAndCloseAuctions();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking auctions");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task CheckAndCloseAuctions()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var endedAuctions = await context.Artworks
                .Include(a => a.Bids)
                    .ThenInclude(b => b.User)
                .Where(a => a.IsAuctionActive
                       && a.AuctionEndDate.HasValue
                       && a.AuctionEndDate.Value <= DateTime.Now)
                .ToListAsync();

            foreach (var artwork in endedAuctions)
            {
  
                artwork.IsAuctionActive = false;
                var winningBid = artwork.Bids
                    .OrderByDescending(b => b.Price)
                    .ThenBy(b => b.Date) 
                    .FirstOrDefault();

                if (winningBid != null)
                {
                    var purchase = new Purchase
                    {
                        ArtworkId = artwork.Id,
                        UserId = winningBid.UserId,
                        ArtworkPrice = winningBid.Price,
                        FramePrice = 0,
                        TotalPrice = winningBid.Price,
                        CreatedAt = DateTime.UtcNow
                    };

                    context.Purchases.Add(purchase);

                    _logger.LogInformation(
                        $"Auction ended for '{artwork.Title}'. Winner: {winningBid.User?.Email} with bid €{winningBid.Price}"
                    );

                }
                else
                {
                    _logger.LogInformation($"Auction ended for '{artwork.Title}' with no bids.");
                }
            }

            if (endedAuctions.Any())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
