using GalleryData;
using MadlenArtGallery.Models;
using MadlenArtGallery.Services.Interfaces;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace MadlenArtGallery.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly AppDbContext _context;

        public StatisticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GalleryStatsViewModel> GetGalleryStatsAsync()
        {
            var authorOfMonth = await GetAuthorOfMonthAsync();

            return new GalleryStatsViewModel
            {
                TotalArtworks = await _context.Artworks.CountAsync(),
                ActiveAuctions = await _context.Artworks
                    .CountAsync(a => a.IsAuctionActive && a.AuctionEndDate > DateTime.Now),
                TotalAuthors = await _context.Authors.CountAsync(),
                AuthorOfMonth = authorOfMonth
            };
        }

        public async Task<AuthorStatViewModel> GetAuthorOfMonthAsync()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var now = DateTime.Now;

            var authorStats = await _context.Artworks
                .Include(a => a.Author)
                .Where(a => !a.IsAuctionActive &&
                           a.AuctionEndDate.HasValue &&
                           a.AuctionEndDate.Value < now &&
                           a.AuctionEndDate.Value.Month == currentMonth &&
                           a.AuctionEndDate.Value.Year == currentYear)
                .GroupBy(a => a.Author)
                .Select(g => new
                {
                    Author = g.Key,
                    TotalSales = g.Count(),
                    TotalRevenue = g.Sum(a => a.Price),
                    TopArtwork = g.OrderByDescending(a => a.Price).FirstOrDefault()
                })
                .OrderByDescending(x => x.TotalRevenue)
                .FirstOrDefaultAsync();

            if (authorStats == null)
                return null;

            return new AuthorStatViewModel
            {
                AuthorName = authorStats.Author.Name,
                TotalSales = authorStats.TotalSales,
                TotalRevenue = authorStats.TotalRevenue,
                FeaturedArtwork = authorStats.TopArtwork
            };
        }
    }
}

