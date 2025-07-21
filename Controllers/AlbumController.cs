using Microsoft.AspNetCore.Mvc;

namespace MelodyApp.Controllers
{
    public class AlbumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
