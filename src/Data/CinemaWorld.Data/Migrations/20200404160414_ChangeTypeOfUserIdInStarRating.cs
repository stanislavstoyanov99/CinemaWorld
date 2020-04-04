namespace CinemaWorld.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangeTypeOfUserIdInStarRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StarRatings_AspNetUsers_UserId1",
                table: "StarRatings");

            migrationBuilder.DropIndex(
                name: "IX_StarRatings_UserId1",
                table: "StarRatings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "StarRatings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "StarRatings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_StarRatings_UserId",
                table: "StarRatings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StarRatings_AspNetUsers_UserId",
                table: "StarRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StarRatings_AspNetUsers_UserId",
                table: "StarRatings");

            migrationBuilder.DropIndex(
                name: "IX_StarRatings_UserId",
                table: "StarRatings");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "StarRatings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "StarRatings",
                type: "nvarchar(450)",
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
    }
}
