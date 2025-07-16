using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MelodyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
