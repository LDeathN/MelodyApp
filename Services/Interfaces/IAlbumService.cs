using MelodyApp.Models.ViewModels;

namespace MelodyApp.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<AlbumViewModel>> GetAllAlbumsAsync();

        Task<AddAlbumViewModel> GetAddAlbumViewModelAsync(); // For populating dropdowns if needed

        Task AddAlbumAsync(AddAlbumViewModel model);

        Task<AlbumViewModel> GetAlbumByIdAsync(int id);

        Task EditAlbumAsync(int id, AddAlbumViewModel model);

        Task DeleteAlbumAsync(int id);
    }
}
