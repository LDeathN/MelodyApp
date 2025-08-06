using MelodyApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyMusicApp.Tests
{
    public class FavoritesTests : TestBase
    {
        [Fact]
        public async Task AddToFavorites_ShouldAddFavorite()
        {
            var context = GetDbContext();
            var userId = "user123";
            var song = new Song { Title = "Favorite Me" };
            context.Songs.Add(song);
            await context.SaveChangesAsync();

            var favorite = new FavoriteSong
            {
                SongId = song.Id,
                UserId = userId
            };

            context.FavoriteSongs.Add(favorite);
            await context.SaveChangesAsync();

            Assert.True(context.FavoriteSongs.Any(f => f.SongId == song.Id && f.UserId == userId));
        }

        [Fact]
        public async Task RemoveFromFavorites_ShouldRemoveFavorite()
        {
            var context = GetDbContext();
            var userId = "user456";
            var song = new Song { Title = "Unfavorite Me" };
            context.Songs.Add(song);
            await context.SaveChangesAsync();

            var favorite = new FavoriteSong { SongId = song.Id, UserId = userId };
            context.FavoriteSongs.Add(favorite);
            await context.SaveChangesAsync();

            context.FavoriteSongs.Remove(favorite);
            await context.SaveChangesAsync();

            Assert.False(context.FavoriteSongs.Any(f => f.UserId == userId));
        }

        [Fact]
        public async Task IsSongFavorited_ShouldReturnTrue()
        {
            var context = GetDbContext();
            var userId = "testUser";
            var song = new Song { Title = "TestFav" };
            context.Songs.Add(song);
            await context.SaveChangesAsync();

            context.FavoriteSongs.Add(new FavoriteSong { SongId = song.Id, UserId = userId });
            await context.SaveChangesAsync();

            var isFavorited = context.FavoriteSongs.Any(f => f.UserId == userId && f.SongId == song.Id);
            Assert.True(isFavorited);
        }
    }
}
