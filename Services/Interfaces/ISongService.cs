using MelodyApp.Models;
using MelodyApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MelodyApp.Services.Interfaces
{
    public interface ISongService
    {
        Task<List<Song>> GetAllSongsAsync();
        Task<Song?> GetByIdAsync(int id);
        Task AddAsync(Song song, IFormFile audioFile);
        Task UpdateAsync(Song song, IFormFile? audioFile = null);
        Task DeleteAsync(int id);
        Task<IEnumerable<GenreViewModel>> GetAllGenresAsync();

    }
}
