using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class AddRole_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RolesBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa_Roli = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesBd", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UsersBd",
                columns: table => new
                {
                    Nr_Sluzbowy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nr_tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data_ur = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Wiek = table.Column<int>(type: "int", nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    IdRoli = table.Column<int>(type: "int", nullable: false),
                    RolaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersBd", x => x.Nr_Sluzbowy);
                    table.ForeignKey(
                        name: "FK_UsersBd_RolesBd_RolaID",
                        column: x => x.RolaID,
                        principalTable: "RolesBd",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersBd_RolaID",
                table: "UsersBd",
                column: "RolaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersBd");

            migrationBuilder.DropTable(
                name: "RolesBd");
        }
    }
}
