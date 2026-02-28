using GalleryData.Models;

namespace MadlenArtGallery.Models
{
    public class UserWithRoleViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
