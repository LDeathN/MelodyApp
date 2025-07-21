using Microsoft.AspNetCore.Mvc;

namespace MelodyApp.Controllers
{
    public class FavoriteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
