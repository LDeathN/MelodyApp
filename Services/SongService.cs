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

        public SongService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Song>> GetAllSongsAsync()
        {
            return await _context.Songs
                .Include(s => s.Genre)
                .ToListAsync();
        }

        public async Task<Song?> GetByIdAsync(int id)
        {
            return await _context.Songs
                .Include(s => s.Genre)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Song song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Song song)
        {
            var existing = await _context.Songs.FindAsync(song.Id);
            if (existing == null) return;

            existing.Title = song.Title;
            existing.GenreId = song.GenreId;

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
