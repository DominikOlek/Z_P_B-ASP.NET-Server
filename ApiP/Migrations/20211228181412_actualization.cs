using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class actualization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrzezID",
                table: "MandatyBd",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MandatyBd_PrzezID",
                table: "MandatyBd",
                column: "PrzezID");

            migrationBuilder.AddForeignKey(
                name: "FK_MandatyBd_UsersBd_PrzezID",
                table: "MandatyBd",
                column: "PrzezID",
                principalTable: "UsersBd",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MandatyBd_UsersBd_PrzezID",
                table: "MandatyBd");

            migrationBuilder.DropIndex(
                name: "IX_MandatyBd_PrzezID",
                table: "MandatyBd");

            migrationBuilder.DropColumn(
                name: "PrzezID",
                table: "MandatyBd");
        }
    }
}
