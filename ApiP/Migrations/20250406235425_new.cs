using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiP.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsersBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nr_tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Nr_telHelp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsersBd", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DriversBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nr_tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfPassLicense = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    IsTimelyLost = table.Column<bool>(type: "bit", nullable: false),
                    IsPermanentLost = table.Column<bool>(type: "bit", nullable: false),
                    Pkt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriversBd", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HistBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PESEL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistBd", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RolesBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesBd", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TaryfikatBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PointNumber = table.Column<int>(type: "int", nullable: false),
                    MonthsOfLost = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaryfikatBd", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TimelyBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverID = table.Column<int>(type: "int", nullable: false),
                    DateOfTicket = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfLost = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfGiveBack = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelyBd", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TimelyBd_DriversBd_DriverID",
                        column: x => x.DriverID,
                        principalTable: "DriversBd",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BadgeNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nr_tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersBd", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsersBd_RolesBd_RoleID",
                        column: x => x.RoleID,
                        principalTable: "RolesBd",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketsBd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasonID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfTicket = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfPayment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DriverID = table.Column<int>(type: "int", nullable: false),
                    CopID = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketsBd", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketsBd_DriversBd_DriverID",
                        column: x => x.DriverID,
                        principalTable: "DriversBd",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketsBd_TaryfikatBd_ReasonID",
                        column: x => x.ReasonID,
                        principalTable: "TaryfikatBd",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketsBd_UsersBd_CopID",
                        column: x => x.CopID,
                        principalTable: "UsersBd",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketsBd_CopID",
                table: "TicketsBd",
                column: "CopID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketsBd_DriverID",
                table: "TicketsBd",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketsBd_ReasonID",
                table: "TicketsBd",
                column: "ReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_TimelyBd_DriverID",
                table: "TimelyBd",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_UsersBd_RoleID",
                table: "UsersBd",
                column: "RoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsersBd");

            migrationBuilder.DropTable(
                name: "HistBd");

            migrationBuilder.DropTable(
                name: "TicketsBd");

            migrationBuilder.DropTable(
                name: "TimelyBd");

            migrationBuilder.DropTable(
                name: "TaryfikatBd");

            migrationBuilder.DropTable(
                name: "UsersBd");

            migrationBuilder.DropTable(
                name: "DriversBd");

            migrationBuilder.DropTable(
                name: "RolesBd");
        }
    }
}
