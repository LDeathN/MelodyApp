using MelodyApp.Data;
using MelodyApp.Models;
using MelodyApp.Models.ViewModels;
using MelodyApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace MelodyApp.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly ApplicationDbContext _context;

        public AlbumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AlbumViewModel>> GetAllAlbumsAsync()
        {
            return await _context.Albums
                .Include(a => a.User)
                .Include(a => a.AlbumSongs)
                .Select(a => new AlbumViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    CoverImageUrl = a.CoverImageUrl,
                    ArtistName = a.User.UserName,
                    SongCount = a.AlbumSongs.Count
                })
                .ToListAsync();
        }

        public async Task<AddAlbumViewModel> GetAddAlbumViewModelAsync()
        {
            var artists = await _context.Artists
                .Select(a => new ArtistDropdownViewModel
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToListAsync();

            return new AddAlbumViewModel
            {
                Artists = artists
            };
        }

        public async Task AddAlbumAsync(AddAlbumViewModel model)
        {
            var album = new Album
            {
                Title = model.Title,
                CoverImageUrl = model.CoverUrl,
                UserId = model.UserId,
            };

            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();
        }

        public async Task<AlbumViewModel> GetAlbumByIdAsync(int id)
        {
            var album = await _context.Albums
                .Include(a => a.User)
                .Include(a => a.AlbumSongs)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (album == null)
                throw new InvalidOperationException("Album not found.");

            return new AlbumViewModel
            {
                Id = album.Id,
                Title = album.Title,
                CoverImageUrl = album.CoverImageUrl,
                ArtistName = album.User.UserName,
                SongCount = album.AlbumSongs.Count
            };
        }

        public async Task EditAlbumAsync(int id, AddAlbumViewModel model)
        {
            var album = await _context.Albums.FindAsync(id);

            if (album == null)
                throw new InvalidOperationException("Album not found.");

            album.Title = model.Title;
            album.CoverImageUrl = model.CoverUrl;
            album.UserId = model.UserId;

            _context.Albums.Update(album);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAlbumAsync(int id)
        {
            var album = await _context.Albums.FindAsync(id);

            if (album == null)
                throw new InvalidOperationException("Album not found.");

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
        }
    }
}
