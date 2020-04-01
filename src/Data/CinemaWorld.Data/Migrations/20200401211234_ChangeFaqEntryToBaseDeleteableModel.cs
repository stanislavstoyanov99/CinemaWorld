namespace CinemaWorld.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangeFaqEntryToBaseDeleteableModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "FaqEntries",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FaqEntries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_FaqEntries_IsDeleted",
                table: "FaqEntries",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FaqEntries_IsDeleted",
                table: "FaqEntries");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "FaqEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FaqEntries");
        }
    }
}
