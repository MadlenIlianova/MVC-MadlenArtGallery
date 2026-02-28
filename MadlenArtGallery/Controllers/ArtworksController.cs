using GalleryData;
using GalleryData.Models;
using MadlenArtGallery.Models;
using MadlenArtGallery.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MadlenArtGallery.Controllers
{
    public class ArtworksController : Controller
    {

        private readonly IArtworkService _artworkService;

        public ArtworksController(IArtworkService artworkService)
        {
            _artworkService = artworkService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string technique, string style, int? yearFrom,
            int? yearTo, int? minPrice, int? maxPrice, string sortBy)
        {
            var model = await _artworkService.SearchArtworksAsync(
                technique, style, yearFrom, yearTo, minPrice, maxPrice, sortBy);

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var artwork = await _artworkService.GetArtworkWithDetailsAsync(id);

            if (artwork == null)
                return NotFound();

            var bidInfo = await _artworkService.GetArtworkBidInfoAsync(id);

            ViewBag.HighestBid = bidInfo.HighestBid;
            ViewBag.BidsCount = bidInfo.BidsCount;

            return View(artwork);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await _artworkService.GetAllAuthorsAsync();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Artwork artwork, IFormFile imageFile)
        {
            ModelState.Remove("Author");
            ModelState.Remove("Bids");
            ModelState.Remove("UserLikedArtworks");
            ModelState.Remove("ImageUrl");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join(", ", errors);
                ViewBag.Authors = await _artworkService.GetAllAuthorsAsync();
                return View(artwork);
            }

            var result = await _artworkService.CreateArtworkAsync(artwork, imageFile);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var artwork = await _artworkService.GetArtworkByIdAsync(id);

            if (artwork == null)
                return NotFound();

            ViewBag.Authors = await _artworkService.GetAllAuthorsAsync();
            return View(artwork);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Artwork artwork, IFormFile imageFile)
        {
            if (id != artwork.Id)
                return NotFound();

            ModelState.Remove("Author");
            ModelState.Remove("Bids");
            ModelState.Remove("UserLikedArtworks");
            ModelState.Remove("ImageUrl");
            ModelState.Remove("imageFile");
            ModelState.Remove("Period");
            ModelState.Remove("Condition");
            ModelState.Remove("Signature");
            ModelState.Remove("CountryOfOrigin");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join(", ", errors);
                ViewBag.Authors = await _artworkService.GetAllAuthorsAsync();
                return View(artwork);
            }

            var result = await _artworkService.UpdateArtworkAsync(id, artwork, imageFile);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            if (!result.Success && result.Message == "Artwork not found!")
                return RedirectToAction("Index");

            ViewBag.Authors = await _artworkService.GetAllAuthorsAsync();
            return result.Success ? RedirectToAction("Index") : View(artwork);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var artwork = await _artworkService.GetArtworkWithDetailsAsync(id);

            if (artwork == null)
                return NotFound();

            return View(artwork);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _artworkService.DeleteArtworkAsync(id);

            if (result.Success)
                TempData["Success"] = result.Message;

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SetAuctionTimer(int artworkId, DateTime endDate)
        {
            var result = await _artworkService.SetAuctionTimerAsync(artworkId, endDate);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction("Index");
        }
    }
}