using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class DodanieTabe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoriaBd",
                columns: table => new
                {
                    PESEL = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriaBd", x => x.PESEL);
                });

            migrationBuilder.CreateTable(
                name: "KierowcyBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nr_tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data_ur = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    CzyOdebrano = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KierowcyBd", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TaryfikatorBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Liczba_PKT = table.Column<int>(type: "int", nullable: false),
                    MiesiąceWstrzymania = table.Column<int>(type: "int", nullable: false),
                    Tytul = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaryfikatorBd", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CzasowoBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdKierowcy = table.Column<int>(type: "int", nullable: false),
                    KierowcaID = table.Column<int>(type: "int", nullable: true),
                    DataWystawienia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataOdebrania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataOddania = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CzasowoBd", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CzasowoBd_KierowcyBd_KierowcaID",
                        column: x => x.KierowcaID,
                        principalTable: "KierowcyBd",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MandatyBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPowodu = table.Column<int>(type: "int", nullable: false),
                    PowodID = table.Column<int>(type: "int", nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataWydania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataOplacenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KierowcyID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MandatyBd", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MandatyBd_KierowcyBd_KierowcyID",
                        column: x => x.KierowcyID,
                        principalTable: "KierowcyBd",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MandatyBd_TaryfikatorBd_PowodID",
                        column: x => x.PowodID,
                        principalTable: "TaryfikatorBd",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CzasowoBd_KierowcaID",
                table: "CzasowoBd",
                column: "KierowcaID");

            migrationBuilder.CreateIndex(
                name: "IX_MandatyBd_KierowcyID",
                table: "MandatyBd",
                column: "KierowcyID");

            migrationBuilder.CreateIndex(
                name: "IX_MandatyBd_PowodID",
                table: "MandatyBd",
                column: "PowodID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CzasowoBd");

            migrationBuilder.DropTable(
                name: "HistoriaBd");

            migrationBuilder.DropTable(
                name: "MandatyBd");

            migrationBuilder.DropTable(
                name: "KierowcyBd");

            migrationBuilder.DropTable(
                name: "TaryfikatorBd");
        }
    }
}
