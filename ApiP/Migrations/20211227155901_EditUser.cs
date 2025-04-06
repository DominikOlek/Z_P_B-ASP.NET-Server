using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class EditUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersBd_RolesBd_RolaID",
                table: "UsersBd");

            migrationBuilder.DropColumn(
                name: "IdRoli",
                table: "UsersBd");

            migrationBuilder.DropColumn(
                name: "Wiek",
                table: "UsersBd");

            migrationBuilder.AlterColumn<int>(
                name: "RolaID",
                table: "UsersBd",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Aktywny",
                table: "UsersBd",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersBd_RolesBd_RolaID",
                table: "UsersBd",
                column: "RolaID",
                principalTable: "RolesBd",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersBd_RolesBd_RolaID",
                table: "UsersBd");

            migrationBuilder.DropColumn(
                name: "Aktywny",
                table: "UsersBd");

            migrationBuilder.AlterColumn<int>(
                name: "RolaID",
                table: "UsersBd",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdRoli",
                table: "UsersBd",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wiek",
                table: "UsersBd",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersBd_RolesBd_RolaID",
                table: "UsersBd",
                column: "RolaID",
                principalTable: "RolesBd",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
