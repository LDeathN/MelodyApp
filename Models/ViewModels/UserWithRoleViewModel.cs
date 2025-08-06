using MelodyApp.Models;

namespace MelodyApp.Models.ViewModels
{
    public class UserWithRoleViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}