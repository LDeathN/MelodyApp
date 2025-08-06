using MelodyApp.Data;
using MelodyApp.Models;
using MelodyApp.Models.ViewModels;
using MelodyApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MelodyApp.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly IAlbumService albumService;
        private readonly ApplicationDbContext _context;

        public AlbumController(IAlbumService albumService, ApplicationDbContext context)
        {
            this.albumService = albumService;
            this._context = context;
        }

        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            int pageSize = 5;

            var albumsQuery = _context.Albums
                .Include(a => a.User)
                .Include(a => a.AlbumSongs)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                albumsQuery = albumsQuery.Where(a => a.Title.Contains(searchTerm));
                ViewData["CurrentFilter"] = searchTerm;
            }

            var totalAlbums = await albumsQuery.CountAsync();
            var albums = await albumsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AlbumViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    CoverImageUrl = a.CoverImageUrl,
                    ArtistName = a.User.UserName,
                    SongCount = a.AlbumSongs.Count
                })
                .ToListAsync();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling(totalAlbums / (double)pageSize);

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
            var album = await albumService.GetAlbumWithSongsByIdAsync(id);
            if (album == null) return NotFound();

            var model = new AlbumDetailsViewModel
            {
                Id = album.Id,
                Title = album.Title,
                CoverImageUrl = album.CoverImageUrl,
                ArtistName = album.User.UserName,
                Songs = album.AlbumSongs.Select(asg => new SongInAlbumViewModel
                {
                    Id = asg.Song.Id,
                    Title = asg.Song.Title,
                    GenreName = asg.Song.Genre?.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSongFromAlbum(int albumId, int songId)
        {
            var album = await _context.Albums
                .Include(a => a.AlbumSongs)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album == null)
            {
                return NotFound();
            }

            var song = album.AlbumSongs.FirstOrDefault(a => a.SongId == songId);
            if (song != null)
            {
                album.AlbumSongs.Remove(song);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = albumId });
        }
    }
}
