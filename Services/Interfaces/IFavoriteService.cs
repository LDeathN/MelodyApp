using MelodyApp.Models.ViewModels;

namespace MelodyApp.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task AddToFavoritesAsync(string userId, int songId);
        Task RemoveFromFavoritesAsync(string userId, int songId);
        Task<bool> IsFavoriteAsync(string userId, int songId);
        Task<IEnumerable<SongViewModel>> GetUserFavoritesAsync(string userId);
    }
}
