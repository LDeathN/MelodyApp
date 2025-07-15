namespace MelodyApp.Models
{
    using Microsoft.AspNetCore.Identity;
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Song> Songs { get; set; } = new List<Song>();
        public ICollection<Album> Albums { get; set; } = new List<Album>();
        public ICollection<FavoriteSong> FavoriteSongs { get; set; } = new List<FavoriteSong>();
    }
}
