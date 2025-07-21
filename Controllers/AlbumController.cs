using MelodyApp.Models.ViewModels;
using MelodyApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MelodyApp.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService albumService;

        public AlbumController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        public async Task<IActionResult> Index()
        {
            var albums = await albumService.GetAllAlbumsAsync();
            return View(albums);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = await albumService.GetAddAlbumViewModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAlbumViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await albumService.AddAlbumAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var album = await albumService.GetAlbumByIdAsync(id);
            if (album == null) return NotFound();

            var model = new AddAlbumViewModel
            {
                Title = album.Title,
                UserId = album.UserId,
                CoverUrl = album.CoverImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddAlbumViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await albumService.EditAlbumAsync(id, model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var album = await albumService.GetAlbumByIdAsync(id);
            if (album == null) return NotFound();

            return View(album);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            await albumService.DeleteAlbumAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var album = await albumService.GetAlbumByIdAsync(id);
            if (album == null) return NotFound();

            return View(album);
        }
    }
}
