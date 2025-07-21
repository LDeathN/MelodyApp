namespace MelodyApp.Models.ViewModels
{
    public class AlbumViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string CoverImageUrl { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public int SongCount { get; set; }
    }
}
