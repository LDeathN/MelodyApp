using MelodyApp.Data;
using MelodyApp.Models;
using MelodyApp.Models.ViewModels;
using MelodyApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MelodyApp.Controllers
{
    [Authorize]
    public class SongController : Controller
    {
        private readonly ISongService _songService;
        private readonly ApplicationDbContext _context;

        public SongController(ISongService songService, ApplicationDbContext context)
        {
            _songService = songService;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorites = await _context.FavoriteSongs
                .Where(fs => fs.UserId == userId)
                .Select(fs => fs.Song)
                .ToListAsync();

            return View(favorites);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFromFavorites(int songId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorite = await _context.FavoriteSongs
                .FirstOrDefaultAsync(fs => fs.SongId == songId && fs.UserId == userId);

            if (favorite != null)
            {
                _context.FavoriteSongs.Remove(favorite);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Removed from favorites!";
            }

            return RedirectToAction("Favorites");
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var songsQuery = _context.Songs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                songsQuery = songsQuery.Where(s => s.Title.Contains(searchTerm));
                ViewData["CurrentFilter"] = searchTerm;
            }

            var songs = await songsQuery.ToListAsync();
            return View(songs);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new SongFormModel
            {
                Genres = await _context.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SongFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    }).ToListAsync();

                return View(model);
            }

            var song = new Song
            {
                Title = model.Title,
                GenreId = model.GenreId,
                ArtistId = 1,
            };

            await _songService.AddAsync(song);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddToAlbum(int songId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var albums = await _context.Albums
                .Where(a => a.UserId == userId)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Title
                })
                .ToListAsync();

            var model = new AddSongToAlbumViewModel
            {
                SongId = songId,
                UserAlbums = albums
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToAlbum(AddSongToAlbumViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload albums in case of validation error
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.UserAlbums = await _context.Albums
                    .Where(a => a.UserId == userId)
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Title
                    })
                    .ToListAsync();

                return View(model);
            }

            var exists = await _context.AlbumSongs
                .AnyAsync(x => x.AlbumId == model.SelectedAlbumId && x.SongId == model.SongId);

            if (!exists)
            {
                _context.AlbumSongs.Add(new AlbumSong
                {
                    AlbumId = model.SelectedAlbumId,
                    SongId = model.SongId
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Song", new { id = model.SongId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song == null) return NotFound();

            var model = new SongFormModel
            {
                Title = song.Title,
                GenreId = song.GenreId,
                Genres = await _context.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SongFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToListAsync();
                return View(model);
            }

            var song = new Song
            {
                Id = id,
                Title = model.Title,
                GenreId = model.GenreId
            };

            await _songService.UpdateAsync(song);
            return RedirectToAction(nameof(Index));
        }

        public async Task DeleteAsync(int id)
        {
            var song = await _context.Songs
                .Include(s => s.AlbumSongs) 
                .FirstOrDefaultAsync(s => s.Id == id);

            if (song == null)
                throw new Exception("Song not found");

            _context.AlbumSongs.RemoveRange(song.AlbumSongs);

            _context.Songs.Remove(song);

            await _context.SaveChangesAsync();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _songService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int songId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            TempData["SuccessMessage"] = "Added to favorites!";

            var alreadyFavorite = await _context.FavoriteSongs
                .AnyAsync(fs => fs.SongId == songId && fs.UserId == userId);

            if (!alreadyFavorite)
            {
                var favorite = new FavoriteSong
                {
                    SongId = songId,
                    UserId = userId
                };

                _context.FavoriteSongs.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
