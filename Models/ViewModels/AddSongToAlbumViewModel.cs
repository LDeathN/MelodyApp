using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MelodyApp.Models.ViewModels
{
    public class AddSongToAlbumViewModel
    {
        public int SongId { get; set; }

        [Required]
        [Display(Name = "Select Album")]
        public int SelectedAlbumId { get; set; }

        public IEnumerable<SelectListItem> UserAlbums { get; set; } = new List<SelectListItem>();
    }
}
