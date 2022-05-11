using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.DataLayer.Migrations
{
    public partial class mig_AddFileToProductTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceFile",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceFile",
                table: "Products");
        }
    }
}
