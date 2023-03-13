using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.DataLayer.Migrations
{
    public partial class ChangeOrderSumType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "OrderSum",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderSum",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
