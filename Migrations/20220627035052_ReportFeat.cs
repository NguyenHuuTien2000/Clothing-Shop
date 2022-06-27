using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Computer_Store.Migrations
{
    public partial class ReportFeat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalRevenue = table.Column<double>(type: "float", nullable: true),
                    TotalUnit = table.Column<double>(type: "float", nullable: true),
                    MostBoughtCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondBoughtCategory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyReports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyReports");
        }
    }
}
