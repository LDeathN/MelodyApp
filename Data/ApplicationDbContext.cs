using Humanizer.Localisation;
using MelodyApp.Models;
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FavoriteSong>(entity =>
    {
                entity.HasKey(fs => new { fs.UserId, fs.SongId })
                      .IsClustered(false);

                entity.HasOne(fs => fs.User)
                      .WithMany(u => u.FavoriteSongs)
                      .HasForeignKey(fs => fs.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(fs => fs.Song)
                      .WithMany(s => s.FavoritedBy)
                      .HasForeignKey(fs => fs.SongId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<AlbumSong>(entity =>
            {
                entity.HasKey(asg => new { asg.AlbumId, asg.SongId });

                entity.HasOne(asg => asg.Album)
                      .WithMany(a => a.AlbumSongs)
                      .HasForeignKey(asg => asg.AlbumId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(asg => asg.Song)
                      .WithMany(s => s.AlbumSongs)
                      .HasForeignKey(asg => asg.SongId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
        
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<FavoriteSong> FavoriteSongs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<AlbumSong> AlbumSongs { get; set; }
    }
}
