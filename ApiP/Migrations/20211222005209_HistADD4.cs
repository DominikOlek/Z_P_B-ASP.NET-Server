using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class HistADD4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "HistBd",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistBd",
                table: "HistBd",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HistBd",
                table: "HistBd");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "HistBd");
        }
    }
}
