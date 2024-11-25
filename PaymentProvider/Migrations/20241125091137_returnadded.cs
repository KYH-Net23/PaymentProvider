using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentProvider.Migrations
{
    /// <inheritdoc />
    public partial class returnadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ReturnEntity_ReturnId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturnEntity",
                table: "ReturnEntity");

            migrationBuilder.RenameTable(
                name: "ReturnEntity",
                newName: "Returns");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Returns",
                table: "Returns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Returns_ReturnId",
                table: "Orders",
                column: "ReturnId",
                principalTable: "Returns",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Returns_ReturnId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Returns",
                table: "Returns");

            migrationBuilder.RenameTable(
                name: "Returns",
                newName: "ReturnEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturnEntity",
                table: "ReturnEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ReturnEntity_ReturnId",
                table: "Orders",
                column: "ReturnId",
                principalTable: "ReturnEntity",
                principalColumn: "Id");
        }
    }
}
