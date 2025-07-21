using System.ComponentModel.DataAnnotations;

namespace MelodyApp.Models.ViewModels
{
    public class SongFormModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Artist { get; set; }

        public string? Description { get; set; }

        [Required]
        [Display(Name = "Album")]
        public Guid AlbumId { get; set; }

        // File upload
        [Display(Name = "Audio File")]
        public IFormFile? AudioFile { get; set; }

        // Optional: list of available albums for dropdown
        //public IEnumerable<AlbumViewModel>? Albums { get; set; }
    }
}
