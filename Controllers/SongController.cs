using MelodyApp.Models;
using MelodyApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MelodyApp.Controllers
{
    [Authorize]
    public class SongController : Controller
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        // GET: Songs
        public async Task<IActionResult> Index()
        {
            var songs = await _songService.GetAllSongsAsync();
            return View(songs);
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song == null) return NotFound();
            return View(song);
        }

        // GET: Songs/Add
        [Authorize]
        public async Task<IActionResult> Add()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        // POST: Songs/Add
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Song song, IFormFile audioFile)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(song);
            }

            await _songService.AddAsync(song, audioFile);
            return RedirectToAction(nameof(Index));
        }

        // GET: Songs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song == null) return NotFound();

            await PopulateDropdownsAsync();
            return View(song);
        }

        // POST: Songs/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Song song, IFormFile? audioFile)
        {
            if (id != song.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(song);
            }

            await _songService.UpdateAsync(song, audioFile);
            return RedirectToAction(nameof(Index));
        }

        // GET: Songs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song == null) return NotFound();
            return View(song);
        }

        // POST: Songs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _songService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync()
        {
            var genres = await _songService.GetAllGenresAsync();
            ViewBag.Genres = genres;

            // Assuming you also want artists dropdown, you will need a similar method in ISongService and SongService to get artists
            // For now, you can add a placeholder or add that method next
        }
    }
}
