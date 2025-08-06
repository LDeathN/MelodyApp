using MelodyApp.Data;
using MelodyApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MelodyApp.Models.ViewModels;

namespace MelodyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Load users with their roles (via UserRoles and Roles tables)
            var users = await _context.Users.ToListAsync();

            var roles = await _context.Roles.ToListAsync();

            var userRoles = await _context.UserRoles.ToListAsync();

            var userWithRoles = users.Select(user => new UserWithRoleViewModel
            {
                User = user,
                Roles = userRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(roles,
                          ur => ur.RoleId,
                          r => r.Id,
                          (ur, r) => r.Name)
                    .ToList()
            }).ToList();

            var model = new AdminDashboardViewModel
            {
                Users = userWithRoles,
                Roles = roles,
                Songs = await _context.Songs.Include(s => s.Genre).Include(s => s.Artist).ToListAsync(),
                Albums = await _context.Albums.ToListAsync(),
                Artists = await _context.Artists.ToListAsync()
            };

            return View(model);
        }

        // ------------ USER METHODS ------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                // Log or break here — userId is missing from form POST
                return BadRequest("UserId is null or empty");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                // Log or break here — no user found for this ID
                return NotFound("User not found");
            }

            if (user.Email == "admin@melodyhub.com")
            {
                // Protect your main admin
                return RedirectToAction("Index");
            }

            // Before deletion, log user info (optional)
            // Debug.WriteLine($"Deleting user: {user.Email} with ID: {userId}");

            var userRoles = _context.UserRoles.Where(ur => ur.UserId == userId);
            _context.UserRoles.RemoveRange(userRoles);
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
                return BadRequest();

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound();

            // Remove existing roles
            var userRoles = _context.UserRoles.Where(ur => ur.UserId == userId);
            _context.UserRoles.RemoveRange(userRoles);

            // Find the role by name
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                ModelState.AddModelError("", "Role not found.");
                return RedirectToAction("Index");
            }

            // Add new role
            _context.UserRoles.Add(new IdentityUserRole<string>
            {
                UserId = userId,
                RoleId = role.Id
            });

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ------------ SONG METHODS ------------

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSongConfirmed(int id)
        {
            var song = await _context.Songs
                .Include(s => s.AlbumSongs)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (song == null)
                return NotFound();

            _context.AlbumSongs.RemoveRange(song.AlbumSongs);
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ------------ ALBUM METHODS ------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // ------------ ARTIST METHODS ------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}

