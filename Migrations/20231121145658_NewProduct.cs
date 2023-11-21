using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Computer_Store.Migrations
{
    public partial class NewProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClothesBrand",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClothesSize",
                table: "Products",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClothesBrand",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ClothesSize",
                table: "Products");
        }
    }
}
