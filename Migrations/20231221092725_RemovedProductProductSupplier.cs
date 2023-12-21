using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class RemovedProductProductSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductSupplier");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductSupplierId",
                table: "Products",
                column: "ProductSupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_ProductSupplierId",
                table: "Products",
                column: "ProductSupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_ProductSupplierId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductSupplierId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductProductSupplier",
                columns: table => new
                {
                    ProductSuppliersId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductSupplier", x => new { x.ProductSuppliersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductProductSupplier_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductSupplier_Suppliers_ProductSuppliersId",
                        column: x => x.ProductSuppliersId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductSupplier_ProductsId",
                table: "ProductProductSupplier",
                column: "ProductsId");
        }
    }
}
