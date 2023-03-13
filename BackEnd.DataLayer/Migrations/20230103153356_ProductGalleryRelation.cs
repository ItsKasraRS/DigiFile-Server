using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.DataLayer.Migrations
{
    public partial class ProductGalleryRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductGalleries",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProductGalleries_ProductId",
                table: "ProductGalleries",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGalleries_Products_ProductId",
                table: "ProductGalleries",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGalleries_Products_ProductId",
                table: "ProductGalleries");

            migrationBuilder.DropIndex(
                name: "IX_ProductGalleries_ProductId",
                table: "ProductGalleries");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductGalleries");
        }
    }
}
