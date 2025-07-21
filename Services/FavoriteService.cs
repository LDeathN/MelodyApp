using MelodyApp.Data;
using MelodyApp.Models;
using MelodyApp.Models.ViewModels;
using MelodyApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MelodyApp.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext _context;

        public FavoriteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToFavoritesAsync(string userId, int songId)
        {
            var exists = await _context.FavoriteSongs
                .AnyAsync(f => f.UserId == userId && f.SongId == songId);
            if (!exists)
            {
                _context.FavoriteSongs.Add(new FavoriteSong { UserId = userId, SongId = songId });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromFavoritesAsync(string userId, int songId)
        {
            var fav = await _context.FavoriteSongs
                .FirstOrDefaultAsync(f => f.UserId == userId && f.SongId == songId);
            if (fav != null)
            {
                _context.FavoriteSongs.Remove(fav);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsFavoriteAsync(string userId, int songId)
        {
            return await _context.FavoriteSongs.AnyAsync(f => f.UserId == userId && f.SongId == songId);
        }

        public async Task<IEnumerable<SongViewModel>> GetUserFavoritesAsync(string userId)
        {
            return await _context.FavoriteSongs
                .Where(f => f.UserId == userId)
                .Include(f => f.Song)
                    .ThenInclude(s => s.Artist)
                .Include(f => f.Song)
                    .ThenInclude(s => s.Genre)
                .Select(f => new SongViewModel
                {
                    Id = f.Song.Id,
                    Title = f.Song.Title,
                    ArtistName = f.Song.Artist.Name,
                })
                .ToListAsync();
        }
    }
}
