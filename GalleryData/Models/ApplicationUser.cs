using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryData.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserLikedArtwork> UserLikedArtworks { get; set; }
        public ICollection<Bid> Bids { get; set; }
    }
}
