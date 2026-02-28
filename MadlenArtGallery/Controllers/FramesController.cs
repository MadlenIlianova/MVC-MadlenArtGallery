using GalleryData;
using MadlenArtGallery.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MadlenArtGallery.Controllers
{
    public class FramesController : Controller
    {
        private readonly IFrameService _frameService;

        public FramesController(IFrameService frameService)
        {
            _frameService = frameService;
        }

        public async Task<IActionResult> Index()
        {
            var frames = await _frameService.GetAllFramesWithSizesAsync();
            return View(frames);
        }
    }
}
