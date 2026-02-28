using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryData.Models
{
    public class Frame : BaseModel.BaseEntity
    {
        public int Id { get; set; }
        public string Material { get; set; }    
        public string Color { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<FrameSize> Sizes { get; set; } = new List<FrameSize>();
    }
}
