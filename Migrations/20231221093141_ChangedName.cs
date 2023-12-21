using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class ChangedName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_ProductSupplierId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                newName: "ProductSuppliers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSuppliers",
                table: "ProductSuppliers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductSuppliers_ProductSupplierId",
                table: "Products",
                column: "ProductSupplierId",
                principalTable: "ProductSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductSuppliers_ProductSupplierId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSuppliers",
                table: "ProductSuppliers");

            migrationBuilder.RenameTable(
                name: "ProductSuppliers",
                newName: "Suppliers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_ProductSupplierId",
                table: "Products",
                column: "ProductSupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
