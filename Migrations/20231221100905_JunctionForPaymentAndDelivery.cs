using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class JunctionForPaymentAndDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_DeliveryTypes_DeliveryTypeId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentTypes_PaymentTypeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentTypeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_DeliveryTypeId",
                table: "Deliveries");

            migrationBuilder.CreateTable(
                name: "DeliveryDeliveryType",
                columns: table => new
                {
                    DeliveriesId = table.Column<int>(type: "int", nullable: false),
                    DeliveryTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDeliveryType", x => new { x.DeliveriesId, x.DeliveryTypesId });
                    table.ForeignKey(
                        name: "FK_DeliveryDeliveryType_Deliveries_DeliveriesId",
                        column: x => x.DeliveriesId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryDeliveryType_DeliveryTypes_DeliveryTypesId",
                        column: x => x.DeliveryTypesId,
                        principalTable: "DeliveryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentPaymentType",
                columns: table => new
                {
                    PaymentTypesId = table.Column<int>(type: "int", nullable: false),
                    PaymentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentPaymentType", x => new { x.PaymentTypesId, x.PaymentsId });
                    table.ForeignKey(
                        name: "FK_PaymentPaymentType_PaymentTypes_PaymentTypesId",
                        column: x => x.PaymentTypesId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentPaymentType_Payments_PaymentsId",
                        column: x => x.PaymentsId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDeliveryType_DeliveryTypesId",
                table: "DeliveryDeliveryType",
                column: "DeliveryTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentPaymentType_PaymentsId",
                table: "PaymentPaymentType",
                column: "PaymentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryDeliveryType");

            migrationBuilder.DropTable(
                name: "PaymentPaymentType");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentTypeId",
                table: "Payments",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveryTypeId",
                table: "Deliveries",
                column: "DeliveryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_DeliveryTypes_DeliveryTypeId",
                table: "Deliveries",
                column: "DeliveryTypeId",
                principalTable: "DeliveryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentTypes_PaymentTypeId",
                table: "Payments",
                column: "PaymentTypeId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
