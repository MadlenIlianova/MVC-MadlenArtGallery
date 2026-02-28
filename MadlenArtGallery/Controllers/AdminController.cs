using GalleryData;
using GalleryData.Models;
using MadlenArtGallery.Models;
using MadlenArtGallery.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MadlenArtGallery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return View(stats);
        }

        public async Task<IActionResult> ManageArtworks()
        {
            var artworks = await _adminService.GetAllArtworksWithAuthorsAsync();
            return View(artworks);
        }

        public async Task<IActionResult> ManageUsers()
        {
            var usersWithRoles = await _adminService.GetAllUsersWithRolesAsync();
            return View(usersWithRoles);
        }

        [HttpPost]
        public async Task<IActionResult> MakeUserAdmin(string userId)
        {
            var result = await _adminService.MakeUserAdminAsync(userId);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdmin(string userId)
        {
            var result = await _adminService.RemoveAdminRoleAsync(userId);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _adminService.DeleteUserAsync(userId);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction("ManageUsers");
        }

        public async Task<IActionResult> AuctionWinners()
        {
            var winners = await _adminService.GetAuctionWinnersAsync();
            return View(winners);
        }
    }
}