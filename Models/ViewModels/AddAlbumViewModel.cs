using System.ComponentModel.DataAnnotations;

namespace MelodyApp.Models.ViewModels
{
    public class AddAlbumViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required]
        [Display(Name = "Cover Image URL")]
        public string CoverUrl { get; set; } = null!;

        [Display(Name = "User")]
        [Required]
        public string UserId { get; set; }

        public IEnumerable<ArtistDropdownViewModel> Artists { get; set; } = new List<ArtistDropdownViewModel>();
    }
}
