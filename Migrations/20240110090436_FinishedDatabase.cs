using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class FinishedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrderProductOrder_ProductOrder_ProductOrdersId",
                table: "FinalOrderProductOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrder_Products_ProductId",
                table: "ProductOrder");

            migrationBuilder.DropTable(
                name: "ColourProduct");

            migrationBuilder.DropTable(
                name: "DeliveryDeliveryType");

            migrationBuilder.DropTable(
                name: "PaymentPaymentType");

            migrationBuilder.DropTable(
                name: "ProductSize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductOrder",
                table: "ProductOrder");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrder_ProductId",
                table: "ProductOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinalOrder",
                table: "FinalOrder");

            migrationBuilder.DropColumn(
                name: "PaymentName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeliveryName",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ColourId",
                table: "ProductOrder");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductOrder");

            migrationBuilder.RenameTable(
                name: "ProductOrder",
                newName: "ProductOrders");

            migrationBuilder.RenameTable(
                name: "FinalOrder",
                newName: "FinalOrders");

            migrationBuilder.RenameColumn(
                name: "DeliveryTypeName",
                table: "DeliveryTypes",
                newName: "DeliveryName");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "ProductOrders",
                newName: "ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ProductOrders",
                newName: "TotalPrice");

            migrationBuilder.RenameIndex(
                name: "IX_FinalOrder_PaymentId",
                table: "FinalOrders",
                newName: "IX_FinalOrders_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_FinalOrder_DeliveryId",
                table: "FinalOrders",
                newName: "IX_FinalOrders_DeliveryId");

            migrationBuilder.RenameIndex(
                name: "IX_FinalOrder_CustomerId",
                table: "FinalOrders",
                newName: "IX_FinalOrders_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "PaymentNameId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryNameId",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryTypeId",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductOrders",
                table: "ProductOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinalOrders",
                table: "FinalOrders",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DeliveryNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ColourId = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Colours_ColourId",
                        column: x => x.ColourId,
                        principalTable: "Colours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentNameId",
                table: "Payments",
                column: "PaymentNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentTypeId",
                table: "Payments",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveryNameId",
                table: "Deliveries",
                column: "DeliveryNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveryTypeId",
                table: "Deliveries",
                column: "DeliveryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrders_ProductVariantId",
                table: "ProductOrders",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ColourId",
                table: "ProductVariants",
                column: "ColourId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_SizeId",
                table: "ProductVariants",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_DeliveryNames_DeliveryNameId",
                table: "Deliveries",
                column: "DeliveryNameId",
                principalTable: "DeliveryNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_DeliveryTypes_DeliveryTypeId",
                table: "Deliveries",
                column: "DeliveryTypeId",
                principalTable: "DeliveryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrderProductOrder_FinalOrders_FinalOrdersId",
                table: "FinalOrderProductOrder",
                column: "FinalOrdersId",
                principalTable: "FinalOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrderProductOrder_ProductOrders_ProductOrdersId",
                table: "FinalOrderProductOrder",
                column: "ProductOrdersId",
                principalTable: "ProductOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrders_Customers_CustomerId",
                table: "FinalOrders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrders_Deliveries_DeliveryId",
                table: "FinalOrders",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrders_Payments_PaymentId",
                table: "FinalOrders",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentNames_PaymentNameId",
                table: "Payments",
                column: "PaymentNameId",
                principalTable: "PaymentNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentTypes_PaymentTypeId",
                table: "Payments",
                column: "PaymentTypeId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrders_ProductVariants_ProductVariantId",
                table: "ProductOrders",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_DeliveryNames_DeliveryNameId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_DeliveryTypes_DeliveryTypeId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrderProductOrder_FinalOrders_FinalOrdersId",
                table: "FinalOrderProductOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrderProductOrder_ProductOrders_ProductOrdersId",
                table: "FinalOrderProductOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrders_Customers_CustomerId",
                table: "FinalOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrders_Deliveries_DeliveryId",
                table: "FinalOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalOrders_Payments_PaymentId",
                table: "FinalOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentNames_PaymentNameId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentTypes_PaymentTypeId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrders_ProductVariants_ProductVariantId",
                table: "ProductOrders");

            migrationBuilder.DropTable(
                name: "DeliveryNames");

            migrationBuilder.DropTable(
                name: "PaymentNames");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentNameId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentTypeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_DeliveryNameId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_DeliveryTypeId",
                table: "Deliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductOrders",
                table: "ProductOrders");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrders_ProductVariantId",
                table: "ProductOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinalOrders",
                table: "FinalOrders");

            migrationBuilder.DropColumn(
                name: "PaymentNameId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeliveryNameId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryTypeId",
                table: "Deliveries");

            migrationBuilder.RenameTable(
                name: "ProductOrders",
                newName: "ProductOrder");

            migrationBuilder.RenameTable(
                name: "FinalOrders",
                newName: "FinalOrder");

            migrationBuilder.RenameColumn(
                name: "DeliveryName",
                table: "DeliveryTypes",
                newName: "DeliveryTypeName");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "ProductOrder",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "ProductOrder",
                newName: "SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_FinalOrders_PaymentId",
                table: "FinalOrder",
                newName: "IX_FinalOrder_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_FinalOrders_DeliveryId",
                table: "FinalOrder",
                newName: "IX_FinalOrder_DeliveryId");

            migrationBuilder.RenameIndex(
                name: "IX_FinalOrders_CustomerId",
                table: "FinalOrder",
                newName: "IX_FinalOrder_CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "PaymentName",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryName",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ColourId",
                table: "ProductOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductOrder",
                table: "ProductOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinalOrder",
                table: "FinalOrder",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ColourProduct",
                columns: table => new
                {
                    ColoursId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColourProduct", x => new { x.ColoursId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ColourProduct_Colours_ColoursId",
                        column: x => x.ColoursId,
                        principalTable: "Colours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColourProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "ProductSize",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    SizesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSize", x => new { x.ProductsId, x.SizesId });
                    table.ForeignKey(
                        name: "FK_ProductSize_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSize_Sizes_SizesId",
                        column: x => x.SizesId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrder_ProductId",
                table: "ProductOrder",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ColourProduct_ProductsId",
                table: "ColourProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDeliveryType_DeliveryTypesId",
                table: "DeliveryDeliveryType",
                column: "DeliveryTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentPaymentType_PaymentsId",
                table: "PaymentPaymentType",
                column: "PaymentsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSize_SizesId",
                table: "ProductSize",
                column: "SizesId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOrderProductOrder_ProductOrder_ProductOrdersId",
                table: "FinalOrderProductOrder",
                column: "ProductOrdersId",
                principalTable: "ProductOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrder_Products_ProductId",
                table: "ProductOrder",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
