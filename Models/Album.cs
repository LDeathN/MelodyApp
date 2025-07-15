namespace MelodyApp.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
    
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    
        public ICollection<AlbumSong> AlbumSongs { get; set; } = new List<AlbumSong>();
    }
}
