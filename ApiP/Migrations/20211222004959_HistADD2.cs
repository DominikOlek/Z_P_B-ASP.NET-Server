using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class HistADD2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HistBd",
                table: "HistBd");

            migrationBuilder.DropColumn(
                name: "PESEL",
                table: "HistBd");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PESEL",
                table: "HistBd",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistBd",
                table: "HistBd",
                column: "PESEL");
        }
    }
}
