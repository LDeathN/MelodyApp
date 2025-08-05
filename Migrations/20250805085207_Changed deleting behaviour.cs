using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MelodyApp.Migrations
{
    /// <inheritdoc />
    public partial class Changeddeletingbehaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumSongs_Albums_AlbumId",
                table: "AlbumSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_AlbumSongs_Songs_SongId",
                table: "AlbumSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteSongs_AspNetUsers_UserId",
                table: "FavoriteSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteSongs_Songs_SongId",
                table: "FavoriteSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumSongs_Albums_AlbumId",
                table: "AlbumSongs",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumSongs_Songs_SongId",
                table: "AlbumSongs",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteSongs_AspNetUsers_UserId",
                table: "FavoriteSongs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteSongs_Songs_SongId",
                table: "FavoriteSongs",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumSongs_Albums_AlbumId",
                table: "AlbumSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_AlbumSongs_Songs_SongId",
                table: "AlbumSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteSongs_AspNetUsers_UserId",
                table: "FavoriteSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteSongs_Songs_SongId",
                table: "FavoriteSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumSongs_Albums_AlbumId",
                table: "AlbumSongs",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumSongs_Songs_SongId",
                table: "AlbumSongs",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteSongs_AspNetUsers_UserId",
                table: "FavoriteSongs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteSongs_Songs_SongId",
                table: "FavoriteSongs",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
