using MelodyApp.Data;
using MelodyApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace MyMusicApp.Tests
{
    public class SongTests : TestBase
    {
        private DbContextOptions<ApplicationDbContext> GetInMemoryOptions(string dbName)
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName) // unique name per test
                .Options;
        }

        private ApplicationDbContext GetDbContext([CallerMemberName] string testName = null)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(testName ?? Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            // Just in case, clear the DB (not always needed, but safe)
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public void Test_Songs_ShouldBeEmpty_WhenNoSeeding()
        {
            var context = GetDbContext(nameof(Test_Songs_ShouldBeEmpty_WhenNoSeeding));

            var songs = context.Songs.ToList();

            Assert.Empty(songs); // ✅ Should pass now
        }

        [Fact]
        public async Task CreateSong_ShouldAddSong()
        {
            var context = GetDbContext(nameof(CreateSong_ShouldAddSong));
            var genre = new Genre { Name = "Pop" };
            context.Genres.Add(genre);
            await context.SaveChangesAsync();

            var song = new Song { Title = "New Song", GenreId = genre.Id };

            context.Songs.Add(song);
            await context.SaveChangesAsync();

            Assert.Single(context.Songs);
        }

        [Fact]
        public async Task GetSongById_ShouldReturnCorrectSong()
        {
            var context = GetDbContext();
            var song = new Song { Title = "Get Me" };
            context.Songs.Add(song);
            await context.SaveChangesAsync();

            var result = await context.Songs.FindAsync(song.Id);

            Assert.NotNull(result);
            Assert.Equal("Get Me", result.Title);
        }

        [Fact]
        public void SearchSongs_ShouldReturnFilteredResults()
        {
            var context = GetDbContext();
            context.Songs.AddRange(
                new Song { Title = "Chill Beats" },
                new Song { Title = "Dance Floor" }
            );
            context.SaveChanges();

            var result = context.Songs.Where(s => s.Title.Contains("Chill")).ToList();

            Assert.Single(result);
            Assert.Equal("Chill Beats", result[0].Title);
        }

        [Fact]
        public async Task DeleteSong_ShouldRemoveSong()
        {
            var context = GetDbContext(nameof(DeleteSong_ShouldRemoveSong));

            var genre = new Genre { Name = "Pop" };
            context.Genres.Add(genre);
            await context.SaveChangesAsync();

            var song = new Song { Title = "Delete Me", GenreId = genre.Id };
            context.Songs.Add(song);
            await context.SaveChangesAsync();

            context.Songs.Remove(song);
            await context.SaveChangesAsync();

            Assert.Empty(context.Songs);
        }
    }
}
