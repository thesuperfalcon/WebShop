using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class addedToProductOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SizeId",
                table: "ProductOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SizeId1",
                table: "ProductOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrder_SizeId1",
                table: "ProductOrder",
                column: "SizeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrder_Sizes_SizeId1",
                table: "ProductOrder",
                column: "SizeId1",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrder_Sizes_SizeId1",
                table: "ProductOrder");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrder_SizeId1",
                table: "ProductOrder");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "ProductOrder");

            migrationBuilder.DropColumn(
                name: "SizeId1",
                table: "ProductOrder");
        }
    }
}
