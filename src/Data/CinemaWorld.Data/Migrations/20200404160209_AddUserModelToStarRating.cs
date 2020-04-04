namespace CinemaWorld.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddUserModelToStarRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "StarRatings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "StarRatings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StarRatings_UserId1",
                table: "StarRatings",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StarRatings_AspNetUsers_UserId1",
                table: "StarRatings",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StarRatings_AspNetUsers_UserId1",
                table: "StarRatings");

            migrationBuilder.DropIndex(
                name: "IX_StarRatings_UserId1",
                table: "StarRatings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StarRatings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "StarRatings");
        }
    }
}
