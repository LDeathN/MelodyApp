namespace MelodyApp.Models.ViewModels
{
    public class SongViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ArtistName { get; set; }

        public string? AlbumName { get; set; }

        public string? OwnerUsername { get; set; }

        public bool IsFavorite { get; set; }

        public string? AudioUrl { get; set; } 
    }
}
