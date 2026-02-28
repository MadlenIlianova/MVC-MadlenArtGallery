using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryData.Models
{
    public class UserLikedArtwork : BaseModel.BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ArtworkId { get; set; }
        public Artwork Artwork { get; set; }
    }
}
