using Microsoft.AspNetCore.Identity;

namespace MelodyApp.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<UserWithRoleViewModel> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<Song> Songs { get; set; }
        public List<Album> Albums { get; set; }
        public List<Artist> Artists { get; set; }
    }
}
