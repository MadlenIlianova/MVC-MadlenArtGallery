using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryData.Models
{
    public class FrameSize : BaseModel.BaseEntity
    {
        public int Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Price { get; set; }
        public int FrameId { get; set; }
        public Frame Frame { get; set; }
    }
}
