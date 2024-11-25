using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentProvider.Migrations
{
    /// <inheritdoc />
    public partial class returnsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReturnId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReturnEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResolutionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReturnId",
                table: "Orders",
                column: "ReturnId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ReturnEntity_ReturnId",
                table: "Orders",
                column: "ReturnId",
                principalTable: "ReturnEntity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ReturnEntity_ReturnId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ReturnEntity");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ReturnId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReturnId",
                table: "Orders");
        }
    }
}
