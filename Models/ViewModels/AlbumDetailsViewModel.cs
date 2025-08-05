namespace MelodyApp.Models.ViewModels
{
    public class AlbumDetailsViewModel : AlbumViewModel
    {
        public List<SongInAlbumViewModel> Songs { get; set; } = new();
    }

    public class SongInAlbumViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? GenreName { get; set; }
    }
}
