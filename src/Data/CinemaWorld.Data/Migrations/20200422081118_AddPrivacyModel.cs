namespace CinemaWorld.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;
    public partial class AddPrivacyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Privacies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    PageContent = table.Column<string>(maxLength: 15000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privacies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Privacies_IsDeleted",
                table: "Privacies",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Privacies");
        }
    }
}
