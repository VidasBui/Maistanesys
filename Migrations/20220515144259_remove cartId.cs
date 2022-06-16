using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maistanesys.Migrations
{
    public partial class removecartId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Order_OrderId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Order_OrderId",
                table: "Items",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Order_OrderId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Order_OrderId",
                table: "Items",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
