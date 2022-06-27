using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Computer_Store.Migrations
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyReports_saleReports_SaleReportSaleID",
                table: "DailyReports");

            migrationBuilder.DropTable(
                name: "saleReports");

            migrationBuilder.DropIndex(
                name: "IX_DailyReports_SaleReportSaleID",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "SaleReportSaleID",
                table: "DailyReports");

            migrationBuilder.AlterColumn<int>(
                name: "Sell",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sell",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SaleReportSaleID",
                table: "DailyReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "saleReports",
                columns: table => new
                {
                    SaleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saleReports", x => x.SaleID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyReports_SaleReportSaleID",
                table: "DailyReports",
                column: "SaleReportSaleID");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyReports_saleReports_SaleReportSaleID",
                table: "DailyReports",
                column: "SaleReportSaleID",
                principalTable: "saleReports",
                principalColumn: "SaleID");
        }
    }
}
