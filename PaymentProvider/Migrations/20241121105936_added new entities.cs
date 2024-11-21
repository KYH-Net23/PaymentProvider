using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentProvider.Migrations
{
    /// <inheritdoc />
    public partial class addednewentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "ShippingDetails");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "ShippingDetails");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "ShippingDetails");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ShippingDetails");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "ShippingDetails");

            migrationBuilder.DropColumn(
                name: "PostalPickUpAddress",
                table: "ShippingDetails");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "ShippingDetails");

            migrationBuilder.AddColumn<int>(
                name: "CustomerDeliveryInformationId",
                table: "ShippingDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PostalAgentDeliveryInformationId",
                table: "ShippingDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CustomerDeliveryInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDeliveryInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostalAgentDeliveryInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostalAgentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostalAgentDeliveryInformation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingDetails_CustomerDeliveryInformationId",
                table: "ShippingDetails",
                column: "CustomerDeliveryInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingDetails_PostalAgentDeliveryInformationId",
                table: "ShippingDetails",
                column: "PostalAgentDeliveryInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingDetails_CustomerDeliveryInformation_CustomerDeliveryInformationId",
                table: "ShippingDetails",
                column: "CustomerDeliveryInformationId",
                principalTable: "CustomerDeliveryInformation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingDetails_PostalAgentDeliveryInformation_PostalAgentDeliveryInformationId",
                table: "ShippingDetails",
                column: "PostalAgentDeliveryInformationId",
                principalTable: "PostalAgentDeliveryInformation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingDetails_CustomerDeliveryInformation_CustomerDeliveryInformationId",
                table: "ShippingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingDetails_PostalAgentDeliveryInformation_PostalAgentDeliveryInformationId",
                table: "ShippingDetails");

            migrationBuilder.DropTable(
                name: "CustomerDeliveryInformation");

            migrationBuilder.DropTable(
                name: "PostalAgentDeliveryInformation");

            migrationBuilder.DropIndex(
                name: "IX_ShippingDetails_CustomerDeliveryInformationId",
                table: "ShippingDetails");

            migrationBuilder.DropIndex(
                name: "IX_ShippingDetails_PostalAgentDeliveryInformationId",
                table: "ShippingDetails");

            migrationBuilder.DropColumn(
                name: "CustomerDeliveryInformationId",
                table: "ShippingDetails");

            migrationBuilder.DropColumn(
                name: "PostalAgentDeliveryInformationId",
                table: "ShippingDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ShippingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ShippingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "ShippingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ShippingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "ShippingDetails",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalPickUpAddress",
                table: "ShippingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "ShippingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
