using Microsoft.EntityFrameworkCore.Migrations;

namespace MedalsWebSystem.Migrations
{
    public partial class userRole_productUserAdder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserAdderUserId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserAdderUserId",
                table: "Products",
                column: "UserAdderUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_UserAdderUserId",
                table: "Products",
                column: "UserAdderUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_UserAdderUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserAdderUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserAdderUserId",
                table: "Products");
        }
    }
}
