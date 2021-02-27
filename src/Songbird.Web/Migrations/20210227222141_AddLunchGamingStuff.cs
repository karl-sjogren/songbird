using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Songbird.Web.Migrations
{
    public partial class AddLunchGamingStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LunchGamingDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LunchGamingDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LunchGamingAttendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LunchGamingAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LunchGamingAttendance_LunchGamingDates_DateId",
                        column: x => x.DateId,
                        principalTable: "LunchGamingDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LunchGamingAttendance_LunchGamingDates_DateId1",
                        column: x => x.DateId1,
                        principalTable: "LunchGamingDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LunchGamingAttendance_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LunchGamingAttendance_DateId",
                table: "LunchGamingAttendance",
                column: "DateId");

            migrationBuilder.CreateIndex(
                name: "IX_LunchGamingAttendance_DateId1",
                table: "LunchGamingAttendance",
                column: "DateId1");

            migrationBuilder.CreateIndex(
                name: "IX_LunchGamingAttendance_UserId",
                table: "LunchGamingAttendance",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LunchGamingAttendance");

            migrationBuilder.DropTable(
                name: "LunchGamingDates");
        }
    }
}
