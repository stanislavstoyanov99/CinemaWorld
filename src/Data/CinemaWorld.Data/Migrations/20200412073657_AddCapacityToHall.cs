namespace CinemaWorld.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddCapacityToHall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Halls",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Halls");
        }
    }
}
