using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class AddedFinalOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrderProductOrder_Orders_FinalOrdersId",
                table: "FinalOrderProductOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Deliveries_DeliveryId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "FinalOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_PaymentId",
                table: "FinalOrder",
                newName: "IX_FinalOrder_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryId",
                table: "FinalOrder",
                newName: "IX_FinalOrder_DeliveryId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "FinalOrder",
                newName: "IX_FinalOrder_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinalOrder",
                table: "FinalOrder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrder_Customers_CustomerId",
                table: "FinalOrder",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrder_Deliveries_DeliveryId",
                table: "FinalOrder",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrder_Payments_PaymentId",
                table: "FinalOrder",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrderProductOrder_FinalOrder_FinalOrdersId",
                table: "FinalOrderProductOrder",
                column: "FinalOrdersId",
                principalTable: "FinalOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrder_Customers_CustomerId",
                table: "FinalOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrder_Deliveries_DeliveryId",
                table: "FinalOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrder_Payments_PaymentId",
                table: "FinalOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrderProductOrder_FinalOrder_FinalOrdersId",
                table: "FinalOrderProductOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinalOrder",
                table: "FinalOrder");

            migrationBuilder.RenameTable(
                name: "FinalOrder",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_FinalOrder_PaymentId",
                table: "Orders",
                newName: "IX_Orders_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_FinalOrder_DeliveryId",
                table: "Orders",
                newName: "IX_Orders_DeliveryId");

            migrationBuilder.RenameIndex(
                name: "IX_FinalOrder_CustomerId",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrderProductOrder_Orders_FinalOrdersId",
                table: "FinalOrderProductOrder",
                column: "FinalOrdersId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Deliveries_DeliveryId",
                table: "Orders",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
