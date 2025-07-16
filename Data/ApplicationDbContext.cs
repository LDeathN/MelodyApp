using Humanizer.Localisation;
using MelodyApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MelodyApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
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

            builder.Entity<Artist>(entity =>
            {
                entity.HasMany(a => a.Songs)
                      .WithOne(s => s.Artist)
                      .HasForeignKey(s => s.ArtistId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // 🎤 Seed Artists
            var artists = new[]
            {
                new Artist { Id = 1, Name = "Queen" },
                new Artist { Id = 2, Name = "The Weeknd" },
                new Artist { Id = 3, Name = "Eminem" }
            };
            
            // 🎵 Seed Genres
            var genres = new[]
            {
                new Genre { Id = 1, Name = "Rock" },
                new Genre { Id = 2, Name = "Pop" },
                new Genre { Id = 3, Name = "Hip-Hop" },
                new Genre { Id = 4, Name = "Jazz" },
                new Genre { Id = 5, Name = "Electronic" }
            };
            
            // 🎶 Seed Songs
            var songs = new[]
            {
                new Song { Id = 1, Title = "Bohemian Rhapsody", ArtistId = 1, GenreId = 1, Url = "/songs/bohemian_rhapsody.mp3" },
                new Song { Id = 2, Title = "Blinding Lights", ArtistId = 2, GenreId = 2, Url = "/songs/blinding_lights.mp3" },
                new Song { Id = 3, Title = "Lose Yourself", ArtistId = 3, GenreId = 3, Url = "/songs/lose_yourself.mp3" }
            };
            
            // 💿 Seed Album
            var album = new Album
            {
                Id = 1,
                Title = "Greatest Hits",
                Description = "A mix of legendary tracks.",
                CoverImageUrl = "/images/greatest_hits.jpg",
                UserId = "admin-placeholder" // update later
            };
            
            // 🔗 Seed Album-Song relations
            var albumSongs = new[]
            {
                new AlbumSong { AlbumId = 1, SongId = 1 },
                new AlbumSong { AlbumId = 1, SongId = 2 },
                new AlbumSong { AlbumId = 1, SongId = 3 }
            };
            
            builder.Entity<Artist>().HasData(artists);
            builder.Entity<Genre>().HasData(genres);
            builder.Entity<Song>().HasData(songs);
        }
        
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<FavoriteSong> FavoriteSongs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<AlbumSong> AlbumSongs { get; set; }
        public DbSet<Artist> Artists { get; set; }
    }
}
