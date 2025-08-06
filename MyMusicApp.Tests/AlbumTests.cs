using MelodyApp.Data;
using MelodyApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyMusicApp.Tests
{
    public class AlbumTests : TestBase
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task CreateAlbum_ShouldAddAlbum()
        {
            var context = GetDbContext();
            var album = new Album { Title = "Test Album", UserId = "1"};

            context.Albums.Add(album);
            await context.SaveChangesAsync();

            Assert.Single(context.Albums);
        }

        [Fact]
        public async Task GetAlbumById_ShouldReturnCorrectAlbum()
        {
            var context = GetDbContext();
            var album = new Album { Title = "My Album", UserId = "1" };
            context.Albums.Add(album);
            await context.SaveChangesAsync();

            var result = await context.Albums.FindAsync(album.Id);

            Assert.NotNull(result);
            Assert.Equal("My Album", result.Title);
        }

        [Fact]
        public void SearchAlbums_ShouldReturnFilteredResults()
        {
            var context = GetDbContext();
            context.Albums.AddRange(
                new Album { Title = "Love Songs", UserId = "1" },
                new Album { Title = "Workout Mix", UserId = "1" }
            );
            context.SaveChanges();

            var result = context.Albums.Where(a => a.Title.Contains("Love")).ToList();

            Assert.Single(result);
            Assert.Equal("Love Songs", result[0].Title);
        }

        [Fact]
        public async Task DeleteAlbum_ShouldRemoveAlbum()
        {
            var context = GetDbContext();
            var album = new Album { Title = "ToDelete", UserId = "1" };
            context.Albums.Add(album);
            await context.SaveChangesAsync();

            context.Albums.Remove(album);
            await context.SaveChangesAsync();

            Assert.Empty(context.Albums);
        }
    }
}
