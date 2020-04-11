namespace CinemaWorld.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddShoppingCartModelToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleTransactions_TicketCarts_ShoppingCartId",
                table: "SaleTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketCarts",
                table: "TicketCarts");

            migrationBuilder.RenameTable(
                name: "TicketCarts",
                newName: "ShoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ShoppingCarts",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCarts",
                table: "ShoppingCarts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShoppingCartId",
                table: "AspNetUsers",
                column: "ShoppingCartId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ShoppingCarts_ShoppingCartId",
                table: "AspNetUsers",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleTransactions_ShoppingCarts_ShoppingCartId",
                table: "SaleTransactions",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ShoppingCarts_ShoppingCartId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleTransactions_ShoppingCarts_ShoppingCartId",
                table: "SaleTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ShoppingCartId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCarts",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingCarts");

            migrationBuilder.RenameTable(
                name: "ShoppingCarts",
                newName: "TicketCarts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketCarts",
                table: "TicketCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleTransactions_TicketCarts_ShoppingCartId",
                table: "SaleTransactions",
                column: "ShoppingCartId",
                principalTable: "TicketCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
