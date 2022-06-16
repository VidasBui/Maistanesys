using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maistanesys.Migrations
{
    public partial class removedamountandorderid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Items");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
