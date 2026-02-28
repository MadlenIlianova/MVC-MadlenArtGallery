using GalleryData.Models;
using GalleryData;
using MadlenArtGallery.Models;
using MadlenArtGallery.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MadlenArtGallery.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<DashboardViewModel> GetDashboardStatsAsync()
        {
            return new DashboardViewModel
            {
                TotalArtworks = await _context.Artworks.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                TotalBids = await _context.Bids.CountAsync(),
                TotalPurchases = await _context.Purchases.CountAsync()
            };
        }

        public async Task<List<Artwork>> GetAllArtworksWithAuthorsAsync()
        {
            return await _context.Artworks
                .Include(a => a.Author)
                .ToListAsync();
        }

        public async Task<List<UserWithRoleViewModel>> GetAllUsersWithRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithRoles = new List<UserWithRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserWithRoleViewModel
                {
                    User = user,
                    Roles = roles.ToList()
                });
            }

            return usersWithRoles;
        }

        public async Task<(bool Success, string Message)> MakeUserAdminAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return (false, "User not found!");

            var isAlreadyAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAlreadyAdmin)
                return (false, $"{user.Email} is already an Admin!");

            await _userManager.AddToRoleAsync(user, "Admin");
            return (true, $"{user.Email} is now an Admin!");
        }

        public async Task<(bool Success, string Message)> RemoveAdminRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return (false, "User not found!");

            if (user.Email == "admin@gallery.com")
                return (false, "Cannot remove the main admin!");

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin)
                return (false, $"{user.Email} is not an Admin!");

            await _userManager.RemoveFromRoleAsync(user, "Admin");
            return (true, $"Admin role removed from {user.Email}");
        }

        public async Task<(bool Success, string Message)> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return (false, "User not found!");

            if (user.Email == "admin@gallery.com")
                return (false, "Cannot delete the main admin!");

            await _userManager.DeleteAsync(user);
            return (true, $"User {user.Email} deleted successfully!");
        }

        public async Task<List<Purchase>> GetAuctionWinnersAsync()
        {
            return await _context.Purchases
                .Include(p => p.Artwork)
                    .ThenInclude(a => a.Author)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}

