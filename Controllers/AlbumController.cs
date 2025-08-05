using MelodyApp.Models;
using MelodyApp.Models.ViewModels;
using MelodyApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MelodyApp.Controllers
{
    [Authorize]
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
        public IActionResult Add()
        {
            return View(new AddAlbumViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddAlbumViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UserId = userId;

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
                CoverUrl = album.CoverImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddAlbumViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UserId = userId;

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
