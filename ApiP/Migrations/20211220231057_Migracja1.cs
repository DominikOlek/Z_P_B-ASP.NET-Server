using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class Migracja1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdKierowcy",
                table: "MandatyBd",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Data_orzymania",
                table: "KierowcyBd",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Pkt",
                table: "KierowcyBd",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdKierowcy",
                table: "MandatyBd");

            migrationBuilder.DropColumn(
                name: "Data_orzymania",
                table: "KierowcyBd");

            migrationBuilder.DropColumn(
                name: "Pkt",
                table: "KierowcyBd");
        }
    }
}
