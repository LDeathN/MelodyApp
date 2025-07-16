using System.ComponentModel.DataAnnotations;
namespace MelodyApp.Models
{
    public class Artist
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        // Navigation
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
