using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MelodyApp.Models.ViewModels
{
    public class SongFormModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        //[Required]
        //[Display(Name = "Audio File")]
        //public IFormFile AudioFile { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        // For dropdown
        public IEnumerable<SelectListItem>? Genres { get; set; }
    }
}
