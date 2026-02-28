using GalleryData;
using GalleryData.Models;
using MadlenArtGallery.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MadlenArtGallery.Services
{
    public class FrameService : IFrameService
    {
        private readonly AppDbContext _context;

        public FrameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Frame>> GetAllFramesWithSizesAsync()
        {
            return await _context.Frames
                .Include(f => f.Sizes)
                .ToListAsync();
        }
    }
}
