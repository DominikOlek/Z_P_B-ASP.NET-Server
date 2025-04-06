using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class user4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nr_Sluzbowy",
                table: "UsersBd",
                newName: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "UsersBd",
                newName: "Nr_Sluzbowy");
        }
    }
}
