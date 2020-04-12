namespace CinemaWorld.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class FixTypoErrorInIsAvailableProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvaible",
                table: "Seats");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Seats",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Seats");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvaible",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
