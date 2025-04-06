using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class Mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MandatyBd_KierowcyBd_KierowcyID",
                table: "MandatyBd");

            migrationBuilder.DropForeignKey(
                name: "FK_MandatyBd_TaryfikatorBd_PowodID",
                table: "MandatyBd");

            migrationBuilder.DropColumn(
                name: "IdKierowcy",
                table: "MandatyBd");

            migrationBuilder.DropColumn(
                name: "IdPowodu",
                table: "MandatyBd");

            migrationBuilder.AlterColumn<int>(
                name: "PowodID",
                table: "MandatyBd",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KierowcyID",
                table: "MandatyBd",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MandatyBd_KierowcyBd_KierowcyID",
                table: "MandatyBd",
                column: "KierowcyID",
                principalTable: "KierowcyBd",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MandatyBd_TaryfikatorBd_PowodID",
                table: "MandatyBd",
                column: "PowodID",
                principalTable: "TaryfikatorBd",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MandatyBd_KierowcyBd_KierowcyID",
                table: "MandatyBd");

            migrationBuilder.DropForeignKey(
                name: "FK_MandatyBd_TaryfikatorBd_PowodID",
                table: "MandatyBd");

            migrationBuilder.AlterColumn<int>(
                name: "PowodID",
                table: "MandatyBd",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "KierowcyID",
                table: "MandatyBd",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdKierowcy",
                table: "MandatyBd",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdPowodu",
                table: "MandatyBd",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MandatyBd_KierowcyBd_KierowcyID",
                table: "MandatyBd",
                column: "KierowcyID",
                principalTable: "KierowcyBd",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MandatyBd_TaryfikatorBd_PowodID",
                table: "MandatyBd",
                column: "PowodID",
                principalTable: "TaryfikatorBd",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
