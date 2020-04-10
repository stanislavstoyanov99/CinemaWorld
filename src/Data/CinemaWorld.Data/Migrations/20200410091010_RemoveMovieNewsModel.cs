namespace CinemaWorld.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;
    public partial class RemoveMovieNewsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieNews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieNews",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    NewsId = table.Column<int>(type: "int", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieNews", x => new { x.MovieId, x.NewsId });
                    table.ForeignKey(
                        name: "FK_MovieNews_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovieNews_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieNews_IsDeleted",
                table: "MovieNews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MovieNews_NewsId",
                table: "MovieNews",
                column: "NewsId");
        }
    }
}
