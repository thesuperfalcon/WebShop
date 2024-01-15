using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdressToDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdressDelivery",
                columns: table => new
                {
                    AdressesId = table.Column<int>(type: "int", nullable: false),
                    DeliveriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdressDelivery", x => new { x.AdressesId, x.DeliveriesId });
                    table.ForeignKey(
                        name: "FK_AdressDelivery_Adresses_AdressesId",
                        column: x => x.AdressesId,
                        principalTable: "Adresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdressDelivery_Deliveries_DeliveriesId",
                        column: x => x.DeliveriesId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdressDelivery_DeliveriesId",
                table: "AdressDelivery",
                column: "DeliveriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdressDelivery");
        }
    }
}
