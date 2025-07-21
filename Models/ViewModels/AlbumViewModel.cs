namespace MelodyApp.Models.ViewModels
{
    public class AlbumViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string CoverUrl { get; set; } = null!;

        public string ArtistName { get; set; } = null!;

        public int SongCount { get; set; }
    }
}
