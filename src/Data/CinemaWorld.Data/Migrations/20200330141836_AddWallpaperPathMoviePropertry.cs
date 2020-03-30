namespace CinemaWorld.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddWallpaperPathMoviePropertry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WallpaperPath",
                table: "Movies",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WallpaperPath",
                table: "Movies");
        }
    }
}
