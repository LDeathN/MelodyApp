using MelodyApp.Data;
using MelodyApp.Models;
using MelodyApp.Models.ViewModels;
using MelodyApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MelodyApp.Services
{
    public class SongService : ISongService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SongService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<Song>> GetAllSongsAsync()
        {
            return await _context.Songs
                .Include(s => s.Genre)
                .Include(s => s.Artist)
                .ToListAsync();
        }

        public async Task<Song?> GetByIdAsync(int id)
        {
            return await _context.Songs
                .Include(s => s.Genre)
                .Include(s => s.Artist)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Song song, IFormFile audioFile)
        {
            if (audioFile != null && audioFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(audioFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await audioFile.CopyToAsync(stream);

                song.Url = $"/uploads/{fileName}";
            }

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Song song, IFormFile? audioFile = null)
        {
            var existing = await _context.Songs.FindAsync(song.Id);
            if (existing == null) return;

            existing.Title = song.Title;
            existing.ArtistId = song.ArtistId;
            existing.GenreId = song.GenreId;

            if (audioFile != null && audioFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(audioFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await audioFile.CopyToAsync(stream);

                existing.Url = $"/uploads/{fileName}";
            }

            _context.Songs.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GenreViewModel>> GetAllGenresAsync()
        {
            return await _context.Genres
                .Select(g => new GenreViewModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();
        }
    }
}
