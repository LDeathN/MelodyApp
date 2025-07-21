using MelodyApp.Models;
using MelodyApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MelodyApp.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(IFavoriteService favoriteService, UserManager<ApplicationUser> userManager)
        {
            _favoriteService = favoriteService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int songId)
        {
            var userId = _userManager.GetUserId(User);
            await _favoriteService.AddToFavoritesAsync(userId, songId);
            return RedirectToAction("Index", "Songs");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int songId)
        {
            var userId = _userManager.GetUserId(User);
            await _favoriteService.RemoveFromFavoritesAsync(userId, songId);
            return RedirectToAction("Index");
        }
    }
}
