using GalleryData.Models;
using GalleryData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MadlenArtGallery.Hubs;
using Microsoft.AspNetCore.SignalR;
using MadlenArtGallery.Services.Interfaces;
using MadlenArtGallery.Models;

namespace MadlenArtGallery.Controllers
{
    public class BidsController : Controller
    {
        private readonly IBidService _bidService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public BidsController(IBidService bidService, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _bidService = bidService;
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> MyBids()
        {
            var userId = _userManager.GetUserId(User);
            var myBids = await _bidService.GetUserBidsAsync(userId);
            return View(myBids);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PlaceBid(int artworkId, int bidPrice)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.GetUserAsync(User);

            var result = await _bidService.PlaceBidAsync(artworkId, bidPrice, userId, user.Email);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction("Details", "Artworks", new { id = artworkId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteBid(int id)
        {
            var result = await _bidService.DeleteBidAsync(id);

            if (result.Success)
                TempData["Success"] = result.Message;

            return RedirectToAction("AllBids");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllBids()
        {
            var allBids = await _bidService.GetAllBidsAsync();
            return View(allBids);
        }

        [Authorize]
        public async Task<IActionResult> MyWins()
        {
            var userId = _userManager.GetUserId(User);

            var myWins = await _context.Purchases
                .Include(p => p.Artwork)
                    .ThenInclude(a => a.Author)
                .Include(p => p.Frame)           
                .Include(p => p.FrameSize)       
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(myWins);
        }

        [Authorize]
        public async Task<IActionResult> SelectFrame(int purchaseId)
        {
            var userId = _userManager.GetUserId(User);

            var purchase = await _context.Purchases
                .Include(p => p.Artwork)
                    .ThenInclude(a => a.Author)
                .Include(p => p.Frame)
                .Include(p => p.FrameSize)
                .FirstOrDefaultAsync(p => p.Id == purchaseId && p.UserId == userId);

            if (purchase == null)
            {
                TempData["Error"] = "Purchase not found.";
                return RedirectToAction(nameof(MyWins));
            }

            var frames = await _context.Frames
                .Include(f => f.Sizes)
                .ToListAsync();

            var viewModel = new FrameSelectionViewModel
            {
                PurchaseId = purchase.Id,
                ArtworkTitle = purchase.Artwork.Title,
                ArtworkArtist = purchase.Artwork.Author?.Name ?? "Unknown",
                ArtworkImageUrl = purchase.Artwork.ImageUrl,
                ArtworkPrice = purchase.ArtworkPrice,
                AvailableFrames = frames
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFrameToPurchase(int purchaseId, int frameSizeId)
        {
            var userId = _userManager.GetUserId(User);

            var purchase = await _context.Purchases
                .FirstOrDefaultAsync(p => p.Id == purchaseId && p.UserId == userId);

            if (purchase == null)
            {
                TempData["Error"] = "Purchase not found.";
                return RedirectToAction(nameof(MyWins));
            }

            var frameSize = await _context.FrameSizes
                .FirstOrDefaultAsync(fs => fs.Id == frameSizeId);

            if (frameSize == null)
            {
                TempData["Error"] = "Frame not found.";
                return RedirectToAction(nameof(SelectFrame), new { purchaseId });
            }

            var isChanging = purchase.FrameId.HasValue;

            purchase.FrameId = frameSize.FrameId;
            purchase.FrameSizeId = frameSizeId;
            purchase.FramePrice = frameSize.Price;
            purchase.TotalPrice = purchase.ArtworkPrice + frameSize.Price;
            purchase.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            if (isChanging)
            {
                TempData["Success"] = $"Frame changed successfully! New total: €{purchase.TotalPrice:N2}";
            }
            else
            {
                TempData["Success"] = $"Frame added successfully! New total: €{purchase.TotalPrice:N2}";
            }

            return RedirectToAction(nameof(MyWins));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFrame(int purchaseId)
        {
            var userId = _userManager.GetUserId(User);

            var purchase = await _context.Purchases
                .FirstOrDefaultAsync(p => p.Id == purchaseId && p.UserId == userId);

            if (purchase == null)
            {
                TempData["Error"] = "Purchase not found.";
                return RedirectToAction(nameof(MyWins));
            }

            if (!purchase.FrameId.HasValue)
            {
                TempData["Info"] = "This artwork doesn't have a frame.";
                return RedirectToAction(nameof(MyWins));
            }

            var oldFramePrice = purchase.FramePrice;

            purchase.FrameId = null;
            purchase.FrameSizeId = null;
            purchase.FramePrice = 0;
            purchase.TotalPrice = purchase.ArtworkPrice;
            purchase.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Frame removed successfully! €{oldFramePrice:N2} deducted from total.";
            return RedirectToAction(nameof(MyWins));
        }
    }
}

