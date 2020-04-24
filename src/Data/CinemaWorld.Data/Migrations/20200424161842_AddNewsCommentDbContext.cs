namespace CinemaWorld.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddNewsCommentDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsComment_News_NewsId",
                table: "NewsComment");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsComment_NewsComment_ParentId",
                table: "NewsComment");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsComment_AspNetUsers_UserId",
                table: "NewsComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsComment",
                table: "NewsComment");

            migrationBuilder.RenameTable(
                name: "NewsComment",
                newName: "NewsComments");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComment_UserId",
                table: "NewsComments",
                newName: "IX_NewsComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComment_ParentId",
                table: "NewsComments",
                newName: "IX_NewsComments_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComment_NewsId",
                table: "NewsComments",
                newName: "IX_NewsComments_NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComment_IsDeleted",
                table: "NewsComments",
                newName: "IX_NewsComments_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsComments",
                table: "NewsComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_News_NewsId",
                table: "NewsComments",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_NewsComments_ParentId",
                table: "NewsComments",
                column: "ParentId",
                principalTable: "NewsComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_AspNetUsers_UserId",
                table: "NewsComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_News_NewsId",
                table: "NewsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_NewsComments_ParentId",
                table: "NewsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_AspNetUsers_UserId",
                table: "NewsComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsComments",
                table: "NewsComments");

            migrationBuilder.RenameTable(
                name: "NewsComments",
                newName: "NewsComment");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComments_UserId",
                table: "NewsComment",
                newName: "IX_NewsComment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComments_ParentId",
                table: "NewsComment",
                newName: "IX_NewsComment_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComments_NewsId",
                table: "NewsComment",
                newName: "IX_NewsComment_NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsComments_IsDeleted",
                table: "NewsComment",
                newName: "IX_NewsComment_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsComment",
                table: "NewsComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComment_News_NewsId",
                table: "NewsComment",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComment_NewsComment_ParentId",
                table: "NewsComment",
                column: "ParentId",
                principalTable: "NewsComment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComment_AspNetUsers_UserId",
                table: "NewsComment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
