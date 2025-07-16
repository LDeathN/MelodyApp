using MelodyApp.Models;
using Microsoft.AspNetCore.Identity;

namespace MelodyApp.Data
{
    public static class SeedData
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            string adminEmail = "admin@melodyhub.com";
            string password = "Admin123!";

            // Create roles
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            // Create admin user
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(newAdmin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");

                    // Seed album and relations
                    var album = new Album
                    {
                        Title = "Greatest Hits",
                        Description = "A mix of legendary tracks.",
                        CoverImageUrl = "/images/greatest_hits.jpg",
                        UserId = newAdmin.Id
                    };

                    db.Albums.Add(album);
                    await db.SaveChangesAsync(); // So album gets an Id

                    var albumSongs = new List<AlbumSong>
                {
                    new AlbumSong { AlbumId = album.Id, SongId = 1 },
                    new AlbumSong { AlbumId = album.Id, SongId = 2 },
                    new AlbumSong { AlbumId = album.Id, SongId = 3 }
                };

                    db.AlbumSongs.AddRange(albumSongs);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
