using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class AddAppUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CzasowoBd_KierowcyBd_KierowcaID",
                table: "CzasowoBd");

            migrationBuilder.AlterColumn<int>(
                name: "KierowcaID",
                table: "CzasowoBd",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AppUsersBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nr_tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data_ur = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Nr_telHelp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsersBd", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CzasowoBd_KierowcyBd_KierowcaID",
                table: "CzasowoBd",
                column: "KierowcaID",
                principalTable: "KierowcyBd",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CzasowoBd_KierowcyBd_KierowcaID",
                table: "CzasowoBd");

            migrationBuilder.DropTable(
                name: "AppUsersBd");

            migrationBuilder.AlterColumn<int>(
                name: "KierowcaID",
                table: "CzasowoBd",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CzasowoBd_KierowcyBd_KierowcaID",
                table: "CzasowoBd",
                column: "KierowcaID",
                principalTable: "KierowcyBd",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
