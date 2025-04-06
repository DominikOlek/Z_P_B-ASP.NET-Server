using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class HistADD3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PESEL",
                table: "HistBd",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PESEL",
                table: "HistBd");
        }
    }
}
