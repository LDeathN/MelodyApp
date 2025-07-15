namespace MelodyApp.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
    
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    
        public ICollection<AlbumSong> AlbumSongs { get; set; } = new List<AlbumSong>();
        public ICollection<FavoriteSong> FavoritedBy { get; set; } = new List<FavoriteSong>();
    }
}
