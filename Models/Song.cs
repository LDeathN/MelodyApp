using System.ComponentModel.DataAnnotations;
namespace MelodyApp.Models
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!;

        // Genre relation
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;

        // Artist relation
        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;

        // Other relations
        public ICollection<FavoriteSong> FavoritedBy { get; set; } = new List<FavoriteSong>();
        public ICollection<AlbumSong> AlbumSongs { get; set; } = new List<AlbumSong>();
    }
}
