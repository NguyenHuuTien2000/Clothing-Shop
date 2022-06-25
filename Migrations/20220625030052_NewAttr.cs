using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Computer_Store.Migrations
{
    public partial class NewAttr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "History");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "HistoryItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Expense",
                table: "AspNetUsers",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "HistoryItems");

            migrationBuilder.DropColumn(
                name: "Expense",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "History",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
