using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class changeProductOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrder_Sizes_SizeId1",
                table: "ProductOrder");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrder_SizeId1",
                table: "ProductOrder");

            migrationBuilder.DropColumn(
                name: "SizeId1",
                table: "ProductOrder");

            migrationBuilder.AlterColumn<int>(
                name: "SizeId",
                table: "ProductOrder",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SizeId",
                table: "ProductOrder",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
