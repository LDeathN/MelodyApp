namespace MelodyApp.Models
{
    public class FavoriteSong
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    
        public int SongId { get; set; }
        public Song Song { get; set; }
    }
}   
