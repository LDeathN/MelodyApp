using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MelodyApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //TODO
        //public DbSet<Song> Songs { get; set; }
        //public DbSet<Album> Albums { get; set; }
        //public DbSet<FavoriteSong> FavoriteSongs { get; set; }
        //public DbSet<Genre> Genres { get; set; }
    }
}
