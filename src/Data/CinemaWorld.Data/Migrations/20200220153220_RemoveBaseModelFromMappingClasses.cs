namespace CinemaWorld.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemoveBaseModelFromMappingClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewAuthors",
                table: "ReviewAuthors");

            migrationBuilder.DropIndex(
                name: "IX_ReviewAuthors_ReviewId",
                table: "ReviewAuthors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieNews",
                table: "MovieNews");

            migrationBuilder.DropIndex(
                name: "IX_MovieNews_MovieId",
                table: "MovieNews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_MovieId",
                table: "MovieGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieDirectors",
                table: "MovieDirectors");

            migrationBuilder.DropIndex(
                name: "IX_MovieDirectors_MovieId",
                table: "MovieDirectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieCountries",
                table: "MovieCountries");

            migrationBuilder.DropIndex(
                name: "IX_MovieCountries_MovieId",
                table: "MovieCountries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieActors",
                table: "MovieActors");

            migrationBuilder.DropIndex(
                name: "IX_MovieActors_MovieId",
                table: "MovieActors");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ReviewAuthors");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ReviewAuthors");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ReviewAuthors");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MovieNews");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MovieNews");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "MovieNews");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MovieDirectors");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MovieDirectors");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "MovieDirectors");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MovieCountries");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MovieCountries");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "MovieCountries");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MovieActors");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MovieActors");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "MovieActors");

            migrationBuilder.AlterColumn<int>(
                name: "NewsId",
                table: "MovieNews",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewAuthors",
                table: "ReviewAuthors",
                columns: new[] { "ReviewId", "AuthorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieNews",
                table: "MovieNews",
                columns: new[] { "MovieId", "NewsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres",
                columns: new[] { "MovieId", "GenreId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieDirectors",
                table: "MovieDirectors",
                columns: new[] { "MovieId", "DirectorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieCountries",
                table: "MovieCountries",
                columns: new[] { "MovieId", "CountryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieActors",
                table: "MovieActors",
                columns: new[] { "MovieId", "ActorId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewAuthors",
                table: "ReviewAuthors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieNews",
                table: "MovieNews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieDirectors",
                table: "MovieDirectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieCountries",
                table: "MovieCountries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieActors",
                table: "MovieActors");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ReviewAuthors",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ReviewAuthors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ReviewAuthors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NewsId",
                table: "MovieNews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MovieNews",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MovieNews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "MovieNews",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MovieGenres",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MovieGenres",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "MovieGenres",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MovieDirectors",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MovieDirectors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "MovieDirectors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MovieCountries",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MovieCountries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "MovieCountries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MovieActors",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MovieActors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "MovieActors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewAuthors",
                table: "ReviewAuthors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieNews",
                table: "MovieNews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieDirectors",
                table: "MovieDirectors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieCountries",
                table: "MovieCountries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieActors",
                table: "MovieActors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewAuthors_ReviewId",
                table: "ReviewAuthors",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieNews_MovieId",
                table: "MovieNews",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_MovieId",
                table: "MovieGenres",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieDirectors_MovieId",
                table: "MovieDirectors",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCountries_MovieId",
                table: "MovieCountries",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActors_MovieId",
                table: "MovieActors",
                column: "MovieId");
        }
    }
}
