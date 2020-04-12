namespace CinemaWorld.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddNewPropertiesToSeatModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvaible",
                table: "Seats",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Seats",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvaible",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Seats");
        }
    }
}
