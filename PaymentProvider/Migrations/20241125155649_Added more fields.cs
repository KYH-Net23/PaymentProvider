using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentProvider.Migrations
{
    /// <inheritdoc />
    public partial class Addedmorefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "Model");

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "ShippingDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountedPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "ShippingDetails");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Products",
                newName: "Name");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountedPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
