using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class ChangeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProductOrder");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ProductOrder",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "FinalOrderProductOrder",
                columns: table => new
                {
                    FinalOrdersId = table.Column<int>(type: "int", nullable: false),
                    ProductOrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalOrderProductOrder", x => new { x.FinalOrdersId, x.ProductOrdersId });
                    table.ForeignKey(
                        name: "FK_FinalOrderProductOrder_Orders_FinalOrdersId",
                        column: x => x.FinalOrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinalOrderProductOrder_ProductOrder_ProductOrdersId",
                        column: x => x.ProductOrdersId,
                        principalTable: "ProductOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalOrderProductOrder_ProductOrdersId",
                table: "FinalOrderProductOrder",
                column: "ProductOrdersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalOrderProductOrder");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductOrder");

            migrationBuilder.CreateTable(
                name: "OrderProductOrder",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    ProductOrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProductOrder", x => new { x.OrdersId, x.ProductOrdersId });
                    table.ForeignKey(
                        name: "FK_OrderProductOrder_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProductOrder_ProductOrder_ProductOrdersId",
                        column: x => x.ProductOrdersId,
                        principalTable: "ProductOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductOrder_ProductOrdersId",
                table: "OrderProductOrder",
                column: "ProductOrdersId");
        }
    }
}
